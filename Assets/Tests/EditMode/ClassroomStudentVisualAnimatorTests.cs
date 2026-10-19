using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class ClassroomStudentVisualAnimatorTests
    {
        [TestCase(0f, -1f, false, "StudentA_Idle_Down")]
        [TestCase(0f, 1f, false, "StudentA_Idle_Up")]
        [TestCase(1f, 0f, true, "StudentA_Right_Run")]
        [TestCase(-1f, 0f, true, "StudentA_Left_Run")]
        public void ResolveAnimation_UsesProvidedPrefixAndIndependentDirections(
            float x, float y, bool isMoving, string expectedState)
        {
            CharacterAnimationState state = ClassroomStudentVisualAnimator.ResolveAnimation(
                "StudentA", new Vector2(x, y), isMoving);

            Assert.That(state.Name, Is.EqualTo(expectedState));
            Assert.That(state.FlipX, Is.False);
        }
    }
}
