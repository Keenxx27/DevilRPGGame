using UnityEngine;

namespace RPG
{
    public sealed class ApartmentDoor : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("主角", "我妹妹林春宝这个时候大概已经放学在家里等着我了吧。\n……真不知道这句话是对谁说的……\n好像我们这个家真会来客人一样。"),
            new DialoguePage("主角", "……没有人愿意跟一个半人半恶魔的家伙说话，除了她。\n因为我是她哥哥。")
        };

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
            return CanInvestigate(kind) && dialogueController.StartDialogue(Pages, ConfirmEntry);
        }

        private void ConfirmEntry()
        {
            entryConfirmed = true;
            doorInteraction.enabled = true;
            playerInteraction.RegisterDoor(door);
        }
    }
}
