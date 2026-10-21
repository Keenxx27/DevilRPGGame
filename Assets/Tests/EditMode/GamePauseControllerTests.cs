using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Tests
{
    public class GamePauseControllerTests
    {
        private GameObject controllerObject;
        private GameObject pauseCanvas;

        [TearDown]
        public void TearDown()
        {
            Time.timeScale = 1f;
            PlayerPrefs.DeleteKey(GameLocalization.PreferenceKey);
            GameLocalization.Reload();
            if (pauseCanvas != null) Object.DestroyImmediate(pauseCanvas);
            if (controllerObject != null) Object.DestroyImmediate(controllerObject);
        }

        [Test]
        public void Awake_UsesLocalizedPauseMessage()
        {
            GameLocalization.SetLanguage(GameLanguage.Japanese);
            controllerObject = new GameObject("Game Pause Controller Test");
            GamePauseController controller = controllerObject.AddComponent<GamePauseController>();
            typeof(GamePauseController)
                .GetMethod("CreatePausePage", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(controller, null);
            pauseCanvas = (GameObject)typeof(GamePauseController)
                .GetField("pauseCanvas", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(controller);

            Text pauseText = pauseCanvas.GetComponentInChildren<Text>(true);

            Assert.That(pauseText.text, Is.EqualTo("ゲーム一時停止\n\nEscキーで再開"));
        }
    }
}
