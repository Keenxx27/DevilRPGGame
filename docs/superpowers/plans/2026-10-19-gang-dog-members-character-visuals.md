# 丧家犬普通成员角色美术与挥棍接入实施计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 为两类丧家犬普通成员接入四向透明像素角色视觉，并让高级成员在抓捕前执行可躲避的挥棍前摇。

**Architecture:** 道路中的成员根对象继续承担 Rigidbody2D、Collider2D 与既有巡逻行为。每个根对象新增视觉子对象，使用 Animator 和独立朝向控制器显示角色素材；高级成员脚本新增一次性挥棍状态，在前摇结束后才判定抓捕。

**Tech Stack:** Unity 2022.3.62f1、C#、UnityEditor Animation API、NUnit EditMode、透明 PNG。

## Global Constraints

- 所有素材为透明背景、2D 俯视 RPG 硬像素风。
- 左右朝向必须独立绘制，不使用镜像。
- 低级成员空手；高级成员右手持短棍或撬棍。
- 低级成员仅巡逻与拦路；不追逐、不抓捕。
- 高级成员挥棍前摇为 0.35 秒，攻击期间停步；挥棍结束时才可抓捕。
- 不引入生命值、伤害数值或其他战斗系统。

---

### Task 1: 高级成员挥棍状态

**Files:**
- Modify: `Assets/Scripts/GangDogAdvancedMember.cs`
- Create: `Assets/Tests/EditMode/GangDogAdvancedMemberTests.cs`

**Interfaces:**
- Produces: `public bool IsSwinging { get; }` 与 `public Vector2 FacingDirection { get; }`，供视觉控制器读取。
- Consumes: 已有 `detectionDistance`、`BeginCapture()`、`player.transform.position`。

- [ ] **Step 1: 写入失败测试**

```csharp
[Test]
public void ShouldBeginSwing_RequiresCloseDetectedPlayer()
{
    Assert.That(GangDogAdvancedMember.ShouldBeginSwing(1.1f, 4.8f), Is.True);
    Assert.That(GangDogAdvancedMember.ShouldBeginSwing(1.11f, 4.8f), Is.False);
}
```

- [ ] **Step 2: 运行测试确认失败**

运行 Unity EditMode 测试筛选 `RPG.Tests.GangDogAdvancedMemberTests`；预期失败，因为尚无 `ShouldBeginSwing`。

- [ ] **Step 3: 最小实现挥棍前摇**

```csharp
public const float SwingDistance = 1.1f;
public const float SwingDuration = .35f;
public bool IsSwinging => swingTime > 0f;
public Vector2 FacingDirection => facingDirection;

public static bool ShouldBeginSwing(float distance, float detectionRange)
{
    return distance <= SwingDistance && distance <= detectionRange;
}
```

在 `Update` 内，当成员正在追逐且距离满足 `ShouldBeginSwing` 时，锁定朝向并令 `swingTime = SwingDuration`。挥棍期间不移动；倒计时结束后仅当主角仍在 `.8f` 抓捕距离内调用既有 `BeginCapture()`，否则启动短冷却并恢复追逐。

- [ ] **Step 4: 运行测试确认通过**

运行 Unity EditMode 测试筛选 `RPG.Tests.GangDogAdvancedMemberTests`；预期所有该类测试通过。

### Task 2: 两类成员的四向视觉状态控制器

**Files:**
- Create: `Assets/Scripts/GangDogMemberVisualAnimator.cs`
- Create: `Assets/Tests/EditMode/GangDogMemberVisualAnimatorTests.cs`

**Interfaces:**
- Consumes: `GangDogAdvancedMember.IsSwinging`、`GangDogAdvancedMember.FacingDirection`，以及根对象的世界坐标变化。
- Produces: `ResolveAnimation(bool advanced, bool swinging, Vector2 direction, bool moving)`，返回 `GangDogLow_*` 或 `GangDogAdvanced_*` 动画状态。

- [ ] **Step 1: 写入失败测试**

```csharp
[Test]
public void ResolveAnimation_AdvancedSwingUsesDirectionalSwing()
{
    CharacterAnimationState state = GangDogMemberVisualAnimator.ResolveAnimation(
        true, true, Vector2.left, false);
    Assert.That(state.Name, Is.EqualTo("GangDogAdvanced_Swing_Left"));
}
```

- [ ] **Step 2: 运行测试确认失败**

运行 Unity EditMode 测试筛选 `RPG.Tests.GangDogMemberVisualAnimatorTests`；预期失败，因为控制器不存在。

- [ ] **Step 3: 实现最小状态选择**

```csharp
string prefix = advanced ? "GangDogAdvanced_" : "GangDogLow_";
string action = advanced && swinging ? "Swing" : moving ? "Walk" : "Idle";
return new CharacterAnimationState(Animator.StringToHash(prefix + action + "_" + suffix),
    prefix + action + "_" + suffix, false);
```

控制器应追踪上帧世界坐标变化以判断移动；高级成员挥棍期间优先显示挥棍状态并采用锁定朝向，低级成员仅显示待机或行走。

- [ ] **Step 4: 运行测试确认通过**

运行 Unity EditMode 测试筛选 `RPG.Tests.GangDogMemberVisualAnimatorTests`；预期所有该类测试通过。

### Task 3: 导入两张角色精灵表、创建动画并写入道路场景

**Files:**
- Create: `Assets/Art/Characters/gang-dog-low-member-character-sheet-pixel-v1.png`
- Create: `Assets/Art/Characters/gang-dog-advanced-member-character-sheet-pixel-v1.png`
- Create: `Assets/Editor/GangDogMemberCharacterInstaller.cs`
- Create: `Assets/Character/GangDogLowMember.controller`
- Create: `Assets/Character/GangDogAdvancedMember.controller`
- Create: `Assets/Character/GangDogLow_Idle_{Down,Up,Right,Left}.anim`
- Create: `Assets/Character/GangDogLow_Walk_{Down,Up,Right,Left}.anim`
- Create: `Assets/Character/GangDogAdvanced_Idle_{Down,Up,Right,Left}.anim`
- Create: `Assets/Character/GangDogAdvanced_Walk_{Down,Up,Right,Left}.anim`
- Create: `Assets/Character/GangDogAdvanced_Swing_{Down,Up,Right,Left}.anim`
- Modify: `Assets/Scenes/RoadToSuburbs1.unity`
- Modify: `Assets/Scenes/RoadToSuburbs2.unity`

**Interfaces:**
- Consumes: `GangDogMemberVisualAnimator`、既有根对象名 `丧家犬低级成员` 与 `丧家犬高级成员`。
- Produces: 每个成员根对象下的 `Gang Dog Member Visual` 子对象，保留旧根对象物理与 AI。

- [ ] **Step 1: 准备精灵表布局**

低级成员使用 4 列×3 行：待机、行走 A、行走 B。高级成员使用 4 列×4 行：待机、行走 A、行走 B、挥棍。每格均为 48×64 像素，列顺序固定为 Down、Up、Right、Left。

- [ ] **Step 2: 生成符合设计的透明像素精灵表**

低级成员采用瘦削弓背、灰褐连帽外套、补丁旧裤、旧运动鞋与拾荒袋；高级成员采用宽肩、暗棕工装、工具袋、旧靴与右手短棍。两张图均保持透明背景，左右表情、衣物与持棍手独立构图。

- [ ] **Step 3: 实现安装器**

安装器把两张纹理设为 Multiple Sprite、32 PPU、Point Filter、无压缩，按固定 48×64 网格切片。为每种角色生成四向待机和两帧循环行走；仅高级成员生成四向挥棍。每个场景对象创建视觉子对象、分配相应 Controller，并禁用旧占位 SpriteRenderer；排序层级继承旧渲染器。

- [ ] **Step 4: 无界面执行安装器并检查场景**

运行 `GangDogMemberCharacterInstaller.Apply`，确认道路（1）的两个低级成员和道路（2）的三个高级成员均有视觉子对象，且根对象的 Rigidbody2D、Collider2D 与行为脚本仍保留。

### Task 4: 场景接入验证

**Files:**
- Create: `Assets/Tests/EditMode/GangDogMemberCharacterSceneTests.cs`

**Interfaces:**
- Consumes: 两张精灵表、两个控制器、两个道路场景。
- Produces: 可重复运行的场景配置验证。

- [ ] **Step 1: 写入场景测试**

测试加载道路（1）与道路（2），断言低级成员拥有 `GangDogLowMember` 与 `Gang Dog Member Visual`，高级成员拥有 `GangDogAdvancedMember` 与视觉子对象；断言视觉子对象包含 Animator、SpriteRenderer 和 `GangDogMemberVisualAnimator`。

- [ ] **Step 2: 运行目标 EditMode 测试**

依次运行 `GangDogAdvancedMemberTests`、`GangDogMemberVisualAnimatorTests` 与 `GangDogMemberCharacterSceneTests`；预期通过。随后以 Unity `-batchmode -quit` 进行无界面编译检查，预期日志中无 `error CS`。
