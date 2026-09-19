using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public enum InvestigationKind
    {
        StorageCabinet,
        Podium,
        TeacherDesk,
        Blackboard,
        Flowerpot,
        LibraryBookcase,
        LibraryChair,
        LibraryWallBrick,
        LibraryReadingTable,
        LibraryCounter,
        JanitorRestRoomDoor,
        StorageShelfLeft,
        StorageShelfRight,
        StorageCrateNorth,
        StorageCrateSouth,
        StorageBroom,
        LibraryCompletePage,
        StairwellDoor,
        SchoolGate,
        ApartmentDoor,
        KitchenFoodCabinet,
        LivingRoomLinChunbao,
        LivingRoomSandbag,
        LivingRoomLowCabinet,
        LivingRoomSofa,
        LivingRoomCoffeeTable,
        KitchenStove,
        PasswordNote,
        BathroomVent,
        BathroomMedicineCabinet,
        BathroomToilet,
        LinBedroomDesk,
        LinBedroomChair,
        LinBedroomWardrobe,
        LinBedroomBed,
        LinBedroomPhone,
        ProtagonistBedroomBed,
        ProtagonistBedsideTable,
        ProtagonistPlasterBoxes,
        SuburbanRoadTransition,
        RubbleSwitch,
        RubbleLoreNote
    }

    public interface IInvestigationHandler
    {
        bool CanInvestigate(InvestigationKind kind);
        bool TryInvestigate(InvestigationKind kind);
    }

    public interface ISelectedItemUseHandler
    {
        bool TryUseSelectedItem(InvestigationKind kind);
    }

    public interface IRightClickSelectedItemUseHandler
    {
        bool TryUseSelectedItemWithRightClick(InvestigationKind kind);
    }

    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class InvestigationPoint : MonoBehaviour
    {
        [SerializeField] private InvestigationKind kind;
        [SerializeField] private MonoBehaviour handler;

        private readonly List<PlayerInteraction> registeredPlayers =
            new List<PlayerInteraction>();

        public InvestigationKind Kind => kind;
        public bool IsAvailable => handler is IInvestigationHandler target
            && target.CanInvestigate(kind);

        public bool TryInvestigate()
        {
            return handler is IInvestigationHandler target
                && target.TryInvestigate(kind);
        }

        public bool TryUseSelectedItem()
        {
            return handler is ISelectedItemUseHandler target
                && target.TryUseSelectedItem(kind);
        }

        public bool TryUseSelectedItemWithRightClick()
        {
            return handler is IRightClickSelectedItemUseHandler target
                && target.TryUseSelectedItemWithRightClick(kind);
        }

        public void Configure(InvestigationKind investigationKind, MonoBehaviour investigationHandler)
        {
            kind = investigationKind;
            handler = investigationHandler;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerInteraction interaction = other.GetComponentInParent<PlayerInteraction>();
            if (interaction == null || registeredPlayers.Contains(interaction))
            {
                return;
            }

            registeredPlayers.Add(interaction);
            interaction.RegisterInvestigation(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerInteraction interaction = other.GetComponentInParent<PlayerInteraction>();
            if (interaction == null)
            {
                return;
            }

            registeredPlayers.Remove(interaction);
            interaction.UnregisterInvestigation(this);
        }

        private void OnDisable()
        {
            foreach (PlayerInteraction interaction in registeredPlayers)
            {
                if (interaction != null)
                {
                    interaction.UnregisterInvestigation(this);
                }
            }

            registeredPlayers.Clear();
        }
    }
}
