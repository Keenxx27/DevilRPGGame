using UnityEngine;

namespace RPG
{
    public sealed class BathroomMedicineCabinet : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        private const string KeyId = "apartment.medicineCabinetKey";

        private bool introduced;
        private bool unlocked;
        [SerializeField] private MedicineCabinetSortingPuzzle sortingPuzzle;

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.BathroomMedicineCabinet && !unlocked
                && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            string[] keys = introduced
                ? new[] { "apartment.bathroom.medicineCabinet.repeat" }
                : new[]
                {
                    "apartment.bathroom.medicineCabinet.intro.1",
                    "apartment.bathroom.medicineCabinet.intro.2",
                    "apartment.bathroom.medicineCabinet.intro.3"
                };
            DialoguePage[] pages = new DialoguePage[keys.Length];
            for (int index = 0; index < keys.Length; index++)
                pages[index] = new DialoguePage("主角", GameLocalization.Get(keys[index]));
            if (!dialogue.StartDialogue(pages)) return false;
            introduced = true;
            return true;
        }

        public bool TryUseSelectedItem(InvestigationKind kind)
        {
            if (kind != InvestigationKind.BathroomMedicineCabinet || unlocked || !CanInvestigate(kind))
            {
                return false;
            }

            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            PlayerInteraction player = FindObjectOfType<PlayerInteraction>();
            if (inventory?.SelectedItem == null || inventory.SelectedItem.ItemId != KeyId
                || sortingPuzzle == null || !sortingPuzzle.Show(player)) return false;

            inventory.TryRemoveSelectedById(KeyId, out _);

            unlocked = true;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null) renderer.color = new Color(.5f, .55f, .56f, 1f);
            return true;
        }
    }
}
