using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public sealed class SchoolGate : MonoBehaviour, IInvestigationHandler
    {
        [SerializeField] private DoorController door;

        private DialogueController dialogueController;
        private bool departureConfirmed;
        private DialoguePage[] beforeLibraryPages;
        private DialoguePage[] beforePagePages;
        private DialoguePage[] readyToLeavePages;

        private void Awake()
        {
            beforeLibraryPages = BuildPages("corridor.gate.beforeLibrary");
            beforePagePages = BuildPages("corridor.gate.beforePage");
            readyToLeavePages = BuildPages("corridor.gate.ready");
        }

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
                return dialogueController.StartDialogue(beforeLibraryPages);
            }

            LibraryBookPuzzle puzzle = FindObjectOfType<LibraryBookPuzzle>();
            if (puzzle == null || !puzzle.HasReadCompletePage)
            {
                return dialogueController.StartDialogue(beforePagePages);
            }

            if (!departureConfirmed)
            {
                return dialogueController.StartDialogue(readyToLeavePages,
                    () => departureConfirmed = true);
            }

            return door.ToggleDoor();
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
