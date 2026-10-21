using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Tests
{
    public class RubbleLocalizationTests
    {
        [TearDown]
        public void TearDown()
        {
            PlayerPrefs.DeleteKey(GameLocalization.PreferenceKey);
            GameLocalization.Reload();
            GameObject canvas = GameObject.Find("Broken Dreams Ending Canvas");
            if (canvas != null) Object.DestroyImmediate(canvas);
        }

        [Test]
        public void Batch8_AllApprovedKeysResolveForEveryLanguage()
        {
            string[] keys =
            {
                "rubble.speaker.strayDogsBoss",
                "rubble.encounter.1", "rubble.encounter.2", "rubble.encounter.3",
                "rubble.encounter.4", "rubble.encounter.5", "rubble.encounter.6",
                "rubble.encounter.7", "rubble.encounter.8", "rubble.encounter.9",
                "rubble.encounter.10", "rubble.encounter.11", "rubble.encounter.12",
                "rubble.encounter.13", "rubble.encounter.14",
                "rubble.ui.progress", "rubble.ui.escape",
                "rubble.ending.brokenDreams.title",
                "rubble.ending.brokenDreams.1", "rubble.ending.brokenDreams.2",
                "rubble.ending.brokenDreams.3", "rubble.ending.brokenDreams.4",
                "rubble.ending.brokenDreams.5"
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
        public void Batch8_UsesUserApprovedBossTerminologyAndCorrection()
        {
            Assert.That(GameLocalization.Get("rubble.encounter.10", GameLanguage.English), Is.EqualTo(
                "Don't think that thing on your face can fool me. My son's in the same major as you,\nand he's told me all about you. You freak!"));
            Assert.That(GameLocalization.Get("rubble.encounter.14", GameLanguage.English),
                Does.Contain("half-devil"));
            Assert.That(GameLocalization.Get("rubble.encounter.11", GameLanguage.English),
                Does.Contain("demon"));
        }

        [Test]
        public void Batch8_FormatsProgressForEveryLanguage()
        {
            GameLocalization.SetLanguage(GameLanguage.SimplifiedChinese);
            Assert.That(GameLocalization.Format("rubble.ui.progress", 3),
                Is.EqualTo("逃离丧家犬老大：已开启 3 / 4 个开关"));
            GameLocalization.SetLanguage(GameLanguage.English);
            Assert.That(GameLocalization.Format("rubble.ui.progress", 3),
                Is.EqualTo("Escape the Stray Dogs Boss: 3 / 4 Switches Activated"));
            GameLocalization.SetLanguage(GameLanguage.Japanese);
            Assert.That(GameLocalization.Format("rubble.ui.progress", 3),
                Is.EqualTo("「負け犬」のボスから逃げろ：スイッチ 3 / 4 起動"));
        }

        [Test]
        public void Batch8_BrokenDreamsEndingUsesCameraIndependentCanvasWithSubtitles()
        {
            GameLocalization.SetLanguage(GameLanguage.English);
            GameObject encounterObject = new GameObject("Rubble Encounter Test");
            RubbleBossEncounter encounter = encounterObject.AddComponent<RubbleBossEncounter>();
            typeof(RubbleBossEncounter).GetMethod("ShowFailureEnding",
                BindingFlags.Instance | BindingFlags.NonPublic).Invoke(encounter, null);

            GameObject canvasObject = GameObject.Find("Broken Dreams Ending Canvas");
            Assert.That(canvasObject, Is.Not.Null);
            Assert.That(canvasObject.GetComponent<Canvas>().renderMode,
                Is.EqualTo(RenderMode.ScreenSpaceOverlay));
            Assert.That(canvasObject.GetComponent<Camera>(), Is.Null);
            Assert.That(canvasObject.transform.Find("Ending Title").GetComponent<Text>().text,
                Is.EqualTo("Ending: Dreams Cut Short"));
            Assert.That(canvasObject.transform.Find("Ending Description").GetComponent<Text>().text,
                Does.EndWith("There is nothing before you."));

            Object.DestroyImmediate(encounterObject);
        }

        [Test]
        public void Batch9_UsesApprovedLoreNoteTranslations()
        {
            Assert.That(GameLocalization.Get("rubble.lore.tuition.1", GameLanguage.Japanese),
                Is.EqualTo("そもそも、僕たちの大学の学費はそこまで高くない……"));
            Assert.That(GameLocalization.Get("rubble.lore.tuition.2", GameLanguage.English),
                Does.Contain("education and culture"));
            Assert.That(GameLocalization.Get("rubble.lore.tuition.3", GameLanguage.Japanese),
                Is.EqualTo("なら、僕を殺す理由がどこにある？"));
            Assert.That(GameLocalization.Get("rubble.lore.debt.2", GameLanguage.English),
                Is.EqualTo("Merchandise from big-name demon hunters."));
            Assert.That(GameLocalization.Get("rubble.lore.debt.3", GameLanguage.English),
                Is.EqualTo("Looks like the money you're scraping together for your son isn't just for tuition."));
            Assert.That(GameLocalization.Get("rubble.lore.debt.4", GameLanguage.Japanese),
                Is.EqualTo("……"));
        }
    }
}
