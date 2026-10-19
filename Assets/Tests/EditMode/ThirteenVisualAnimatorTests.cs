using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class ThirteenVisualAnimatorTests
    {
        [Test]
        public void ResolveAnimation_UsesIndependentLeftRunWithoutSpriteFlip()
        {
            CharacterAnimationState state = ThirteenVisualAnimator.ResolveAnimation(Vector2.left, true);

            Assert.That(state.Name, Is.EqualTo("Thirteen_Left_Run"));
            Assert.That(state.FlipX, Is.False);
        }
    }
}
