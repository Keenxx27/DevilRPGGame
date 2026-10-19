# 教授角色美术接入实施计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 为 `SchoolClassroom` 中的教授接入一套深色西装、灰白短发的四向站立与行走像素角色表，并不改变既有课堂剧情引用。

**Architecture:** 将教授图像导入为固定的 4×3 多精灵表；编辑器安装器生成八个动画片段和一个专属 Animator Controller。教授根对象继续承载既有广播和移动逻辑，新增子对象承载可见 SpriteRenderer、Animator 与一个只根据根对象位移选择方向的轻量视觉动画组件。

**Tech Stack:** Unity 2022.3.62f1、C#、UnityEditor Animation API、NUnit EditMode 测试、PNG 像素精灵。

## Global Constraints

- 素材必须是透明背景的 2D 俯视 RPG 硬像素风，192×192 像素，4 列×3 行、每格 48×64 像素。
- 四方向均独立绘制；四个行走方向各有两帧，不能使用镜像代替左/右朝向。
- 教授外观固定为无胡须的灰白短发、细框眼镜、炭灰西装、白衬衫、暗红领带、深灰西裤和黑色皮鞋。
- 教授视觉使用 `Vector3.one * 1.25f` 缩放，高度略高于林健太，并使用同一套大头紧凑构图、32 PPU、Point 过滤与无压缩导入配置。
- 只替换 `Professor` 的可见表现；不得破坏既有课堂广播、移动、位置、父级、排序引用或交互逻辑。
- 不修改 `Assets/Scenes Copy`；不重置或删除任何现有工作区改动。

---

## 文件结构

- 创建：`Assets/Art/Characters/professor-character-sheet-pixel-v1.png` — 最终透明角色表。
- 创建：`Assets/Scripts/ProfessorVisualAnimator.cs` — 根据教授根对象位移选择八种动画状态。
- 创建：`Assets/Editor/ProfessorCharacterInstaller.cs` — 精灵导入、动画片段、控制器和场景视觉子对象的幂等安装器。
- 创建：`Assets/Character/Professor.controller` — 教授动画控制器。
- 创建：`Assets/Character/Professor_*.anim` — 四个站立及四个行走动画片段。
- 创建：`Assets/Tests/EditMode/ProfessorVisualAnimatorTests.cs` — 状态选择单元测试。
- 创建：`Assets/Tests/EditMode/ProfessorCharacterSceneTests.cs` — 教室场景接入回归测试。
- 修改：`Assets/Scenes/SchoolClassroom.unity` — 仅由安装器新增/更新 `Professor Visual` 子对象并停用旧 Renderer。

### Task 1: 生成并规范化教授角色表

**Files:**
- Create: `Assets/Art/Characters/professor-character-sheet-pixel-v1.png`
- Modify: `Assets/Art/Characters/professor-character-sheet-pixel-v1.png.meta`（由 Unity 导入器生成）

**Interfaces:**
- Produces: 12 个 48×64 单元格，供 `ProfessorCharacterInstaller.Frames` 命名导入。

- [ ] **Step 1: 生成透明背景预览图**

使用图像生成工具创建一个 4 列×3 行角色表：首行依次为下、上、右、左的站立姿态；第二、三行按下、上、右、左方向分别放置步行 A/B 帧。明确要求灰白后梳短发、细框眼镜、炭灰西装、白衬衫、暗红领带、无胡须、硬像素与透明背景。

- [ ] **Step 2: 人工确认外观与方向**

检查每格：左/右不应镜像；衣摆和领带只在行走帧轻摆；教授高度略高于林健太；背景 Alpha 为透明。将用户确认的版本放入目标路径。

- [ ] **Step 3: 规范化图像尺寸与像素**

将确认图裁切/缩放为 192×192，使用最近邻缩放；每个单元格严格为 48×64，像素边缘无半透明背景光晕。

- [ ] **Step 4: 在 Unity 中检查导入结果**

确认 `.meta` 导入为 Multiple、32 PPU、Point、Uncompressed、Alpha transparency，且能被下一任务切出 12 张精灵。

### Task 2: 为教授视觉动画建立可测试的最小运行时组件

**Files:**
- Create: `Assets/Tests/EditMode/ProfessorVisualAnimatorTests.cs`
- Create: `Assets/Scripts/ProfessorVisualAnimator.cs`

**Interfaces:**
- Produces: `ProfessorVisualAnimator.ResolveAnimation(Vector2 direction, bool isMoving): CharacterAnimationState`。
- Consumes: 现有 `RPG.CharacterAnimationState`、Unity `Animator` 和 `SpriteRenderer`。

- [ ] **Step 1: 写出失败的方向选择测试**

```csharp
[TestCase(0f, -1f, false, "Professor_Idle_Down")]
[TestCase(0f, 1f, false, "Professor_Idle_Up")]
[TestCase(1f, 0f, true, "Professor_Right_Run")]
[TestCase(-1f, 0f, true, "Professor_Left_Run")]
public void ResolveAnimation_UsesIndependentDirectionalStates(
    float x, float y, bool isMoving, string expectedState)
{
    CharacterAnimationState state = ProfessorVisualAnimator.ResolveAnimation(
        new Vector2(x, y), isMoving);

    Assert.That(state.Name, Is.EqualTo(expectedState));
    Assert.That(state.FlipX, Is.False);
}
```

- [ ] **Step 2: 运行 EditMode 测试并确认 RED**

运行 `ProfessorVisualAnimatorTests`。预期因 `ProfessorVisualAnimator` 尚不存在而编译/测试失败。

- [ ] **Step 3: 编写最小动画状态组件**

```csharp
[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public sealed class ProfessorVisualAnimator : MonoBehaviour
{
    public static CharacterAnimationState ResolveAnimation(Vector2 direction, bool isMoving)
    {
        // 以横向优先判断四向；返回 Professor_Idle_* 或 Professor_*_Run。
    }

    private void LateUpdate()
    {
        // 比较根对象所在世界位置的相邻帧差值；移动时更新朝向，
        // 仅在状态 hash 改变时 Animator.Play，并始终保持 SpriteRenderer.flipX 为 false。
    }
}
```

- [ ] **Step 4: 运行目标测试并确认 GREEN**

运行 `ProfessorVisualAnimatorTests`，预期四个方向、站立/行走状态和 `FlipX == false` 均通过。

### Task 3: 建立编辑器安装器、动画资源和教室接入

**Files:**
- Create: `Assets/Editor/ProfessorCharacterInstaller.cs`
- Create: `Assets/Character/Professor.controller`
- Create: `Assets/Character/Professor_Idle_Down.anim`
- Create: `Assets/Character/Professor_Idle_Up.anim`
- Create: `Assets/Character/Professor_Idle_Right.anim`
- Create: `Assets/Character/Professor_Idle_Left.anim`
- Create: `Assets/Character/Professor_Down_Run.anim`
- Create: `Assets/Character/Professor_Up_Run.anim`
- Create: `Assets/Character/Professor_Right_Run.anim`
- Create: `Assets/Character/Professor_Left_Run.anim`
- Modify: `Assets/Scenes/SchoolClassroom.unity`

**Interfaces:**
- Consumes: `ProfessorVisualAnimator`、Task 1 的 PNG 和教授现有根对象 `Professor`。
- Produces: 可从 Unity 菜单执行的 `ProfessorCharacterInstaller.Apply()`，并创建 `Professor Visual` 子对象。

- [ ] **Step 1: 实现精灵表命名与导入**

在 `ProfessorCharacterInstaller` 中声明：

```csharp
private const string ArtworkPath =
    "Assets/Art/Characters/professor-character-sheet-pixel-v1.png";
private const string ControllerPath = "Assets/Character/Professor.controller";
private const string ClassroomScenePath = "Assets/Scenes/SchoolClassroom.unity";
```

按列定义 `Professor_Idle_Down/Walk_Down_A/Walk_Down_B`、`Up`、`Right`、`Left`，每个 Rect 为 `new Rect(column * 48, (2 - row) * 64, 48, 64)`，pivot 为 `(0.5f, 0f)`。设置 Multiple、32 PPU、Point、Uncompressed、Alpha transparency 后强制导入。

- [ ] **Step 2: 实现八个动画片段和控制器状态**

为四个 Idle 片段绑定单张 Sprite；为四个 `*_Run` 片段绑定 A/B/A 帧，帧率 12 FPS 且 loop。控制器的状态名必须与 Task 2 返回值完全一致，默认状态为 `Professor_Idle_Down`。

- [ ] **Step 3: 实现幂等场景视觉安装**

打开 `SchoolClassroom.unity`，在完整层级中唯一查找 `Professor`。验证根对象存在原始 `SpriteRenderer` 和现有课堂广播组件；创建或复用其直接子对象 `Professor Visual`，在其上确保存在 `SpriteRenderer`、`Animator`、`ProfessorVisualAnimator`。将控制器、`Professor_Idle_Down`、原 Renderer 的 sorting order、白色色调和本地原点写入子对象，并设置 `visual.localScale = Vector3.one * 1.25f`。最后仅禁用原 Renderer，并保存场景。

- [ ] **Step 4: 执行安装器并进行编译检查**

执行 `ProfessorCharacterInstaller.Apply` 后，以 Unity `-batchmode -quit` 打开项目。预期退出码 0 且日志无 C# compiler error。若 `TextureImporter.spritesheet` 产生既有 API 弃用警告，记录但不把它误判为失败。

### Task 4: 回归验证教授场景与既有剧情边界

**Files:**
- Create: `Assets/Tests/EditMode/ProfessorCharacterSceneTests.cs`
- Modify: `Assets/Scenes/SchoolClassroom.unity`（仅在安装器修正测试失败的确定性配置时）

**Interfaces:**
- Consumes: `ProfessorCharacterInstaller.Apply()` 产物与 `Professor Visual`。
- Produces: 对视觉隔离与既有组件保留的 EditMode 回归保护。

- [ ] **Step 1: 写出场景接入测试**

```csharp
[Test]
public void SchoolClassroom_SeparatesProfessorVisualFromExistingProfessorLogic()
{
    Scene scene = EditorSceneManager.OpenScene(
        "Assets/Scenes/SchoolClassroom.unity", OpenSceneMode.Single);
    Transform professor = scene.GetRootGameObjects()
        .SelectMany(root => root.GetComponentsInChildren<Transform>(true))
        .Single(node => node.name == "Professor");

    SpriteRenderer legacy = professor.GetComponent<SpriteRenderer>();
    Transform visual = professor.Find("Professor Visual");

    Assert.That(legacy, Is.Not.Null);
    Assert.That(legacy.enabled, Is.False);
    Assert.That(visual, Is.Not.Null);
    Assert.That(visual.GetComponent<Animator>(), Is.Not.Null);
    Assert.That(visual.GetComponent<ProfessorVisualAnimator>(), Is.Not.Null);
    Assert.That(visual.GetComponent<SpriteRenderer>().sortingOrder,
        Is.EqualTo(legacy.sortingOrder));
    Assert.That(professor.GetComponent<BroadcastDialogueTrigger>(), Is.Not.Null);
}
```

- [ ] **Step 2: 运行教授相关 EditMode 测试**

运行 `ProfessorVisualAnimatorTests` 与 `ProfessorCharacterSceneTests`。预期全部通过；若 batch runner 未写出 XML，改由 Unity Test Runner 运行同名测试并记录实际结果，不能把缺失报告当成通过。

- [ ] **Step 3: 做最终非破坏性核查**

检查 `Professor Visual` 是 `Professor` 的直接子对象；根对象仍保留原有剧情组件、Transform、父级、层级和可移动性。检查 8 个 `.anim`、控制器和 12 个子 Sprite 均存在；确认场景不引用 `Assets/Scenes Copy`。

- [ ] **Step 4: 报告可验证结果**

向用户报告已生成和接入的实际文件、场景位置、Unity 编译结果，以及实际获得的测试结果或测试运行器限制；不要声称未执行或无报告支撑的测试已通过。
