# 郊区道路多语言化第六批审核稿：第一段道路

状态：待用户审核；尚未接入游戏代码。

既定人物与专名规则：主角日文自称使用「僕」；“丧家犬”统一译为 `Stray Dogs` / `負け犬`；“归墟”统一译为 `the Abyss` / `深淵`。

范围：`RoadToSuburbs1` 场景的进入独白、低级丧家犬成员首次拦路台词，以及抵达第二段道路入口前的独白。本批不包含第二段道路的高级成员台词、碎石堆首领剧情或任何玩法改动。

| 键 | 说话人 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `suburbs.road1.intro.1` | 主角 | 丧家犬没有总部，没有老板，没有规矩。 | The Stray Dogs have no headquarters, no boss, and no rules. | 「負け犬」には本部も、ボスも、決まりもない。 |
| `suburbs.road1.intro.2` | 主角 | 他们是恶魔猎人里最底层的一群，捡别人不要的恶魔残骸，<br>在归墟边缘翻垃圾，在黑市角落里摆地摊。 | They're the lowest rung among demon hunters, scavenging demon remains no one else wants,<br>rummaging through trash at the edge of the Abyss and hawking their finds in corners of the black market. | あいつらは悪魔狩りの中でも最底辺の連中だ。誰も欲しがらない悪魔の残骸を拾い、<br>深淵の縁でゴミを漁り、闇市の片隅に露店を出す。 |
| `suburbs.road1.intro.3` | 主角 | 恐怕半人半魔的我，也会成为他们捡的“垃圾”吧…… | I'm half demon myself. Maybe that makes me more “trash” for them to pick up... | 半人半魔の僕も、あいつらに拾われる「ゴミ」になるのだろうか…… |
| `suburbs.road1.intro.4` | 主角 | 最好绕着他们走，他们在马路上摆满了障碍物，能不被抢已经是最好的了。 | Best to steer clear of them. They've littered the road with barricades. If I make it through without getting robbed, I'll be lucky. | できるだけあいつらを避けて進もう。道路には障害物が並べられている。奪われずに通れれば、それだけで幸運だ。 |
| `suburbs.speaker.strayDogs` | 说话人名称 | 丧家犬 | Stray Dogs | 負け犬 |
| `suburbs.road1.lowMember.block` | 丧家犬 | 这是我们的地盘，没你过去的份！ | This is our turf. You're not getting through! | ここは俺たちの縄張りだ。お前を通す気はねえ！ |
| `suburbs.road1.endpoint.1` | 主角 | 呼……逃过一劫。 | Phew... That was close. | ふう……危ないところだった。 |
| `suburbs.road1.endpoint.2` | 主角 | 但前面的路就不好说了，可能会遇到更加危险的丧家犬成员。 | But I can't say the same for the road ahead. I may run into even more dangerous members of the Stray Dogs. | だが、この先もそううまくいくとは限らない。もっと危険な「負け犬」のメンバーに出くわすかもしれない。 |

## 审核要点

1. 英文中的 `the Stray Dogs` 作为帮派整体名称使用；说话人名称不加冠词，显示为 `Stray Dogs`。
2. `hawking their finds` 对应“摆地摊”，保留其靠拾荒物维生的意味，不误译成普通商贩经营。
3. 低级成员的日文使用粗鲁口语「俺たち」「～ねえ」，与帮派成员身份相符；这不是主角的自称。
4. 主角第三句明确使用「僕」，遵循已确认的人物语气规则。
5. 审核通过前，本批不会写入本地化词库或改动场景脚本。
