using UnityEngine;

namespace RPG
{
    public class PrivateDialogueTrigger : MonoBehaviour
    {
        [SerializeField] private DialoguePage[] pages;

        public DialoguePage[] Pages => pages;

        private void OnTriggerEnter2D(Collider2D other)
        {
            DialogueController controller = other.GetComponentInParent<DialogueController>();
            if (controller != null)
            {
                controller.RegisterPrivateDialogue(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            DialogueController controller = other.GetComponentInParent<DialogueController>();
            if (controller != null)
            {
                controller.UnregisterPrivateDialogue(this);
            }
        }
    }
}
