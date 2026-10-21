# 碎石堆多语言化第八批审核稿：丧家犬老大遭遇战

状态：第八批全部译文已获用户批准，并已完成代码接入。

既定人物与专名规则：主角日文自称使用「僕」；“丧家犬”统一译为 `Stray Dogs` / `負け犬`；“丧家犬老大”译为 `Stray Dogs Boss` / `「負け犬」のボス`。丧家犬成员在英文对白中以 `devil` 而非 `demon` 称呼恶魔；主角与旁白仍使用 `demon`。角色名“十三”在英文与日文中均直接使用对应的数字词 `Thirteen` / `十三`，不作音译（本批尚未出现该角色）。

范围：`RubblePile` 场景进入后的尸体调查与丧家犬老大对峙、战斗进度提示，以及抓捕失败三次后出现的“梦想折翼”结局标题。本批不包含两张剧情纸条的补充对话，也不改变战斗、抓捕、开关或结局触发逻辑。

## 第一部分：遭遇战开场

| 键 | 说话人 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `rubble.speaker.strayDogsBoss` | 说话人名称 | 丧家犬老大 | Stray Dogs Boss | 「負け犬」のボス |
| `rubble.encounter.1` | 主角 | 那里有什么东西。 | There's something over there. | あそこに何かある。 |
| `rubble.encounter.2` | 主角 | 是一只……恶魔。 | It's a... demon. | 悪魔だ……。 |
| `rubble.encounter.3` | 主角 | 它看起来不像新闻里那些狰狞的怪物。<br>更像一只走错地方的、古怪的蜥蜴。 | It doesn't look like those hideous monsters on the news.<br>It's more like some strange lizard that wandered into the wrong place. | ニュースで見るような、恐ろしい怪物には見えない。<br>むしろ、迷い込んできた奇妙なトカゲのようだ。 |
| `rubble.encounter.4` | 主角 | …… | ... | …… |
| `rubble.encounter.5` | 主角 | 可它已经死了。 | But it's already dead. | だが、もう死んでいる。 |
| `rubble.encounter.6` | 主角 | 看来是旁边的电棍造成的。 | It looks like the stun baton beside it did this. | そばにあるスタンバトンでやられたようだ。 |
| `rubble.encounter.7` | 主角 | （左脸的痕迹传来一阵细微的、冰凉的悸动，<br>仿佛在共鸣某种遥远的痛苦。） | (A faint, icy throb runs through the mark on the left side of my face,<br>as though it were resonating with some distant pain.) | （左頬の痕に、かすかな冷たい疼きが走る。<br>まるで、遠い痛みと共鳴しているかのようだ。） |
| `rubble.encounter.8` | 丧家犬老大 | 在这儿干什么呢你？没看到我正在进货吗？ | What're you doing here? Can't you see I'm stocking up? | こんな所で何してやがる？　俺が仕入れ中なのが見えねえのか？ |
| `rubble.encounter.9` | 主角 | 没……没什么。 | N-Nothing... | な……何でもない。 |
| `rubble.encounter.10` | 丧家犬老大 | 别以为你脸上那片膏药能瞒过我，我儿子跟你大学一个专业的，<br>早跟我说过你。你这半人半魔的怪物！ | Don't think that thing on your face can fool me. My son's in the same major as you,<br>and he's told me all about you. You freak! | 顔の膏薬で俺の目をごまかせると思うな。息子は大学でお前と同じ専攻で、<br>とっくにお前のことを聞いてるんだ。この半人半魔の化け物め！ |
| `rubble.encounter.11` | 主角 | 你已经杀了那只恶魔，还不够吗？ | You've already killed that demon. Isn't that enough? | もうあの悪魔を殺しただろう。それでも足りないのか？ |
| `rubble.encounter.12` | 丧家犬老大 | 还不够，凑儿子的学费还不够！ | Not enough! It's not enough to cover my son's tuition! | 足りねえ！　息子の学費にはまだ足りねえんだ！ |
| `rubble.encounter.13` | 丧家犬老大 | 还不够让他读完下一年，好不再做我这恶心的营生！ | Not enough to get him through another year, so he'll never have to do this filthy work I do! | あいつがもう一年通いきって、俺みたいな汚え商売をしなくて済むには、まだ足りねえ！ |
| `rubble.encounter.14` | 丧家犬老大 | 但依我看，抓到了你这半魔……抵得上再抓五十只……<br>不，一百只刚才那样的！ | But the way I see it, catching a half-devil like you... would be worth another fifty...<br>No, a hundred of those things! | だが俺の見立てじゃ、お前みたいな半魔を捕まえりゃ……さっきのをあと五十匹……<br>いや、百匹捕まえるのと同じ値打ちだ！ |

## 第二部分：战斗界面与失败结局

| 键 | 用途 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `rubble.ui.progress` | 开关进度，`{0}` 为已开启数量 | 逃离丧家犬老大：已开启 {0} / 4 个开关 | Escape the Stray Dogs Boss: {0} / 4 Switches Activated | 「負け犬」のボスから逃げろ：スイッチ {0} / 4 起動 |
| `rubble.ui.escape` | 四个开关全部开启后的提示 | 尽快进入铁门离开 | Get through the iron gate—quick! | 急いで鉄扉から脱出しろ！ |
| `rubble.ending.brokenDreams.title` | 失败结局标题 | 结局：梦想折翼 | Ending: Dreams Cut Short | 結末：夢、潰える |
| `rubble.ending.brokenDreams.1` | 标题下方字幕 | 你倒在了这个集杀手与父亲于一体的男人的刀下，再也不会醒来。 | You fell to the blade of this man, both killer and father, never to wake again. | 殺し屋であり、父親でもあるこの男の刃に倒れ、あなたは二度と目を覚ますことはない。 |
| `rubble.ending.brokenDreams.2` | 标题下方字幕 | 你深知，法律不可能对他予以制裁，他甚至可能在猎魔机构里得到些认可。 | You know full well that the law will never punish him. He may even earn some recognition from the demon-hunting agency. | 法が彼を裁くことはないと、あなたはよく分かっている。彼は悪魔狩り機関から多少の評価すら得るかもしれない。 |
| `rubble.ending.brokenDreams.3` | 标题下方字幕 | 麻袋套上你的头前，你还在挂念着你最亲近的人，毕竟日后她只能靠自己了。 | Before the sack is pulled over your head, your thoughts are still with the person closest to you. After all, from now on, she'll have to fend for herself. | 麻袋を頭にかぶせられる前も、あなたは最も親しい人のことを案じている。これからは、彼女一人で生きていくしかないのだから。 |
| `rubble.ending.brokenDreams.4` | 标题下方字幕 | 但至少......她不用再同一个半人半魔的家伙过日子了。 | But at least... she won't have to live with someone who's half human and half demon anymore. | だが、少なくとも……彼女はもう、半人半魔のやつと暮らさずに済む。 |
| `rubble.ending.brokenDreams.5` | 标题下方字幕 | 眼前什么都没有。 | There is nothing before you. | 目の前には、何もない。 |

## 审核要点

1. 主角观察恶魔尸体时使用 `demon`；丧家犬老大在第 14 句中称主角为 `half-devil`。第 10 句经用户修订后不再直接说出 `devil`，但双方不同的英语用词规则保持不变。
2. 老大所说的“进货”保留为黑色幽默式的 `stocking up` / 「仕入れ中」，表现他把恶魔尸体视作商品。
3. 第 10 句英文按用户审定稿使用概括性的 `that thing on your face` 与直接辱骂 `You freak!`，不在英文中明确说出膏药或“半人半魔”；中文与日文保持原意。
4. `this filthy work I do` / 「俺みたいな汚え商売」强调老大本人厌恶这门营生，同时仍希望儿子借教育脱身。
5. “梦想折翼”英文采用含有夭折意味的标题式表达 `Dreams Cut Short`；日文采用较凝练的 `夢、潰える`，而非逐字翻译翅膀折断。
6. “梦想折翼”页面将在标题下显示用户提供的五句结尾字幕；最后两句保留主角临死前将自己视作亲近之人负担的残酷反讽。
7. 第 2 句属于结局旁白，因此使用 `demon-hunting agency`，不采用丧家犬成员口语中的 `devil`。
8. 用户没有要求该结局增加“感谢游玩”页面，本批不自行添加。
9. 用户已明确批准本文全部译文；代码接入以本文最终审定内容为准。
