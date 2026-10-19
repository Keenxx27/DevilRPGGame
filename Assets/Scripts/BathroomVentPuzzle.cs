using UnityEngine;

namespace RPG
{
    public sealed class BathroomVentPuzzle : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        private const string KnifeId = "apartment.knife";
        private const string StickyKnifeId = "apartment.stickyKnife";

        [SerializeField] private WorldItem passwordNote;
        private bool stickyKnifePrompted;
        private bool notePulled;
        private int pulls;

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.BathroomVent && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            return FindObjectOfType<DialogueController>().StartDialogue(CreatePages(
                "apartment.bathroom.vent.observe", "apartment.bathroom.vent.default"));
        }

        public bool TryUseSelectedItem(InvestigationKind kind)
        {
            if (kind != InvestigationKind.BathroomVent || !CanInvestigate(kind)) return false;
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory?.SelectedItem == null) return false;
            if (inventory.SelectedItem.ItemId == StickyKnifeId && !notePulled)
            {
                if (!stickyKnifePrompted)
                {
                    if (!FindObjectOfType<DialogueController>().StartDialogue(CreatePages(
                            "apartment.bathroom.vent.observe", "apartment.bathroom.vent.stickyKnife"))) return false;
                    stickyKnifePrompted = true;
                    return true;
                }

                pulls++;
                if (pulls >= 3)
                {
                    notePulled = true;
                    if (passwordNote != null) passwordNote.gameObject.SetActive(true);
                }
                return true;
            }

            if (inventory.SelectedItem.ItemId != KnifeId)
            {
                return false;
            }

            return FindObjectOfType<DialogueController>().StartDialogue(CreatePages(
                "apartment.bathroom.vent.observe", "apartment.bathroom.vent.knife.1",
                "apartment.bathroom.vent.knife.2"));
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
