using UnityEngine;

namespace RPG
{
    public sealed class MapTransitionPoint : MonoBehaviour, IInvestigationHandler
    {
        private const float ActivationDistance = .6f;
        private const float ResetDistance = 1.1f;
        private const float RequiredStayDuration = .15f;

        [SerializeField] private string destinationScene;
        [SerializeField] private string destinationId;
        private float stayTimer;
        private bool hasActivated;

        public void Configure(string sceneName, string spawnId)
        {
            destinationScene = sceneName;
            destinationId = spawnId;
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.SuburbanRoadTransition && dialogue != null
                && !dialogue.IsPlaying && !string.IsNullOrWhiteSpace(destinationId);
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            MapTransitionService transition = FindObjectOfType<PlayerInteraction>()
                ?.GetComponent<MapTransitionService>();
            if (transition == null) return false;
            transition.RequestTransition(destinationScene, destinationId);
            return true;
        }

        private void Update()
        {
            PlayerInteraction player = FindObjectOfType<PlayerInteraction>();
            if (player == null) return;

            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance > ResetDistance)
            {
                hasActivated = false;
                stayTimer = 0f;
                return;
            }

            if (hasActivated || distance > ActivationDistance) return;
            DialogueController dialogue = player.GetComponent<DialogueController>();
            if (dialogue == null || dialogue.IsPlaying) return;

            stayTimer += Time.deltaTime;
            if (stayTimer < RequiredStayDuration) return;

            MapTransitionService transition = player.GetComponent<MapTransitionService>();
            if (transition == null) return;
            hasActivated = true;
            transition.RequestTransition(destinationScene, destinationId);
        }
    }
}
