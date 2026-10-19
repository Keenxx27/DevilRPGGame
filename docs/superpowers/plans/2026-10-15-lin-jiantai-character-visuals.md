# 林健太角色外观 Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 生成并接入林健太的 2D 俯视像素角色素材，使其具备独立四向站立、四向两帧行走、固定碰撞与正确座位遮挡。

**Architecture:** 用单张透明 PNG 精灵表承载 12 帧像素角色图像，由 Unity 多精灵导入器切分。现有动画剪辑继续驱动一个视觉子物体的 `SpriteRenderer`，而角色物理碰撞体迁移到不缩放的独立碰撞子物体。`MainCharacterMovement` 从“右向贴图加左右镜像”改为真正的四向状态机。

**Tech Stack:** Unity 2022.3、C#、Unity Animator、Sprite Renderer、Image Generation、NUnit EditMode tests。

## Global Constraints

- 每帧为 48 x 64 像素，透明背景，32 Pixels Per Unit；视觉子物体缩放为 1.2。
- 站立四帧，行走四方向各两帧，共 12 帧；左右必须独立绘制且不得镜像。
- 林健太左脸的大面积暗红树皮状侵蚀始终保留在角色自身左脸，不发光。
- 主角根对象既有的 CircleCollider2D（半径 0.5）保持不变，不随视觉缩放或动画帧改变。
- 教室座位遮挡使用：课桌 9 < 人物 10 < 椅子 11；不新增坐姿。
- 不修改 `Assets/Scenes Copy`。

---

### Task 1: 生成并验证林健太精灵表

**Files:**
- Create: `Assets/Art/Characters/lin-jiantai-character-sheet-pixel-v1.png`
- Create: `Assets/Art/Characters/lin-jiantai-character-sheet-pixel-v1.png.meta`

**Interfaces:**
- Produces: 4 列 x 3 行的透明 PNG；列依次为下、上、右、左，第一行为对应站立帧，第二、三行为该方向行走帧 A/B。
- Consumed by: Task 2 的切分配置和六个现有动画剪辑、两个新增左向动画剪辑。

- [ ] **Step 1: 生成一张 192 x 192 像素透明精灵表。**

使用图像生成工具，提示词必须包含：2D 俯视 RPG 像素风、四列三行等大帧、48 x 64 像素帧、普通大学生比例、帽兜戴起的深灰近黑连帽衫、中短黑发与左侧碎刘海、左脸大面积暗红树皮状侵蚀、灰白内搭、深灰蓝长裤、黑白运动鞋、站立与两帧走路姿态、透明背景、无文字、无阴影地面。

- [ ] **Step 2: 检查图片透明通道、画布与帧顺序。**

Run: `Get-Item Assets/Art/Characters/lin-jiantai-character-sheet-pixel-v1.png`

Expected: 画布为 192 x 192 像素，四列三行均有角色，背景透明；左向和右向的脸部侵蚀不互为镜像。

- [ ] **Step 3: 配置 Unity 导入元数据。**

在图片导入设置中明确设置：`Sprite (2D and UI)`、`Multiple`、`Pixels Per Unit = 32`、`Filter Mode = Point`、`Compression = None`、12 个 48 x 64 切片，枢轴为 `Bottom Center`。切片命名：

```text
LinJiantai_Idle_Down, LinJiantai_Walk_Down_A, LinJiantai_Walk_Down_B,
LinJiantai_Idle_Up, LinJiantai_Walk_Up_A, LinJiantai_Walk_Up_B,
LinJiantai_Idle_Right, LinJiantai_Walk_Right_A, LinJiantai_Walk_Right_B,
LinJiantai_Idle_Left, LinJiantai_Walk_Left_A, LinJiantai_Walk_Left_B
```

- [ ] **Step 4: 在 Unity 中检查每个切片。**

Expected: 每一帧脚底处于同一底部基线；正面、左向和右向的左脸侵蚀区域可辨识；背面不显示脸部侵蚀；没有白色或灰色背景。

### Task 2: 接入独立四向动画、视觉与物理分离

**Files:**
- Modify: `Assets/Scripts/MainCharacterMovement.cs`
- Modify: `Assets/Character/Player_Idle_Down.anim`
- Modify: `Assets/Character/Player_Idle_Up.anim`
- Modify: `Assets/Character/Player_Idle_Right.anim`
- Create: `Assets/Character/Player_Idle_Left.anim`
- Modify: `Assets/Character/Player_Down_Run.anim`
- Modify: `Assets/Character/Player_Up_Run.anim`
- Modify: `Assets/Character/Player_Right_Run.anim`
- Create: `Assets/Character/Player_Left_Run.anim`
- Modify: `Assets/Player_Down_Run_01.controller`
- Modify: active player configuration in each gameplay scene through the existing scene installer/editor workflow.
- Test: `Assets/Tests/EditMode/MainCharacterMovementTests.cs`

**Interfaces:**
- Consumes: `LinJiantai_Idle_*` and `LinJiantai_Walk_*` sprites from Task 1.
- Produces: `MainCharacterMovement` plays `Player_Idle_Left` and `Player_Left_Run` for negative horizontal movement and never flips the visual renderer.

- [ ] **Step 1: 写入失败测试，覆盖左向独立动画与无镜像渲染。**

在 `MainCharacterMovementTests` 中增加一个可测试的方向状态解析接口，例如：

```csharp
[Test]
public void ResolveAnimation_UsesLeftStatesWithoutSpriteFlip()
{
    CharacterAnimationState state = MainCharacterMovement.ResolveAnimation(Vector2.left, true);
    Assert.That(state.Name, Is.EqualTo("Player_Left_Run"));
    Assert.That(state.FlipX, Is.False);
}
```

- [ ] **Step 2: 运行 EditMode 测试，确认新测试因接口不存在而失败。**

Run: Unity Test Runner 的 `RPG.Tests.MainCharacterMovementTests`

Expected: 新增测试编译失败或失败，提示 `ResolveAnimation` 或 `CharacterAnimationState` 尚不存在。

- [ ] **Step 3: 最小化实现四向状态解析。**

在 `MainCharacterMovement` 中新增左向 idle/run 哈希和 `CharacterAnimationState`，令水平负方向返回 `Player_Idle_Left` 或 `Player_Left_Run`，并在所有方向将 `SpriteRenderer.flipX` 设为 `false`。保持原有面对方向和速度计算不变。

```csharp
public readonly struct CharacterAnimationState
{
    public readonly int Hash;
    public readonly string Name;
    public readonly bool FlipX;
}
```

- [ ] **Step 4: 将 8 个动画剪辑绑定至新精灵表。**

四个 idle 剪辑各绑定对应 `LinJiantai_Idle_*` 单帧；四个 run 剪辑以 12 FPS 循环绑定 A、B、A 帧。将左向 clip 纳入 `Player_Down_Run_01.controller`，状态名必须是 `Player_Idle_Left` 和 `Player_Left_Run`。

- [ ] **Step 5: 分离视觉子物体与碰撞体。**

保留玩家根对象的 Rigidbody2D 与 CircleCollider2D（半径 0.5）。创建或复用 `Player Visual` 子物体承载 Animator 与 SpriteRenderer，局部缩放固定为 1.2；关闭根对象旧静态 SpriteRenderer。不得缩放、移动或替换根对象的 CircleCollider2D。

- [ ] **Step 6: 在所有游戏场景应用同一玩家配置。**

使用项目既有的场景生成器/编辑器安装流程更新各可进入场景中的玩家对象，保证它们均引用同一控制器、视觉子物体和碰撞结构；不改动 `Assets/Scenes Copy`。

- [ ] **Step 7: 运行目标测试与无界面编译。**

Run: Unity Test Runner 的 `RPG.Tests.MainCharacterMovementTests`

Expected: 全部通过；左向测试验证非镜像状态。

Run: `Unity.exe -batchmode -quit -projectPath D:\Documents\GitHub\DevilRPGGame -logFile Logs\verify-lin-jiantai-character.log`

Expected: 日志包含 `Exiting batchmode successfully now!`，且没有 `error CS`。

### Task 3: 验证教室视觉比例与座位遮挡

**Files:**
- Modify: `Assets/Editor/SchoolLibraryInstaller.cs`（仅在现有生成器缺少统一玩家视觉安装入口时）
- Test: `Assets/Tests/EditMode/MainCharacterMovementTests.cs`
- Test: `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs`（仅在测试中已能定位玩家与座位渲染器时）

**Interfaces:**
- Consumes: Task 2 的视觉子物体、碰撞结构和八个 Animator 状态。
- Produces: 可重复安装的玩家视觉配置；在课堂座位剧情中保持 desk < player < chair 排序。

- [ ] **Step 1: 写入失败测试，验证视觉与物理分离规格。**

```csharp
[Test]
public void PlayerVisual_DoesNotScaleTheCollisionBox()
{
    Assert.That(visual.localScale, Is.EqualTo(Vector3.one * 1.2f));
    Assert.That(collider.radius, Is.EqualTo(.5f));
}
```

- [ ] **Step 2: 运行新增测试，确认在应用统一场景配置之前失败。**

Run: Unity Test Runner 的 `RPG.Tests.MainCharacterMovementTests`

Expected: 失败，指出视觉子物体或独立碰撞体尚未满足规格。

- [ ] **Step 3: 用最小生成器改动提供可重复的安装入口。**

仅当 Task 2 的场景配置无法通过现有流程重复应用时，在 `SchoolLibraryInstaller` 新增明确命名的菜单项，例如 `Apply Lin Jiantai Character Visuals`。它只负责创建/更新 `Player Visual`、`Player Collision`、Animator controller、缩放、碰撞尺寸与排序，不创建新场景物件。

- [ ] **Step 4: 应用后运行测试和人工场景检查。**

Expected: 主角高度约 2.4 世界单位，高于课桌约 1.56 与椅子约 1.20；根对象碰撞体仍为半径 0.5 的 CircleCollider2D。座位时课桌排序 9、人物 10、椅子 11；普通行走不被固定为座位排序。

- [ ] **Step 5: 全量验证。**

Run: Unity Test Runner 的全部 EditMode 测试。

Expected: 全部通过；没有新 Console 错误。
