using UnityEngine;

namespace RPG
{
    public sealed class LinChunbaoPhone : MonoBehaviour, IInvestigationHandler
    {
        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.LinBedroomPhone && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            return CanInvestigate(kind);
        }
    }
}
