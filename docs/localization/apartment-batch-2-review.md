# 公寓多语言化第二批审核稿：客厅与林春宝主剧情

状态：第二批全部译文已获用户批准，可以进入代码接入

人物语气：主角日文自称「僕」；林春宝日文自称「俺」。

术语：林春宝口语中的“小归墟”使用 `small hole of Abyss` / `小さな深淵の穴`。

## 用户审定规则与修改记录

以下内容是用户在审核过程中明确提出或确认的约束。代码接入必须以“最终审定内容”为准，不得恢复早期草稿。

| 项目 | 早期草稿或风险 | 最终审定内容 | 后续约束 |
|---|---|---|---|
| 日文第一人称 | 主角曾误用「俺」 | 主角统一使用「僕」；林春宝统一使用「俺」 | 后续所有公寓批次继续执行 |
| “小归墟”的口语译法 | 曾建议较正式的 `minor Abyssal rift` / `小規模な深淵孔` | `small hole of Abyss` / `小さな深淵の穴` | 仅用于林春宝的口语；正式机构术语另行处理 |
| “完了，我哥读书读傻了。”英文 | `It's hopeless. All that studying has scrambled my brother's brain.` | `Well, all that studying has scrambled up your brain.` | 必须逐字使用审定版 |
| “完了，我哥读书读傻了。”日文 | `だめだ。兄ちゃんは勉強しすぎて頭がおかしくなった。` | `あーあ、兄ちゃん、勉強しすぎて頭がこんがらがっちまったな。` | 保留林春宝随意、带调侃的口吻 |
| 结局 1 中文末句 | `了无归属地活下去。` | `永远找不到归属。` | 与“无归属者”主题呼应 |
| 结局 1 标题及说明 | 原代码只有标题 `结局：关我何事` | 使用本文“已批准：结局 1 字幕”中的完整中英日标题与四行说明 | 标题下必须新增说明文字，不得只替换标题 |
| 第二批第三部分 | 初稿提交后无修改意见 | 本文第三部分列出的 26 条译文全部获批 | 按表格逐字接入 |

## 第一部分：厨房初遇与餐桌谈话

| 键 | 说话人/用途 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.kitchen.encounter.1` | 主角 | 高手？ | An expert, huh? | 達人？ |
| `apartment.kitchen.encounter.2` | 林春宝 | 那是。搏击社的训练包括手腕稳定性和空间感，<br>懂不懂？这算应用练习。 | You bet. Combat club training includes wrist control and spatial awareness.<br>Get it? This is practical training. | 当然だろ。格闘部の練習には手首の安定性と空間認識も含まれるんだ。<br>分かるか？　これは実践練習だ。 |
| `apartment.kitchen.appearance.question` | 林春宝问题 | 今天没贴那个丑兮兮的胶布？ | Not wearing that ugly bandage today? | 今日はあのダサい絆創膏、貼ってないのか？ |
| `apartment.kitchen.appearance.answer.1` | 主角选项 | 1. 天热，闷。 | 1. It's hot. It gets stuffy. | 1. 暑くて、蒸れるから。 |
| `apartment.kitchen.appearance.answer.2` | 主角选项 | 2. 确实忘记了，等会能帮我拿一个吗？ | 2. I did forget it. Could you get me one later? | 2. 確かに忘れた。あとで一枚取ってきてくれる？ |
| `apartment.kitchen.appearance.answer.3` | 主角选项 | 3. 这不关你的事。 | 3. That's none of your business. | 3. 君には関係ない。 |
| `apartment.kitchen.appearance.response.1` | 林春宝回应 | 挺好。 | Good. | そっか。 |
| `apartment.kitchen.appearance.response.2` | 林春宝回应 | 那行，就在洗手间药柜里。 | All right. They're in the medicine cabinet in the bathroom. | 分かった。洗面所の薬棚にある。 |
| `apartment.kitchen.appearance.response.3` | 林春宝回应 | 你什么时候变成这样了？ | When did you get like this? | いつからそんなふうになったんだ？ |
| `apartment.kitchen.table.1` | 林春宝 | 反正家里就我，看着看着也习惯了。<br>说起来—— | It's just me at home anyway. I've gotten used to seeing it.<br>Speaking of which— | どうせ家には俺しかいないし、見慣れてる。<br>そういえば—— |
| `apartment.kitchen.table.2` | 主角 | 嗯？ | Hmm? | ん？ |
| `apartment.kitchen.table.3` | 林春宝 | 下午路过郊区，听说又有个小归墟开了，<br>指甲盖那么大。猎魔机构的人去了，<br>封了三条街，吵死了。 | I passed by the suburbs this afternoon and heard another small hole of Abyss had opened.<br>It was no bigger than a fingernail, but the demon-hunting agency showed up<br>and blocked off three streets. What a racket. | 今日の午後、郊外を通ったら、また小さな深淵の穴が開いたらしい。<br>爪くらいの大きさなのに、悪魔狩り機関の連中が来て、<br>通りを三本も封鎖したんだ。うるさくてたまらなかった。 |
| `apartment.kitchen.table.4` | 主角 | 然后呢？ | And then? | それで？ |
| `apartment.kitchen.table.5` | 林春宝 | 还能然后？老一套呗。<br>清场，开枪，广播叫大家别慌。 | What else? Same old routine.<br>Clear the area, open fire, then announce that everyone should stay calm. | ほかに何があるんだ？　いつものやつだよ。<br>人を追い出して、銃を撃って、放送で落ち着けって言う。 |
| `apartment.kitchen.table.6` | 主角 | …… | ... | …… |
| `apartment.kitchen.table.7` | 林春宝 | 我跟你说，社长我今天教了新招，<br>侧身闪接低扫腿，配合呼吸节奏，<br>发力点特别—— | I'm telling you, your club president taught them a new move today—<br>sidestep into a low sweep, timed with your breathing.<br>The key is where you put your force— | 聞いてくれよ。部長の俺が今日は新しい技を教えたんだ。<br>横にかわしてから下段払い、呼吸のリズムも合わせる。<br>力を入れるポイントが特に—— |
| `apartment.kitchen.table.8` | 主角 | …… | ... | …… |
| `apartment.kitchen.concern.question` | 林春宝问题 | 哥，你又在想奇怪的事。 | Bro, you're thinking about something weird again. | 兄ちゃん、また変なこと考えてるだろ。 |
| `apartment.kitchen.concern.answer.1` | 主角选项 | 1. 是的 | 1. Yes. | 1. そうだ。 |
| `apartment.kitchen.concern.answer.2` | 主角选项 | 2. 没有 | 2. No. | 2. いや。 |
| `apartment.kitchen.concern.response.1` | 林春宝回应 | 我就说嘛，有什么心事说给我听听！ | I knew it. Tell me what's on your mind! | やっぱりな。何を悩んでるのか、俺に話してみろよ！ |
| `apartment.kitchen.concern.response.2` | 林春宝回应 | 有。每次你露出这种‘全世界都错了就我对’的表情，<br>就是你脸上这玩意最红的时候。<br>现在它就在发亮。 | Yes, you are. Whenever you get that “everyone in the world is wrong except me” look,<br>that thing on your face turns its reddest.<br>It's glowing right now. | 考えてる。兄ちゃんが「世界中が間違っていて、自分だけが正しい」って顔をすると、<br>顔のそれが一番赤くなるんだ。<br>今だって光ってる。 |

## 第二部分：恶魔观念争论与最终选择

| 键 | 说话人/用途 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.kitchen.belief.1` | 主角 | ……我只是觉得，他们从来不去想，<br>有没有别的可能性。 | ...I just think they never stop to consider<br>whether there might be another possibility. | ……僕はただ、あの人たちは別の可能性があるかもしれないって、<br>考えようともしないと思っただけだ。 |
| `apartment.kitchen.belief.2` | 林春宝 | 恶魔哎，大哥。 | They're demons, bro. | 悪魔だぞ、兄ちゃん。 |
| `apartment.kitchen.belief.3` | 林春宝 | 它们从归墟地洞里爬出来，见人就扑，<br>资料片里都播了无数遍了。能有什么可能性？<br>难道请它们喝茶？ | They crawl out of holes of Abyss and pounce on anyone they see.<br>We've watched that footage countless times. What other possibility could there be?<br>Should we invite them over for tea? | あいつらは深淵の穴から這い出して、見境なく人に襲いかかる。<br>資料映像で何度も見ただろ。ほかにどんな可能性があるんだ？<br>お茶にでも誘えっていうのか？ |
| `apartment.kitchen.belief.4` | 主角 | 资料片也是人拍的。也许……<br>它们只是不知道该怎么和我们打招呼。 | That footage was made by people too. Maybe...<br>they just don't know how to greet us. | あの映像だって、人が撮ったものだ。もしかしたら……<br>彼らは僕たちへの挨拶の仕方を知らないだけかもしれない。 |
| `apartment.kitchen.belief.5` | 林春宝动作 | （重重靠回椅背大笑起来） | (She throws herself back against the chair and bursts out laughing.) | （勢いよく椅子の背にもたれ、大声で笑い出す。） |
| `apartment.kitchen.belief.6` | 林春宝 | 完了，我哥读书读傻了。 | Well, all that studying has scrambled up your brain. | あーあ、兄ちゃん、勉強しすぎて頭がこんがらがっちまったな。 |
| `apartment.kitchen.belief.7` | 林春宝 | …… | ... | …… |
| `apartment.kitchen.belief.8` | 林春宝 | 你要不要重新考虑考虑？ | Want to think that over? | もう一度、考え直してみないか？ |
| `apartment.kitchen.reconsider.question` | 林春宝问题 | 你要不要重新考虑考虑？ | Want to think that over? | もう一度、考え直してみないか？ |
| `apartment.kitchen.reconsider.answer.1` | 主角选项 | 1. 我是认真的 | 1. I'm serious. | 1. 本気だ。 |
| `apartment.kitchen.reconsider.answer.2` | 主角选项 | 2. 你说的对 | 2. You're right. | 2. 春宝の言うとおりだ。 |
| `apartment.kitchen.resolve.1` | 林春宝，认真路线 | 听着，不管你怎么想，外面的人可不会陪你发呆。<br>真有哪天哪个不长眼的恶魔找你麻烦—— | Listen. Whatever you believe, people out there won't wait around while you daydream.<br>If some dumb demon ever comes looking for trouble— | いいか。兄ちゃんが何を考えていようと、外の連中はぼんやり待ってなんかくれない。<br>いつか物の分からない悪魔が兄ちゃんに手を出してきたら—— |
| `apartment.kitchen.resolve.2` | 林春宝，认真路线 | 有岔子，交给我。 | If anything goes wrong, leave it to me. | 何かあったら、俺に任せろ。 |
| `apartment.kitchen.resolve.3` | 主角，认真路线 | 这样看来，我也不该耽搁了，<br>我现在就去郊区。 | In that case, I shouldn't waste any more time either.<br>I'll head to the suburbs now. | そういうことなら、僕もぐずぐずしてはいられない。<br>今から郊外へ行く。 |
| `apartment.kitchen.resolve.4` | 林春宝，认真路线 | 行啊。不过我建议你从食品柜里拿点东西带上，<br>万一有必要吸引它们呢。 | Sure. But you should take something from the food cabinet.<br>You might need it to lure them over. | いいぞ。でも、食品棚から何か持っていったほうがいい。<br>あいつらをおびき寄せる必要があるかもしれないからな。 |
| `apartment.kitchen.dismissive.1` | 林春宝，退缩路线 | 好吧，看来我哥脑子本质上还是正常的。 | All right. Looks like my brother is basically sane after all. | そうか。兄ちゃんの頭は根本的にはまともだったみたいだな。 |
| `apartment.kitchen.dismissive.2` | 林春宝，退缩路线 | 也许不过是被那帮坏同学欺负后的气话罢了，<br>对吗？ | Maybe you only said all that because those jerks at school bullied you.<br>Right? | 学校の嫌な連中にいじめられて、つい言ってみただけなんだろ。<br>そうだよな？ |
| `apartment.kitchen.dismissive.3` | 主角，退缩路线 | …… | ... | …… |
| `apartment.kitchen.dismissive.4` | 林春宝，退缩路线 | 得了，好好吃晚餐吧。 | Enough of that. Let's just eat dinner. | もういい。ちゃんと夕飯を食べよう。 |
| `apartment.ui.choiceTutorial` | 选择操作提示 | 通过按下数字键选择答复，按下E键确认答复 | Press a Number Key to Select a Response, then Press E to Confirm | 数字キーで返答を選び、Eキーで決定 |

## 已批准：结局 1 字幕

| 键 | 用途 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.ending.indifferent.title` | 结局标题 | 结局：关我何事 | Ending: None of My Concern | 結末：僕には関係ない |
| `apartment.ending.indifferent.1` | 结尾说明 | 你选择了不向自己最亲近的人说出真相。 | You chose not to tell the truth to the person closest to you. | あなたは、最も身近な人に真実を告げないことを選んだ。 |
| `apartment.ending.indifferent.2` | 结尾说明 | 兴许，“与恶魔成为朋友”不过是个笑话——哪怕对你而言也是如此。 | Perhaps “befriending demons” was nothing more than a joke—even to you. | もしかすると、「悪魔と友達になる」など、ただの冗談だったのかもしれない。あなた自身にとってさえ。 |
| `apartment.ending.indifferent.3` | 结尾说明 | 你将继续以一个被歧视、被排挤、被狩猎之人的身份活下去。 | You will go on living as someone discriminated against, cast out, and hunted. | あなたはこれからも、差別され、疎外され、狩られる者として生きていく。 |
| `apartment.ending.indifferent.4` | 结尾说明 | 永远找不到归属。 | Never finding a place to belong. | 永遠に、居場所を見つけられないまま。 |

## 第三部分：客厅互动、沙袋与手套柜

| 键 | 说话人/用途 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.livingRoom.lin.first` | 林春宝 | 在家里找找看，也许能有解开密码的线索。 | Look around the apartment. There might be a clue to the password somewhere. | 家の中を探してみろよ。パスワードを解く手掛かりがあるかもしれない。 |
| `apartment.livingRoom.lin.repeat` | 林春宝 | 看我干嘛，我怎么知道密码？ | Why are you looking at me? How would I know the password? | なんで俺を見るんだよ。パスワードなんて知るわけないだろ？ |
| `apartment.livingRoom.lin.departureRepeat` | 林春宝 | 你还磨蹭什么？我还要补沙袋呢！ | What are you still dawdling for? I've got a sandbag to mend! | 何をまだぐずぐずしてるんだ？　俺はサンドバッグも直さなきゃならないんだぞ！ |
| `apartment.livingRoom.phoneReward.1` | 林春宝 | 哥，你居然把我手机里那关小游戏打过去了？<br>我卡了好几天呢。 | Bro, you actually beat that minigame on my phone?<br>I'd been stuck on it for days. | 兄ちゃん、俺のスマホのミニゲーム、クリアしたのか？<br>何日も詰まってたんだぞ。 |
| `apartment.livingRoom.phoneReward.2` | 主角 | 只是躲开几块障碍而已。<br>你把这种东西当训练？ | All I did was dodge a few obstacles.<br>You call that training? | 障害物をいくつか避けただけだ。<br>こんなものをトレーニングにしているのか？ |
| `apartment.livingRoom.phoneReward.3` | 林春宝 | 当然，练反应和手眼协调。<br>对了，我刚在你床头柜旁边找到这个。 | Of course. It trains your reflexes and hand-eye coordination.<br>Oh, right. I found this beside your nightstand. | もちろん。反射神経と目と手の連携を鍛えるんだ。<br>そうだ、さっき兄ちゃんのベッド脇の棚のそばでこれを見つけた。 |
| `apartment.livingRoom.phoneReward.4` | 林春宝 | 估计是你翻东西时掉的。拿好，<br>别又把钥匙和膏药混在一起。 | You probably dropped it while rummaging around. Here.<br>Don't mix up your keys and plasters again. | 物を探してるときに落としたんだろ。ほら。<br>また鍵と膏薬を一緒にするなよ。 |
| `apartment.livingRoom.phoneReward.5` | 主角 | ……谢谢。 | ...Thank you. | ……ありがとう。 |
| `apartment.livingRoom.repair.1` | 林春宝 | 你是把那胶布贴到了菜刀上？ | Did you stick that plaster onto the kitchen knife? | あの膏薬を包丁に貼りつけたのか？ |
| `apartment.livingRoom.repair.2` | 主角 | 也许这膏药能派上用场…… | Maybe this plaster can still be useful... | この膏薬が役に立つかもしれない…… |
| `apartment.livingRoom.repair.3` | 林春宝 | 就它了！要是用针线缝缝补补的要花掉我一整晚。 | That's perfect! Stitching it up with a needle and thread would take me all night. | それだ！　針と糸で縫ってたら一晩中かかっちまう。 |
| `apartment.livingRoom.repair.4` | 主角 | 所以…… | So... | それで…… |
| `apartment.livingRoom.repair.5` | 林春宝 | 嗯？ | Hmm? | ん？ |
| `apartment.livingRoom.repair.6` | 主角 | 我能去你的房间里找线索了吗？ | Can I look for clues in your room now? | 春宝の部屋で手掛かりを探してもいいか？ |
| `apartment.livingRoom.repair.7` | 林春宝 | 随便你，去吧！ | Do whatever you want. Go ahead! | 好きにしろ。行ってこい！ |
| `apartment.livingRoom.sandbag.1` | 主角 | 这是春宝平时拿来锻炼用的沙袋。 | This is the sandbag Chunbao usually trains with. | これは春宝が普段のトレーニングに使っているサンドバッグだ。 |
| `apartment.livingRoom.sandbag.2` | 主角 | 春宝在高中里是搏击社的社长，<br>一直表现突出。 | Chunbao is the president of her high school's combat club.<br>She's always stood out there. | 春宝は高校の格闘部で部長を務めていて、<br>ずっと目立った活躍をしている。 |
| `apartment.livingRoom.sandbag.ready` | 林春宝 | 要把我这沙袋想象成房东那张可恶的大脸<br>打一拳的话，最好先带上拳击手套。 | If you're going to picture our landlord's nasty face on my sandbag<br>and take a swing at it, you'd better put on the boxing gloves first. | 俺のサンドバッグを、あの憎たらしい大家の顔だと思って<br>殴るなら、先にボクシンググローブをつけたほうがいいぞ。 |
| `apartment.livingRoom.gloveCabinet.intro.1` | 林春宝 | 我才想起来，拳击手套放在这里头。 | I just remembered—the boxing gloves are in here. | 今思い出した。ボクシンググローブはこの中だ。 |
| `apartment.livingRoom.gloveCabinet.intro.2` | 主角 | 看上去柜门铰链生锈了开不了啊。 | Looks like the cabinet won't open because the hinges have rusted shut. | 蝶番が錆びついて、扉が開かないみたいだ。 |
| `apartment.livingRoom.gloveCabinet.intro.3` | 林春宝 | 那就让它开得了！ | Then make it open! | なら、開くようにしろ！ |
| `apartment.livingRoom.gloveCabinet.repeat` | 主角 | 柜门的铰链锈死了，得找东西撬开。 | The cabinet hinges are rusted solid. I need something to pry it open. | 扉の蝶番が完全に錆びついている。何かでこじ開けないと。 |
| `apartment.livingRoom.gloveCabinet.open.1` | 主角 | 菜刀正好能撬开这扇柜门。 | The kitchen knife is just right for prying this cabinet open. | 包丁なら、この扉をこじ開けるのにちょうどよさそうだ。 |
| `apartment.livingRoom.gloveCabinet.open.2` | 主角 | 拳击手套在里面。 | The boxing gloves are inside. | 中にボクシンググローブがある。 |
| `apartment.ui.boxing.stage` | 拳击训练阶段 | 拳击训练　第 {0} / 3 阶段 | Boxing Training  Stage {0} / 3 | ボクシング訓練　第 {0} / 3 段階 |
| `apartment.item.boxingGloves` | 物品显示名 | 拳击手套 | Boxing Gloves | ボクシンググローブ |

## 第一部分审核重点

- `small hole of Abyss` / `小さな深淵の穴` 是否符合林春宝的口语。
- 林春宝称呼主角使用 `Bro` / `兄ちゃん`。
- 主角第三个选择的日文 `君には関係ない` 是否足够符合他较礼貌但疏离的语气。
- “社长我”处理为英文 `your club president`、日文 `部長の俺`，保留她自信张扬的感觉。

## 审核状态

- [x] 第一部分英文获批
- [x] 第一部分日文获批
- [x] 第一部分键名获批
- [x] 允许继续提交第二部分译文
- [x] 第二部分英文获批
- [x] 第二部分日文获批
- [x] 第二部分键名获批
- [x] 结局 1 标题与说明获批
- [x] 允许继续提交第三部分译文
- [x] 第三部分英文获批
- [x] 第三部分日文获批
- [x] 第三部分键名获批
- [x] 第二批全部译文获批
- [x] 允许进入第二批代码接入
