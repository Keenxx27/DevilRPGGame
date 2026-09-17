using System.Collections;
using UnityEngine;

namespace RPG
{
    public sealed class ApartmentEntranceIntro : MonoBehaviour
    {
        private static readonly DialoguePage[] Pages =
        {
            new DialoguePage("主角", "（侵蚀痕迹的灼热尚未消退。）\n不知是从何时起养成了这种自言自语的行为……\n可能从小便是这样。"),
            new DialoguePage("主角", "啊，公寓楼就在前面。\n虽说老旧，但也是个家。")
        };

        private IEnumerator Start()
        {
            yield return null;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue != null)
            {
                dialogue.StartDialogue(Pages);
            }
        }
    }
}
