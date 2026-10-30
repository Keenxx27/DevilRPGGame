using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public sealed class LibraryInteractionDebug : MonoBehaviour
    {
        private const string LibrarySceneName = "3_SchoolLibrary";
        private const string Prefix = "[LibraryInteractionDebug]";
        private static readonly FieldInfo NearbyInvestigationsField =
            typeof(PlayerInteraction).GetField("nearbyInvestigations",
                BindingFlags.Instance | BindingFlags.NonPublic);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (FindObjectOfType<LibraryInteractionDebug>() != null) return;
            GameObject host = new GameObject("Library Interaction Debug");
            DontDestroyOnLoad(host);
            host.AddComponent<LibraryInteractionDebug>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
            if (IsLibraryActive()) StartCoroutine(LogSceneAuditAfterFrame());
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void Update()
        {
            if (!IsLibraryActive() || !Input.GetKeyDown(KeyCode.E)) return;
            LogInteractionState("E pressed (before interaction)");
            StartCoroutine(LogAfterInteraction());
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == LibrarySceneName) StartCoroutine(LogSceneAuditAfterFrame());
        }

        private IEnumerator LogSceneAuditAfterFrame()
        {
            yield return null;
            InvestigationPoint[] points = FindObjectsOfType<InvestigationPoint>(true);
            int libraryPoints = 0;
            int missingColliders = 0;
            int invalidTriggers = 0;
            var details = new StringBuilder();

            foreach (InvestigationPoint point in points)
            {
                if (point.gameObject.scene.name != LibrarySceneName) continue;
                libraryPoints++;
                Collider2D collider = point.GetComponent<Collider2D>();
                if (collider == null) missingColliders++;
                else if (!collider.enabled || !collider.isTrigger) invalidTriggers++;
                details.Append("\n - ").Append(point.name)
                    .Append(" kind=").Append(point.Kind)
                    .Append(" active=").Append(point.gameObject.activeInHierarchy)
                    .Append(" available=").Append(point.IsAvailable)
                    .Append(" collider=").Append(collider == null ? "MISSING" : collider.GetType().Name)
                    .Append(" enabled=").Append(collider != null && collider.enabled)
                    .Append(" trigger=").Append(collider != null && collider.isTrigger);
            }

            Debug.Log($"{Prefix} Scene audit: points={libraryPoints}, "
                + $"missingColliders={missingColliders}, invalidTriggers={invalidTriggers}."
                + details, this);
        }

        private IEnumerator LogAfterInteraction()
        {
            yield return null;
            LogInteractionState("one frame after E");
        }

        private void LogInteractionState(string phase)
        {
            PlayerInteraction interaction = FindObjectOfType<PlayerInteraction>();
            LibraryBookPuzzle puzzle = FindObjectOfType<LibraryBookPuzzle>(true);
            if (interaction == null)
            {
                Debug.LogError($"{Prefix} {phase}: PlayerInteraction was not found.", this);
                return;
            }

            PlayerInventory inventory = interaction.GetComponent<PlayerInventory>();
            DialogueController dialogue = interaction.GetComponent<DialogueController>();
            List<InvestigationPoint> nearby = NearbyInvestigationsField?.GetValue(interaction)
                as List<InvestigationPoint>;
            InvestigationPoint nearest = FindNearestRegistered(interaction.transform.position, nearby);

            var message = new StringBuilder();
            message.Append(Prefix).Append(' ').Append(phase)
                .Append(" | player=").Append(interaction.transform.position)
                .Append(" interactionEnabled=").Append(interaction.enabled)
                .Append(" dialoguePlaying=").Append(dialogue != null && dialogue.IsPlaying)
                .Append(" selectedItem=").Append(inventory?.SelectedItem == null
                    ? "none" : inventory.SelectedItem.ItemId)
                .Append(" nearbyRegistered=").Append(nearby?.Count ?? -1)
                .Append(" nearest=").Append(nearest == null ? "none" : nearest.name)
                .Append(" nearestKind=").Append(nearest == null ? "none" : nearest.Kind.ToString())
                .Append(" nearestAvailable=").Append(nearest != null && nearest.IsAvailable);

            if (nearest != null)
            {
                Collider2D collider = nearest.GetComponent<Collider2D>();
                message.Append(" nearestDistance=")
                    .Append(Vector2.Distance(interaction.transform.position, nearest.transform.position).ToString("F2"))
                    .Append(" collider=").Append(collider == null ? "MISSING" : collider.GetType().Name)
                    .Append(" colliderEnabled=").Append(collider != null && collider.enabled)
                    .Append(" isTrigger=").Append(collider != null && collider.isTrigger);
            }

            if (puzzle != null)
            {
                message.Append(" | openingFinished=").Append(ReadField(puzzle, "openingFinished"))
                    .Append(" bookcaseInvestigations=").Append(ReadField(puzzle, "bookcaseInvestigations"))
                    .Append(" chairPushes=").Append(ReadField(puzzle, "chairPushes"))
                    .Append(" wallOpened=").Append(ReadField(puzzle, "wallOpened"))
                    .Append(" placedFragments=").Append(ReadField(puzzle, "placedFragments"))
                    .Append(" counterInvestigated=").Append(ReadField(puzzle, "counterInvestigated"))
                    .Append(" gluePulledOut=").Append(ReadField(puzzle, "gluePulledOut"))
                    .Append(" pageCompleted=").Append(ReadField(puzzle, "pageCompleted"));
            }
            else
            {
                message.Append(" | LibraryBookPuzzle=MISSING");
            }

            if (nearest == null)
            {
                Debug.LogWarning(message.ToString(), this);
            }
            else
            {
                Debug.Log(message.ToString(), this);
            }
        }

        private static InvestigationPoint FindNearestRegistered(
            Vector3 playerPosition, List<InvestigationPoint> nearby)
        {
            InvestigationPoint nearest = null;
            float nearestDistance = float.PositiveInfinity;
            if (nearby == null) return null;
            foreach (InvestigationPoint point in nearby)
            {
                if (point == null || !point.gameObject.activeInHierarchy) continue;
                float distance = (point.transform.position - playerPosition).sqrMagnitude;
                if (distance >= nearestDistance) continue;
                nearest = point;
                nearestDistance = distance;
            }
            return nearest;
        }

        private static object ReadField(object target, string fieldName)
        {
            FieldInfo field = target.GetType().GetField(fieldName,
                BindingFlags.Instance | BindingFlags.NonPublic);
            return field == null ? "FIELD_MISSING" : field.GetValue(target);
        }

        private static bool IsLibraryActive()
        {
            return SceneManager.GetActiveScene().name == LibrarySceneName;
        }
    }
}
