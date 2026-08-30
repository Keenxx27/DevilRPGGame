using NUnit.Framework;
using RPG;

namespace RPG.Tests
{
    public class InventorySlotsTests
    {
        [Test]
        public void NewInventory_HasRequestedCapacityAndSelectsFirstSlot()
        {
            var slots = new InventorySlots<object>(3);

            Assert.That(slots.Capacity, Is.EqualTo(3));
            Assert.That(slots.SelectedIndex, Is.Zero);
        }

        [Test]
        public void TryAdd_UsesLeftmostEmptySlot()
        {
            var slots = new InventorySlots<object>(3);
            var first = new object();
            var second = new object();
            var replacement = new object();

            Assert.That(slots.TryAdd(first, out int firstIndex), Is.True);
            Assert.That(firstIndex, Is.Zero);
            Assert.That(slots.TryAdd(second, out _), Is.True);
            slots.Select(0);
            Assert.That(slots.RemoveSelected(), Is.SameAs(first));

            Assert.That(slots.TryAdd(replacement, out int replacementIndex), Is.True);
            Assert.That(replacementIndex, Is.Zero);
            Assert.That(slots.Get(0), Is.SameAs(replacement));
        }

        [Test]
        public void TryAdd_WhenFullReturnsFalseWithoutChangingContents()
        {
            var slots = new InventorySlots<object>(3);
            var first = new object();
            var second = new object();
            var third = new object();
            slots.TryAdd(first, out _);
            slots.TryAdd(second, out _);
            slots.TryAdd(third, out _);

            Assert.That(slots.TryAdd(new object(), out int slotIndex), Is.False);
            Assert.That(slotIndex, Is.EqualTo(-1));
            Assert.That(slots.Get(0), Is.SameAs(first));
            Assert.That(slots.Get(1), Is.SameAs(second));
            Assert.That(slots.Get(2), Is.SameAs(third));
        }

        [Test]
        public void Select_ChangesSelectedSlotAndRejectsOutOfRangeIndices()
        {
            var slots = new InventorySlots<object>(3);

            Assert.That(slots.Select(2), Is.True);
            Assert.That(slots.SelectedIndex, Is.EqualTo(2));
            Assert.That(slots.Select(-1), Is.False);
            Assert.That(slots.Select(3), Is.False);
            Assert.That(slots.SelectedIndex, Is.EqualTo(2));
        }

        [Test]
        public void RemoveSelected_ReturnsSelectedItemAndClearsItsSlot()
        {
            var slots = new InventorySlots<object>(3);
            var first = new object();
            var second = new object();
            var third = new object();
            slots.TryAdd(first, out _);
            slots.TryAdd(second, out _);
            slots.TryAdd(third, out _);
            slots.Select(2);

            Assert.That(slots.RemoveSelected(), Is.SameAs(third));
            Assert.That(slots.Get(2), Is.Null);
            slots.Select(2);
            Assert.That(slots.RemoveSelected(), Is.Null);
        }
    }
}
