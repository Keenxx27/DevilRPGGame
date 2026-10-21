using NUnit.Framework;

namespace RPG.Tests
{
    public class RoadHomeLocalizationTests
    {
        [Test]
        public void Batch12_AllApprovedKeysResolveForEveryLanguage()
        {
            string[] keys =
            {
                "roadHome.quarter.1", "roadHome.quarter.2",
                "roadHome.half.1", "roadHome.half.2",
                "roadHome.threeQuarter.1", "roadHome.threeQuarter.2"
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
        public void Batch12_UsesApprovedEnglishAndJapaneseWording()
        {
            Assert.That(GameLocalization.Get("roadHome.quarter.1", GameLanguage.English),
                Is.EqualTo("A secret, overwhelming joy spreads through the corruption mark that runs across my face."));
            Assert.That(GameLocalization.Get("roadHome.half.1", GameLanguage.Japanese),
                Is.EqualTo("分かった。僕はこの目で見た。"));
            Assert.That(GameLocalization.Get("roadHome.half.2", GameLanguage.English),
                Does.EndWith("Thirteen, for example."));
            Assert.That(GameLocalization.Get("roadHome.threeQuarter.1", GameLanguage.Japanese),
                Does.Contain("そして僕は"));
            Assert.That(GameLocalization.Get("roadHome.threeQuarter.2", GameLanguage.English),
                Is.EqualTo("I'm taking Thirteen home to meet Chunbao."));
        }
    }
}
