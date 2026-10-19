using UnityEngine;

namespace RPG
{
    public sealed class BathroomToiletPuzzle : MonoBehaviour, IInvestigationHandler
    {
        [SerializeField] private WorldItem medicineCabinetKey;
        private bool introduced;
        private int flushCount;

        private void Awake()
        {
            medicineCabinetKey?.ConfigureLocalization("apartment.medicineCabinetKey",
                "apartment.item.medicineCabinetKey");
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.BathroomToilet && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (!introduced)
            {
                if (!dialogue.StartDialogue(PlayerPages("apartment.bathroom.toilet.intro.1",
                        "apartment.bathroom.toilet.intro.2", "apartment.bathroom.toilet.intro.3"))) return false;
                introduced = true;
                return true;
            }

            flushCount++;
            switch (flushCount)
            {
                case 1:
                    return true;
                case 2:
                    return dialogue.StartDialogue(LinPages("apartment.bathroom.toilet.second.1",
                        "apartment.bathroom.toilet.second.2"));
                case 3:
                    return dialogue.StartDialogue(LinPages("apartment.bathroom.toilet.third"));
                case 4:
                    return dialogue.StartDialogue(PlayerPages("apartment.bathroom.toilet.fourth.1",
                        "apartment.bathroom.toilet.fourth.2"), RevealKey);
                default:
                    return true;
            }
        }

        private void RevealKey()
        {
            if (medicineCabinetKey != null) medicineCabinetKey.gameObject.SetActive(true);
        }

        private static DialoguePage[] PlayerPages(params string[] keys) => CreatePages("主角", keys);

        private static DialoguePage[] LinPages(params string[] keys) =>
            CreatePages(GameLocalization.Get("apartment.speaker.linChunbao"), keys);

        private static DialoguePage[] CreatePages(string speaker, string[] keys)
        {
            DialoguePage[] pages = new DialoguePage[keys.Length];
            for (int index = 0; index < keys.Length; index++)
                pages[index] = new DialoguePage(speaker, GameLocalization.Get(keys[index]));
            return pages;
        }
    }
}
