using UnityEngine;

namespace RPG
{
    public sealed class BathroomPasswordNote : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("主角", "虽然一大半都被胶糊住了，但我还是看得出来……是一张上面写着数字8的纸条。"),
            new DialoguePage("主角", "或许与密码锁有关。")
        };

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.PasswordNote && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            return FindObjectOfType<DialogueController>().StartDialogue(Pages);
        }
    }
}
