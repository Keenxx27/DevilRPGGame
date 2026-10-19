using UnityEngine;

namespace RPG
{
    public sealed class LockedGloveCabinet : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        private const string KnifeId = "apartment.knife";
        [SerializeField] private WorldItem boxingGloves;
        [SerializeField] private SpriteRenderer cabinetArtworkRenderer;
        [SerializeField] private Sprite openArtwork;
        private bool introduced;
        private bool opened;

        private void Awake()
        {
            boxingGloves?.ConfigureLocalization("apartment.boxingGloves", "apartment.item.boxingGloves");
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.LivingRoomLowCabinet && !opened
                && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            DialoguePage[] pages = introduced
                ? new[] { PlayerPage("apartment.livingRoom.gloveCabinet.repeat") }
                : new[]
                {
                    LinPage("apartment.livingRoom.gloveCabinet.intro.1"),
                    PlayerPage("apartment.livingRoom.gloveCabinet.intro.2"),
                    LinPage("apartment.livingRoom.gloveCabinet.intro.3")
                };
            if (!FindObjectOfType<DialogueController>().StartDialogue(pages)) return false;
            introduced = true;
            return true;
        }

        public bool TryUseSelectedItem(InvestigationKind kind)
        {
            if (kind != InvestigationKind.LivingRoomLowCabinet || !introduced || opened) return false;
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory?.SelectedItem == null || inventory.SelectedItem.ItemId != KnifeId) return false;

            DialogueController dialogue = FindObjectOfType<DialogueController>();
            DialoguePage[] pages =
            {
                PlayerPage("apartment.livingRoom.gloveCabinet.open.1"),
                PlayerPage("apartment.livingRoom.gloveCabinet.open.2")
            };
            if (dialogue == null || dialogue.IsPlaying || !dialogue.StartDialogue(pages, OpenCabinet))
            {
                return false;
            }
            opened = true;
            return true;
        }

        private void OpenCabinet()
        {
            if (boxingGloves != null) boxingGloves.gameObject.SetActive(true);
            if (cabinetArtworkRenderer != null && openArtwork != null)
            {
                cabinetArtworkRenderer.sprite = openArtwork;
            }
        }

        private static DialoguePage PlayerPage(string key) =>
            new DialoguePage("主角", GameLocalization.Get(key));

        private static DialoguePage LinPage(string key) =>
            new DialoguePage(GameLocalization.Get("apartment.speaker.linChunbao"), GameLocalization.Get(key));
    }
}
