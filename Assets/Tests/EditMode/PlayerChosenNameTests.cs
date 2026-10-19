using NUnit.Framework;
using RPG;

namespace RPG.Tests
{
    public class PlayerChosenNameTests
    {
        [TearDown]
        public void TearDown()
        {
            PlayerChosenName.Set(null);
        }

        [Test]
        public void ResolveDialogueSpeaker_UsesProtagonistBeforeANameIsConfirmed()
        {
            PlayerChosenName.Set(null);

            Assert.That(PlayerChosenName.ResolveDialogueSpeaker("主角"), Is.EqualTo("主角"));
        }

        [Test]
        public void ResolveDialogueSpeaker_UsesConfirmedPlayerNameForProtagonist()
        {
            PlayerChosenName.Set("林健太");

            Assert.That(PlayerChosenName.ResolveDialogueSpeaker("主角"), Is.EqualTo("林健太"));
        }

        [Test]
        public void ResolveDialogueSpeaker_KeepsOtherSpeakersUnchanged()
        {
            PlayerChosenName.Set("测试名");

            Assert.That(PlayerChosenName.ResolveDialogueSpeaker("林春宝"), Is.EqualTo("林春宝"));
        }
    }
}
