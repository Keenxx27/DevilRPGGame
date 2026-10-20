using UnityEngine;

namespace RPG
{
    public sealed class PasswordNote : MonoBehaviour, IInvestigationHandler
    {
        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.PasswordNote && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            return CanInvestigate(kind)
                && FindObjectOfType<DialogueController>().StartDialogue(new[]
                {
                    new DialoguePage("主角", GameLocalization.Get(
                        "apartment.protagonistBedroom.passwordNote.1")),
                    new DialoguePage("主角", GameLocalization.Get(
                        "apartment.protagonistBedroom.passwordNote.2"))
                });
        }
    }
}
