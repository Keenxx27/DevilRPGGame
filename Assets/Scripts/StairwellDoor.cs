using UnityEngine;

namespace RPG
{
    public sealed class StairwellDoor : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("主角", "是通往楼梯间的门，上了楼就是其他教室。"),
            new DialoguePage("主角", "我已经上完课了，没必要回楼上。")
        };

        private DialogueController dialogueController;

        private void Start()
        {
            dialogueController = FindObjectOfType<DialogueController>();
            if (dialogueController == null)
            {
                Debug.LogError("StairwellDoor 缺少 DialogueController。", this);
                enabled = false;
            }
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            return enabled && kind == InvestigationKind.StairwellDoor
                && dialogueController != null && !dialogueController.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            return CanInvestigate(kind) && dialogueController.StartDialogue(Pages);
        }
    }
}
