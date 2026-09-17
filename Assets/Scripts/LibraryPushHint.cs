using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class LibraryPushHint : MonoBehaviour
    {
        private Text text;

        public void Show(string message)
        {
            EnsureText();
            text.text = message;
            text.enabled = true;
        }

        public void Hide()
        {
            if (text != null) text.enabled = false;
        }

        private void EnsureText()
        {
            if (text != null) return;

            GameObject canvasObject = new GameObject("Library Push Hint Canvas");
            canvasObject.transform.SetParent(transform, false);
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 20;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());
            canvasObject.AddComponent<GraphicRaycaster>();

            GameObject textObject = new GameObject("Library Push Hint Text");
            textObject.transform.SetParent(canvasObject.transform, false);
            RectTransform rect = textObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.16f);
            rect.anchorMax = new Vector2(0.5f, 0.16f);
            rect.sizeDelta = new Vector2(520f, 40f);
            text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 26;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
        }
    }
}
