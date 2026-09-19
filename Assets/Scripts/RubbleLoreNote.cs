using UnityEngine;

namespace RPG
{
    public sealed class RubbleLoreNote : MonoBehaviour, IInvestigationHandler
    {
        [SerializeField] private RubbleBossEncounter encounter;
        [SerializeField] private bool isTuitionReceipt;
        [SerializeField] private bool hasBeenRead;

        private static readonly DialoguePage[] TuitionPages =
        {
            new DialoguePage("主角", "其实我们大学的学费也不贵啊……"),
            new DialoguePage("主角", "毕竟现代社会人人都想当高收入的恶魔猎人，\n已经不怎么重视文化教育了。"),
            new DialoguePage("主角", "那你又有什么理由杀我？"),
            new DialoguePage("丧家犬老大", "……")
        };

        private static readonly DialoguePage[] DebtPages =
        {
            new DialoguePage("主角", "这账单上的东西不太一般啊，不是水电费，而是……"),
            new DialoguePage("主角", "购买大牌恶魔猎人周边的消费。"),
            new DialoguePage("主角", "看来你给儿子凑的钱好像不只是学费。"),
            new DialoguePage("丧家犬老大", "……")
        };

        public void Configure(RubbleBossEncounter owner, bool tuitionReceipt)
        {
            encounter = owner;
            isTuitionReceipt = tuitionReceipt;
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            return kind == InvestigationKind.RubbleLoreNote && !hasBeenRead && encounter != null
                && encounter.IsBattleActive;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            if (!encounter.PlayLoreDialogue(isTuitionReceipt ? TuitionPages : DebtPages)) return false;
            hasBeenRead = true;
            return true;
        }
    }
}
