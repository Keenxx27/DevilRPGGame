# 郊区道路多语言化第七批审核稿：第二段道路

状态：第七批全部译文已获用户批准，并已完成代码接入。

既定人物与专名规则：主角日文自称使用「僕」；“丧家犬”统一译为 `Stray Dogs` / `負け犬`；帮派成员使用粗鲁、口语化的表达，并在英文中以 `devil` 而非 `demon` 称呼恶魔。

范围：`RoadToSuburbs2` 场景的进入独白、高级丧家犬成员首次抓住主角时的台词，以及抵达碎石堆入口前的独白。本批不包含碎石堆首领剧情、战斗界面或任何玩法改动。

| 键 | 说话人 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `suburbs.road2.intro.1` | 主角 | 更加复杂的道路。 | The road is even more complicated here. | さらに入り組んだ道だ。 |
| `suburbs.road2.intro.2` | 主角 | 话说回来……自言自语的现象越来越严重了。 | Come to think of it... I've been talking to myself more and more lately. | そういえば……最近、独り言がますますひどくなっている。 |
| `suburbs.road2.advancedMember.capture` | 丧家犬 | 不把你当恶魔拆了卖已经算好的了，滚回去！ | You're lucky we haven't carved you up and sold you as devil parts. Now get lost! | 悪魔としてバラして売らねえだけありがたいと思え。とっとと失せろ！ |
| `suburbs.road2.endpoint` | 主角 | 丧家犬们只期望能够平安无事地度过每一天。<br>他们常睡在一起，挤在后巷的某处安家过夜——<br>如同身处老鼠洞一般。 | The Stray Dogs ask for nothing more than to make it through each day unharmed.<br>They often sleep huddled together, making a home for the night somewhere in a back alley—<br>as if they were living in a rat hole. | 「負け犬」の連中は、ただ一日を無事に過ごせればそれでいい。<br>いつもどこかの路地裏に身を寄せ合い、一緒に眠って夜を明かす——<br>まるで鼠穴の中にいるように。 |

## 审核要点

1. `The road is even more complicated here.` 将“道路”理解为主角眼前的路线与障碍布局，而非抽象的人生道路。
2. `I've been talking to myself more and more lately.` 保留主角察觉自言自语加重的意味；日文没有强行加入自称，因此不与已确认的「僕」规则冲突。
3. 高级成员的英文 `carved you up and sold you as devil parts` 明确表达“当作恶魔拆解后售卖”，并遵循“丧家犬成员称恶魔为 `devil`”的用词规则；日文用「バラす」「売らねえ」「とっとと失せろ」表现粗野帮派口吻。
4. 出口独白中的 `ask for nothing more than` 强调他们的愿望仅仅是平安度日；`huddled together` 同时保留拥挤、抱团取暖与生存困境。
5. 用户已明确批准本文全部译文；代码接入以本文最终审定内容为准。
