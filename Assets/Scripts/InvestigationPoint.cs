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
        Flowerpot
    }

    public interface IInvestigationHandler
    {
        bool CanInvestigate(InvestigationKind kind);
        bool TryInvestigate(InvestigationKind kind);
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
