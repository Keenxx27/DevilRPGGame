using UnityEngine;

namespace RPG
{
    public sealed class ApartmentDepartureDialogue : MonoBehaviour
    {
        private const string DriedMeatId = "apartment.driedMeat";
        private bool hasPlayed;

        private void Update()
        {
            if (hasPlayed) return;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            LinChunbaoLivingRoomDialogue linChunbao = FindObjectOfType<LinChunbaoLivingRoomDialogue>();
            if (dialogue == null || inventory == null || linChunbao == null || dialogue.IsPlaying
                || !linChunbao.IsAtSofa || !inventory.HasItemById(DriedMeatId)) return;

            Vector2 position = dialogue.transform.position;
            if (position.x < 132f || position.x > 148f || position.y < -5f || position.y > 5f)
            {
                return;
            }

            hasPlayed = dialogue.StartDialogue(CreatePages(), linChunbao.MarkDepartureConversationComplete);
        }

        private static DialoguePage[] CreatePages()
        {
            string lin = GameLocalization.Get("apartment.speaker.linChunbao");
            return new[]
            {
                new DialoguePage(lin, GameLocalization.Get("apartment.departure.ready")),
                new DialoguePage("主角", GameLocalization.Get("apartment.departure.askLocation")),
                new DialoguePage(lin, GameLocalization.Get("apartment.departure.directions")),
                new DialoguePage("主角", GameLocalization.Get("apartment.departure.understood")),
                new DialoguePage(lin, GameLocalization.Get("apartment.departure.warningLead")),
                new DialoguePage("主角", GameLocalization.Get("apartment.departure.prompt")),
                new DialoguePage(lin, GameLocalization.Get("apartment.departure.gangWarning")),
                new DialoguePage("主角", GameLocalization.Get("apartment.departure.contactPromise"))
            };
        }
    }
}
