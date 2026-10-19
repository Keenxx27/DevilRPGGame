using UnityEngine;

namespace RPG
{
    public sealed class LockedFoodCabinet : MonoBehaviour, IInvestigationHandler
    {
        private DialogueController dialogue;
        private bool hasBeenInvestigated;
        private bool unlocked;
        private bool meatTaken;
        private FoodCabinetPasswordLock passwordLock;
        [SerializeField] private WorldItem driedMeat;

        public void Configure(WorldItem meat)
        {
            driedMeat = meat;
            if (driedMeat != null)
            {
                driedMeat.ConfigureLocalization("apartment.driedMeat", "apartment.item.driedMeat");
                driedMeat.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            driedMeat?.ConfigureLocalization("apartment.driedMeat", "apartment.item.driedMeat");
            dialogue = FindObjectOfType<DialogueController>();
            passwordLock = GetComponent<FoodCabinetPasswordLock>();
            if (passwordLock == null) passwordLock = gameObject.AddComponent<FoodCabinetPasswordLock>();
        }

        public bool CanInvestigate(InvestigationKind kind) => kind == InvestigationKind.KitchenFoodCabinet
            && dialogue != null && !dialogue.IsPlaying;

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;

            if (unlocked)
            {
                if (meatTaken) return dialogue.StartDialogue(PlayerPages("apartment.kitchen.foodCabinet.repeat"));
                return dialogue.StartDialogue(PlayerPages("apartment.kitchen.foodCabinet.open.1",
                    "apartment.kitchen.foodCabinet.open.2"), GiveDriedMeat);
            }

            if (hasBeenInvestigated)
            {
                return passwordLock != null && passwordLock.Show(FindObjectOfType<PlayerInteraction>());
            }
            if (!dialogue.StartDialogue(CreateIntroductionPages(), NotifyFirstInvestigationComplete))
            {
                return false;
            }

            hasBeenInvestigated = true;
            return true;
        }

        private static void NotifyFirstInvestigationComplete()
        {
            FindObjectOfType<LinChunbaoEncounter>()?.LeaveKitchenForLivingRoom();
        }

        public void OpenCabinet()
        {
            if (unlocked) return;
            unlocked = true;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null) renderer.color = new Color(.2f, .12f, .06f, 1f);
        }

        private void GiveDriedMeat()
        {
            if (driedMeat == null)
            {
                meatTaken = true;
                return;
            }

            driedMeat.gameObject.SetActive(true);
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            meatTaken = inventory != null && inventory.TryStore(driedMeat);
        }

        private static DialoguePage[] CreateIntroductionPages()
        {
            string lin = GameLocalization.Get("apartment.speaker.linChunbao");
            return new[]
            {
                new DialoguePage(lin, GameLocalization.Get("apartment.kitchen.foodCabinet.intro.1")),
                new DialoguePage(lin, GameLocalization.Get("apartment.kitchen.foodCabinet.intro.2")),
                new DialoguePage(lin, GameLocalization.Get("apartment.kitchen.foodCabinet.intro.3")),
                new DialoguePage("主角", GameLocalization.Get("apartment.kitchen.foodCabinet.intro.4")),
                new DialoguePage(lin, GameLocalization.Get("apartment.kitchen.foodCabinet.intro.5"))
            };
        }

        private static DialoguePage[] PlayerPages(params string[] keys)
        {
            DialoguePage[] pages = new DialoguePage[keys.Length];
            for (int index = 0; index < keys.Length; index++)
                pages[index] = new DialoguePage("主角", GameLocalization.Get(keys[index]));
            return pages;
        }
    }
}
