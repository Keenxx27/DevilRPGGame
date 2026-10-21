using UnityEngine;

namespace RPG
{
    public sealed class AbyssEntranceInteraction : MonoBehaviour, IInvestigationHandler
    {
        private static readonly string[] OpeningKeys =
        {
            "abyss.entrance.opening.1", "abyss.entrance.opening.2", "abyss.entrance.opening.3"
        };

        private static readonly string[] MossKeys =
        {
            "abyss.entrance.moss"
        };

        private static readonly string[] CrystalKeys =
        {
            "abyss.entrance.crystal"
        };

        private static readonly string[] PassageKeys =
        {
            "abyss.entrance.passage.1", "abyss.entrance.passage.2", "abyss.entrance.passage.3"
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
                return dialogue.StartDialogue(CreatePages(MossKeys));
            }

            if (kind == InvestigationKind.AbyssWarmCrystal)
            {
                return dialogue.StartDialogue(CreatePages(CrystalKeys));
            }

            return dialogue.StartDialogue(CreatePages(PassageKeys), TransitionToMushroomCave);
        }

        private void PlayOpening(Transform player)
        {
            if (openingPlayed || player == null) return;
            DialogueController dialogue = player.GetComponent<DialogueController>();
            if (dialogue == null || !dialogue.StartDialogue(CreatePages(OpeningKeys))) return;
            openingPlayed = true;
        }

        private static DialoguePage[] CreatePages(string[] keys)
        {
            DialoguePage[] pages = new DialoguePage[keys.Length];
            for (int index = 0; index < keys.Length; index++)
            {
                pages[index] = new DialoguePage("主角", GameLocalization.Get(keys[index]));
            }

            return pages;
        }

        private static void TransitionToMushroomCave()
        {
            FindObjectOfType<PlayerInteraction>()?.GetComponent<MapTransitionService>()
                ?.RequestTransition("FluorescentMushroomCave", "FluorescentMushroomCaveFromAbyss");
        }
    }
}
