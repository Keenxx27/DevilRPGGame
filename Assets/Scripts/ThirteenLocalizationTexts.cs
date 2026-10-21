using System.Collections.Generic;

namespace RPG
{
    public static class ThirteenLocalizationTexts
    {
        public static Dictionary<string, string[]> Create()
        {
            return new Dictionary<string, string[]>
            {
                { "thirteen.speaker.unknown", new[] { "？？？", "???", "？？？" } },
                { "thirteen.speaker.thirteen", new[] { "十三", "Thirteen", "十三" } },
                { "thirteen.encounter.discovery.1", new[]
                    {
                        "那里有什么东西在动。", "Something is moving over there.", "あそこで何かが動いている。"
                    }
                },
                { "thirteen.encounter.discovery.2", new[]
                    {
                        "第一眼，真的像一只小黑猫。\n它蜷缩在最大的荧光蘑菇下，正好奇地打量着我。",
                        "At first glance, it really does look like a little black cat.\nIt's curled up beneath the largest glowing mushroom, watching me curiously.",
                        "一見すると、本当に小さな黒猫のようだ。\n一番大きな光るキノコの下で丸くなり、興味深そうに僕を見つめている。"
                    }
                },
                { "thirteen.encounter.discovery.3", new[]
                    {
                        "但它不是猫。耳朵的位置是两片薄膜般的结构，\n尾巴尖还燃着一小簇冰蓝色的火焰。",
                        "But it isn't a cat. In place of ears are two thin, membrane-like structures,\nand a small ice-blue flame burns at the tip of its tail.",
                        "だが、猫ではない。耳の位置には薄い膜のような器官が二つあり、\n尻尾の先には小さな氷青色の炎が灯っている。"
                    }
                },
                { "thirteen.encounter.gift.1", new[]
                    {
                        "它没有看肉干，反而慢慢靠近了我。",
                        "It doesn't even look at the dried meat. Instead, it slowly approaches me.",
                        "干し肉には目もくれず、代わりにゆっくりと僕へ近づいてくる。"
                    }
                },
                { "thirteen.encounter.gift.2", new[]
                    {
                        "它抬起前爪，小心翼翼地碰了碰我的手指。\n冰凉，还有一点粗糙。",
                        "It raises a forepaw and carefully touches my finger.\nCold, and a little rough.",
                        "前足を上げ、恐る恐る僕の指に触れる。\n冷たくて、少しざらついている。"
                    }
                },
                { "thirteen.encounter.gift.3", new[]
                    {
                        "它又跑回蘑菇丛，叼来一块内部仿佛流淌着星光的晶石。\n这是......你的礼物吗？",
                        "It runs back into the mushrooms and returns with a crystal that seems to have starlight flowing inside.\nIs this... a gift for me?",
                        "キノコの群れへ駆け戻り、中に星明かりが流れているような結晶をくわえてくる。\nこれは……僕への贈り物なのか？"
                    }
                },
                { "thirteen.encounter.gift.4", new[]
                    {
                        "我……我没有恶意。", "I... I mean you no harm.", "僕は……君を傷つけるつもりはない。"
                    }
                },
                { "thirteen.encounter.gift.5", new[]
                    {
                        "（它发出一个由三个高低不同音调组成的清脆音节。）",
                        "(It lets out a clear, chiming syllable composed of three notes at different pitches.)",
                        "（高さの異なる三つの音からなる、澄んだ音節を発する。）"
                    }
                },
                { "thirteen.encounter.gift.6", new[]
                    {
                        "十三……\n这个名字，怎么会忽然出现在我脑子里？",
                        "Thirteen...\nWhy did that name suddenly appear in my mind?",
                        "十三……\nなぜ、この名前が突然僕の頭に浮かんだんだ？"
                    }
                },
                { "thirteen.encounter.gift.7", new[]
                    {
                        "（它歪了歪头，耳膜轻轻转动。）",
                        "(It tilts its head, its ear membranes gently swiveling.)",
                        "（首をかしげ、耳の膜をそっと動かす。）"
                    }
                },
                { "thirteen.encounter.gift.8", new[]
                    {
                        "我伸出手，掌心向上。\n十三极轻地舔了一下我的指尖。",
                        "I hold out my hand, palm up.\nThirteen gives the tip of my finger the lightest lick.",
                        "僕は掌を上にして手を差し出す。\n十三は僕の指先をほんの少しだけ舐めた。"
                    }
                },
                { "thirteen.ui.offerDriedMeat", new[]
                    {
                        "选中肉干后，在十三面前按 Q 放下肉干",
                        "With Dried Meat selected, press Q in front of Thirteen to place it down",
                        "干し肉を選択し、十三の前でQキーを押して置く"
                    }
                }
            };
        }
    }
}
