using UnityEngine;

namespace RPG
{
    public sealed class RubbleLoreNote : MonoBehaviour, IInvestigationHandler
    {
        [SerializeField] private RubbleBossEncounter encounter;
        [SerializeField] private bool isTuitionReceipt;
        [SerializeField] private bool hasBeenRead;

        private static readonly string[] TuitionKeys =
        {
            "rubble.lore.tuition.1", "rubble.lore.tuition.2",
            "rubble.lore.tuition.3", "rubble.lore.tuition.4"
        };

        private static readonly string[] DebtKeys =
        {
            "rubble.lore.debt.1", "rubble.lore.debt.2",
            "rubble.lore.debt.3", "rubble.lore.debt.4"
        };

        public void Configure(RubbleBossEncounter owner, bool tuitionReceipt)
        {
            encounter = owner;
            isTuitionReceipt = tuitionReceipt;
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            return kind == InvestigationKind.RubbleLoreNote && !hasBeenRead && encounter != null
                && encounter.IsBattleActive;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            if (!encounter.PlayLoreDialogue(CreatePages(isTuitionReceipt ? TuitionKeys : DebtKeys))) return false;
            hasBeenRead = true;
            return true;
        }

        private static DialoguePage[] CreatePages(string[] keys)
        {
            DialoguePage[] pages = new DialoguePage[keys.Length];
            string bossName = GameLocalization.Get("rubble.speaker.strayDogsBoss");
            for (int index = 0; index < keys.Length; index++)
            {
                pages[index] = new DialoguePage(index == keys.Length - 1 ? bossName : "主角",
                    GameLocalization.Get(keys[index]));
            }

            return pages;
        }
    }
}
