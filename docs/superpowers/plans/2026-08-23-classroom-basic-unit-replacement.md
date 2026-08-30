# Classroom Basic Unit Replacement Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 将37个教室静态物件替换为 `Basic Unit` 方形 Sprite，同时保留主角圆形 Sprite 和现有场景行为。

**Architecture:** 只修改 `SampleScene.unity` 的目标 SpriteRenderer 引用。使用一次性 PowerShell 验证脚本按教室生成 fileID 范围检查真实场景序列化结果，并以 RED-GREEN 流程证明替换有效。

**Tech Stack:** Unity 2022.3.62f3c1、Unity Scene YAML、PowerShell、Unity Roslyn 编译器。

## Global Constraints

- 方形 Sprite：fileID `7482667652216324306`，GUID `311925a002f4447b3a28927169b83ea6`。
- 主角圆形 Sprite：fileID `-2413806693520163455`，GUID `a86470a33a6bf42c4b3595704624658b`。
- 仅替换37个教室可见静物的 `m_Sprite` 行。
- 不修改 Transform、颜色、排序、层级、脚本、`Basic Unit` 或 `Professor`。

---

### Task 1: 替换教室静物 Sprite

**Files:**
- Create: `work/classroom-scene/verify_basic_unit_replacement.ps1`
- Modify: `Assets/Scenes/SampleScene.unity`
- Test: `work/classroom-scene/verify_basic_unit_replacement.ps1`

**Interfaces:**
- Consumes: `SampleScene.unity` 中目标 fileID 范围及 `Basic Unit`、`Main Character` 的 SpriteRenderer。
- Produces: 37个使用方形 Sprite 的教室静物，同时保留主角圆形 Sprite。

- [x] **Step 1: 编写失败检查**

  解析场景 YAML，断言目标的37个 SpriteRenderer 全部使用方形引用，并分别断言 `Basic Unit` 与 `Main Character` 的引用未变。

- [x] **Step 2: 验证检查按预期失败**

  Run: `powershell -File work/classroom-scene/verify_basic_unit_replacement.ps1`

  Expected: FAIL，报告目标 SpriteRenderer 仍有37个圆形引用。

- [x] **Step 3: 执行最小替换**

  仅在 fileID `2000000001` 至 `2000000451` 的目标 SpriteRenderer 块内，将圆形 `m_Sprite` 行替换为方形引用。

- [x] **Step 4: 验证替换与回归**

  Run: 资源检查、原教室结构检查、Unity Roslyn 编译及移动行为夹具。

  Expected: 37/37 方形引用、主角圆形引用保留、结构检查通过、移动行为 7/7 通过。

- [x] **Step 5: 记录环境状态**

  确认 Unity 编辑器未运行，并列出本次修改的 Unity Assets 文件。
