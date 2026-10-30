using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    /// <summary>
    /// Read-only diagnostics for library paper pickup and unexplained colliders.
    /// </summary>
    public sealed class LibraryPhysicsPickupDebug : MonoBehaviour
    {
        private const string SceneName = "3_SchoolLibrary";
        private const string Prefix = "[LibraryPhysicsPickupDebug]";
        private const float ScanRadius = 3f;

        private static readonly FieldInfo NearbyItemsField =
            typeof(PlayerInventory).GetField("nearbyItems",
                BindingFlags.Instance | BindingFlags.NonPublic);
        private Vector3 lastAutomaticScanPosition;
        private bool hasAutomaticScanPosition;
        private string lastSolidContactSignature = string.Empty;
        private readonly ContactPoint2D[] contactBuffer = new ContactPoint2D[32];

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (FindObjectOfType<LibraryPhysicsPickupDebug>() != null) return;
            GameObject host = new GameObject("Library Physics Pickup Debug");
            DontDestroyOnLoad(host);
            host.AddComponent<LibraryPhysicsPickupDebug>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
            if (IsTargetScene()) StartCoroutine(AuditAfterFrame());
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void Update()
        {
            if (!IsTargetScene()) return;
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory == null) return;

            if (!hasAutomaticScanPosition
                || Vector2.Distance(lastAutomaticScanPosition,
                    inventory.transform.position) >= 2f)
            {
                hasAutomaticScanPosition = true;
                lastAutomaticScanPosition = inventory.transform.position;
                LogNearbyColliders("automatic position scan", inventory);
            }

            if (!Input.GetKeyDown(KeyCode.E)) return;
            LogPickupState("E pressed (before interaction)", inventory);
            LogNearbyColliders("E pressed collider scan", inventory);
            StartCoroutine(LogAfterE());
        }

        private void FixedUpdate()
        {
            if (!IsTargetScene()) return;
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory == null) return;

            LogSolidContactsWhenChanged(inventory);
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            hasAutomaticScanPosition = false;
            if (scene.name == SceneName) StartCoroutine(AuditAfterFrame());
        }

        private IEnumerator AuditAfterFrame()
        {
            yield return null;
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            LogPickupState("scene audit", inventory);
            if (inventory != null) LogNearbyColliders("scene collider audit", inventory);
        }

        private IEnumerator LogAfterE()
        {
            yield return null;
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            LogPickupState("one frame after E", inventory);
        }

        private void LogPickupState(string phase, PlayerInventory inventory)
        {
            List<WorldItem> registered = inventory == null ? null
                : NearbyItemsField?.GetValue(inventory) as List<WorldItem>;
            Collider2D[] playerColliders = inventory == null
                ? new Collider2D[0]
                : inventory.GetComponentsInChildren<Collider2D>(true);
            int usedSlots = CountUsedSlots(inventory);
            var report = new StringBuilder();
            report.Append(Prefix).Append(' ').Append(phase)
                .Append(" | player=").Append(inventory != null
                    ? inventory.transform.position.ToString() : "missing")
                .Append(" playerColliders=").Append(playerColliders.Length)
                .Append(" slots=").Append(usedSlots).Append("/5")
                .Append(" registeredItems=[").Append(DescribeItems(registered)).Append(']');

            int paperCount = 0;
            foreach (WorldItem item in FindObjectsOfType<WorldItem>(true))
            {
                if (!IsPaper(item)) continue;
                paperCount++;
                Collider2D collider = item.GetComponent<Collider2D>();
                bool isRegistered = registered != null && registered.Contains(item);
                float distance = inventory == null ? -1f
                    : Vector2.Distance(inventory.transform.position, item.transform.position);
                bool touchingAny = IsTouchingAny(playerColliders, collider);
                report.Append("\n - paper '").Append(item.name).Append("' path=")
                    .Append(GetPath(item.transform))
                    .Append(" activeSelf=").Append(item.gameObject.activeSelf)
                    .Append(" activeHierarchy=").Append(item.gameObject.activeInHierarchy)
                    .Append(" pickup=").Append(item.CanPickUp)
                    .Append(" itemId=").Append(item.ItemId)
                    .Append(" position=").Append(item.transform.position)
                    .Append(" distance=").Append(distance.ToString("F2"))
                    .Append(" collider=").Append(Describe(collider))
                    .Append(" touchingAnyPlayerCollider=").Append(touchingAny)
                    .Append(" registered=").Append(isRegistered);
            }

            report.Append(" | paperCount=").Append(paperCount);
            if (inventory == null || paperCount == 0)
                Debug.LogWarning(report.ToString(), this);
            else
                Debug.Log(report.ToString(), this);
        }

        private void LogNearbyColliders(string phase, PlayerInventory inventory)
        {
            Collider2D[] playerColliders = inventory.GetComponentsInChildren<Collider2D>(true);
            Collider2D[] nearby = Physics2D.OverlapCircleAll(inventory.transform.position,
                ScanRadius, Physics2D.AllLayers);
            var report = new StringBuilder();
            report.Append(Prefix).Append(' ').Append(phase)
                .Append(" | player=").Append(inventory.transform.position)
                .Append(" scanRadius=").Append(ScanRadius.ToString("F1"))
                .Append(" colliders=").Append(nearby.Length);

            foreach (Collider2D collider in nearby)
            {
                if (collider == null || IsPlayerCollider(collider, playerColliders)) continue;
                ColliderDistance2D closest = ClosestPlayerDistance(playerColliders, collider);
                SpriteRenderer renderer = collider.GetComponent<SpriteRenderer>();
                if (renderer == null) renderer = collider.GetComponentInChildren<SpriteRenderer>(true);
                report.Append("\n - '").Append(collider.name).Append("' path=")
                    .Append(GetPath(collider.transform))
                    .Append(" layer=").Append(LayerMask.LayerToName(collider.gameObject.layer))
                    .Append(" collider=").Append(Describe(collider))
                    .Append(" playerDistance=").Append(closest.distance.ToString("F2"))
                    .Append(" overlapped=").Append(closest.isOverlapped)
                    .Append(" hasRenderer=").Append(renderer != null)
                    .Append(" rendererEnabled=").Append(renderer != null && renderer.enabled)
                    .Append(" sprite=").Append(renderer != null && renderer.sprite != null
                        ? renderer.sprite.name : "none");
            }

            Debug.Log(report.ToString(), this);
        }

        private void LogSolidContactsWhenChanged(PlayerInventory inventory)
        {
            Collider2D[] playerColliders = inventory.GetComponentsInChildren<Collider2D>(true);
            var contactsByPath = new SortedDictionary<string, string>();

            foreach (Collider2D playerCollider in playerColliders)
            {
                if (playerCollider == null || !playerCollider.enabled || playerCollider.isTrigger)
                    continue;

                int contactCount = playerCollider.GetContacts(contactBuffer);
                for (int index = 0; index < contactCount; index++)
                {
                    ContactPoint2D contact = contactBuffer[index];
                    Collider2D obstacle = contact.otherCollider;
                    if (obstacle == null || !obstacle.enabled || obstacle.isTrigger
                        || IsPlayerCollider(obstacle, playerColliders))
                    {
                        continue;
                    }

                    string path = GetPath(obstacle.transform);
                    contactsByPath[path] = "playerCollider=" + Describe(playerCollider)
                        + " obstacleCollider=" + Describe(obstacle)
                        + " contactPoint=" + contact.point
                        + " normal=" + contact.normal;
                }
            }

            string signature = string.Join("|", contactsByPath.Keys);
            if (signature == lastSolidContactSignature) return;
            lastSolidContactSignature = signature;

            var report = new StringBuilder();
            report.Append(Prefix).Append(" solid contacts changed | player=")
                .Append(inventory.transform.position)
                .Append(" contacts=").Append(contactsByPath.Count);
            foreach (KeyValuePair<string, string> contact in contactsByPath)
            {
                report.Append("\n - path=").Append(contact.Key)
                    .Append(' ').Append(contact.Value);
            }

            Debug.Log(report.ToString(), this);
        }

        private static bool IsPaper(WorldItem item)
        {
            string name = item.name.ToLowerInvariant();
            string id = item.ItemId == null ? string.Empty : item.ItemId.ToLowerInvariant();
            return name.Contains("fragment") || name.Contains("page")
                || id.Contains("fragment") || id.Contains("page");
        }

        private static bool IsTouchingAny(Collider2D[] players, Collider2D target)
        {
            if (target == null) return false;
            foreach (Collider2D player in players)
            {
                if (player != null && player.IsTouching(target)) return true;
            }
            return false;
        }

        private static ColliderDistance2D ClosestPlayerDistance(
            Collider2D[] players, Collider2D target)
        {
            ColliderDistance2D best = default(ColliderDistance2D);
            best.distance = float.PositiveInfinity;
            foreach (Collider2D player in players)
            {
                if (player == null || !player.enabled) continue;
                ColliderDistance2D current = player.Distance(target);
                if (current.distance < best.distance) best = current;
            }
            return best;
        }

        private static bool IsPlayerCollider(Collider2D candidate, Collider2D[] players)
        {
            foreach (Collider2D player in players)
                if (candidate == player) return true;
            return false;
        }

        private static int CountUsedSlots(PlayerInventory inventory)
        {
            if (inventory == null) return -1;
            int count = 0;
            for (int index = 0; index < 5; index++)
                if (inventory.GetSlotItem(index) != null) count++;
            return count;
        }

        private static string DescribeItems(List<WorldItem> items)
        {
            if (items == null) return "unavailable";
            var result = new List<string>();
            foreach (WorldItem item in items)
                result.Add(item == null ? "null" : item.name);
            return result.Count == 0 ? "none" : string.Join(",", result);
        }

        private static string Describe(Collider2D collider)
        {
            if (collider == null) return "missing";
            return $"{collider.GetType().Name}(enabled={collider.enabled},"
                + $"trigger={collider.isTrigger},bounds={collider.bounds})";
        }

        private static string GetPath(Transform transform)
        {
            string path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }
            return path;
        }

        private static bool IsTargetScene()
        {
            return SceneManager.GetActiveScene().name == SceneName;
        }
    }
#endif
}
