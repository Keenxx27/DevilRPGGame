using UnityEngine;

namespace RPG
{
    public sealed class ApartmentLivingRoomIntro : MonoBehaviour
    {
        private static DialoguePage[] CreatePages()
        {
            return new[]
            {
                new DialoguePage("主角", GameLocalization.Get("apartment.livingRoom.intro.1")),
                new DialoguePage("主角", GameLocalization.Get("apartment.livingRoom.intro.2")),
                new DialoguePage("主角", GameLocalization.Get("apartment.livingRoom.intro.3"))
            };
        }

        private bool hasPlayed;

        private void Update()
        {
            if (hasPlayed || ThirteenCompanionState.IsTravelingWithPlayer) return;

            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null) return;

            Vector2 position = dialogue.transform.position;
            if (position.x < 132f || position.x > 148f || position.y < -5f || position.y > 5f)
            {
                return;
            }

            hasPlayed = dialogue.StartDialogue(CreatePages());
        }
    }
}
