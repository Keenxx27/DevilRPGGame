using NUnit.Framework;

namespace RPG.Tests
{
    public class AbyssLocalizationTests
    {
        [Test]
        public void Batch10_AllApprovedKeysResolveForEveryLanguage()
        {
            string[] keys =
            {
                "abyss.entrance.opening.1", "abyss.entrance.opening.2",
                "abyss.entrance.opening.3", "abyss.entrance.moss",
                "abyss.entrance.crystal", "abyss.entrance.passage.1",
                "abyss.entrance.passage.2", "abyss.entrance.passage.3"
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
        public void Batch10_UsesApprovedSensoryAndProtagonistTranslations()
        {
            Assert.That(GameLocalization.Get("abyss.entrance.opening.3", GameLanguage.English),
                Does.EndWith("a strange, sweet tang."));
            Assert.That(GameLocalization.Get("abyss.entrance.moss", GameLanguage.Japanese),
                Does.Contain("逆さにかかった天の川"));
            Assert.That(GameLocalization.Get("abyss.entrance.passage.2", GameLanguage.English),
                Is.EqualTo("Silent. Beautiful. It breathes to a rhythm from deep beneath the earth."));
            Assert.That(GameLocalization.Get("abyss.entrance.passage.3", GameLanguage.Japanese),
                Is.EqualTo("もしかすると、僕はもともとここから来たのかもしれない。"));
        }
    }
}
