using UnityEngine;

namespace RPG
{
    public sealed class ApartmentDepartureDialogue : MonoBehaviour
    {
        private const string DriedMeatName = "肉干";
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("林春宝", "你准备好了？"),
            new DialoguePage("主角", "我什么都拿好了，告诉我那个新开的小归墟在哪里吧。"),
            new DialoguePage("林春宝", "就在郊区。出了公寓楼你向右转，\n从公交车站的方向看来就是向左转。"),
            new DialoguePage("主角", "明白了。"),
            new DialoguePage("林春宝", "还有一件事……"),
            new DialoguePage("主角", "嗯？"),
            new DialoguePage("林春宝", "那里绝对有丧家犬的人，就是我们市里\n那个臭名昭著的猎魔帮派。"),
            new DialoguePage("主角", "明白了，真出了什么事我会联系你的。")
        };

        private bool hasPlayed;

        private void Update()
        {
            if (hasPlayed) return;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            LinChunbaoLivingRoomDialogue linChunbao = FindObjectOfType<LinChunbaoLivingRoomDialogue>();
            if (dialogue == null || inventory == null || linChunbao == null || dialogue.IsPlaying
                || !linChunbao.IsAtSofa || !inventory.HasItem(DriedMeatName)) return;

            Vector2 position = dialogue.transform.position;
            if (position.x < 132f || position.x > 148f || position.y < -5f || position.y > 5f)
            {
                return;
            }

            hasPlayed = dialogue.StartDialogue(Pages, linChunbao.MarkDepartureConversationComplete);
        }
    }
}
