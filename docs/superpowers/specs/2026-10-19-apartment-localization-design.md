# 公寓章节多语言化设计

## 目标

将玩家从进入公寓到离开公寓期间的完整流程接入现有简体中文、英文、日文三语系统，确保对白、说话人、物品显示名、谜题界面、操作提示、选择项及结局画面均随已保存语言切换，同时不改变剧情逻辑、谜题状态或场景流程。

## 范围

覆盖以下六个场景及其完整剧情闭环：

- `ApartmentEntrance`
- `ApartmentLivingRoom`
- `ApartmentKitchen`
- `ApartmentBathroom`
- `ApartmentLinChunbaoBedroom`
- `ApartmentProtagonistBedroom`

范围包括返家对白、房间调查、林春宝主剧情、所有公寓谜题和小游戏、离家对白、行政人员冲突，以及公寓相关结局画面。郊区、丧家犬和归墟章节不属于本轮范围。

## 翻译规则

- 中文原文是英文和日文译文的唯一语义基准。
- 翻译不得改变剧情含义、人物关系、人物语气、选项结果或结局条件。
- 可以为了界面适配调整英文和日文换行，但不得以删减文本代替排版处理。
- 玩家输入的姓名保持原样，不翻译、不转写、不改变大小写。
- `"主角"` 继续作为内部识别标记，由 `PlayerChosenName` 在显示时替换为玩家姓名。
- 林春宝显示为：简体中文 `林春宝`、英文 `Lin Chunbao`、日文 `林春宝`。
- 行政人员显示为：简体中文 `行政人员`、英文 `Municipal Officer`、日文 `行政職員`。
- 日文对白中，性格讲礼貌的主角统一以「僕」自称；性格大大咧咧的林春宝统一以「俺」自称。
- 林春宝口语中的“小归墟”固定译为英文 `small hole of Abyss`、日文 `小さな深淵の穴`；正式资料或机构术语不受这条口语规范限制。
- 已经由用户批准的人名、术语和表达需要在后续批次复用，避免同一概念出现不同译法。

## 翻译审核关卡

每一批必须先完成译文审核，未经用户明确批准不得接入代码：

1. 按实际剧情顺序提取中文原文，并标注文本键、说话人、使用位置和触发条件。
2. 提交简体中文、英文、日文对照稿；对双关、专有名词、语气选择或多种可行译法附上简短译注。
3. 等待用户逐条修改、部分批准或整批批准。
4. 只将已经批准的译文接入项目；未批准部分保持待审状态，不擅自补齐。
5. 完成代码与场景验证，报告结果并取得确认后，才进入下一批翻译。

每批流程固定为：

`提取原文 → 提交三语对照稿 → 用户批准 → 代码接入 → 自动与人工验证 → 用户确认`

## 架构

继续使用现有 `GameLocalization` 查询接口和 `PlayerPrefs` 语言保存机制，不引入 Unity Localization Package。

公寓文本采用统一命名空间：

- `apartment.speaker.*`
- `apartment.entrance.*`
- `apartment.livingRoom.*`
- `apartment.kitchen.*`
- `apartment.bathroom.*`
- `apartment.chunbaoBedroom.*`
- `apartment.protagonistBedroom.*`
- `apartment.departure.*`
- `apartment.confrontation.*`
- `apartment.ui.*`
- `apartment.item.*`

核心语言读取、保存、格式化和缺失键行为继续留在 `GameLocalization.cs`。公寓三语数据放在独立的 `ApartmentLocalizationTexts.cs`，由核心字典合并读取。学校现有文本暂不迁移，避免无关重构。

公寓脚本不再直接持有面向玩家的中文文本，而是在实际需要播放或显示时通过本地化键获取文本。说话人、对话正文、选项、教程、谜题标题、阶段提示和结局文字采用相同的数据来源。

## 物品身份与显示名称

本地化后的物品显示名称不得参与游戏逻辑判断。

- 物品匹配、合成、开锁和任务条件统一使用稳定内部 ID。
- `WorldItem.DisplayName` 只负责根据当前语言返回显示名称。
- 当前依赖“菜刀”“膏药”“粘竿菜刀”“拳击手套”“肉干”“药柜钥匙”“床头柜钥匙”等中文名称的判断，应在对应批次改为稳定 ID。
- 语言切换不得改变物品身份、背包内容、谜题进度或任务状态。

## 实施批次

### 第一批：进入公寓

覆盖：

- `ApartmentEntranceIntro`
- `ApartmentDoor`
- `ApartmentEntranceRightSideHint`
- `ApartmentLivingRoomIntro`
- 公寓入口及客厅初次进入对白
- 主角姓名显示与方向提示

### 第二批：客厅与林春宝主剧情

覆盖：

- `LinChunbaoEncounter`
- `LinChunbaoLivingRoomDialogue`
- `LivingRoomSandbag`
- `LockedGloveCabinet`
- `SandbagBoxingMinigame`
- 所有锁定选择问题、回答选项和选择教程
- 拳击训练阶段提示
- “关我何事”结局画面
- 林春宝、拳击手套、沙袋相关文本和物品显示名

该批包含最多剧情分支，需要逐条验证所有选择路线。

### 第三批：厨房与洗手间

覆盖：

- `KitchenStoveKnife`
- `LockedFoodCabinet`
- `FoodCabinetPasswordLock`
- `BathroomMedicineCabinet`
- `BathroomVentPuzzle`
- `BathroomToiletPuzzle`
- `BathroomPasswordNote`
- `MedicineCabinetSortingPuzzle`
- `PlasterKnifeCrafting`
- 食品柜密码界面、药柜整理界面及操作提示
- 菜刀、肉干、药柜钥匙、膏药、粘竿菜刀等物品显示名

### 第四批：两间卧室

覆盖：

- `ProtagonistBedroomInteractions`
- `LinChunbaoBedroomDoor`
- `LinChunbaoBedroomBed`
- `LinChunbaoPhone`
- `PasswordNote`
- 手机小游戏标题与操作提示
- 床头柜钥匙、密码纸条等物品显示名
- 卧室进入条件和手机小游戏完成后的跨房间反馈

### 第五批：离开公寓及剧情收尾

覆盖：

- `ApartmentDepartureDialogue`
- `ApartmentConfrontation`
- 离家前检查与郊区方向说明
- 林春宝、主角、行政人员的完整冲突对白
- “无归属者”结局标题与感谢文字
- 公寓章节结束前的全流程三语回归检查

## 验证要求

### 数据完整性

- 每个公寓键必须恰好具有简体中文、英文、日文三个非空值。
- 禁止重复键和引用不存在的键。
- 带 `{0}` 等格式化占位符的三种译文必须具有一致的参数数量和用途。
- 未知键继续返回键名，使漏译在测试和游玩时可见，不静默回退成中文。

### 逻辑安全

- 对话顺序、选项数量、分支结果、场景跳转和结局条件与中文原版一致。
- 语言切换不影响背包、合成、开锁、密码、药柜、沙袋或手机小游戏状态。
- 物品逻辑只比较稳定 ID，不比较本地化显示名。

### 自动测试

- 扩充 `GameLocalizationTests`，覆盖每批代表性中、英、日译文。
- 增加所有公寓键三语齐全的集中测试。
- 测试格式化占位符的一致性。
- 对菜刀、膏药、钥匙、拳击手套和肉干等关键物品验证：显示名随语言改变，内部 ID 保持不变。
- 每批运行相关 EditMode 测试和 Unity 编译。

### 场景检查

- 分别以简体中文、英文、日文检查该批涉及的场景和剧情分支。
- 检查长文本溢出、选项遮挡、换行不自然及日文字体缺字。
- 检查说话人、正文、教程、谜题标题、阶段提示和结局文字是否仍残留面向玩家的硬编码中文。
- 允许保留内部对象名称、代码注释及作为主角识别标记的 `"主角"`。
- 第五批结束后，从语言选择场景开始，分别以三种语言跑完公寓章节关键路径。

## 每批完成标准

一批只有同时满足以下条件才算完成：

- 该批所有译文已经过用户批准。
- 获批译文已经接入所有相关显示位置。
- 物品及剧情逻辑不再依赖本地化显示文本。
- 相关自动测试通过，Unity 编译通过。
- 三种语言的场景冒烟检查没有发现缺字、溢出或残留中文。
- 完成结果已向用户报告并得到确认。
