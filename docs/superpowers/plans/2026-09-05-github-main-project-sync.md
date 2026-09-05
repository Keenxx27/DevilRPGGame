# GitHub 正式主项目同步实施计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 将已验收的学校图书馆功能迁入 `D:\Documents\GitHub\DevilRPGGame`，修复正式教室的主角动画层级，同时完整保留 GitHub 主项目现有美术、动画和资源 GUID。

**Architecture:** 以 GitHub `main` 当前提交为唯一基线，在独立 Git worktree 的 `codex/sync-school-library` 分支中执行。场景修改通过一次性 Unity Editor 安装器完成，图书馆场景按原 GUID 迁入，走廊只增加图书馆门和出生点；先以场景结构测试建立 RED，再实施最小修改达到 GREEN。

**Tech Stack:** Unity 2022.3.62f3c1、C#、Unity Editor Scene API、NUnit EditMode Tests、Git worktree、现有 `DoorController`、`DoorExitPortal`、`MapSpawnPoint`。

## Global Constraints

- 正式主项目固定为 `D:\Documents\GitHub\DevilRPGGame`；旧项目 `D:\RPG project 1` 只读使用。
- GitHub 主项目的 `Assets/Character`、`Assets/Pixel2D`、动画片段、Animator Controller、Sprite、材质、Prefab 及全部 `.meta` 不得覆盖、删除或重新生成。
- 图书馆继续使用已验收的 Basic Unit 占位布局；本计划不替换图书馆美术。
- 不迁移 `Library`、`Temp`、`Logs`、`UserSettings`、解决方案文件或旧项目的 `Packages`。
- 主目录当前未提交的 `Packages/packages-lock.json`、`ProjectSettings/PackageManagerSettings.asset`、`ProjectSettings/ProjectVersion.txt` 不得修改或丢失。工作树可以镜像这三个中国版 Unity 环境配置以运行测试，但不得暂存或提交它们。
- 不整体覆盖 GitHub 版 `SchoolClassroom.unity` 或 `SchoolCorridor.unity`。
- 一次性安装器在场景保存后必须删除，不进入最终交付。
- 完整回归必须达到 `failed=0`、`skipped=0`，最终日志不得包含未预期异常。
- 未经用户明确批准，不合并到 `main`，不推送远程仓库。
- 按用户偏好由主代理内联执行，不使用子代理。

## File Structure

- Modify: `Assets/Tests/EditMode/ClassroomInvestigationSceneTests.cs` — 主角动画视觉结构验收。
- Modify: `Assets/Scenes/SchoolClassroom.unity` — 在现有玩家根对象下接入纯动画视觉子对象。
- Create: `Assets/Scenes/SchoolLibrary.unity` and `.meta` — 已验收的 Basic Unit 图书馆场景。
- Modify: `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs` — 完整图书馆布局、通道、排除组件和双向门验收。
- Modify: `Assets/Scenes/SchoolCorridor.unity` — 只改造第一扇装饰门并增加返回出生点。
- Modify: `ProjectSettings/EditorBuildSettings.asset` — 追加图书馆场景。
- Temporary Modify/Delete: `Assets/Editor/SchoolLibraryInstaller.cs` and `.meta` — 精确写入角色视觉、走廊门和 Build Settings，随后删除。
- Create: `.superpowers/sdd/2026-09-05-github-main-project-sync/progress.md` — 记录每阶段测试证据。

---

### Task 1: 创建隔离工作树并记录 GitHub 基线

**Files:**
- Create: `D:\Documents\GitHub\DevilRPGGame-sync` — 独立 worktree。
- Create: `.superpowers/sdd/2026-09-05-github-main-project-sync/progress.md`
- Verify: 原主目录三个未提交配置文件。

**Interfaces:**
- Consumes: GitHub 主项目 `main` 中包含本计划文档的当前提交。
- Produces: 分支 `codex/sync-school-library`、本地未提交的中国版 Unity 环境配置和可重复使用的基线测试报告。

- [ ] **Step 1: 确认 Unity 已关闭且原主目录状态未变化**

Run:

```powershell
Get-Process Unity -ErrorAction SilentlyContinue
git -c safe.directory='D:/Documents/GitHub/DevilRPGGame' `
  -C 'D:\Documents\GitHub\DevilRPGGame' status --short --branch
```

Expected: 没有 Unity 进程；只显示已知三个未提交配置文件；`git log -1` 对应本计划文档提交。

- [ ] **Step 2: 创建隔离工作树**

Run:

```powershell
git -c safe.directory='D:/Documents/GitHub/DevilRPGGame' `
  -C 'D:\Documents\GitHub\DevilRPGGame' worktree add `
  -b codex/sync-school-library `
  'D:\Documents\GitHub\DevilRPGGame-sync' main
```

Expected: 新工作树位于指定路径并检出 `codex/sync-school-library`；原主目录仍在 `main` 且脏文件不变。

- [ ] **Step 3: 镜像本机 China Unity 环境配置**

Copy these three files from the original checkout to the same relative paths in the worktree:

```text
Packages/packages-lock.json
ProjectSettings/PackageManagerSettings.asset
ProjectSettings/ProjectVersion.txt
```

Expected: worktree `git status --short` lists exactly these three local modifications. Record their SHA256 hashes and never include them in any `git add` command.

- [ ] **Step 4: 运行完整 EditMode 基线**

Run:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' `
  -batchmode -projectPath 'D:\Documents\GitHub\DevilRPGGame-sync' `
  -runTests -testPlatform EditMode `
  -testResults 'D:\Documents\GitHub\DevilRPGGame-sync\Logs\sync-baseline.xml' `
  -logFile 'D:\Documents\GitHub\DevilRPGGame-sync\Logs\sync-baseline.log'
```

Expected: 测试运行完成。当前尚未实现的 `RPG.Tests.SchoolLibrarySceneTests` 可以作为已知 RED 失败；除此 fixture 之外若有任何失败，或出现任何 skipped case，记录具体用例并停止，不进入 Task 2。

- [ ] **Step 5: 建立进度记录**

Create `progress.md` with the exact baseline total/passed/failed/skipped counts, current commit, branch, and the three untouched dirty paths in the original checkout.

- [ ] **Step 6: 提交基线记录**

```powershell
git add .superpowers/sdd/2026-09-05-github-main-project-sync/progress.md
git commit -m "test: record main project sync baseline"
```

---

### Task 2: 修复正式教室的动画主角视觉层级

**Files:**
- Modify: `Assets/Tests/EditMode/ClassroomInvestigationSceneTests.cs`
- Modify: `Assets/Editor/SchoolLibraryInstaller.cs`
- Modify: `Assets/Scenes/SchoolClassroom.unity`

**Interfaces:**
- Consumes: `Assets/Scenes/Classroom_1.unity` 中 `Main Character/Player_Down_Run_01` 的 `Animator` 与 `SpriteRenderer` 配置。
- Produces: 正式 `SchoolClassroom` 中无额外 Collider 的动画视觉子对象；保留玩家根级运动和交互组件。

- [ ] **Step 1: 添加角色结构验收测试**

Add to `ClassroomInvestigationSceneTests`:

```csharp
[Test]
public void SchoolClassroom_ConfiguresAnimatedCharacterVisualWithoutDuplicatePhysics()
{
    Scene scene = OpenSampleScene();
    GameObject player = Find(scene, "Main Character");
    Animator animator = player.GetComponentInChildren<Animator>(true);
    SpriteRenderer renderer = player.GetComponentInChildren<SpriteRenderer>(true);

    Assert.That(animator, Is.Not.Null);
    Assert.That(animator.runtimeAnimatorController, Is.Not.Null);
    Assert.That(renderer, Is.Not.Null);
    Assert.That(animator.gameObject, Is.SameAs(renderer.gameObject));
    Assert.That(player.GetComponent<SpriteRenderer>(), Is.Null,
        "Root renderer would duplicate the animated visual.");
    Assert.That(animator.GetComponent<Collider2D>(), Is.Null,
        "Visual child must not change player physics.");
    Assert.That(player.GetComponent<MainCharacterMovement>(), Is.Not.Null);
    Assert.That(player.GetComponent<Rigidbody2D>(), Is.Not.Null);
}
```

- [ ] **Step 2: 运行新测试确认 RED**

Run the `RPG.Tests.ClassroomInvestigationSceneTests.SchoolClassroom_ConfiguresAnimatedCharacterVisualWithoutDuplicatePhysics` filter.

Expected: FAIL because `SchoolClassroom/Main Character` has no child `Animator` and still owns the static root `SpriteRenderer`.

- [ ] **Step 3: 在一次性安装器中加入角色迁移方法**

Add constants and an editor entry point to `SchoolLibraryInstaller`:

```csharp
private const string ReferenceClassroomScenePath = "Assets/Scenes/Classroom_1.unity";

public static void InstallAnimatedCharacterVisual()
{
    Scene targetScene = EditorSceneManager.OpenScene(
        ClassroomScenePath, OpenSceneMode.Single);
    GameObject targetPlayer = FindInScene(targetScene, "Main Character");

    Scene referenceScene = EditorSceneManager.OpenScene(
        ReferenceClassroomScenePath, OpenSceneMode.Additive);
    GameObject referencePlayer = FindInScene(referenceScene, "Main Character");
    Animator referenceAnimator = referencePlayer.GetComponentInChildren<Animator>(true);
    SpriteRenderer referenceRenderer = referenceAnimator == null
        ? null
        : referenceAnimator.GetComponent<SpriteRenderer>();

    if (referenceAnimator == null || referenceRenderer == null ||
        referenceAnimator.runtimeAnimatorController == null)
    {
        throw new InvalidOperationException(
            "Classroom_1 animated character visual is incomplete.");
    }

    GameObject visual = new GameObject(referenceAnimator.gameObject.name);
    visual.transform.SetParent(targetPlayer.transform, false);
    visual.transform.localPosition = referenceAnimator.transform.localPosition;
    visual.transform.localRotation = referenceAnimator.transform.localRotation;
    visual.transform.localScale = referenceAnimator.transform.localScale;

    SpriteRenderer renderer = visual.AddComponent<SpriteRenderer>();
    EditorUtility.CopySerialized(referenceRenderer, renderer);
    Animator animator = visual.AddComponent<Animator>();
    EditorUtility.CopySerialized(referenceAnimator, animator);

    SpriteRenderer oldRenderer = targetPlayer.GetComponent<SpriteRenderer>();
    if (oldRenderer != null)
    {
        UnityEngine.Object.DestroyImmediate(oldRenderer);
    }

    EditorSceneManager.CloseScene(referenceScene, true);
    EditorSceneManager.MarkSceneDirty(targetScene);
    EditorSceneManager.SaveScene(targetScene);
    AssetDatabase.SaveAssets();
}
```

Also add this recursive lookup. Do not copy the reference child `BoxCollider2D`.

```csharp
private static GameObject FindInScene(Scene scene, string objectName)
{
    foreach (GameObject root in scene.GetRootGameObjects())
    {
        foreach (Transform item in root.GetComponentsInChildren<Transform>(true))
        {
            if (item.name == objectName)
            {
                return item.gameObject;
            }
        }
    }

    throw new InvalidOperationException(
        scene.path + " is missing required object " + objectName + ".");
}
```

- [ ] **Step 4: 运行角色安装方法**

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' `
  -batchmode -quit `
  -projectPath 'D:\Documents\GitHub\DevilRPGGame-sync' `
  -executeMethod SchoolLibraryInstaller.InstallAnimatedCharacterVisual `
  -logFile 'D:\Documents\GitHub\DevilRPGGame-sync\Logs\sync-character-install.log'
```

Expected: Unity exit code 0 and only `SchoolClassroom.unity` changes among production assets.

- [ ] **Step 5: 运行角色测试确认 GREEN**

Run the whole `RPG.Tests.ClassroomInvestigationSceneTests` fixture.

Expected: all fixture tests pass; log contains neither the original Animator/SpriteRenderer error nor missing-reference/compile errors.

- [ ] **Step 6: 提交角色修复**

```powershell
git add Assets/Tests/EditMode/ClassroomInvestigationSceneTests.cs `
  Assets/Scenes/SchoolClassroom.unity
git commit -m "fix: connect animated player visual in classroom"
```

Do not stage the temporary installer yet.

---

### Task 3: 建立完整图书馆验收 RED

**Files:**
- Modify: `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs`

**Interfaces:**
- Consumes: 已批准规格中的布局、中央通道、排除组件、门和出生点契约。
- Produces: 7 个 NUnit case，能分别捕获缺失图书馆、错误碰撞、阻塞通道和装饰门误改。

- [ ] **Step 1: 扩展阻挡与排除断言**

Append all eight bookcase names to `blockingNames`. Add these component exclusions after `PlayerInteraction`:

```csharp
AssertNoComponents<PlayerInventory>(scene);
AssertNoComponents<MainCharacterMovement>(scene);
AssertNoComponents<DialogueHUD>(scene);
AssertNoComponents<InventoryHUD>(scene);
```

- [ ] **Step 2: 添加椅子、通道和装饰门测试**

Add the verified tests from `D:\RPG project 1\Assets\Tests\EditMode\SchoolLibrarySceneTests.cs`:

```csharp
[Test]
public void SchoolLibrary_ChairsAreFixedNonBlockingDecoration()
{
    Scene scene = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
    string[] names =
    {
        "Reading Chair Left North", "Reading Chair Left South",
        "Reading Chair Right North", "Reading Chair Right South"
    };
    foreach (string name in names)
    {
        GameObject chair = Find(scene, name);
        Assert.That(chair, Is.Not.Null, name);
        Assert.That(chair.GetComponent<Collider2D>(), Is.Null, name);
        Assert.That(chair.GetComponent<WorldItem>(), Is.Null, name);
        Assert.That(chair.GetComponent<ThrowableChair>(), Is.Null, name);
    }
}

[Test]
public void SchoolLibrary_CentralPassagesRemainClear()
{
    Scene scene = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
    Physics2D.SyncTransforms();
    AssertPassageClear(scene, new Vector2(60f, -1.6f), new Vector2(2f, 6f),
        "vertical passage");
    AssertPassageClear(scene, new Vector2(60f, 3.6f), new Vector2(13f, 1f),
        "horizontal passage");
}

[TestCase("Decorative Door 2")]
[TestCase("Decorative Door 3")]
public void Corridor_RemainingDecorativeDoorsStayNonInteractive(string name)
{
    Scene scene = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
    GameObject door = Find(scene, name);
    Assert.That(door, Is.Not.Null, name);
    Assert.That(door.GetComponent<DoorController>(), Is.Null, name);
    Assert.That(door.GetComponentInChildren<DoorExitPortal>(true), Is.Null, name);
}
```

Add this helper, filtering to the loaded scene, ignoring triggers and boundary names beginning with `Library Wall `:

```csharp
private static void AssertPassageClear(
    Scene scene, Vector2 center, Vector2 size, string passageName)
{
    string[] blockers = Physics2D.OverlapBoxAll(center, size, 0f)
        .Where(collider => collider.gameObject.scene == scene
            && !collider.isTrigger
            && !collider.name.StartsWith("Library Wall "))
        .Select(collider => collider.name)
        .ToArray();

    Assert.That(blockers, Is.Empty,
        passageName + " blocked by: " + string.Join(", ", blockers));
}
```

- [ ] **Step 3: 加强出口位置断言**

After asserting `Library Exit Door`, add:

```csharp
Assert.That(Find(library, "Library Exit Door").transform.position,
    Is.EqualTo(new Vector3(60f, -5.8f, 0f)), "Library Exit Door");
```

- [ ] **Step 4: 运行图书馆测试确认 RED**

Run filter `RPG.Tests.SchoolLibrarySceneTests`.

Expected: failures are limited to absent `SchoolLibrary.unity`, absent enabled Build Settings entry, absent `Library Door`, and related dependent layout/transition assertions. Compilation errors or unrelated failures must be fixed in the test fixture before proceeding.

- [ ] **Step 5: 提交验收测试**

```powershell
git add Assets/Tests/EditMode/SchoolLibrarySceneTests.cs
git commit -m "test: expand school library acceptance coverage"
```

---

### Task 4: 精确迁移图书馆场景与双向走廊入口

**Files:**
- Create: `Assets/Scenes/SchoolLibrary.unity`
- Create: `Assets/Scenes/SchoolLibrary.unity.meta`
- Modify: `Assets/Editor/SchoolLibraryInstaller.cs`
- Modify: `Assets/Scenes/SchoolCorridor.unity`
- Modify: `ProjectSettings/EditorBuildSettings.asset`

**Interfaces:**
- Consumes: 旧项目中已通过 7/7 验收的图书馆场景；主项目当前 `Decorative Door 1`；现有门与出生点脚本的序列化字段。
- Produces: `Library Door -> SchoolLibrary/LibraryFromCorridor`、`Library Exit Door -> SchoolCorridor/CorridorFromLibrary` 和三个启用的 Build Settings 场景。

- [ ] **Step 1: 迁入图书馆场景及原始 GUID**

Mechanically copy only:

```text
D:\RPG project 1\Assets\Scenes\SchoolLibrary.unity
D:\RPG project 1\Assets\Scenes\SchoolLibrary.unity.meta
```

to the worktree's `Assets/Scenes`. Verify the `.meta` GUID is unchanged and every referenced GUID resolves under the GitHub main project's `Assets` or `Packages`.

- [ ] **Step 2: 在安装器中加入走廊转换装配**

Add `InstallTransitions()` that opens `Assets/Scenes/SchoolCorridor.unity`, finds `Decorative Door 1`, renames it `Library Door`, leaves its existing root `SpriteRenderer` and art reference intact, and adds/configures only these components and objects:

```csharp
public static void InstallTransitions()
{
    const string corridorPath = "Assets/Scenes/SchoolCorridor.unity";
    Scene corridor = EditorSceneManager.OpenScene(corridorPath, OpenSceneMode.Single);
    GameObject root = FindInScene(corridor, "Decorative Door 1");
    root.name = "Library Door";

    BoxCollider2D interaction = root.GetComponent<BoxCollider2D>()
        ?? root.AddComponent<BoxCollider2D>();
    interaction.isTrigger = true;
    interaction.size = new Vector2(2.2f, 2.2f);

    GameObject panel = CreateDoorPanel(root.transform, "Library Door Panel");
    DoorController door = root.GetComponent<DoorController>()
        ?? root.AddComponent<DoorController>();
    SetObjectReference(door, "rotatingTransform", panel.transform);
    SetObjectReference(door, "blockingCollider",
        panel.GetComponent<BoxCollider2D>());

    GameObject portalObject = new GameObject("Library Door Portal");
    portalObject.transform.SetParent(root.transform, false);
    portalObject.transform.localPosition = new Vector3(0f, 0.7f, 0f);
    BoxCollider2D portalCollider = portalObject.AddComponent<BoxCollider2D>();
    portalCollider.isTrigger = true;
    portalCollider.size = new Vector2(1.6f, 1.2f);
    DoorExitPortal portal = portalObject.AddComponent<DoorExitPortal>();
    SetObjectReference(portal, "door", door);
    SetString(portal, "destinationScene", "SchoolLibrary");
    SetString(portal, "destinationId", "LibraryFromCorridor");

    CreateSpawn("CorridorFromLibrary", new Vector2(27f, 1.1f),
        new Vector3(29f, 0f, -10f));
    EditorSceneManager.SaveScene(corridor);
    AddLibraryToBuildSettings();
    AssetDatabase.SaveAssets();
}
```

Use these helpers. They preserve the root renderer component, create no visual-child collider beyond the intended door panel, and fail immediately when a serialized field changes name:

```csharp
private static GameObject CreateDoorPanel(Transform root, string name)
{
    SpriteRenderer source = root.GetComponent<SpriteRenderer>();
    if (source == null || source.sprite == null)
    {
        throw new InvalidOperationException(root.name + " needs a SpriteRenderer.");
    }

    GameObject panel = new GameObject(name);
    panel.transform.SetParent(root, false);
    panel.transform.localPosition = Vector3.zero;
    panel.transform.localScale = new Vector3(1.4f, 0.25f, 1f);
    SpriteRenderer renderer = panel.AddComponent<SpriteRenderer>();
    renderer.sprite = source.sprite;
    renderer.sharedMaterial = source.sharedMaterial;
    renderer.color = new Color(0.35f, 0.18f, 0.08f, 1f);
    renderer.sortingOrder = 4;
    BoxCollider2D collider = panel.AddComponent<BoxCollider2D>();
    collider.isTrigger = false;
    source.enabled = false;
    return panel;
}

private static void CreateSpawn(string id, Vector2 position, Vector3 cameraPosition)
{
    GameObject item = new GameObject(id);
    item.transform.position = position;
    MapSpawnPoint spawn = item.AddComponent<MapSpawnPoint>();
    SetString(spawn, "id", id);
    SetVector3(spawn, "cameraPosition", cameraPosition);
}

private static void SetObjectReference(
    UnityEngine.Object target, string field, UnityEngine.Object value)
{
    SerializedObject serialized = new SerializedObject(target);
    SerializedProperty property = serialized.FindProperty(field);
    if (property == null) throw new InvalidOperationException(field);
    property.objectReferenceValue = value;
    serialized.ApplyModifiedPropertiesWithoutUndo();
}

private static void SetString(UnityEngine.Object target, string field, string value)
{
    SerializedObject serialized = new SerializedObject(target);
    SerializedProperty property = serialized.FindProperty(field);
    if (property == null) throw new InvalidOperationException(field);
    property.stringValue = value;
    serialized.ApplyModifiedPropertiesWithoutUndo();
}

private static void SetVector3(UnityEngine.Object target, string field, Vector3 value)
{
    SerializedObject serialized = new SerializedObject(target);
    SerializedProperty property = serialized.FindProperty(field);
    if (property == null) throw new InvalidOperationException(field);
    property.vector3Value = value;
    serialized.ApplyModifiedPropertiesWithoutUndo();
}
```

- [ ] **Step 3: 运行转换安装方法**

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' `
  -batchmode -quit `
  -projectPath 'D:\Documents\GitHub\DevilRPGGame-sync' `
  -executeMethod SchoolLibraryInstaller.InstallTransitions `
  -logFile 'D:\Documents\GitHub\DevilRPGGame-sync\Logs\sync-library-install.log'
```

Expected: Unity exit code 0; changes are limited to the copied library scene, `SchoolCorridor.unity`, and `EditorBuildSettings.asset`. `Decorative Door 2` and `Decorative Door 3` remain noninteractive.

- [ ] **Step 4: 运行图书馆测试确认 GREEN**

Run filter `RPG.Tests.SchoolLibrarySceneTests`.

Expected: 7/7 cases pass, 0 failed, 0 skipped. Scan the log for `MissingReferenceException|NullReferenceException|error CS|Failed to resolve packages|executeMethod method.*threw`; expected no matches.

- [ ] **Step 5: 删除一次性安装器**

Delete with `apply_patch`:

```text
Assets/Editor/SchoolLibraryInstaller.cs
Assets/Editor/SchoolLibraryInstaller.cs.meta
```

Re-run the 7 library cases after Unity recompiles; expected 7/7 pass.

- [ ] **Step 6: 提交图书馆迁移**

```powershell
git add Assets/Scenes/SchoolLibrary.unity `
  Assets/Scenes/SchoolLibrary.unity.meta `
  Assets/Scenes/SchoolCorridor.unity `
  ProjectSettings/EditorBuildSettings.asset `
  Assets/Editor/SchoolLibraryInstaller.cs `
  Assets/Editor/SchoolLibraryInstaller.cs.meta
git commit -m "feat: migrate school library into main project"
```

The two installer paths are staged as deletions because they were tracked in the GitHub baseline.

---

### Task 5: 完整回归、Play Mode 冒烟与资产保全审计

**Files:**
- Verify: all files changed on `codex/sync-school-library` since `main`.
- Modify: `.superpowers/sdd/2026-09-05-github-main-project-sync/progress.md`

**Interfaces:**
- Consumes: Tasks 1–4 completed branch.
- Produces: 可审查的最终差异、测试报告和不触碰原主目录脏文件的证明。

- [ ] **Step 1: 运行完整 EditMode 回归**

Run all EditMode tests into `Logs/sync-full-editmode.xml` and `Logs/sync-full-editmode.log`.

Expected: result `Passed`, `failed=0`, `skipped=0`; log has no original character error, missing references, null references, compiler errors, package-resolution failures, or execute-method exceptions.

- [ ] **Step 2: 执行正式场景 Play Mode 冒烟**

Open only the worktree project in Unity 2022.3.62f3c1. Open `Assets/Scenes/SchoolClassroom.unity`, enter Play Mode, and verify:

1. Console does not emit `Main Character 的子对象上需要 Animator 和 SpriteRenderer。`。
2. 主角可用四向输入移动；静止、上下、左右移动动画切换，向左时水平翻转。
3. 调查、对话和物品栏仍可使用。
4. 通过教室门进入走廊，再通过 `Library Door` 进入图书馆。
5. 图书馆中主角可沿中央通道移动，并能通过 `Library Exit Door` 返回走廊正确出生点。

Any failure blocks delivery and must be recorded with the Console stack trace.

- [ ] **Step 3: 审计美术和非目标文件**

Run:

```powershell
git diff --name-status main...codex/sync-school-library
git diff --exit-code main...codex/sync-school-library -- `
  Assets/Character Assets/Pixel2D Packages `
  ProjectSettings/PackageManagerSettings.asset `
  ProjectSettings/ProjectVersion.txt
```

Expected: the first command lists only the planned tests/scenes/Build Settings/progress changes and installer deletions; the second exits 0. Also run `git diff --exit-code -- Assets/Character Assets/Pixel2D` and verify no `.meta` under the protected art directories changed. The three worktree-only China Unity environment files may remain modified but uncommitted.

- [ ] **Step 4: 确认原主目录脏文件保持原样**

Run `git status --short` in `D:\Documents\GitHub\DevilRPGGame`.

Expected: the same three pre-existing modified paths remain and no synchronization file appears in that checkout before merge approval.

- [ ] **Step 5: 写入最终证据并提交**

Update `progress.md` with target/full test counts, log scan result, Play Mode checklist, protected-path audit, final commit range, and any non-blocking observations.

```powershell
git add .superpowers/sdd/2026-09-05-github-main-project-sync/progress.md
git commit -m "test: record main project sync verification"
```

- [ ] **Step 6: 停止在功能分支等待合并批准**

Show `git log --oneline main..codex/sync-school-library`, `git diff --stat main...codex/sync-school-library`, the final test report, and original checkout status. Do not merge or push until the user explicitly approves.
