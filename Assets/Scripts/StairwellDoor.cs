using UnityEngine;

namespace RPG
{
    public sealed class StairwellDoor : MonoBehaviour, IInvestigationHandler
    {
        private DialogueController dialogueController;
        private DialoguePage[] pages;

        private void Awake()
        {
            pages = new[]
            {
                new DialoguePage("主角", GameLocalization.Get("corridor.stairwell.1")),
                new DialoguePage("主角", GameLocalization.Get("corridor.stairwell.2"))
            };
        }

        private void Start()
        {
            dialogueController = FindObjectOfType<DialogueController>();
            if (dialogueController == null)
            {
                Debug.LogError("StairwellDoor 缺少 DialogueController。", this);
                enabled = false;
            }
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            return enabled && kind == InvestigationKind.StairwellDoor
                && dialogueController != null && !dialogueController.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            return CanInvestigate(kind) && dialogueController.StartDialogue(pages);
        }
    }
}
