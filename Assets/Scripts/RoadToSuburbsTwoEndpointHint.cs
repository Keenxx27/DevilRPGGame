using UnityEngine;

namespace RPG
{
    public sealed class RoadToSuburbsTwoEndpointHint : MonoBehaviour
    {
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("主角", "丧家犬们只期望能够平安无事地度过每一天。\n他们常睡在一起，挤在后巷的某处安家过夜——\n如同身处老鼠洞一般。")
        };

        private bool hasPlayed;

        private void Update()
        {
            if (hasPlayed) return;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null || dialogue.IsPlaying) return;

            Vector2 position = dialogue.transform.position;
            if (position.x > 262f || position.y > -4.6f) return;
            hasPlayed = dialogue.StartDialogue(Pages);
        }
    }
}
