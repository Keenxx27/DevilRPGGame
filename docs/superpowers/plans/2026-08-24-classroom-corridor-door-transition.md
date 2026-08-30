# 教室门与学校走廊双向切换 Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 实现可用 `E` 平滑开关的教室门、独立学校走廊场景、完全开门后的双向自动传送，并在往返时保留全部当前运行状态。

**Architecture:** `DoorMotion` 负责可单测的门状态机，`DoorController` 将状态映射到 Transform 与 Collider2D。`PlayerInteraction` 成为唯一 `E` 键入口；`MapTransitionService` 以 Additive 方式按需加载走廊，通过 `MapSpawnPoint` ID 传送同一玩家与主摄像机。教室场景始终加载，走廊位于远离教室的世界坐标区域，因此现有物品引用和地图状态无需序列化。

**Tech Stack:** Unity 2022.3、C#、Unity 2D Physics、SceneManager Additive、NUnit EditMode、Unity YAML 场景

## Global Constraints

- `E` 在门的交互范围内只控制距离最近的门；无门时才拾取物品。
- 门必须平滑旋转 90°，动画完成前保持阻挡，完全开启后才允许通行。
- 走廊门必须能返回教室，两个方向均由穿过门外 Trigger 自动传送。
- `SampleScene` 始终加载；`SchoolCorridor` 只加载一次且不卸载。
- 玩家、主摄像机、Canvas、物品栏和 `WorldItem` 实例不得复制或重建。
- 走廊只使用 `Basic Unit` 占位美术，不增加美术包或第三方依赖。
- 项目不是 Git 仓库，不执行提交或分支集成步骤；修改前保存备份。

## Fixed Asset GUIDs

- `DoorMotion.cs.meta`: `7c4c9060f30a4f678e1f136202408001`
- `DoorController.cs.meta`: `7c4c9060f30a4f678e1f136202408002`
- `PlayerInteraction.cs.meta`: `7c4c9060f30a4f678e1f136202408003`
- `MapSpawnPoint.cs.meta`: `7c4c9060f30a4f678e1f136202408004`
- `MapTransitionService.cs.meta`: `7c4c9060f30a4f678e1f136202408005`
- `DoorExitPortal.cs.meta`: `7c4c9060f30a4f678e1f136202408006`
- `DoorMotionTests.cs.meta`: `7c4c9060f30a4f678e1f136202408101`
- `PlayerInteractionTests.cs.meta`: `7c4c9060f30a4f678e1f136202408102`
- `MapTransitionTests.cs.meta`: `7c4c9060f30a4f678e1f136202408103`
- `SchoolCorridor.unity.meta`: `7c4c9060f30a4f678e1f136202408201`

---

### Task 1: 门状态机与平滑动画

**Files:**
- Create: `Assets/Scripts/DoorMotion.cs`
- Create: `Assets/Scripts/DoorMotion.cs.meta`
- Create: `Assets/Scripts/DoorController.cs`
- Create: `Assets/Scripts/DoorController.cs.meta`
- Create: `Assets/Tests/EditMode/DoorMotionTests.cs`
- Create: `Assets/Tests/EditMode/DoorMotionTests.cs.meta`

**Interfaces:**
- Produces: `DoorState` 枚举；`DoorMotion(float closedAngle, float openAngle)`；`DoorMotion.Toggle()`；`DoorMotion.Step(float speed, float deltaTime)`；`CurrentAngle`、`State`、`IsFullyOpen`、`BlocksPassage`。
- Produces: `DoorController.ToggleDoor()`、`DoorController.IsFullyOpen`、`DoorController.State`。

- [ ] **Step 1: 写门状态机失败测试**

```csharp
[Test]
public void Opening_BlocksUntilAngleIsReached()
{
    var motion = new DoorMotion(0f, 90f);
    Assert.That(motion.Toggle(), Is.True);
    motion.Step(45f, 1f);
    Assert.That(motion.State, Is.EqualTo(DoorState.Opening));
    Assert.That(motion.BlocksPassage, Is.True);
    motion.Step(45f, 1f);
    Assert.That(motion.IsFullyOpen, Is.True);
    Assert.That(motion.BlocksPassage, Is.False);
}

[Test]
public void Closing_BlocksImmediatelyAndIgnoresToggleWhileMoving()
{
    var motion = new DoorMotion(0f, 90f);
    motion.Toggle();
    motion.Step(90f, 1f);
    Assert.That(motion.Toggle(), Is.True);
    Assert.That(motion.BlocksPassage, Is.True);
    Assert.That(motion.Toggle(), Is.False);
}
```

- [ ] **Step 2: 编译测试并确认 RED**

使用 Unity Roslyn 编译运行时与 EditMode 测试程序集。预期因 `DoorMotion`、`DoorState` 不存在而失败。

- [ ] **Step 3: 实现最小状态机和控制器**

`DoorMotion.Toggle()` 只允许 `Closed -> Opening` 和 `Open -> Closing`；`Step()` 使用 `Mathf.MoveTowardsAngle`，到达目标后切换到 `Open` 或 `Closed`。`DoorController.Update()` 调用 `Step(Time.deltaTime)`，将角度写入指定铰链 Transform，并同步门板 Collider：除 `Open` 状态外一律启用。

```csharp
public bool Toggle()
{
    if (State == DoorState.Closed) State = DoorState.Opening;
    else if (State == DoorState.Open) State = DoorState.Closing;
    else return false;
    return true;
}

public void Step(float speed, float deltaTime)
{
    float target = State == DoorState.Opening ? openAngle : closedAngle;
    CurrentAngle = Mathf.MoveTowardsAngle(CurrentAngle, target, speed * deltaTime);
    if (Mathf.Abs(Mathf.DeltaAngle(CurrentAngle, target)) > 0.001f) return;
    State = State == DoorState.Opening ? DoorState.Open : DoorState.Closed;
}
```

- [ ] **Step 4: 重新编译并运行门测试，确认 GREEN**

预期门测试全部通过，运行时与 EditMode 测试程序集无编译错误。

### Task 2: 统一 E 键与门优先级

**Files:**
- Create: `Assets/Scripts/PlayerInteraction.cs`
- Create: `Assets/Scripts/PlayerInteraction.cs.meta`
- Modify: `Assets/Scripts/PlayerInventory.cs`
- Create: `Assets/Tests/EditMode/PlayerInteractionTests.cs`
- Create: `Assets/Tests/EditMode/PlayerInteractionTests.cs.meta`
- Modify: `Assets/Tests/EditMode/InventoryInteractionTests.cs`

**Interfaces:**
- Consumes: `DoorController.ToggleDoor()`。
- Produces: `PlayerInventory.TryPickUpNearest(): bool`，不再由 `PlayerInventory.Update()` 读取 `KeyCode.E`。
- Produces: `PlayerInteraction.RegisterDoor(DoorController)`、`UnregisterDoor(DoorController)`、`TryInteract(): bool`。

- [ ] **Step 1: 写交互优先级失败测试**

```csharp
[Test]
public void DoorInRange_ConsumesInteractionBeforePickup()
{
    PlayerInteraction interaction = CreatePlayerWithInteraction(out PlayerInventory inventory);
    DoorController door = CreateConfiguredDoor(new Vector2(1f, 0f));
    WorldItem card = CreateItem(new Vector2(0.5f, 0f));
    inventory.RegisterNearby(card);
    interaction.RegisterDoor(door);

    Assert.That(interaction.TryInteract(), Is.True);
    Assert.That(door.State, Is.EqualTo(DoorState.Opening));
    Assert.That(card.gameObject.activeSelf, Is.True);
}

[Test]
public void NoDoorInRange_UsesExistingPickup()
{
    PlayerInteraction interaction = CreatePlayerWithInteraction(out PlayerInventory inventory);
    WorldItem card = CreateItem(Vector2.right);
    inventory.RegisterNearby(card);

    Assert.That(interaction.TryInteract(), Is.True);
    Assert.That(inventory.GetSlotItem(0), Is.SameAs(card));
}
```

- [ ] **Step 2: 编译并确认 RED**

预期因 `PlayerInteraction` 和公开返回值形式的 `TryPickUpNearest()` 不存在而失败。

- [ ] **Step 3: 实现唯一 E 键入口**

`PlayerInteraction.Update()` 只在 `Input.GetKeyDown(KeyCode.E)` 时调用 `TryInteract()`；清理空引用后选择与玩家距离平方最小的门。只要有门就调用 `ToggleDoor()` 并返回 `true`，即使门因动画中返回 `false` 也不执行拾取。`PlayerInventory.Update()` 删除 E 分支，`TryPickUpNearest()` 改为公开并返回是否成功拾取。

```csharp
public bool TryInteract()
{
    DoorController nearest = FindNearestDoor();
    if (nearest != null)
    {
        nearest.ToggleDoor();
        return true;
    }
    return inventory.TryPickUpNearest();
}
```

- [ ] **Step 4: 运行交互与现有物品栏测试，确认 GREEN**

预期门优先、无门拾取、满栏、丢弃和槽位选择测试全部通过。

### Task 3: 出生点、Portal 与 Additive 传送

**Files:**
- Create: `Assets/Scripts/MapSpawnPoint.cs`
- Create: `Assets/Scripts/MapSpawnPoint.cs.meta`
- Create: `Assets/Scripts/MapTransitionService.cs`
- Create: `Assets/Scripts/MapTransitionService.cs.meta`
- Create: `Assets/Scripts/DoorExitPortal.cs`
- Create: `Assets/Scripts/DoorExitPortal.cs.meta`
- Create: `Assets/Tests/EditMode/MapTransitionTests.cs`
- Create: `Assets/Tests/EditMode/MapTransitionTests.cs.meta`

**Interfaces:**
- Produces: `MapSpawnPoint.Id`、`CameraPosition`、静态 `TryFind(string, out MapSpawnPoint)`。
- Produces: `MapTransitionService.RequestTransition(string sceneName, string destinationId)`。
- Produces: `MapTransitionService.ApplyDestination(Rigidbody2D player, Camera camera, MapSpawnPoint destination)`。
- Consumes: `DoorController.IsFullyOpen`。

- [ ] **Step 1: 写传送规则失败测试**

```csharp
[Test]
public void ApplyDestination_MovesPlayerAndCameraAndClearsVelocity()
{
    Rigidbody2D player = CreatePlayerBody(new Vector2(1f, 2f), new Vector2(3f, 4f));
    Camera camera = CreateCamera();
    MapSpawnPoint spawn = CreateSpawn("CorridorFromClassroom", new Vector2(30f, 0f),
        new Vector3(30f, 0f, -10f));

    MapTransitionService.ApplyDestination(player, camera, spawn);

    Assert.That(player.position, Is.EqualTo(new Vector2(30f, 0f)));
    Assert.That(player.velocity, Is.EqualTo(Vector2.zero));
    Assert.That(camera.transform.position, Is.EqualTo(new Vector3(30f, 0f, -10f)));
}

[TestCase(false, true, false)]
[TestCase(true, false, false)]
[TestCase(true, true, true)]
public void PortalRequiresFullyOpenDoorAndPlayer(bool open, bool player, bool expected)
{
    Assert.That(DoorExitPortal.CanTransition(open, player), Is.EqualTo(expected));
}
```

- [ ] **Step 2: 编译并确认 RED**

预期因三个传送组件不存在而失败。

- [ ] **Step 3: 实现出生点注册与传送**

`MapSpawnPoint.OnEnable/OnDisable` 维护按 ID 查询的静态字典并拒绝重复 ID。`MapTransitionService` 在目的场景未加载时启动一次 `SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive)`，加载后查找出生点；找到玩家 Rigidbody2D 与 `Camera.main` 后调用 `ApplyDestination()`。任一步失败记录错误并释放切换锁。

```csharp
public static void ApplyDestination(Rigidbody2D player, Camera camera, MapSpawnPoint destination)
{
    player.position = destination.transform.position;
    player.velocity = Vector2.zero;
    camera.transform.position = destination.CameraPosition;
}
```

- [ ] **Step 4: 实现 Portal 门状态门禁**

`DoorExitPortal.OnTriggerEnter2D()` 仅接受带 `PlayerInteraction` 的碰撞体，且只有 `DoorController.IsFullyOpen` 时请求传送。公开纯函数 `CanTransition(bool doorFullyOpen, bool isPlayer)` 供规则测试。

- [ ] **Step 5: 运行传送测试，确认 GREEN**

预期目的位置、摄像机位置、速度清零和 Portal 门禁测试全部通过。

### Task 4: 教室门、走廊场景与 Build Settings

**Files:**
- Modify: `Assets/Scenes/SampleScene.unity`
- Create: `Assets/Scenes/SchoolCorridor.unity`
- Create: `Assets/Scenes/SchoolCorridor.unity.meta`
- Modify: `ProjectSettings/EditorBuildSettings.asset`
- Create: `work/door-corridor/verify_door_corridor_scenes.ps1`

**Interfaces:**
- Consumes: 六个新运行时组件的固定脚本 GUID。
- Produces: `CorridorFromClassroom` 与 `ClassroomFromCorridor` 两个唯一出生点 ID。

- [ ] **Step 1: 写场景结构失败校验**

校验脚本必须验证：新脚本及 `.meta` 存在；Main Character 挂载 `PlayerInteraction` 与 `MapTransitionService`；教室 Door 挂载 `DoorController` 和交互 Trigger；右墙被拆成上下两段并留下无碰撞门洞；教室 Portal 指向 `SchoolCorridor/CorridorFromClassroom`；走廊只有地面、墙、门、Portal、出生点和装饰门，不含第二个玩家、Main Camera 或 Canvas；两个场景均进入 Build Settings；全部 fileID 唯一。

- [ ] **Step 2: 运行场景校验并确认 RED**

Run: `powershell -NoProfile -ExecutionPolicy Bypass -File "work\door-corridor\verify_door_corridor_scenes.ps1"`

Expected: FAIL，报告脚本、走廊场景和门交互组件尚不存在。

- [ ] **Step 3: 备份并改造教室门洞**

保存 `SampleScene.before-door-corridor.unity` 和 `EditorBuildSettings.before-door-corridor.asset`。将完整 `Wall Right` 改为门洞上段，新增下段；把 `Door` 根对象移到铰链位置并调整 `Door Panel` 为铰链子偏移，保持门关闭时原有视觉位置。

- [ ] **Step 4: 写入教室交互组件**

Main Character 增加 `PlayerInteraction` 与 `MapTransitionService`。Door 根增加 `DoorController` 与半径约 1.6 的交互 Trigger。门外增加 `DoorExitPortal`，目的场景为 `SchoolCorridor`、目的 ID 为 `CorridorFromClassroom`。门内增加 `ClassroomFromCorridor` 出生点，玩家位置在教室侧，摄像机位置保持 `{x: 0, y: 0, z: -10}`。

- [ ] **Step 5: 搭建 SchoolCorridor**

在远离教室的世界坐标区域搭建约 16x6 的走廊，使用 `Basic Unit` 作为地面、上下左右墙体和门板。走廊对应门使用相同 DoorController/Trigger/Portal，目的 ID 为 `ClassroomFromCorridor`；出生点 `CorridorFromClassroom` 位于走廊侧，摄像机位置为走廊中心 `{z: -10}`。装饰门只有 SpriteRenderer，不挂交互组件。

- [ ] **Step 6: 更新 Build Settings 并确认 GREEN**

在 `SampleScene` 后加入启用的 `Assets/Scenes/SchoolCorridor.unity`。运行场景校验，预期 `DOOR_CORRIDOR_SCENE_VERIFICATION=PASS`。

### Task 5: 完整回归与交付

**Files:**
- Test: `Assets/Tests/EditMode/*.cs`
- Test: `work/door-corridor/verify_door_corridor_scenes.ps1`
- Test: existing `work/**/verify_*.ps1` and harnesses

**Interfaces:**
- Produces: 可复核的编译、逻辑测试、场景结构和改动范围证据。

- [ ] **Step 1: 重新编译运行时和 EditMode 测试程序集**

Unity Roslyn 必须返回退出码 0；当前 Unity 2022.3.62f3c1 的 `SceneManager` 已包含在 `UnityEngine.CoreModule.dll`，离线编译不引用不存在的独立 SceneManagement 模块。

- [ ] **Step 2: 运行所有新增逻辑测试和既有 harness**

门状态机、E 优先级、传送规则、HUD 生命周期、InventorySlots、InventoryInteraction、Movement 和 MovementPhysicsContract 必须全部通过。

- [ ] **Step 3: 运行全部场景校验**

新走廊校验、Canvas、物品栏、教室碰撞、Basic Unit 替换和教室结构校验必须全部通过；若既有校验因预期根对象或墙体数量发生有意变化，则只更新对应精确断言。

- [ ] **Step 4: 检查 Unity 进程与人工验证边界**

确认 Unity 编辑器未在后台运行。自动校验不宣称完成真实 Play Mode 视觉测试；交付时要求用户打开 Unity 验证平滑动画和双向穿门体验。
