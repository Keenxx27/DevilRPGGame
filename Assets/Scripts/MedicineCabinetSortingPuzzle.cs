using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class MedicineCabinetSortingPuzzle : MonoBehaviour
    {
        private static readonly Color[] MedicineColors =
        {
            new Color(.78f, .17f, .14f, 1f),
            new Color(.92f, .76f, .12f, 1f),
            new Color(.17f, .42f, .86f, 1f),
            new Color(.18f, .63f, .28f, 1f)
        };

        // 两行均为黄、蓝、绿、红；每一步向左交换可依次将一整行归位。
        private readonly int[] medicines = { 1, 2, 3, 0, 1, 2, 3, 0 };
        private GameObject canvasObject;
        private Image[] medicineImages;
        private Outline[] highlights;
        private MainCharacterMovement movement;
        private PlayerInteraction interaction;
        private PlayerInventory inventory;
        private DialogueController dialogue;
        private bool movementWasEnabled;
        private bool interactionWasEnabled;
        private bool inventoryWasEnabled;
        private bool dialogueWasEnabled;
        private bool isVisible;
        private bool completed;
        private int selectedIndex = -1;
        private int cursorIndex;

        [SerializeField] private WorldItem plaster;

        public bool Show(PlayerInteraction player)
        {
            if (isVisible || completed || player == null) return false;
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
            selectedIndex = -1;
            cursorIndex = 0;
            isVisible = true;
            canvasObject.SetActive(true);
            RefreshDisplay();
            return true;
        }

        private void Update()
        {
            if (!isVisible || Time.timeScale <= 0f) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (selectedIndex >= 0) selectedIndex = -1;
                else if (!IsCorrect(cursorIndex)) selectedIndex = cursorIndex;
                RefreshDisplay();
                return;
            }

            int destination = DestinationForInput(selectedIndex >= 0 ? selectedIndex : cursorIndex);
            if (destination < 0) return;
            if (selectedIndex < 0)
            {
                cursorIndex = destination;
                RefreshDisplay();
                return;
            }
            if (destination < 0 || IsCorrect(destination)) return;

            int selectedMedicine = medicines[selectedIndex];
            medicines[selectedIndex] = medicines[destination];
            medicines[destination] = selectedMedicine;
            cursorIndex = destination;
            selectedIndex = IsCorrect(destination) ? -1 : destination;
            RefreshDisplay();
            if (AllMedicinesCorrect()) Complete();
        }

        private int DestinationForInput(int origin)
        {
            int row = origin / 4;
            int column = origin % 4;
            if (Input.GetKeyDown(KeyCode.W) && row > 0) return origin - 4;
            if (Input.GetKeyDown(KeyCode.S) && row < 1) return origin + 4;
            if (Input.GetKeyDown(KeyCode.A) && column > 0) return origin - 1;
            if (Input.GetKeyDown(KeyCode.D) && column < 3) return origin + 1;
            return -1;
        }

        private bool IsCorrect(int index) => medicines[index] == index % 4;

        private bool AllMedicinesCorrect()
        {
            for (int index = 0; index < medicines.Length; index++)
            {
                if (!IsCorrect(index)) return false;
            }
            return true;
        }

        private void Complete()
        {
            completed = true;
            isVisible = false;
            canvasObject.SetActive(false);
            movement.enabled = movementWasEnabled;
            interaction.enabled = interactionWasEnabled;
            inventory.enabled = inventoryWasEnabled;
            dialogue.enabled = dialogueWasEnabled;
            if (plaster != null) plaster.gameObject.SetActive(true);
        }

        private void EnsureCanvas()
        {
            if (canvasObject != null) return;
            canvasObject = new GameObject("Medicine Cabinet Puzzle Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 45;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());

            GameObject panel = new GameObject("Medicine Cabinet Panel");
            panel.transform.SetParent(canvasObject.transform, false);
            Image panelImage = panel.AddComponent<Image>();
            panelImage.color = new Color(.15f, .12f, .09f, .97f);
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(.5f, .5f);
            panelRect.anchorMax = new Vector2(.5f, .5f);
            panelRect.sizeDelta = new Vector2(840f, 470f);

            CreateText("Title", panel.transform, "药柜", 34, new Vector2(0f, 190f), Color.white);
            CreateText("Hint", panel.transform, "E 选中药品    WASD 与相邻药品交换    归位的药品无法移动", 18,
                new Vector2(0f, -192f), new Color(.92f, .92f, .92f, 1f));

            medicineImages = new Image[8];
            highlights = new Outline[8];
            for (int column = 0; column < 4; column++)
            {
                GameObject compartment = new GameObject("Compartment " + column);
                compartment.transform.SetParent(panel.transform, false);
                Image compartmentImage = compartment.AddComponent<Image>();
                compartmentImage.color = new Color(.37f, .21f, .11f, 1f);
                RectTransform compartmentRect = compartment.GetComponent<RectTransform>();
                compartmentRect.anchorMin = new Vector2(.5f, .5f);
                compartmentRect.anchorMax = new Vector2(.5f, .5f);
                compartmentRect.sizeDelta = new Vector2(165f, 285f);
                compartmentRect.anchoredPosition = new Vector2(-285f + column * 190f, 10f);

                GameObject marker = new GameObject("Color Marker");
                marker.transform.SetParent(compartment.transform, false);
                Image markerImage = marker.AddComponent<Image>();
                markerImage.color = MedicineColors[column];
                RectTransform markerRect = marker.GetComponent<RectTransform>();
                markerRect.anchorMin = new Vector2(.5f, 0f);
                markerRect.anchorMax = new Vector2(.5f, 0f);
                markerRect.sizeDelta = new Vector2(28f, 28f);
                markerRect.anchoredPosition = new Vector2(0f, 24f);

                for (int row = 0; row < 2; row++)
                {
                    int index = row * 4 + column;
                    GameObject medicine = new GameObject("Medicine " + index);
                    medicine.transform.SetParent(compartment.transform, false);
                    Image medicineImage = medicine.AddComponent<Image>();
                    medicineImages[index] = medicineImage;
                    Outline outline = medicine.AddComponent<Outline>();
                    outline.effectColor = Color.white;
                    outline.effectDistance = new Vector2(3f, -3f);
                    highlights[index] = outline;
                    RectTransform medicineRect = medicine.GetComponent<RectTransform>();
                    medicineRect.anchorMin = new Vector2(.5f, .5f);
                    medicineRect.anchorMax = new Vector2(.5f, .5f);
                    medicineRect.sizeDelta = new Vector2(112f, 74f);
                    medicineRect.anchoredPosition = new Vector2(0f, row == 0 ? 62f : -32f);
                }
            }
            canvasObject.SetActive(false);
        }

        private void RefreshDisplay()
        {
            for (int index = 0; index < medicines.Length; index++)
            {
                medicineImages[index].color = MedicineColors[medicines[index]];
                highlights[index].enabled = index == selectedIndex || index == cursorIndex;
                highlights[index].effectColor = index == selectedIndex
                    ? Color.white
                    : new Color(1f, .82f, .15f, .75f);
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
            rect.sizeDelta = new Vector2(780f, 58f);
            rect.anchoredPosition = position;
            return text;
        }
    }
}
