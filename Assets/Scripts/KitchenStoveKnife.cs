using UnityEngine;

namespace RPG
{
    public sealed class KitchenStoveKnife : MonoBehaviour, IInvestigationHandler
    {
        [SerializeField] private WorldItem knife;
        private bool exposed;

        private void Awake()
        {
            knife?.ConfigureLocalization("apartment.knife", "apartment.item.knife");
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.KitchenStove && !exposed
                && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            DialoguePage[] pages =
            {
                new DialoguePage("主角", GameLocalization.Get("apartment.kitchen.stoveKnife"))
            };
            if (!dialogue.StartDialogue(pages, ExposeKnife)) return false;
            exposed = true;
            return true;
        }

        private void ExposeKnife()
        {
            if (knife != null) knife.gameObject.SetActive(true);
        }
    }
}
