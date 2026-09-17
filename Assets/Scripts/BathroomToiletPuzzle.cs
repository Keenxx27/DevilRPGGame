using UnityEngine;

namespace RPG
{
    public sealed class BathroomToiletPuzzle : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] IntroductionPages =
        {
            new DialoguePage("主角", "也许我该往这里面看看……说不准会发现什么。"),
            new DialoguePage("主角", "该怎么做呢？"),
            new DialoguePage("主角", "例如说……冲马桶？")
        };
        private static readonly DialoguePage[] SecondFlushPages =
        {
            new DialoguePage("林春宝", "哥，你在冲马桶吗？"),
            new DialoguePage("林春宝", "省着点用水，我可不想再打几次黑拳挣水费！")
        };
        private static readonly DialoguePage[] ThirdFlushPages =
        {
            new DialoguePage("林春宝", "你怕不是又把浴盐球掉进去了？")
        };
        private static readonly DialoguePage[] FourthFlushPages =
        {
            new DialoguePage("主角", "……这房东果真是个狠人。"),
            new DialoguePage("主角", "而且也是个坏人。拿准了我因为脸上这道痕迹而不敢报警的把柄。")
        };

        [SerializeField] private WorldItem medicineCabinetKey;
        private bool introduced;
        private int flushCount;

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.BathroomToilet && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (!introduced)
            {
                if (!dialogue.StartDialogue(IntroductionPages)) return false;
                introduced = true;
                return true;
            }

            flushCount++;
            switch (flushCount)
            {
                case 1:
                    return true;
                case 2:
                    return dialogue.StartDialogue(SecondFlushPages);
                case 3:
                    return dialogue.StartDialogue(ThirdFlushPages);
                case 4:
                    return dialogue.StartDialogue(FourthFlushPages, RevealKey);
                default:
                    return true;
            }
        }

        private void RevealKey()
        {
            if (medicineCabinetKey != null) medicineCabinetKey.gameObject.SetActive(true);
        }
    }
}
