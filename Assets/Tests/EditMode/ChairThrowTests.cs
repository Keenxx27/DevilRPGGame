using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class ChairThrowTests
    {
        private readonly List<GameObject> created = new List<GameObject>();

        [TearDown]
        public void TearDown()
        {
            foreach (GameObject item in created)
            {
                if (item != null)
                {
                    Object.DestroyImmediate(item);
                }
            }
            created.Clear();
        }

        [Test]
        public void SelectedChair_QUseStartsFlightInsteadOfNormalDrop()
        {
            PlayerInventory inventory = CreatePlayer();
            ThrowableChair chair = CreateChair(Vector2.zero);
            inventory.RegisterNearby(chair.GetComponent<WorldItem>());
            Assert.That(inventory.TryPickUpNearest(), Is.True);

            EditModeTestLifecycle.Invoke(inventory, "DropSelected");

            Assert.That(chair.IsFlying, Is.True);
            Assert.That(inventory.GetSlotItem(0), Is.Null);
            Assert.That(chair.GetComponent<WorldItem>().CanPickUp, Is.False);
        }

        [Test]
        public void Chair_ReachesTwoUnitsInAboutPointThreeSecondsThenBecomesPickupable()
        {
            ThrowableChair chair = CreateChair(Vector2.zero);

            Assert.That(chair.BeginThrow(Vector2.right), Is.True);
            chair.AdvanceFlight(0.15f);
            Assert.That(chair.IsFlying, Is.True);
            Assert.That(chair.transform.position.x, Is.EqualTo(1f).Within(0.05f));

            chair.AdvanceFlight(0.15f);
            Assert.That(chair.IsFlying, Is.False);
            Assert.That(chair.transform.position.x, Is.EqualTo(2f).Within(0.05f));
            Assert.That(chair.GetComponent<WorldItem>().CanPickUp, Is.True);
        }

        [Test]
        public void Flowerpot_BreaksOnlyOnceAndOnlyForFlyingChair()
        {
            ThrowableChair chair = CreateChair(Vector2.zero);
            FlowerpotBreakable flowerpot = CreateFlowerpot(out GameObject intact,
                out GameObject broken, out GameObject card);
            int brokenCount = 0;
            flowerpot.Broken += () => brokenCount++;

            Assert.That(flowerpot.TryBreak(chair), Is.False);
            Assert.That(flowerpot.IsBroken, Is.False);

            chair.BeginThrow(Vector2.right);
            Assert.That(flowerpot.TryBreak(chair), Is.True);
            Assert.That(flowerpot.TryBreak(chair), Is.False);

            Assert.That(flowerpot.IsBroken, Is.True);
            Assert.That(brokenCount, Is.EqualTo(1));
            Assert.That(intact.activeSelf, Is.False);
            Assert.That(broken.activeSelf, Is.True);
            Assert.That(card.activeSelf, Is.True);
        }

        [Test]
        public void FlyingChairCollision_BreaksFlowerpotAndStopsBeforeMaximumDistance()
        {
            ThrowableChair chair = CreateChair(Vector2.zero);
            FlowerpotBreakable flowerpot = CreateFlowerpot(
                out _, out _, out _);
            flowerpot.transform.position = new Vector2(1.5f, 0f);
            Physics2D.SyncTransforms();

            chair.BeginThrow(Vector2.right);
            chair.AdvanceFlight(0.3f);

            Assert.That(flowerpot.IsBroken, Is.True);
            Assert.That(chair.IsFlying, Is.False);
            Assert.That(chair.transform.position.x, Is.LessThan(2f));
            Assert.That(chair.GetComponent<WorldItem>().CanPickUp, Is.True);
        }

        [Test]
        public void FlyingChairCollision_WithSolidDeskStopsAndBecomesPickupable()
        {
            ThrowableChair chair = CreateChair(Vector2.zero);
            GameObject desk = Track(new GameObject("Student Desk"));
            desk.transform.position = new Vector2(1.5f, 0f);
            desk.AddComponent<BoxCollider2D>();
            Physics2D.SyncTransforms();

            chair.BeginThrow(Vector2.right);
            chair.AdvanceFlight(0.3f);

            Assert.That(chair.IsFlying, Is.False);
            Assert.That(chair.transform.position.x, Is.LessThan(2f));
            Assert.That(chair.GetComponent<WorldItem>().CanPickUp, Is.True);
        }

        [Test]
        public void SolidObstacleBeforeFlowerpot_StopsChairWithoutBreakingPot()
        {
            ThrowableChair chair = CreateChair(Vector2.zero);
            GameObject wall = Track(new GameObject("Wall"));
            wall.transform.position = new Vector2(0.9f, 0f);
            wall.AddComponent<BoxCollider2D>();
            FlowerpotBreakable flowerpot = CreateFlowerpot(
                out _, out _, out _);
            flowerpot.transform.position = new Vector2(1.8f, 0f);
            Physics2D.SyncTransforms();

            chair.BeginThrow(Vector2.right);
            chair.AdvanceFlight(0.3f);

            Assert.That(chair.IsFlying, Is.False);
            Assert.That(chair.transform.position.x, Is.LessThan(1f));
            Assert.That(flowerpot.IsBroken, Is.False);
        }

        private PlayerInventory CreatePlayer()
        {
            GameObject player = Track(new GameObject("Player"));
            player.AddComponent<Rigidbody2D>();
            player.AddComponent<CircleCollider2D>();
            player.AddComponent<MainCharacterMovement>();
            PlayerInventory inventory = player.AddComponent<PlayerInventory>();
            EditModeTestLifecycle.Invoke(inventory, "Awake");
            return inventory;
        }

        private ThrowableChair CreateChair(Vector2 position)
        {
            GameObject chairObject = Track(new GameObject("Chair"));
            chairObject.transform.position = position;
            WorldItem item = chairObject.AddComponent<WorldItem>();
            SetField(item, "displayName", "Chair");
            EditModeTestLifecycle.Invoke(item, "Awake");
            ThrowableChair chair = chairObject.AddComponent<ThrowableChair>();
            EditModeTestLifecycle.Invoke(chair, "Awake");
            return chair;
        }

        private FlowerpotBreakable CreateFlowerpot(
            out GameObject intact, out GameObject broken, out GameObject card)
        {
            GameObject root = Track(new GameObject("Flowerpot"));
            BoxCollider2D collider = root.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            intact = Track(new GameObject("Intact"));
            broken = Track(new GameObject("Broken"));
            card = Track(new GameObject("Library Card"));
            intact.transform.SetParent(root.transform);
            broken.transform.SetParent(root.transform);
            broken.SetActive(false);
            card.SetActive(false);
            FlowerpotBreakable flowerpot = root.AddComponent<FlowerpotBreakable>();
            SetField(flowerpot, "intactVisual", intact);
            SetField(flowerpot, "brokenVisual", broken);
            SetField(flowerpot, "libraryCard", card);
            return flowerpot;
        }

        private GameObject Track(GameObject item)
        {
            created.Add(item);
            return item;
        }

        private static void SetField(object target, string fieldName, object value)
        {
            target.GetType().GetField(fieldName,
                    BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(target, value);
        }
    }
}
