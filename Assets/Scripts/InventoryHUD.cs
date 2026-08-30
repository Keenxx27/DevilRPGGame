using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class InventoryHUD : MonoBehaviour
    {
        private const int SlotCount = 3;

        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private Image[] selectionBorders;
        [SerializeField] private Image[] itemIcons;

        private bool configurationErrorReported;

        private void Start()
        {
            Refresh();
        }

        private void LateUpdate()
        {
            Refresh();
        }

        public bool Refresh()
        {
            if (!HasValidConfiguration())
            {
                if (!configurationErrorReported)
                {
                    Debug.LogError(
                        "InventoryHUD requires one inventory, three borders, and three icons.", this);
                    configurationErrorReported = true;
                }

                return false;
            }

            for (int index = 0; index < SlotCount; index++)
            {
                selectionBorders[index].enabled = inventory.SelectedSlotIndex == index;
                WorldItem item = inventory.GetSlotItem(index);
                itemIcons[index].enabled = item != null && item.Icon != null;
                if (itemIcons[index].enabled)
                {
                    itemIcons[index].sprite = item.Icon;
                    itemIcons[index].color = item.IconColor;
                }
            }

            return true;
        }

        private bool HasValidConfiguration()
        {
            if (inventory == null || selectionBorders == null || itemIcons == null
                || selectionBorders.Length != SlotCount || itemIcons.Length != SlotCount)
            {
                return false;
            }

            for (int index = 0; index < SlotCount; index++)
            {
                if (selectionBorders[index] == null || itemIcons[index] == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
