# 玩家姓名在对话框中的显示 Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 让主角对白在未起名时显示“主角”，并在玩家确认有效名字后显示该名字。

**Architecture:** 将玩家姓名状态及角色名显示规则集中到 `PlayerChosenName`。`DialogueHUD` 是普通对话和锁定选项对话共用的唯一显示入口，因此只在该入口调用角色名解析方法，剧情脚本继续保留“主角”这一原始标识。

**Tech Stack:** Unity 2022.3、C#、NUnit 编辑模式测试。

## Global Constraints

- 不修改 `Assets/Scenes Copy`。
- 不改写既有剧情页面的角色名文本。
- 非“主角”角色名必须原样显示。
- “林健太”彩蛋确认后视为有效名字，显示“林健太”。

---

### Task 1: 玩家姓名显示规则

**Files:**
- Modify: `Assets/Scripts/PlayerChosenName.cs`
- Create: `Assets/Tests/EditMode/PlayerChosenNameTests.cs`

**Interfaces:**
- Produces: `PlayerChosenName.ResolveDialogueSpeaker(string speaker)`，返回用于对话框显示的角色名。
- Produces: `PlayerChosenName.Set(string value)`，空白值恢复未命名状态。

- [x] **Step 1: Write the failing test**

```csharp
[Test]
public void ResolveDialogueSpeaker_UsesProtagonistBeforeANameIsConfirmed()
{
    PlayerChosenName.Set(null);

    Assert.That(PlayerChosenName.ResolveDialogueSpeaker("主角"), Is.EqualTo("主角"));
}

[Test]
public void ResolveDialogueSpeaker_UsesConfirmedPlayerNameForProtagonist()
{
    PlayerChosenName.Set("林健太");

    Assert.That(PlayerChosenName.ResolveDialogueSpeaker("主角"), Is.EqualTo("林健太"));
}

[Test]
public void ResolveDialogueSpeaker_KeepsOtherSpeakersUnchanged()
{
    PlayerChosenName.Set("测试名");

    Assert.That(PlayerChosenName.ResolveDialogueSpeaker("林春宝"), Is.EqualTo("林春宝"));
}
```

- [x] **Step 2: Run test to verify it fails**

Run Unity EditMode test filter `RPG.Tests.PlayerChosenNameTests`.

Expected: 编译失败，提示 `ResolveDialogueSpeaker` 尚不存在。

- [x] **Step 3: Write minimal implementation**

```csharp
private static string value;

public static string Value => string.IsNullOrWhiteSpace(value) ? "主角" : value;

public static void Set(string value)
{
    PlayerChosenName.value = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

public static string ResolveDialogueSpeaker(string speaker)
{
    return speaker == "主角" ? Value : speaker;
}
```

- [x] **Step 4: Run test to verify it passes**

Run Unity EditMode test filter `RPG.Tests.PlayerChosenNameTests`.

Expected: 3 tests pass.

### Task 2: 对话框统一使用显示名

**Files:**
- Modify: `Assets/Scripts/DialogueHUD.cs`
- Modify: `Assets/Tests/EditMode/DialogueHUDTests.cs`

**Interfaces:**
- Consumes: `PlayerChosenName.ResolveDialogueSpeaker(string speaker)`。
- Produces: 普通对话 `TryShow` / `SetPage` 与锁定选项 `TryShowLockedChoice` 中一致的主角名称显示。

- [x] **Step 1: Write the failing test**

```csharp
[Test]
public void TryShow_ReplacesProtagonistSpeakerWithConfirmedPlayerName()
{
    PlayerChosenName.Set("测试名");
    DialogueHUD hud = CreateHud(out _, out Text speaker, out _, out _);

    hud.TryShow("主角", "测试", true);

    Assert.That(speaker.text, Is.EqualTo("测试名"));
}

[Test]
public void TryShowLockedChoice_KeepsNonProtagonistSpeakerUnchanged()
{
    PlayerChosenName.Set("测试名");
    DialogueHUD hud = CreateHud(out _, out Text speaker, out _, out _);

    hud.TryShowLockedChoice("林春宝", "测试");

    Assert.That(speaker.text, Is.EqualTo("林春宝"));
}
```

- [x] **Step 2: Run test to verify it fails**

Run Unity EditMode test filter `RPG.Tests.DialogueHUDTests`.

Expected: 首个新增测试失败，实际文本仍为“主角”。

- [x] **Step 3: Write minimal implementation**

```csharp
speakerText.text = PlayerChosenName.ResolveDialogueSpeaker(speaker ?? string.Empty);
```

将该语句分别放入 `SetPage` 与 `TryShowLockedChoice`，替代直接赋值。

- [x] **Step 4: Run test to verify it passes**

Run Unity EditMode test filters `RPG.Tests.PlayerChosenNameTests` 和 `RPG.Tests.DialogueHUDTests`。

Expected: 全部通过。

### Task 3: 最终验证

**Files:**
- Verify: `Assets/Scripts/PlayerChosenName.cs`
- Verify: `Assets/Scripts/DialogueHUD.cs`
- Verify: `Assets/Tests/EditMode/PlayerChosenNameTests.cs`
- Verify: `Assets/Tests/EditMode/DialogueHUDTests.cs`

- [x] **Step 1: 编译并运行相关测试**

Run Unity EditMode filters `RPG.Tests.PlayerChosenNameTests`、`RPG.Tests.DialogueHUDTests`。

Expected: 零失败，Unity 日志没有 `error CS`、`Exception` 或 `Aborting batchmode`。

- [x] **Step 2: 检查范围**

确认生产代码仅改动姓名状态与对话框角色名显示入口；未改动任何剧情对白页面或场景文件。
