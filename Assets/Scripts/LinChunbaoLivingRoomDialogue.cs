using UnityEngine;

namespace RPG
{
    public sealed class LinChunbaoLivingRoomDialogue : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        private const string StickyKnifeName = "粘竿菜刀";
        private static readonly DialoguePage[] FirstPages =
        {
            new DialoguePage("林春宝", "在家里找找看，也许能有解开密码的线索。")
        };

        private static readonly DialoguePage[] RepeatPages =
        {
            new DialoguePage("林春宝", "看我干嘛，我怎么知道密码？")
        };
        private static readonly DialoguePage[] DepartureRepeatPages =
        {
            new DialoguePage("林春宝", "你还磨蹭什么？我还要补沙袋呢！")
        };
        private static readonly DialoguePage[] PhoneRewardPages =
        {
            new DialoguePage("林春宝", "哥，你居然把我手机里那关小游戏打过去了？\n我卡了好几天呢。"),
            new DialoguePage("主角", "只是躲开几块障碍而已。\n你把这种东西当训练？"),
            new DialoguePage("林春宝", "当然，练反应和手眼协调。\n对了，我刚在你床头柜旁边找到这个。"),
            new DialoguePage("林春宝", "估计是你翻东西时掉的。拿好，\n别又把钥匙和膏药混在一起。"),
            new DialoguePage("主角", "……谢谢。")
        };
        private static readonly DialoguePage[] RepairSandbagPages =
        {
            new DialoguePage("林春宝", "你是把那胶布贴到了菜刀上？"),
            new DialoguePage("主角", "也许这膏药能派上用场……"),
            new DialoguePage("林春宝", "就它了！要是用针线缝缝补补的要花掉我一整晚。"),
            new DialoguePage("主角", "所以……"),
            new DialoguePage("林春宝", "嗯？"),
            new DialoguePage("主角", "我能去你的房间里找线索了吗？"),
            new DialoguePage("林春宝", "随便你，去吧！")
        };

        private bool isAtSofa;
        private bool hasBeenInvestigated;
        private bool hasGivenBedsideKey;
        private bool departureConversationComplete;

        public bool IsAtSofa => isAtSofa;

        public void ActivateAtSofa()
        {
            isAtSofa = true;
        }

        public void MarkDepartureConversationComplete()
        {
            departureConversationComplete = true;
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.LivingRoomLinChunbao
                && isAtSofa && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (LinChunbaoPhone.GameCompleted && !hasGivenBedsideKey)
            {
                return dialogue.StartDialogue(PhoneRewardPages, GiveBedsideKey);
            }

            if (departureConversationComplete)
            {
                return dialogue.StartDialogue(DepartureRepeatPages);
            }

            DialoguePage[] pages = hasBeenInvestigated ? RepeatPages : FirstPages;
            if (!dialogue.StartDialogue(pages)) return false;
            hasBeenInvestigated = true;
            return true;
        }

        public bool TryUseSelectedItem(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            SandbagBoxingMinigame boxing = FindObjectOfType<SandbagBoxingMinigame>();
            if (boxing == null || !boxing.IsBroken) return false;

            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory?.SelectedItem == null || inventory.SelectedItem.DisplayName != StickyKnifeName)
            {
                return false;
            }

            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (!dialogue.StartDialogue(RepairSandbagPages, GrantBedroomAccess)) return false;
            inventory.TryRemoveSelected(StickyKnifeName, out _);
            return true;
        }

        private static void GrantBedroomAccess()
        {
            FindObjectOfType<LinChunbaoBedroomDoor>()?.GrantAccess();
        }

        private void GiveBedsideKey()
        {
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory == null || inventory.HasItem("床头柜钥匙"))
            {
                hasGivenBedsideKey = true;
                return;
            }

            GameObject keyObject = new GameObject("Bedside Table Key");
            keyObject.transform.position = inventory.transform.position;
            keyObject.transform.localScale = new Vector3(.5f, .3f, 1f);
            SpriteRenderer renderer = keyObject.AddComponent<SpriteRenderer>();
            SpriteRenderer source = GetComponent<SpriteRenderer>();
            renderer.sprite = source != null ? source.sprite : null;
            renderer.sharedMaterial = source != null ? source.sharedMaterial : null;
            renderer.color = new Color(.72f, .56f, .2f, 1f);
            renderer.sortingOrder = 5;
            BoxCollider2D collider = keyObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            WorldItem key = keyObject.AddComponent<WorldItem>();
            key.Configure("床头柜钥匙", true);
            hasGivenBedsideKey = inventory.TryStore(key);
            if (!hasGivenBedsideKey)
            {
                key.Drop(inventory.transform.position + Vector3.right * .8f);
            }
        }
    }
}
