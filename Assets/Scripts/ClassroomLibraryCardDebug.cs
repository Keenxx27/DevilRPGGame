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
    /// Read-only runtime diagnostics for the first-classroom library-card pickup.
    /// </summary>
    public sealed class ClassroomLibraryCardDebug : MonoBehaviour
    {
        private const string SceneName = "1_SchoolClassroom";
        private const string Prefix = "[ClassroomLibraryCardDebug]";

        private static readonly FieldInfo NearbyItemsField =
            typeof(PlayerInventory).GetField("nearbyItems",
                BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo NearbyDoorsField =
            typeof(PlayerInteraction).GetField("nearbyDoors",
                BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo NearbyInvestigationsField =
            typeof(PlayerInteraction).GetField("nearbyInvestigations",
                BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo LibraryCardField =
            typeof(ClassroomLibraryCardPuzzle).GetField("libraryCard",
                BindingFlags.Instance | BindingFlags.NonPublic);

        private string previousState;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (FindObjectOfType<ClassroomLibraryCardDebug>() != null) return;
            GameObject host = new GameObject("Classroom Library Card Debug");
            DontDestroyOnLoad(host);
            host.AddComponent<ClassroomLibraryCardDebug>();
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

            string state = BuildCompactState();
            if (state != previousState)
            {
                previousState = state;
                Debug.Log($"{Prefix} Runtime state changed: {state}", this);
            }

            if (!Input.GetKeyDown(KeyCode.E)) return;
            LogDetailedState("E pressed (before interaction)");
            StartCoroutine(LogAfterInteraction());
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            previousState = null;
            if (scene.name == SceneName) StartCoroutine(AuditAfterFrame());
        }

        private IEnumerator AuditAfterFrame()
        {
            yield return null;
            LogDetailedState("scene audit");
        }

        private IEnumerator LogAfterInteraction()
        {
            yield return null;
            LogDetailedState("one frame after E");
        }

        private void LogDetailedState(string phase)
        {
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>(true);
            PlayerInteraction interaction = inventory != null
                ? inventory.GetComponent<PlayerInteraction>() : null;
            ClassroomLibraryCardPuzzle puzzle =
                FindObjectOfType<ClassroomLibraryCardPuzzle>(true);
            WorldItem card = GetLibraryCard(puzzle);
            FlowerpotBreakable flowerpot = FindObjectOfType<FlowerpotBreakable>(true);

            List<WorldItem> nearbyItems = inventory == null ? null
                : NearbyItemsField?.GetValue(inventory) as List<WorldItem>;
            List<DoorController> nearbyDoors = interaction == null ? null
                : NearbyDoorsField?.GetValue(interaction) as List<DoorController>;
            List<InvestigationPoint> nearbyInvestigations = interaction == null ? null
                : NearbyInvestigationsField?.GetValue(interaction)
                    as List<InvestigationPoint>;

            Collider2D playerCollider = inventory != null
                ? inventory.GetComponent<Collider2D>() : null;
            Rigidbody2D playerBody = inventory != null
                ? inventory.GetComponent<Rigidbody2D>() : null;
            Collider2D cardCollider = card != null
                ? card.GetComponent<Collider2D>() : null;

            bool cardRegistered = card != null && nearbyItems != null
                && nearbyItems.Contains(card);
            bool touching = playerCollider != null && cardCollider != null
                && playerCollider.IsTouching(cardCollider);
            int usedSlots = CountUsedSlots(inventory);

            var message = new StringBuilder();
            message.Append(Prefix).Append(' ').Append(phase)
                .Append(" | scene=").Append(SceneManager.GetActiveScene().name)
                .Append(" playerInventory=").Append(inventory != null)
                .Append(" playerInteraction=").Append(interaction != null)
                .Append(" playerPosition=").Append(inventory != null
                    ? inventory.transform.position.ToString() : "missing")
                .Append(" playerCollider=").Append(Describe(playerCollider))
                .Append(" playerRigidbody=").Append(Describe(playerBody))
                .Append(" inventorySlots=").Append(usedSlots).Append("/5")
                .Append(" nearbyItems=").Append(nearbyItems?.Count ?? -1)
                .Append(" nearbyDoors=").Append(nearbyDoors?.Count ?? -1)
                .Append(" nearbyInvestigations=")
                    .Append(nearbyInvestigations?.Count ?? -1)
                .Append(" | puzzle=").Append(puzzle != null)
                .Append(" puzzleState=").Append(puzzle != null
                    ? puzzle.State.ToString() : "missing")
                .Append(" flowerpot=").Append(flowerpot != null)
                .Append(" flowerpotBroken=").Append(flowerpot != null && flowerpot.IsBroken)
                .Append(" | card=").Append(card != null)
                .Append(" cardObject=").Append(card != null ? card.name : "missing")
                .Append(" cardActiveSelf=").Append(card != null && card.gameObject.activeSelf)
                .Append(" cardActiveInHierarchy=")
                    .Append(card != null && card.gameObject.activeInHierarchy)
                .Append(" cardPosition=").Append(card != null
                    ? card.transform.position.ToString() : "missing")
                .Append(" cardCanPickUp=").Append(card != null && card.CanPickUp)
                .Append(" cardItemId=").Append(card != null ? card.ItemId : "missing")
                .Append(" cardCollider=").Append(Describe(cardCollider))
                .Append(" touchingPlayer=").Append(touching)
                .Append(" registeredNearby=").Append(cardRegistered)
                .Append(" registeredItems=[").Append(DescribeItems(nearbyItems)).Append(']');

            if (inventory == null || card == null || cardCollider == null
                || (card.gameObject.activeInHierarchy && !cardCollider.isTrigger)
                || (touching && !cardRegistered))
            {
                Debug.LogWarning(message.ToString(), card != null ? card : this);
            }
            else
            {
                Debug.Log(message.ToString(), card);
            }
        }

        private string BuildCompactState()
        {
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>(true);
            ClassroomLibraryCardPuzzle puzzle =
                FindObjectOfType<ClassroomLibraryCardPuzzle>(true);
            WorldItem card = GetLibraryCard(puzzle);
            List<WorldItem> nearby = inventory == null ? null
                : NearbyItemsField?.GetValue(inventory) as List<WorldItem>;
            bool registered = card != null && nearby != null && nearby.Contains(card);
            Collider2D collider = card != null ? card.GetComponent<Collider2D>() : null;
            return $"puzzle={(puzzle != null ? puzzle.State.ToString() : "missing")}, "
                + $"card={(card != null)}, active={(card != null && card.gameObject.activeInHierarchy)}, "
                + $"collider={Describe(collider)}, registered={registered}, "
                + $"slots={CountUsedSlots(inventory)}/5";
        }

        private static WorldItem GetLibraryCard(ClassroomLibraryCardPuzzle puzzle)
        {
            return puzzle == null ? null : LibraryCardField?.GetValue(puzzle) as WorldItem;
        }

        private static int CountUsedSlots(PlayerInventory inventory)
        {
            if (inventory == null) return -1;
            int used = 0;
            for (int index = 0; index < 5; index++)
            {
                if (inventory.GetSlotItem(index) != null) used++;
            }
            return used;
        }

        private static string DescribeItems(List<WorldItem> items)
        {
            if (items == null) return "unavailable";
            var descriptions = new List<string>();
            foreach (WorldItem item in items)
            {
                if (item == null) descriptions.Add("null");
                else descriptions.Add($"{item.name}(active="
                    + $"{item.gameObject.activeInHierarchy},pickup={item.CanPickUp})");
            }
            return descriptions.Count == 0 ? "none" : string.Join(", ", descriptions);
        }

        private static string Describe(Collider2D collider)
        {
            if (collider == null) return "missing";
            return $"{collider.GetType().Name}(enabled={collider.enabled},"
                + $"trigger={collider.isTrigger},bounds={collider.bounds})";
        }

        private static string Describe(Rigidbody2D body)
        {
            if (body == null) return "missing";
            return $"Rigidbody2D(type={body.bodyType},simulated={body.simulated})";
        }

        private static bool IsTargetScene()
        {
            return SceneManager.GetActiveScene().name == SceneName;
        }
    }
#endif
}
