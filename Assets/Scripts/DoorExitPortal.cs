using UnityEngine;

namespace RPG
{
    public class DoorExitPortal : MonoBehaviour
    {
        [SerializeField] private DoorController door;
        [SerializeField] private string destinationScene;
        [SerializeField] private string destinationId;

        private bool waitingForPlayerExit;

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

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponentInParent<PlayerInteraction>() != null)
            {
                waitingForPlayerExit = false;
            }
        }

        public static void SuppressPortalsAtDestination(Rigidbody2D player)
        {
            if (player == null) return;
            Collider2D playerCollider = player.GetComponentInChildren<Collider2D>();
            if (playerCollider == null) return;
            foreach (DoorExitPortal portal in FindObjectsOfType<DoorExitPortal>())
            {
                Collider2D portalCollider = portal.GetComponent<Collider2D>();
                if (portalCollider != null
                    && portalCollider.bounds.Intersects(playerCollider.bounds))
                {
                    portal.waitingForPlayerExit = true;
                }
            }
        }

        private void TryTransition(Collider2D other)
        {
            PlayerInteraction player = other.GetComponentInParent<PlayerInteraction>();
            if (waitingForPlayerExit
                || !CanTransition(door != null && door.IsFullyOpen, player != null))
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
