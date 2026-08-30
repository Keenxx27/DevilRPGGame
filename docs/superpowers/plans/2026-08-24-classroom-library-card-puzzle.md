# 教室借书卡解谜 Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.
>
> **状态：** 待用户审批。实施计划获批前不得修改运行时代码、测试或场景。  
> **批准规格：** `docs/superpowers/specs/2026-08-24-classroom-library-card-puzzle-design.md`（项目基线核对日期：2026-08-29）

**Goal:** 在教授广播后开放教室调查，让任意一张椅子经三格物品栏投出并砸碎花盆，最终由玩家按 `E` 拾取显现的现有借书卡完成解谜。

**Architecture:** 用 `ClassroomLibraryCardPuzzle` 管理单向剧情状态，以 `InvestigationPoint` 接入现有 `PlayerInteraction` 优先级，以 `WorldItem` 的拾取门控和投掷接口接入现有三格物品栏。`ThrowableChair` 用 Collider2D 扫掠实现约 0.3 秒、2 世界单位的无阻挡飞行，`BreakableFlowerpot` 只接受飞行椅子的单次破坏请求并显现原有 `Library Card`。

**Tech Stack:** Unity 2022.3.62f3c1、C#、Legacy Input、Unity 2D Physics、Unity Scene API/YAML、UnityEngine.UI、NUnit EditMode、PowerShell 场景校验。

## Global Constraints

- 必须先完成各任务的失败测试，再写最小实现，再运行通过验证。
- 所有新增对话复用现有每秒 12 字逐字显示、`R` 翻页、`Space` 跳过和输入锁定。
- 原 10 页广播正常结束或被 `Space` 跳过后，必须自动播放 `主角：肯定不是放在别的同学的课桌。`。
- 调查前独白结束或被跳过后才开放五个调查目标。
- `E` 优先级固定为“门 > 最近调查点 > 最近可拾取物品”。
- 只要门或可调查点这一较高优先级类别找到有效目标，本次 `E` 就被消费；动作未改变状态或调查对话启动失败时也不得落到低优先级拾取。
- 四个普通调查点可重复；花盆按首次完整、再次完整、破碎三个状态处理。
- 首次花盆七页独白结束或被跳过前，全部 12 张椅子不可拾取；结束或跳过后全部解锁。
- 任意椅子占用三格物品栏中的一格，不堆叠；物品栏满时不覆盖。
- 选中椅子按 `Q` 后沿最近面朝方向平滑飞行约 0.3 秒、最多约 2 世界单位。
- 飞行椅子只在命中完整花盆、现有墙体、12 张学生课桌或最大距离时落地；落地后恢复可拾取。
- 只有飞行中的椅子能砸碎花盆；普通物品、地面椅子和角色接触无效。
- 花盆只破碎一次；椅子保留在命中位置旁，现有 `Library Card` 显现但仍需靠近按 `E` 拾取。
- 借书卡首次成功进入物品栏时完成解谜；原有 `Q` 丢弃和再次 `E` 拾取保持有效且不重复完成。
- 花盆全部可见部件复用 `Basic Unit` 方形 Sprite：棕色盆体、绿色植物、棕色碎片；不引入图片资源。
- 12 张椅子只使用 Trigger 拾取 Collider，不增加角色阻挡 Collider 或 Rigidbody2D。
- 不修改无关代码，不增加存档、任务 UI、提示图标、音效、粒子、伤害或战斗。
- 项目当前不是 Git 仓库；每个任务的提交步骤记录为 `SKIP_COMMIT_NON_GIT_PROJECT`，不初始化仓库、不执行 Git 提交。

---

## 文件结构

### 新增运行时代码

- `Assets/Scripts/InvestigationPoint.cs`：调查种类、调查处理接口、Trigger 登记与注销。
- `Assets/Scripts/ChairThrowObstacle.cs`：标识椅子飞行应停止的墙体和学生课桌。
- `Assets/Scripts/ThrowableChair.cs`：椅子投掷状态、扫掠检测、落地和恢复拾取。
- `Assets/Scripts/BreakableFlowerpot.cs`：花盆命中区、一次性破碎、视觉切换与借书卡显现。
- `Assets/Scripts/ClassroomLibraryCardPuzzle.cs`：开场串联、调查页面、椅子解锁和解谜完成状态。

每个新增运行时脚本同时创建对应 `.meta`。固定 GUID：

```text
InvestigationPoint.cs       7c4c9060f30a4f678e1f136202408013
ChairThrowObstacle.cs       7c4c9060f30a4f678e1f136202408014
ThrowableChair.cs           7c4c9060f30a4f678e1f136202408015
BreakableFlowerpot.cs       7c4c9060f30a4f678e1f136202408016
ClassroomLibraryCardPuzzle.cs 7c4c9060f30a4f678e1f136202408017
```

### 修改运行时代码

- `Assets/Scripts/DialogueController.cs`：增加一次性对话完成回调。
- `Assets/Scripts/BroadcastDialogueTrigger.cs`：转发广播完成通知。
- `Assets/Scripts/PlayerInteraction.cs`：登记调查点并执行三级 `E` 优先级。
- `Assets/Scripts/PlayerInventory.cs`：过滤不可拾取物品，并把 `Q` 分为普通丢弃和特殊投掷。
- `Assets/Scripts/WorldItem.cs`：增加拾取门控、拾取事件和可选投掷行为分派。

### 新增/修改测试

- Modify: `Assets/Tests/EditMode/DialogueControllerTests.cs`
- Modify: `Assets/Tests/EditMode/DialogueTriggerTests.cs`
- Modify: `Assets/Tests/EditMode/PlayerInteractionTests.cs`
- Modify: `Assets/Tests/EditMode/InventoryInteractionTests.cs`
- Create: `Assets/Tests/EditMode/ThrowableChairTests.cs`
- Create: `Assets/Tests/EditMode/BreakableFlowerpotTests.cs`
- Create: `Assets/Tests/EditMode/ClassroomLibraryCardPuzzleTests.cs`
- Create: `Assets/Tests/EditMode/ClassroomLibraryCardPuzzleSceneTests.cs`

新增测试文件同时创建对应 `.meta`，由 Unity 导入时生成 GUID；测试脚本不被场景引用，不要求固定 GUID。

### 场景与验证

- Modify: `Assets/Scenes/SampleScene.unity`
- Create temporarily, then remove after successful scene save: `Assets/Editor/ClassroomLibraryCardPuzzleInstaller.cs` 及 `.meta`
- Create: `work/classroom-library-card-puzzle/verify_scene.ps1`
- Create: `work/classroom-library-card-puzzle/manual-playmode-checklist.md`

---

### Task 1: 对话完成回调与广播串联通知

**Files:**
- Modify: `Assets/Scripts/DialogueController.cs`
- Modify: `Assets/Scripts/BroadcastDialogueTrigger.cs`
- Modify: `Assets/Tests/EditMode/DialogueControllerTests.cs`
- Modify: `Assets/Tests/EditMode/DialogueTriggerTests.cs`

**Interfaces:**
- Produces: `bool DialogueController.StartDialogue(DialoguePage[] pages, System.Action onFinished)`。
- Preserves: `bool DialogueController.StartDialogue(DialoguePage[] pages)`。
- Produces: `event System.Action BroadcastDialogueTrigger.Completed`。
- Guarantees: 完成回调在正常结束、`Space` 跳过或已启动后的 HUD 失败路径调用一次；启动请求被拒绝时不调用。

- [ ] **Step 1: 写入失败的对话回调测试**

在 `DialogueControllerTests.cs` 增加：

```csharp
[Test]
public void CompletionCallback_RunsOnceAfterSkipAndCanChainDialogue()
{
    DialogueController controller = CreateController(out _, out _, out _);
    int completed = 0;

    Assert.That(controller.StartDialogue(
        OnePage("教授", "第一段"),
        () =>
        {
            completed++;
            Assert.That(controller.IsPlaying, Is.False);
            Assert.That(controller.StartDialogue(OnePage("主角", "第二段")), Is.True);
        }), Is.True);

    controller.SkipDialogue();

    Assert.That(completed, Is.EqualTo(1));
    Assert.That(controller.IsPlaying, Is.True);
    Assert.That(controller.CurrentSpeaker, Is.EqualTo("主角"));
}

[Test]
public void CompletionCallback_DoesNotRunWhenStartIsRejected()
{
    DialogueController controller = CreateController(out _, out _, out _);
    int completed = 0;
    controller.StartDialogue(OnePage("教授", "进行中"));

    Assert.That(controller.StartDialogue(
        OnePage("主角", "被拒绝"), () => completed++), Is.False);
    Assert.That(completed, Is.Zero);
}
```

再增加“最后一页显示完成后 `TryAdvanceDialogue()` 返回 `Completed` 并调用一次回调”的测试；通过 `Tick(1f)` 让一页短文本完整显示。

- [ ] **Step 2: 运行 RED 测试**

Run:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\RPG project 1' -runTests -testPlatform EditMode -testFilter 'RPG.Tests.DialogueControllerTests' -testResults 'D:\RPG project 1\Temp\classroom-puzzle-dialogue-red.xml' -logFile 'D:\RPG project 1\Logs\classroom-puzzle-dialogue-red.log'
```

Expected: 编译失败，指出缺少带 `Action` 参数的 `StartDialogue`。

- [ ] **Step 3: 实现一次性完成回调**

在 `DialogueController` 中加入：

```csharp
private System.Action onFinished;

public bool StartDialogue(DialoguePage[] pages)
{
    return StartDialogue(pages, null);
}

public bool StartDialogue(DialoguePage[] pages, System.Action completion)
{
    if (IsPlaying || !HasValidPages(pages)) return false;
    if (hud == null || movement == null || playerInteraction == null
        || playerInventory == null)
    {
        Debug.LogError(
            "DialogueController requires HUD and gameplay component references.", this);
        return false;
    }

    onFinished = completion;
    playback = new DialoguePlayback(pages);
    movementWasEnabled = movement.enabled;
    interactionWasEnabled = playerInteraction.enabled;
    inventoryWasEnabled = playerInventory.enabled;
    movement.enabled = false;
    playerInteraction.enabled = false;
    playerInventory.enabled = false;

    if (!hud.TryShow(playback.Speaker, playback.VisibleText, playback.IsPageComplete))
    {
        FinishDialogue();
        return false;
    }
    return true;
}
```

把 `FinishDialogue()` 的顺序改为：捕获回调、将字段置空、隐藏 HUD、恢复组件、将 `playback = null`，最后调用捕获的回调。不得在回调后再次覆写 `playback` 或组件状态。

- [ ] **Step 4: 写入失败的广播完成通知测试**

在 `DialogueTriggerTests.cs` 增加：

```csharp
[Test]
public void BroadcastTrigger_RaisesCompletedAfterSkippedDialogue()
{
    DialogueController controller = CreateController();
    BroadcastDialogueTrigger trigger = Track(new GameObject("Broadcast"))
        .AddComponent<BroadcastDialogueTrigger>();
    SetField(trigger, "controller", controller);
    SetField(trigger, "pages", OnePage("教授"));
    int completed = 0;
    trigger.Completed += () => completed++;

    Assert.That(trigger.Trigger(), Is.True);
    Assert.That(completed, Is.Zero);
    controller.SkipDialogue();

    Assert.That(completed, Is.EqualTo(1));
}
```

- [ ] **Step 5: 实现广播完成通知并运行 GREEN**

在 `BroadcastDialogueTrigger` 中加入：

```csharp
public event System.Action Completed;

private void NotifyCompleted()
{
    Completed?.Invoke();
}
```

把首次请求改为 `controller.StartDialogue(pages, NotifyCompleted)`；`HasTriggered` 与只触发一次语义不变。重新运行 `DialogueControllerTests` 和 `DialogueTriggerTests`。

Expected: 两组测试全部通过，既有输入锁定和私聊测试无回归。

- [ ] **Step 6: 跳过提交**

记录：`SKIP_COMMIT_NON_GIT_PROJECT`。

---

### Task 2: 调查点与三级 E 优先级

**Files:**
- Create: `Assets/Scripts/InvestigationPoint.cs`
- Create: `Assets/Scripts/InvestigationPoint.cs.meta`
- Modify: `Assets/Scripts/PlayerInteraction.cs`
- Modify: `Assets/Tests/EditMode/PlayerInteractionTests.cs`

**Interfaces:**
- Produces: `enum InvestigationKind { StorageCabinet, TeacherDesk, Podium, Blackboard, Flowerpot }`。
- Produces: `IInvestigationHandler.CanInvestigate(InvestigationKind kind) -> bool`。
- Produces: `IInvestigationHandler.TryInvestigate(InvestigationKind kind) -> bool`。
- Produces: `InvestigationPoint.Kind`、`IsAvailable`、`TryInvestigate()`。
- Produces: `PlayerInteraction.RegisterInvestigation(InvestigationPoint)`、`UnregisterInvestigation(InvestigationPoint)`、`ClearNearbyInvestigations()`。

- [ ] **Step 1: 创建失败的优先级测试**

在测试内增加实现 `IInvestigationHandler` 的 `FakeInvestigationHandler : MonoBehaviour`，用 `Available` 控制 `CanInvestigate`，用 `TryResult` 控制 `TryInvestigate` 返回值，并用 `Calls` 记录调用次数。增加以下测试：

```csharp
[Test]
public void DoorConsumesInteractionBeforeInvestigationAndPickup()
{
    PlayerInteraction interaction = CreatePlayer(out PlayerInventory inventory);
    DoorController door = CreateDoor("Door", new Vector2(1f, 0f));
    FakeInvestigationHandler handler = CreateInvestigation(
        new Vector2(0.5f, 0f), out InvestigationPoint point);
    WorldItem item = CreateItem(new Vector2(0.25f, 0f));
    interaction.RegisterDoor(door);
    interaction.RegisterInvestigation(point);
    inventory.RegisterNearby(item);

    Assert.That(interaction.TryInteract(), Is.True);
    Assert.That(door.State, Is.EqualTo(DoorState.Opening));
    Assert.That(handler.Calls, Is.Zero);
    Assert.That(inventory.GetSlotItem(0), Is.Null);
}

[Test]
public void InvestigationConsumesInteractionBeforePickup()
{
    PlayerInteraction interaction = CreatePlayer(out PlayerInventory inventory);
    FakeInvestigationHandler handler = CreateInvestigation(
        Vector2.right, out InvestigationPoint point);
    WorldItem item = CreateItem(new Vector2(0.5f, 0f));
    interaction.RegisterInvestigation(point);
    inventory.RegisterNearby(item);

    Assert.That(interaction.TryInteract(), Is.True);
    Assert.That(handler.Calls, Is.EqualTo(1));
    Assert.That(inventory.GetSlotItem(0), Is.Null);
}
```

另测：

- 两个调查点只调用最近者。
- `Available == false` 的调查点不是有效目标，应被忽略并回退拾取。
- `Available == true` 但 `TryResult == false` 的调查点仍消费本次 `E`，不得拾取附近物品。
- 门已处于 Opening/Closing、`ToggleDoor()` 返回 false 时仍消费本次 `E`，不得调查或拾取。
- 停用调查点会从已登记玩家中注销。

失败消费测试的核心断言为：

```csharp
handler.Available = true;
handler.TryResult = false;

Assert.That(interaction.TryInteract(), Is.True);
Assert.That(handler.Calls, Is.EqualTo(1));
Assert.That(inventory.GetSlotItem(0), Is.Null);
Assert.That(item.gameObject.activeSelf, Is.True);
```

- [ ] **Step 2: 运行 RED**

Expected: 编译失败，报告 `InvestigationPoint`、`IInvestigationHandler` 和注册方法缺失。

- [ ] **Step 3: 实现调查点**

`InvestigationPoint.cs` 的核心定义固定为：

```csharp
public enum InvestigationKind
{
    StorageCabinet,
    TeacherDesk,
    Podium,
    Blackboard,
    Flowerpot
}

public interface IInvestigationHandler
{
    bool CanInvestigate(InvestigationKind kind);
    bool TryInvestigate(InvestigationKind kind);
}

[RequireComponent(typeof(BoxCollider2D))]
public sealed class InvestigationPoint : MonoBehaviour
{
    [SerializeField] private InvestigationKind kind;
    [SerializeField] private MonoBehaviour handler;
    private readonly List<PlayerInteraction> registeredPlayers =
        new List<PlayerInteraction>();

    public InvestigationKind Kind => kind;
    public bool IsAvailable => handler is IInvestigationHandler target
        && target.CanInvestigate(kind);

    public bool TryInvestigate()
    {
        return handler is IInvestigationHandler target
            && target.TryInvestigate(kind);
    }
}
```

`OnTriggerEnter2D` 查找 `PlayerInteraction`、去重登记并记录玩家；`OnTriggerExit2D` 注销并移除；`OnDisable` 从所有已记录玩家注销后清空列表。非玩家 Collider 不产生状态。

- [ ] **Step 4: 实现 PlayerInteraction 三级优先级**

增加 `nearbyInvestigations` 列表和最近项清理/距离选择。`TryInteract()` 固定为：

```csharp
public bool TryInteract()
{
    DoorController door = FindNearestDoor();
    if (door != null)
    {
        door.ToggleDoor();
        return true;
    }

    InvestigationPoint investigation = FindNearestInvestigation();
    if (investigation != null)
    {
        investigation.TryInvestigate();
        return true;
    }

    return inventory != null && inventory.TryPickUpNearest();
}
```

`FindNearestInvestigation` 倒序清理 null 或 inactive 条目；对 `!IsAvailable` 的条目只跳过而不移除，保证调查状态稍后开放时，站在原 Trigger 内的玩家无需先离开再进入。其余可用条目按平方距离选择最近对象。

- [ ] **Step 5: 运行 GREEN 与门/拾取回归**

Expected: `PlayerInteractionTests` 全部通过；既有“门优先于拾取”“无门时拾取最近物品”行为保持不变。

- [ ] **Step 6: 跳过提交**

记录：`SKIP_COMMIT_NON_GIT_PROJECT`。

---

### Task 3: 世界物品拾取门控、拾取事件与 Q 行为分派

**Files:**
- Modify: `Assets/Scripts/WorldItem.cs`
- Modify: `Assets/Scripts/PlayerInventory.cs`
- Modify: `Assets/Tests/EditMode/InventoryInteractionTests.cs`

**Interfaces:**
- Produces: `enum WorldItemThrowResult { NotSupported, Rejected, Started }`。
- Produces: `IWorldItemThrowBehavior.TryThrow(Vector2 origin, Vector2 direction) -> bool`。
- Produces: `WorldItem.CanPickUp`、`SetCanPickUp(bool)`、`event Action<WorldItem> PickedUp`。
- Produces: `WorldItem.TryThrow(Vector2 origin, Vector2 direction) -> WorldItemThrowResult`。
- Preserves: 普通物品 `Q` 使用 `DropDistance = 1.2f`。

- [ ] **Step 1: 写入失败的拾取门控测试**

在 `InventoryInteractionTests.cs` 增加：

```csharp
[Test]
public void DisabledPickup_IsIgnoredUntilEnabledWithoutReregistering()
{
    PlayerInventory inventory = CreatePlayer();
    WorldItem chair = CreateItem("Chair", Vector2.right);
    chair.SetCanPickUp(false);
    inventory.RegisterNearby(chair);

    Assert.That(inventory.TryPickUpNearest(), Is.False);
    chair.SetCanPickUp(true);
    Assert.That(inventory.TryPickUpNearest(), Is.True);
    Assert.That(inventory.GetSlotItem(0), Is.SameAs(chair));
}

[Test]
public void SuccessfulPickup_RaisesEventOnce()
{
    PlayerInventory inventory = CreatePlayer();
    WorldItem card = CreateItem("Library Card", Vector2.right);
    int calls = 0;
    card.PickedUp += picked =>
    {
        Assert.That(picked, Is.SameAs(card));
        calls++;
    };
    inventory.RegisterNearby(card);

    Assert.That(inventory.TryPickUpNearest(), Is.True);
    Assert.That(calls, Is.EqualTo(1));
}
```

- [ ] **Step 2: 写入失败的特殊 Q 分派测试**

创建测试用 `FakeThrowBehavior : MonoBehaviour, IWorldItemThrowBehavior`。覆盖：返回 true 时槽位清空且收到玩家位置/归一化朝向；返回 false 时槽位与 inactive 物品保持不变；无行为组件的借书卡仍丢到 1.2 单位。

核心断言：

```csharp
Assert.That(thrower.Origin, Is.EqualTo((Vector2)inventory.transform.position));
Assert.That(thrower.Direction, Is.EqualTo(Vector2.down));
Assert.That(inventory.GetSlotItem(0), Is.Null);
```

- [ ] **Step 3: 运行 RED**

Expected: 编译失败，指出拾取门控、事件和投掷接口不存在。

- [ ] **Step 4: 实现 WorldItem 扩展**

在 `WorldItem.cs` 中加入：

```csharp
public enum WorldItemThrowResult { NotSupported, Rejected, Started }

public interface IWorldItemThrowBehavior
{
    bool TryThrow(Vector2 origin, Vector2 direction);
}

[SerializeField] private bool canPickUp = true;
public bool CanPickUp => canPickUp;
public event System.Action<WorldItem> PickedUp;

public void SetCanPickUp(bool value) => canPickUp = value;

public WorldItemThrowResult TryThrow(Vector2 origin, Vector2 direction)
{
    foreach (MonoBehaviour behaviour in GetComponents<MonoBehaviour>())
    {
        if (behaviour is IWorldItemThrowBehavior throwBehavior)
        {
            return throwBehavior.TryThrow(origin, direction)
                ? WorldItemThrowResult.Started
                : WorldItemThrowResult.Rejected;
        }
    }
    return WorldItemThrowResult.NotSupported;
}
```

`PickUp()` 在停用 GameObject 后执行 `PickedUp?.Invoke(this)`；不得清除订阅者。`PlayerInventory.FindNearestActiveItem()` 过滤 `!item.CanPickUp`。

- [ ] **Step 5: 实现 PlayerInventory 的 Q 分流**

把 `DropSelected()` 改为先窥视后提交：

```csharp
private void DropSelected()
{
    WorldItem item = slots.Get(slots.SelectedIndex);
    if (item == null) return;

    Vector2 direction = movement.FacingDirection.sqrMagnitude > 0f
        ? movement.FacingDirection.normalized
        : Vector2.down;
    WorldItemThrowResult result = item.TryThrow(transform.position, direction);
    if (result == WorldItemThrowResult.Rejected) return;

    slots.RemoveSelected();
    if (result == WorldItemThrowResult.NotSupported)
    {
        item.Drop(CalculateDropPosition(transform.position, direction, DropDistance));
    }
}
```

- [ ] **Step 6: 运行 GREEN 和库存回归**

Expected: `InventoryInteractionTests`、`InventorySlotsTests`、`InventoryHUDTests` 全部通过；普通借书卡丢弃位置保持 1.2 单位。

- [ ] **Step 7: 跳过提交**

记录：`SKIP_COMMIT_NON_GIT_PROJECT`。

---

### Task 4: 椅子飞行、障碍停止与花盆单次破碎

**Files:**
- Create: `Assets/Scripts/ChairThrowObstacle.cs`
- Create: `Assets/Scripts/ChairThrowObstacle.cs.meta`
- Create: `Assets/Scripts/ThrowableChair.cs`
- Create: `Assets/Scripts/ThrowableChair.cs.meta`
- Create: `Assets/Scripts/BreakableFlowerpot.cs`
- Create: `Assets/Scripts/BreakableFlowerpot.cs.meta`
- Create: `Assets/Tests/EditMode/ThrowableChairTests.cs`
- Create: `Assets/Tests/EditMode/BreakableFlowerpotTests.cs`

**Interfaces:**
- Produces: 空标记组件 `ChairThrowObstacle`。
- Produces: `ThrowableChair : MonoBehaviour, IWorldItemThrowBehavior`。
- Produces: `ThrowableChair.IsFlying`、`TravelledDistance`、`TryThrow(origin, direction)`、`Tick(deltaTime)`。
- Produces: `BreakableFlowerpot.IsBroken`、`IsHitbox(Collider2D)`、`TryBreak(ThrowableChair)`、`event Action Broken`。
- Constants: 默认 `throwDuration = 0.3f`、`throwDistance = 2f`、`collisionSkin = 0.01f`。

- [ ] **Step 1: 写入失败的椅子飞行测试**

`ThrowableChairTests` 至少覆盖：

```csharp
[Test]
public void Throw_ReachesTwoUnitsInPointThreeSecondsAndBecomesPickupable()
{
    ThrowableChair chair = CreateChair(out WorldItem item);

    Assert.That(chair.TryThrow(Vector2.zero, Vector2.right), Is.True);
    Assert.That(chair.IsFlying, Is.True);
    Assert.That(item.CanPickUp, Is.False);

    chair.Tick(0.15f);
    Assert.That(chair.transform.position.x, Is.EqualTo(1f).Within(0.02f));
    chair.Tick(0.15f);

    Assert.That(chair.transform.position.x, Is.EqualTo(2f).Within(0.02f));
    Assert.That(chair.IsFlying, Is.False);
    Assert.That(item.CanPickUp, Is.True);
}
```

另测零方向回退向下、飞行中拒绝第二次投掷、带 `ChairThrowObstacle` 的 BoxCollider2D 使椅子提前落地且不穿透。碰撞测试放置物体后调用 `Physics2D.SyncTransforms()`。

- [ ] **Step 2: 运行 RED**

Expected: 编译失败，报告三个新运行时类型缺失。

- [ ] **Step 3: 实现障碍标记与椅子飞行**

`ChairThrowObstacle` 保持空类。`ThrowableChair` 缓存同对象 `WorldItem` 和 Trigger `BoxCollider2D`，`Update()` 调用 `Tick(Time.deltaTime)`。启动逻辑：

```csharp
public bool TryThrow(Vector2 origin, Vector2 direction)
{
    if (IsFlying) return false;
    direction = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.down;
    transform.position = origin;
    gameObject.SetActive(true);
    worldItem.SetCanPickUp(false);
    flightDirection = direction;
    travelledDistance = 0f;
    IsFlying = true;
    return true;
}
```

`Tick` 每次计算 `step = min(throwDistance - travelledDistance, throwDistance / throwDuration * deltaTime)`，用椅子 Collider 的 `Cast` 和 `ContactFilter2D.NoFilter()`（`useTriggers = true`）取得沿方向最近的有效命中。仅接受：

```csharp
ChairThrowObstacle obstacle = hit.collider.GetComponentInParent<ChairThrowObstacle>();
BreakableFlowerpot pot = hit.collider.GetComponentInParent<BreakableFlowerpot>();
bool validPotHit = pot != null && pot.IsHitbox(hit.collider) && !pot.IsBroken;
```

命中时移动 `max(0, hit.distance - collisionSkin)`，若为完整花盆则在 `IsFlying` 仍为 true 时调用 `pot.TryBreak(this)`，再执行 `Land()`。未命中则移动完整 step；累计到 2f 时 `Land()`。`Land()` 设置 `IsFlying = false` 并恢复 `worldItem.SetCanPickUp(true)`。

- [ ] **Step 4: 写入失败的花盆测试**

`BreakableFlowerpotTests` 创建完整盆体、植物、初始 inactive 碎片、初始 inactive 借书卡、专用 Trigger hitbox 和调查点。测试：普通 null 请求失败；非飞行椅子失败；飞行椅子首次请求成功并切换所有 active 状态；第二次请求失败且 `Broken` 事件总计一次。

核心断言：

```csharp
Assert.That(pot.TryBreak(flyingChair), Is.True);
Assert.That(pot.IsBroken, Is.True);
Assert.That(intactBody.activeSelf, Is.False);
Assert.That(plant.activeSelf, Is.False);
Assert.That(shards.activeSelf, Is.True);
Assert.That(card.activeSelf, Is.True);
Assert.That(investigation.gameObject.activeSelf, Is.False);
Assert.That(brokenCalls, Is.EqualTo(1));
```

- [ ] **Step 5: 实现花盆单次破碎**

固定序列化字段：`intactBody`、`plant`、`shards`、`hitbox`、`investigationPoint`、`libraryCard`。实现：

```csharp
public bool TryBreak(ThrowableChair chair)
{
    if (isBroken || chair == null || !chair.IsFlying) return false;
    isBroken = true;
    intactBody.SetActive(false);
    plant.SetActive(false);
    shards.SetActive(true);
    investigationPoint.gameObject.SetActive(false);
    libraryCard.gameObject.SetActive(true);
    Broken?.Invoke();
    return true;
}

public bool IsHitbox(Collider2D candidate)
{
    return candidate != null && candidate == hitbox;
}
```

`Awake()` 验证全部引用；缺失时记录一次 `Debug.LogError`，不得伪造对象或自动寻找同名物体。

- [ ] **Step 6: 运行 GREEN**

Expected: `ThrowableChairTests` 与 `BreakableFlowerpotTests` 全部通过；命中花盆后椅子 active、落地并可再次拾取，花盆只触发一次。

- [ ] **Step 7: 跳过提交**

记录：`SKIP_COMMIT_NON_GIT_PROJECT`。

---

### Task 5: 教室借书卡谜题状态机与精确台词

**Files:**
- Create: `Assets/Scripts/ClassroomLibraryCardPuzzle.cs`
- Create: `Assets/Scripts/ClassroomLibraryCardPuzzle.cs.meta`
- Create: `Assets/Tests/EditMode/ClassroomLibraryCardPuzzleTests.cs`

**Interfaces:**
- Produces: `enum ClassroomPuzzleState { WaitingForOpening, PlayingSearchIntro, Searching, PlayingFirstFlowerpot, ChairsUnlocked, FlowerpotBroken, Completed }`。
- Produces: `ClassroomLibraryCardPuzzle : MonoBehaviour, IInvestigationHandler`。
- Produces: `State`、`IsCompleted`、`CanInvestigate(kind)`、`TryInvestigate(kind)`。
- Consumes: `BroadcastDialogueTrigger.Completed`、`BreakableFlowerpot.Broken`、`WorldItem.PickedUp`。

- [ ] **Step 1: 写入失败的开场与调查状态测试**

测试夹具创建完整 `DialogueController`/HUD、`BroadcastDialogueTrigger`、12 个 `WorldItem` 椅子、inactive 借书卡、`BreakableFlowerpot` 和谜题组件，并通过反射写入页面数组。至少覆盖：

```csharp
[Test]
public void OpeningSkip_ChainsSearchIntroThenOpensInvestigations()
{
    PuzzleFixture fixture = CreateFixture();
    fixture.Broadcast.Trigger();

    fixture.Dialogue.SkipDialogue();
    Assert.That(fixture.Puzzle.State,
        Is.EqualTo(ClassroomPuzzleState.PlayingSearchIntro));
    Assert.That(fixture.Dialogue.CurrentSpeaker, Is.EqualTo("主角"));
    Assert.That(fixture.Puzzle.CanInvestigate(InvestigationKind.Blackboard), Is.False);

    fixture.Dialogue.SkipDialogue();
    Assert.That(fixture.Puzzle.State, Is.EqualTo(ClassroomPuzzleState.Searching));
    Assert.That(fixture.Puzzle.CanInvestigate(InvestigationKind.Blackboard), Is.True);
}
```

再测四个普通调查可重复且状态不变；每种调查启动的 `CurrentSpeaker` 均为“主角”。

- [ ] **Step 2: 写入失败的花盆解锁与完成测试**

覆盖：12 张椅子初始 `CanPickUp == false`；首次花盆调查启动后仍全部 false；跳过七页后全部 true 且状态为 `ChairsUnlocked`；再次完整花盆调查不重复解锁；花盆破碎进入 `FlowerpotBroken`；借书卡首次 `PickUp()` 进入 `Completed` 并启动完成独白；再次拾取不重复启动。

- [ ] **Step 3: 运行 RED**

Expected: 编译失败，报告 `ClassroomLibraryCardPuzzle` 和 `ClassroomPuzzleState` 缺失。

- [ ] **Step 4: 实现订阅生命周期和状态转换**

固定序列化字段：

```csharp
[SerializeField] private DialogueController dialogueController;
[SerializeField] private BroadcastDialogueTrigger openingBroadcast;
[SerializeField] private BreakableFlowerpot flowerpot;
[SerializeField] private WorldItem libraryCard;
[SerializeField] private WorldItem[] chairs;
[SerializeField] private DialoguePage[] searchIntroPages;
[SerializeField] private DialoguePage[] storageCabinetPages;
[SerializeField] private DialoguePage[] teacherDeskPages;
[SerializeField] private DialoguePage[] podiumPages;
[SerializeField] private DialoguePage[] blackboardPages;
[SerializeField] private DialoguePage[] firstFlowerpotPages;
[SerializeField] private DialoguePage[] repeatFlowerpotPages;
[SerializeField] private DialoguePage[] brokenFlowerpotPages;
[SerializeField] private DialoguePage[] cardPickedUpPages;
```

`OnEnable` 订阅三项事件，`OnDisable` 对称退订。`Awake` 把全部非 null 椅子设为不可拾取。广播完成后先将状态设为 `PlayingSearchIntro`，再调用 `StartDialogue(searchIntroPages, FinishSearchIntro)`；启动返回 false 且状态未被完成回调改变时恢复 `WaitingForOpening` 并记录错误。`FinishSearchIntro` 将状态改为 `Searching`。

- [ ] **Step 5: 实现调查分派**

普通调查使用以下固定映射：

```csharp
private DialoguePage[] PagesFor(InvestigationKind kind)
{
    switch (kind)
    {
        case InvestigationKind.StorageCabinet: return storageCabinetPages;
        case InvestigationKind.TeacherDesk: return teacherDeskPages;
        case InvestigationKind.Podium: return podiumPages;
        case InvestigationKind.Blackboard: return blackboardPages;
        default: return null;
    }
}
```

花盆首次调查时先置 `PlayingFirstFlowerpot`，成功启动七页后等待完成回调；回调把 12 张椅子全部 `SetCanPickUp(true)` 并置 `ChairsUnlocked`。首次启动失败时恢复 `Searching`。`ChairsUnlocked` 状态再次调查完整花盆时播放重复一页且不改状态。`FlowerpotBroken`/`Completed` 仍允许四个普通调查；花盆调查返回 false。

- [ ] **Step 6: 实现破碎和借书卡完成**

`BreakableFlowerpot.Broken` 只在 `ChairsUnlocked` 状态把状态置为 `FlowerpotBroken` 并播放 `brokenFlowerpotPages`。`WorldItem.PickedUp` 只在参数与 `libraryCard` 为同一实例、状态为 `FlowerpotBroken` 时把状态置为 `Completed` 并播放 `cardPickedUpPages`；状态先推进，因此重复拾取不会重复播放。

- [ ] **Step 7: 写入精确页面夹具并运行 GREEN**

测试中的页面数组必须逐字使用规格中的 15 条台词，并额外断言首次花盆数组长度为 7、其他新增段落长度为 1。运行 `ClassroomLibraryCardPuzzleTests` 与 Task 1–4 的全部测试。

Expected: 状态单向推进；正常结束与 `Space` 跳过走相同完成回调；四个普通点重复调查；花盆与借书卡只完成一次。

- [ ] **Step 8: 跳过提交**

记录：`SKIP_COMMIT_NON_GIT_PROJECT`。

---

### Task 6: SampleScene 调查点、花盆、12 张椅子与借书卡装配

**Files:**
- Create temporarily: `Assets/Editor/ClassroomLibraryCardPuzzleInstaller.cs`
- Create temporarily: `Assets/Editor/ClassroomLibraryCardPuzzleInstaller.cs.meta`
- Modify: `Assets/Scenes/SampleScene.unity`
- Create: `Assets/Tests/EditMode/ClassroomLibraryCardPuzzleSceneTests.cs`
- Create: `work/classroom-library-card-puzzle/verify_scene.ps1`

**Interfaces:**
- Consumes: Task 1–5 的全部运行时组件。
- Produces: `SampleScene` 中唯一的 `Classroom Library Card Puzzle`、五个调查点、一个 Basic Unit 花盆组合、12 张可投掷椅子和原有借书卡引用。
- Preserves: 主角、教授、门、Portal、Canvas、原静物 Transform 和 `Basic Unit` inactive 状态。

- [ ] **Step 1: 写入失败的语义场景测试**

`ClassroomLibraryCardPuzzleSceneTests` 用 `EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity", OpenSceneMode.Single)`，按层级查找并断言：

```csharp
Assert.That(Object.FindObjectsOfType<ClassroomLibraryCardPuzzle>().Length, Is.EqualTo(1));
Assert.That(Object.FindObjectsOfType<InvestigationPoint>().Length, Is.EqualTo(5));
Assert.That(Object.FindObjectsOfType<ThrowableChair>().Length, Is.EqualTo(12));
Assert.That(Object.FindObjectsOfType<BreakableFlowerpot>().Length, Is.EqualTo(1));
GameObject card = FindIncludingInactive(scene, "Library Card");
Assert.That(card, Is.Not.Null);
Assert.That(card.activeSelf, Is.False);
```

对 12 个名称匹配 `Chair R1C1` 至 `Chair R4C3` 的对象逐一断言只有一个 Trigger `BoxCollider2D`、有 `WorldItem`/`ThrowableChair`、`CanPickUp == false`、无 Rigidbody2D。断言五个调查种类各出现一次；17 个 `ChairThrowObstacle` 精确绑定 5 段墙体和 12 张学生课桌，不绑定教师桌、讲台或椅子。

- [ ] **Step 2: 写入失败的 PowerShell YAML 校验并运行 RED**

`verify_scene.ps1` 读取 `SampleScene.unity` 和五个 `.meta` GUID，检查：

```text
CLASSROOM_LIBRARY_CARD_PUZZLE_SCENE=PASS
Puzzle controller count = 1
InvestigationPoint count = 5
ThrowableChair count = 12
Chair WorldItem count = 12
Chair trigger count = 12
Chair non-trigger collider count = 0
Chair Rigidbody2D count = 0
ChairThrowObstacle count = 17
BreakableFlowerpot count = 1
Library Card GameObject count = 1 and initial active = 0
Basic Unit root active = 0
Approved dialogue lines matched = 15
```

Run:

```powershell
& 'D:\RPG project 1\work\classroom-library-card-puzzle\verify_scene.ps1'
```

Expected: FAIL，报告谜题对象和新组件尚未配置。

- [ ] **Step 3: 创建一次性 Editor 安装器**

安装器入口固定为：

```csharp
public static class ClassroomLibraryCardPuzzleInstaller
{
    private const string ScenePath = "Assets/Scenes/SampleScene.unity";

    public static void Install()
    {
        Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        ConfigureFlowerpot(scene);
        ConfigureChairs(scene);
        ConfigureObstacles(scene);
        ConfigurePuzzle(scene);
        ConfigureInvestigations(scene);
        EditorSceneManager.MarkSceneDirty(scene);
        if (!EditorSceneManager.SaveScene(scene))
            throw new InvalidOperationException("Failed to save SampleScene puzzle setup.");
    }
}
```

所有查找使用完整对象名并要求恰好一个匹配；缺失或重复立即抛 `InvalidOperationException`。组件已存在时复用，不重复添加，使安装器可安全执行两次。所有私有序列化字段通过 `SerializedObject`/`SerializedProperty` 写入，不使用运行时反射。

- [ ] **Step 4: 配置五个调查 Trigger**

在对应目标下创建以下子对象，全部 `BoxCollider2D.isTrigger = true`，连接唯一谜题控制器：

```text
Storage Cabinet Investigation -> StorageCabinet
Teacher Desk Investigation    -> TeacherDesk
Podium Investigation          -> Podium
Blackboard Investigation      -> Blackboard
Flowerpot Investigation       -> Flowerpot
```

前四个分别置于其父对象本地原点；黑板 Trigger 本地中心下移到玩家可接近的黑板下沿。尺寸分别贴合：储物柜 `(1.8, 2.4)`、教师桌 `(3.2, 1.5)`、讲台 `(1.5, 1.5)`、黑板 `(7.0, 1.4)`、花盆 `(1.6, 1.8)`。调查点使用 Trigger，不改变现有阻挡 Collider。

- [ ] **Step 5: 创建 Basic Unit 花盆组合并迁移现有借书卡**

在 `Teacher Area` 下创建 `Flowerpot`，世界位置 `(-3.55, 3.15, 0)`。全部 SpriteRenderer 复用 `Basic Unit` 的 Sprite：

```text
Pot Body: local (0, 0, 0), scale (0.72, 0.55, 1), color (0.42, 0.23, 0.10, 1), sortingOrder 2
Plant: local (0, 0.58, 0), scale (0.28, 0.72, 1), color (0.18, 0.48, 0.18, 1), sortingOrder 1
Shards: initially inactive
  Shard Left: local (-0.28, -0.12, 0), scale (0.38, 0.18, 1), z rotation 18°, brown, sortingOrder 2
  Shard Middle: local (0, -0.20, 0), scale (0.32, 0.16, 1), z rotation -8°, brown, sortingOrder 2
  Shard Right: local (0.30, -0.12, 0), scale (0.36, 0.18, 1), z rotation -20°, brown, sortingOrder 2
```

花盆根对象添加 `BreakableFlowerpot` 和专用 `BoxCollider2D` Trigger，hitbox size `(0.82, 1.35)`、offset `(0, 0.28)`。调查 Trigger 是独立子对象，不作为 hitbox。

将现有唯一 `Library Card` 移到世界位置 `(-3.55, 3.05, 0)`，sortingOrder 改为 3，保留 Sprite、颜色、缩放、`WorldItem` 和 Trigger；设 `activeSelf = false`。不得创建第二张卡。

- [ ] **Step 6: 配置 12 张椅子和 17 个停止障碍**

对 `Chair R1C1` 至 `Chair R4C3`：添加 `WorldItem(displayName = "Chair", canPickUp = false)`、贴合现有 SpriteRenderer 本地矩形的 `BoxCollider2D(isTrigger = true)`、`ThrowableChair(throwDuration = 0.3, throwDistance = 2, collisionSkin = 0.01)`。不改 Transform、SpriteRenderer、颜色和 sortingOrder，不添加 Rigidbody2D。

为 `Wall Top`、`Wall Bottom`、`Wall Left`、`Wall Right Upper`、`Wall Right Lower` 和 12 张 `Student Desk RnCn` 添加 `ChairThrowObstacle`。不标记 `Teacher Desk`、`Podium`、`Blackboard Surface`、`Storage Cabinet Body`、门、教授或主角。

- [ ] **Step 7: 配置谜题控制器及精确 15 条台词**

创建根对象 `Classroom Library Card Puzzle`，添加控制器，连接现有主角 `DialogueController`、教授 `BroadcastDialogueTrigger`、花盆、唯一借书卡和按 R1C1→R4C3 顺序排列的 12 张椅子。

九个页面数组精确配置：

```text
searchIntroPages[1]: 肯定不是放在别的同学的课桌。
storageCabinetPages[1]: 储物柜里没有。看来也没人把它随手塞进来。
teacherDeskPages[1]: 教授的桌子上也没有。最好别在这里乱翻。
podiumPages[1]: 讲台下面空空的。
blackboardPages[1]: 黑板附近也没有。总不可能夹在粉笔槽里吧。
firstFlowerpotPages[7]:
  花盆下面……好像压着什么。
  是我的借书卡。
  花盆纹丝不动。真够沉的。
  把这种东西藏在这里，很好玩吗？
  就因为我脸上的痕迹，他们就觉得可以随便欺负我。
  生气又能怎么样……我总不能在教室里和所有人打一架。
  得找个够结实的东西，把花盆砸开。
repeatFlowerpotPages[1]: 还是推不动。得找个东西把它砸开。
brokenFlowerpotPages[1]: 碎了。借书卡就在下面。
cardPickedUpPages[1]: 终于拿回来了。该去图书馆了。
```

每页 `speaker` 均为 `主角`。

- [ ] **Step 8: 执行安装器并移除一次性脚本**

Run:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\RPG project 1' -executeMethod ClassroomLibraryCardPuzzleInstaller.Install -logFile 'D:\RPG project 1\Logs\classroom-library-card-installer.log'
```

Expected: 退出码 0，日志无编译异常，`SampleScene.unity` 被保存。再次执行同一命令，Expected 仍为退出码 0 且场景组件计数不增加。随后用 `apply_patch` 删除仅由本任务创建的安装器 `.cs` 与 `.meta`，保留已保存场景。

- [ ] **Step 9: 运行场景 GREEN 验证**

运行 `ClassroomLibraryCardPuzzleSceneTests` 和 `verify_scene.ps1`。

Expected: Unity 场景测试全部通过；PowerShell 输出 `CLASSROOM_LIBRARY_CARD_PUZZLE_SCENE=PASS` 及固定计数。

- [ ] **Step 10: 跳过提交**

记录：`SKIP_COMMIT_NON_GIT_PROJECT`。

---

### Task 7: 完整回归、代码审查与 Play Mode 验收交付

**Files:**
- Verify: `Assets/Scripts/*.cs`
- Verify: `Assets/Tests/EditMode/*.cs`
- Verify: `Assets/Scenes/SampleScene.unity`
- Create: `work/classroom-library-card-puzzle/manual-playmode-checklist.md`

**Interfaces:**
- Consumes: Task 1–6 的最终实现和场景。
- Produces: 自动化验证记录、代码审查结论和可逐项勾选的人工验收清单。

- [ ] **Step 1: 运行全部 EditMode 测试**

Run:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\RPG project 1' -runTests -testPlatform EditMode -testResults 'D:\RPG project 1\Temp\classroom-library-card-all-editmode.xml' -logFile 'D:\RPG project 1\Logs\classroom-library-card-all-editmode.log'
```

Expected: 退出码 0，XML 中 failures 为 0；新增与既有 Dialogue、Inventory、Interaction、Door、Movement、MapTransition、HUD 测试全部通过。

- [ ] **Step 2: 运行场景与历史回归校验**

先运行 `work/classroom-library-card-puzzle/verify_scene.ps1`，再执行项目中现存的教室结构、碰撞、Basic Unit、物品栏 Canvas、教授外观、对话和门/走廊 PowerShell 校验脚本。若旧校验仍断言“椅子 Collider 数量为 0”或“Library Card 初始 active”，只更新该断言以识别本功能批准的 12 个 Trigger 和卡片初始隐藏；不得放宽墙体/课桌实体碰撞、椅子无阻挡或对象唯一性检查。

Expected: 全部脚本退出码 0；旧校验只包含可追溯到本规格的断言变化。

- [ ] **Step 3: 审查代码边界和状态安全**

逐项检查：

```text
DialogueController 回调先清旧状态再调用且只调用一次
BroadcastDialogueTrigger 仍只触发一次
PlayerInteraction 不读取调查台词，优先级为 door/investigation/item
WorldItem 普通丢弃行为未改变，飞行失败不丢失槽位引用
ThrowableChair 每帧扫掠且只识别标记障碍/专用花盆 hitbox
BreakableFlowerpot 只接受 IsFlying 椅子且破碎事件只发一次
ClassroomLibraryCardPuzzle 的状态只单向推进，重复拾卡不重复完成
新增组件缺失引用时记录错误，不通过名称字符串执行运行时判定
所有改动行均可追溯到已批准规格，无无关重构
```

发现问题时先新增或收紧复现测试，再做最小修正并重跑 Step 1–2。

- [ ] **Step 4: 写入人工 Play Mode 清单**

`manual-playmode-checklist.md` 必须逐项包含：原广播正常结束路径、原广播 `Space` 跳过路径、调查前输入锁定、四个普通点重复调查、首次/再次花盆台词、七页结束和跳过两种椅子解锁路径、三格满栏、12 张椅子任取、四向与斜向投掷、最大距离、墙/学生课桌/花盆命中、普通物品无效、花盆仅破碎一次、椅子保留与重新拾取、卡片手动拾取、完成台词、丢卡和再次拾卡不重复完成、门优先级回归、走廊往返不重播原广播。

- [ ] **Step 5: 人工 Play Mode 验收**

打开 Unity `SampleScene`，按清单逐项记录通过/失败。任何失败都先写最小复现测试；只有 Unity 视觉/输入时序无法由 EditMode 覆盖的项目允许只记录人工证据。

Expected: 全部清单通过；花盆 Basic Unit 组合视觉可辨识，卡片破碎前不可见、破碎后清晰可见，椅子始终不阻挡角色。

- [ ] **Step 6: 最终验证后跳过提交**

重新运行 Step 1 和 Step 2，保存最新测试结果和日志。记录：`SKIP_COMMIT_NON_GIT_PROJECT`。
