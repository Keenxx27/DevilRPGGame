using UnityEngine;

namespace RPG
{
    public sealed class RoadToSuburbsOneEndpointHint : MonoBehaviour
    {
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("主角", "呼……逃过一劫。"),
            new DialoguePage("主角", "但前面的路就不好说了，可能会遇到更加危险的丧家犬成员。")
        };

        private bool hasPlayed;

        private void Update()
        {
            if (hasPlayed) return;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null || dialogue.IsPlaying) return;

            Vector2 position = dialogue.transform.position;
            if (position.x > 233f || position.y < .2f) return;
            hasPlayed = dialogue.StartDialogue(Pages);
        }
    }
}
