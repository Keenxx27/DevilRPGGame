using UnityEngine;

namespace RPG
{
    public sealed class JanitorRestRoomDoor : MonoBehaviour, IInvestigationHandler
    {
        [SerializeField] private WorldItem keyRing;

        private DialogueController dialogueController;
        private PlayerInventory inventory;
        private bool keyRingRevealed;
        private DialoguePage[] discoveryPages;

        private void Awake()
        {
            discoveryPages = new[]
            {
                new DialoguePage("主角", GameLocalization.Get("corridor.janitor.1")),
                new DialoguePage("主角", GameLocalization.Get("corridor.janitor.2")),
                new DialoguePage("主角", GameLocalization.Get("corridor.janitor.3"))
            };
        }

        private void Start()
        {
            if (keyRing != null)
            {
                keyRing.ConfigureLocalization("corridor.janitorKeyRing", "corridor.item.janitorKeyRing");
            }
            dialogueController = FindObjectOfType<DialogueController>();
            inventory = FindObjectOfType<PlayerInventory>();
            if (dialogueController == null || inventory == null || keyRing == null)
            {
                Debug.LogError("JanitorRestRoomDoor 缺少对话、物品栏或钥匙串。", this);
                enabled = false;
            }
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            return enabled && kind == InvestigationKind.JanitorRestRoomDoor
                && !keyRingRevealed && dialogueController != null
                && !dialogueController.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            return dialogueController.StartDialogue(discoveryPages, RevealKeyRing);
        }

        private void RevealKeyRing()
        {
            keyRingRevealed = true;
            keyRing.gameObject.SetActive(true);
            inventory.RegisterNearby(keyRing);
        }
    }
}
