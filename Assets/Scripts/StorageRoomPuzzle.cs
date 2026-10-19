using UnityEngine;

namespace RPG
{
    public sealed class StorageRoomPuzzle : MonoBehaviour, IInvestigationHandler
    {
        [SerializeField] private WorldItem broom;

        private DialogueController dialogueController;
        private PlayerInventory inventory;
        private bool broomInvestigated;
        private DialoguePage[] leftShelfPages;
        private DialoguePage[] rightShelfPages;
        private DialoguePage[] northCratePages;
        private DialoguePage[] southCratePages;
        private DialoguePage[] broomWithoutGluePages;
        private DialoguePage[] broomWithGluePages;

        private void Awake()
        {
            leftShelfPages = BuildPages("storage.shelf.left");
            rightShelfPages = BuildPages("storage.shelf.right");
            northCratePages = BuildPages("storage.crate.north");
            southCratePages = BuildPages("storage.crate.south");
            broomWithoutGluePages = BuildPages("storage.broom.beforeGlue");
            broomWithGluePages = BuildPages("storage.broom.afterGlue");
        }

        private void Start()
        {
            if (broom != null)
            {
                broom.ConfigureLocalization("storage.broom", "storage.item.broom");
            }
            dialogueController = FindObjectOfType<DialogueController>();
            inventory = FindObjectOfType<PlayerInventory>();
            if (dialogueController == null || inventory == null || broom == null)
            {
                Debug.LogError("StorageRoomPuzzle 缺少对话、物品栏或扫帚。", this);
                enabled = false;
            }
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            if (!enabled || dialogueController == null || dialogueController.IsPlaying)
            {
                return false;
            }

            switch (kind)
            {
                case InvestigationKind.StorageShelfLeft:
                case InvestigationKind.StorageShelfRight:
                case InvestigationKind.StorageCrateNorth:
                case InvestigationKind.StorageCrateSouth:
                    return true;
                case InvestigationKind.StorageBroom:
                    return !broomInvestigated;
                default:
                    return false;
            }
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            switch (kind)
            {
                case InvestigationKind.StorageShelfLeft:
                    return dialogueController.StartDialogue(leftShelfPages);
                case InvestigationKind.StorageShelfRight:
                    return dialogueController.StartDialogue(rightShelfPages);
                case InvestigationKind.StorageCrateNorth:
                    return dialogueController.StartDialogue(northCratePages);
                case InvestigationKind.StorageCrateSouth:
                    return dialogueController.StartDialogue(southCratePages);
                case InvestigationKind.StorageBroom:
                    return InvestigateBroom();
                default:
                    return false;
            }
        }

        private bool InvestigateBroom()
        {
            LibraryBookPuzzle libraryPuzzle = FindObjectOfType<LibraryBookPuzzle>();
            DialoguePage[] pages = libraryPuzzle != null && libraryPuzzle.HasDiscoveredGlue
                ? broomWithGluePages : broomWithoutGluePages;
            return dialogueController.StartDialogue(pages, () =>
            {
                broomInvestigated = true;
                broom.SetPickupEnabled(true);
                inventory.RegisterNearby(broom);
            });
        }

        private static DialoguePage[] BuildPages(params string[] keys)
        {
            DialoguePage[] pages = new DialoguePage[keys.Length];
            for (int index = 0; index < keys.Length; index++)
            {
                pages[index] = new DialoguePage("主角", GameLocalization.Get(keys[index]));
            }

            return pages;
        }
    }
}
