using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class GangDogMemberVisualAnimatorTests
    {
        [TestCase(false, false, false, 0f, -1f, "GangDogLow_Idle_Down")]
        [TestCase(false, false, true, -1f, 0f, "GangDogLow_Walk_Left")]
        [TestCase(true, false, true, 1f, 0f, "GangDogAdvanced_Walk_Right")]
        [TestCase(true, true, false, 0f, 1f, "GangDogAdvanced_Swing_Up")]
        [TestCase(true, true, true, -1f, 0f, "GangDogAdvanced_Swing_Left")]
        public void ResolveAnimation_UsesIndependentDirectionalStates(
            bool advanced, bool swinging, bool moving, float x, float y, string expected)
        {
            CharacterAnimationState state = GangDogMemberVisualAnimator.ResolveAnimation(
                advanced, swinging, new Vector2(x, y), moving);

            Assert.That(state.Name, Is.EqualTo(expected));
            Assert.That(state.FlipX, Is.False);
        }
    }
}
