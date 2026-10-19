using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(PlayerInventory))]
    public sealed class PlasterKnifeCrafting : MonoBehaviour
    {
        private const string PlasterId = "apartment.plaster";
        private const string KnifeId = "apartment.knife";
        private const string StickyKnifeId = "apartment.stickyKnife";
        private const string StickyKnifeSpriteResourcePath = "sticky-pole-knife-pixel-v1";
        private PlayerInventory inventory;
        private DialogueController dialogue;
        private bool crafting;
        private bool crafted;

        private void Awake()
        {
            inventory = GetComponent<PlayerInventory>();
            dialogue = GetComponent<DialogueController>();
        }

        private void Update()
        {
            if (crafted || crafting || inventory == null || dialogue == null || dialogue.IsPlaying
                || !inventory.HasItemById(PlasterId) || !inventory.HasItemById(KnifeId)) return;

            crafting = dialogue.StartDialogue(new[]
            {
                new DialoguePage("主角", GameLocalization.Get("apartment.crafting.stickyKnife"))
            }, CraftStickyKnife);
        }

        private void CraftStickyKnife()
        {
            if (!inventory.TryRemoveFirstById(PlasterId, out WorldItem plaster)
                || !inventory.TryRemoveFirstById(KnifeId, out WorldItem knife))
            {
                crafting = false;
                return;
            }

            GameObject itemObject = new GameObject("Sticky Pole Knife");
            itemObject.transform.position = transform.position;
            SpriteRenderer renderer = itemObject.AddComponent<SpriteRenderer>();
            Sprite stickyKnifeSprite = Resources.Load<Sprite>(StickyKnifeSpriteResourcePath);
            renderer.sprite = stickyKnifeSprite != null ? stickyKnifeSprite : knife.Icon != null ? knife.Icon : plaster.Icon;
            renderer.color = stickyKnifeSprite != null ? Color.white : new Color(.92f, .78f, .58f, 1f);
            renderer.sortingOrder = 5;
            BoxCollider2D collider = itemObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            WorldItem stickyKnife = itemObject.AddComponent<WorldItem>();
            stickyKnife.ConfigureLocalization(StickyKnifeId, "apartment.item.stickyKnife");
            inventory.TryStore(stickyKnife);
            crafted = true;
            crafting = false;
        }
    }
}
