using NUnit.Framework;
using RPG;

namespace RPG.Tests
{
    public class GangDogAdvancedMemberTests
    {
        [TestCase(1.1f, 4.8f, true)]
        [TestCase(1.11f, 4.8f, false)]
        [TestCase(1.1f, 1f, false)]
        public void ShouldBeginSwing_RequiresCloseDetectedPlayer(float distance, float detectionRange, bool expected)
        {
            Assert.That(GangDogAdvancedMember.ShouldBeginSwing(distance, detectionRange), Is.EqualTo(expected));
        }
    }
}
