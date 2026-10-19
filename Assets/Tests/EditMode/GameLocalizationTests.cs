using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class GameLocalizationTests
    {
        [TearDown]
        public void TearDown()
        {
            PlayerPrefs.DeleteKey(GameLocalization.PreferenceKey);
            GameLocalization.Reload();
        }

        [Test]
        public void Get_UsesTheSavedEnglishLanguage()
        {
            GameLocalization.SetLanguage(GameLanguage.English);

            Assert.That(GameLocalization.Get("tutorial.title"), Is.EqualTo("Game Controls"));
        }

        [Test]
        public void Format_InsertsThePlayerNameWithoutChangingIt()
        {
            GameLocalization.SetLanguage(GameLanguage.Japanese);

            Assert.That(GameLocalization.Format("nameEntry.normalResult", "Lin Jiantai"),
                Is.EqualTo("あなたの名前は Lin Jiantai。"));
        }

        [Test]
        public void Reload_UsesSimplifiedChineseWhenNoLanguageWasSaved()
        {
            PlayerPrefs.DeleteKey(GameLocalization.PreferenceKey);
            GameLocalization.Reload();

            Assert.That(GameLocalization.CurrentLanguage, Is.EqualTo(GameLanguage.SimplifiedChinese));
        }

        [Test]
        public void Get_UsesApprovedEnglishClassroomStudentLine()
        {
            GameLocalization.SetLanguage(GameLanguage.English);

            Assert.That(GameLocalization.Get("classroom.dismissal.student1"), Is.EqualTo(
                "…Why is even easy prey to demon hunters taking a course on demon hunting? Aren’t him afraid?"));
        }

        [Test]
        public void Get_UsesApprovedEnglishClassroomProtagonistLine()
        {
            GameLocalization.SetLanguage(GameLanguage.English);

            Assert.That(GameLocalization.Get("classroom.afterClass.protagonist2"),
                Is.EqualTo("What if they’re just animals..."));
        }

        [Test]
        public void Get_UsesEnglishClassroomFlowerpotDiscoveryLine()
        {
            GameLocalization.SetLanguage(GameLanguage.English);

            Assert.That(GameLocalization.Get("classroom.card.flowerpot.first.1"),
                Is.EqualTo("There's... something under the flowerpot."));
        }

        [Test]
        public void Get_UsesJapaneseClassroomLibraryCardPickupLine()
        {
            GameLocalization.SetLanguage(GameLanguage.Japanese);

            Assert.That(GameLocalization.Get("classroom.card.pickup"),
                Is.EqualTo("ようやく取り戻せた。図書館へ行こう。"));
        }

        [Test]
        public void Get_UsesEnglishLibraryOpeningLine()
        {
            GameLocalization.SetLanguage(GameLanguage.English);

            Assert.That(GameLocalization.Get("library.opening.1"), Is.EqualTo(
                "It’s more desolate than I imagined. Dust coats the shelves; even the librarian is gone."));
        }

        [Test]
        public void Get_UsesJapaneseLibraryOpeningFinalLine()
        {
            GameLocalization.SetLanguage(GameLanguage.Japanese);

            Assert.That(GameLocalization.Get("library.opening.4"), Is.EqualTo("まずは書架から探そう。"));
        }

        [Test]
        public void Get_UsesArchaicEnglishLibraryStoneSpiritExcerpt()
        {
            GameLocalization.SetLanguage(GameLanguage.English);

            Assert.That(GameLocalization.Get("library.environment.bookcase.4.1"), Does.Contain(
                "Thus spake an old man in his twilight years"));
        }

        [Test]
        public void Get_UsesJapaneseLibraryCounterLine()
        {
            GameLocalization.SetLanguage(GameLanguage.Japanese);

            Assert.That(GameLocalization.Get("library.environment.counter.1"),
                Is.EqualTo("カウンターの下に接着剤の瓶がある。でも、手が届かない。"));
        }

        [Test]
        public void Get_UsesArchaicEnglishCompletedPageLine()
        {
            GameLocalization.SetLanguage(GameLanguage.English);

            Assert.That(GameLocalization.Get("library.completed.4"), Is.EqualTo(
                "Though their forms be unlike, their hearts' desire to endure is one."));
        }

        [Test]
        public void Get_UsesJapaneseCorridorSchoolGateLine()
        {
            GameLocalization.SetLanguage(GameLanguage.Japanese);

            Assert.That(GameLocalization.Get("corridor.gate.beforeLibrary"), Is.EqualTo(
                "学校の門だ。でも、まだ図書館へ行っていない。帰るべきじゃない。"));
        }

        [Test]
        public void WorldItem_UsesLocalizedCorridorKeyRingNameWithoutChangingItemId()
        {
            GameLocalization.SetLanguage(GameLanguage.English);
            GameObject itemObject = new GameObject("Test Key Ring");
            itemObject.AddComponent<SpriteRenderer>();
            itemObject.AddComponent<BoxCollider2D>();
            WorldItem item = itemObject.AddComponent<WorldItem>();
            item.ConfigureLocalization("corridor.janitorKeyRing", "corridor.item.janitorKeyRing");

            Assert.That(item.ItemId, Is.EqualTo("corridor.janitorKeyRing"));
            Assert.That(item.DisplayName, Is.EqualTo("Janitor's Key Ring"));
            Object.DestroyImmediate(itemObject);
        }

        [Test]
        public void Get_UsesJapaneseStorageOpeningLine()
        {
            GameLocalization.SetLanguage(GameLanguage.Japanese);

            Assert.That(GameLocalization.Get("storage.opening.1"), Is.EqualTo("中は真っ暗で、何も見えない。"));
        }

        [Test]
        public void Get_UsesEnglishFullInventoryLine()
        {
            GameLocalization.SetLanguage(GameLanguage.English);

            Assert.That(GameLocalization.Get("inventory.full"), Is.EqualTo(
                "Inventory is full. I should put down something I don't need right now."));
        }

        [Test]
        public void WorldItem_UsesLocalizedBroomNameWithoutChangingItemId()
        {
            GameLocalization.SetLanguage(GameLanguage.English);
            GameObject itemObject = new GameObject("Test Broom");
            itemObject.AddComponent<SpriteRenderer>();
            itemObject.AddComponent<BoxCollider2D>();
            WorldItem item = itemObject.AddComponent<WorldItem>();
            item.ConfigureLocalization("storage.broom", "storage.item.broom");

            Assert.That(item.ItemId, Is.EqualTo("storage.broom"));
            Assert.That(item.DisplayName, Is.EqualTo("Broom"));
            Object.DestroyImmediate(itemObject);
        }

        [Test]
        public void WorldItem_UsesLocalizedNameWithoutChangingItemId()
        {
            GameLocalization.SetLanguage(GameLanguage.Japanese);
            GameObject itemObject = new GameObject("Test Item");
            itemObject.AddComponent<SpriteRenderer>();
            itemObject.AddComponent<BoxCollider2D>();
            WorldItem item = itemObject.AddComponent<WorldItem>();
            item.ConfigureLocalization("library.fragment", "library.item.fragment");

            Assert.That(item.ItemId, Is.EqualTo("library.fragment"));
            Assert.That(item.DisplayName, Is.EqualTo("残頁の断片"));
            Object.DestroyImmediate(itemObject);
        }
    }
}
