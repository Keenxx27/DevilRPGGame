using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(DoorController))]
    public sealed class LinChunbaoBedroomDoor : MonoBehaviour, IDoorInteractionOverride
    {
        private static readonly DialoguePage[] BeforeSandbagPages =
        {
            new DialoguePage("林春宝", "喂喂，做人可得讲点界限！"),
            new DialoguePage("主角", "万一里边有密码锁相关的线索呢？"),
            new DialoguePage("林春宝", "那是我的房间！要找线索也要我自己进去找。")
        };
        private static readonly DialoguePage[] AfterSandbagPages =
        {
            new DialoguePage("林春宝", "喂喂，做人可得讲点界限！"),
            new DialoguePage("主角", "万一里边有密码锁相关的线索呢？"),
            new DialoguePage("林春宝", "那是我的房间！要找线索……"),
            new DialoguePage("林春宝", "至少得帮我修好这个沙袋才让你进去。")
        };

        private DoorController door;
        private bool permissionGranted;

        private void Awake()
        {
            door = GetComponent<DoorController>();
        }

        public bool TryInteract(PlayerInteraction player)
        {
            if (permissionGranted) return door != null && door.ToggleDoor();
            DialogueController dialogue = player != null ? player.GetComponent<DialogueController>() : null;
            if (dialogue == null || dialogue.IsPlaying) return false;
            SandbagBoxingMinigame boxing = FindObjectOfType<SandbagBoxingMinigame>();
            return dialogue.StartDialogue(boxing != null && boxing.IsBroken
                ? AfterSandbagPages
                : BeforeSandbagPages);
        }

        public void GrantAccess()
        {
            permissionGranted = true;
        }
    }
}
