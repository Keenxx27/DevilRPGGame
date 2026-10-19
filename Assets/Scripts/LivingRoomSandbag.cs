using UnityEngine;

namespace RPG
{
    public sealed class LivingRoomSandbag : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        [SerializeField] private SandbagBoxingMinigame boxing;

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.LivingRoomSandbag && (boxing == null || !boxing.IsBroken)
                && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            LinChunbaoLivingRoomDialogue lin = FindObjectOfType<LinChunbaoLivingRoomDialogue>();
            return dialogue.StartDialogue(lin != null && lin.IsAtSofa
                ? new[] { new DialoguePage(GameLocalization.Get("apartment.speaker.linChunbao"),
                    GameLocalization.Get("apartment.livingRoom.sandbag.ready")) }
                : new[]
                {
                    new DialoguePage("主角", GameLocalization.Get("apartment.livingRoom.sandbag.1")),
                    new DialoguePage("主角", GameLocalization.Get("apartment.livingRoom.sandbag.2"))
                });
        }

        public bool TryUseSelectedItem(InvestigationKind kind)
        {
            if (kind != InvestigationKind.LivingRoomSandbag || boxing == null || boxing.IsBroken)
            {
                return false;
            }
            PlayerInteraction player = FindObjectOfType<PlayerInteraction>();
            return player?.GetComponent<PlayerInventory>()?.SelectedItem?.ItemId == "apartment.boxingGloves"
                && boxing.StartBoxing(player);
        }
    }
}
