using UnityEngine;

namespace RPG
{
    public sealed class LinChunbaoBedroomBed : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] FirstPages =
        {
            new DialoguePage("主角", "春宝的厨艺很好，但一直不怎么勤做家务，床铺一直很乱。"),
            new DialoguePage("主角", "每天的学业已经很繁重了，有时候还要去打黑拳赚钱，她怎么可能有时间呢？"),
            new DialoguePage("主角", "等等，这是……"),
            new DialoguePage("主角", "是春宝的手机，还开着，上面在运行一款游戏。"),
            new DialoguePage("主角", "看来跟往常一样，她想不靠任何人的帮助将游戏破关。"),
            new DialoguePage("主角", "她一直这么要强。"),
            new DialoguePage("主角", "这次就由我来吧，就当是给她一个惊喜……")
        };
        private static readonly DialoguePage[] RepeatPages =
        {
            new DialoguePage("主角", "还是别把这本来就乱的床铺弄得更乱的好。")
        };

        [SerializeField] private WorldItem phone;
        private bool explored;

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.LinBedroomBed && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (explored) return dialogue.StartDialogue(RepeatPages);
            if (!dialogue.StartDialogue(FirstPages, RevealPhone)) return false;
            explored = true;
            return true;
        }

        private void RevealPhone()
        {
            if (phone != null) phone.gameObject.SetActive(true);
        }
    }
}
