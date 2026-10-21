using System.Collections.Generic;

namespace RPG
{
    public static class RubbleLocalizationTexts
    {
        public static Dictionary<string, string[]> Create()
        {
            return new Dictionary<string, string[]>
            {
                { "rubble.speaker.strayDogsBoss", new[] { "丧家犬老大", "Stray Dogs Boss", "「負け犬」のボス" } },
                { "rubble.encounter.1", new[] { "那里有什么东西。", "There's something over there.", "あそこに何かある。" } },
                { "rubble.encounter.2", new[] { "是一只……恶魔。", "It's a... demon.", "悪魔だ……。" } },
                { "rubble.encounter.3", new[]
                    {
                        "它看起来不像新闻里那些狰狞的怪物。\n更像一只走错地方的、古怪的蜥蜴。",
                        "It doesn't look like those hideous monsters on the news.\nIt's more like some strange lizard that wandered into the wrong place.",
                        "ニュースで見るような、恐ろしい怪物には見えない。\nむしろ、迷い込んできた奇妙なトカゲのようだ。"
                    }
                },
                { "rubble.encounter.4", new[] { "……", "...", "……" } },
                { "rubble.encounter.5", new[] { "可它已经死了。", "But it's already dead.", "だが、もう死んでいる。" } },
                { "rubble.encounter.6", new[]
                    {
                        "看来是旁边的电棍造成的。",
                        "It looks like the stun baton beside it did this.",
                        "そばにあるスタンバトンでやられたようだ。"
                    }
                },
                { "rubble.encounter.7", new[]
                    {
                        "（左脸的痕迹传来一阵细微的、冰凉的悸动，\n仿佛在共鸣某种遥远的痛苦。）",
                        "(A faint, icy throb runs through the mark on the left side of my face,\nas though it were resonating with some distant pain.)",
                        "（左頬の痕に、かすかな冷たい疼きが走る。\nまるで、遠い痛みと共鳴しているかのようだ。）"
                    }
                },
                { "rubble.encounter.8", new[]
                    {
                        "在这儿干什么呢你？没看到我正在进货吗？",
                        "What're you doing here? Can't you see I'm stocking up?",
                        "こんな所で何してやがる？　俺が仕入れ中なのが見えねえのか？"
                    }
                },
                { "rubble.encounter.9", new[] { "没……没什么。", "N-Nothing...", "な……何でもない。" } },
                { "rubble.encounter.10", new[]
                    {
                        "别以为你脸上那片膏药能瞒过我，我儿子跟你大学一个专业的，\n早跟我说过你。你这半人半魔的怪物！",
                        "Don't think that thing on your face can fool me. My son's in the same major as you,\nand he's told me all about you. You freak!",
                        "顔の膏薬で俺の目をごまかせると思うな。息子は大学でお前と同じ専攻で、\nとっくにお前のことを聞いてるんだ。この半人半魔の化け物め！"
                    }
                },
                { "rubble.encounter.11", new[]
                    {
                        "你已经杀了那只恶魔，还不够吗？",
                        "You've already killed that demon. Isn't that enough?",
                        "もうあの悪魔を殺しただろう。それでも足りないのか？"
                    }
                },
                { "rubble.encounter.12", new[]
                    {
                        "还不够，凑儿子的学费还不够！",
                        "Not enough! It's not enough to cover my son's tuition!",
                        "足りねえ！　息子の学費にはまだ足りねえんだ！"
                    }
                },
                { "rubble.encounter.13", new[]
                    {
                        "还不够让他读完下一年，好不再做我这恶心的营生！",
                        "Not enough to get him through another year, so he'll never have to do this filthy work I do!",
                        "あいつがもう一年通いきって、俺みたいな汚え商売をしなくて済むには、まだ足りねえ！"
                    }
                },
                { "rubble.encounter.14", new[]
                    {
                        "但依我看，抓到了你这半魔……抵得上再抓五十只……\n不，一百只刚才那样的！",
                        "But the way I see it, catching a half-devil like you... would be worth another fifty...\nNo, a hundred of those things!",
                        "だが俺の見立てじゃ、お前みたいな半魔を捕まえりゃ……さっきのをあと五十匹……\nいや、百匹捕まえるのと同じ値打ちだ！"
                    }
                },
                { "rubble.ui.progress", new[]
                    {
                        "逃离丧家犬老大：已开启 {0} / 4 个开关",
                        "Escape the Stray Dogs Boss: {0} / 4 Switches Activated",
                        "「負け犬」のボスから逃げろ：スイッチ {0} / 4 起動"
                    }
                },
                { "rubble.ui.escape", new[]
                    {
                        "尽快进入铁门离开", "Get through the iron gate—quick!", "急いで鉄扉から脱出しろ！"
                    }
                },
                { "rubble.ending.brokenDreams.title", new[]
                    {
                        "结局：梦想折翼", "Ending: Dreams Cut Short", "結末：夢、潰える"
                    }
                },
                { "rubble.ending.brokenDreams.1", new[]
                    {
                        "你倒在了这个集杀手与父亲于一体的男人的刀下，再也不会醒来。",
                        "You fell to the blade of this man, both killer and father, never to wake again.",
                        "殺し屋であり、父親でもあるこの男の刃に倒れ、あなたは二度と目を覚ますことはない。"
                    }
                },
                { "rubble.ending.brokenDreams.2", new[]
                    {
                        "你深知，法律不可能对他予以制裁，他甚至可能在猎魔机构里得到些认可。",
                        "You know full well that the law will never punish him. He may even earn some recognition from the demon-hunting agency.",
                        "法が彼を裁くことはないと、あなたはよく分かっている。彼は悪魔狩り機関から多少の評価すら得るかもしれない。"
                    }
                },
                { "rubble.ending.brokenDreams.3", new[]
                    {
                        "麻袋套上你的头前，你还在挂念着你最亲近的人，毕竟日后她只能靠自己了。",
                        "Before the sack is pulled over your head, your thoughts are still with the person closest to you. After all, from now on, she'll have to fend for herself.",
                        "麻袋を頭にかぶせられる前も、あなたは最も親しい人のことを案じている。これからは、彼女一人で生きていくしかないのだから。"
                    }
                },
                { "rubble.ending.brokenDreams.4", new[]
                    {
                        "但至少......她不用再同一个半人半魔的家伙过日子了。",
                        "But at least... she won't have to live with someone who's half human and half demon anymore.",
                        "だが、少なくとも……彼女はもう、半人半魔のやつと暮らさずに済む。"
                    }
                },
                { "rubble.ending.brokenDreams.5", new[]
                    {
                        "眼前什么都没有。", "There is nothing before you.", "目の前には、何もない。"
                    }
                },
                { "rubble.lore.tuition.1", new[]
                    {
                        "其实我们大学的学费也不贵啊……",
                        "Our university's tuition isn't actually that expensive...",
                        "そもそも、僕たちの大学の学費はそこまで高くない……"
                    }
                },
                { "rubble.lore.tuition.2", new[]
                    {
                        "毕竟现代社会人人都想当高收入的恶魔猎人，\n已经不怎么重视文化教育了。",
                        "After all, these days everyone wants to become a high-paid demon hunter.\nNo one places much value on education and culture anymore.",
                        "今じゃ誰もが高収入の悪魔狩りになりたがって、\n学問や教養はもうほとんど重んじられていないからな。"
                    }
                },
                { "rubble.lore.tuition.3", new[]
                    {
                        "那你又有什么理由杀我？",
                        "Then what reason do you have to kill me?",
                        "なら、僕を殺す理由がどこにある？"
                    }
                },
                { "rubble.lore.tuition.4", new[] { "……", "...", "……" } },
                { "rubble.lore.debt.1", new[]
                    {
                        "这账单上的东西不太一般啊，不是水电费，而是……",
                        "The charges on this bill are a little unusual. They're not for utilities, but...",
                        "この請求書、内容が少し変だ。水道光熱費じゃなくて……"
                    }
                },
                { "rubble.lore.debt.2", new[]
                    {
                        "购买大牌恶魔猎人周边的消费。",
                        "Merchandise from big-name demon hunters.",
                        "有名な悪魔狩りのグッズを買った代金だ。"
                    }
                },
                { "rubble.lore.debt.3", new[]
                    {
                        "看来你给儿子凑的钱好像不只是学费。",
                        "Looks like the money you're scraping together for your son isn't just for tuition.",
                        "どうやら、息子のために工面している金は、学費だけじゃないようだな。"
                    }
                },
                { "rubble.lore.debt.4", new[] { "……", "...", "……" } }
            };
        }
    }
}
