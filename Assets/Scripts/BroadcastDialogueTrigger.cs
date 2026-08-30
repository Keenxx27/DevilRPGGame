using UnityEngine;

namespace RPG
{
    public class BroadcastDialogueTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueController controller;
        [SerializeField] private DialoguePage[] pages;

        public bool HasTriggered { get; private set; }
        public event System.Action Completed;

        private void Start()
        {
            Trigger();
        }

        public bool Trigger()
        {
            if (HasTriggered)
            {
                return false;
            }

            if (controller == null || pages == null || pages.Length == 0)
            {
                Debug.LogError(
                    "BroadcastDialogueTrigger requires a controller and dialogue pages.", this);
                return false;
            }

            if (!controller.StartDialogue(pages, NotifyCompleted))
            {
                return false;
            }

            HasTriggered = true;
            return true;
        }

        private void NotifyCompleted()
        {
            Completed?.Invoke();
        }
    }
}
