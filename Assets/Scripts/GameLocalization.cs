using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public enum GameLanguage
    {
        SimplifiedChinese,
        English,
        Japanese
    }

    public static class GameLocalization
    {
        public const string PreferenceKey = "game.language";

        private static GameLanguage currentLanguage = LoadLanguage();

        private static readonly Dictionary<string, string[]> Texts = new Dictionary<string, string[]>
        {
            { "language.title", new[] { "请选择语言", "Select Language", "言語を選択" } },
            { "language.hint", new[]
                {
                    "使用左右方向键选择，按 Enter 确认",
                    "Use Left/Right Arrow Keys to Select, Press Enter to Confirm",
                    "左右キーで選択し、Enterキーで決定"
                }
            },
            { "nameEntry.prompt", new[]
                {
                    "告知介于人魔之间的存在的名字。",
                    "Tell the being between human and demon your name.",
                    "人と魔のあいだに在る者の名を告げよ。"
                }
            },
            { "nameEntry.placeholder", new[]
                {
                    "输入名字后按 Enter 确认",
                    "Enter a name, then press Enter to confirm",
                    "名前を入力してEnterキーで決定"
                }
            },
            { "nameEntry.confirmHint", new[] { "按Enter确认", "Press Enter to Confirm", "Enterキーで決定" } },
            { "nameEntry.normalResult", new[] { "你的名字是 {0}", "Your name is {0}.", "あなたの名前は {0}。" } },
            { "nameEntry.kentaEasterEgg", new[] { "真正的名字", "Your true name.", "本当の名前。" } },
            { "nameEntry.chunbaoEasterEgg", new[] { "去取个你自己的名字！", "Get a name of your own!", "自分の名前をつけて！" } },
            { "nameEntry.chuxuanEasterEgg", new[] { "不要这样。", "Don't do that.", "やめて。" } },
            { "nameEntry.thirteenEasterEgg", new[] { "喵。", "Meow.", "ニャー。" } },
            { "nameEntry.emptyEasterEgg", new[] { "不可输入空白内容", "A blank name cannot be entered.", "空白の名前は入力できません。" } },
            { "tutorial.title", new[] { "游戏操作", "Game Controls", "ゲーム操作" } },
            { "tutorial.instructions", new[]
                {
                    "WASD——移动\nE——交互及拾取\nQ——丢弃及放置\nR——对话翻页\n空格——跳过对话\nEsc——暂停游戏",
                    "WASD — Move\nE — Interact / Pick Up\nQ — Drop / Place\nR — Advance Dialogue\nSpace — Skip Dialogue\nEsc — Pause Game",
                    "WASD — 移動\nE — 調べる／拾う\nQ — 捨てる／置く\nR — 会話を進める\nSpace — 会話をスキップ\nEsc — ポーズ"
                }
            },
            { "tutorial.startHint", new[] { "按E键开始游戏", "Press E to Start", "Eキーでゲーム開始" } },
            { "inventory.full", new[] { "东西太满了，最好先把暂时用不到的东西放下。", "Inventory is full. I should put down something I don't need right now.", "持ち物がいっぱいだ。今は使わない物を置いておいたほうがいい。" } },
            { "classroom.speaker.professor", new[] { "教授", "Professor", "教授" } },
            { "classroom.speaker.student1", new[] { "学生1", "Student 1", "学生1" } },
            { "classroom.speaker.student2", new[] { "学生2", "Student 2", "学生2" } },
            { "classroom.lecture.1", new[] { "所以同学们要明确，", "So, students, you need to understand this:", "つまり、諸君に理解してほしいのは、" } },
            { "classroom.lecture.2", new[] { "恶魔是归墟侵蚀具象化的、充满攻击性的副产物。", "Demons are aggressive byproducts—the physical manifestations of the Abyss’s corruption.", "悪魔とは、深淵の侵食が具現化した、攻撃性に満ちた副産物だ。" } },
            { "classroom.lecture.3", new[] { "恶魔猎人的任务，就是像清除癌细胞一样，\n将它们从地球的肌体上彻底清除。", "A demon hunter’s duty is to eradicate them from the body of the Earth,\njust as one would remove cancer cells.", "悪魔狩りの任務は、癌細胞を取り除くように、\n奴らを地球という肉体から完全に排除することだ。" } },
            { "classroom.dismissal.professor", new[] { "下课铃响了，同学们下课吧。", "The bell has rung. Class dismissed, everyone.", "終業のベルが鳴った。今日はここまでだ。" } },
            { "classroom.dismissal.student1", new[] { "……怎么连恶魔猎人眼里的肥肉，都来听关于猎魔的课程？不怕被吓死吗？", "…Why is even easy prey to demon hunters taking a course on demon hunting? Aren’t him afraid?", "……悪魔狩り屋にとっての格好の獲物まで、悪魔狩りの講義を聞きに来るのか？　怖くないのかよ。" } },
            { "classroom.dismissal.student2", new[] { "带着侵蚀痕迹的情况很罕见。听说大都是沾染了归墟能量、又侥幸活下来的人……总之，离他远点。", "People with corruption marks are rare. I hear most were exposed to Abyssal energy and somehow survived… Anyway, stay away from him.", "侵食の痕を持つ人なんて珍しい。深淵のエネルギーに触れて、それでも生き残った人間がほとんどだって……とにかく、あいつには近づかないほうがいい。" } },
            { "classroom.afterClass.protagonist1", new[] { "（左脸被归墟侵蚀的痕迹……正在发热、灼痛。）", "(The mark on the left side of my face, where the Abyss corroded it… is burning, throbbing.)", "（左頬に刻まれた、深淵に侵食された痕が……熱を帯び、焼けるように痛む。）" } },
            { "classroom.afterClass.protagonist2", new[] { "如果它们只是动物。", "What if they’re just animals...", "もし、あれがただの動物なら。" } },
            { "classroom.afterClass.protagonist3", new[] { "如果它们中，有比动物更复杂的存在。", "What if some of them are something more complex than animals?", "もしあの中に、動物よりも複雑な存在がいるなら。" } },
            { "classroom.afterClass.protagonist4", new[] { "我们到底在做什么？", "What exactly are we doing?", "俺たちは、いったい何をしている？" } },
            { "classroom.afterClass.protagonist5", new[] { "也许我该去图书馆看看……", "Maybe I should check the library…", "図書館に行ってみるべきか……" } },
            { "classroom.afterClass.protagonist6", new[] { "不知道我的借书卡去哪里了，", "I wonder where my library card went.", "借りた図書カード、どこへやったんだろう。" } },
            { "classroom.afterClass.protagonist7", new[] { "可能是被哪个家伙藏起来了。", "Maybe one of those guys hid it.", "誰かが隠したのかもしれない。" } },
            { "classroom.card.searchIntro", new[] { "肯定不是放在别的同学的课桌。", "It definitely isn't on any of the other students' desks.", "ほかの学生の机に置いてあるはずがない。" } },
            { "classroom.card.storageCabinet", new[] { "储物柜里没有。看来也没人把它随手塞进来。", "It's not in the storage cabinet. Looks like no one just shoved it in here, either.", "収納棚の中にはない。誰かが適当に突っ込んだわけでもなさそうだ。" } },
            { "classroom.card.podium", new[] { "教授的桌子上也没有。最好别在这里乱翻。", "It's not on the professor's desk either. Better not rummage around here.", "教授の机の上にもない。ここを勝手に漁るのはやめておこう。" } },
            { "classroom.card.teacherDesk", new[] { "讲台下面空空的。", "Nothing under the podium.", "教壇の下は空っぽだ。" } },
            { "classroom.card.blackboard", new[] { "黑板附近也没有。总不可能夹在粉笔槽里吧。", "Nothing near the blackboard, either. It's not like it could be tucked into the chalk tray.", "黒板の近くにもない。まさかチョーク受けに挟まっているわけでもないだろう。" } },
            { "classroom.card.flowerpot.first.1", new[] { "花盆下面……好像压着什么。", "There's... something under the flowerpot.", "植木鉢の下に……何かが押さえつけられている。" } },
            { "classroom.card.flowerpot.first.2", new[] { "是我的借书卡。", "It's my library card.", "俺の図書カードだ。" } },
            { "classroom.card.flowerpot.first.3", new[] { "花盆纹丝不动。真够沉的。", "That flowerpot won't budge. It's seriously heavy.", "植木鉢はびくともしない。本当に重いな。" } },
            { "classroom.card.flowerpot.first.4", new[] { "把这种东西藏在这里，很好玩吗？", "Is hiding something like this really that funny?", "こんなところに隠して、何が楽しいんだ？" } },
            { "classroom.card.flowerpot.first.5", new[] { "就因为我脸上的痕迹，他们就觉得可以随便欺负我。", "Just because of the mark on my face, they think they can push me around.", "顔のこの痕があるだけで、あいつらは好き勝手に俺をいじめていいと思っている。" } },
            { "classroom.card.flowerpot.first.6", new[] { "生气又能怎么样……我总不能在教室里和所有人打一架。", "What would getting angry accomplish… It's not like I can pick a fight with everyone in the classroom.", "腹を立てたところで、どうにもならない……教室で全員と殴り合うわけにもいかない。" } },
            { "classroom.card.flowerpot.first.7", new[] { "得找个够结实的东西，把花盆砸开。", "I need something sturdy enough to smash the flowerpot.", "この植木鉢を壊すには、もっと頑丈なものが必要だ。" } },
            { "classroom.card.flowerpot.repeat", new[] { "还是推不动。得找个东西把它砸开。", "Still won't move. I need something to smash it open.", "やっぱり動かない。何かで壊さないと。" } },
            { "classroom.card.flowerpot.broken", new[] { "碎了。借书卡就在下面。", "It's shattered. The library card is underneath.", "割れた。図書カードは下にある。" } },
            { "classroom.card.pickup", new[] { "终于拿回来了。该去图书馆了。", "Finally got it back. Time to go to the library.", "ようやく取り戻せた。図書館へ行こう。" } },
            { "corridor.libraryDoor.missingCard", new[] { "图书馆门需要刷借书卡入内，我得先拿到借书卡。", "I need a library card to enter the library. I should find mine first.", "図書館に入るには図書カードが必要だ。まずは自分のカードを見つけないと。" } },
            { "corridor.janitor.1", new[] { "这是校工的休息室，\n还是别进去打扰他们了。", "This is the janitor's break room. I should leave them alone.", "ここは用務員の休憩室だ。邪魔しないほうがいい。" } },
            { "corridor.janitor.2", new[] { "……门上挂着一串钥匙。\n校工休息室、各间教室，甚至储藏室的都有。", "...There's a ring of keys hanging on the door. Keys to the janitor's room, every classroom—even the storage room.", "……扉に鍵束が掛かっている。用務員休憩室、各教室、さらには倉庫の鍵まである。" } },
            { "corridor.janitor.3", new[] { "也许派得上用场。", "These might come in handy.", "役に立つかもしれない。" } },
            { "corridor.storageDoor.missingKey", new[] { "储藏室门锁着。没有钥匙进不去。", "The storage room is locked. I can't get in without a key.", "倉庫は施錠されている。鍵がなければ入れない。" } },
            { "corridor.stairwell.1", new[] { "是通往楼梯间的门，上了楼就是其他教室。", "This leads to the stairwell. The other classrooms are upstairs.", "これは階段室へ通じる扉だ。上へ行けば、ほかの教室がある。" } },
            { "corridor.stairwell.2", new[] { "我已经上完课了，没必要回楼上。", "I've already finished class. There's no reason to go back upstairs.", "もう授業は終わった。わざわざ上へ戻る必要はない。" } },
            { "corridor.gate.beforeLibrary", new[] { "这是学校的大门，\n但我还没去图书馆，不该回家。", "This is the school gate, but I haven't gone to the library yet. I shouldn't head home.", "学校の門だ。でも、まだ図書館へ行っていない。帰るべきじゃない。" } },
            { "corridor.gate.beforePage", new[] { "这是学校的大门，\n但我觉得还没到回去的时候……", "This is the school gate, but it doesn't feel like time to go home yet...", "学校の門だ。でも、まだ帰るときじゃない気がする……。" } },
            { "corridor.gate.ready", new[] { "这是学校的大门，\n通过它我便能坐公交回家了。", "This is the school gate. I can catch the bus home through it.", "学校の門だ。ここを出れば、バスで家に帰れる。" } },
            { "corridor.item.janitorKeyRing", new[] { "校工钥匙串", "Janitor's Key Ring", "用務員の鍵束" } },
            { "storage.opening.1", new[] { "里面真黑，完全看不见。", "It's pitch-black in here. I can't see a thing.", "中は真っ暗で、何も見えない。" } },
            { "storage.opening.2", new[] { "也不知道电灯开关在哪里，\n看来只能摸索着找东西了。", "I have no idea where the light switch is. Looks like I'll have to feel my way around and search.", "電灯のスイッチがどこにあるのかも分からない。手探りで物を探すしかなさそうだ。" } },
            { "storage.shelf.left", new[] { "架子上全是受潮的清洁用品和旧档案。\n没有能用的。", "The shelves are full of damp cleaning supplies and old files. Nothing useful.", "棚には湿気た清掃用品と古い書類がぎっしりある。使えそうな物はない。" } },
            { "storage.shelf.right", new[] { "墙边堆满了工具和杂物。\n黑暗里很难分辨清楚。", "Tools and odds and ends are piled along the wall. It's hard to make them out in the dark.", "壁際には工具や雑多な物が積まれている。暗くてよく見分けられない。" } },
            { "storage.crate.north", new[] { "箱子里是替换下来的桌椅零件。\n翻不出什么特别的。", "The box holds replacement parts for desks and chairs. Nothing special turns up.", "箱の中身は取り替えられた机と椅子の部品だ。特別な物は見つからない。" } },
            { "storage.crate.south", new[] { "这里有股发霉的纸味。\n只是些报废的试卷和杂物。", "It smells like moldy paper here. Just discarded test papers and clutter.", "ここはカビの生えた紙の匂いがする。廃棄された試験用紙と雑物ばかりだ。" } },
            { "storage.broom.beforeGlue", new[] { "这是……一把扫帚。\n暂时不知道能有什么用。", "This is... a broom. I don't know what it could be useful for yet.", "これは……箒だ。今のところ、何に使えるのか分からない。" } },
            { "storage.broom.afterGlue", new[] { "这是……一把扫帚。\n正是我要的东西。", "This is... a broom. It's exactly what I need.", "これは……箒だ。まさに必要な物だ。" } },
            { "storage.item.broom", new[] { "扫帚", "Broom", "箒" } },
            { "library.opening.1", new[] { "这里比我想象中更荒凉。\n书架积着灰，连管理员都不在。", "It’s more desolate than I imagined. Dust coats the shelves; even the librarian is gone.", "思っていたよりずっと荒れている。書架には埃が積もり、司書の姿もない。" } },
            { "library.opening.2", new[] { "电子检索只会给出被整理过的答案；\n真正难以归档的东西，往往躲在这些无人问津的角落。", "Electronic searches only return answers that have already been catalogued; the things hardest to archive are often hidden in these forgotten corners.", "電子検索が返すのは、整理された答えだけだ。記録に収まりきらないものほど、こういう誰にも顧みられない片隅に隠れている。" } },
            { "library.opening.3", new[] { "脆弱的手稿、私印的游记、没有被官方档案收录的地方志异……\n如果有关于真实的恶魔的线索，也许就在这里。", "Fragile manuscripts, privately printed travelogues, local tales omitted from official archives... If there are any clues about what demons truly are, they may be here.", "脆い手稿、私家版の紀行文、公式の記録には収められなかった各地の奇譚……悪魔の本当の姿につながる手掛かりがあるなら、ここかもしれない。" } },
            { "library.opening.4", new[] { "先从书架开始找吧。", "Let’s start with the shelves.", "まずは書架から探そう。" } },
            { "library.environment.bookcase.first", new[] { "这些古籍书名夸张，内容真伪难辨，但这正是我需要的——主流叙事之外的、残破的、带着人味的声音。", "The titles of these old books are extravagant, and their contents are hard to verify. But that's exactly what I need—the broken, human voices outside the mainstream narrative.", "これらの古書は題名ばかり仰々しく、内容も真偽定かでない。だが、俺に必要なのはまさにこれだ――主流の語りの外にある、断片的で、人の息づかいを残した声。" } },
            { "library.environment.bookcase.1", new[] { "某村樵夫坠入地穴，被发光蘑菇所救。", "A woodcutter from some village fell into an underground cavern and was saved by glowing mushrooms.", "ある村の樵が地下の洞穴へ落ち、光る茸に救われた。" } },
            { "library.environment.bookcase.2", new[] { "某族世代祭祀‘地母’，换取矿产丰饶。", "A certain clan worshipped the “Earth Mother” for generations, in exchange for abundant mineral wealth.", "ある一族は代々「地母」を祀り、鉱物の豊かさを得た。" } },
            { "library.environment.bookcase.4.1", new[] { "……暮年老叟言，其祖曾于矿道深处，遇一能言之石精。石精不害人，反指路避塌方。问其何所求，答曰：‘勿再深掘，惊我眠乡。’后矿竭，人散，地动亦息。", "…Thus spake an old man in his twilight years: deep within the mineways, his forebear once encountered a stone spirit that spake. The spirit harmed no man; rather, it pointed the way and spared him from the cave-in. Asked what it sought, it answered: “Delve no deeper, lest ye rouse my sleeping home.” Thereafter the mine was spent, the people scattered, and the trembling of the earth likewise ceased.", "……暮年の老翁、かく語れり。其の祖、かつて坑道の奥深くにて、物言ふ石精に出会へり。石精、人を害さず、かえって崩落を避くる道を示せり。何を求むるやと問へば、答へて曰く、「これより深く掘るなかれ。我が眠りの郷を驚かすなかれ」と。のち鉱脈尽き、人は散じ、地の揺れもまた息みたり。" } },
            { "library.environment.bookcase.4.2", new[] { "……果真是荒谬至极。\n等等，掉出了什么东西？", "…What utter nonsense. Wait—did something just fall out?", "……まったく荒唐無稽だ。待て、何か落ちてきた？" } },
            { "library.environment.chair", new[] { "椅子底下像是压着什么。\n先把它推开看看。", "There seems to be something trapped beneath this chair. I should push it aside and take a look.", "この椅子の下に、何かが挟まっているようだ。押しのけて見てみよう。" } },
            { "library.environment.wall", new[] { "这块墙砖松得不太自然，里面似乎卡着什么。\n借书卡边缘够硬，也许能把它撬出来。", "This brick is loose in a way that doesn't seem natural. Something appears to be lodged inside. The edge of my library card is hard enough—maybe I can pry it out.", "この壁の煉瓦、妙に緩んでいる。中に何かが挟まっているようだ。図書カードの縁なら十分硬い。これでこじ開けられるかもしれない。" } },
            { "library.environment.counter.1", new[] { "柜台底下有一瓶胶水。\n可我的手够不到它。", "Beneath the counter, there's a bottle of glue. But I can't reach it.", "カウンターの下に接着剤の瓶がある。でも、手が届かない。" } },
            { "library.environment.counter.2", new[] { "得找个长一点的东西，\n把它勾出来。", "I need something longer to hook it out with.", "もっと長い物を使って、引っかけて取り出さないと。" } },
            { "library.environment.counter.broom", new[] { "扫帚够得着。\n得慢慢把它勾出来。", "The broom can reach it. I need to ease it out slowly.", "箒なら届く。ゆっくり引っかけて出そう。" } },
            { "library.environment.fragments.second", new[] { "拼起来似乎能显示出一段话，\n最好把它们放到一张桌子上摊开来看。", "Put together, they look like they might reveal a passage. I should spread them out on a table.", "つなぎ合わせれば、何かの文章が現れそうだ。机の上に広げて見たほうがいい。" } },
            { "library.environment.table.fragmentsPlaced", new[] { "还得拿点东西将它们粘起来。", "I'll need something to glue them together.", "これらを貼り合わせる物が必要だ。" } },
            { "library.glue.1", new[] { "终于是找到能把它们粘起来的东西了……", "At last, I've found something to glue them together with...", "ようやく、これを貼り合わせられる物を見つけた……" } },
            { "library.completed.1", new[] { "粘好了。\n（被归墟侵蚀的痕迹在微微发热，不是刺痛，\n而是一种温暖的、引导般的脉动。）", "It's done.\n(The mark corroded by the Abyss is gently warming. Not pain, but a warm, guiding pulse.)", "貼り終えた。\n（深淵に侵食された痕が、かすかに熱を帯びている。痛みではない。温かく、導くような脈動だ。）" } },
            { "library.completed.2", new[] { "地母。石精。\n在更古老的记录里，恶魔似乎有过别的名字，\n不那么绝对，甚至带着一丝敬畏。", "The Earth Mother. The stone spirit.\nIn older records, it seems demons once had other names—less absolute, perhaps even tinged with reverence.", "地母。石精。\nより古い記録では、悪魔には別の名があったらしい。今ほど断定的ではなく、そこにはわずかな畏敬さえ含まれている。" } },
            { "library.completed.3", new[] { "让我读读这残页上写着什么……", "Let me see what this fragment says...", "この残頁に何が書かれているのか、読んでみよう……" } },
            { "library.completed.4", new[] { "其形虽异，其求存之心同。", "Though their forms be unlike, their hearts' desire to endure is one.", "其の形は異なれど、其の生を求むる心は同じ。" } },
            { "library.completed.5", new[] { "（痕迹的灼热达到了顶点，不再是不适，\n而是一种清晰的、近乎共鸣的震颤。）", "(The burning in the mark reaches its peak. It is no longer discomfort, but a clear, almost resonant trembling.)", "（痕の灼熱が頂点に達した。もはや不快感ではない。明瞭で、ほとんど共鳴に近い震えだ。）" } },
            { "library.completed.6", new[] { "求存之心。\n我要自己去看看。\n不是透过报告，不是透过枪口。", "The will to endure.\nI have to see it for myself.\nNot through reports. Not through gun barrels.", "生を求むる心。\n自分の目で確かめなければ。\n報告書を通してではなく、銃口越しでもなく。" } },
            { "library.completed.7", new[] { "去和它们……对话。\n先回家去吧。", "Go... talk to them.\nI should go home first.", "行って……彼らと話そう。\nまずは家に帰ろう。" } },
            { "library.revisit.3", new[] { "我想我已经读过了，回家去吧。", "I think I've read this before. I should go home.", "もう読んだはずだ。家に帰ろう。" } },
            { "library.hint.push", new[] { "按 E 用力推开（{0}/3）", "Press E to Push Hard ({0}/3)", "Eキーで力を込めて押す（{0}/3）" } },
            { "library.hint.hook", new[] { "按 E 用扫帚勾取（{0}/3）", "Press E to Hook with Broom ({0}/3)", "Eキーで箒を使って引っかける（{0}/3）" } },
            { "library.hint.apply", new[] { "按 E 涂抹胶水（{0}/3）", "Press E to Apply Glue ({0}/3)", "Eキーで接着剤を塗る（{0}/3）" } },
            { "library.item.fragment", new[] { "残页碎片", "Page Fragment", "残頁の断片" } },
            { "library.item.glue", new[] { "胶水", "Glue", "接着剤" } },
            { "library.item.completePage", new[] { "完整残页", "Completed Fragment", "完成した残頁" } },
            { "library.item.card", new[] { "借书卡", "Library Card", "図書カード" } }
        };

        public static GameLanguage CurrentLanguage => currentLanguage;

        public static void SetLanguage(GameLanguage language)
        {
            currentLanguage = language;
            PlayerPrefs.SetInt(PreferenceKey, (int)language);
            PlayerPrefs.Save();
        }

        public static void Reload()
        {
            currentLanguage = LoadLanguage();
        }

        public static string Get(string key)
        {
            return Get(key, currentLanguage);
        }

        public static string Get(string key, GameLanguage language)
        {
            if (!Texts.TryGetValue(key, out string[] translations)) return key;
            return translations[(int)language];
        }

        public static string Format(string key, params object[] values)
        {
            return string.Format(Get(key), values);
        }

        private static GameLanguage LoadLanguage()
        {
            int saved = PlayerPrefs.GetInt(PreferenceKey, (int)GameLanguage.SimplifiedChinese);
            return saved >= (int)GameLanguage.SimplifiedChinese && saved <= (int)GameLanguage.Japanese
                ? (GameLanguage)saved
                : GameLanguage.SimplifiedChinese;
        }
    }
}
