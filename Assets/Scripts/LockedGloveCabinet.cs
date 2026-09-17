using UnityEngine;

namespace RPG
{
    public sealed class LockedGloveCabinet : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        private const string KnifeName = "菜刀";
        private static readonly DialoguePage[] IntroductionPages =
        {
            new DialoguePage("林春宝", "我才想起来，拳击手套放在这里头。"),
            new DialoguePage("主角", "看上去柜门铰链生锈了开不了啊。"),
            new DialoguePage("林春宝", "那就让它开得了！")
        };
        private static readonly DialoguePage[] RepeatPages =
        {
            new DialoguePage("主角", "柜门的铰链锈死了，得找东西撬开。")
        };
        private static readonly DialoguePage[] OpenPages =
        {
            new DialoguePage("主角", "菜刀正好能撬开这扇柜门。"),
            new DialoguePage("主角", "拳击手套在里面。")
        };

        [SerializeField] private WorldItem boxingGloves;
        private bool introduced;
        private bool opened;

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.LivingRoomLowCabinet && !opened
                && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            if (!FindObjectOfType<DialogueController>().StartDialogue(
                    introduced ? RepeatPages : IntroductionPages)) return false;
            introduced = true;
            return true;
        }

        public bool TryUseSelectedItem(InvestigationKind kind)
        {
            if (kind != InvestigationKind.LivingRoomLowCabinet || !introduced || opened) return false;
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory?.SelectedItem == null || inventory.SelectedItem.DisplayName != KnifeName) return false;

            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null || dialogue.IsPlaying || !dialogue.StartDialogue(OpenPages, OpenCabinet))
            {
                return false;
            }
            opened = true;
            return true;
        }

        private void OpenCabinet()
        {
            if (boxingGloves != null) boxingGloves.gameObject.SetActive(true);
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null) renderer.color = new Color(.26f, .16f, .1f, 1f);
        }
    }
}
