using UnityEngine;

namespace RPG
{
    public sealed class StorageRoomPuzzle : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] LeftShelfPages =
        {
            new DialoguePage("主角", "架子上全是受潮的清洁用品和旧档案。\n没有能用的。")
        };
        private static readonly DialoguePage[] RightShelfPages =
        {
            new DialoguePage("主角", "墙边堆满了工具和杂物。\n黑暗里很难分辨清楚。")
        };
        private static readonly DialoguePage[] NorthCratePages =
        {
            new DialoguePage("主角", "箱子里是替换下来的桌椅零件。\n翻不出什么特别的。")
        };
        private static readonly DialoguePage[] SouthCratePages =
        {
            new DialoguePage("主角", "这里有股发霉的纸味。\n只是些报废的试卷和杂物。")
        };
        private static readonly DialoguePage[] BroomWithoutGluePages =
        {
            new DialoguePage("主角", "这是……一把扫帚。\n暂时不知道能有什么用。")
        };
        private static readonly DialoguePage[] BroomWithGluePages =
        {
            new DialoguePage("主角", "这是……一把扫帚。\n正是我要的东西。")
        };

        [SerializeField] private WorldItem broom;

        private DialogueController dialogueController;
        private PlayerInventory inventory;
        private bool broomInvestigated;

        private void Start()
        {
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
                    return dialogueController.StartDialogue(LeftShelfPages);
                case InvestigationKind.StorageShelfRight:
                    return dialogueController.StartDialogue(RightShelfPages);
                case InvestigationKind.StorageCrateNorth:
                    return dialogueController.StartDialogue(NorthCratePages);
                case InvestigationKind.StorageCrateSouth:
                    return dialogueController.StartDialogue(SouthCratePages);
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
                ? BroomWithGluePages : BroomWithoutGluePages;
            return dialogueController.StartDialogue(pages, () =>
            {
                broomInvestigated = true;
                broom.SetPickupEnabled(true);
                inventory.RegisterNearby(broom);
            });
        }
    }
}
