using System;
using System.Collections.Generic;

namespace RPG
{
    public enum DialogueAdvanceResult
    {
        Blocked,
        Advanced,
        Completed
    }

    public sealed class DialoguePlayback
    {
        private readonly IReadOnlyList<DialoguePage> pages;
        private float revealedCharacters;
        private int visibleCharacterCount;

        public int PageIndex { get; private set; }
        public string Speaker => CurrentPage.Speaker;
        public string VisibleText => CurrentPage.Text.Substring(0, visibleCharacterCount);
        public bool IsPageComplete => visibleCharacterCount >= CurrentPage.Text.Length;
        public bool IsComplete { get; private set; }

        private DialoguePage CurrentPage => pages[PageIndex];

        public DialoguePlayback(IReadOnlyList<DialoguePage> pages)
        {
            if (pages == null || pages.Count == 0)
            {
                throw new ArgumentException("Dialogue requires at least one page.", nameof(pages));
            }

            this.pages = pages;
        }

        public void Step(float charactersPerSecond, float deltaTime)
        {
            if (IsComplete || IsPageComplete || charactersPerSecond <= 0f || deltaTime <= 0f)
            {
                return;
            }

            revealedCharacters += charactersPerSecond * deltaTime;
            visibleCharacterCount = Math.Min(
                CurrentPage.Text.Length,
                (int)Math.Floor(revealedCharacters));
        }

        public DialogueAdvanceResult TryAdvance()
        {
            if (IsComplete)
            {
                return DialogueAdvanceResult.Completed;
            }

            if (!IsPageComplete)
            {
                return DialogueAdvanceResult.Blocked;
            }

            if (PageIndex >= pages.Count - 1)
            {
                IsComplete = true;
                return DialogueAdvanceResult.Completed;
            }

            PageIndex++;
            revealedCharacters = 0f;
            visibleCharacterCount = 0;
            return DialogueAdvanceResult.Advanced;
        }

        public void Skip()
        {
            IsComplete = true;
        }

        public void RevealCurrentPage()
        {
            visibleCharacterCount = CurrentPage.Text.Length;
            revealedCharacters = visibleCharacterCount;
        }
    }
}
