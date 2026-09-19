using UnityEngine;

namespace RPG
{
    public sealed class LockedFoodCabinet : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] FirstInvestigationPages =
        {
            new DialoguePage("林春宝", "话说这房东也真是个狠人。\n欠他三个月的租金，不停水，不停电……"),
            new DialoguePage("林春宝", "居然把食品柜上了把锁！"),
            new DialoguePage("林春宝", "还是密码锁！"),
            new DialoguePage("主角", "这种情况不报警吗？"),
            new DialoguePage("林春宝", "万一警察见到你直接呼叫恶魔猎人了呢？")
        };
        private static readonly DialoguePage[] OpenPages =
        {
            new DialoguePage("主角", "终于是把这食品柜打开了……"),
            new DialoguePage("主角", "这袋肉干似乎是拿来吸引恶魔的不二之选，\n毕竟它们大部分都吃肉吧……")
        };
        private static readonly DialoguePage[] RepeatOpenPages =
        {
            new DialoguePage("主角", "我没必要带更多了。")
        };

        private DialogueController dialogue;
        private bool hasBeenInvestigated;
        private bool unlocked;
        private bool meatTaken;
        private FoodCabinetPasswordLock passwordLock;
        [SerializeField] private WorldItem driedMeat;

        public void Configure(WorldItem meat)
        {
            driedMeat = meat;
            if (driedMeat != null) driedMeat.gameObject.SetActive(false);
        }

        private void Start()
        {
            dialogue = FindObjectOfType<DialogueController>();
            passwordLock = GetComponent<FoodCabinetPasswordLock>();
            if (passwordLock == null) passwordLock = gameObject.AddComponent<FoodCabinetPasswordLock>();
        }

        public bool CanInvestigate(InvestigationKind kind) => kind == InvestigationKind.KitchenFoodCabinet
            && dialogue != null && !dialogue.IsPlaying;

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;

            if (unlocked)
            {
                if (meatTaken) return dialogue.StartDialogue(RepeatOpenPages);
                return dialogue.StartDialogue(OpenPages, GiveDriedMeat);
            }

            if (hasBeenInvestigated)
            {
                return passwordLock != null && passwordLock.Show(FindObjectOfType<PlayerInteraction>());
            }
            if (!dialogue.StartDialogue(FirstInvestigationPages, NotifyFirstInvestigationComplete))
            {
                return false;
            }

            hasBeenInvestigated = true;
            return true;
        }

        private static void NotifyFirstInvestigationComplete()
        {
            FindObjectOfType<LinChunbaoEncounter>()?.LeaveKitchenForLivingRoom();
        }

        public void OpenCabinet()
        {
            if (unlocked) return;
            unlocked = true;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null) renderer.color = new Color(.2f, .12f, .06f, 1f);
        }

        private void GiveDriedMeat()
        {
            if (driedMeat == null)
            {
                meatTaken = true;
                return;
            }

            driedMeat.gameObject.SetActive(true);
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            meatTaken = inventory != null && inventory.TryStore(driedMeat);
        }
    }
}
