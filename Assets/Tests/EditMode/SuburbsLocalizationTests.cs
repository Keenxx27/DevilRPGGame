using NUnit.Framework;

namespace RPG.Tests
{
    public class SuburbsLocalizationTests
    {
        [Test]
        public void RoadOneTranslations_AllKeysResolveForEveryLanguage()
        {
            string[] keys =
            {
                "suburbs.road1.intro.1",
                "suburbs.road1.intro.2",
                "suburbs.road1.intro.3",
                "suburbs.road1.intro.4",
                "suburbs.speaker.strayDogs",
                "suburbs.road1.lowMember.block",
                "suburbs.road1.endpoint.1",
                "suburbs.road1.endpoint.2"
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

        [Test]
        public void RoadTwo_UsesApprovedTranslationsAndGangTerminology()
        {
            Assert.That(GameLocalization.Get("suburbs.road2.intro.1", GameLanguage.English),
                Is.EqualTo("The road is even more complicated here."));
            Assert.That(GameLocalization.Get("suburbs.road2.intro.2", GameLanguage.Japanese),
                Is.EqualTo("そういえば……最近、独り言がますますひどくなっている。"));
            Assert.That(GameLocalization.Get("suburbs.road2.advancedMember.capture", GameLanguage.English),
                Is.EqualTo("You're lucky we haven't carved you up and sold you as devil parts. Now get lost!"));
            Assert.That(GameLocalization.Get("suburbs.road2.advancedMember.capture", GameLanguage.English),
                Does.Not.Contain("demon"));
            Assert.That(GameLocalization.Get("suburbs.road2.endpoint", GameLanguage.Japanese),
                Is.EqualTo("「負け犬」の連中は、ただ一日を無事に過ごせればそれでいい。\nいつもどこかの路地裏に身を寄せ合い、一緒に眠って夜を明かす——\nまるで鼠穴の中にいるように。"));
        }
    }
}
