using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG
{
    public sealed class NameEntryStart : MonoBehaviour
    {
        private const string InstructionsSceneName = "GameInstructions";
        private const float FeedbackDuration = 1.35f;

        private InputField nameInput;
        private Text prompt;
        private Text feedback;
        private bool awaitingFeedback;

        private void Awake()
        {
            Time.timeScale = 1f;
            CreatePage();
        }

        private IEnumerator Start()
        {
            yield return null;
            nameInput.ActivateInputField();
            nameInput.Select();
        }

        private void Update()
        {
            if (awaitingFeedback) return;
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                ConfirmName();
            }
        }

        private void ConfirmName()
        {
            UiAudio.PlayConfirm();
            string submittedName = nameInput.text.Trim();
            string normalized = Normalize(submittedName);
            NameResult result = ClassifyName(normalized);
            if (result == NameResult.Normal) PlayerChosenName.Set(submittedName);
            else if (result == NameResult.TrueName) PlayerChosenName.Set("林健太");

            awaitingFeedback = true;
            prompt.gameObject.SetActive(false);
            nameInput.gameObject.SetActive(false);
            feedback.gameObject.SetActive(true);
            feedback.text = GetFeedback(result, submittedName);
            StartCoroutine(ContinueAfterFeedback(result));
        }

        private IEnumerator ContinueAfterFeedback(NameResult result)
        {
            yield return new WaitForSecondsRealtime(FeedbackDuration);
            if (result == NameResult.Normal || result == NameResult.TrueName)
            {
                SceneManager.LoadScene(InstructionsSceneName, LoadSceneMode.Single);
                yield break;
            }

            feedback.gameObject.SetActive(false);
            prompt.gameObject.SetActive(true);
            nameInput.text = string.Empty;
            nameInput.gameObject.SetActive(true);
            awaitingFeedback = false;
            nameInput.ActivateInputField();
            nameInput.Select();
        }

        private void CreatePage()
        {
            if (FindObjectOfType<EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("Event System");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }

            GameObject canvasObject = new GameObject("Name Entry Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 50;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());

            GameObject background = new GameObject("Black Background");
            background.transform.SetParent(canvasObject.transform, false);
            Image backgroundImage = background.AddComponent<Image>();
            backgroundImage.color = Color.black;
            Stretch(backgroundImage.rectTransform);

            prompt = CreateText("Prompt", canvasObject.transform,
                GameLocalization.Get("nameEntry.prompt"), 38, new Vector2(0f, 150f), Color.white);
            feedback = CreateText("Feedback", canvasObject.transform, string.Empty, 38,
                new Vector2(0f, 35f), Color.white);
            feedback.gameObject.SetActive(false);

            GameObject inputObject = new GameObject("Name Input");
            inputObject.transform.SetParent(canvasObject.transform, false);
            Image inputBackground = inputObject.AddComponent<Image>();
            inputBackground.color = new Color(1f, 1f, 1f, .12f);
            nameInput = inputObject.AddComponent<InputField>();
            nameInput.lineType = InputField.LineType.SingleLine;
            nameInput.characterLimit = 24;
            RectTransform inputRect = inputObject.GetComponent<RectTransform>();
            inputRect.anchorMin = new Vector2(.5f, .5f);
            inputRect.anchorMax = new Vector2(.5f, .5f);
            inputRect.pivot = new Vector2(.5f, .5f);
            inputRect.sizeDelta = new Vector2(620f, 68f);
            inputRect.anchoredPosition = new Vector2(0f, 35f);

            Text inputText = CreateText("Text", inputObject.transform, string.Empty, 32,
                Vector2.zero, Color.white);
            inputText.alignment = TextAnchor.MiddleLeft;
            RectTransform textRect = inputText.rectTransform;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(18f, 0f);
            textRect.offsetMax = new Vector2(-18f, 0f);
            nameInput.textComponent = inputText;

            Text placeholder = CreateText("Placeholder", inputObject.transform,
                GameLocalization.Get("nameEntry.placeholder"), 26, Vector2.zero, new Color(.7f, .7f, .7f, 1f));
            placeholder.alignment = TextAnchor.MiddleLeft;
            RectTransform placeholderRect = placeholder.rectTransform;
            placeholderRect.anchorMin = Vector2.zero;
            placeholderRect.anchorMax = Vector2.one;
            placeholderRect.offsetMin = new Vector2(18f, 0f);
            placeholderRect.offsetMax = new Vector2(-18f, 0f);
            nameInput.placeholder = placeholder;

            CreateText("Confirm Hint", canvasObject.transform, GameLocalization.Get("nameEntry.confirmHint"), 22,
                new Vector2(0f, -32f), Color.white);
        }

        private static Text CreateText(string name, Transform parent, string content, int fontSize,
            Vector2 position, Color color)
        {
            GameObject textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            Text text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = content;
            text.fontSize = fontSize;
            text.color = color;
            text.alignment = TextAnchor.MiddleCenter;
            RectTransform rect = text.rectTransform;
            rect.anchorMin = new Vector2(.5f, .5f);
            rect.anchorMax = new Vector2(.5f, .5f);
            rect.pivot = new Vector2(.5f, .5f);
            rect.sizeDelta = new Vector2(1120f, 110f);
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

        private static string Normalize(string value)
        {
            return value.Trim().ToLowerInvariant().Replace(" ", string.Empty).Replace("-", string.Empty)
                .Replace("_", string.Empty);
        }

        private static NameResult ClassifyName(string normalized)
        {
            if (string.IsNullOrEmpty(normalized) || normalized == "滚木" || normalized == "棍母")
            {
                return NameResult.Empty;
            }

            if (normalized == "林健太" || normalized == "linjiantai" || normalized == "jiantailin")
            {
                return NameResult.TrueName;
            }

            if (normalized == "林春宝" || normalized == "linchunbao" || normalized == "chunbaolin")
            {
                return NameResult.LinChunbao;
            }

            if (normalized == "余楚轩" || normalized == "yuchuxuan" || normalized == "chuxuanyu")
            {
                return NameResult.YuChuxuan;
            }

            if (normalized == "十三" || normalized == "thirteen" || normalized == "shisan"
                || normalized == "sanshi")
            {
                return NameResult.Thirteen;
            }

            return NameResult.Normal;
        }

        private static string GetFeedback(NameResult result, string submittedName)
        {
            switch (result)
            {
                case NameResult.TrueName: return GameLocalization.Get("nameEntry.kentaEasterEgg");
                case NameResult.LinChunbao: return GameLocalization.Get("nameEntry.chunbaoEasterEgg");
                case NameResult.YuChuxuan: return GameLocalization.Get("nameEntry.chuxuanEasterEgg");
                case NameResult.Thirteen: return GameLocalization.Get("nameEntry.thirteenEasterEgg");
                case NameResult.Empty: return GameLocalization.Get("nameEntry.emptyEasterEgg");
                default: return GameLocalization.Format("nameEntry.normalResult", submittedName);
            }
        }

        private enum NameResult
        {
            Normal,
            TrueName,
            LinChunbao,
            YuChuxuan,
            Thirteen,
            Empty
        }
    }
}
