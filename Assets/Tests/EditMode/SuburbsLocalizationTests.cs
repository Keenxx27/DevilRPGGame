using System.Collections.Generic;
using NUnit.Framework;

namespace RPG.Tests
{
    public class SuburbsLocalizationTests
    {
        [Test]
        public void RoadOneTranslations_AllKeysContainThreeNonEmptyValues()
        {
            Dictionary<string, string[]> texts = SuburbsLocalizationTexts.Create();

            Assert.That(texts, Has.Count.EqualTo(8));
            foreach (KeyValuePair<string, string[]> pair in texts)
            {
                Assert.That(pair.Key, Does.StartWith("suburbs."));
                Assert.That(pair.Value, Has.Length.EqualTo(3));
                Assert.That(pair.Value, Has.All.Not.Null.And.Not.Empty);
            }
        }

        [Test]
        public void RoadOne_UsesApprovedIntroTranslations()
        {
            Assert.That(GameLocalization.Get("suburbs.road1.intro.2", GameLanguage.English),
                Is.EqualTo("They're the lowest rung among demon hunters, scavenging demon remains no one else wants,\nrummaging through trash at the edge of the Abyss and hawking their finds in corners of the black market."));
            Assert.That(GameLocalization.Get("suburbs.road1.intro.3", GameLanguage.Japanese),
                Is.EqualTo("半人半魔の僕も、あいつらに拾われる「ゴミ」になるのだろうか……"));
        }

        [Test]
        public void RoadOne_UsesApprovedGangAndEndpointTranslations()
        {
            Assert.That(GameLocalization.Get("suburbs.speaker.strayDogs", GameLanguage.English),
                Is.EqualTo("Stray Dogs"));
            Assert.That(GameLocalization.Get("suburbs.speaker.strayDogs", GameLanguage.Japanese),
                Is.EqualTo("負け犬"));
            Assert.That(GameLocalization.Get("suburbs.road1.lowMember.block", GameLanguage.Japanese),
                Is.EqualTo("ここは俺たちの縄張りだ。お前を通す気はねえ！"));
            Assert.That(GameLocalization.Get("suburbs.road1.endpoint.2", GameLanguage.English),
                Is.EqualTo("But I can't say the same for the road ahead. I may run into even more dangerous members of the Stray Dogs."));
        }
    }
}
