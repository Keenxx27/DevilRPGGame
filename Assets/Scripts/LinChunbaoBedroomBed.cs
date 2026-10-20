using UnityEngine;

namespace RPG
{
    public sealed class LinChunbaoBedroomBed : MonoBehaviour, IInvestigationHandler
    {
        [SerializeField] private WorldItem phone;
        private bool explored;

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.LinBedroomBed && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (explored) return dialogue.StartDialogue(CreatePages(
                "apartment.chunbaoBedroom.bed.repeat"));
            if (!dialogue.StartDialogue(CreatePages(
                    "apartment.chunbaoBedroom.bed.first.1", "apartment.chunbaoBedroom.bed.first.2",
                    "apartment.chunbaoBedroom.bed.first.3", "apartment.chunbaoBedroom.bed.first.4",
                    "apartment.chunbaoBedroom.bed.first.5", "apartment.chunbaoBedroom.bed.first.6",
                    "apartment.chunbaoBedroom.bed.first.7"), RevealPhone)) return false;
            explored = true;
            return true;
        }

        private void RevealPhone()
        {
            if (phone != null) phone.gameObject.SetActive(true);
        }

        private static DialoguePage[] CreatePages(params string[] keys)
        {
            DialoguePage[] pages = new DialoguePage[keys.Length];
            for (int index = 0; index < keys.Length; index++)
                pages[index] = new DialoguePage("主角", GameLocalization.Get(keys[index]));
            return pages;
        }
    }
}
