using UnityEngine;

namespace RPG
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private Transform rotatingTransform;
        [SerializeField] private Collider2D blockingCollider;
        [SerializeField] private float openAngleDegrees = 90f;
        [SerializeField, Min(0f)] private float degreesPerSecond = 180f;

        private DoorMotion motion;

        public bool IsFullyOpen => motion != null && motion.IsFullyOpen;
        public DoorState State => motion != null ? motion.State : DoorState.Closed;

        private void Awake()
        {
            if (rotatingTransform == null)
            {
                rotatingTransform = transform;
            }

            if (blockingCollider == null)
            {
                Debug.LogError("DoorController requires a blocking collider.", this);
                enabled = false;
                return;
            }

            float closedAngle = rotatingTransform.localEulerAngles.z;
            motion = new DoorMotion(closedAngle, closedAngle + openAngleDegrees);
            ApplyMotion();
        }

        private void Update()
        {
            Advance(Time.deltaTime);
        }

        public bool ToggleDoor()
        {
            if (motion == null || !motion.Toggle())
            {
                return false;
            }

            ApplyMotion();
            return true;
        }

        public void Advance(float deltaTime)
        {
            if (motion == null)
            {
                return;
            }

            motion.Step(degreesPerSecond, deltaTime);
            ApplyMotion();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerInteraction interaction = other.GetComponentInParent<PlayerInteraction>();
            if (interaction != null)
            {
                interaction.RegisterDoor(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerInteraction interaction = other.GetComponentInParent<PlayerInteraction>();
            if (interaction != null)
            {
                interaction.UnregisterDoor(this);
            }
        }

        private void ApplyMotion()
        {
            rotatingTransform.localRotation = Quaternion.Euler(0f, 0f, motion.CurrentAngle);
            blockingCollider.enabled = motion.BlocksPassage;
        }
    }
}
