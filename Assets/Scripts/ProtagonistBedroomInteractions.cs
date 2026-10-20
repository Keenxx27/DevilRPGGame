using UnityEngine;

namespace RPG
{
    public sealed class ProtagonistBedroomDialogue : MonoBehaviour, IInvestigationHandler
    {
        public void Configure(InvestigationKind investigationKind, DialoguePage[] dialoguePages)
        {
            // 保留安装器兼容入口；运行时文本按调查类型和当前语言生成。
        }

        public bool CanInvestigate(InvestigationKind investigationKind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return IsSupported(investigationKind)
                && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind investigationKind)
        {
            return CanInvestigate(investigationKind)
                && FindObjectOfType<DialogueController>().StartDialogue(CreatePages(investigationKind));
        }

        private static bool IsSupported(InvestigationKind investigationKind) =>
            investigationKind == InvestigationKind.ProtagonistBedroomBed
            || investigationKind == InvestigationKind.ProtagonistPlasterBoxes;

        private static DialoguePage[] CreatePages(InvestigationKind investigationKind)
        {
            string prefix = investigationKind == InvestigationKind.ProtagonistBedroomBed
                ? "apartment.protagonistBedroom.bed."
                : "apartment.protagonistBedroom.plasterBoxes.";
            int count = investigationKind == InvestigationKind.ProtagonistBedroomBed ? 2 : 5;
            DialoguePage[] localizedPages = new DialoguePage[count];
            for (int index = 0; index < count; index++)
                localizedPages[index] = new DialoguePage("主角", GameLocalization.Get(prefix + (index + 1)));
            return localizedPages;
        }
    }

    public sealed class ProtagonistBedsideTable : MonoBehaviour, IInvestigationHandler,
        IRightClickSelectedItemUseHandler
    {
        private const string KeyId = "apartment.bedsideKey";

        private bool unlocked;
        private bool opened;
        [SerializeField] private GameObject openDrawer;
        [SerializeField] private GameObject closedArtwork;
        [SerializeField] private GameObject openArtwork;

        public void Configure(GameObject drawer)
        {
            openDrawer = drawer;
            if (openDrawer != null) openDrawer.SetActive(false);
        }

        public void ConfigureArtwork(GameObject closedStateArtwork, GameObject openStateArtwork)
        {
            closedArtwork = closedStateArtwork;
            openArtwork = openStateArtwork;
            if (closedArtwork != null) closedArtwork.SetActive(true);
            if (openArtwork != null) openArtwork.SetActive(false);
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
                return FindObjectOfType<DialogueController>().StartDialogue(PlayerPages(
                    "apartment.protagonistBedroom.bedside.open.1",
                    "apartment.protagonistBedroom.bedside.open.2"));
            }

            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            DialoguePage[] pages = inventory != null && inventory.HasItemById(KeyId)
                ? PlayerPages("apartment.protagonistBedroom.bedside.hasKey.1",
                    "apartment.protagonistBedroom.bedside.hasKey.2")
                : PlayerPages("apartment.protagonistBedroom.bedside.locked.1",
                    "apartment.protagonistBedroom.bedside.locked.2");
            return FindObjectOfType<DialogueController>().StartDialogue(pages);
        }

        public bool TryUseSelectedItemWithRightClick(InvestigationKind kind)
        {
            if (kind != InvestigationKind.ProtagonistBedsideTable || unlocked || !CanInvestigate(kind)) return false;
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory?.SelectedItem == null || inventory.SelectedItem.ItemId != KeyId) return false;

            if (!inventory.TryRemoveSelectedById(KeyId, out _)) return false;
            unlocked = true;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null) renderer.color = new Color(.19f, .1f, .06f, 1f);
            return true;
        }

        private void OpenDrawer()
        {
            opened = true;
            if (openDrawer != null) openDrawer.SetActive(true);
            if (closedArtwork != null) closedArtwork.SetActive(false);
            if (openArtwork != null) openArtwork.SetActive(true);
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null) renderer.color = new Color(.16f, .08f, .05f, 1f);
        }

        private static DialoguePage[] PlayerPages(params string[] keys)
        {
            DialoguePage[] localizedPages = new DialoguePage[keys.Length];
            for (int index = 0; index < keys.Length; index++)
                localizedPages[index] = new DialoguePage("主角", GameLocalization.Get(keys[index]));
            return localizedPages;
        }
    }
}
