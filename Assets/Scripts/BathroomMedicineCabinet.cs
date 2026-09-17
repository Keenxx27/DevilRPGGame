using UnityEngine;

namespace RPG
{
    public sealed class BathroomMedicineCabinet : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        private const string KeyName = "药柜钥匙";
        private static readonly DialoguePage[] FirstPages =
        {
            new DialoguePage("主角", "药柜被锁住了……"),
            new DialoguePage("主角", "看来还是房东干的，我每天都要从里面拿膏药出来贴在脸上，不可能把它锁住的。"),
            new DialoguePage("主角", "话说回来……钥匙去哪里了？")
        };
        private static readonly DialoguePage[] RepeatPages =
        {
            new DialoguePage("主角", "现在的当务之急是找到药柜的钥匙。")
        };

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
            if (!dialogue.StartDialogue(introduced ? RepeatPages : FirstPages)) return false;
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
            if (inventory?.SelectedItem == null || inventory.SelectedItem.DisplayName != KeyName
                || sortingPuzzle == null || !sortingPuzzle.Show(player)) return false;

            inventory.TryRemoveSelected(KeyName, out _);

            unlocked = true;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null) renderer.color = new Color(.5f, .55f, .56f, 1f);
            return true;
        }
    }
}
