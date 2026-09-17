using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class DialogueHUD : MonoBehaviour
    {
        private const string Hint = "R 继续　Space 跳过";

        [SerializeField] private GameObject panel;
        [SerializeField] private Text speakerText;
        [SerializeField] private Text bodyText;
        [SerializeField] private Text hintText;

        private bool configurationErrorReported;

        public bool IsVisible => panel != null && panel.activeSelf;

        private void Awake()
        {
            Hide();
            ApplySystemFont();
        }

        public bool TryShow(string speaker, string visibleText, bool canAdvance)
        {
            if (!SetPage(speaker, visibleText, canAdvance))
            {
                return false;
            }

            panel.SetActive(true);
            return true;
        }

        public bool SetPage(string speaker, string visibleText, bool canAdvance)
        {
            if (!HasValidConfiguration())
            {
                if (!configurationErrorReported)
                {
                    Debug.LogError(
                        "DialogueHUD requires a panel and three Text references.", this);
                    configurationErrorReported = true;
                }

                return false;
            }

            speakerText.text = speaker ?? string.Empty;
            bodyText.text = visibleText ?? string.Empty;
            hintText.text = Hint;
            Color hintColor = hintText.color;
            hintColor.a = canAdvance ? 1f : 0.45f;
            hintText.color = hintColor;
            return true;
        }

        public bool TryShowLockedChoice(string speaker, string text)
        {
            if (!HasValidConfiguration())
            {
                return false;
            }

            speakerText.text = speaker ?? string.Empty;
            bodyText.text = text ?? string.Empty;
            hintText.text = string.Empty;
            panel.SetActive(true);
            return true;
        }

        public void Hide()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        private bool HasValidConfiguration()
        {
            return panel != null && speakerText != null && bodyText != null && hintText != null;
        }

        private void ApplySystemFont()
        {
            if (speakerText == null || bodyText == null || hintText == null)
            {
                return;
            }

            try
            {
                Font font = Font.CreateDynamicFontFromOSFont(
                    new[] { "Microsoft YaHei", "SimHei", "Arial" }, 28);
                if (font == null)
                {
                    Debug.LogWarning("Unable to create a system font for dialogue text.", this);
                    return;
                }

                speakerText.font = font;
                bodyText.font = font;
                hintText.font = font;
            }
            catch (Exception exception)
            {
                Debug.LogWarning(
                    "Unable to create a system font for dialogue text: " + exception.Message,
                    this);
            }
        }
    }
}
