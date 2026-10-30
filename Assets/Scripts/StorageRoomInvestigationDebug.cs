using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    /// <summary>
    /// Runtime-only diagnostics for the five storage-room investigation points.
    /// It does not change puzzle state or interaction ranges.
    /// </summary>
    public sealed class StorageRoomInvestigationDebug : MonoBehaviour
    {
        private const string SceneName = "4_SchoolStorageRoom";
        private const string Prefix = "[StorageRoomInvestigationDebug]";
        private static readonly FieldInfo NearbyInvestigationsField =
            typeof(PlayerInteraction).GetField("nearbyInvestigations",
                BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo BroomInvestigatedField =
            typeof(StorageRoomPuzzle).GetField("broomInvestigated",
                BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly HashSet<InvestigationKind> observedInvestigations =
            new HashSet<InvestigationKind>();
        private PlayerInteraction player;
        private StorageRoomPuzzle puzzle;
        private string previousState;
        private float nextAuditTime;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (FindObjectOfType<StorageRoomInvestigationDebug>() != null) return;
            GameObject host = new GameObject("Storage Room Investigation Debug");
            DontDestroyOnLoad(host);
            host.AddComponent<StorageRoomInvestigationDebug>();
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name != SceneName) return;

            if (player == null) player = FindObjectOfType<PlayerInteraction>();
            if (puzzle == null) puzzle = FindObjectOfType<StorageRoomPuzzle>();

            if (Time.unscaledTime >= nextAuditTime)
            {
                nextAuditTime = Time.unscaledTime + 0.5f;
                LogWhenStateChanges();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(LogInteractionResult());
            }
        }

        private void LogWhenStateChanges()
        {
            string state = BuildState(false);
            if (state == previousState) return;
            previousState = state;
            Debug.Log(Prefix + " State changed\n" + BuildState(true));
        }

        private IEnumerator LogInteractionResult()
        {
            InvestigationPoint candidate = FindNearestRegisteredPoint();
            string before = candidate == null ? "none" : candidate.Kind.ToString();
            yield return null;

            DialogueController dialogue = player != null
                ? player.GetComponent<DialogueController>() : null;
            bool dialogueStarted = dialogue != null && dialogue.IsPlaying;
            if (candidate != null && dialogueStarted)
            {
                observedInvestigations.Add(candidate.Kind);
            }

            Debug.Log(Prefix + " E result | candidate=" + before
                + " dialogueStarted=" + dialogueStarted
                + " observedInvestigated=[" + string.Join(", ",
                    observedInvestigations.Select(kind => kind.ToString()).ToArray()) + "]\n"
                + BuildState(true));
        }

        private InvestigationPoint FindNearestRegisteredPoint()
        {
            List<InvestigationPoint> registered = GetRegisteredPoints();
            if (player == null || registered.Count == 0) return null;

            return registered
                .Where(point => point != null && point.gameObject.activeInHierarchy)
                .OrderBy(point =>
                    ((Vector2)point.transform.position - (Vector2)player.transform.position)
                    .sqrMagnitude)
                .FirstOrDefault();
        }

        private List<InvestigationPoint> GetRegisteredPoints()
        {
            if (player == null || NearbyInvestigationsField == null)
            {
                return new List<InvestigationPoint>();
            }

            return NearbyInvestigationsField.GetValue(player)
                as List<InvestigationPoint> ?? new List<InvestigationPoint>();
        }

        private string BuildState(bool detailed)
        {
            if (player == null)
            {
                return "player=missing";
            }

            List<InvestigationPoint> registered = GetRegisteredPoints();
            Collider2D playerCollider = player.GetComponentInChildren<Collider2D>();
            InvestigationPoint[] points = FindObjectsOfType<InvestigationPoint>(true)
                .Where(point => point.gameObject.scene.name == SceneName)
                .OrderBy(point => point.Kind)
                .ToArray();

            List<string> rows = new List<string>
            {
                "playerPos=" + Format(player.transform.position)
                    + " playerCollider=" + Describe(playerCollider)
                    + " registered=" + registered.Count
            };

            foreach (InvestigationPoint point in points)
            {
                Collider2D trigger = point.GetComponent<Collider2D>();
                bool registeredNow = registered.Contains(point);
                bool touching = playerCollider != null && trigger != null
                    && playerCollider.IsTouching(trigger);
                string row = point.Kind
                    + " object='" + point.name + "'"
                    + " pos=" + Format(point.transform.position)
                    + " available=" + point.IsAvailable
                    + " registered=" + registeredNow
                    + " touching=" + touching;
                if (detailed) row += " trigger=" + Describe(trigger);
                rows.Add(row);
            }

            bool broomInvestigated = puzzle != null && BroomInvestigatedField != null
                && (bool)BroomInvestigatedField.GetValue(puzzle);
            rows.Add("broomInvestigated=" + broomInvestigated
                + " observedInvestigated=[" + string.Join(", ",
                    observedInvestigations.Select(kind => kind.ToString()).ToArray()) + "]");
            return string.Join("\n", rows.ToArray());
        }

        private static string Describe(Collider2D collider)
        {
            if (collider == null) return "missing";
            Bounds bounds = collider.bounds;
            return collider.GetType().Name
                + "(enabled=" + collider.enabled
                + ",trigger=" + collider.isTrigger
                + ",center=" + Format(bounds.center)
                + ",size=" + Format(bounds.size) + ")";
        }

        private static string Format(Vector3 value)
        {
            return "(" + value.x.ToString("F2") + ","
                + value.y.ToString("F2") + "," + value.z.ToString("F2") + ")";
        }
    }
}
