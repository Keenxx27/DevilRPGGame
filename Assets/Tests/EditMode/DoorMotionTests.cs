using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class DoorMotionTests
    {
        [Test]
        public void Opening_BlocksUntilOpenAngleIsReached()
        {
            var motion = new DoorMotion(0f, 90f);

            Assert.That(motion.Toggle(), Is.True);
            motion.Step(45f, 1f);

            Assert.That(motion.State, Is.EqualTo(DoorState.Opening));
            Assert.That(motion.CurrentAngle, Is.EqualTo(45f).Within(0.0001f));
            Assert.That(motion.BlocksPassage, Is.True);

            motion.Step(45f, 1f);

            Assert.That(motion.State, Is.EqualTo(DoorState.Open));
            Assert.That(motion.CurrentAngle, Is.EqualTo(90f).Within(0.0001f));
            Assert.That(motion.IsFullyOpen, Is.True);
            Assert.That(motion.BlocksPassage, Is.False);
        }

        [Test]
        public void Closing_BlocksImmediatelyAndReturnsToClosedAngle()
        {
            var motion = new DoorMotion(0f, 90f);
            motion.Toggle();
            motion.Step(90f, 1f);

            Assert.That(motion.Toggle(), Is.True);
            Assert.That(motion.State, Is.EqualTo(DoorState.Closing));
            Assert.That(motion.BlocksPassage, Is.True);

            motion.Step(90f, 1f);

            Assert.That(motion.State, Is.EqualTo(DoorState.Closed));
            Assert.That(motion.CurrentAngle, Is.EqualTo(0f).Within(0.0001f));
            Assert.That(motion.BlocksPassage, Is.True);
        }

        [Test]
        public void Toggle_WhileMovingIsIgnored()
        {
            var motion = new DoorMotion(0f, 90f);

            Assert.That(motion.Toggle(), Is.True);
            Assert.That(motion.Toggle(), Is.False);
            motion.Step(30f, 1f);
            Assert.That(motion.Toggle(), Is.False);
            Assert.That(motion.State, Is.EqualTo(DoorState.Opening));
        }

        [Test]
        public void Controller_DisablesColliderOnlyAfterOpeningFinishes()
        {
            var doorObject = new GameObject("Door");
            doorObject.SetActive(false);
            BoxCollider2D blocker = doorObject.AddComponent<BoxCollider2D>();
            DoorController controller = doorObject.AddComponent<DoorController>();
            SetPrivateField(controller, "blockingCollider", blocker);
            SetPrivateField(controller, "openAngleDegrees", 90f);
            SetPrivateField(controller, "degreesPerSecond", 90f);
            doorObject.SetActive(true);
            EditModeTestLifecycle.Invoke(controller, "Awake");

            try
            {
                Assert.That(controller.ToggleDoor(), Is.True);
                controller.Advance(0.5f);
                Assert.That(blocker.enabled, Is.True);
                Assert.That(doorObject.transform.localEulerAngles.z,
                    Is.EqualTo(45f).Within(0.0001f));

                controller.Advance(0.5f);
                Assert.That(blocker.enabled, Is.False);
                Assert.That(controller.IsFullyOpen, Is.True);

                Assert.That(controller.ToggleDoor(), Is.True);
                Assert.That(blocker.enabled, Is.True,
                    "关闭指令被接受时必须立即恢复门洞碰撞");
            }
            finally
            {
                Object.DestroyImmediate(doorObject);
            }
        }

        private static void SetPrivateField<T>(object target, string name, T value)
        {
            FieldInfo field = target.GetType().GetField(
                name, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(target, value);
        }
    }
}
