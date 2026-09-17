using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(PlayerInventory))]
    public sealed class PlasterKnifeCrafting : MonoBehaviour
    {
        private const string PlasterName = "膏药";
        private const string KnifeName = "菜刀";
        private const string StickyKnifeName = "粘竿菜刀";
        private static readonly DialoguePage[] CraftPages =
        {
            new DialoguePage("主角", "兴许我能把这平时拿来遮挡的膏药贴在钝刀上，做成一个特别奇怪的粘竿。")
        };

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
                || !inventory.HasItem(PlasterName) || !inventory.HasItem(KnifeName)) return;

            crafting = dialogue.StartDialogue(CraftPages, CraftStickyKnife);
        }

        private void CraftStickyKnife()
        {
            if (!inventory.TryRemoveFirst(PlasterName, out WorldItem plaster)
                || !inventory.TryRemoveFirst(KnifeName, out WorldItem knife))
            {
                crafting = false;
                return;
            }

            GameObject itemObject = new GameObject("Sticky Pole Knife");
            itemObject.transform.position = transform.position;
            SpriteRenderer renderer = itemObject.AddComponent<SpriteRenderer>();
            renderer.sprite = knife.Icon != null ? knife.Icon : plaster.Icon;
            renderer.color = new Color(.92f, .78f, .58f, 1f);
            renderer.sortingOrder = 5;
            BoxCollider2D collider = itemObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            WorldItem stickyKnife = itemObject.AddComponent<WorldItem>();
            stickyKnife.Configure(StickyKnifeName, true);
            inventory.TryStore(stickyKnife);
            crafted = true;
            crafting = false;
        }
    }
}
