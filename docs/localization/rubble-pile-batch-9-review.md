# 碎石堆多语言化第九批审核稿：缴费单与欠款条

状态：第九批全部译文已获用户批准，并已完成代码接入。

既定人物与专名规则：主角日文自称使用「僕」；“丧家犬老大”继续使用 `Stray Dogs Boss` / `「負け犬」のボス`。丧家犬成员在英文对白中以 `devil` 而非 `demon` 称呼恶魔；本批老大只有沉默反应，没有直接说出该词。

范围：`RubblePile` 战斗期间调查“学生缴费单”与“欠款条”时触发的补充对话。本批不改变纸条位置、调查条件、暂停首领行为或战斗流程。

## 第一部分：学生缴费单

| 键 | 说话人 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `rubble.lore.tuition.1` | 主角 | 其实我们大学的学费也不贵啊…… | Our university's tuition isn't actually that expensive... | そもそも、僕たちの大学の学費はそこまで高くない…… |
| `rubble.lore.tuition.2` | 主角 | 毕竟现代社会人人都想当高收入的恶魔猎人，<br>已经不怎么重视文化教育了。 | After all, these days everyone wants to become a high-paid demon hunter.<br>No one places much value on education and culture anymore. | 今じゃ誰もが高収入の悪魔狩りになりたがって、<br>学問や教養はもうほとんど重んじられていないからな。 |
| `rubble.lore.tuition.3` | 主角 | 那你又有什么理由杀我？ | Then what reason do you have to kill me? | なら、僕を殺す理由がどこにある？ |
| `rubble.lore.tuition.4` | 丧家犬老大 | …… | ... | …… |

## 第二部分：欠款条

| 键 | 说话人 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `rubble.lore.debt.1` | 主角 | 这账单上的东西不太一般啊，不是水电费，而是…… | The charges on this bill are a little unusual. They're not for utilities, but... | この請求書、内容が少し変だ。水道光熱費じゃなくて…… |
| `rubble.lore.debt.2` | 主角 | 购买大牌恶魔猎人周边的消费。 | Merchandise from big-name demon hunters. | 有名な悪魔狩りのグッズを買った代金だ。 |
| `rubble.lore.debt.3` | 主角 | 看来你给儿子凑的钱好像不只是学费。 | Looks like the money you're scraping together for your son isn't just for tuition. | どうやら、息子のために工面している金は、学費だけじゃないようだな。 |
| `rubble.lore.debt.4` | 丧家犬老大 | …… | ... | …… |

## 审核要点

1. “文化教育”英文采用较宽泛的 `education and culture`，日文用「学問や教養」，保留社会轻视非猎魔教育与文化素养的含义。
2. “大牌恶魔猎人周边”不是猎魔装备，而是知名猎魔人的粉丝商品，因此译为 `Merchandise from big-name demon hunters.` / 「有名な悪魔狩りのグッズ」。
3. `scraping together` 对应“凑钱”，强调老大经济窘迫，同时揭示钱并非全用于学费。
4. 主角质问老大时，日文明确使用「僕」，遵循已确认的人物语气。
5. 两处老大的沉默均使用既有本地化说话人名称，而不是硬编码中文名称。
6. 用户已明确批准本文全部译文；代码接入以本文最终审定内容为准。
