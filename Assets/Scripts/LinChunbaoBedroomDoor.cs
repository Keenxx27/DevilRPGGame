using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(DoorController))]
    public sealed class LinChunbaoBedroomDoor : MonoBehaviour, IDoorInteractionOverride
    {
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
            return dialogue.StartDialogue(CreatePages(boxing != null && boxing.IsBroken));
        }

        public void GrantAccess()
        {
            permissionGranted = true;
        }

        private static DialoguePage[] CreatePages(bool sandbagBroken)
        {
            string lin = GameLocalization.Get("apartment.speaker.linChunbao");
            if (!sandbagBroken)
            {
                return new[]
                {
                    new DialoguePage(lin, GameLocalization.Get("apartment.chunbaoBedroom.door.boundary")),
                    new DialoguePage("主角", GameLocalization.Get("apartment.chunbaoBedroom.door.clueQuestion")),
                    new DialoguePage(lin, GameLocalization.Get("apartment.chunbaoBedroom.door.beforeSandbag"))
                };
            }

            return new[]
            {
                new DialoguePage(lin, GameLocalization.Get("apartment.chunbaoBedroom.door.boundary")),
                new DialoguePage("主角", GameLocalization.Get("apartment.chunbaoBedroom.door.clueQuestion")),
                new DialoguePage(lin, GameLocalization.Get("apartment.chunbaoBedroom.door.afterSandbag.1")),
                new DialoguePage(lin, GameLocalization.Get("apartment.chunbaoBedroom.door.afterSandbag.2"))
            };
        }
    }
}
