using UnityEngine;

namespace RPG
{
    public sealed class LivingRoomSandbag : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        private static readonly DialoguePage[] BeforeFoodCabinetPages =
        {
            new DialoguePage("主角", "这是春宝平时拿来锻炼用的沙袋。"),
            new DialoguePage("主角", "春宝在高中里是搏击社的社长，\n一直表现突出。")
        };

        private static readonly DialoguePage[] AfterFoodCabinetPages =
        {
            new DialoguePage("林春宝", "要把我这沙袋想象成房东那张可恶的大脸\n打一拳的话，最好先带上拳击手套。")
        };

        [SerializeField] private SandbagBoxingMinigame boxing;

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.LivingRoomSandbag && (boxing == null || !boxing.IsBroken)
                && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            LinChunbaoLivingRoomDialogue lin = FindObjectOfType<LinChunbaoLivingRoomDialogue>();
            return dialogue.StartDialogue(lin != null && lin.IsAtSofa
                ? AfterFoodCabinetPages
                : BeforeFoodCabinetPages);
        }

        public bool TryUseSelectedItem(InvestigationKind kind)
        {
            if (kind != InvestigationKind.LivingRoomSandbag || boxing == null || boxing.IsBroken)
            {
                return false;
            }
            PlayerInteraction player = FindObjectOfType<PlayerInteraction>();
            return player?.GetComponent<PlayerInventory>()?.SelectedItem?.DisplayName == "拳击手套"
                && boxing.StartBoxing(player);
        }
    }
}
