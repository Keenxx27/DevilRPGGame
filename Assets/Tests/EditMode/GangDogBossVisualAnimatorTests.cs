using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class GangDogBossVisualAnimatorTests
    {
        [TestCase(false, false, false, 0f, -1f, "GangDogBoss_Idle_Down")]
        [TestCase(false, false, true, 1f, 0f, "GangDogBoss_Walk_Right")]
        [TestCase(true, false, true, -1f, 0f, "GangDogBoss_Charge_Left")]
        [TestCase(false, true, true, 0f, 1f, "GangDogBoss_Stun_Up")]
        public void ResolveAnimation_PrioritizesStunThenChargeThenMovement(
            bool charging, bool stunned, bool moving, float x, float y, string expected)
        {
            CharacterAnimationState state = GangDogBossVisualAnimator.ResolveAnimation(
                charging, stunned, new Vector2(x, y), moving);

            Assert.That(state.Name, Is.EqualTo(expected));
            Assert.That(state.FlipX, Is.False);
        }
    }
}
