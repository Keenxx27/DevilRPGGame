using UnityEngine;

namespace RPG
{
    public sealed class BathroomPasswordNote : MonoBehaviour, IInvestigationHandler
    {
        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.PasswordNote && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            return FindObjectOfType<DialogueController>().StartDialogue(new[]
            {
                new DialoguePage("主角", GameLocalization.Get("apartment.bathroom.passwordNote.1")),
                new DialoguePage("主角", GameLocalization.Get("apartment.bathroom.passwordNote.2"))
            });
        }
    }
}
