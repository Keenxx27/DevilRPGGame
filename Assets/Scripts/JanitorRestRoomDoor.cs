using UnityEngine;

namespace RPG
{
    public sealed class JanitorRestRoomDoor : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] DiscoveryPages =
        {
            new DialoguePage("主角", "这是校工的休息室，\n还是别进去打扰他们了。"),
            new DialoguePage("主角", "……门上挂着一串钥匙。\n校工休息室、各间教室，甚至储藏室的都有。"),
            new DialoguePage("主角", "也许派得上用场。")
        };

        [SerializeField] private WorldItem keyRing;

        private DialogueController dialogueController;
        private PlayerInventory inventory;
        private bool keyRingRevealed;

        private void Start()
        {
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
            return dialogueController.StartDialogue(DiscoveryPages, RevealKeyRing);
        }

        private void RevealKeyRing()
        {
            keyRingRevealed = true;
            keyRing.gameObject.SetActive(true);
            inventory.RegisterNearby(keyRing);
        }
    }
}
