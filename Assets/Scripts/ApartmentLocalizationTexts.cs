using System.Collections.Generic;

namespace RPG
{
    public static class ApartmentLocalizationTexts
    {
        public static Dictionary<string, string[]> Create()
        {
            return new Dictionary<string, string[]>
            {
                { "apartment.speaker.linChunbao", new[] { "林春宝", "Lin Chunbao", "林春宝" } },
                { "apartment.speaker.municipalOfficer", new[] { "行政人员", "Municipal Officer", "行政職員" } },
                { "apartment.entrance.intro.1", new[]
                    {
                        "（侵蚀痕迹的灼热尚未消退。）\n不知是从何时起养成了这种自言自语的行为……\n可能从小便是这样。",
                        "(The burning in the corruption mark still hasn't faded.)\nI don't know when I started talking to myself like this...\nMaybe I've always done it, ever since I was little.",
                        "（侵食痕の灼熱は、まだ消えていない。）\nいつからこんなふうに独り言を言うようになったのだろう……\n幼い頃からだったのかもしれない。"
                    }
                },
                { "apartment.entrance.intro.2", new[]
                    {
                        "啊，公寓楼就在前面。\n虽说老旧，但也是个家。",
                        "Ah, the apartment building is just ahead.\nIt's old, but it's still home.",
                        "ああ、アパートはもう目の前だ。\n古びてはいるが、それでも我が家だ。"
                    }
                },
                { "apartment.entrance.door.1", new[]
                    {
                        "我妹妹林春宝这个时候大概已经放学在家里等着我了吧。\n……真不知道这句话是对谁说的……\n好像我们这个家真会来客人一样。",
                        "My younger sister, Lin Chunbao, should be home from school and waiting for me by now.\n...I don't even know who I'm saying this to...\nAs if anyone would ever come to visit us.",
                        "妹の林春宝は、もう学校から帰って家で僕を待っている頃だろう。\n……いったい誰に向かって言っているんだ……\nまるで、この家に本当に客が来るみたいじゃないか。"
                    }
                },
                { "apartment.entrance.door.2", new[]
                    {
                        "……没有人愿意跟一个半人半恶魔的家伙说话，除了她。\n因为我是她哥哥。",
                        "...No one wants to talk to someone who's half human and half demon. No one except her.\nBecause I'm her brother.",
                        "……半人半魔のやつと話したがる人間なんて、彼女以外にはいない。\n僕が、彼女の兄だからだ。"
                    }
                },
                { "apartment.entrance.wrongWay", new[]
                    {
                        "这边不是通往郊区的路，我正后方的路才是。",
                        "This isn't the way to the suburbs. The road directly behind me is.",
                        "こっちは郊外へ向かう道じゃない。真後ろの道だ。"
                    }
                },
                { "apartment.livingRoom.intro.1", new[]
                    {
                        "厨房里飘出煎蛋的香味。\n春宝大概在做晚餐吧。",
                        "The smell of fried eggs is drifting from the kitchen.\nChunbao must be making dinner.",
                        "台所から目玉焼きの香りが漂ってくる。\n春宝が夕食を作っているのだろう。"
                    }
                },
                { "apartment.livingRoom.intro.2", new[]
                    {
                        "自从六岁时那次使我脸上留下痕迹的意外以来\n春宝便和我相依为命，确实辛苦她了。",
                        "Ever since the accident when I was six left this mark on my face,\nChunbao and I have had no one but each other. She's been through a lot because of it.",
                        "六歳のときの事故で顔にこの痕が残って以来、\n春宝と僕はずっと二人で支え合ってきた。彼女には本当に苦労をかけている。"
                    }
                },
                { "apartment.livingRoom.intro.3", new[]
                    {
                        "我到底在和谁说话啊……\n不管了，先去厨房。",
                        "Who am I even talking to...?\nNever mind. I should head to the kitchen.",
                        "僕はいったい誰に話しかけているんだ……\nまあいい。先に台所へ行こう。"
                    }
                }
            };
        }
    }
}
