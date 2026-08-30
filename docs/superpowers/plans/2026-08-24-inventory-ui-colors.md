# 物品栏 UI 高可见度配色实施计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 将右上角物品栏槽位背景改为不透明白色，并将当前选中槽位边框改为不透明绿色。

**Architecture:** 保留现有 Canvas、父子层级和 `InventoryHUD` 交互，为 `Inventory Panel` 增加标准 UGUI `Image`，并修改六个槽位 `Image` 的序列化颜色。通过场景校验脚本验证具体对象的颜色和全部既有结构，避免误改场景中的其他 `Image` 或 SpriteRenderer。

**Tech Stack:** Unity 2022.3、Unity YAML 场景、PowerShell 结构校验

## Global Constraints

- `Inventory Panel` 增加标准 UGUI `Image` 并设为 `RGBA(1, 0.82, 0.1, 1)`。
- `Background 1`、`Background 2`、`Background 3` 必须为 `RGBA(1, 1, 1, 1)`。
- `Border 1`、`Border 2`、`Border 3` 必须为 `RGBA(0, 1, 0, 1)`。
- 不修改位置、尺寸、层级、脚本和物品交互逻辑。
- 项目不是 Git 仓库，不执行提交步骤。

---

### Task 1: 用场景结构校验锁定新配色

**Files:**
- Modify: `work/canvas-inventory/verify_canvas_inventory_ui.ps1`
- Test: `work/canvas-inventory/verify_canvas_inventory_ui.ps1`

**Interfaces:**
- Consumes: `D:\RPG project 1\Assets\Scenes\SampleScene.unity`
- Produces: 退出码 `0` 表示 Canvas 结构和目标配色均符合规范；非零表示失败。

- [ ] **Step 1: 更新颜色断言**

将校验器对三个边框的期望颜色改为 `{r: 0, g: 1, b: 0, a: 1}`，对三个背景的期望颜色改为 `{r: 1, g: 1, b: 1, a: 1}`，并保留面板黄色断言。

- [ ] **Step 2: 运行校验并确认 RED**

Run: `powershell -NoProfile -ExecutionPolicy Bypass -File "work\canvas-inventory\verify_canvas_inventory_ui.ps1"`

Expected: `FAIL`，原因是场景仍使用黄色边框和半透明深色背景。

### Task 2: 最小修改场景 UI 与颜色

**Files:**
- Modify: `Assets/Scenes/SampleScene.unity`
- Test: `work/canvas-inventory/verify_canvas_inventory_ui.ps1`

**Interfaces:**
- Consumes: `InventoryHUD.selectionBorders` 对 `Border 1`、`Border 2`、`Border 3` 的引用。
- Produces: 带 `CanvasRenderer` 和黄色 `Image` 的面板、白色槽位背景、绿色选中边框。

- [ ] **Step 1: 添加黄色面板并修改六个 Image 颜色**

为 `Inventory Panel` 添加 CanvasRenderer fileID `2120000012` 与 Image fileID `2120000013`，面板 Image 设为黄色。将 Image fileID `2120000113`、`2120000213`、`2120000313` 设为绿色，将 Image fileID `2120000123`、`2120000223`、`2120000323` 设为白色；不得修改其他字段。`2120000133`、`2120000233`、`2120000333` 是物品图标，不得改动。

- [ ] **Step 2: 运行目标校验并确认 GREEN**

Run: `powershell -NoProfile -ExecutionPolicy Bypass -File "work\canvas-inventory\verify_canvas_inventory_ui.ps1"`

Expected: `CANVAS_INVENTORY_UI_VERIFICATION=PASS`。

- [ ] **Step 3: 运行完整回归验证**

运行运行时编译、EditMode 测试程序集编译、HUD 生命周期、物品栏逻辑、交互、移动、碰撞和全部场景校验。期望全部退出码为 `0` 且无 Unity 残留进程。
