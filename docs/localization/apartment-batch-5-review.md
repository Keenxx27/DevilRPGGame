# 公寓多语言化第五批审核稿：离家与行政人员冲突

状态：第五批全部译文已获用户批准，并已完成代码接入。

既定人物语气：主角日文自称「僕」；林春宝日文自称「俺」。林春宝口语中的“小归墟”继续使用 `small hole of Abyss` / `小さな深淵の穴`。

范围：取得肉干后的离家对白；携十三回到公寓后，林春宝、主角与两名行政人员的冲突对白；“无归属者”结局标题、结局字幕与单独的感谢页面。本批不改变人物移动、房门状态、场景跳转、结局触发或按钮行为。

## 第一部分：离家对白

| 键 | 说话人 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.departure.ready` | 林春宝 | 你准备好了？ | Are you ready? | 準備はできたか？ |
| `apartment.departure.askLocation` | 主角 | 我什么都拿好了，告诉我那个新开的小归墟在哪里吧。 | I've got everything. Tell me where that newly opened small hole of Abyss is. | 必要なものは全部持った。新しく開いた小さな深淵の穴がどこにあるのか、教えてくれ。 |
| `apartment.departure.directions` | 林春宝 | 就在郊区。出了公寓楼你向右转，<br>从公交车站的方向看来就是向左转。 | It's in the suburbs. Turn right when you leave the apartment building.<br>Looking from the bus stop, that would be a left. | 郊外だ。アパートを出たら右に曲がれ。<br>バス停のほうから見れば左だ。 |
| `apartment.departure.understood` | 主角 | 明白了。 | Understood. | 分かった。 |
| `apartment.departure.warningLead` | 林春宝 | 还有一件事…… | One more thing... | もう一つ…… |
| `apartment.departure.prompt` | 主角 | 嗯？ | Hmm? | うん？ |
| `apartment.departure.gangWarning` | 林春宝 | 那里绝对有丧家犬的人，就是我们市里<br>那个臭名昭著的猎魔帮派。 | There'll definitely be Stray Dogs there—you know, that notorious<br>demon-hunting gang in our city. | あそこには絶対「負け犬」の連中がいる。うちの街で悪名高い、<br>あの悪魔狩りギャングだ。 |
| `apartment.departure.contactPromise` | 主角 | 明白了，真出了什么事我会联系你的。 | Understood. If anything happens, I'll contact you. | 分かった。何かあったら連絡する。 |

## 第二部分：行政人员冲突

说话人显示名继续使用既有键：`apartment.speaker.linChunbao` 与 `apartment.speaker.municipalOfficer`。其中“行政人员”已审定为 `Municipal Officer` / `行政職員`。

### 初次对峙

| 键 | 说话人 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.confrontation.initial.linChallenge` | 林春宝 | ……凭什么？就凭一张没盖章的破纸？<br>我哥脸上那点东西怎么了？碍着谁了？！ | ...On what grounds? Because of one lousy piece of paper without so much as a stamp?<br>What's wrong with that mark on my brother's face? Who's it hurting?! | ……何の権利があって？　判も押してないこんな紙切れ一枚で？<br>兄貴の顔のあれが何だってんだ？　誰に迷惑かけたよ！？ |
| `apartment.confrontation.initial.officialNotice` | 行政人员 | 林春宝同学，请冷静。这是学校的决定，也是为了全体师生的安全考虑。<br>您兄长面部的异常情况，根据《非人知性体接触者管理暂行条例》第七款，需要接受指定机构的隔离检查。<br>在检查结果出来前，暂停其一切校内活动。 | Miss Lin Chunbao, please remain calm. This is the school's decision, made with the safety of all students and faculty in mind.<br>Under Article 7 of the Interim Regulations on the Management of Persons in Contact with Non-Human Sapient Entities, the abnormality on your brother's face requires an isolation examination at a designated facility.<br>All of his on-campus activities are suspended pending the results. | 林春宝さん、落ち着いてください。これは学校の決定であり、全学生および教職員の安全を考慮したものです。<br>お兄様の顔面に見られる異常については、『非人間知性体接触者管理暫定条例』第七条に基づき、指定機関での隔離検査を受けていただく必要があります。<br>検査結果が出るまで、学内での一切の活動を停止します。 |
| `apartment.confrontation.initial.linAccusation` | 林春宝 | 不是房东就是哪个混蛋搞的鬼！ | It was either the landlord or some other bastard who set this up! | 大家か、どこかのクソ野郎が仕組んだに決まってる！ |

### 向主角宣读通知

| 键 | 说话人 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.confrontation.notice.report` | 行政人员 | 屋主回来了。通知您已经看到。请于四十八小时内，携带这份通知，<br>前往市立第三检疫中心报到。逾期未到，将由安全部门强制执行。 | The resident is back. You have seen the notice. Please take it with you and report<br>to Municipal Quarantine Center No. 3 within forty-eight hours. Failure to do so by the deadline will result in compulsory action by the Security Department. | 住人が戻りましたね。通知はご覧いただいたとおりです。この通知を持参のうえ、四十八時間以内に<br>市立第三検疫センターへ出頭してください。期限までに出頭しない場合は、安全部門が強制執行します。 |
| `apartment.confrontation.notice.silence` | 主角 | …… | ... | …… |
| `apartment.confrontation.notice.acknowledge` | 主角 | 我知道了。 | I understand. | 分かりました。 |

### 行政人员威胁

| 键 | 说话人 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.confrontation.threat.lead` | 行政人员 | 你知道吗…… | You know... | 知っていますか…… |
| `apartment.confrontation.threat.cleanup` | 行政人员 | 如果我们俩是清理部门的人，会考虑当场把你和你妹妹的人头<br>抛到大街上。 | If the two of us were from the Cleanup Department, we'd consider cutting off your and your sister's heads right here<br>and tossing them into the street. | 私たち二人が処理部門の者だったなら、あなたと妹さんの首をその場ではねて、<br>大通りに放り出すことも考えたでしょう。 |
| `apartment.confrontation.threat.leave` | 行政人员 | 走。 | Let's go. | 行きましょう。 |

### 行政人员离开后

| 键 | 说话人 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.confrontation.final.linPlea` | 林春宝 | 哥！你不能去！那种地方进去谁知道会怎样！<br>我们走，现在就收拾东西…… | Bro! You can't go! Who knows what they'll do to you in a place like that?<br>Let's leave. We'll pack our things right now... | 兄貴！　行っちゃ駄目だ！　あんな所に入ったら何をされるか分からない！<br>ここを出よう。今すぐ荷物をまとめて…… |
| `apartment.confrontation.final.evictionOrder` | 主角 | 这停学通知措辞严谨，引用条例，看似合法合规。<br>但它本质上是一纸驱逐令。 | This suspension notice is carefully worded and cites the regulations. On the surface, it appears perfectly lawful.<br>But in essence, it's an eviction order. | この停学通知は用語も厳密で、条例まで引用している。一見すると、法に則った正当なものだ。<br>だが本質的には、ただの追放命令だ。 |
| `apartment.confrontation.final.noPlace` | 主角 | 看来，这里真的没有我的位置了。 | It seems there really is no place for me here. | どうやら、ここには本当に僕の居場所がないらしい。 |
| `apartment.confrontation.final.leaveTogether` | 主角 | 所以，我们得去别处看看。 | So we'll have to look somewhere else. | だから、僕たちは別の場所を探さなければならない。 |

## 第三部分：“无归属者”结局画面

| 键 | 用途 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.ending.noBelonging.title` | 结局标题 | 结局：无归属者 | Ending: Nowhere to Belong | 結末：居場所なき者 |
| `apartment.ending.noBelonging.1` | 标题下方字幕 | 窗外的城市在夜雨中灯火阑珊，远处，郊区方向的夜空似乎比别处更暗沉一些。 | Beyond the window, the city's lights glow faintly through the rainy night. In the distance, the sky over the suburbs seems darker than anywhere else. | 窓の外では、夜の雨に濡れた街の灯がかすかに瞬いている。遠く、郊外の方角の夜空は、ほかの場所よりも暗く沈んで見える。 |
| `apartment.ending.noBelonging.2` | 标题下方字幕 | 那里有归墟，也有你的归属。 | There lies the Abyss—and a place where you belong. | そこには深淵があり、あなたの居場所もある。 |
| `apartment.ending.noBelonging.3` | 标题下方字幕 | 那里有被掩盖的真相，和正在发生的、无人听见的对话。 | There are truths concealed there, and conversations taking place that no one else can hear. | そこには覆い隠された真実と、誰にも聞かれることなく交わされている対話がある。 |
| `apartment.ending.noBelonging.4` | 标题下方字幕 | 你将通知慢慢折起，塞进口袋，和那块温热的晶石放在一起。 | You slowly fold the notice and slip it into your pocket, placing it beside the warm crystal. | あなたは通知をゆっくりと折りたたみ、あの温かな晶石と一緒にポケットへしまう。 |
| `apartment.ending.noBelonging.5` | 标题下方字幕 | 这是真正属于你的结局。 | This is the ending that truly belongs to you. | これこそが、本当にあなた自身の結末だ。 |

字幕显示完毕后切换至单独页面：

| 键 | 用途 | 简体中文 | English | 日本語 |
|---|---|---|---|---|
| `apartment.ending.thanks` | 单独感谢页面 | 感谢游玩 | Thank You for Playing | プレイしていただき、ありがとうございました |

## 审核结论

1. “丧家犬”暂译为 `Stray Dogs` / `負け犬`。如果该帮派已有正式外文名，应统一使用正式名称。
2. 《非人知性体接触者管理暂行条例》采用正式法规风格：`Interim Regulations on the Management of Persons in Contact with Non-Human Sapient Entities` / `非人間知性体接触者管理暫定条例`。
3. 原句“屋主回来了”按当前剧情语境理解为行政人员注意到公寓住人归来，因此译作 `The resident is back.` / `住人が戻りましたね。`；若“屋主”特指房屋所有人，需要改为 `The homeowner is back.` / `家主が戻りましたね。`。
4. “无归属者”标题不采用生硬的直译 `The Unbelonging`，推荐 `Nowhere to Belong`；日文采用具有标题感的 `居場所なき者`。
5. 结局画面分为两页：第一页显示“无归属者”标题及五行字幕；字幕结束后切换至第二页，仅显示“感谢游玩”。
6. 用户已明确批准本文全部译文；代码接入以本文最终审定内容为准。
