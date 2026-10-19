using System.Collections.Generic;
using NUnit.Framework;

namespace RPG.Tests
{
    public class ApartmentLocalizationTests
    {
        [Test]
        public void ApartmentTranslations_AllKeysContainThreeNonEmptyValues()
        {
            Dictionary<string, string[]> texts = ApartmentLocalizationTexts.Create();

            Assert.That(texts, Is.Not.Empty);
            foreach (KeyValuePair<string, string[]> pair in texts)
            {
                Assert.That(pair.Key, Does.StartWith("apartment."));
                Assert.That(pair.Value, Has.Length.EqualTo(3));
                Assert.That(pair.Value, Has.All.Not.Null.And.Not.Empty);
            }
        }

        [Test]
        public void Get_UsesLocalizedLinChunbaoName()
        {
            Assert.That(GameLocalization.Get("apartment.speaker.linChunbao", GameLanguage.SimplifiedChinese),
                Is.EqualTo("林春宝"));
            Assert.That(GameLocalization.Get("apartment.speaker.linChunbao", GameLanguage.English),
                Is.EqualTo("Lin Chunbao"));
            Assert.That(GameLocalization.Get("apartment.speaker.linChunbao", GameLanguage.Japanese),
                Is.EqualTo("林春宝"));
        }

        [Test]
        public void Get_UsesLocalizedMunicipalOfficerName()
        {
            Assert.That(GameLocalization.Get("apartment.speaker.municipalOfficer", GameLanguage.SimplifiedChinese),
                Is.EqualTo("行政人员"));
            Assert.That(GameLocalization.Get("apartment.speaker.municipalOfficer", GameLanguage.English),
                Is.EqualTo("Municipal Officer"));
            Assert.That(GameLocalization.Get("apartment.speaker.municipalOfficer", GameLanguage.Japanese),
                Is.EqualTo("行政職員"));
        }
    }
}
