# 公寓章节多语言化实施计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 将公寓入口、客厅、厨房、洗手间、两间卧室、离家剧情及行政人员冲突完整接入简体中文、英文、日文系统。

**Architecture:** 保留 `GameLocalization` 的查询、格式化和语言持久化职责，将公寓文本放入独立的 `ApartmentLocalizationTexts` 数据类并在启动时合并。所有物品逻辑使用既有 `WorldItem.ItemId`、`PlayerInventory.HasItemById` 和 `TryRemoveSelectedById`，本地化后的 `DisplayName` 只用于显示。

**Tech Stack:** Unity 2022.3.62f3c1、C#、Unity UI、NUnit/EditMode Test Runner、自建 `GameLocalization`。

## Global Constraints

- 每批译文必须先提交用户检查；用户明确批准前不得写入运行时代码。
- 中文原文是英、日译文的唯一语义基准，不改变剧情、语气、选项结果或结局条件。
- 玩家姓名保持原样；`"主角"` 继续作为 `PlayerChosenName` 的内部识别标记。
- 林春宝固定为 `林春宝` / `Lin Chunbao` / `林春宝`。
- 行政人员固定为 `行政人员` / `Municipal Officer` / `行政職員`。
- 日文对白中主角统一以「僕」自称，林春宝统一以「俺」自称。
- 未知键继续返回键名；不引入 Unity Localization Package，不迁移学校文本。
- 每批先写失败测试，再实现、运行相关 EditMode 测试和 Unity 编译，最后单独提交。
- 禁止把工作区现有无关改动加入提交。

## 文件结构与固定接口

- Create: `Assets/Scripts/ApartmentLocalizationTexts.cs` — 只保存已获批准的公寓三语文本。
- Modify: `Assets/Scripts/GameLocalization.cs` — 合并核心文本与公寓文本，公开 API 不变。
- Create: `Assets/Tests/EditMode/ApartmentLocalizationTests.cs` — 完整性、译文与稳定物品 ID 测试。
- Create: `docs/localization/apartment-batch-1-review.md` 至 `apartment-batch-5-review.md` — 用户审核记录。
- Modify: 各任务列出的运行时脚本 — 只替换显示文本来源和中文显示名判断。

```csharp
namespace RPG
{
    public static class ApartmentLocalizationTexts
    {
        public static Dictionary<string, string[]> Create()
        {
            return new Dictionary<string, string[]>();
        }
    }
}
```

`GameLocalization` 使用下列私有方法合并数据：

```csharp
private static Dictionary<string, string[]> CreateTexts()
{
    var texts = new Dictionary<string, string[]>(CoreTexts);
    foreach (KeyValuePair<string, string[]> pair in ApartmentLocalizationTexts.Create())
        texts.Add(pair.Key, pair.Value);
    return texts;
}
```

---

### Task 1: 公寓本地化数据边界

**Files:**
- Create: `Assets/Scripts/ApartmentLocalizationTexts.cs`
- Create: `Assets/Scripts/ApartmentLocalizationTexts.cs.meta`
- Modify: `Assets/Scripts/GameLocalization.cs`
- Create: `Assets/Tests/EditMode/ApartmentLocalizationTests.cs`
- Create: `Assets/Tests/EditMode/ApartmentLocalizationTests.cs.meta`

**Interfaces:**
- Consumes: `GameLanguage`、`GameLocalization.Get(string, GameLanguage)`。
- Produces: `ApartmentLocalizationTexts.Create(): Dictionary<string, string[]>`。

- [ ] **Step 1: 写失败测试**

```csharp
[Test]
public void ApartmentTranslations_AllKeysContainThreeNonEmptyValues()
{
    Dictionary<string, string[]> texts = ApartmentLocalizationTexts.Create();
    Assert.That(texts, Is.Not.Empty);
    foreach (KeyValuePair<string, string[]> pair in texts)
    {
        Assert.That(pair.Key, Does.StartWith("apartment."));
        Assert.That(pair.Value, Has.Length.EqualTo(3));
        Assert.That(pair.Value, Has.All.Not.Null.And.Not.Empty);
    }
}

[Test]
public void Get_UsesLocalizedLinChunbaoName()
{
    Assert.That(GameLocalization.Get("apartment.speaker.linChunbao", GameLanguage.SimplifiedChinese), Is.EqualTo("林春宝"));
    Assert.That(GameLocalization.Get("apartment.speaker.linChunbao", GameLanguage.English), Is.EqualTo("Lin Chunbao"));
    Assert.That(GameLocalization.Get("apartment.speaker.linChunbao", GameLanguage.Japanese), Is.EqualTo("林春宝"));
}
```

- [ ] **Step 2: 运行测试，确认因类型不存在而失败**

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\Documents\GitHub\DevilRPGGame' -runTests -testPlatform EditMode -testFilter 'RPG.Tests.ApartmentLocalizationTests' -testResults 'D:\Documents\GitHub\DevilRPGGame\Logs\apartment-localization-infrastructure-red.xml' -logFile 'D:\Documents\GitHub\DevilRPGGame\Logs\apartment-localization-infrastructure-red.log'
```

Expected: FAIL，`ApartmentLocalizationTexts` 不存在。

- [ ] **Step 3: 创建数据类并接入核心字典**

首批只加入已批准的人物名：

```csharp
{ "apartment.speaker.linChunbao", new[] { "林春宝", "Lin Chunbao", "林春宝" } },
{ "apartment.speaker.municipalOfficer", new[] { "行政人员", "Municipal Officer", "行政職員" } }
```

将原字典改名为 `CoreTexts`，新增 `Texts = CreateTexts()`；不得改变 `Get`、`Format`、`SetLanguage`、`Reload`。

- [ ] **Step 4: 复跑 `ApartmentLocalizationTests` 和 `GameLocalizationTests`**

Expected: 两组测试全部 PASS，XML 中 `failed="0"`。

- [ ] **Step 5: 提交基础设施**

```powershell
git add -- Assets/Scripts/ApartmentLocalizationTexts.cs Assets/Scripts/ApartmentLocalizationTexts.cs.meta Assets/Scripts/GameLocalization.cs Assets/Tests/EditMode/ApartmentLocalizationTests.cs Assets/Tests/EditMode/ApartmentLocalizationTests.cs.meta
git commit -m "feat: add apartment localization data source"
```

---

### Task 2: 第一批——进入公寓

**Files:**
- Create: `docs/localization/apartment-batch-1-review.md`
- Modify: `Assets/Scripts/ApartmentLocalizationTexts.cs`
- Modify: `Assets/Scripts/ApartmentEntranceIntro.cs`
- Modify: `Assets/Scripts/ApartmentDoor.cs`
- Modify: `Assets/Scripts/ApartmentEntranceRightSideHint.cs`
- Modify: `Assets/Scripts/ApartmentLivingRoomIntro.cs`
- Modify: `Assets/Tests/EditMode/ApartmentLocalizationTests.cs`

**Interfaces:**
- Consumes: `GameLocalization.Get(string)`、`PlayerChosenName.ResolveDialogueSpeaker(string)`。
- Produces: `apartment.entrance.*`、`apartment.livingRoom.intro.*`。

- [ ] **Step 1: 生成三语审核稿并暂停**

按剧情顺序列出四个脚本的中文原文、建议键、英文、日文、使用位置、触发条件和译注。记录用户修改；未获得逐条或整批批准不得继续。

- [ ] **Step 2: 用审核稿中的批准字符串写失败测试**

```csharp
[Test]
public void Batch1_EntranceIntroUsesApprovedTranslations()
{
    const string key = "apartment.entrance.intro.1";
    Assert.That(GameLocalization.Get(key, GameLanguage.English), Is.Not.EqualTo(key));
    Assert.That(GameLocalization.Get(key, GameLanguage.Japanese), Is.Not.EqualTo(key));
}
```

在同一测试方法中继续加入英文和日文的精确 `Is.EqualTo(...)` 断言，断言字面量必须逐字复制自用户已批准的审核稿，不得重新翻译或改写。

- [ ] **Step 3: 运行测试，确认返回键名而失败**
- [ ] **Step 4: 加入获批键，并在播放时通过 `GameLocalization.Get` 构造页面**

保留 `"主角"` 作为内部标记，不改触发器、跳转和一次性状态。

- [ ] **Step 5: 运行新旧本地化测试与 Unity 编译**
- [ ] **Step 6: 中、英、日检查入口和客厅首次进入；确认姓名、换行和方向提示**
- [ ] **Step 7: 提交第一批**

```powershell
git add -- docs/localization/apartment-batch-1-review.md Assets/Scripts/ApartmentLocalizationTexts.cs Assets/Scripts/ApartmentEntranceIntro.cs Assets/Scripts/ApartmentDoor.cs Assets/Scripts/ApartmentEntranceRightSideHint.cs Assets/Scripts/ApartmentLivingRoomIntro.cs Assets/Tests/EditMode/ApartmentLocalizationTests.cs
git commit -m "feat: localize apartment arrival sequence"
```

---

### Task 3: 第二批——客厅与林春宝主剧情

**Files:**
- Create: `docs/localization/apartment-batch-2-review.md`
- Modify: `Assets/Scripts/ApartmentLocalizationTexts.cs`
- Modify: `Assets/Scripts/LinChunbaoEncounter.cs`
- Modify: `Assets/Scripts/LinChunbaoLivingRoomDialogue.cs`
- Modify: `Assets/Scripts/LivingRoomSandbag.cs`
- Modify: `Assets/Scripts/LockedGloveCabinet.cs`
- Modify: `Assets/Scripts/SandbagBoxingMinigame.cs`
- Modify: `Assets/Tests/EditMode/ApartmentLocalizationTests.cs`

**Interfaces:**
- Consumes: 人物名键、`WorldItem.ItemId`、`PlayerInventory.SelectedItem`。
- Produces: `apartment.livingRoom.*`、`apartment.ui.choice.*`、`apartment.ui.boxing.*`、`apartment.item.boxingGloves`。

- [ ] **Step 1: 审核开场问答、三个锁定问题、全部答案和分支、客厅对白、沙袋/手套柜、拳击教程、阶段文字及“关我何事”结局；暂停等待批准**
- [ ] **Step 2: 写失败测试，覆盖人物名、问题、回答、教程、格式化阶段文本、结局标题和拳击手套三语名**
- [ ] **Step 3: 运行测试并确认缺键失败**
- [ ] **Step 4: 接入获批文本，将三处物品比较从 `DisplayName` 改为 `ItemId`**

```csharp
item.ConfigureLocalization("apartment.boxingGloves", "apartment.item.boxingGloves");
```

不得改变 `ChoiceStage`、答案索引或结局分支。

- [ ] **Step 5: 运行公寓测试、林春宝相关测试和 Unity 编译**
- [ ] **Step 6: 三语验证全部选择路线、拳击三阶段和结局界面**
- [ ] **Step 7: 提交第二批，提交信息 `feat: localize apartment living room story`**

---

### Task 4: 第三批——厨房与洗手间

**Files:**
- Create: `docs/localization/apartment-batch-3-review.md`
- Modify: `Assets/Scripts/ApartmentLocalizationTexts.cs`
- Modify: `Assets/Scripts/KitchenStoveKnife.cs`
- Modify: `Assets/Scripts/LockedFoodCabinet.cs`
- Modify: `Assets/Scripts/FoodCabinetPasswordLock.cs`
- Modify: `Assets/Scripts/BathroomMedicineCabinet.cs`
- Modify: `Assets/Scripts/BathroomVentPuzzle.cs`
- Modify: `Assets/Scripts/BathroomToiletPuzzle.cs`
- Modify: `Assets/Scripts/BathroomPasswordNote.cs`
- Modify: `Assets/Scripts/MedicineCabinetSortingPuzzle.cs`
- Modify: `Assets/Scripts/PlasterKnifeCrafting.cs`
- Modify: `Assets/Scripts/ApartmentDepartureDialogue.cs`
- Modify: `Assets/Tests/EditMode/ApartmentLocalizationTests.cs`

**Interfaces:**
- Consumes: `ConfigureLocalization`、`HasItemById`、`TryRemoveSelectedById`。
- Produces: `apartment.kitchen.*`、`apartment.bathroom.*`、谜题 UI 和物品键。

- [ ] **Step 1: 审核全部调查、厕所分支、通风口三种状态、纸条、食品柜/药柜 UI 和物品名；暂停等待批准**
- [ ] **Step 2: 写失败测试，覆盖菜刀、肉干、药柜钥匙、膏药、粘竿菜刀的三语显示名与稳定 ID**
- [ ] **Step 3: 运行测试并确认失败**
- [ ] **Step 4: 接入获批文本并使用以下稳定 ID**

```text
apartment.knife
apartment.driedMeat
apartment.medicineCabinetKey
apartment.plaster
apartment.stickyKnife
```

将涉及脚本的 `DisplayName`/`HasItem` 判断改为 `ItemId`/`HasItemById`，生成物品时调用 `ConfigureLocalization(id, key)`。

- [ ] **Step 5: 运行公寓测试、库存回归测试和 Unity 编译**
- [ ] **Step 6: 三语验证厨房、食品柜密码、厕所、通风口、药柜和合成**
- [ ] **Step 7: 提交第三批，提交信息 `feat: localize apartment kitchen and bathroom`**

---

### Task 5: 第四批——两间卧室

**Files:**
- Create: `docs/localization/apartment-batch-4-review.md`
- Modify: `Assets/Scripts/ApartmentLocalizationTexts.cs`
- Modify: `Assets/Scripts/ProtagonistBedroomInteractions.cs`
- Modify: `Assets/Scripts/LinChunbaoBedroomDoor.cs`
- Modify: `Assets/Scripts/LinChunbaoBedroomBed.cs`
- Modify: `Assets/Scripts/LinChunbaoPhone.cs`
- Modify: `Assets/Scripts/PasswordNote.cs`
- Modify: `Assets/Scripts/LinChunbaoLivingRoomDialogue.cs`
- Modify: `Assets/Tests/EditMode/ApartmentLocalizationTests.cs`
- Modify: `Assets/Tests/EditMode/ApartmentBedroomDoorwayTests.cs`

**Interfaces:**
- Consumes: 已批准人物名和物品 ID。
- Produces: `apartment.chunbaoBedroom.*`、`apartment.protagonistBedroom.*`、手机 UI、`apartment.item.bedsideKey`。

- [ ] **Step 1: 审核两种房门状态、床铺、手机首次/重复调查、手机游戏 UI、纸条和钥匙；暂停等待批准**
- [ ] **Step 2: 写覆盖上述状态与床头柜钥匙稳定 ID 的失败测试**
- [ ] **Step 3: 运行测试并确认失败**
- [ ] **Step 4: 接入获批文本，使用 ID `apartment.bedsideKey`，将 `HasItem`/`DisplayName` 改为 `HasItemById`/`ItemId`**
- [ ] **Step 5: 运行公寓测试、卧室门测试和 Unity 编译**
- [ ] **Step 6: 三语验证两间卧室、跨房间反馈和手机小游戏**
- [ ] **Step 7: 提交第四批，提交信息 `feat: localize apartment bedrooms`**

---

### Task 6: 第五批——离开公寓与行政人员冲突

**Files:**
- Create: `docs/localization/apartment-batch-5-review.md`
- Modify: `Assets/Scripts/ApartmentLocalizationTexts.cs`
- Modify: `Assets/Scripts/ApartmentDepartureDialogue.cs`
- Modify: `Assets/Scripts/ApartmentConfrontation.cs`
- Modify: `Assets/Tests/EditMode/ApartmentLocalizationTests.cs`

**Interfaces:**
- Consumes: 两个人物名键及 `apartment.driedMeat`。
- Produces: `apartment.departure.*`、`apartment.confrontation.*`、结局标题和感谢文字。

- [ ] **Step 1: 审核离家对白、行政人员冲突、无归属者结局标题和感谢文字；暂停等待批准**
- [ ] **Step 2: 写覆盖行政人员名称、代表性长句、离家说明与结局 UI 的失败测试**
- [ ] **Step 3: 运行测试并确认失败**
- [ ] **Step 4: 在对话开始时构造本地化页面，不在静态初始化时缓存语言**

不得改变对象启停、角色移动、场景跳转、结局触发或按钮行为。

- [ ] **Step 5: 运行第五批测试和 Unity 编译**
- [ ] **Step 6: 三语验证正常离家路线和无归属者结局**
- [ ] **Step 7: 提交第五批，提交信息 `feat: localize apartment departure and confrontation`**

---

### Task 7: 全量回归与残留文本审计

**Files:**
- Modify: `Assets/Tests/EditMode/ApartmentLocalizationTests.cs`
- Modify only if audit finds an omission: Task 2–6 已列公寓脚本。

**Interfaces:**
- Consumes: 五批获批文本与稳定物品 ID。
- Produces: 三种语言均可完成的公寓关键路径。

- [ ] **Step 1: 增加格式化占位符一致性测试**

```csharp
[Test]
public void ApartmentTranslations_FormatPlaceholderCountsMatchAcrossLanguages()
{
    foreach (KeyValuePair<string, string[]> pair in ApartmentLocalizationTexts.Create())
    {
        int expected = CountFormatPlaceholders(pair.Value[0]);
        Assert.That(CountFormatPlaceholders(pair.Value[1]), Is.EqualTo(expected), pair.Key);
        Assert.That(CountFormatPlaceholders(pair.Value[2]), Is.EqualTo(expected), pair.Key);
    }
}
```

测试类内用正则 `\{\d+[^}]*\}` 实现 `CountFormatPlaceholders`，不增加运行时 API。

- [ ] **Step 2: 扫描面向玩家的硬编码中文**

```powershell
rg -n '[\p{Han}\p{Hiragana}\p{Katakana}]' Assets/Scripts -g '*.cs' | rg 'Apartment|Bathroom|Bedroom|Kitchen|LivingRoom|LinChunbao|FoodCabinet|MedicineCabinet|Sandbag|LockedGlove|PasswordNote|PlasterKnife'
```

只允许注释、内部对象名及 `"主角"` 标记。遗漏文本必须回到对应已批准审核稿，不得临时翻译。

- [ ] **Step 3: 运行完整 EditMode 测试**

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\Documents\GitHub\DevilRPGGame' -runTests -testPlatform EditMode -testResults 'D:\Documents\GitHub\DevilRPGGame\Logs\apartment-localization-full.xml' -logFile 'D:\Documents\GitHub\DevilRPGGame\Logs\apartment-localization-full.log'
```

Expected: XML result Passed 且 `failed="0"`；日志无编译错误。

- [ ] **Step 4: 从 `LanguageSelection` 开始完成中、英、日关键路径人工检查**

依次检查姓名、返家、林春宝选择、手套与沙袋、厨房和洗手间谜题、两间卧室、正常离家及行政人员冲突结局；检查溢出、遮挡、日文字体缺字和残留中文。

- [ ] **Step 5: 复跑完整测试并保留 XML 与日志证据**

不得以“批处理成功退出”代替测试通过证据。

- [ ] **Step 6: 只提交审计产生的测试和修复**

提交前运行 `git diff --cached --name-only`，确认没有用户无关改动；提交信息为 `test: verify apartment localization coverage`。
