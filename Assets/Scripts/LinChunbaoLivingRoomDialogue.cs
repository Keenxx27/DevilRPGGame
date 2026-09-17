using UnityEngine;

namespace RPG
{
    public sealed class LinChunbaoLivingRoomDialogue : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        private const string StickyKnifeName = "粘竿菜刀";
        private static readonly DialoguePage[] FirstPages =
        {
            new DialoguePage("林春宝", "在家里找找看，也许能有解开密码的线索。")
        };

        private static readonly DialoguePage[] RepeatPages =
        {
            new DialoguePage("林春宝", "看我干嘛，我怎么知道密码？")
        };
        private static readonly DialoguePage[] RepairSandbagPages =
        {
            new DialoguePage("林春宝", "你是把那胶布贴到了菜刀上？"),
            new DialoguePage("主角", "也许这膏药能派上用场……"),
            new DialoguePage("林春宝", "就它了！要是用针线缝缝补补的要花掉我一整晚。"),
            new DialoguePage("主角", "所以……"),
            new DialoguePage("林春宝", "嗯？"),
            new DialoguePage("主角", "我能去你的房间里找线索了吗？"),
            new DialoguePage("林春宝", "随便你，去吧！")
        };

        private bool isAtSofa;
        private bool hasBeenInvestigated;

        public bool IsAtSofa => isAtSofa;

        public void ActivateAtSofa()
        {
            isAtSofa = true;
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.LivingRoomLinChunbao
                && isAtSofa && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            DialoguePage[] pages = hasBeenInvestigated ? RepeatPages : FirstPages;
            if (!dialogue.StartDialogue(pages)) return false;
            hasBeenInvestigated = true;
            return true;
        }

        public bool TryUseSelectedItem(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            SandbagBoxingMinigame boxing = FindObjectOfType<SandbagBoxingMinigame>();
            if (boxing == null || !boxing.IsBroken) return false;

            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory?.SelectedItem == null || inventory.SelectedItem.DisplayName != StickyKnifeName)
            {
                return false;
            }

            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (!dialogue.StartDialogue(RepairSandbagPages, GrantBedroomAccess)) return false;
            inventory.TryRemoveSelected(StickyKnifeName, out _);
            return true;
        }

        private static void GrantBedroomAccess()
        {
            FindObjectOfType<LinChunbaoBedroomDoor>()?.GrantAccess();
        }
    }
}
