using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class LinChunbaoVisualAnimatorTests
    {
        [Test]
        public void ResolveAnimation_UsesIndependentLeftWalkWithoutSpriteFlip()
        {
            CharacterAnimationState state = LinChunbaoVisualAnimator.ResolveAnimation(Vector2.left, true);

            Assert.That(state.Name, Is.EqualTo("LinChunbao_Left_Run"));
            Assert.That(state.FlipX, Is.False);
        }
    }
}
