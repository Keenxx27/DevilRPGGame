# 十三角色素材与接入 Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 制作十三的 2D 俯视硬边像素图集，并接入现有的持久跟随角色，使它静止时坐下、跟随玩家时四足小跑。

**Architecture:** 一张透明背景的 4 列 × 3 行图集提供四个坐姿静止帧与四向两帧小跑动画。新的视觉脚本只观察既有 `ThirteenFollower` 改变的根对象位置，在子物体上切换 Animator 状态；不修改跟随、跨场景持久化、剧情或交互流程。

**Tech Stack:** Unity 2022.3、C#、Unity Animator、Unity Test Framework、PNG Sprite Multiple 导入。

## Global Constraints

- 美术必须是 2D 俯视视角硬边像素风，透明背景、有限色阶、无平滑插画渲染。
- 单帧为 48×64 像素，图集为 192×192 像素；四列依次为下、上、右、左，三行依次为坐姿静止、行走 A、行走 B。
- 十三是现实成年家猫比例，静坐高度约为主角小腿中下段；黑色短毛、蓝色圆瞳、猫耳轮廓的薄膜耳和冰蓝绒状尾焰必须可读。
- 静止的四个方向全为坐姿；移动时才四足低重心小跑，每方向两帧。
- 左右方向独立绘制，不通过 `flipX` 镜像替代。
- 不修改 `ThirteenFollower` 的跟随、持久化、场景重连与命名行为，也不修改 `ThirteenEncounter` 的剧情流程。

---

### Task 1: 生成并验证十三图集

**Files:**
- Create: `Assets/Art/Characters/thirteen-character-sheet-pixel-v1.png`
- Create: `Assets/Art/Characters/thirteen-character-sheet-pixel-v1.png.meta`

**Interfaces:**
- Produces: 名为 `Thirteen_Idle_Down`、`Thirteen_Walk_Down_A`、`Thirteen_Walk_Down_B`，以及上、右、左方向对应帧的 12 个 Sprite。

- [ ] **Step 1: 生成透明图集**

使用图像生成工具制作四列三行的十三。每格均为独立完整角色；下、上、右、左的静止帧为坐姿，行走帧为四足小跑。图中不得出现地面、蘑菇、文字、边框、发光背景或其他角色。

- [ ] **Step 2: 检查像素风和轮廓**

检查图集具有硬边像素簇、有限色阶和透明背景。确认黑色毛发仍有少量深蓝灰轮廓，蓝色圆瞳与冰蓝尾焰可见，耳朵保持猫耳轮廓而非鳍片。

- [ ] **Step 3: 按最终像素尺寸导入**

将确认的图集裁为 192×192 像素并置入 `Assets/Art/Characters/`。安装器配置为 Multiple、PPU 32、Point 过滤、无压缩、底部中心 Pivot。

### Task 2: 创建坐姿/小跑动画和视觉状态脚本

**Files:**
- Create: `Assets/Character/Thirteen_Idle_Down.anim`
- Create: `Assets/Character/Thirteen_Idle_Up.anim`
- Create: `Assets/Character/Thirteen_Idle_Right.anim`
- Create: `Assets/Character/Thirteen_Idle_Left.anim`
- Create: `Assets/Character/Thirteen_Down_Run.anim`
- Create: `Assets/Character/Thirteen_Up_Run.anim`
- Create: `Assets/Character/Thirteen_Right_Run.anim`
- Create: `Assets/Character/Thirteen_Left_Run.anim`
- Create: `Assets/Scripts/ThirteenVisualAnimator.cs`
- Create: `Assets/Tests/EditMode/ThirteenVisualAnimatorTests.cs`

**Interfaces:**
- Consumes: Task 1 的 12 个 Sprite 与 `ThirteenFollower` 已改变的根对象 Transform 位置。
- Produces: `ThirteenVisualAnimator.ResolveAnimation(Vector2 direction, bool isMoving)`，返回八个 `Thirteen_*` Animator 状态之一。

- [ ] **Step 1: 写出失败的左向小跑测试**

在 `ThirteenVisualAnimatorTests.cs` 编写：

```csharp
[Test]
public void ResolveAnimation_UsesIndependentLeftRunWithoutSpriteFlip()
{
    CharacterAnimationState state = ThirteenVisualAnimator.ResolveAnimation(Vector2.left, true);
    Assert.That(state.Name, Is.EqualTo("Thirteen_Left_Run"));
    Assert.That(state.FlipX, Is.False);
}
```

- [ ] **Step 2: 运行测试确认失败**

在 Unity Test Runner 的 EditMode 中运行 `ThirteenVisualAnimatorTests`。预期失败原因：`ThirteenVisualAnimator` 尚未定义。

- [ ] **Step 3: 实现最小视觉状态脚本**

创建 `ThirteenVisualAnimator` 并放在视觉子物体上。在 `LateUpdate` 比较根对象位置与上一帧位置：位移大于 `0.001f` 时，按位移方向播放 `Thirteen_*_Run`；静止时按最后朝向播放 `Thirteen_Idle_*`。所有结果的 `FlipX` 为 `false`。

- [ ] **Step 4: 创建八个动画片段和控制器状态**

四个 Idle 动画引用 Task 1 的坐姿帧；四个 Run 动画以 12 FPS 循环播放 `Walk_A`、`Walk_B`、`Walk_A`。Animator Controller 的状态名必须与视觉脚本返回值一致，默认状态为 `Thirteen_Idle_Down`。

- [ ] **Step 5: 运行测试确认通过**

在 Unity Test Runner 的 EditMode 中再次运行 `ThirteenVisualAnimatorTests`。预期：左向小跑为独立 `Thirteen_Left_Run` 且不发生镜像。

### Task 3: 将视觉接入持久十三根对象

**Files:**
- Create: `Assets/Editor/ThirteenCharacterInstaller.cs`
- Modify: `Assets/Scenes/FluorescentMushroomCave.unity`
- Create: `Assets/Tests/EditMode/ThirteenCharacterSceneTests.cs`

**Interfaces:**
- Consumes: Task 1 图集、Task 2 动画与现有 `ThirteenFollower`。
- Produces: 洞穴中的 `？？？` 根对象下的 `Thirteen Visual` 子物体。完成投喂后，该根对象被命名为 `十三` 并持久化时，视觉子物体会一并保留。

- [ ] **Step 1: 写出失败的场景配置测试**

在 `ThirteenCharacterSceneTests.cs` 打开 `Assets/Scenes/FluorescentMushroomCave.unity`，断言 `？？？` 根对象仍含 `ThirteenFollower`，其 `Thirteen Visual` 子物体有启用的 SpriteRenderer、Animator、`ThirteenVisualAnimator`，根对象旧 SpriteRenderer 为禁用状态。

- [ ] **Step 2: 运行测试确认失败**

在 Unity Test Runner 的 EditMode 中运行 `ThirteenCharacterSceneTests`。预期失败原因：`Thirteen Visual` 子物体尚不存在。

- [ ] **Step 3: 实现编辑器安装器**

安装器切分图集、创建动画与控制器，并在洞穴 `？？？` 对象下创建 `Thirteen Visual`。子物体局部位置为零，排序层级沿用根对象，应用猫体大小的视觉缩放；根对象的 `ThirteenFollower`、`ThirteenEncounter` 引用、Transform、Collider 与旧有子物体不得改变，仅禁用根对象旧 SpriteRenderer。

- [ ] **Step 4: 运行安装器并检查持久化路径**

执行 `ThirteenCharacterInstaller.Apply`。确认 `ThirteenEncounter.BeginReturnHome` 仍通过根对象上的 `ThirteenFollower.BeginPersistentFollowing()` 持久化整个对象，因此视觉子物体自动跨场景保留。

- [ ] **Step 5: 运行场景测试确认通过**

在 Unity Test Runner 的 EditMode 中运行 `ThirteenCharacterSceneTests`。预期：通过，且视觉与持久跟随根对象分离。

### Task 4: 编译与运行时核对

**Files:**
- Verify: `Assets/Scenes/FluorescentMushroomCave.unity`
- Verify: `Assets/Scripts/ThirteenFollower.cs`
- Verify: `Assets/Scripts/ThirteenEncounter.cs`

**Interfaces:**
- Consumes: Tasks 1–3 的接入。
- Produces: 洞穴中坐姿待机、投喂后持久跟随时四足小跑的十三视觉。

- [ ] **Step 1: 执行 Unity 无界面编译检查**

运行：

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\Documents\GitHub\DevilRPGGame' -logFile 'D:\Documents\GitHub\DevilRPGGame\Logs\verify-thirteen-character.log'
```

预期：退出代码为 0，日志不含 C# 编译错误。

- [ ] **Step 2: 在编辑器中进行运行时检查**

在荧光菌洞靠近十三，确认其四向静止为坐姿。完成肉干投喂后，让玩家在不同方向移动，确认十三四足小跑跟随，随后触发回家与跨场景转移，确认同一对象及其视觉仍存在。

- [ ] **Step 3: 运行相关 EditMode 测试**

在 Unity Test Runner 中运行 `ThirteenVisualAnimatorTests` 与 `ThirteenCharacterSceneTests`。预期：全部通过。
