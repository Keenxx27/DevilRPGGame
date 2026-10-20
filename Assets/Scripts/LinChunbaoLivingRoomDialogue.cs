using UnityEngine;

namespace RPG
{
    public sealed class LinChunbaoLivingRoomDialogue : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        private const string StickyKnifeId = "apartment.stickyKnife";
        private const string BedsideKeySpriteResourcePath = "bedside-table-key-pixel-v1";
        private static DialoguePage PlayerPage(string key) =>
            new DialoguePage("主角", GameLocalization.Get(key));

        private static DialoguePage LinPage(string key) =>
            new DialoguePage(GameLocalization.Get("apartment.speaker.linChunbao"), GameLocalization.Get(key));

        private bool isAtSofa;
        private bool hasBeenInvestigated;
        private bool hasGivenBedsideKey;
        private bool departureConversationComplete;

        public bool IsAtSofa => isAtSofa;

        public void ActivateAtSofa()
        {
            isAtSofa = true;
        }

        public void MarkDepartureConversationComplete()
        {
            departureConversationComplete = true;
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.LivingRoomLinChunbao
                && isAtSofa && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (LinChunbaoPhone.GameCompleted && !hasGivenBedsideKey)
            {
                return dialogue.StartDialogue(CreatePhoneRewardPages(), GiveBedsideKey);
            }

            if (departureConversationComplete)
            {
                return dialogue.StartDialogue(new[] { LinPage("apartment.livingRoom.lin.departureRepeat") });
            }

            DialoguePage[] pages = { LinPage(hasBeenInvestigated
                ? "apartment.livingRoom.lin.repeat"
                : "apartment.livingRoom.lin.first") };
            if (!dialogue.StartDialogue(pages)) return false;
            hasBeenInvestigated = true;
            return true;
        }

        public bool TryUseSelectedItem(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            SandbagBoxingMinigame boxing = FindObjectOfType<SandbagBoxingMinigame>();
            if (boxing == null || !boxing.IsBroken) return false;

            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory?.SelectedItem == null || inventory.SelectedItem.ItemId != StickyKnifeId)
            {
                return false;
            }

            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (!dialogue.StartDialogue(CreateRepairSandbagPages(), GrantBedroomAccess)) return false;
            inventory.TryRemoveSelectedById(StickyKnifeId, out _);
            return true;
        }

        private static void GrantBedroomAccess()
        {
            FindObjectOfType<LinChunbaoBedroomDoor>()?.GrantAccess();
        }

        private static DialoguePage[] CreatePhoneRewardPages()
        {
            return new[]
            {
                LinPage("apartment.livingRoom.phoneReward.1"), PlayerPage("apartment.livingRoom.phoneReward.2"),
                LinPage("apartment.livingRoom.phoneReward.3"), LinPage("apartment.livingRoom.phoneReward.4"),
                PlayerPage("apartment.livingRoom.phoneReward.5")
            };
        }

        private static DialoguePage[] CreateRepairSandbagPages()
        {
            return new[]
            {
                LinPage("apartment.livingRoom.repair.1"), PlayerPage("apartment.livingRoom.repair.2"),
                LinPage("apartment.livingRoom.repair.3"), PlayerPage("apartment.livingRoom.repair.4"),
                LinPage("apartment.livingRoom.repair.5"), PlayerPage("apartment.livingRoom.repair.6"),
                LinPage("apartment.livingRoom.repair.7")
            };
        }

        private void GiveBedsideKey()
        {
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory == null || inventory.HasItemById("apartment.bedsideKey"))
            {
                hasGivenBedsideKey = true;
                return;
            }

            GameObject keyObject = new GameObject("Bedside Table Key");
            keyObject.transform.position = inventory.transform.position;
            keyObject.transform.localScale = new Vector3(.5f, .3f, 1f);
            SpriteRenderer renderer = keyObject.AddComponent<SpriteRenderer>();
            SpriteRenderer source = GetComponent<SpriteRenderer>();
            Sprite keySprite = Resources.Load<Sprite>(BedsideKeySpriteResourcePath);
            renderer.sprite = keySprite != null ? keySprite : source != null ? source.sprite : null;
            renderer.sharedMaterial = source != null ? source.sharedMaterial : null;
            renderer.color = keySprite != null ? Color.white : new Color(.72f, .56f, .2f, 1f);
            renderer.sortingOrder = 5;
            BoxCollider2D collider = keyObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            WorldItem key = keyObject.AddComponent<WorldItem>();
            key.ConfigureLocalization("apartment.bedsideKey", "apartment.item.bedsideKey");
            hasGivenBedsideKey = inventory.TryStore(key);
            if (!hasGivenBedsideKey)
            {
                key.Drop(inventory.transform.position + Vector3.right * .8f);
            }
        }
    }
}
