using UnityEngine;

namespace RPG
{
    public sealed class BathroomVentPuzzle : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        private const string KnifeName = "菜刀";
        private const string StickyKnifeName = "粘竿菜刀";
        private static readonly DialoguePage[] DefaultPages =
        {
            new DialoguePage("主角", "从这里看去，旁边的墙上贴着一张纸像是写着什么数字，也许和密码锁有关。"),
            new DialoguePage("主角", "但我够不到它，最好找个长一点的东西把它勾过来。")
        };
        private static readonly DialoguePage[] KnifePages =
        {
            new DialoguePage("主角", "从这里看去，旁边的墙上贴着一张纸像是写着什么数字，也许和密码锁有关。"),
            new DialoguePage("主角", "但我够不到它，这把刀够长，可是很可能会把那张纸弄掉。"),
            new DialoguePage("主角", "得找点什么黏的东西涂在刀上，把纸张像粘知了一样粘下来。")
        };
        private static readonly DialoguePage[] StickyKnifePages =
        {
            new DialoguePage("主角", "从这里看去，旁边的墙上贴着一张纸像是写着什么数字，也许和密码锁有关。"),
            new DialoguePage("主角", "现在有了这粘竿菜刀，我就能把它粘过来了。")
        };

        [SerializeField] private WorldItem passwordNote;
        private bool stickyKnifePrompted;
        private bool notePulled;
        private int pulls;

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.BathroomVent && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            return FindObjectOfType<DialogueController>().StartDialogue(DefaultPages);
        }

        public bool TryUseSelectedItem(InvestigationKind kind)
        {
            if (kind != InvestigationKind.BathroomVent || !CanInvestigate(kind)) return false;
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory?.SelectedItem == null) return false;
            if (inventory.SelectedItem.DisplayName == StickyKnifeName && !notePulled)
            {
                if (!stickyKnifePrompted)
                {
                    if (!FindObjectOfType<DialogueController>().StartDialogue(StickyKnifePages)) return false;
                    stickyKnifePrompted = true;
                    return true;
                }

                pulls++;
                if (pulls >= 3)
                {
                    notePulled = true;
                    if (passwordNote != null) passwordNote.gameObject.SetActive(true);
                }
                return true;
            }

            if (inventory.SelectedItem.DisplayName != KnifeName)
            {
                return false;
            }

            return FindObjectOfType<DialogueController>().StartDialogue(KnifePages);
        }
    }
}
