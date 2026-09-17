using UnityEngine;

namespace RPG
{
    public sealed class LivingRoomFurnitureDialogue : MonoBehaviour, IInvestigationHandler
    {
        [SerializeField] private InvestigationKind kind;
        [SerializeField] private DialoguePage[] pages;

        public bool CanInvestigate(InvestigationKind requestedKind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return requestedKind == kind && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind requestedKind)
        {
            if (!CanInvestigate(requestedKind)) return false;
            return FindObjectOfType<DialogueController>()?.StartDialogue(pages) == true;
        }
    }
}
