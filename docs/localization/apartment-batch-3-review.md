# 公寓多语言化第三批审核稿：厨房与洗手间

状态：第三批全部译文已获用户批准并完成代码接入。

既定人物语气：主角日文自称「僕」；林春宝日文自称「俺」。

范围：厨房和洗手间调查、食品柜密码锁、厕所分支、通风口三种状态、数字纸条、药柜整理界面及本批物品名。离家对白留在第五批审核，本批只为其后续逻辑准备肉干稳定 ID。

## 用户审定规则与修改记录

| 项目 | 早期草稿 | 最终审定内容 | 后续约束 |
|---|---|---|---|
| 通风口三种状态的共用观察台词 | 从这里看去，旁边的墙上贴着一张纸像是写着什么数字，也许和密码锁有关。 | 从这里看去，下水道里贴着一张纸像是写着什么数字，也许和密码锁有关。 | 默认、持菜刀和持粘竿菜刀三种状态均使用修改后的同一条中英日文本 |

## 第一部分：厨房调查与食品柜

| 键 | 说话人/用途 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.kitchen.stoveKnife` | 主角 | 灶台上有一把菜刀。<br>也许能用来撬开生锈的铰链。 | There's a kitchen knife on the stove.<br>Maybe I can use it to pry open the rusty hinges. | コンロの上に包丁がある。<br>錆びた蝶番をこじ開けるのに使えるかもしれない。 |
| `apartment.kitchen.foodCabinet.intro.1` | 林春宝 | 话说这房东也真是个狠人。<br>欠他三个月的租金，不停水，不停电…… | You know, our landlord really is ruthless.<br>We owe him three months' rent, and he doesn't cut off the water or electricity... | それにしても、あの大家も本当に容赦ないよな。<br>家賃を三か月滞納しても、水も電気も止めずに…… |
| `apartment.kitchen.foodCabinet.intro.2` | 林春宝 | 居然把食品柜上了把锁！ | He puts a lock on the food cabinet instead! | 食品棚に鍵をかけやがった！ |
| `apartment.kitchen.foodCabinet.intro.3` | 林春宝 | 还是密码锁！ | A combination lock, no less! | しかもダイヤル錠だぞ！ |
| `apartment.kitchen.foodCabinet.intro.4` | 主角 | 这种情况不报警吗？ | Shouldn't we call the police about this? | こういうときは警察に通報しないのか？ |
| `apartment.kitchen.foodCabinet.intro.5` | 林春宝 | 万一警察见到你直接呼叫恶魔猎人了呢？ | What if the police see you and call the demon hunters straight away? | 警察が兄ちゃんを見て、いきなり悪魔狩りを呼んだらどうするんだ？ |
| `apartment.kitchen.foodCabinet.open.1` | 主角 | 终于是把这食品柜打开了…… | I finally got the food cabinet open... | やっと食品棚を開けられた…… |
| `apartment.kitchen.foodCabinet.open.2` | 主角 | 这袋肉干似乎是拿来吸引恶魔的不二之选，<br>毕竟它们大部分都吃肉吧…… | This bag of dried meat seems like the perfect thing for luring demons.<br>Most of them eat meat, after all... | この干し肉なら、悪魔をおびき寄せるのにうってつけだろう。<br>悪魔の大半は肉を食べるはずだから…… |
| `apartment.kitchen.foodCabinet.repeat` | 主角 | 我没必要带更多了。 | I don't need to take any more. | これ以上持っていく必要はない。 |
| `apartment.ui.foodCabinet.title` | 密码锁标题 | 食品柜密码锁 | Food Cabinet Combination Lock | 食品棚のダイヤル錠 |
| `apartment.ui.foodCabinet.hint` | 密码锁操作提示 | A / D 选择密码位　　W / S 调整数值<br>Q 退出 | A / D: Select Digit　　W / S: Change Value<br>Q: Exit | A / D：桁を選択　　W / S：数字を変更<br>Q：終了 |

## 第二部分：药柜与整理谜题

| 键 | 说话人/用途 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.bathroom.medicineCabinet.intro.1` | 主角 | 药柜被锁住了…… | The medicine cabinet is locked... | 薬棚に鍵がかかっている…… |
| `apartment.bathroom.medicineCabinet.intro.2` | 主角 | 看来还是房东干的，我每天都要从里面拿膏药出来贴在脸上，不可能把它锁住的。 | Looks like the landlord did this too. I take a plaster from here every day to cover my face, so I would never lock it. | これも大家の仕業らしい。僕は毎日ここから膏薬を出して顔に貼るんだから、自分で鍵をかけるはずがない。 |
| `apartment.bathroom.medicineCabinet.intro.3` | 主角 | 话说回来……钥匙去哪里了？ | Come to think of it... where did the key go? | そういえば……鍵はどこへ行ったんだ？ |
| `apartment.bathroom.medicineCabinet.repeat` | 主角 | 现在的当务之急是找到药柜的钥匙。 | The first thing I need to do is find the medicine cabinet key. | 今はまず、薬棚の鍵を見つけないと。 |
| `apartment.ui.medicineCabinet.title` | 整理谜题标题 | 药柜 | Medicine Cabinet | 薬棚 |
| `apartment.ui.medicineCabinet.hint` | 整理谜题操作提示 | E 选中药品　　WASD 与相邻药品交换　　归位的药品无法移动 | E: Select Medicine　　WASD: Swap with an Adjacent Medicine　　Correctly Placed Medicines Cannot Be Moved | E：薬を選択　　WASD：隣の薬と交換　　正しい位置の薬は移動不可 |

## 第三部分：厕所分支

| 键 | 说话人/用途 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.bathroom.toilet.intro.1` | 主角 | 也许我该往这里面看看……说不准会发现什么。 | Maybe I should take a look inside... I might find something. | 中を見てみたほうがいいかもしれない……何か見つかるかも。 |
| `apartment.bathroom.toilet.intro.2` | 主角 | 该怎么做呢？ | How should I go about it? | どうすればいい？ |
| `apartment.bathroom.toilet.intro.3` | 主角 | 例如说……冲马桶？ | Maybe... flush the toilet? | 例えば……水を流してみるとか？ |
| `apartment.bathroom.toilet.second.1` | 林春宝 | 哥，你在冲马桶吗？ | Bro, are you flushing the toilet? | 兄ちゃん、トイレの水を流してるのか？ |
| `apartment.bathroom.toilet.second.2` | 林春宝 | 省着点用水，我可不想再打几次黑拳挣水费！ | Go easy on the water. I don't want to fight a few more underground matches just to pay the water bill! | 水は節約してくれよ。水道代のために、また何試合も闇試合をするのはごめんだからな！ |
| `apartment.bathroom.toilet.third` | 林春宝 | 你怕不是又把浴盐球掉进去了？ | Don't tell me you dropped another bath bomb in there? | またバスボムを落としたんじゃないだろうな？ |
| `apartment.bathroom.toilet.fourth.1` | 主角 | ……这房东果真是个狠人。 | ...That landlord really is ruthless. | ……あの大家、本当に容赦がないな。 |
| `apartment.bathroom.toilet.fourth.2` | 主角 | 而且也是个坏人。拿准了我因为脸上这道痕迹而不敢报警的把柄。 | And a cruel man, too. He knows I won't dare call the police because of this mark on my face, and he's using that against me. | それに悪人でもある。僕が顔のこの痕のせいで警察に通報できないと見抜いて、そこにつけ込んでいる。 |

## 第四部分：通风口、纸条与合成

| 键 | 说话人/用途 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.bathroom.vent.observe` | 主角；三种状态共用 | 从这里看去，下水道里贴着一张纸像是写着什么数字，也许和密码锁有关。 | From here, I can see a piece of paper stuck inside the sewer. It looks like there's some kind of number written on it—maybe it has something to do with the combination lock. | ここから見ると、下水道の中に紙が貼りついていて、何か数字が書かれているようだ。ダイヤル錠と関係があるのかもしれない。 |
| `apartment.bathroom.vent.default` | 主角；未持合适物品 | 但我够不到它，最好找个长一点的东西把它勾过来。 | But I can't reach it. I should find something long enough to hook it over here. | でも手が届かない。何か長い物で引っかけて、こっちへ寄せたほうがよさそうだ。 |
| `apartment.bathroom.vent.knife.1` | 主角；持菜刀 | 但我够不到它，这把刀够长，可是很可能会把那张纸弄掉。 | But I can't reach it. This knife is long enough, but I'd probably just knock the paper down. | でも手が届かない。この包丁なら長さは足りるが、紙を落としてしまいそうだ。 |
| `apartment.bathroom.vent.knife.2` | 主角；持菜刀 | 得找点什么黏的东西涂在刀上，把纸张像粘知了一样粘下来。 | I need something sticky to put on the knife, so I can catch the paper the way you'd catch a cicada with a sticky pole. | 包丁に何か粘る物をつけて、粘着竿でセミを捕るみたいに紙をくっつけて取らないと。 |
| `apartment.bathroom.vent.stickyKnife` | 主角；持粘竿菜刀 | 现在有了这粘竿菜刀，我就能把它粘过来了。 | Now that I have this sticky-pole knife, I can stick the paper to it and pull it over here. | この粘着竿包丁があれば、紙をくっつけてこっちへ引き寄せられる。 |
| `apartment.bathroom.passwordNote.1` | 主角 | 虽然一大半都被胶糊住了，但我还是看得出来……是一张上面写着数字8的纸条。 | Most of it is smeared with glue, but I can still make it out... It's a note with the number 8 written on it. | 半分以上が糊でべたべたになっているが、それでも分かる……数字の「8」が書かれた紙だ。 |
| `apartment.bathroom.passwordNote.2` | 主角 | 或许与密码锁有关。 | It might have something to do with the combination lock. | ダイヤル錠と関係があるのかもしれない。 |
| `apartment.crafting.stickyKnife` | 主角 | 兴许我能把这平时拿来遮挡的膏药贴在钝刀上，做成一个特别奇怪的粘竿。 | Maybe I can stick this plaster—the one I usually use to cover my face—onto the blunt knife and make a very strange kind of sticky pole. | いつも顔を隠すのに使っているこの膏薬を、なまくらな包丁に貼りつければ、かなり妙な粘着竿を作れるかもしれない。 |

## 第五部分：物品显示名与稳定 ID

| 稳定 ID | 简体中文 | English | 日本語 | 译注 |
|---|---|---|---|---|
| `apartment.knife` | 菜刀 | Kitchen Knife | 包丁 | 普通厨房菜刀；剧情称刀刃较钝 |
| `apartment.driedMeat` | 肉干 | Dried Meat | 干し肉 | 用于吸引恶魔 |
| `apartment.medicineCabinetKey` | 药柜钥匙 | Medicine Cabinet Key | 薬棚の鍵 | 厕所分支取得 |
| `apartment.plaster` | 膏药 | Plaster | 膏薬 | 主角平时贴在脸上；沿用第二批英文 `plaster` |
| `apartment.stickyKnife` | 粘竿菜刀 | Sticky-Pole Knife | 粘着竿包丁 | 临时组合物品，刻意保留古怪感 |

## 待审核事项

1. 请确认林春宝关于房租、报警和水费的英文、日文口语强度是否合适。
2. 中文“像粘知了一样”按“像用粘竿粘知了（捕蝉）一样”理解并翻译；如果原句另有所指，请指出。
3. 请重点确认组合物品名 `Sticky-Pole Knife` / `粘着竿包丁`。这两个名称刻意直译并保留滑稽、临时拼装的感觉。
4. 用户已明确批准本文全部译文；代码接入必须继续以本文最终审定内容为准。
