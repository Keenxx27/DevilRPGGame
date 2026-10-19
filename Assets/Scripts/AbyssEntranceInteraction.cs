using UnityEngine;

namespace RPG
{
    public sealed class AbyssEntranceInteraction : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] OpeningPages =
        {
            new DialoguePage("主角", "......没想到竟从那么高的地方掉下来......"),
            new DialoguePage("主角", "并非想象中地狱般的景象。"),
            new DialoguePage("主角", "没有熔岩，没有硫磺味。取而代之的是一种潮湿的、带着奇异甜腥的泥土气息。")
        };

        private static readonly DialoguePage[] MossPages =
        {
            new DialoguePage("主角", "这些发出幽蓝荧光的苔藓像是倒挂的星河。")
        };

        private static readonly DialoguePage[] CrystalPages =
        {
            new DialoguePage("主角", "这些散落的温热晶体轻轻震颤，发出风铃般的细微叮咚声。")
        };

        private static readonly DialoguePage[] PassagePages =
        {
            new DialoguePage("主角", "这是一个完全陌生的生态系统。"),
            new DialoguePage("主角", "寂静，美丽，带着地底深处的呼吸节奏。"),
            new DialoguePage("主角", "或许，我原本来自此地。")
        };

        [SerializeField] private bool openingPlayed;

        private void OnEnable()
        {
            AbyssEntranceDropIn.PlayerFinishedDropping += PlayOpening;
        }

        private void OnDisable()
        {
            AbyssEntranceDropIn.PlayerFinishedDropping -= PlayOpening;
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            if (kind != InvestigationKind.AbyssGlowingMoss
                && kind != InvestigationKind.AbyssWarmCrystal
                && kind != InvestigationKind.AbyssCavePassage)
            {
                return false;
            }

            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;

            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (kind == InvestigationKind.AbyssGlowingMoss)
            {
                return dialogue.StartDialogue(MossPages);
            }

            if (kind == InvestigationKind.AbyssWarmCrystal)
            {
                return dialogue.StartDialogue(CrystalPages);
            }

            return dialogue.StartDialogue(PassagePages, TransitionToMushroomCave);
        }

        private void PlayOpening(Transform player)
        {
            if (openingPlayed || player == null) return;
            DialogueController dialogue = player.GetComponent<DialogueController>();
            if (dialogue == null || !dialogue.StartDialogue(OpeningPages)) return;
            openingPlayed = true;
        }

        private static void TransitionToMushroomCave()
        {
            FindObjectOfType<PlayerInteraction>()?.GetComponent<MapTransitionService>()
                ?.RequestTransition("FluorescentMushroomCave", "FluorescentMushroomCaveFromAbyss");
        }
    }
}
