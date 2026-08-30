using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class InventoryInteractionTests
    {
        private readonly System.Collections.Generic.List<GameObject> createdObjects =
            new System.Collections.Generic.List<GameObject>();

        [TearDown]
        public void TearDown()
        {
            foreach (GameObject createdObject in createdObjects)
            {
                if (createdObject != null)
                {
                    Object.DestroyImmediate(createdObject);
                }
            }

            createdObjects.Clear();
        }

        [Test]
        public void CalculateDropPosition_PlacesItemAtRequestedDistanceInFacingDirection()
        {
            Vector2 position = PlayerInventory.CalculateDropPosition(
                new Vector2(2f, 3f), Vector2.right, 1.2f);

            Assert.That(position.x, Is.EqualTo(3.2f).Within(0.0001f));
            Assert.That(position.y, Is.EqualTo(3f).Within(0.0001f));
        }

        [Test]
        public void CalculateDropPosition_DefaultsDownWhenFacingIsZero()
        {
            Vector2 position = PlayerInventory.CalculateDropPosition(
                new Vector2(2f, 3f), Vector2.zero, 1.2f);

            Assert.That(position.x, Is.EqualTo(2f).Within(0.0001f));
            Assert.That(position.y, Is.EqualTo(1.8f).Within(0.0001f));
        }

        [Test]
        public void TriggeredNearbyItems_PickUpNearestAndDisableSameInstance()
        {
            PlayerInventory inventory = CreatePlayer();
            WorldItem farther = CreateItem("Farther", new Vector2(2f, 0f));
            WorldItem nearer = CreateItem("Nearer", new Vector2(1f, 0f));
            Collider2D playerCollider = inventory.GetComponent<Collider2D>();
            EditModeTestLifecycle.Invoke(farther, "OnTriggerEnter2D", playerCollider);
            EditModeTestLifecycle.Invoke(nearer, "OnTriggerEnter2D", playerCollider);

            inventory.TryPickUpNearest();

            Assert.That(inventory.GetSlotItem(0), Is.SameAs(nearer));
            Assert.That(nearer.gameObject.activeSelf, Is.False);
            Assert.That(farther.gameObject.activeSelf, Is.True);
        }

        [Test]
        public void PickupDisabledItem_IsSkippedUntilEnabled()
        {
            PlayerInventory inventory = CreatePlayer();
            WorldItem chair = CreateItem("Chair", new Vector2(0.5f, 0f));
            WorldItem card = CreateItem("Card", new Vector2(1f, 0f));
            chair.SetPickupEnabled(false);
            inventory.RegisterNearby(chair);
            inventory.RegisterNearby(card);

            Assert.That(inventory.TryPickUpNearest(), Is.True);
            Assert.That(inventory.GetSlotItem(0), Is.SameAs(card));
            Assert.That(chair.gameObject.activeSelf, Is.True);

            chair.SetPickupEnabled(true);
            Assert.That(inventory.TryPickUpNearest(), Is.True);
            Assert.That(inventory.GetSlotItem(1), Is.SameAs(chair));
        }

        [Test]
        public void PickUp_RaisesEventForTheSameWorldItem()
        {
            PlayerInventory inventory = CreatePlayer();
            WorldItem item = CreateItem("Library Card", Vector2.zero);
            WorldItem pickedUp = null;
            item.PickedUp += value => pickedUp = value;
            inventory.RegisterNearby(item);

            Assert.That(inventory.TryPickUpNearest(), Is.True);

            Assert.That(pickedUp, Is.SameAs(item));
        }

        [Test]
        public void FullInventory_DoesNotDisableOrOverwriteFourthItem()
        {
            PlayerInventory inventory = CreatePlayer();
            WorldItem[] stored = new WorldItem[3];
            for (int index = 0; index < stored.Length; index++)
            {
                stored[index] = CreateItem("Stored " + index, Vector2.right);
                inventory.RegisterNearby(stored[index]);
                inventory.TryPickUpNearest();
            }

            WorldItem fourth = CreateItem("Fourth", Vector2.right);
            inventory.RegisterNearby(fourth);
            inventory.TryPickUpNearest();

            Assert.That(fourth.gameObject.activeSelf, Is.True);
            Assert.That(inventory.GetSlotItem(0), Is.SameAs(stored[0]));
            Assert.That(inventory.GetSlotItem(1), Is.SameAs(stored[1]));
            Assert.That(inventory.GetSlotItem(2), Is.SameAs(stored[2]));
        }

        [Test]
        public void DropSelected_ReactivatesSameItemBelowUnmovedPlayer()
        {
            PlayerInventory inventory = CreatePlayer();
            inventory.transform.position = new Vector2(2f, 3f);
            WorldItem item = CreateItem("Card", new Vector2(2f, 3f));
            inventory.RegisterNearby(item);
            inventory.TryPickUpNearest();

            EditModeTestLifecycle.Invoke(inventory, "DropSelected");

            Assert.That(item.gameObject.activeSelf, Is.True);
            Assert.That(item.transform.position.x, Is.EqualTo(2f).Within(0.0001f));
            Assert.That(item.transform.position.y, Is.EqualTo(1.8f).Within(0.0001f));
            Assert.That(inventory.GetSlotItem(0), Is.Null);
        }

        [Test]
        public void DropSelected_OnEmptySlotLeavesInventoryEmpty()
        {
            PlayerInventory inventory = CreatePlayer();

            EditModeTestLifecycle.Invoke(inventory, "DropSelected");

            Assert.That(inventory.GetSlotItem(0), Is.Null);
            Assert.That(inventory.GetSlotItem(1), Is.Null);
            Assert.That(inventory.GetSlotItem(2), Is.Null);
        }

        private PlayerInventory CreatePlayer()
        {
            GameObject player = new GameObject("Player");
            createdObjects.Add(player);
            player.AddComponent<CircleCollider2D>();
            player.AddComponent<MainCharacterMovement>();
            PlayerInventory inventory = player.AddComponent<PlayerInventory>();
            EditModeTestLifecycle.Invoke(inventory, "Awake");
            return inventory;
        }

        private WorldItem CreateItem(string name, Vector2 position)
        {
            GameObject item = new GameObject(name);
            createdObjects.Add(item);
            item.transform.position = position;
            return item.AddComponent<WorldItem>();
        }
    }
}
