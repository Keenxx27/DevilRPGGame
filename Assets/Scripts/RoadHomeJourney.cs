using UnityEngine;

namespace RPG
{
    public sealed class RoadHomeJourney : MonoBehaviour
    {
        private static readonly string[] QuarterKeys =
        {
            "roadHome.quarter.1", "roadHome.quarter.2"
        };

        private static readonly string[] HalfKeys =
        {
            "roadHome.half.1", "roadHome.half.2"
        };

        private static readonly string[] ThreeQuarterKeys =
        {
            "roadHome.threeQuarter.1", "roadHome.threeQuarter.2"
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
            string[] keys = nextMilestone == 0 ? QuarterKeys
                : nextMilestone == 1 ? HalfKeys : ThreeQuarterKeys;
            DialoguePage[] pages = CreatePages(keys);
            if (dialogue.StartDialogue(pages)) nextMilestone++;
        }

        private static DialoguePage[] CreatePages(string[] keys)
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
