using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    /// <summary>
    /// Runtime diagnostics for chair pickup. This component creates itself in
    /// development builds and only writes logs when the nearby-item state changes
    /// or the player presses E/Q.
    /// </summary>
    public sealed class ChairPickupDebug : MonoBehaviour
    {
        private const string Prefix = "[ChairPickupDebug]";
        private const float NearbyReportDistance = 2.5f;

        private static readonly FieldInfo NearbyItemsField =
            typeof(PlayerInventory).GetField("nearbyItems",
                BindingFlags.Instance | BindingFlags.NonPublic);

        private PlayerInventory inventory;
        private PlayerInteraction interaction;
        private Collider2D playerCollider;
        private MainCharacterMovement movement;
        private string previousNearbyState;
        private bool reportAfterInteraction;
        private bool reportAfterThrow;
        private WorldItem qSelectedItem;
        private ThrowableChair qChair;
        private bool missingPlayerReported;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateDebugger()
        {
            if (FindObjectOfType<ChairPickupDebug>() != null)
            {
                return;
            }

            GameObject debugObject = new GameObject("Chair Pickup Debug");
            DontDestroyOnLoad(debugObject);
            debugObject.AddComponent<ChairPickupDebug>();
        }

        private void Start()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
            FindPlayer();
            ReportStartupState();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            inventory = null;
            interaction = null;
            playerCollider = null;
            movement = null;
            previousNearbyState = null;
            missingPlayerReported = false;
            FindPlayer();
        }

        private void Update()
        {
            if (inventory == null)
            {
                FindPlayer();
            }

            ReportNearbyStateWhenChanged();

            if (Input.GetKeyDown(KeyCode.E))
            {
                Report("E pressed (before interaction)");
                reportAfterInteraction = true;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                qSelectedItem = inventory != null ? inventory.SelectedItem : null;
                qChair = qSelectedItem != null
                    ? qSelectedItem.GetComponent<ThrowableChair>() : null;
                ReportThrow("Q pressed (before inventory use)");
                reportAfterThrow = true;
            }
        }

        private void LateUpdate()
        {
            if (reportAfterInteraction)
            {
                reportAfterInteraction = false;
                Report("E interaction result");
            }

            if (reportAfterThrow)
            {
                reportAfterThrow = false;
                ReportThrow("Q result after Update");
                StartCoroutine(ReportThrowProgress());
            }
        }

        private void FindPlayer()
        {
            inventory = FindObjectOfType<PlayerInventory>(true);
            if (inventory == null)
            {
                interaction = null;
                playerCollider = null;
                movement = null;
                return;
            }

            interaction = inventory.GetComponent<PlayerInteraction>();
            playerCollider = inventory.GetComponent<Collider2D>();
            movement = inventory.GetComponent<MainCharacterMovement>();
            missingPlayerReported = false;
        }

        private void ReportStartupState()
        {
            int chairCount = 0;
            int pickupEnabledCount = 0;
            foreach (ThrowableChair chair in FindObjectsOfType<ThrowableChair>(true))
            {
                chairCount++;
                WorldItem item = chair.GetComponent<WorldItem>();
                if (item != null && item.CanPickUp)
                {
                    pickupEnabledCount++;
                }
            }

            Debug.Log($"{Prefix} Started. PlayerInventory={inventory != null}, "
                + $"PlayerInteraction={interaction != null}, PlayerCollider={Describe(playerCollider)}, "
                + $"chairs={chairCount}, pickupEnabledChairs={pickupEnabledCount}.", this);
        }

        private void ReportNearbyStateWhenChanged()
        {
            string state = BuildNearbyState();
            if (state == previousNearbyState)
            {
                return;
            }

            previousNearbyState = state;
            Debug.Log($"{Prefix} Nearby registration changed: {state}", this);
        }

        private string BuildNearbyState()
        {
            if (inventory == null)
            {
                return "PlayerInventory not found";
            }

            List<WorldItem> nearbyItems = GetRegisteredNearbyItems();
            if (nearbyItems == null)
            {
                return "nearbyItems field unavailable";
            }

            var names = new List<string>();
            foreach (WorldItem item in nearbyItems)
            {
                if (item != null)
                {
                    names.Add($"{item.name}(active={item.gameObject.activeInHierarchy}, "
                        + $"canPickUp={item.CanPickUp})");
                }
            }

            return names.Count == 0 ? "none" : string.Join(", ", names);
        }

        private void Report(string phase)
        {
            if (inventory == null)
            {
                FindPlayer();
                if (inventory == null)
                {
                    if (!missingPlayerReported)
                    {
                        Debug.LogWarning($"{Prefix} {phase}: no PlayerInventory exists in "
                            + $"scene '{SceneManager.GetActiveScene().name}'. Diagnostics will "
                            + "resume automatically when a player is available.", this);
                        missingPlayerReported = true;
                    }
                    return;
                }
            }

            int usedSlots = 0;
            for (int index = 0; index < 3; index++)
            {
                if (inventory.GetSlotItem(index) != null)
                {
                    usedSlots++;
                }
            }

            List<WorldItem> registered = GetRegisteredNearbyItems();
            Debug.Log($"{Prefix} {phase}: player={inventory.transform.position}, "
                + $"interaction={interaction != null}, collider={Describe(playerCollider)}, "
                + $"inventorySlots={usedSlots}/3, registeredNearby={registered?.Count ?? -1}, "
                + $"registeredItems=[{BuildNearbyState()}].", this);

            ThrowableChair[] chairs = FindObjectsOfType<ThrowableChair>(true);
            int reported = 0;
            foreach (ThrowableChair chair in chairs)
            {
                float distance = Vector2.Distance(inventory.transform.position,
                    chair.transform.position);
                if (distance > NearbyReportDistance)
                {
                    continue;
                }

                WorldItem item = chair.GetComponent<WorldItem>();
                Collider2D chairCollider = chair.GetComponent<Collider2D>();
                bool touching = playerCollider != null && chairCollider != null
                    && playerCollider.IsTouching(chairCollider);
                bool registeredWithInventory = registered != null && item != null
                    && registered.Contains(item);

                Debug.Log($"{Prefix} Chair '{chair.name}': distance={distance:F2}, "
                    + $"active={chair.gameObject.activeInHierarchy}, "
                    + $"WorldItem={item != null}, canPickUp={item != null && item.CanPickUp}, "
                    + $"collider={Describe(chairCollider)}, touchingPlayer={touching}, "
                    + $"registeredNearby={registeredWithInventory}, flying={chair.IsFlying}.", chair);
                reported++;
            }

            if (reported == 0)
            {
                Debug.LogWarning($"{Prefix} No chair is within {NearbyReportDistance:F1} units "
                    + "of the player. Move closer and press E again.", this);
            }
        }

        private List<WorldItem> GetRegisteredNearbyItems()
        {
            return NearbyItemsField?.GetValue(inventory) as List<WorldItem>;
        }

        private void ReportThrow(string phase)
        {
            string selected = qSelectedItem == null
                ? "none"
                : $"{qSelectedItem.name}(active={qSelectedItem.gameObject.activeInHierarchy}, "
                    + $"canPickUp={qSelectedItem.CanPickUp})";
            string chair = qChair == null
                ? "none"
                : $"{qChair.name}(active={qChair.gameObject.activeInHierarchy}, "
                    + $"flying={qChair.IsFlying}, position={qChair.transform.position}, "
                    + $"collider={Describe(qChair.GetComponent<Collider2D>())})";
            Vector2 facing = movement != null ? movement.FacingDirection : Vector2.zero;
            ClassroomLibraryCardPuzzle puzzle = FindObjectOfType<ClassroomLibraryCardPuzzle>();

            Debug.Log($"{Prefix} {phase}: selectedItem={selected}, selectedChair={chair}, "
                + $"facing={facing}, puzzleState={(puzzle != null ? puzzle.State.ToString() : "missing")}, "
                + $"currentInventorySelection={(inventory != null && inventory.SelectedItem != null ? inventory.SelectedItem.name : "none")}.",
                qChair != null ? qChair : this);

            foreach (FlowerpotBreakable flowerpot in FindObjectsOfType<FlowerpotBreakable>(true))
            {
                Collider2D potCollider = flowerpot.GetComponent<Collider2D>();
                float distance = qChair != null
                    ? Vector2.Distance(qChair.transform.position, flowerpot.transform.position)
                    : (inventory != null
                        ? Vector2.Distance(inventory.transform.position, flowerpot.transform.position)
                        : -1f);
                Debug.Log($"{Prefix} Flowerpot '{flowerpot.name}': active={flowerpot.gameObject.activeInHierarchy}, "
                    + $"broken={flowerpot.IsBroken}, position={flowerpot.transform.position}, "
                    + $"distanceFromChairOrPlayer={distance:F2}, collider={Describe(potCollider)}.", flowerpot);
            }
        }

        private IEnumerator ReportThrowProgress()
        {
            for (int sample = 1; sample <= 8; sample++)
            {
                yield return new WaitForSecondsRealtime(0.05f);
                ReportThrow($"Q flight sample {sample}/8");
            }
        }

        private static string Describe(Collider2D collider)
        {
            if (collider == null)
            {
                return "missing";
            }

            return $"{collider.GetType().Name}(enabled={collider.enabled}, "
                + $"trigger={collider.isTrigger}, bounds={collider.bounds})";
        }
    }
#endif
}
