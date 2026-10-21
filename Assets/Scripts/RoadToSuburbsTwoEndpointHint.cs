using UnityEngine;

namespace RPG
{
    public sealed class RoadToSuburbsTwoEndpointHint : MonoBehaviour
    {
        private bool hasPlayed;

        private void Update()
        {
            if (hasPlayed) return;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null || dialogue.IsPlaying) return;

            Vector2 position = dialogue.transform.position;
            if (position.x > 262f || position.y > -4.6f) return;
            hasPlayed = dialogue.StartDialogue(new[]
            {
                new DialoguePage("主角", GameLocalization.Get("suburbs.road2.endpoint"))
            });
        }
    }
}
