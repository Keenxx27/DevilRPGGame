using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace RPG.Tests
{
    public class DialogueHUDTests
    {
        private GameObject root;

        [TearDown]
        public void TearDown()
        {
            PlayerChosenName.Set(null);
            PlayerPrefs.DeleteKey(GameLocalization.PreferenceKey);
            GameLocalization.Reload();
            if (root != null)
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void TryShow_SetsContentAndActivatesPanel()
        {
            DialogueHUD hud = CreateHud(
                out GameObject panel, out Text speaker, out Text body, out Text hint);

            Assert.That(hud.TryShow("教授", "所以同学们要明确，", false), Is.True);
            Assert.That(panel.activeSelf, Is.True);
            Assert.That(speaker.text, Is.EqualTo("教授"));
            Assert.That(body.text, Is.EqualTo("所以同学们要明确，"));
            Assert.That(hint.text, Does.Contain("R 继续"));
            Assert.That(hint.color.a, Is.EqualTo(0.45f).Within(0.001f));
        }

        [Test]
        public void SetPage_UsesFullHintAlphaWhenPageCanAdvance()
        {
            DialogueHUD hud = CreateHud(out _, out _, out _, out Text hint);

            hud.SetPage("主角", "测试", true);

            Assert.That(hint.color.a, Is.EqualTo(1f).Within(0.001f));
        }

        [Test]
        public void SetPage_UsesLocalizedDialogueHint()
        {
            GameLocalization.SetLanguage(GameLanguage.Japanese);
            DialogueHUD hud = CreateHud(out _, out _, out _, out Text hint);

            hud.SetPage("主角", "测试", true);

            Assert.That(hint.text, Is.EqualTo("R 次へ　Space スキップ"));
        }

        [Test]
        public void Hide_DeactivatesPanel()
        {
            DialogueHUD hud = CreateHud(out GameObject panel, out _, out _, out _);
            hud.TryShow("主角", "测试", true);

            hud.Hide();

            Assert.That(panel.activeSelf, Is.False);
            Assert.That(hud.IsVisible, Is.False);
        }

        [Test]
        public void TryShow_ReturnsFalseWhenConfigurationIsMissing()
        {
            root = new GameObject("HUD");
            DialogueHUD hud = root.AddComponent<DialogueHUD>();
            LogAssert.Expect(LogType.Error,
                "DialogueHUD requires a panel and three Text references.");

            Assert.That(hud.TryShow("教授", "测试", true), Is.False);
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void TryShow_ReplacesProtagonistSpeakerWithConfirmedPlayerName()
        {
            PlayerChosenName.Set("测试名");
            DialogueHUD hud = CreateHud(out _, out Text speaker, out _, out _);

            hud.TryShow("主角", "测试", true);

            Assert.That(speaker.text, Is.EqualTo("测试名"));
        }

        [Test]
        public void TryShowLockedChoice_KeepsNonProtagonistSpeakerUnchanged()
        {
            PlayerChosenName.Set("测试名");
            DialogueHUD hud = CreateHud(out _, out Text speaker, out _, out _);

            hud.TryShowLockedChoice("林春宝", "测试");

            Assert.That(speaker.text, Is.EqualTo("林春宝"));
        }

        private DialogueHUD CreateHud(
            out GameObject panel, out Text speaker, out Text body, out Text hint)
        {
            root = new GameObject("HUD");
            DialogueHUD hud = root.AddComponent<DialogueHUD>();
            panel = new GameObject("Panel");
            panel.transform.SetParent(root.transform);
            speaker = new GameObject("Speaker").AddComponent<Text>();
            body = new GameObject("Body").AddComponent<Text>();
            hint = new GameObject("Hint").AddComponent<Text>();
            speaker.transform.SetParent(panel.transform);
            body.transform.SetParent(panel.transform);
            hint.transform.SetParent(panel.transform);
            SetField(hud, "panel", panel);
            SetField(hud, "speakerText", speaker);
            SetField(hud, "bodyText", body);
            SetField(hud, "hintText", hint);
            return hud;
        }

        private static void SetField(object target, string fieldName, object value)
        {
            target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(target, value);
        }
    }
}
