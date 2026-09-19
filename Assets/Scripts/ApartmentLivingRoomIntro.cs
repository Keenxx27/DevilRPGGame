using UnityEngine;

namespace RPG
{
    public sealed class ApartmentLivingRoomIntro : MonoBehaviour
    {
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("主角", "厨房里飘出煎蛋的香味。\n春宝大概在做晚餐吧。"),
            new DialoguePage("主角", "自从六岁时那次使我脸上留下痕迹的意外以来\n春宝便和我相依为命，确实辛苦她了。"),
            new DialoguePage("主角", "我到底在和谁说话啊……\n不管了，先去厨房。")
        };

        private bool hasPlayed;

        private void Update()
        {
            if (hasPlayed || ThirteenCompanionState.IsTravelingWithPlayer) return;

            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null) return;

            Vector2 position = dialogue.transform.position;
            if (position.x < 132f || position.x > 148f || position.y < -5f || position.y > 5f)
            {
                return;
            }

            hasPlayed = dialogue.StartDialogue(Pages);
        }
    }
}
