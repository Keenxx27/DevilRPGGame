using System.Collections.Generic;

namespace RPG
{
    public static class SuburbsLocalizationTexts
    {
        public static Dictionary<string, string[]> Create()
        {
            return new Dictionary<string, string[]>
            {
                { "suburbs.road1.intro.1", new[]
                    {
                        "丧家犬没有总部，没有老板，没有规矩。",
                        "The Stray Dogs have no headquarters, no boss, and no rules.",
                        "「負け犬」には本部も、ボスも、決まりもない。"
                    }
                },
                { "suburbs.road1.intro.2", new[]
                    {
                        "他们是恶魔猎人里最底层的一群，捡别人不要的恶魔残骸，\n在归墟边缘翻垃圾，在黑市角落里摆地摊。",
                        "They're the lowest rung among demon hunters, scavenging demon remains no one else wants,\nrummaging through trash at the edge of the Abyss and hawking their finds in corners of the black market.",
                        "あいつらは悪魔狩りの中でも最底辺の連中だ。誰も欲しがらない悪魔の残骸を拾い、\n深淵の縁でゴミを漁り、闇市の片隅に露店を出す。"
                    }
                },
                { "suburbs.road1.intro.3", new[]
                    {
                        "恐怕半人半魔的我，也会成为他们捡的“垃圾”吧……",
                        "I'm half demon myself. Maybe that makes me more “trash” for them to pick up...",
                        "半人半魔の僕も、あいつらに拾われる「ゴミ」になるのだろうか……"
                    }
                },
                { "suburbs.road1.intro.4", new[]
                    {
                        "最好绕着他们走，他们在马路上摆满了障碍物，能不被抢已经是最好的了。",
                        "Best to steer clear of them. They've littered the road with barricades. If I make it through without getting robbed, I'll be lucky.",
                        "できるだけあいつらを避けて進もう。道路には障害物が並べられている。奪われずに通れれば、それだけで幸運だ。"
                    }
                },
                { "suburbs.speaker.strayDogs", new[] { "丧家犬", "Stray Dogs", "負け犬" } },
                { "suburbs.road1.lowMember.block", new[]
                    {
                        "这是我们的地盘，没你过去的份！",
                        "This is our turf. You're not getting through!",
                        "ここは俺たちの縄張りだ。お前を通す気はねえ！"
                    }
                },
                { "suburbs.road1.endpoint.1", new[]
                    {
                        "呼……逃过一劫。", "Phew... That was close.", "ふう……危ないところだった。"
                    }
                },
                { "suburbs.road1.endpoint.2", new[]
                    {
                        "但前面的路就不好说了，可能会遇到更加危险的丧家犬成员。",
                        "But I can't say the same for the road ahead. I may run into even more dangerous members of the Stray Dogs.",
                        "だが、この先もそううまくいくとは限らない。もっと危険な「負け犬」のメンバーに出くわすかもしれない。"
                    }
                },
                { "suburbs.road2.intro.1", new[]
                    {
                        "更加复杂的道路。", "The road is even more complicated here.", "さらに入り組んだ道だ。"
                    }
                },
                { "suburbs.road2.intro.2", new[]
                    {
                        "话说回来……自言自语的现象越来越严重了。",
                        "Come to think of it... I've been talking to myself more and more lately.",
                        "そういえば……最近、独り言がますますひどくなっている。"
                    }
                },
                { "suburbs.road2.advancedMember.capture", new[]
                    {
                        "不把你当恶魔拆了卖已经算好的了，滚回去！",
                        "You're lucky we haven't carved you up and sold you as devil parts. Now get lost!",
                        "悪魔としてバラして売らねえだけありがたいと思え。とっとと失せろ！"
                    }
                },
                { "suburbs.road2.endpoint", new[]
                    {
                        "丧家犬们只期望能够平安无事地度过每一天。\n他们常睡在一起，挤在后巷的某处安家过夜——\n如同身处老鼠洞一般。",
                        "The Stray Dogs ask for nothing more than to make it through each day unharmed.\nThey often sleep huddled together, making a home for the night somewhere in a back alley—\nas if they were living in a rat hole.",
                        "「負け犬」の連中は、ただ一日を無事に過ごせればそれでいい。\nいつもどこかの路地裏に身を寄せ合い、一緒に眠って夜を明かす——\nまるで鼠穴の中にいるように。"
                    }
                }
            };
        }
    }
}
