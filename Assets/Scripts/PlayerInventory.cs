using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(MainCharacterMovement))]
    public class PlayerInventory : MonoBehaviour
    {
        private const int SlotCount = 3;
        private const float DropDistance = 1.2f;

        private static readonly DialoguePage[] FullInventoryPages =
        {
            new DialoguePage("主角", "东西太满了，最好先把暂时用不到的东西放下。")
        };

        private readonly List<WorldItem> nearbyItems = new List<WorldItem>();
        private InventorySlots<WorldItem> slots;
        private MainCharacterMovement movement;

        public int SelectedSlotIndex => slots.SelectedIndex;
        public WorldItem SelectedItem => slots.Get(slots.SelectedIndex);

        private void Awake()
        {
            slots = new InventorySlots<WorldItem>(SlotCount);
            movement = GetComponent<MainCharacterMovement>();
            if (GetComponent<PlasterKnifeCrafting>() == null)
            {
                gameObject.AddComponent<PlasterKnifeCrafting>();
            }
        }

        private void Update()
        {
            HandleSlotSelection();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayerInteraction interaction = GetComponent<PlayerInteraction>();
                if (interaction == null || !interaction.TryUseSelectedItemOnReadingTable())
                {
                    DropSelected();
                }
            }
        }

        public WorldItem GetSlotItem(int index)
        {
            return slots.Get(index);
        }

        public bool HasItem(string displayName)
        {
            for (int index = 0; index < slots.Capacity; index++)
            {
                WorldItem item = slots.Get(index);
                if (item != null && item.DisplayName == displayName)
                {
                    return true;
                }
            }

            return false;
        }

        public int CountItems(string displayName)
        {
            int count = 0;
            for (int index = 0; index < slots.Capacity; index++)
            {
                WorldItem item = slots.Get(index);
                if (item != null && item.DisplayName == displayName)
                {
                    count++;
                }
            }

            return count;
        }

        public bool TryRemoveFirst(string displayName, out WorldItem item)
        {
            for (int index = 0; index < slots.Capacity; index++)
            {
                WorldItem candidate = slots.Get(index);
                if (candidate != null && candidate.DisplayName == displayName)
                {
                    item = slots.Remove(index);
                    return true;
                }
            }

            item = null;
            return false;
        }

        public bool TryRemoveSelected(string displayName, out WorldItem item)
        {
            WorldItem selected = SelectedItem;
            if (selected == null || selected.DisplayName != displayName)
            {
                item = null;
                return false;
            }

            item = slots.RemoveSelected();
            return true;
        }

        public bool TryStore(WorldItem item)
        {
            if (item == null || !item.CanPickUp || !slots.TryAdd(item, out _))
            {
                return false;
            }

            nearbyItems.Remove(item);
            item.PickUp();
            return true;
        }

        public void RegisterNearby(WorldItem item)
        {
            if (item != null && !nearbyItems.Contains(item))
            {
                nearbyItems.Add(item);
            }
        }

        public void UnregisterNearby(WorldItem item)
        {
            nearbyItems.Remove(item);
        }

        public void ClearNearbyItems()
        {
            nearbyItems.Clear();
        }

        public void ClearAllItems()
        {
            for (int index = 0; index < slots.Capacity; index++)
            {
                WorldItem item = slots.Remove(index);
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }

            nearbyItems.Clear();
        }

        public static Vector2 CalculateDropPosition(Vector2 playerPosition, Vector2 facing, float distance)
        {
            Vector2 direction = facing.sqrMagnitude > 0f ? facing.normalized : Vector2.down;
            return playerPosition + direction * distance;
        }

        private void HandleSlotSelection()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                slots.Select(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                slots.Select(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                slots.Select(2);
            }
        }

        public bool TryPickUpNearest()
        {
            WorldItem nearest = FindNearestActiveItem();
            if (nearest == null)
            {
                return false;
            }

            if (TryStore(nearest))
            {
                return true;
            }

            DialogueController dialogue = GetComponent<DialogueController>();
            return dialogue != null && dialogue.StartDialogue(FullInventoryPages);
        }

        private WorldItem FindNearestActiveItem()
        {
            WorldItem nearest = null;
            float nearestDistance = float.PositiveInfinity;

            for (int index = nearbyItems.Count - 1; index >= 0; index--)
            {
                WorldItem item = nearbyItems[index];
                if (item == null || !item.gameObject.activeInHierarchy)
                {
                    nearbyItems.RemoveAt(index);
                    continue;
                }

                if (!item.CanPickUp)
                {
                    continue;
                }

                float distance = (item.transform.position - transform.position).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearest = item;
                    nearestDistance = distance;
                }
            }

            return nearest;
        }

        private void DropSelected()
        {
            WorldItem item = slots.RemoveSelected();
            if (item == null)
            {
                return;
            }

            ThrowableChair chair = item.GetComponent<ThrowableChair>();
            if (chair != null
                && chair.BeginThrow(transform.position, movement.FacingDirection))
            {
                return;
            }

            Vector2 position = CalculateDropPosition(transform.position, movement.FacingDirection, DropDistance);
            item.Drop(position);
        }
    }
}
