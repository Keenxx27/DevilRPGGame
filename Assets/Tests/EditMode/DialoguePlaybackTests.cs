using System;
using NUnit.Framework;
using RPG;

namespace RPG.Tests
{
    public class DialoguePlaybackTests
    {
        [Test]
        public void Step_RevealsTwelveCharactersPerSecond()
        {
            var playback = new DialoguePlayback(new[]
            {
                new DialoguePage("教授", "一二三四五六七八九十甲乙丙")
            });

            playback.Step(12f, 0.5f);

            Assert.That(playback.VisibleText, Is.EqualTo("一二三四五六"));
            Assert.That(playback.IsPageComplete, Is.False);
        }

        [Test]
        public void TryAdvance_IsBlockedUntilPageCompletes()
        {
            DialoguePlayback playback = CreateTwoPagePlayback();

            Assert.That(playback.TryAdvance(), Is.EqualTo(DialogueAdvanceResult.Blocked));
            Assert.That(playback.PageIndex, Is.Zero);
        }

        [Test]
        public void TryAdvance_MovesToNextPageAndClearsVisibleText()
        {
            DialoguePlayback playback = CreateTwoPagePlayback();
            playback.Step(100f, 1f);

            Assert.That(playback.TryAdvance(), Is.EqualTo(DialogueAdvanceResult.Advanced));
            Assert.That(playback.PageIndex, Is.EqualTo(1));
            Assert.That(playback.VisibleText, Is.Empty);
            Assert.That(playback.Speaker, Is.EqualTo("主角"));
        }

        [Test]
        public void TryAdvance_CompletesAfterLastPageFinishes()
        {
            DialoguePlayback playback = CreateTwoPagePlayback();
            playback.Step(100f, 1f);
            playback.TryAdvance();
            playback.Step(100f, 1f);

            Assert.That(playback.TryAdvance(), Is.EqualTo(DialogueAdvanceResult.Completed));
            Assert.That(playback.IsComplete, Is.True);
        }

        [Test]
        public void Skip_CompletesWholeSequenceImmediately()
        {
            DialoguePlayback playback = CreateTwoPagePlayback();

            playback.Skip();

            Assert.That(playback.IsComplete, Is.True);
        }

        [Test]
        public void Constructor_RejectsEmptyPages()
        {
            Assert.Throws<ArgumentException>(() =>
                new DialoguePlayback(Array.Empty<DialoguePage>()));
        }

        private static DialoguePlayback CreateTwoPagePlayback()
        {
            return new DialoguePlayback(new[]
            {
                new DialoguePage("教授", "第一句话。"),
                new DialoguePage("主角", "第二句话。")
            });
        }
    }
}
