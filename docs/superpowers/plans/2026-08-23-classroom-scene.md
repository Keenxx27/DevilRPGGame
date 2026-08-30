# Classroom Scene Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 在 `SampleScene` 中直接序列化一个可识别的俯视 2D 教室，同时保留现有角色移动功能。

**Architecture:** 只修改一个 Unity 场景文件。先用结构校验建立 RED 基线，再通过工作区内的一次性机械生成脚本写入静态 `GameObject`、`Transform` 与 `SpriteRenderer` YAML，最后执行结构、脚本编译和行为回归检查；生成脚本与备份不进入 Unity 项目。

**Tech Stack:** Unity 2022.3.62f3c1、Unity Scene YAML、2D URP、SpriteRenderer、PowerShell/Python 机械场景生成。

## Global Constraints

- 教室采用俯视 2D，前方位于画面上侧，空间约宽 16、高 10 个 Unity 单位。
- 不引入外部图片、美术包、Tilemap 或运行时场景生成脚本。
- 复用 `Main Character` 的圆形 Sprite：`fileID -2413806693520163455`、GUID `a86470a33a6bf42c4b3595704624658b`。
- 保留 `Main Character`、`Professor`、`Global Light 2D` 与 `MainCharacterMovement`，默认速度仍为 5。
- 不添加 `Rigidbody2D`、`Collider2D`、寻路、交互、对话、动画、镜头跟随或场景切换。
- 所有新物件静态序列化到 `Assets/Scenes/SampleScene.unity`。
- 项目没有 Git 仓库；不初始化 Git，不执行提交。

---

## File Structure

- Modify: `Assets/Scenes/SampleScene.unity` — 教室层级、静态物件、角色位置、摄像机尺寸。
- Create temporarily: `work/classroom-scene/build_classroom_scene.py` — 机械生成完整 Unity YAML 块并更新现有对象。
- Create temporarily: `work/classroom-scene/verify_classroom_scene.ps1` — 在写入前后验证场景合同。
- Create backup: `work/classroom-scene/SampleScene.before-classroom.unity` — 修改前可恢复副本。

### Task 1: 建立场景合同并确认 RED

**Files:**
- Test: `work/classroom-scene/verify_classroom_scene.ps1`
- Read: `Assets/Scenes/SampleScene.unity`

**Interfaces:**
- Consumes: Unity 场景 YAML 文本。
- Produces: 退出码；缺少教室时失败，满足全部场景合同后成功。

- [ ] **Step 1: 保存修改前备份**

精确复制 `Assets/Scenes/SampleScene.unity` 到 `work/classroom-scene/SampleScene.before-classroom.unity`，覆盖前确认源文件与目标文件的绝对路径不同。

- [ ] **Step 2: 创建结构验证脚本**

`verify_classroom_scene.ps1` 必须读取场景全文并逐项断言：

```powershell
$requiredNames = @(
    'Classroom', 'Floor', 'Floor Surface', 'Walls',
    'Wall Top', 'Wall Bottom', 'Wall Left', 'Wall Right',
    'Blackboard', 'Blackboard Surface', 'Teacher Area',
    'Teacher Desk', 'Podium', 'Student Desks',
    'Windows', 'Window 1', 'Window 2', 'Window 3',
    'Door', 'Door Panel', 'Storage Cabinet', 'Storage Cabinet Body'
)

$studentDeskNames = 1..4 | ForEach-Object {
    $row = $_
    1..3 | ForEach-Object { "Student Desk R${row}C$_" }
}
$chairNames = 1..4 | ForEach-Object {
    $row = $_
    1..3 | ForEach-Object { "Chair R${row}C$_" }
}

foreach ($name in $requiredNames + $studentDeskNames + $chairNames) {
    if (-not $sceneText.Contains("m_Name: $name")) {
        throw "缺少场景对象: $name"
    }
}

if ([regex]::Matches($sceneText, 'm_Name: Student Desk R\dC\d').Count -ne 12) {
    throw '学生桌数量不是 12'
}
if ([regex]::Matches($sceneText, 'm_Name: Chair R\dC\d').Count -ne 12) {
    throw '椅子数量不是 12'
}
if (-not $sceneText.Contains('moveSpeed: 5')) {
    throw 'MainCharacterMovement 默认速度被改动'
}
if ($sceneText.Contains('--- !u!50 ') -or
    $sceneText.Contains('--- !u!58 ') -or
    $sceneText.Contains('--- !u!61 ')) {
    throw '场景中出现未批准的 2D 物理组件'
}
```

脚本还必须对摄像机对象 `519420031` 断言 `orthographic size: 5.7`，对 `Main Character` 与 `Professor` 的 SpriteRenderer 断言 `m_SortingOrder: 10`，并检查 `SceneRoots` 只包含摄像机、全局光照与 `Classroom` 根节点。
验收数量必须明确为三扇窗户和一扇门：`Window 1`、`Window 2`、`Window 3` 各出现一次，`Door Panel` 出现一次。

- [ ] **Step 3: 运行校验并确认 RED**

Run：

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File work/classroom-scene/verify_classroom_scene.ps1
```

Expected：失败并明确报告 `缺少场景对象: Classroom`；失败原因是教室尚未搭建，而不是脚本语法或路径错误。

### Task 2: 机械写入静态教室物件

**Files:**
- Create temporarily: `work/classroom-scene/build_classroom_scene.py`
- Modify: `Assets/Scenes/SampleScene.unity`

**Interfaces:**
- Consumes: 当前场景、现有对象 ID、下方完整布局表。
- Produces: 包含静态教室层级的确定性 Unity Scene YAML。

- [ ] **Step 1: 创建确定性场景生成脚本**

脚本必须在写入前验证以下锚点各出现一次，否则停止：

```text
--- !u!4 &519420032      Main Camera Transform
--- !u!20 &519420031     Main Camera Camera
--- !u!4 &1667414359     Main Character Transform
--- !u!212 &1667414358   Main Character SpriteRenderer
--- !u!4 &35893607       Professor Transform
--- !u!212 &35893606     Professor SpriteRenderer
--- !u!1660057539 &9223372036854775807  SceneRoots
```

若已存在 `m_Name: Classroom`，脚本停止，避免重复生成。所有新对象从 fileID `2000000000` 开始分配；每个节点固定占用十个编号，GameObject 为基数、SpriteRenderer 为基数加一、Transform 为基数加二。组节点仅序列化 GameObject 与 Transform；可见物件序列化 GameObject、SpriteRenderer 与 Transform。

生成脚本必须使用以下完整布局数据：

| 父节点 | 对象 | 位置 (x,y) | 缩放 (x,y) | 颜色 RGB | 排序 |
|---|---|---:|---:|---|---:|
| Floor | Floor Surface | (0,0) | (16,10) | (0.72,0.68,0.55) | -20 |
| Walls | Wall Top | (0,4.8) | (16,0.35) | (0.22,0.18,0.15) | -10 |
| Walls | Wall Bottom | (0,-4.8) | (16,0.35) | (0.22,0.18,0.15) | -10 |
| Walls | Wall Left | (-7.8,0) | (0.35,10) | (0.22,0.18,0.15) | -10 |
| Walls | Wall Right | (7.8,0) | (0.35,10) | (0.22,0.18,0.15) | -10 |
| Blackboard | Blackboard Surface | (0,4.15) | (6.5,0.8) | (0.04,0.18,0.12) | -5 |
| Teacher Area | Teacher Desk | (0,2.75) | (2.6,0.75) | (0.42,0.23,0.10) | 0 |
| Teacher Area | Podium | (-2.4,3.1) | (0.9,0.9) | (0.50,0.31,0.16) | 0 |
| Windows | Window 1 | (-7.45,2.5) | (0.25,1.25) | (0.40,0.78,0.95) | -5 |
| Windows | Window 2 | (-7.45,0) | (0.25,1.25) | (0.40,0.78,0.95) | -5 |
| Windows | Window 3 | (-7.45,-2.5) | (0.25,1.25) | (0.40,0.78,0.95) | -5 |
| Door | Door Panel | (7.45,-3.25) | (0.30,1.45) | (0.55,0.34,0.16) | -5 |
| Storage Cabinet | Storage Cabinet Body | (6.65,2.8) | (1.25,1.8) | (0.38,0.27,0.18) | -5 |

十二张学生桌的位置为三列 `x = -4, 0, 4` 与四排 `y = 1.45, 0.15, -1.15, -2.45` 的笛卡尔积；每张桌缩放 `(1.55, 0.48)`、颜色 `(0.46,0.27,0.13)`、排序 `0`。对应椅子使用同一 x、`y - 0.52`，缩放 `(0.72,0.40)`、颜色 `(0.20,0.14,0.10)`、排序 `1`。

组节点层级必须为：

```text
Classroom
├── Floor
├── Walls
├── Blackboard
├── Teacher Area
│   └── Professor（现有对象）
├── Student Desks
├── Windows
├── Door
├── Storage Cabinet
└── Main Character（现有对象）
```

脚本对现有对象执行以下精确更新：

```text
Main Camera: position (0,0,-10), orthographic size 5.7
Main Character: local position (-4,-3.25,0), parent Classroom, sorting order 10
Professor: local position (0,3.35,0), parent Teacher Area, sorting order 10
SceneRoots: Main Camera、Global Light 2D、Classroom
```

- [ ] **Step 2: 运行生成脚本并检查机械写入状态**

Run：

```powershell
python work/classroom-scene/build_classroom_scene.py
```

Expected：退出码 0；场景包含一个且仅一个 `Classroom`；脚本报告新增 9 个组节点、37 个可见静态物件，并完成 3 个现有对象的更新。

- [ ] **Step 3: 运行结构校验并确认 GREEN**

再次运行 Task 1 Step 3。

Expected：退出码 0；报告教室组节点、十二桌、十二椅、三窗、门、储物柜、角色位置、摄像机和无物理组件全部通过。

### Task 3: 回归验证与最终范围检查

**Files:**
- Verify: `Assets/Scenes/SampleScene.unity`
- Verify: `Assets/Scripts/MainCharacterMovement.cs`
- Verify: `Assets/Tests/EditMode/MainCharacterMovementTests.cs`

**Interfaces:**
- Consumes: 完成后的场景和现有移动代码。
- Produces: 可审计的结构、编译与行为验证结果。

- [ ] **Step 1: 编译现有移动程序集和测试程序集**

使用 Unity 自带 Mono 编译器，引用 `UnityEngine.CoreModule.dll`、`UnityEngine.InputLegacyModule.dll`、`netstandard.dll` 与 NUnit；预期运行时程序集和 EditMode 测试程序集均退出码 0。

- [ ] **Step 2: 运行现有移动行为夹具**

运行之前已建立的 `MovementHarness.exe` 等价流程，验证 W/A/S/D、无输入、斜向限速和 deltaTime 缩放共 7 项；预期 `7/7 通过`。

- [ ] **Step 3: 核对最终修改范围**

确认 Unity 项目内只修改 `Assets/Scenes/SampleScene.unity`，没有新增运行时脚本、物理组件或外部素材。保留工作区中的备份与验证脚本，便于恢复和审计，但不把它们复制进 Unity 项目。

- [ ] **Step 4: 人工 Unity 验证交接**

由于当前 Codex 沙箱无法可靠启动 Unity 批处理，提示用户打开 Unity，等待场景导入后检查 Scene/Game 视图，并在 Play 模式确认角色仍可 WASD 移动。不得把静态/Mono 替代验证描述为 Unity Editor Test Runner 已通过。
