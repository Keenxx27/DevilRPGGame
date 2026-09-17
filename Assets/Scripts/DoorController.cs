using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public interface IDoorInteractionOverride
    {
        bool TryInteract(PlayerInteraction player);
    }

    public class DoorController : MonoBehaviour
    {
        private static readonly Dictionary<string, bool> DoorGroupStates =
            new Dictionary<string, bool>();

        [SerializeField] private Transform rotatingTransform;
        [SerializeField] private Collider2D blockingCollider;
        [SerializeField] private float openAngleDegrees = 90f;
        [SerializeField, Min(0f)] private float degreesPerSecond = 180f;
        [SerializeField] private bool animateVisual = true;
        [SerializeField] private string requiredItemDisplayName;
        [SerializeField, TextArea] private string missingItemDialogue;
        [SerializeField] private bool requireSelectedItem;
        [SerializeField] private bool consumeRequiredItem;
        [SerializeField] private bool keepOpenWhenUnlocked;
        [SerializeField] private string doorGroup;

        private DoorMotion motion;
        private bool unlocked;

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
            if (!string.IsNullOrWhiteSpace(doorGroup))
            {
                if (DoorGroupStates.TryGetValue(doorGroup, out bool shouldOpen))
                {
                    SetDoorTarget(shouldOpen, false);
                }
                else
                {
                    DoorGroupStates.Add(doorGroup, false);
                }
            }
        }

        private void Update()
        {
            SyncDoorGroup();
            Advance(Time.deltaTime);
        }

        public bool ToggleDoor()
        {
            return SetDoorTarget(!TargetsOpen(), true);
        }

        public bool TryInteract(PlayerInteraction player)
        {
            IDoorInteractionOverride interactionOverride = GetComponent(typeof(IDoorInteractionOverride))
                as IDoorInteractionOverride;
            if (interactionOverride != null)
            {
                return interactionOverride.TryInteract(player);
            }

            if (string.IsNullOrWhiteSpace(requiredItemDisplayName))
            {
                return ToggleDoor();
            }

            if (unlocked)
            {
                return keepOpenWhenUnlocked ? OpenDoor() : ToggleDoor();
            }

            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (!requireSelectedItem
                && inventory?.HasItem(requiredItemDisplayName) == true)
            {
                return ToggleDoor();
            }

            if (requireSelectedItem && inventory != null
                && inventory.SelectedItem != null
                && inventory.SelectedItem.DisplayName == requiredItemDisplayName)
            {
                if (consumeRequiredItem
                    && !inventory.TryRemoveSelected(requiredItemDisplayName, out _))
                {
                    return false;
                }

                unlocked = true;
                return true;
            }

            DialogueController dialogue = player.GetComponent<DialogueController>();
            return dialogue != null
                && dialogue.StartDialogue(new[]
                {
                    new DialoguePage("主角", missingItemDialogue)
                });
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

        private bool OpenDoor()
        {
            return SetDoorTarget(true, true);
        }

        private void SyncDoorGroup()
        {
            if (string.IsNullOrWhiteSpace(doorGroup)
                || !DoorGroupStates.TryGetValue(doorGroup, out bool shouldOpen))
            {
                return;
            }

            SetDoorTarget(shouldOpen, false);
        }

        private bool SetDoorTarget(bool shouldOpen, bool publishToGroup)
        {
            if (motion == null) return false;
            if (publishToGroup && !string.IsNullOrWhiteSpace(doorGroup))
            {
                DoorGroupStates[doorGroup] = shouldOpen;
            }

            if (TargetsOpen() == shouldOpen) return true;
            if (!motion.Toggle()) return false;

            ApplyMotion();
            return true;
        }

        private bool TargetsOpen()
        {
            return motion != null && (motion.State == DoorState.Open
                || motion.State == DoorState.Opening);
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
            if (animateVisual)
            {
                rotatingTransform.localRotation = Quaternion.Euler(0f, 0f, motion.CurrentAngle);
            }
            blockingCollider.enabled = motion.BlocksPassage;
        }
    }
}
