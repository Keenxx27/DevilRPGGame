using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace RPG.Tests
{
    public class UiAudioTests
    {
        private static readonly FieldInfo SourceField = typeof(UiAudio).GetField("source",
            BindingFlags.Static | BindingFlags.NonPublic);

        [TearDown]
        public void TearDown()
        {
            GameObject host = GameObject.Find("UI Audio");
            if (host != null) Object.DestroyImmediate(host);
            SourceField.SetValue(null, null);
        }

        [Test]
        public void PlayConfirm_InEditMode_DoesNotCallDontDestroyOnLoad()
        {
            Assert.That(Application.isPlaying, Is.False);
            Assert.DoesNotThrow(UiAudio.PlayConfirm);
            Assert.That(GameObject.Find("UI Audio"), Is.Not.Null);
        }
    }
}
