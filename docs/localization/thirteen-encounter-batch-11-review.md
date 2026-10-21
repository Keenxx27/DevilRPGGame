# 荧光菌洞多语言化第十一批审核稿：与十三相遇

状态：第十一批全部译文已获用户批准，并已完成代码接入。

既定人物与专名规则：主角日文自称使用「僕」；角色名“十三”在英文版使用数字词 `Thirteen`，在日文版使用数字词「十三」，不使用音译。身份揭示前的说话人名称统一显示为 `???` / `？？？`。

范围：主角在 `FluorescentMushroomCave` 初次发现十三、按照提示放下肉干，以及十三赠送晶石并成为同行者的完整对话。本批不改变肉干物品 ID、放置判定、晶石演出、跟随状态或返家场景跳转。

## 第一部分：初次发现

| 键 | 说话人 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `thirteen.encounter.discovery.1` | 主角 | 那里有什么东西在动。 | Something is moving over there. | あそこで何かが動いている。 |
| `thirteen.encounter.discovery.2` | 主角 | 第一眼，真的像一只小黑猫。<br>它蜷缩在最大的荧光蘑菇下，正好奇地打量着我。 | At first glance, it really does look like a little black cat.<br>It's curled up beneath the largest glowing mushroom, watching me curiously. | 一見すると、本当に小さな黒猫のようだ。<br>一番大きな光るキノコの下で丸くなり、興味深そうに僕を見つめている。 |
| `thirteen.encounter.discovery.3` | 主角 | 但它不是猫。耳朵的位置是两片薄膜般的结构，<br>尾巴尖还燃着一小簇冰蓝色的火焰。 | But it isn't a cat. In place of ears are two thin, membrane-like structures,<br>and a small ice-blue flame burns at the tip of its tail. | だが、猫ではない。耳の位置には薄い膜のような器官が二つあり、<br>尻尾の先には小さな氷青色の炎が灯っている。 |

## 第二部分：放下肉干后的互动

| 键 | 说话人 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `thirteen.encounter.gift.1` | 主角 | 它没有看肉干，反而慢慢靠近了我。 | It doesn't even look at the dried meat. Instead, it slowly approaches me. | 干し肉には目もくれず、代わりにゆっくりと僕へ近づいてくる。 |
| `thirteen.encounter.gift.2` | 主角 | 它抬起前爪，小心翼翼地碰了碰我的手指。<br>冰凉，还有一点粗糙。 | It raises a forepaw and carefully touches my finger.<br>Cold, and a little rough. | 前足を上げ、恐る恐る僕の指に触れる。<br>冷たくて、少しざらついている。 |
| `thirteen.encounter.gift.3` | 主角 | 它又跑回蘑菇丛，叼来一块内部仿佛流淌着星光的晶石。<br>这是......你的礼物吗？ | It runs back into the mushrooms and returns with a crystal that seems to have starlight flowing inside.<br>Is this... a gift for me? | キノコの群れへ駆け戻り、中に星明かりが流れているような結晶をくわえてくる。<br>これは……僕への贈り物なのか？ |
| `thirteen.encounter.gift.4` | 主角 | 我……我没有恶意。 | I... I mean you no harm. | 僕は……君を傷つけるつもりはない。 |
| `thirteen.encounter.gift.5` | ？？？ | （它发出一个由三个高低不同音调组成的清脆音节。） | (It lets out a clear, chiming syllable composed of three notes at different pitches.) | （高さの異なる三つの音からなる、澄んだ音節を発する。） |
| `thirteen.encounter.gift.6` | 主角 | 十三……<br>这个名字，怎么会忽然出现在我脑子里？ | Thirteen...<br>Why did that name suddenly appear in my mind? | 十三……<br>なぜ、この名前が突然僕の頭に浮かんだんだ？ |
| `thirteen.encounter.gift.7` | 十三 | （它歪了歪头，耳膜轻轻转动。） | (It tilts its head, its ear membranes gently swiveling.) | （首をかしげ、耳の膜をそっと動かす。） |
| `thirteen.encounter.gift.8` | 主角 | 我伸出手，掌心向上。<br>十三极轻地舔了一下我的指尖。 | I hold out my hand, palm up.<br>Thirteen gives the tip of my finger the lightest lick. | 僕は掌を上にして手を差し出す。<br>十三は僕の指先をほんの少しだけ舐めた。 |

## 第三部分：操作提示

| 键 | 用途 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `thirteen.ui.offerDriedMeat` | 放置肉干提示 | 选中肉干后，在十三面前按 Q 放下肉干 | With Dried Meat selected, press Q in front of Thirteen to place it down | 干し肉を選択し、十三の前でQキーを押して置く |

## 审核要点

1. “十三”作为名字时，英文全部使用 `Thirteen`，日文全部使用「十三」；旁白、说话人名称和操作提示保持一致。
2. 初见阶段以中性的 `it` 描写十三，避免在身份揭示前擅自指定性别。
3. `I mean you no harm.` 保留主角紧张但主动表达善意的语气；日文明确使用「僕」。
4. “三个高低不同音调组成的清脆音节”译为一个由三个不同音高构成的音节，而不是三句话或三个独立叫声。
5. “耳膜”在英日文中按照十三可转动的外部膜状听觉结构处理为 `ear membranes` / 「耳の膜」，不按人体鼓膜直译。
6. 肉干名称沿用已审定的 `Dried Meat` / 「干し肉」，不改变其内部物品 ID `apartment.driedMeat`。
7. 用户已明确批准本文全部译文；代码接入以本文最终审定内容为准。
