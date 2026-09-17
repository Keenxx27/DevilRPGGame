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

        private DialogueController dialogue;
        private bool hasBeenInvestigated;
        private bool opened;
        private FoodCabinetPasswordLock passwordLock;

        private void Start()
        {
            dialogue = FindObjectOfType<DialogueController>();
            passwordLock = GetComponent<FoodCabinetPasswordLock>();
            if (passwordLock == null) passwordLock = gameObject.AddComponent<FoodCabinetPasswordLock>();
        }

        public bool CanInvestigate(InvestigationKind kind) => kind == InvestigationKind.KitchenFoodCabinet
            && !opened && dialogue != null && !dialogue.IsPlaying;

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;

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
            if (opened) return;
            opened = true;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null) renderer.color = new Color(.2f, .12f, .06f, 1f);
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null) collider.enabled = false;
        }
    }
}
