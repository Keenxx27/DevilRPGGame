# 丧家犬老大角色美术接入实施计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 为瓦砾堆 Boss 制作并接入四向站立、行走、拔刀冲刺、撞墙硬直的 2D 俯视硬像素角色视觉。

**Architecture:** 根对象 `GangDogBoss` 继续处理物理与 AI；新增视觉子对象用 Animator 接收状态。动画驱动读取 Boss 已有的 `prepareTime`、`chargeTime` 和 `stunTime`，从而不改动抓捕、冲刺或开关机制。

**Tech Stack:** Unity 2022.3.62f1、C#、UnityEditor Animation API、NUnit EditMode、透明 PNG。

## Global Constraints

- 素材始终为透明背景、2D 俯视 RPG 硬像素风；左右方向独立绘制，不镜像。
- 保留全部既有 `GangDogBoss`、`RubbleBossEncounter` 与 `RubbleSwitchPuzzle` 的机制和引用。
- 普通行走、冲刺与硬直的视觉状态必须分别对应既有 AI 时序。

### Task 1: 生成和确认动作精灵表

**Files:**
- Create: `Assets/Art/Characters/gang-dog-boss-character-sheet-pixel-v1.png`

- [ ] 生成透明 2D 俯视硬像素预览，包含 4 向站立、4 向两帧行走、4 向两帧拔刀冲刺、4 向撞墙捂头硬直；人物采用已确认的中年工装、胡茬、浅眉伤与短猎刀设计。
- [ ] 用户确认后，以最近邻规范为精灵表并写入目标路径。

### Task 2: 测试并实现 Boss 视觉状态选择

**Files:**
- Create: `Assets/Tests/EditMode/GangDogBossVisualAnimatorTests.cs`
- Create: `Assets/Scripts/GangDogBossVisualAnimator.cs`
- Modify: `Assets/Scripts/GangDogBoss.cs`

- [ ] 先写失败测试，断言 `ResolveAnimation` 在硬直、冲刺、准备、普通移动和静止时依次返回对应的 `Stun`、`Charge`、`Walk`、`Idle` 状态。
- [ ] 将 `GangDogBoss` 的当前状态以只读属性公开给视觉组件，不改变其数值与 AI 流程。
- [ ] 实现视觉组件，通过根对象位移方向选择四向状态；准备/冲刺使用拔刀冲刺视觉，硬直使用捂头视觉。
- [ ] 重跑目标测试并确认实际结果。

### Task 3: 生成动画资源并接入瓦砾堆

**Files:**
- Create: `Assets/Editor/GangDogBossCharacterInstaller.cs`
- Create: `Assets/Character/GangDogBoss.controller` 与对应 `.anim`
- Modify: `Assets/Scenes/RubbleHeap.unity`

- [ ] 导入精灵表并生成 16 个动作状态的片段和控制器。
- [ ] 在 Boss 根对象下创建视觉子对象，继承排序层级、配置初始站立精灵，停用旧 Renderer。
- [ ] 执行安装器并以 Unity batchmode 编译；记录实际错误或成功结果。
