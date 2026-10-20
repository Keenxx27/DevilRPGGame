using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Tests
{
    public class ApartmentLocalizationTests
    {
        [TearDown]
        public void TearDown()
        {
            UnityEngine.PlayerPrefs.DeleteKey(GameLocalization.PreferenceKey);
            GameLocalization.Reload();
            DestroyIfPresent("No Belonging Ending Canvas");
            DestroyIfPresent("Indifferent Ending Canvas");
        }

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

        [Test]
        public void Batch1_UsesApprovedArrivalTranslations()
        {
            Dictionary<string, string[]> texts = ApartmentLocalizationTexts.Create();

            Assert.That(texts["apartment.entrance.intro.1"], Is.EqualTo(new[]
            {
                "（侵蚀痕迹的灼热尚未消退。）\n不知是从何时起养成了这种自言自语的行为……\n可能从小便是这样。",
                "(The burning in the corruption mark still hasn't faded.)\nI don't know when I started talking to myself like this...\nMaybe I've always done it, ever since I was little.",
                "（侵食痕の灼熱は、まだ消えていない。）\nいつからこんなふうに独り言を言うようになったのだろう……\n幼い頃からだったのかもしれない。"
            }));
            Assert.That(texts["apartment.entrance.intro.2"], Is.EqualTo(new[]
            {
                "啊，公寓楼就在前面。\n虽说老旧，但也是个家。",
                "Ah, the apartment building is just ahead.\nIt's old, but it's still home.",
                "ああ、アパートはもう目の前だ。\n古びてはいるが、それでも我が家だ。"
            }));
            Assert.That(texts["apartment.entrance.door.1"], Is.EqualTo(new[]
            {
                "我妹妹林春宝这个时候大概已经放学在家里等着我了吧。\n……真不知道这句话是对谁说的……\n好像我们这个家真会来客人一样。",
                "My younger sister, Lin Chunbao, should be home from school and waiting for me by now.\n...I don't even know who I'm saying this to...\nAs if anyone would ever come to visit us.",
                "妹の林春宝は、もう学校から帰って家で僕を待っている頃だろう。\n……いったい誰に向かって言っているんだ……\nまるで、この家に本当に客が来るみたいじゃないか。"
            }));
            Assert.That(texts["apartment.entrance.door.2"], Is.EqualTo(new[]
            {
                "……没有人愿意跟一个半人半恶魔的家伙说话，除了她。\n因为我是她哥哥。",
                "...No one wants to talk to someone who's half human and half demon. No one except her.\nBecause I'm her brother.",
                "……半人半魔のやつと話したがる人間なんて、彼女以外にはいない。\n僕が、彼女の兄だからだ。"
            }));
            Assert.That(texts["apartment.entrance.wrongWay"], Is.EqualTo(new[]
            {
                "这边不是通往郊区的路，我正后方的路才是。",
                "This isn't the way to the suburbs. The road directly behind me is.",
                "こっちは郊外へ向かう道じゃない。真後ろの道だ。"
            }));
            Assert.That(texts["apartment.livingRoom.intro.1"], Is.EqualTo(new[]
            {
                "厨房里飘出煎蛋的香味。\n春宝大概在做晚餐吧。",
                "The smell of fried eggs is drifting from the kitchen.\nChunbao must be making dinner.",
                "台所から目玉焼きの香りが漂ってくる。\n春宝が夕食を作っているのだろう。"
            }));
            Assert.That(texts["apartment.livingRoom.intro.2"], Is.EqualTo(new[]
            {
                "自从六岁时那次使我脸上留下痕迹的意外以来\n春宝便和我相依为命，确实辛苦她了。",
                "Ever since the accident when I was six left this mark on my face,\nChunbao and I have had no one but each other. She's been through a lot because of it.",
                "六歳のときの事故で顔にこの痕が残って以来、\n春宝と僕はずっと二人で支え合ってきた。彼女には本当に苦労をかけている。"
            }));
            Assert.That(texts["apartment.livingRoom.intro.3"], Is.EqualTo(new[]
            {
                "我到底在和谁说话啊……\n不管了，先去厨房。",
                "Who am I even talking to...?\nNever mind. I should head to the kitchen.",
                "僕はいったい誰に話しかけているんだ……\nまあいい。先に台所へ行こう。"
            }));
        }

        [Test]
        public void Batch2_UsesUserApprovedDialogueCorrections()
        {
            Dictionary<string, string[]> texts = ApartmentLocalizationTexts.Create();

            Assert.That(texts["apartment.kitchen.table.3"][1], Does.Contain("small hole of Abyss"));
            Assert.That(texts["apartment.kitchen.table.3"][2], Does.Contain("小さな深淵の穴"));
            Assert.That(texts["apartment.kitchen.table.7"][2], Does.Contain("部長の俺"));
            Assert.That(texts["apartment.kitchen.belief.6"][1],
                Is.EqualTo("Well, all that studying has scrambled up your brain."));
            Assert.That(texts["apartment.kitchen.belief.6"][2],
                Is.EqualTo("あーあ、兄ちゃん、勉強しすぎて頭がこんがらがっちまったな。"));
        }

        [Test]
        public void Batch2_UsesApprovedIndifferentEnding()
        {
            Dictionary<string, string[]> texts = ApartmentLocalizationTexts.Create();

            Assert.That(texts["apartment.ending.indifferent.title"], Is.EqualTo(new[]
            {
                "结局：关我何事", "Ending: None of My Concern", "結末：僕には関係ない"
            }));
            Assert.That(texts["apartment.ending.indifferent.4"], Is.EqualTo(new[]
            {
                "永远找不到归属。", "Never finding a place to belong.", "永遠に、居場所を見つけられないまま。"
            }));
        }

        [Test]
        public void Batch2_FormatsBoxingStageForEveryLanguage()
        {
            GameLocalization.SetLanguage(GameLanguage.SimplifiedChinese);
            Assert.That(GameLocalization.Format("apartment.ui.boxing.stage", 2),
                Is.EqualTo("拳击训练　第 2 / 3 阶段"));
            GameLocalization.SetLanguage(GameLanguage.English);
            Assert.That(GameLocalization.Format("apartment.ui.boxing.stage", 2),
                Is.EqualTo("Boxing Training  Stage 2 / 3"));
            GameLocalization.SetLanguage(GameLanguage.Japanese);
            Assert.That(GameLocalization.Format("apartment.ui.boxing.stage", 2),
                Is.EqualTo("ボクシング訓練　第 2 / 3 段階"));
        }

        [Test]
        public void Batch2_BoxingGlovesUseLocalizedNameWithoutChangingItemId()
        {
            GameLocalization.SetLanguage(GameLanguage.English);
            var gameObject = new UnityEngine.GameObject("Boxing Gloves Test");
            gameObject.AddComponent<UnityEngine.SpriteRenderer>();
            gameObject.AddComponent<UnityEngine.BoxCollider2D>();
            WorldItem item = gameObject.AddComponent<WorldItem>();
            item.Configure("拳击手套", true);
            item.ConfigureLocalization("apartment.boxingGloves", "apartment.item.boxingGloves");

            Assert.That(item.ItemId, Is.EqualTo("apartment.boxingGloves"));
            Assert.That(item.DisplayName, Is.EqualTo("Boxing Gloves"));

            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void Batch3_UsesApprovedVentCorrection()
        {
            Assert.That(GameLocalization.Get("apartment.bathroom.vent.observe", GameLanguage.SimplifiedChinese),
                Is.EqualTo("从这里看去，下水道里贴着一张纸像是写着什么数字，也许和密码锁有关。"));
            Assert.That(GameLocalization.Get("apartment.bathroom.vent.observe", GameLanguage.English),
                Is.EqualTo("From here, I can see a piece of paper stuck inside the sewer. It looks like there's some kind of number written on it—maybe it has something to do with the combination lock."));
            Assert.That(GameLocalization.Get("apartment.bathroom.vent.observe", GameLanguage.Japanese),
                Is.EqualTo("ここから見ると、下水道の中に紙が貼りついていて、何か数字が書かれているようだ。ダイヤル錠と関係があるのかもしれない。"));
        }

        [TestCase("apartment.knife", "apartment.item.knife", "Kitchen Knife", "包丁")]
        [TestCase("apartment.driedMeat", "apartment.item.driedMeat", "Dried Meat", "干し肉")]
        [TestCase("apartment.medicineCabinetKey", "apartment.item.medicineCabinetKey", "Medicine Cabinet Key", "薬棚の鍵")]
        [TestCase("apartment.plaster", "apartment.item.plaster", "Plaster", "膏薬")]
        [TestCase("apartment.stickyKnife", "apartment.item.stickyKnife", "Sticky-Pole Knife", "粘着竿包丁")]
        public void Batch3_ItemsUseLocalizedNamesWithStableIds(
            string itemId, string nameKey, string english, string japanese)
        {
            var gameObject = new UnityEngine.GameObject("Apartment Item Test");
            WorldItem item = gameObject.AddComponent<WorldItem>();
            item.ConfigureLocalization(itemId, nameKey);

            GameLocalization.SetLanguage(GameLanguage.English);
            Assert.That(item.ItemId, Is.EqualTo(itemId));
            Assert.That(item.DisplayName, Is.EqualTo(english));
            GameLocalization.SetLanguage(GameLanguage.Japanese);
            Assert.That(item.ItemId, Is.EqualTo(itemId));
            Assert.That(item.DisplayName, Is.EqualTo(japanese));

            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void Batch4_UsesApprovedBedroomTranslations()
        {
            Dictionary<string, string[]> texts = ApartmentLocalizationTexts.Create();

            Assert.That(texts["apartment.chunbaoBedroom.desk.1"], Is.EqualTo(new[]
            {
                "上面放着春宝小时候最喜欢的玩具，红熊可米。",
                "On it is Chunbao's favorite childhood toy, Red Bear Comy.",
                "上には、春宝が子供の頃に一番気に入っていたおもちゃ、「赤いクマのコミー」が置いてある。"
            }));
            Assert.That(texts["apartment.protagonistBedroom.plasterBoxes.5"], Is.EqualTo(new[]
            {
                "所以有没有人能告诉我我到底是什么！",
                "Then can somebody tell me what I am?!",
                "だったら誰か、僕がいったい何なのか教えてくれ！"
            }));
            Assert.That(texts["apartment.ui.phoneGame.title"], Is.EqualTo(new[]
            {
                "林春宝的手机游戏", "Lin Chunbao's Phone Game", "林春宝のスマホゲーム"
            }));
        }

        [Test]
        public void Batch4_BedsideKeyUsesLocalizedNameWithStableId()
        {
            var gameObject = new UnityEngine.GameObject("Bedside Key Test");
            WorldItem item = gameObject.AddComponent<WorldItem>();
            item.ConfigureLocalization("apartment.bedsideKey", "apartment.item.bedsideKey");

            Assert.That(item.ItemId, Is.EqualTo("apartment.bedsideKey"));
            Assert.That(GameLocalization.Get("apartment.item.bedsideKey", GameLanguage.SimplifiedChinese),
                Is.EqualTo("床头柜钥匙"));
            Assert.That(GameLocalization.Get("apartment.item.bedsideKey", GameLanguage.English),
                Is.EqualTo("Nightstand Key"));
            Assert.That(GameLocalization.Get("apartment.item.bedsideKey", GameLanguage.Japanese),
                Is.EqualTo("ベッド脇の棚の鍵"));

            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void Batch5_UsesApprovedDepartureConfrontationAndEndingTranslations()
        {
            Dictionary<string, string[]> texts = ApartmentLocalizationTexts.Create();

            Assert.That(texts["apartment.departure.askLocation"], Is.EqualTo(new[]
            {
                "我什么都拿好了，告诉我那个新开的小归墟在哪里吧。",
                "I've got everything. Tell me where that newly opened small hole of Abyss is.",
                "必要なものは全部持った。新しく開いた小さな深淵の穴がどこにあるのか、教えてくれ。"
            }));
            Assert.That(texts["apartment.confrontation.final.noPlace"], Is.EqualTo(new[]
            {
                "看来，这里真的没有我的位置了。",
                "It seems there really is no place for me here.",
                "どうやら、ここには本当に僕の居場所がないらしい。"
            }));
            Assert.That(texts["apartment.ending.noBelonging.5"], Is.EqualTo(new[]
            {
                "这是真正属于你的结局。",
                "This is the ending that truly belongs to you.",
                "これこそが、本当にあなた自身の結末だ。"
            }));
        }

        [Test]
        public void Batch5_NoBelongingEndingUsesCameraIndependentTwoPageCanvas()
        {
            GameLocalization.SetLanguage(GameLanguage.English);
            GameObject confrontationObject = new GameObject("Confrontation Test");
            ApartmentConfrontation confrontation = confrontationObject.AddComponent<ApartmentConfrontation>();
            typeof(ApartmentConfrontation).GetMethod("ShowTrueEnding",
                BindingFlags.Instance | BindingFlags.NonPublic).Invoke(confrontation, null);

            GameObject canvasObject = GameObject.Find("No Belonging Ending Canvas");
            Assert.That(canvasObject, Is.Not.Null);
            Assert.That(canvasObject.GetComponent<Canvas>().renderMode,
                Is.EqualTo(RenderMode.ScreenSpaceOverlay));
            Assert.That(canvasObject.GetComponent<Camera>(), Is.Null);

            Transform endingPage = canvasObject.transform.Find("Ending Page");
            Transform thanksPage = canvasObject.transform.Find("Thanks Page");
            Assert.That(endingPage, Is.Not.Null);
            Assert.That(thanksPage, Is.Not.Null);
            Assert.That(endingPage.gameObject.activeSelf, Is.True);
            Assert.That(thanksPage.gameObject.activeSelf, Is.False);
            Assert.That(FindText(endingPage, "Ending Title").text, Is.EqualTo("Ending: Nowhere to Belong"));
            Assert.That(FindText(endingPage, "Ending Description").text,
                Does.Contain("This is the ending that truly belongs to you."));

            canvasObject.GetComponent<NoBelongingEndingPages>().ShowThanksPage();
            Assert.That(endingPage.gameObject.activeSelf, Is.False);
            Assert.That(thanksPage.gameObject.activeSelf, Is.True);
            Assert.That(FindText(thanksPage, "Thanks").text, Is.EqualTo("Thank You for Playing"));

            UnityEngine.Object.DestroyImmediate(confrontationObject);
        }

        private static Text FindText(Transform parent, string name)
        {
            Transform child = parent.Find(name);
            Assert.That(child, Is.Not.Null, "Missing ending text object: " + name);
            return child.GetComponent<Text>();
        }

        private static void DestroyIfPresent(string name)
        {
            GameObject gameObject = GameObject.Find(name);
            if (gameObject != null) UnityEngine.Object.DestroyImmediate(gameObject);
        }
    }
}
