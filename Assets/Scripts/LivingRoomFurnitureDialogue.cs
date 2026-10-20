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
            return FindObjectOfType<DialogueController>()?.StartDialogue(
                CreateLocalizedPages(requestedKind) ?? pages) == true;
        }

        private static DialoguePage[] CreateLocalizedPages(InvestigationKind requestedKind)
        {
            string prefix;
            int count;
            switch (requestedKind)
            {
                case InvestigationKind.LinBedroomDesk:
                case InvestigationKind.LinBedroomChair:
                    prefix = "apartment.chunbaoBedroom.desk.";
                    count = 5;
                    break;
                case InvestigationKind.LinBedroomWardrobe:
                    prefix = "apartment.chunbaoBedroom.wardrobe.";
                    count = 3;
                    break;
                default:
                    return null;
            }

            DialoguePage[] localizedPages = new DialoguePage[count];
            for (int index = 0; index < count; index++)
                localizedPages[index] = new DialoguePage("主角", GameLocalization.Get(prefix + (index + 1)));
            return localizedPages;
        }
    }
}
