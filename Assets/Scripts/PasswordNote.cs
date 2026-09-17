using UnityEngine;

namespace RPG
{
    public sealed class PasswordNote : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("主角", "……是一张上面写着数字6的纸条。"),
            new DialoguePage("主角", "或许与密码锁有关。")
        };

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.PasswordNote && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            return CanInvestigate(kind)
                && FindObjectOfType<DialogueController>().StartDialogue(Pages);
        }
    }
}
