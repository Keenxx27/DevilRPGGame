using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class InventoryHUD : MonoBehaviour
    {
        private const int SlotCount = 5;
        private const float SlotSize = 84f;
        private const float SlotSpacing = 8f;
        private const float PanelPadding = 10f;
        private const float IconInset = 4f;

        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private Image[] selectionBorders;
        [SerializeField] private Image[] itemIcons;

        private bool configurationErrorReported;

        private void Awake()
        {
            BuildVerticalLayout();
        }

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
                        "InventoryHUD requires one inventory, five borders, and five icons.", this);
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

        private void BuildVerticalLayout()
        {
            Transform panelTransform = transform.Find("Inventory Panel");
            if (panelTransform == null) return;

            RectTransform panel = panelTransform as RectTransform;
            panel.anchorMin = Vector2.one;
            panel.anchorMax = Vector2.one;
            panel.pivot = Vector2.one;
            panel.anchoredPosition = new Vector2(-24f, -24f);
            panel.sizeDelta = new Vector2(
                SlotSize + PanelPadding * 2f,
                SlotSize * SlotCount + SlotSpacing * (SlotCount - 1) + PanelPadding * 2f);

            Transform template = panelTransform.Find("Slot 1");
            if (template == null) return;

            var borders = new Image[SlotCount];
            var icons = new Image[SlotCount];
            for (int index = 0; index < SlotCount; index++)
            {
                int number = index + 1;
                Transform slot = panelTransform.Find("Slot " + number);
                if (slot == null)
                {
                    slot = Instantiate(template.gameObject, panelTransform, false).transform;
                    RenameSlotChildren(slot, number);
                }

                RectTransform slotRect = slot as RectTransform;
                slotRect.anchorMin = new Vector2(0f, 1f);
                slotRect.anchorMax = new Vector2(0f, 1f);
                slotRect.pivot = new Vector2(0f, 1f);
                slotRect.anchoredPosition = new Vector2(
                    PanelPadding, -PanelPadding - index * (SlotSize + SlotSpacing));
                slotRect.sizeDelta = new Vector2(SlotSize, SlotSize);

                borders[index] = slot.Find("Selection Border " + number)?.GetComponent<Image>();
                icons[index] = slot.Find("Item Icon " + number)?.GetComponent<Image>();
                if (icons[index] != null)
                {
                    RectTransform iconRect = icons[index].rectTransform;
                    iconRect.anchorMin = Vector2.zero;
                    iconRect.anchorMax = Vector2.one;
                    iconRect.anchoredPosition = Vector2.zero;
                    iconRect.sizeDelta = new Vector2(-IconInset * 2f, -IconInset * 2f);
                    icons[index].preserveAspect = true;
                }

                Text numberLabel = slot.Find("Number Label " + number)?.GetComponent<Text>();
                if (numberLabel != null)
                {
                    numberLabel.text = number.ToString();
                    numberLabel.fontSize = 18;
                }
            }

            selectionBorders = borders;
            itemIcons = icons;
        }

        private static void RenameSlotChildren(Transform slot, int number)
        {
            slot.name = "Slot " + number;
            foreach (Transform child in slot)
            {
                int separator = child.name.LastIndexOf(' ');
                if (separator >= 0)
                {
                    child.name = child.name.Substring(0, separator + 1) + number;
                }
            }
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
