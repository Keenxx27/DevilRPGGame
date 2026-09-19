using UnityEngine;

namespace RPG
{
    public sealed class ProtagonistBedroomDialogue : MonoBehaviour, IInvestigationHandler
    {
        private DialoguePage[] pages;
        private InvestigationKind kind;

        public void Configure(InvestigationKind investigationKind, DialoguePage[] dialoguePages)
        {
            kind = investigationKind;
            pages = dialoguePages;
        }

        public bool CanInvestigate(InvestigationKind investigationKind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return investigationKind == kind && pages != null && pages.Length > 0
                && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind investigationKind)
        {
            return CanInvestigate(investigationKind)
                && FindObjectOfType<DialogueController>().StartDialogue(pages);
        }
    }

    public sealed class ProtagonistBedsideTable : MonoBehaviour, IInvestigationHandler,
        IRightClickSelectedItemUseHandler
    {
        private const string KeyName = "床头柜钥匙";
        private static readonly DialoguePage[] LockedPages =
        {
            new DialoguePage("主角", "这是我的床头柜，衣服什么的都放在里面。"),
            new DialoguePage("主角", "但是现在锁住了，钥匙我从昨天就一直在找。")
        };
        private static readonly DialoguePage[] HasKeyPages =
        {
            new DialoguePage("主角", "这是我的床头柜，衣服什么的都放在里面。"),
            new DialoguePage("主角", "现在找到了钥匙，终于可以打开它了。")
        };
        private static readonly DialoguePage[] OpenPages =
        {
            new DialoguePage("主角", "……除了衣服外，床头柜里有一张上面写着数字7的纸条。"),
            new DialoguePage("主角", "或许与密码锁有关。")
        };

        private bool unlocked;
        private bool opened;
        [SerializeField] private GameObject openDrawer;

        public void Configure(GameObject drawer)
        {
            openDrawer = drawer;
            if (openDrawer != null) openDrawer.SetActive(false);
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.ProtagonistBedsideTable && !opened
                && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            if (unlocked)
            {
                OpenDrawer();
                return FindObjectOfType<DialogueController>().StartDialogue(OpenPages);
            }

            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            DialoguePage[] pages = inventory != null && inventory.HasItem(KeyName)
                ? HasKeyPages : LockedPages;
            return FindObjectOfType<DialogueController>().StartDialogue(pages);
        }

        public bool TryUseSelectedItemWithRightClick(InvestigationKind kind)
        {
            if (kind != InvestigationKind.ProtagonistBedsideTable || unlocked || !CanInvestigate(kind)) return false;
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory?.SelectedItem == null || inventory.SelectedItem.DisplayName != KeyName) return false;

            if (!inventory.TryRemoveSelected(KeyName, out _)) return false;
            unlocked = true;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null) renderer.color = new Color(.19f, .1f, .06f, 1f);
            return true;
        }

        private void OpenDrawer()
        {
            opened = true;
            if (openDrawer != null) openDrawer.SetActive(true);
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null) renderer.color = new Color(.16f, .08f, .05f, 1f);
        }
    }
}
