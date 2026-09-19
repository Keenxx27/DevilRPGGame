using UnityEngine;

namespace RPG
{
    public sealed class RoadHomeJourney : MonoBehaviour
    {
        private static readonly DialoguePage[] QuarterPages =
        {
            new DialoguePage("主角", "一种隐秘的、巨大的喜悦蔓延上我这道贯穿面部的侵蚀痕迹。"),
            new DialoguePage("主角", "几乎要使我笑出来。")
        };

        private static readonly DialoguePage[] HalfPages =
        {
            new DialoguePage("主角", "我知道了。我亲眼看见了。"),
            new DialoguePage("主角", "它们不是怪物，它们可以交流，它们有自己的逻辑和……礼仪。\n例如十三。")
        };

        private static readonly DialoguePage[] ThreeQuarterPages =
        {
            new DialoguePage("主角", "这个世界是错的。而我，找到了第一块真实的拼图。"),
            new DialoguePage("主角", "我要带着十三回去认识认识春宝。")
        };

        [SerializeField] private float startX;
        [SerializeField] private float endX;
        private int nextMilestone;

        public void Configure(float rightStart, float leftEnd)
        {
            startX = rightStart;
            endX = leftEnd;
        }

        private void Update()
        {
            if (nextMilestone >= 3) return;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null || dialogue.IsPlaying) return;
            float progress = Mathf.Clamp01((startX - dialogue.transform.position.x) / (startX - endX));
            float threshold = (nextMilestone + 1) * .25f;
            if (progress < threshold) return;
            DialoguePage[] pages = nextMilestone == 0 ? QuarterPages
                : nextMilestone == 1 ? HalfPages : ThreeQuarterPages;
            if (dialogue.StartDialogue(pages)) nextMilestone++;
        }
    }
}
