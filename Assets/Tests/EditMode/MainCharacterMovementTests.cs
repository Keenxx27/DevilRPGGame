using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class MainCharacterMovementTests
    {
        [Test]
        public void AddingMovementAlsoAddsRequiredRigidbody2D()
        {
            GameObject gameObject = new GameObject("Movement Test");
            try
            {
                gameObject.AddComponent<MainCharacterMovement>();

                Assert.That(gameObject.GetComponent<Rigidbody2D>(), Is.Not.Null);
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
            }
        }

        [Test]
        public void MissingAnimator_DoesNotDisableMovement()
        {
            GameObject gameObject = new GameObject("Movement Test");
            try
            {
                gameObject.AddComponent<SpriteRenderer>();
                MainCharacterMovement movement = gameObject.AddComponent<MainCharacterMovement>();

                Assert.That(movement.enabled, Is.True);
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
            }
        }

        [TestCase(0f, 1f, 0f, 5f)]
        [TestCase(-1f, 0f, -5f, 0f)]
        [TestCase(0f, -1f, 0f, -5f)]
        [TestCase(1f, 0f, 5f, 0f)]
        public void CalculateDelta_MovesInRequestedCardinalDirection(
            float inputX, float inputY, float expectedX, float expectedY)
        {
            Vector3 delta = MainCharacterMovement.CalculateDelta(
                new Vector2(inputX, inputY), 5f, 1f);

            Assert.That(delta.x, Is.EqualTo(expectedX).Within(0.0001f));
            Assert.That(delta.y, Is.EqualTo(expectedY).Within(0.0001f));
            Assert.That(delta.z, Is.Zero);
        }

        [Test]
        public void CalculateDelta_ProducesNoMovementForNoInput()
        {
            Vector3 delta = MainCharacterMovement.CalculateDelta(Vector2.zero, 5f, 1f);

            Assert.That(delta, Is.EqualTo(Vector3.zero));
        }

        [Test]
        public void CalculateDelta_LimitsDiagonalMovementToConfiguredSpeed()
        {
            Vector3 delta = MainCharacterMovement.CalculateDelta(Vector2.one, 5f, 1f);

            Assert.That(delta.magnitude, Is.EqualTo(5f).Within(0.0001f));
        }

        [Test]
        public void CalculateDelta_ScalesMovementByDeltaTime()
        {
            Vector3 delta = MainCharacterMovement.CalculateDelta(Vector2.right, 5f, 0.25f);

            Assert.That(delta, Is.EqualTo(new Vector3(1.25f, 0f, 0f)));
        }

        [Test]
        public void CalculateFacingDirection_UsesNormalizedNonZeroInput()
        {
            Vector2 facing = MainCharacterMovement.CalculateFacingDirection(Vector2.down, Vector2.one);

            Assert.That(facing.x, Is.EqualTo(0.7071068f).Within(0.0001f));
            Assert.That(facing.y, Is.EqualTo(0.7071068f).Within(0.0001f));
        }

        [Test]
        public void CalculateFacingDirection_KeepsLastDirectionWhenInputStops()
        {
            Vector2 facing = MainCharacterMovement.CalculateFacingDirection(Vector2.right, Vector2.zero);

            Assert.That(facing, Is.EqualTo(Vector2.right));
        }

        [Test]
        public void CalculateFacingDirection_DefaultsDownWhenBothDirectionsAreZero()
        {
            Vector2 facing = MainCharacterMovement.CalculateFacingDirection(Vector2.zero, Vector2.zero);

            Assert.That(facing, Is.EqualTo(Vector2.down));
        }
    }
}
