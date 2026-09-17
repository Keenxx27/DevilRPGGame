using UnityEngine;

namespace RPG
{
    public sealed class KitchenStoveKnife : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("主角", "灶台上有一把菜刀。\n也许能用来撬开生锈的铰链。")
        };

        [SerializeField] private WorldItem knife;
        private bool exposed;

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
            if (!dialogue.StartDialogue(Pages, ExposeKnife)) return false;
            exposed = true;
            return true;
        }

        private void ExposeKnife()
        {
            if (knife != null) knife.gameObject.SetActive(true);
        }
    }
}
