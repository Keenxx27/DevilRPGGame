using UnityEngine;

namespace RPG
{
    public sealed class ApartmentEntranceRightSideHint : MonoBehaviour
    {
        private static DialoguePage[] CreatePages()
        {
            return new[]
            {
                new DialoguePage("主角", GameLocalization.Get("apartment.entrance.wrongWay"))
            };
        }

        private bool isInRightSide;

        private void Update()
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null) return;
            if (dialogue.transform.position.x < 128.5f)
            {
                isInRightSide = false;
                return;
            }

            if (!isInRightSide && !dialogue.IsPlaying)
            {
                isInRightSide = dialogue.StartDialogue(CreatePages());
            }
        }
    }
}
