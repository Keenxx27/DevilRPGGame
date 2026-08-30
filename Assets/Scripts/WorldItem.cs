using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class WorldItem : MonoBehaviour
    {
        [SerializeField] private string displayName = "Item";
        [SerializeField] private bool pickupEnabled = true;

        private SpriteRenderer spriteRenderer;

        public string DisplayName => displayName;
        public Sprite Icon => spriteRenderer.sprite;
        public Color IconColor => spriteRenderer.color;
        public bool CanPickUp => pickupEnabled;
        public event System.Action<WorldItem> PickedUp;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.RegisterNearby(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.UnregisterNearby(this);
            }
        }

        public void PickUp()
        {
            if (!CanPickUp)
            {
                return;
            }

            gameObject.SetActive(false);
            PickedUp?.Invoke(this);
        }

        public void Drop(Vector2 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
        }

        public void SetPickupEnabled(bool enabled)
        {
            pickupEnabled = enabled;
        }
    }
}
