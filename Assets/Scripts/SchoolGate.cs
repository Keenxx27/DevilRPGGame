using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public sealed class SchoolGate : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] BeforeLibraryPages =
        {
            new DialoguePage("主角", "这是学校的大门，\n但我还没去图书馆，不该回家。")
        };
        private static readonly DialoguePage[] BeforePagePages =
        {
            new DialoguePage("主角", "这是学校的大门，\n但我觉得还没到回去的时候……")
        };
        private static readonly DialoguePage[] ReadyToLeavePages =
        {
            new DialoguePage("主角", "这是学校的大门，\n通过它我便能坐公交回家了。")
        };

        [SerializeField] private DoorController door;

        private DialogueController dialogueController;
        private bool departureConfirmed;

        private void Start()
        {
            dialogueController = FindObjectOfType<DialogueController>();
            if (dialogueController == null || door == null)
            {
                Debug.LogError("SchoolGate 缺少对话控制器或门组件。", this);
                enabled = false;
            }
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            return enabled && kind == InvestigationKind.SchoolGate
                && dialogueController != null && !dialogueController.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            if (!SceneManager.GetSceneByName("SchoolLibrary").isLoaded)
            {
                return dialogueController.StartDialogue(BeforeLibraryPages);
            }

            LibraryBookPuzzle puzzle = FindObjectOfType<LibraryBookPuzzle>();
            if (puzzle == null || !puzzle.HasReadCompletePage)
            {
                return dialogueController.StartDialogue(BeforePagePages);
            }

            if (!departureConfirmed)
            {
                return dialogueController.StartDialogue(ReadyToLeavePages,
                    () => departureConfirmed = true);
            }

            return door.ToggleDoor();
        }
    }
}
