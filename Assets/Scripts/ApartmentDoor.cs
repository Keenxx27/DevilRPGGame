using UnityEngine;

namespace RPG
{
    public sealed class ApartmentDoor : MonoBehaviour, IInvestigationHandler
    {
        private static DialoguePage[] CreatePages()
        {
            return new[]
            {
                new DialoguePage("主角", GameLocalization.Get("apartment.entrance.door.1")),
                new DialoguePage("主角", GameLocalization.Get("apartment.entrance.door.2"))
            };
        }

        private DialogueController dialogueController;
        [SerializeField] private DoorController door;
        [SerializeField] private BoxCollider2D doorInteraction;
        private PlayerInteraction playerInteraction;
        private bool entryConfirmed;

        private void Start()
        {
            dialogueController = FindObjectOfType<DialogueController>();
            playerInteraction = FindObjectOfType<PlayerInteraction>();
            if (dialogueController == null || door == null || doorInteraction == null
                || playerInteraction == null)
            {
                Debug.LogError("ApartmentDoor 缺少进入公寓所需组件。", this);
                enabled = false;
            }
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            return enabled && !entryConfirmed && kind == InvestigationKind.ApartmentDoor
                && dialogueController != null && !dialogueController.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            return CanInvestigate(kind) && dialogueController.StartDialogue(CreatePages(), ConfirmEntry);
        }

        private void ConfirmEntry()
        {
            entryConfirmed = true;
            doorInteraction.enabled = true;
            playerInteraction.RegisterDoor(door);
        }
    }
}
