using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class FoodCabinetPasswordLock : MonoBehaviour
    {
        private readonly int[] digits = new int[4];
        private GameObject canvasObject;
        private Text[] digitTexts;
        private Image[] highlights;
        private MainCharacterMovement movement;
        private PlayerInteraction interaction;
        private PlayerInventory inventory;
        private DialogueController dialogue;
        private bool movementWasEnabled;
        private bool interactionWasEnabled;
        private bool inventoryWasEnabled;
        private bool dialogueWasEnabled;
        private bool isVisible;
        private int selectedIndex;
        private LockedFoodCabinet cabinet;

        private void Awake()
        {
            cabinet = GetComponent<LockedFoodCabinet>();
        }

        public bool Show(PlayerInteraction player)
        {
            if (isVisible || player == null || cabinet == null) return false;
            movement = player.GetComponent<MainCharacterMovement>();
            interaction = player;
            inventory = player.GetComponent<PlayerInventory>();
            dialogue = player.GetComponent<DialogueController>();
            if (movement == null || inventory == null || dialogue == null || dialogue.IsPlaying) return false;

            movementWasEnabled = movement.enabled;
            interactionWasEnabled = interaction.enabled;
            inventoryWasEnabled = inventory.enabled;
            dialogueWasEnabled = dialogue.enabled;
            movement.enabled = false;
            interaction.enabled = false;
            inventory.enabled = false;
            dialogue.enabled = false;
            EnsureCanvas();
            selectedIndex = 0;
            isVisible = true;
            canvasObject.SetActive(true);
            RefreshDisplay();
            return true;
        }

        private void Update()
        {
            if (!isVisible || Time.timeScale <= 0f) return;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Close(false);
                return;
            }
            if (Input.GetKeyDown(KeyCode.A)) selectedIndex = (selectedIndex + 3) % 4;
            else if (Input.GetKeyDown(KeyCode.D)) selectedIndex = (selectedIndex + 1) % 4;
            else if (Input.GetKeyDown(KeyCode.W)) digits[selectedIndex] = (digits[selectedIndex] + 1) % 10;
            else if (Input.GetKeyDown(KeyCode.S)) digits[selectedIndex] = (digits[selectedIndex] + 9) % 10;
            else return;

            RefreshDisplay();
            if (digits[0] == 6 && digits[1] == 8 && digits[2] == 1 && digits[3] == 7)
            {
                Close(true);
            }
        }

        private void Close(bool unlocked)
        {
            isVisible = false;
            canvasObject.SetActive(false);
            movement.enabled = movementWasEnabled;
            interaction.enabled = interactionWasEnabled;
            inventory.enabled = inventoryWasEnabled;
            dialogue.enabled = dialogueWasEnabled;
            if (unlocked) cabinet.OpenCabinet();
        }

        private void EnsureCanvas()
        {
            if (canvasObject != null) return;
            canvasObject = new GameObject("Food Cabinet Password Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 45;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());

            GameObject panel = new GameObject("Password Panel");
            panel.transform.SetParent(canvasObject.transform, false);
            Image panelImage = panel.AddComponent<Image>();
            panelImage.color = new Color(.78f, .78f, .78f, .98f);
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(.5f, .5f);
            panelRect.anchorMax = new Vector2(.5f, .5f);
            panelRect.sizeDelta = new Vector2(560f, 300f);

            CreateText("Title", panel.transform, "食品柜密码锁", 31, new Vector2(0f, 95f), Color.black);
            CreateText("Hint", panel.transform, "A / D 选择密码位    W / S 调整数值\nQ 退出", 19,
                new Vector2(0f, -100f), new Color(.12f, .12f, .12f, 1f));

            digitTexts = new Text[4];
            highlights = new Image[4];
            for (int index = 0; index < 4; index++)
            {
                GameObject slot = new GameObject("Digit Slot " + index);
                slot.transform.SetParent(panel.transform, false);
                Image highlight = slot.AddComponent<Image>();
                highlights[index] = highlight;
                RectTransform slotRect = slot.GetComponent<RectTransform>();
                slotRect.anchorMin = new Vector2(.5f, .5f);
                slotRect.anchorMax = new Vector2(.5f, .5f);
                slotRect.sizeDelta = new Vector2(76f, 82f);
                slotRect.anchoredPosition = new Vector2(-132f + index * 88f, 0f);
                digitTexts[index] = CreateText("Digit", slot.transform, "0", 48, Vector2.zero, Color.black);
            }
            canvasObject.SetActive(false);
        }

        private void RefreshDisplay()
        {
            for (int index = 0; index < digits.Length; index++)
            {
                digitTexts[index].text = digits[index].ToString();
                highlights[index].color = index == selectedIndex
                    ? new Color(1f, .82f, .08f, 1f)
                    : new Color(.93f, .93f, .93f, 1f);
            }
        }

        private static Text CreateText(string name, Transform parent, string content, int size,
            Vector2 position, Color color)
        {
            GameObject textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            Text text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = content;
            text.fontSize = size;
            text.color = color;
            text.alignment = TextAnchor.MiddleCenter;
            RectTransform rect = text.rectTransform;
            rect.anchorMin = new Vector2(.5f, .5f);
            rect.anchorMax = new Vector2(.5f, .5f);
            rect.sizeDelta = new Vector2(440f, 62f);
            rect.anchoredPosition = position;
            return text;
        }
    }
}
