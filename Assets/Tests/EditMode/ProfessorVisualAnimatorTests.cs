using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class ProfessorVisualAnimatorTests
    {
        [TestCase(0f, -1f, false, "Professor_Idle_Down")]
        [TestCase(0f, 1f, false, "Professor_Idle_Up")]
        [TestCase(1f, 0f, true, "Professor_Right_Run")]
        [TestCase(-1f, 0f, true, "Professor_Left_Run")]
        public void ResolveAnimation_UsesIndependentDirectionalStates(
            float x, float y, bool isMoving, string expectedState)
        {
            CharacterAnimationState state = ProfessorVisualAnimator.ResolveAnimation(
                new Vector2(x, y), isMoving);

            Assert.That(state.Name, Is.EqualTo(expectedState));
            Assert.That(state.FlipX, Is.False);
        }
    }
}
