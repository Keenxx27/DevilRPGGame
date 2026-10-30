using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    /// <summary>Read-only diagnostics for placing library fragments on a table.</summary>
    public sealed class LibraryFragmentPlacementDebug : MonoBehaviour
    {
        private const string SceneName = "3_SchoolLibrary";
        private const string Prefix = "[LibraryFragmentPlacementDebug]";
        private const string FragmentItemId = "library.fragment";

        private static readonly FieldInfo NearbyInvestigationsField =
            typeof(PlayerInteraction).GetField("nearbyInvestigations",
                BindingFlags.Instance | BindingFlags.NonPublic);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (FindObjectOfType<LibraryFragmentPlacementDebug>() != null) return;
            GameObject host = new GameObject("Library Fragment Placement Debug");
            DontDestroyOnLoad(host);
            host.AddComponent<LibraryFragmentPlacementDebug>();
        }

        private void Update()
        {
            if (!IsTargetScene() || !Input.GetKeyDown(KeyCode.E)) return;
            LogState("E pressed (before interaction)");
            StartCoroutine(LogAfterE());
        }

        private IEnumerator LogAfterE()
        {
            yield return null;
            LogState("one frame after E");
        }

        private void LogState(string phase)
        {
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            PlayerInteraction interaction = inventory != null
                ? inventory.GetComponent<PlayerInteraction>() : null;
            LibraryBookPuzzle puzzle = FindObjectOfType<LibraryBookPuzzle>(true);
            DialogueController dialogue = inventory != null
                ? inventory.GetComponent<DialogueController>() : null;
            List<InvestigationPoint> nearby = interaction == null ? null
                : NearbyInvestigationsField?.GetValue(interaction)
                    as List<InvestigationPoint>;

            WorldItem selected = inventory != null ? inventory.SelectedItem : null;
            int fragmentCount = CountFragments(inventory);
            var message = new StringBuilder();
            message.Append(Prefix).Append(' ').Append(phase)
                .Append(" | player=").Append(inventory != null
                    ? inventory.transform.position.ToString() : "missing")
                .Append(" selectedSlot=").Append(inventory != null
                    ? (inventory.SelectedSlotIndex + 1).ToString() : "missing")
                .Append(" selectedItem=").Append(selected != null ? selected.name : "none")
                .Append(" selectedItemId=").Append(selected != null ? selected.ItemId : "none")
                .Append(" selectedIsFragment=")
                    .Append(selected != null && selected.ItemId == FragmentItemId)
                .Append(" fragmentsInInventory=").Append(fragmentCount)
                .Append(" dialoguePlaying=").Append(dialogue != null && dialogue.IsPlaying)
                .Append(" nearbyInvestigations=").Append(nearby?.Count ?? -1)
                .Append(" | puzzle=").Append(puzzle != null)
                .Append(" openingFinished=").Append(ReadField(puzzle, "openingFinished"))
                .Append(" placedFragments=").Append(ReadField(puzzle, "placedFragments"))
                .Append(" placementFinished=").Append(ReadField(puzzle, "placementFinished"))
                .Append(" fragmentTable=").Append(DescribeTransform(
                    ReadField(puzzle, "fragmentTable") as Transform));

            if (nearby != null)
            {
                foreach (InvestigationPoint point in nearby)
                {
                    if (point == null)
                    {
                        message.Append("\n - nearby=null");
                        continue;
                    }

                    Collider2D collider = point.GetComponent<Collider2D>();
                    float distance = inventory == null ? -1f
                        : Vector2.Distance(inventory.transform.position,
                            point.transform.position);
                    message.Append("\n - nearby '").Append(point.name)
                        .Append("' kind=").Append(point.Kind)
                        .Append(" active=").Append(point.gameObject.activeInHierarchy)
                        .Append(" available=").Append(point.IsAvailable)
                        .Append(" distance=").Append(distance.ToString("F2"))
                        .Append(" collider=").Append(Describe(collider));
                }
            }

            foreach (InvestigationPoint point in
                FindObjectsOfType<InvestigationPoint>(true))
            {
                if (point.gameObject.scene.name != SceneName
                    || point.Kind != InvestigationKind.LibraryReadingTable) continue;
                bool registered = nearby != null && nearby.Contains(point);
                float distance = inventory == null ? -1f
                    : Vector2.Distance(inventory.transform.position,
                        point.transform.position);
                message.Append("\n - table '").Append(point.name)
                    .Append("' registered=").Append(registered)
                    .Append(" available=").Append(point.IsAvailable)
                    .Append(" distance=").Append(distance.ToString("F2"))
                    .Append(" collider=").Append(Describe(point.GetComponent<Collider2D>()));
            }

            if (inventory == null || selected == null
                || selected.ItemId != FragmentItemId
                || nearby == null || !HasRegisteredTable(nearby))
                Debug.LogWarning(message.ToString(), this);
            else
                Debug.Log(message.ToString(), this);
        }

        private static int CountFragments(PlayerInventory inventory)
        {
            if (inventory == null) return -1;
            int count = 0;
            for (int index = 0; index < 5; index++)
            {
                WorldItem item = inventory.GetSlotItem(index);
                if (item != null && item.ItemId == FragmentItemId) count++;
            }
            return count;
        }

        private static bool HasRegisteredTable(List<InvestigationPoint> nearby)
        {
            foreach (InvestigationPoint point in nearby)
                if (point != null && point.Kind == InvestigationKind.LibraryReadingTable)
                    return true;
            return false;
        }

        private static object ReadField(object target, string name)
        {
            if (target == null) return "missing";
            FieldInfo field = target.GetType().GetField(name,
                BindingFlags.Instance | BindingFlags.NonPublic);
            return field == null ? "field-missing" : field.GetValue(target);
        }

        private static string DescribeTransform(Transform value)
        {
            return value == null ? "none" : $"{value.name}@{value.position}";
        }

        private static string Describe(Collider2D collider)
        {
            if (collider == null) return "missing";
            return $"{collider.GetType().Name}(enabled={collider.enabled},"
                + $"trigger={collider.isTrigger},bounds={collider.bounds})";
        }

        private static bool IsTargetScene()
        {
            return SceneManager.GetActiveScene().name == SceneName;
        }
    }
#endif
}
