# 林春宝角色素材与接入 Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 制作林春宝的八姿态像素角色素材，并将其接入厨房与后续转移至客厅的剧情角色对象。

**Architecture:** 一张透明背景的 4 列 × 3 行角色图集提供四个站姿与每方向两帧行走动画。编辑器安装器负责切图、创建动画、配置 Animator Controller，并给厨房中唯一的 `Lin Chunbao` 对象添加独立视觉子物体；一个小型 NPC 视觉脚本根据其 Transform 位移选择朝向和行走/站立动画。

**Tech Stack:** Unity 2022.3、C#、Unity Animator、Unity Test Framework、PNG Sprite Multiple 导入。

## Global Constraints

- 单帧为 48×64 像素，图集为 192×192 像素，透明背景，PPU 为 32，Point 过滤。
- 春宝角色本体视觉高度约为林健太的九成，保持修长学生比例，不采用低龄化的大头大眼比例。
- 四列依次为下、上、右、左；三行依次为站立、行走 A、行走 B。
- 每个方向都使用独立绘制帧，不以 `flipX` 镜像替代。
- 角色外观严格遵循 `docs/superpowers/specs/2026-10-18-lin-chunbao-character-visual-design.md`。
- 不改变林春宝现有剧情逻辑、位置、父子关系、碰撞配置或调查交互。

---

### Task 1: 生成并验证林春宝角色图集

**Files:**
- Create: `Assets/Art/Characters/lin-chunbao-character-sheet-pixel-v1.png`
- Create: `Assets/Art/Characters/lin-chunbao-character-sheet-pixel-v1.png.meta`

**Interfaces:**
- Produces: 12 个帧名为 `LinChunbao_Idle_Down`、`LinChunbao_Walk_Down_A`、`LinChunbao_Walk_Down_B`，其余方向按同一命名规则的 Sprite。

- [ ] **Step 1: 生成透明 PNG 图集**

使用图像生成工具，以 4×3 图集产出深棕肩部松马尾、蓝绿色宽松训练短袖、深色运动长裤、单侧暖红腕带的林春宝。画面必须包含下/上/右/左站姿及每方向 A/B 两帧行走；跑步帧仅显示腿、手臂与马尾的轻微摆动。

- [ ] **Step 2: 检查图集布局与可读性**

检查 PNG 为透明背景，且每个 48×64 单元没有串格。确认蓝绿色上衣的亮度高于林健太的近黑色连帽衫，且左右方向不是运行时镜像的同一帧。

- [ ] **Step 3: 以像素尺寸导入项目**

将图集置入 `Assets/Art/Characters/`。后续安装器会将其配置为 Multiple、PPU 32、Point 过滤、无压缩、底部中心 Pivot。

### Task 2: 创建林春宝四向动画与位移驱动脚本

**Files:**
- Create: `Assets/Character/LinChunbao_Idle_Down.anim`
- Create: `Assets/Character/LinChunbao_Idle_Up.anim`
- Create: `Assets/Character/LinChunbao_Idle_Right.anim`
- Create: `Assets/Character/LinChunbao_Idle_Left.anim`
- Create: `Assets/Character/LinChunbao_Down_Run.anim`
- Create: `Assets/Character/LinChunbao_Up_Run.anim`
- Create: `Assets/Character/LinChunbao_Right_Run.anim`
- Create: `Assets/Character/LinChunbao_Left_Run.anim`
- Create: `Assets/Scripts/LinChunbaoVisualAnimator.cs`
- Create: `Assets/Tests/EditMode/LinChunbaoVisualAnimatorTests.cs`

**Interfaces:**
- Consumes: Task 1 的 12 个 Sprite 与 `Transform.position` 的逐帧变化。
- Produces: `LinChunbaoVisualAnimator.ResolveAnimation(Vector2 direction, bool isMoving)`，返回需播放的动画状态名；视觉 Animator 使用八个同名状态。

- [ ] **Step 1: 写出失败的左向独立状态测试**

在 `LinChunbaoVisualAnimatorTests.cs` 编写：

```csharp
[Test]
public void ResolveAnimation_UsesIndependentLeftWalkWithoutSpriteFlip()
{
    CharacterAnimationState state = LinChunbaoVisualAnimator.ResolveAnimation(Vector2.left, true);
    Assert.That(state.Name, Is.EqualTo("LinChunbao_Left_Run"));
    Assert.That(state.FlipX, Is.False);
}
```

- [ ] **Step 2: 运行测试确认失败**

在 Unity Test Runner 的 EditMode 中运行 `LinChunbaoVisualAnimatorTests`。预期失败原因：`LinChunbaoVisualAnimator` 尚未定义。

- [ ] **Step 3: 实现最小的视觉选择脚本**

创建 `LinChunbaoVisualAnimator`，在 `LateUpdate` 比较当前和上一帧位置：位移大于 `0.001f` 时选择对应方向的 Run 状态，静止时选择上次朝向的 Idle 状态。`ResolveAnimation` 对 `Vector2.left` 必须返回 `LinChunbao_Left_Run` 和 `FlipX = false`；四个方向均不镜像。

- [ ] **Step 4: 创建八个动画片段和 Animator 状态**

站立片段仅引用各方向站立帧；行走片段以 12 FPS 依次播放 `Walk_A`、`Walk_B`、`Walk_A`，开启循环。Animator Controller 包含与八个动画片段同名的状态，视觉脚本仅在状态变化时调用 `Animator.Play`。

- [ ] **Step 5: 运行测试确认通过**

在 Unity Test Runner 的 EditMode 中再次运行 `LinChunbaoVisualAnimatorTests`。预期：左向状态为独立的 `LinChunbao_Left_Run`，且没有镜像。

### Task 3: 接入厨房林春宝对象并保护现有剧情逻辑

**Files:**
- Create: `Assets/Editor/LinChunbaoCharacterInstaller.cs`
- Modify: `Assets/Scenes/ApartmentKitchen.unity`
- Create: `Assets/Tests/EditMode/LinChunbaoCharacterSceneTests.cs`

**Interfaces:**
- Consumes: Task 1 图集与 Task 2 的动画/脚本。
- Produces: `ApartmentKitchen` 中 `Lin Chunbao` 下的 `Lin Chunbao Visual` 子物体，供既有 `LinChunbaoEncounter` 的 Transform 移动直接驱动。

- [ ] **Step 1: 写出失败的场景配置测试**

在 `LinChunbaoCharacterSceneTests.cs` 打开 `Assets/Scenes/ApartmentKitchen.unity`，断言唯一的 `Lin Chunbao` 对象拥有名称为 `Lin Chunbao Visual` 的子物体，其 SpriteRenderer 启用、排序层级为 10、Animator 与 `LinChunbaoVisualAnimator` 均存在；根对象原 SpriteRenderer 被禁用。

- [ ] **Step 2: 运行测试确认失败**

在 Unity Test Runner 的 EditMode 中运行 `LinChunbaoCharacterSceneTests`。预期失败原因：视觉子物体尚不存在。

- [ ] **Step 3: 实现编辑器安装器**

安装器执行时：配置图集切片；创建/更新八个动画片段与控制器；在 `ApartmentKitchen` 的 `Lin Chunbao` 下创建 `Lin Chunbao Visual`；将 Animator、SpriteRenderer 与 `LinChunbaoVisualAnimator` 置于子物体；子物体位置为零、缩放与林健太一致为 1.2、排序层级沿用根对象的 10；禁用根对象旧 SpriteRenderer。不得添加、删除或调整根对象上的 `LinChunbaoLivingRoomDialogue`、其 Transform 或既有子物体。

- [ ] **Step 4: 运行安装器并验证剧情对象**

执行 `LinChunbaoCharacterInstaller.Apply`。检查 `LinChunbaoEncounter.MoveThenTalk` 与 `MoveLinToLivingRoom` 继续移动根对象，子物体因为继承 Transform 自动播放行走动画；`LinChunbaoLivingRoomDialogue` 仍在根对象上。

- [ ] **Step 5: 运行场景测试确认通过**

在 Unity Test Runner 的 EditMode 中运行 `LinChunbaoCharacterSceneTests`。预期：通过，且所有断言表明视觉已与剧情对象分离。

### Task 4: 编译与运行时核对

**Files:**
- Verify: `Assets/Scenes/ApartmentKitchen.unity`
- Verify: `Assets/Scenes/ApartmentLivingRoom.unity`

**Interfaces:**
- Consumes: Tasks 1–3 的完整接入。
- Produces: 可在厨房剧情移动与转移至客厅后持续显示的林春宝角色视觉。

- [ ] **Step 1: 执行 Unity 无界面编译检查**

运行：

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\Documents\GitHub\DevilRPGGame' -logFile 'D:\Documents\GitHub\DevilRPGGame\Logs\verify-lin-chunbao-character.log'
```

预期：退出代码为 0，日志不含 C# 编译错误。

- [ ] **Step 2: 在编辑器中进行运行时检查**

从厨房场景进入游戏，确认春宝静止时显示下向站姿；触发其走向餐桌和离开厨房时确认对应朝向的行走动画与马尾摆动；她被移入客厅后仍显示同一视觉子物体。确认她的剧情文字、调查与位置均未改变。

- [ ] **Step 3: 运行相关 EditMode 测试**

在 Unity Test Runner 中运行 `LinChunbaoVisualAnimatorTests` 与 `LinChunbaoCharacterSceneTests`。预期：全部通过。
