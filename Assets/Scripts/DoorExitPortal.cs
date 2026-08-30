using UnityEngine;

namespace RPG
{
    public class DoorExitPortal : MonoBehaviour
    {
        [SerializeField] private DoorController door;
        [SerializeField] private string destinationScene;
        [SerializeField] private string destinationId;

        private void Awake()
        {
            if (door == null || string.IsNullOrWhiteSpace(destinationId))
            {
                Debug.LogError(
                    "DoorExitPortal requires a door and destination id.", this);
                enabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryTransition(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryTransition(other);
        }

        private void TryTransition(Collider2D other)
        {
            PlayerInteraction player = other.GetComponentInParent<PlayerInteraction>();
            if (!CanTransition(door != null && door.IsFullyOpen, player != null))
            {
                return;
            }

            MapTransitionService transition = player.GetComponent<MapTransitionService>();
            if (transition == null)
            {
                Debug.LogError("Player requires MapTransitionService.", player);
                return;
            }

            transition.RequestTransition(destinationScene, destinationId);
        }

        public static bool CanTransition(bool doorFullyOpen, bool isPlayer)
        {
            return doorFullyOpen && isPlayer;
        }
    }
}
