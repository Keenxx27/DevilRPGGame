using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class PlayerInteractionTests
    {
        private readonly List<GameObject> createdObjects = new List<GameObject>();

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
        public void DoorInRange_ConsumesInteractionBeforePickup()
        {
            PlayerInteraction interaction = CreatePlayer(out PlayerInventory inventory);
            DoorController door = CreateDoor("Door", Vector2.right);
            WorldItem card = CreateItem(new Vector2(0.5f, 0f));
            inventory.RegisterNearby(card);
            interaction.RegisterDoor(door);

            Assert.That(interaction.TryInteract(), Is.True);
            Assert.That(door.State, Is.EqualTo(DoorState.Opening));
            Assert.That(card.gameObject.activeSelf, Is.True);
            Assert.That(inventory.GetSlotItem(0), Is.Null);
        }

        [Test]
        public void NoDoorInRange_UsesExistingPickup()
        {
            PlayerInteraction interaction = CreatePlayer(out PlayerInventory inventory);
            WorldItem card = CreateItem(Vector2.right);
            inventory.RegisterNearby(card);

            Assert.That(interaction.TryInteract(), Is.True);
            Assert.That(inventory.GetSlotItem(0), Is.SameAs(card));
            Assert.That(card.gameObject.activeSelf, Is.False);
        }

        [Test]
        public void MultipleDoorsInRange_TogglesNearestDoorOnly()
        {
            PlayerInteraction interaction = CreatePlayer(out _);
            DoorController farther = CreateDoor("Farther Door", new Vector2(2f, 0f));
            DoorController nearer = CreateDoor("Nearer Door", new Vector2(1f, 0f));
            interaction.RegisterDoor(farther);
            interaction.RegisterDoor(nearer);

            interaction.TryInteract();

            Assert.That(nearer.State, Is.EqualTo(DoorState.Opening));
            Assert.That(farther.State, Is.EqualTo(DoorState.Closed));
        }

        [Test]
        public void DoorConsumesInteractionBeforeInvestigationAndPickup()
        {
            PlayerInteraction interaction = CreatePlayer(out PlayerInventory inventory);
            DoorController door = CreateDoor("Door", Vector2.right);
            FakeInvestigationHandler handler = CreateInvestigation(
                new Vector2(0.5f, 0f), out InvestigationPoint point);
            WorldItem item = CreateItem(new Vector2(0.25f, 0f));
            interaction.RegisterDoor(door);
            interaction.RegisterInvestigation(point);
            inventory.RegisterNearby(item);

            Assert.That(interaction.TryInteract(), Is.True);

            Assert.That(door.State, Is.EqualTo(DoorState.Opening));
            Assert.That(handler.Calls, Is.Zero);
            Assert.That(inventory.GetSlotItem(0), Is.Null);
        }

        [Test]
        public void InvestigationConsumesInteractionBeforePickup()
        {
            PlayerInteraction interaction = CreatePlayer(out PlayerInventory inventory);
            FakeInvestigationHandler handler = CreateInvestigation(
                Vector2.right, out InvestigationPoint point);
            WorldItem item = CreateItem(new Vector2(0.5f, 0f));
            interaction.RegisterInvestigation(point);
            inventory.RegisterNearby(item);

            Assert.That(interaction.TryInteract(), Is.True);

            Assert.That(handler.Calls, Is.EqualTo(1));
            Assert.That(inventory.GetSlotItem(0), Is.Null);
            Assert.That(item.gameObject.activeSelf, Is.True);
        }

        [Test]
        public void FailedInvestigationStillConsumesInteractionBeforePickup()
        {
            PlayerInteraction interaction = CreatePlayer(out PlayerInventory inventory);
            FakeInvestigationHandler handler = CreateInvestigation(
                Vector2.right, out InvestigationPoint point);
            handler.TryResult = false;
            WorldItem item = CreateItem(new Vector2(0.5f, 0f));
            interaction.RegisterInvestigation(point);
            inventory.RegisterNearby(item);

            Assert.That(interaction.TryInteract(), Is.True);

            Assert.That(handler.Calls, Is.EqualTo(1));
            Assert.That(inventory.GetSlotItem(0), Is.Null);
            Assert.That(item.gameObject.activeSelf, Is.True);
        }

        [Test]
        public void UnavailableInvestigationFallsBackToPickup()
        {
            PlayerInteraction interaction = CreatePlayer(out PlayerInventory inventory);
            FakeInvestigationHandler handler = CreateInvestigation(
                Vector2.right, out InvestigationPoint point);
            handler.Available = false;
            WorldItem item = CreateItem(new Vector2(0.5f, 0f));
            interaction.RegisterInvestigation(point);
            inventory.RegisterNearby(item);

            Assert.That(interaction.TryInteract(), Is.True);

            Assert.That(handler.Calls, Is.Zero);
            Assert.That(inventory.GetSlotItem(0), Is.SameAs(item));
        }

        [Test]
        public void MultipleInvestigationsUseNearestAvailablePoint()
        {
            PlayerInteraction interaction = CreatePlayer(out _);
            FakeInvestigationHandler farther = CreateInvestigation(
                new Vector2(2f, 0f), out InvestigationPoint fartherPoint);
            FakeInvestigationHandler nearer = CreateInvestigation(
                new Vector2(1f, 0f), out InvestigationPoint nearerPoint);
            interaction.RegisterInvestigation(fartherPoint);
            interaction.RegisterInvestigation(nearerPoint);

            Assert.That(interaction.TryInteract(), Is.True);

            Assert.That(nearer.Calls, Is.EqualTo(1));
            Assert.That(farther.Calls, Is.Zero);
        }

        private PlayerInteraction CreatePlayer(out PlayerInventory inventory)
        {
            GameObject player = CreateObject("Player", false);
            player.AddComponent<CircleCollider2D>();
            player.AddComponent<MainCharacterMovement>();
            inventory = player.AddComponent<PlayerInventory>();
            PlayerInteraction interaction = player.AddComponent<PlayerInteraction>();
            player.SetActive(true);
            EditModeTestLifecycle.Invoke(inventory, "Awake");
            EditModeTestLifecycle.Invoke(interaction, "Awake");
            return interaction;
        }

        private DoorController CreateDoor(string name, Vector2 position)
        {
            GameObject doorObject = CreateObject(name, false);
            doorObject.transform.position = position;
            BoxCollider2D blocker = doorObject.AddComponent<BoxCollider2D>();
            DoorController door = doorObject.AddComponent<DoorController>();
            SetPrivateField(door, "blockingCollider", blocker);
            doorObject.SetActive(true);
            EditModeTestLifecycle.Invoke(door, "Awake");
            return door;
        }

        private WorldItem CreateItem(Vector2 position)
        {
            GameObject itemObject = CreateObject("Library Card");
            itemObject.transform.position = position;
            return itemObject.AddComponent<WorldItem>();
        }

        private FakeInvestigationHandler CreateInvestigation(
            Vector2 position, out InvestigationPoint point)
        {
            GameObject investigationObject = CreateObject("Investigation", false);
            investigationObject.transform.position = position;
            investigationObject.AddComponent<BoxCollider2D>().isTrigger = true;
            FakeInvestigationHandler handler =
                investigationObject.AddComponent<FakeInvestigationHandler>();
            point = investigationObject.AddComponent<InvestigationPoint>();
            SetPrivateField(point, "handler", handler);
            investigationObject.SetActive(true);
            return handler;
        }

        private GameObject CreateObject(string name, bool active = true)
        {
            var gameObject = new GameObject(name);
            gameObject.SetActive(active);
            createdObjects.Add(gameObject);
            return gameObject;
        }

        private static void SetPrivateField<T>(object target, string name, T value)
        {
            FieldInfo field = target.GetType().GetField(
                name, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(target, value);
        }

        private sealed class FakeInvestigationHandler : MonoBehaviour, IInvestigationHandler
        {
            public bool Available { get; set; } = true;
            public bool TryResult { get; set; } = true;
            public int Calls { get; private set; }

            public bool CanInvestigate(InvestigationKind kind)
            {
                return Available;
            }

            public bool TryInvestigate(InvestigationKind kind)
            {
                Calls++;
                return TryResult;
            }
        }
    }
}
