using UnityEngine;

namespace RPG
{
    public sealed class ApartmentEntranceRightSideHint : MonoBehaviour
    {
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("主角", "这边不是通往郊区的路，我正后方的路才是。")
        };

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
                isInRightSide = dialogue.StartDialogue(Pages);
            }
        }
    }
}
