# 归墟入口调查剧情实现计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 为归墟入口增加首次入场独白、可重复的苔藓与晶体调查，以及通道调查后传送至荧光菌洞的流程。

**Architecture:** 新增一个专职 `AbyssEntranceInteraction` 组件，提供三类调查行为并监听入场下落完成事件。场景生成器只负责把组件和调查点绑定到既有物体，不改变美术子物体、碰撞墙或出生点。

**Tech Stack:** Unity 2022.3、C#、现有 `DialogueController`、`InvestigationPoint`、`MapTransitionService`。

## Global Constraints

- 不修改 `ScenesCopy` 或 `Scenes Copy`。
- 不新增测试；以 Unity 无界面编译和源码/场景配置检查验证。
- 调查文本须逐字采用用户提供的中文文本。

---

### Task 1: 归墟入口交互组件与场景绑定

**Files:**
- Create: `Assets/Scripts/AbyssEntranceInteraction.cs`
- Modify: `Assets/Scripts/InvestigationPoint.cs`
- Modify: `Assets/Scripts/AbyssEntranceDropIn.cs`
- Modify: `Assets/Editor/SchoolLibraryInstaller.cs`

**Interfaces:**
- Consumes: `DialogueController.StartDialogue(DialoguePage[], Action)`、`MapTransitionService.RequestTransition(string, string)`。
- Produces: `AbyssEntranceInteraction : IInvestigationHandler`，处理 `AbyssGlowingMoss`、`AbyssWarmCrystal` 与 `AbyssCavePassage`。

- [ ] **Step 1: 写出失败条件**

确认当前 `InvestigationKind` 不包含归墟入口三种调查类型，且通道仅由 `MapTransitionPoint` 靠近自动传送。

- [ ] **Step 2: 实现最小组件**

新增枚举项和组件：下落结束后只播放一次三页入场独白；苔藓和晶体调查每次各播放一页；通道调查播放三页并在对话完成回调中调用：

```csharp
transition.RequestTransition("FluorescentMushroomCave", "FluorescentMushroomCaveFromAbyss");
```

- [ ] **Step 3: 绑定场景生成器**

在归墟入口创建流程中添加组件、为四处苔藓与三处晶体创建调查点，并移除通道的 `MapTransitionPoint`，改为通道调查点。

- [ ] **Step 4: 验证**

用 Unity `-batchmode -quit` 编译项目，并检查源代码中三组台词及调查点配置。
