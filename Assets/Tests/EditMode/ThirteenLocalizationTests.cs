using NUnit.Framework;

namespace RPG.Tests
{
    public class ThirteenLocalizationTests
    {
        [Test]
        public void Batch11_AllApprovedKeysResolveForEveryLanguage()
        {
            string[] keys =
            {
                "thirteen.speaker.unknown", "thirteen.speaker.thirteen",
                "thirteen.encounter.discovery.1", "thirteen.encounter.discovery.2",
                "thirteen.encounter.discovery.3", "thirteen.encounter.gift.1",
                "thirteen.encounter.gift.2", "thirteen.encounter.gift.3",
                "thirteen.encounter.gift.4", "thirteen.encounter.gift.5",
                "thirteen.encounter.gift.6", "thirteen.encounter.gift.7",
                "thirteen.encounter.gift.8", "thirteen.ui.offerDriedMeat"
            };

            foreach (string key in keys)
            {
                foreach (GameLanguage language in new[]
                    {
                        GameLanguage.SimplifiedChinese,
                        GameLanguage.English,
                        GameLanguage.Japanese
                    })
                {
                    string localized = GameLocalization.Get(key, language);
                    Assert.That(localized, Is.Not.Null.And.Not.Empty);
                    Assert.That(localized, Is.Not.EqualTo(key));
                }
            }
        }

        [Test]
        public void Batch11_UsesNumberWordsForThirteenName()
        {
            Assert.That(GameLocalization.Get("thirteen.speaker.thirteen", GameLanguage.English),
                Is.EqualTo("Thirteen"));
            Assert.That(GameLocalization.Get("thirteen.speaker.thirteen", GameLanguage.Japanese),
                Is.EqualTo("十三"));
            Assert.That(GameLocalization.Get("thirteen.encounter.gift.6", GameLanguage.English),
                Does.StartWith("Thirteen..."));
            Assert.That(GameLocalization.Get("thirteen.encounter.gift.8", GameLanguage.Japanese),
                Does.Contain("十三は僕の指先"));
            Assert.That(GameLocalization.Get("thirteen.ui.offerDriedMeat", GameLanguage.English),
                Does.Contain("in front of Thirteen"));
        }
    }
}
