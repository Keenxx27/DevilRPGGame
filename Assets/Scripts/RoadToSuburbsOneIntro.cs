using UnityEngine;

namespace RPG
{
    public sealed class RoadToSuburbsOneIntro : MonoBehaviour
    {
        private bool hasPlayed;

        private void Update()
        {
            if (hasPlayed) return;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null || dialogue.IsPlaying) return;

            Vector2 position = dialogue.transform.position;
            if (position.x < 246f || position.y > -3.2f) return;
            hasPlayed = dialogue.StartDialogue(new[]
            {
                new DialoguePage("主角", GameLocalization.Get("suburbs.road1.intro.1")),
                new DialoguePage("主角", GameLocalization.Get("suburbs.road1.intro.2")),
                new DialoguePage("主角", GameLocalization.Get("suburbs.road1.intro.3")),
                new DialoguePage("主角", GameLocalization.Get("suburbs.road1.intro.4"))
            });
        }
    }
}
