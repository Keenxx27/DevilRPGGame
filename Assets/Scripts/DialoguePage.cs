using System;
using UnityEngine;

namespace RPG
{
    [Serializable]
    public sealed class DialoguePage
    {
        [SerializeField] private string speaker;
        [SerializeField, TextArea] private string text;

        public string Speaker => speaker;
        public string Text => text;

        public DialoguePage(string speaker, string text)
        {
            this.speaker = speaker;
            this.text = text;
        }
    }
}
