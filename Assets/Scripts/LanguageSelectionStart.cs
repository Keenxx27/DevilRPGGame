using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG
{
    public static class GameStartShortcut
    {
        private const string ClassroomSceneName = "1_SchoolClassroom";

        public static bool TrySkipToClassroom()
        {
            if (!Input.GetKeyDown(KeyCode.O)) return false;

            Time.timeScale = 1f;
            GameLocalization.SetLanguage(GameLanguage.English);
            PlayerChosenName.Set(null);

            if (Object.FindObjectOfType<GamePauseController>() == null)
            {
                new GameObject("Game Pause Controller").AddComponent<GamePauseController>();
            }

            UiAudio.PlayConfirm();
            SceneManager.LoadScene(ClassroomSceneName, LoadSceneMode.Single);
            return true;
        }
    }

    public sealed class LanguageSelectionStart : MonoBehaviour
    {
        private const string NameEntrySceneName = "NameEntry";

        private readonly string[] languageNames = { "简体中文", "English", "日本語" };
        private Text title;
        private Text hint;
        private Text[] options;
        private int selectedIndex;

        private void Awake()
        {
            Time.timeScale = 1f;
            selectedIndex = (int)GameLocalization.CurrentLanguage;
            CreatePage();
            RefreshPage();
        }

        private void Update()
        {
            if (GameStartShortcut.TrySkipToClassroom()) return;

            if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeSelection(-1);
            if (Input.GetKeyDown(KeyCode.RightArrow)) ChangeSelection(1);
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) Confirm();
        }

        private void ChangeSelection(int change)
        {
            selectedIndex = (selectedIndex + change + languageNames.Length) % languageNames.Length;
            RefreshPage();
        }

        private void Confirm()
        {
            GameLocalization.SetLanguage((GameLanguage)selectedIndex);
            UiAudio.PlayConfirm();
            SceneManager.LoadScene(NameEntrySceneName, LoadSceneMode.Single);
        }

        private void CreatePage()
        {
            GameObject canvasObject = new GameObject("Language Selection Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 50;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());

            Image background = new GameObject("Black Background").AddComponent<Image>();
            background.transform.SetParent(canvasObject.transform, false);
            background.color = Color.black;
            Stretch(background.rectTransform);

            title = CreateText("Title", canvasObject.transform, 38, new Vector2(0f, 150f));
            hint = CreateText("Selection Hint", canvasObject.transform, 22, new Vector2(0f, -170f));
            options = new Text[languageNames.Length];
            for (int index = 0; index < languageNames.Length; index++)
            {
                options[index] = CreateText("Option " + index, canvasObject.transform, 30,
                    new Vector2((index - 1) * 260f, 35f));
                options[index].text = languageNames[index];
            }
        }

        private void RefreshPage()
        {
            GameLanguage selectedLanguage = (GameLanguage)selectedIndex;
            title.text = GameLocalization.Get("language.title", selectedLanguage);
            hint.text = GameLocalization.Get("language.hint", selectedLanguage);
            for (int index = 0; index < options.Length; index++)
            {
                options[index].color = index == selectedIndex ? new Color(1f, .86f, .3f, 1f) : Color.white;
            }
        }

        private static Text CreateText(string name, Transform parent, int fontSize, Vector2 position)
        {
            GameObject textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            Text text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            RectTransform rect = text.rectTransform;
            rect.anchorMin = new Vector2(.5f, .5f);
            rect.anchorMax = new Vector2(.5f, .5f);
            rect.pivot = new Vector2(.5f, .5f);
            rect.sizeDelta = new Vector2(420f, 100f);
            rect.anchoredPosition = position;
            return text;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}
