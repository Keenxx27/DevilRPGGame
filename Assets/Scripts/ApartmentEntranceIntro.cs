using System.Collections;
using UnityEngine;

namespace RPG
{
    public sealed class ApartmentEntranceIntro : MonoBehaviour
    {
        private static DialoguePage[] CreatePages()
        {
            return new[]
            {
                new DialoguePage("主角", GameLocalization.Get("apartment.entrance.intro.1")),
                new DialoguePage("主角", GameLocalization.Get("apartment.entrance.intro.2"))
            };
        }

        private IEnumerator Start()
        {
            yield return null;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue != null)
            {
                dialogue.StartDialogue(CreatePages());
            }
        }
    }
}
