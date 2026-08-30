# 学校图书馆地图实施计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 新建一张可进入、可探索并可返回学校走廊的简易学校图书馆地图。

**Architecture:** 沿用现有 additive 场景加载架构，不增加生产运行时代码。新增 `SchoolLibrary` 场景，以 Basic Unit 组合地图与家具；将走廊 `Decorative Door 1` 改造成标准 `DoorController`/`DoorExitPortal` 入口，并通过两个唯一的 `MapSpawnPoint` 完成双向传送。

**Tech Stack:** Unity 2022.3.62f3c1、C#、Unity 2D、NUnit EditMode Tests、Unity YAML 场景、现有 `DoorController`、`DoorExitPortal`、`MapTransitionService`、`MapSpawnPoint`。

## Global Constraints

- 新场景固定为 `Assets/Scenes/SchoolLibrary.unity`。
- 走廊入口固定使用当前 `Decorative Door 1`，改名为 `Library Door`。
- `Decorative Door 2` 和 `Decorative Door 3` 保持装饰状态。
- 图书馆约 18×12 世界单位，中心固定为 `(60, 0)`，相机位置为 `(60, 0, -10)`。
- 图书馆入口出生点 ID 固定为 `LibraryFromCorridor`。
- 走廊返回出生点 ID 固定为 `CorridorFromLibrary`。
- 图书馆只实现可进入、可探索、可返回；不添加 NPC、调查点、剧情、可拾取物或借阅功能。
- 图书馆不创建玩家、主相机、对话 UI 或物品栏。
- 所有临时视觉均复用项目 Basic Unit Sprite 和材质。
- 项目不是 Git 仓库；每个任务以测试报告作为检查点，不执行 Git 提交。

## File Structure

- Create: `Assets/Scenes/SchoolLibrary.unity` — 图书馆地图、家具、出口门、入口出生点。
- Modify: `Assets/Scenes/SchoolCorridor.unity` — 将第一扇装饰门改成图书馆入口并添加返回出生点。
- Modify: `ProjectSettings/EditorBuildSettings.asset` — 注册 `SchoolLibrary` 场景。
- Create: `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs` — 场景结构、家具碰撞、排除组件、双向传送和 Build Settings 验收。
- Create: `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs.meta` — 固定测试脚本 GUID。
- Temporary Create/Delete: `Assets/Editor/SchoolLibraryInstaller.cs` 与 `.meta` — 通过 Unity Editor API 可靠创建并保存场景；安装完成后删除，不进入最终项目。

---

### Task 1: 写入图书馆场景验收测试

**Files:**
- Create: `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs`
- Create: `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs.meta`

**Interfaces:**
- Consumes: `EditorSceneManager.OpenScene(string, OpenSceneMode)`、`EditorBuildSettings.scenes`、现有门和出生点组件。
- Produces: `SchoolLibrarySceneTests`，为后续场景安装提供固定对象名、坐标、组件和传送目标契约。

- [ ] **Step 1: 创建测试文件和固定场景契约**

测试类使用以下常量：

```csharp
private const string LibraryScenePath = "Assets/Scenes/SchoolLibrary.unity";
private const string CorridorScenePath = "Assets/Scenes/SchoolCorridor.unity";

private static readonly string[] RequiredLayoutObjects =
{
    "Library Floor",
    "Library Wall Top",
    "Library Wall Bottom Left",
    "Library Wall Bottom Right",
    "Library Wall Left",
    "Library Wall Right",
    "Circulation Counter Horizontal",
    "Circulation Counter Vertical",
    "Reading Table Left",
    "Reading Table Right",
    "Bookcase Top 1",
    "Bookcase Top 2",
    "Bookcase Top 3",
    "Bookcase Top 4",
    "Bookcase Left 1",
    "Bookcase Left 2",
    "Bookcase Right 1",
    "Bookcase Right 2"
};
```

添加三个测试：

```csharp
[Test]
public void BuildSettings_ContainsEnabledSchoolLibraryScene()
{
    Assert.That(EditorBuildSettings.scenes.Any(scene =>
        scene.enabled && scene.path == LibraryScenePath), Is.True);
}

[Test]
public void SchoolLibrary_ConfiguresCompactBasicUnitLayoutAndExcludesGameplaySystems()
{
    Scene scene = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
    foreach (string objectName in RequiredLayoutObjects)
    {
        Assert.That(Find(scene, objectName), Is.Not.Null, objectName);
    }

    AssertBlocking(scene, "Library Wall Top");
    AssertBlocking(scene, "Library Wall Bottom Left");
    AssertBlocking(scene, "Library Wall Bottom Right");
    AssertBlocking(scene, "Library Wall Left");
    AssertBlocking(scene, "Library Wall Right");
    AssertBlocking(scene, "Reading Table Left");
    AssertBlocking(scene, "Reading Table Right");
    AssertBlocking(scene, "Circulation Counter Horizontal");
    AssertBlocking(scene, "Circulation Counter Vertical");

    Assert.That(Object.FindObjectsOfType<WorldItem>(true), Is.Empty);
    Assert.That(Object.FindObjectsOfType<ThrowableChair>(true), Is.Empty);
    Assert.That(Object.FindObjectsOfType<FlowerpotBreakable>(true), Is.Empty);
    Assert.That(Object.FindObjectsOfType<ClassroomLibraryCardPuzzle>(true), Is.Empty);
    Assert.That(Object.FindObjectsOfType<PlayerInteraction>(true), Is.Empty);
    Assert.That(Object.FindObjectsOfType<Camera>(true), Is.Empty);
}

[Test]
public void CorridorAndLibrary_ConfigureMatchingBidirectionalDoorTransitions()
{
    Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
    AssertDoor(corridor, "Library Door", "SchoolLibrary", "LibraryFromCorridor");
    AssertSpawn(corridor, "CorridorFromLibrary",
        new Vector2(27f, 1.1f), new Vector3(29f, 0f, -10f));
    Assert.That(Find(corridor, "Decorative Door 2"), Is.Not.Null);
    Assert.That(Find(corridor, "Decorative Door 3"), Is.Not.Null);

    Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
    AssertDoor(library, "Library Exit Door", "SchoolCorridor", "CorridorFromLibrary");
    AssertSpawn(library, "LibraryFromCorridor",
        new Vector2(60f, -4.7f), new Vector3(60f, 0f, -10f));
}
```

辅助断言读取 `DoorExitPortal` 私有字段：

```csharp
private static void AssertDoor(
    Scene scene, string name, string destinationScene, string destinationId)
{
    GameObject root = Find(scene, name);
    Assert.That(root, Is.Not.Null, name);
    DoorController door = root.GetComponent<DoorController>();
    DoorExitPortal portal = root.GetComponentInChildren<DoorExitPortal>(true);
    Assert.That(door, Is.Not.Null, name);
    Assert.That(portal, Is.Not.Null, name);
    Assert.That(GetField<DoorController>(portal, "door"), Is.SameAs(door));
    Assert.That(GetField<string>(portal, "destinationScene"), Is.EqualTo(destinationScene));
    Assert.That(GetField<string>(portal, "destinationId"), Is.EqualTo(destinationId));
}
```

`AssertSpawn(Scene scene, string name, Vector2 expectedPosition, Vector3 expectedCameraPosition)` 必须通过反射读取 `MapSpawnPoint.cameraPosition`，同时检查出生点世界坐标和相机位置。

- [ ] **Step 2: 创建测试 `.meta`**

```yaml
fileFormatVersion: 2
guid: 7c4c9060f30a4f678e1f136202408119
timeCreated: 1787961600
```

- [ ] **Step 3: 运行测试并确认 RED**

Run:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -projectPath 'D:\RPG project 1' -runTests -testPlatform EditMode -testFilter 'RPG.Tests.SchoolLibrarySceneTests' -testResults 'D:\RPG project 1\Logs\school-library-red.xml' -logFile 'D:\RPG project 1\Logs\school-library-red.log'
```

Expected: FAIL；`SchoolLibrary.unity` 尚不存在、Build Settings 尚未注册该场景、走廊中也不存在 `Library Door`。

- [ ] **Step 4: 检查测试失败原因**

只接受与上述缺失场景/对象/配置相关的失败。若出现 C# 编译错误，先修复测试夹具；不得通过忽略日志或降低断言来制造 RED。

---

### Task 2: 创建 Basic Unit 图书馆布局

**Files:**
- Temporary Create: `Assets/Editor/SchoolLibraryInstaller.cs`
- Temporary Create: `Assets/Editor/SchoolLibraryInstaller.cs.meta`
- Create: `Assets/Scenes/SchoolLibrary.unity`
- Modify: `ProjectSettings/EditorBuildSettings.asset`

**Interfaces:**
- Consumes: Task 1 的固定对象名；`SchoolClassroom` 中名为 `Basic Unit` 的 `SpriteRenderer` 作为 Sprite/材质来源。
- Produces: `SchoolLibrary.unity` 的静态地图布局和 Build Settings 条目；Task 3 在同一安装器中追加双向门。

- [ ] **Step 1: 创建 Editor 安装器骨架**

```csharp
public static class SchoolLibraryInstaller
{
    private const string LibraryPath = "Assets/Scenes/SchoolLibrary.unity";
    private const string ClassroomPath = "Assets/Scenes/SchoolClassroom.unity";
    private static Sprite sourceSprite;
    private static Material sourceMaterial;

    public static void InstallLayout()
    {
        LoadBasicUnitSource();
        Scene library = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,
            NewSceneMode.Single);
        CreateLibraryLayout();
        EditorSceneManager.SaveScene(library, LibraryPath);
        AddLibraryToBuildSettings();
        AssetDatabase.SaveAssets();
    }
}
```

`LoadBasicUnitSource()` 必须打开 `SchoolClassroom`，递归查找 `Basic Unit`，保存它的 `sprite` 与 `sharedMaterial`，然后再新建空场景。

- [ ] **Step 2: 实现统一 Basic Unit 工厂**

```csharp
private static GameObject CreateUnit(
    string name, Vector2 position, Vector2 scale, Color color,
    int sortingOrder, bool blocking, Transform parent)
{
    GameObject item = new GameObject(name);
    item.transform.SetParent(parent, false);
    item.transform.position = position;
    item.transform.localScale = new Vector3(scale.x, scale.y, 1f);
    SpriteRenderer renderer = item.AddComponent<SpriteRenderer>();
    renderer.sprite = sourceSprite;
    renderer.sharedMaterial = sourceMaterial;
    renderer.color = color;
    renderer.sortingOrder = sortingOrder;
    if (blocking)
    {
        BoxCollider2D collider = item.AddComponent<BoxCollider2D>();
        collider.isTrigger = false;
    }
    return item;
}
```

固定颜色：

```csharp
Color floor = new Color(0.62f, 0.48f, 0.32f, 1f);
Color wall = new Color(0.82f, 0.76f, 0.62f, 1f);
Color darkWood = new Color(0.28f, 0.16f, 0.08f, 1f);
Color counter = new Color(0.38f, 0.22f, 0.11f, 1f);
Color chair = new Color(0.40f, 0.10f, 0.10f, 1f);
```

- [ ] **Step 3: 按固定坐标创建房间和家具**

创建根对象 `School Library`，所有布局对象放在其子层级：

| 对象 | 位置 | 缩放 | 阻挡 |
|---|---:|---:|---:|
| Library Floor | (60, 0) | (18, 12) | 否 |
| Library Wall Top | (60, 6) | (18, 0.5) | 是 |
| Library Wall Bottom Left | (55, -6) | (8, 0.5) | 是 |
| Library Wall Bottom Right | (65, -6) | (8, 0.5) | 是 |
| Library Wall Left | (51, 0) | (0.5, 12) | 是 |
| Library Wall Right | (69, 0) | (0.5, 12) | 是 |
| Circulation Counter Horizontal | (54.5, -3.4) | (4.2, 0.8) | 是 |
| Circulation Counter Vertical | (52.8, -2.2) | (0.8, 3.2) | 是 |
| Reading Table Left | (56.3, 1.0) | (3.2, 1.2) | 是 |
| Reading Table Right | (63.7, 1.0) | (3.2, 1.2) | 是 |

顶部书架：`Bookcase Top 1..4` 位于 `(53.3,5.2)`、`(56.7,5.2)`、`(63.3,5.2)`、`(66.7,5.2)`，缩放 `(3.0,0.7)`，均阻挡。

侧面书架：

- `Bookcase Left 1`：`(51.8, 2.7)`，`(0.7, 2.2)`。
- `Bookcase Left 2`：`(51.8, 0.0)`，`(0.7, 2.2)`。
- `Bookcase Right 1`：`(68.2, 2.7)`，`(0.7, 2.2)`。
- `Bookcase Right 2`：`(68.2, 0.0)`，`(0.7, 2.2)`。

阅读椅子固定为无碰撞 Basic Unit，不添加任何脚本：

- 左桌：`Reading Chair Left North` `(56.3,2.0)`、`Reading Chair Left South` `(56.3,0.0)`。
- 右桌：`Reading Chair Right North` `(63.7,2.0)`、`Reading Chair Right South` `(63.7,0.0)`。
- 每把椅子缩放 `(0.75,0.45)`，使用暗红色。

- [ ] **Step 4: 注册 Build Settings**

```csharp
private static void AddLibraryToBuildSettings()
{
    List<EditorBuildSettingsScene> scenes =
        EditorBuildSettings.scenes.ToList();
    if (!scenes.Any(scene => scene.path == LibraryPath))
    {
        scenes.Add(new EditorBuildSettingsScene(LibraryPath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }
}
```

- [ ] **Step 5: 执行布局安装**

Run:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\RPG project 1' -executeMethod SchoolLibraryInstaller.InstallLayout -logFile 'D:\RPG project 1\Logs\school-library-layout-installer.log'
```

Expected: Unity 退出码 0，创建 `SchoolLibrary.unity`，Build Settings 包含三个启用场景。

- [ ] **Step 6: 运行布局相关测试**

运行 Task 1 的测试过滤器。Expected: Build Settings 与布局测试 PASS；双向门测试仍 FAIL。

---

### Task 3: 装配走廊入口与图书馆出口

**Files:**
- Modify: `Assets/Editor/SchoolLibraryInstaller.cs`
- Modify: `Assets/Scenes/SchoolCorridor.unity`
- Modify: `Assets/Scenes/SchoolLibrary.unity`
- Delete after install: `Assets/Editor/SchoolLibraryInstaller.cs`
- Delete after install: `Assets/Editor/SchoolLibraryInstaller.cs.meta`

**Interfaces:**
- Consumes: `DoorController` 的 `rotatingTransform`/`blockingCollider` 字段；`DoorExitPortal` 的 `door`/`destinationScene`/`destinationId` 字段；`MapSpawnPoint` 的 `id`/`cameraPosition` 字段。
- Produces: `Library Door → SchoolLibrary/LibraryFromCorridor` 和 `Library Exit Door → SchoolCorridor/CorridorFromLibrary`。

- [ ] **Step 1: 实现可复用门装配函数**

门根对象包含交互 Trigger，门板包含阻挡 Collider，传送区域为独立子对象：

```csharp
private static DoorController ConfigureDoor(
    GameObject root, string rootName, Vector2 triggerSize,
    string destinationScene, string destinationId)
{
    root.name = rootName;
    BoxCollider2D interaction = root.GetComponent<BoxCollider2D>()
        ?? root.AddComponent<BoxCollider2D>();
    interaction.isTrigger = true;
    interaction.size = triggerSize;

    GameObject panel = CreateUnit(rootName + " Panel", root.transform.position,
        new Vector2(1.4f, 0.25f), new Color(0.35f, 0.18f, 0.08f, 1f),
        4, true, root.transform);
    panel.transform.localPosition = Vector3.zero;

    DoorController door = root.GetComponent<DoorController>()
        ?? root.AddComponent<DoorController>();
    SetSerializedReference(door, "rotatingTransform", panel.transform);
    SetSerializedReference(door, "blockingCollider",
        panel.GetComponent<BoxCollider2D>());

    GameObject portalObject = new GameObject(rootName + " Portal");
    portalObject.transform.SetParent(root.transform, false);
    BoxCollider2D portalCollider = portalObject.AddComponent<BoxCollider2D>();
    portalCollider.isTrigger = true;
    portalCollider.size = new Vector2(1.6f, 1.2f);
    DoorExitPortal portal = portalObject.AddComponent<DoorExitPortal>();
    SetSerializedReference(portal, "door", door);
    SetSerializedString(portal, "destinationScene", destinationScene);
    SetSerializedString(portal, "destinationId", destinationId);
    return door;
}
```

如果 `Decorative Door 1` 已带 SpriteRenderer，先保留其 Sprite/材质作为根装饰或禁用根 SpriteRenderer，不得同时显示旧门板和新门板。

- [ ] **Step 2: 改造走廊第一扇门**

在 `InstallTransitions()` 中打开 `SchoolCorridor`：

- 查找 `Decorative Door 1`，位置保持 `(27, 2.5)`。
- 改名为 `Library Door`。
- 配置目标 `SchoolLibrary` / `LibraryFromCorridor`。
- 门交互 Trigger 使用 `(2.2, 2.2)`。
- 门板围绕根对象旋转 90 度，速度使用 `DoorController` 默认 180 度/秒。
- Portal 放在门内侧，局部位置 `(0, 0.7)`。
- 创建 `CorridorFromLibrary` 于 `(27, 1.1)`，相机位置 `(29, 0, -10)`。
- 保存 `SchoolCorridor`。

`Decorative Door 2` 和 `Decorative Door 3` 不添加 `DoorController` 或 `DoorExitPortal`。

- [ ] **Step 3: 创建图书馆出口门和入口出生点**

打开 `SchoolLibrary`：

- 创建 `Library Exit Door` 于 `(60, -5.8)`。
- 配置目标 `SchoolCorridor` / `CorridorFromLibrary`。
- 门交互 Trigger 使用 `(2.2, 2.2)`。
- Portal 局部位置 `(0, -0.7)`。
- 创建 `LibraryFromCorridor` 于 `(60, -4.7)`，相机位置 `(60, 0, -10)`。
- 出生点与底部墙段、门板均不得重叠。
- 保存 `SchoolLibrary`。

出生点配置函数：

```csharp
private static MapSpawnPoint CreateSpawn(
    string name, Vector2 position, Vector3 cameraPosition)
{
    GameObject spawnObject = new GameObject(name);
    spawnObject.transform.position = position;
    MapSpawnPoint spawn = spawnObject.AddComponent<MapSpawnPoint>();
    SetSerializedString(spawn, "id", name);
    SetSerializedVector3(spawn, "cameraPosition", cameraPosition);
    return spawn;
}
```

- [ ] **Step 4: 执行双向切换安装**

Run:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\RPG project 1' -executeMethod SchoolLibraryInstaller.InstallTransitions -logFile 'D:\RPG project 1\Logs\school-library-transitions-installer.log'
```

Expected: Unity 退出码 0，两个场景保存成功。

- [ ] **Step 5: 删除临时安装器**

使用 `apply_patch` 删除：

- `Assets/Editor/SchoolLibraryInstaller.cs`
- `Assets/Editor/SchoolLibraryInstaller.cs.meta`

确认 `Assets/Editor` 中没有本任务临时文件残留。

- [ ] **Step 6: 运行图书馆测试并确认 GREEN**

Run Task 1 的测试命令。

Expected: `SchoolLibrarySceneTests` 全部 PASS；场景加载日志无 `MissingReferenceException`、`NullReferenceException` 或未预期错误。

---

### Task 4: 增强通道与排除范围回归保护

**Files:**
- Modify: `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs`

**Interfaces:**
- Consumes: Task 2/3 已保存的两个场景。
- Produces: 对中央通道、固定椅子和未改动装饰门的回归保护。

- [ ] **Step 1: 添加固定椅子断言**

```csharp
[Test]
public void SchoolLibrary_ChairsAreFixedNonBlockingDecoration()
{
    Scene scene = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
    string[] chairs =
    {
        "Reading Chair Left North", "Reading Chair Left South",
        "Reading Chair Right North", "Reading Chair Right South"
    };
    foreach (string chairName in chairs)
    {
        GameObject chair = Find(scene, chairName);
        Assert.That(chair, Is.Not.Null, chairName);
        Assert.That(chair.GetComponent<Collider2D>(), Is.Null, chairName);
        Assert.That(chair.GetComponent<WorldItem>(), Is.Null, chairName);
        Assert.That(chair.GetComponent<ThrowableChair>(), Is.Null, chairName);
    }
}
```

- [ ] **Step 2: 添加中央通道清空测试**

用 `Physics2D.OverlapBoxAll` 检查以下两条矩形区域没有非 Trigger 阻挡 Collider：

- 纵向通道：中心 `(60, -1.6)`，尺寸 `(2.0, 6.0)`。
- 横向通道：中心 `(60, 3.6)`，尺寸 `(13.0, 1.0)`。

测试必须忽略场景边界墙，但不得忽略书架、柜台和桌子的 Collider。若固定坐标与家具发生边缘接触，将通道尺寸缩小到不接触边缘的实际净空，不得删除家具碰撞。

- [ ] **Step 3: 添加装饰门未改动测试**

```csharp
[TestCase("Decorative Door 2")]
[TestCase("Decorative Door 3")]
public void Corridor_RemainingDecorativeDoorsStayNonInteractive(string doorName)
{
    Scene scene = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
    GameObject door = Find(scene, doorName);
    Assert.That(door, Is.Not.Null);
    Assert.That(door.GetComponent<DoorController>(), Is.Null);
    Assert.That(door.GetComponentInChildren<DoorExitPortal>(true), Is.Null);
}
```

- [ ] **Step 4: 运行图书馆测试**

Expected: 所有图书馆测试 PASS。失败时只调整实际错误的场景坐标或组件，不放宽“固定椅子无阻挡”和“装饰门不可交互”的要求。

---

### Task 5: 完整回归与交付审查

**Files:**
- Verify: `Assets/Scenes/SchoolLibrary.unity`
- Verify: `Assets/Scenes/SchoolCorridor.unity`
- Verify: `ProjectSettings/EditorBuildSettings.asset`
- Verify: `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs`

**Interfaces:**
- Consumes: 完整 Unity 项目。
- Produces: 最终自动化证据和只读代码/场景审查结论。

- [ ] **Step 1: 运行全部 EditMode 测试**

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -projectPath 'D:\RPG project 1' -runTests -testPlatform EditMode -testResults 'D:\RPG project 1\Logs\school-library-full-editmode.xml' -logFile 'D:\RPG project 1\Logs\school-library-full-editmode.log'
```

Expected: 所有测试 PASS，`failed=0`、`skipped=0`。

- [ ] **Step 2: 检查日志洁净度**

搜索最终日志：

```powershell
Select-String 'D:\RPG project 1\Logs\school-library-full-editmode.log' `
  -Pattern 'MissingReferenceException|NullReferenceException|error CS|executeMethod method.*threw'
```

Expected: 无匹配。

- [ ] **Step 3: 检查最终文件与临时文件**

确认：

- `SchoolLibrary.unity` 和 `.meta` 存在。
- Build Settings 中三个场景均启用。
- `Assets/Editor` 中没有 `SchoolLibraryInstaller` 残留。
- 图书馆中不存在玩家、相机、教室解谜组件或可拾取物。

- [ ] **Step 4: 请求独立只读审查**

审查重点：规格覆盖、双向目标 ID、出生点净空、装饰门未误改、Basic Unit 家具层级、碰撞与通道连通、测试是否掩盖日志。

只有在无 Critical/Important 问题且完整回归通过后才交付。
