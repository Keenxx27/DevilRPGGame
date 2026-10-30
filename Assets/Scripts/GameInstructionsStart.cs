using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG
{
    public sealed class GameInstructionsStart : MonoBehaviour
    {
        private const string ClassroomSceneName = "1_SchoolClassroom";
        private const string InstructionsSceneName = "GameInstructions";
        private bool acceptsInput;

        private void Awake()
        {
            Time.timeScale = 1f;
            if (FindObjectOfType<GamePauseController>() == null)
            {
                new GameObject("Game Pause Controller").AddComponent<GamePauseController>();
            }

            CreatePage();
        }

        private IEnumerator Start()
        {
            yield return null;
            acceptsInput = true;
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == InstructionsSceneName
                && acceptsInput && Input.GetKeyDown(KeyCode.E))
            {
                acceptsInput = false;
                SceneManager.LoadScene(ClassroomSceneName, LoadSceneMode.Single);
            }
        }

        private static void CreatePage()
        {
            GameObject canvasObject = new GameObject("Instructions Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 50;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());

            GameObject background = new GameObject("Black Background");
            background.transform.SetParent(canvasObject.transform, false);
            Image image = background.AddComponent<Image>();
            image.color = Color.black;
            RectTransform backgroundRect = image.rectTransform;
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;

            CreateText("Title", canvasObject.transform, GameLocalization.Get("tutorial.title"),
                46, new Vector2(0f, 195f), TextAnchor.MiddleCenter);
            CreateText("Instructions", canvasObject.transform,
                GameLocalization.Get("tutorial.instructions"),
                30, new Vector2(0f, -30f), TextAnchor.MiddleCenter);
            CreateText("Start Hint", canvasObject.transform, GameLocalization.Get("tutorial.startHint"),
                24, new Vector2(0f, -350f), TextAnchor.MiddleCenter);
        }

        private static void CreateText(string name, Transform parent, string content,
            int fontSize, Vector2 position, TextAnchor alignment)
        {
            GameObject textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            Text text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = content;
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = alignment;
            RectTransform rect = text.rectTransform;
            rect.anchorMin = new Vector2(.5f, .5f);
            rect.anchorMax = new Vector2(.5f, .5f);
            rect.sizeDelta = new Vector2(860f, 520f);
            rect.anchoredPosition = position;
        }
    }
}
