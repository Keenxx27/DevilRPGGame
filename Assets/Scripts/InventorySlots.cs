using System;

namespace RPG
{
    public sealed class InventorySlots<T> where T : class
    {
        private readonly T[] items;

        public int Capacity => items.Length;
        public int SelectedIndex { get; private set; }

        public InventorySlots(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            items = new T[capacity];
        }

        public bool Select(int index)
        {
            if (index < 0 || index >= items.Length)
            {
                return false;
            }

            SelectedIndex = index;
            return true;
        }

        public bool TryAdd(T item, out int slotIndex)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            for (int index = 0; index < items.Length; index++)
            {
                if (items[index] != null)
                {
                    continue;
                }

                items[index] = item;
                slotIndex = index;
                return true;
            }

            slotIndex = -1;
            return false;
        }

        public T Get(int index)
        {
            return items[index];
        }

        public T RemoveSelected()
        {
            T item = items[SelectedIndex];
            items[SelectedIndex] = null;
            return item;
        }

        public T Remove(int index)
        {
            if (index < 0 || index >= items.Length)
            {
                return null;
            }

            T item = items[index];
            items[index] = null;
            return item;
        }
    }
}
