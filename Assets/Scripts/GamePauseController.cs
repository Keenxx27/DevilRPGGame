using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG
{
    public sealed class GamePauseController : MonoBehaviour
    {
        private const string InstructionsSceneName = "GameInstructions";
        private GameObject pauseCanvas;
        private MainCharacterMovement movement;
        private PlayerInteraction interaction;
        private PlayerInventory inventory;
        private DialogueController dialogue;
        private bool movementWasEnabled;
        private bool interactionWasEnabled;
        private bool inventoryWasEnabled;
        private bool dialogueWasEnabled;
        private bool isPaused;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            CreatePausePage();
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == InstructionsSceneName) return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetPaused(!isPaused);
            }
        }

        private void SetPaused(bool paused)
        {
            isPaused = paused;
            if (paused)
            {
                CachePlayerComponents();
                Time.timeScale = 0f;
                pauseCanvas.SetActive(true);
                return;
            }

            Time.timeScale = 1f;
            pauseCanvas.SetActive(false);
            RestorePlayerComponents();
        }

        private void CachePlayerComponents()
        {
            movement = FindObjectOfType<MainCharacterMovement>();
            interaction = FindObjectOfType<PlayerInteraction>();
            inventory = FindObjectOfType<PlayerInventory>();
            dialogue = FindObjectOfType<DialogueController>();
            if (movement != null) { movementWasEnabled = movement.enabled; movement.enabled = false; }
            if (interaction != null) { interactionWasEnabled = interaction.enabled; interaction.enabled = false; }
            if (inventory != null) { inventoryWasEnabled = inventory.enabled; inventory.enabled = false; }
            if (dialogue != null) { dialogueWasEnabled = dialogue.enabled; dialogue.enabled = false; }
        }

        private void RestorePlayerComponents()
        {
            if (movement != null) movement.enabled = movementWasEnabled;
            if (interaction != null) interaction.enabled = interactionWasEnabled;
            if (inventory != null) inventory.enabled = inventoryWasEnabled;
            if (dialogue != null) dialogue.enabled = dialogueWasEnabled;
        }

        private void CreatePausePage()
        {
            pauseCanvas = new GameObject("Pause Canvas");
            DontDestroyOnLoad(pauseCanvas);
            Canvas canvas = pauseCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 60;
            UiScaleSettings.Configure(pauseCanvas.AddComponent<CanvasScaler>());

            GameObject background = new GameObject("Pause Background");
            background.transform.SetParent(pauseCanvas.transform, false);
            Image image = background.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, .8f);
            RectTransform backgroundRect = image.rectTransform;
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;

            GameObject textObject = new GameObject("Pause Text");
            textObject.transform.SetParent(pauseCanvas.transform, false);
            Text text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = "游戏已暂停\n\n按 Esc 继续";
            text.fontSize = 40;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            RectTransform textRect = text.rectTransform;
            textRect.anchorMin = new Vector2(.5f, .5f);
            textRect.anchorMax = new Vector2(.5f, .5f);
            textRect.sizeDelta = new Vector2(640f, 240f);
            pauseCanvas.SetActive(false);
        }
    }
}
