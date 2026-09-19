using UnityEngine;

namespace RPG
{
    public sealed class RoadToSuburbsOneIntro : MonoBehaviour
    {
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("主角", "丧家犬没有总部，没有老板，没有规矩。"),
            new DialoguePage("主角", "他们是恶魔猎人里最底层的一群，捡别人不要的恶魔残骸，\n在归墟边缘翻垃圾，在黑市角落里摆地摊。"),
            new DialoguePage("主角", "恐怕半人半魔的我，也会成为他们捡的“垃圾”吧……"),
            new DialoguePage("主角", "最好绕着他们走，能不被堵路已经是最好的了。")
        };

        private bool hasPlayed;

        private void Update()
        {
            if (hasPlayed) return;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null || dialogue.IsPlaying) return;

            Vector2 position = dialogue.transform.position;
            if (position.x < 246f || position.y > -3.2f) return;
            hasPlayed = dialogue.StartDialogue(Pages);
        }
    }
}
