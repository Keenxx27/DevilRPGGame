using UnityEngine;

namespace RPG
{
    public sealed class RoadToSuburbsTwoIntro : MonoBehaviour
    {
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("主角", "更加复杂的道路。"),
            new DialoguePage("主角", "话说回来……自言自语的现象越来越严重了。")
        };

        private bool hasPlayed;

        private void Update()
        {
            if (hasPlayed) return;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null || dialogue.IsPlaying) return;

            Vector2 position = dialogue.transform.position;
            if (position.x < 275f || position.y < .2f) return;
            hasPlayed = dialogue.StartDialogue(Pages);
        }
    }
}
