# Standard Canvas Inventory UI Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 用保存在 `SampleScene` 层级中的标准 Unity UGUI Canvas 替换不可见的 `OnGUI` 三格物品栏，同时保持现有库存行为不变。

**Architecture:** `PlayerInventory` 继续独占库存状态；`InventoryHUD` 改为只读取库存并刷新三个 UGUI `Image` 边框与三个物品图标。Canvas、Panel、槽位和数字标签全部序列化在场景中，HUD 组件挂在 Canvas 上并通过显式引用连接主角与 UI。

**Tech Stack:** Unity 2022.3.62f3c1、C#、Unity UGUI 1.0.0、NUnit、Unity Scene YAML、PowerShell。

## Global Constraints

- Canvas 必须是 `Screen Space - Overlay`，CanvasScaler 使用 `1920×1080`、Match `0.5`。
- Panel 锚定右上角，右/上边距各16像素；三个槽位均为 `64×64`，间距8像素，总宽208像素。
- 当前槽位显示黄色边框；空槽隐藏图标；有物品时使用 `WorldItem.Icon` 与 `IconColor`。
- `Inventory Canvas`、Panel、三个槽位及其子节点必须在 Hierarchy 中可见和编辑。
- 不创建 EventSystem，不加入鼠标交互、动画、堆叠、提示或新美术资源。
- 不改变 `PlayerInventory`、`WorldItem`、角色移动、拾取、丢弃和碰撞行为。
- 项目不是 Git 仓库；修改前备份脚本、asmdef、场景和验证器，不执行 commit。

---

### Task 1: 将 InventoryHUD 改为可测试的 UGUI 控制器

**Files:**
- Modify: `Assets/Scripts/InventoryHUD.cs`
- Modify: `Assets/Scripts/RPG.Runtime.asmdef`
- Create: `Assets/Tests/EditMode/InventoryHUDTests.cs`
- Create: `Assets/Tests/EditMode/InventoryHUDTests.cs.meta`
- Modify: `Assets/Tests/EditMode/RPG.EditModeTests.asmdef`

**Interfaces:**
- Consumes: `PlayerInventory.SelectedSlotIndex`、`PlayerInventory.GetSlotItem(int)`、`WorldItem.Icon`、`WorldItem.IconColor`。
- Produces: `InventoryHUD.Refresh() -> bool`；场景序列化字段 `inventory`、`selectionBorders`、`itemIcons`。

- [ ] **Step 1: 写入失败的 HUD 行为测试**

在 `InventoryHUDTests.cs` 中使用真实 GameObject、`PlayerInventory`、`WorldItem` 与 `UnityEngine.UI.Image`。用测试内反射设置私有序列化字段，避免给生产类增加仅供测试使用的配置 API。

必须覆盖三个独立行为：

```csharp
[Test]
public void Refresh_SelectsOnlyCurrentBorderAndHidesEmptyIcons()
{
    InventoryHUD hud = CreateConfiguredHud(out PlayerInventory inventory,
        out Image[] borders, out Image[] icons);

    Assert.That(hud.Refresh(), Is.True);
    Assert.That(borders[0].enabled, Is.True);
    Assert.That(borders[1].enabled, Is.False);
    Assert.That(borders[2].enabled, Is.False);
    Assert.That(icons, Has.All.Matches<Image>(icon => !icon.enabled));
}
```

```csharp
[Test]
public void Refresh_ShowsPickedUpItemsSpriteAndColor()
{
    InventoryHUD hud = CreateConfiguredHud(out PlayerInventory inventory,
        out _, out Image[] icons);
    WorldItem card = CreateItemWithSprite(Color.blue);
    inventory.RegisterNearby(card);
    inventory.gameObject.SendMessage("TryPickUpNearest");

    hud.Refresh();

    Assert.That(icons[0].enabled, Is.True);
    Assert.That(icons[0].sprite, Is.SameAs(card.Icon));
    Assert.That(icons[0].color, Is.EqualTo(Color.blue));
}
```

第三项通过反射取得 `PlayerInventory` 的 `slots`，执行 `Select(2)`，断言刷新后只有第3个边框启用。

第四项创建未配置引用的 HUD，先用 `LogAssert.Expect(LogType.Error, "InventoryHUD requires one inventory, three borders, and three icons.")` 捕获错误，再断言 `Refresh()` 返回 false；连续第二次调用不得重复记录错误。每个测试销毁创建的 GameObject、Sprite 和 Texture2D。

- [ ] **Step 2: 运行 RED**

使用 Unity Roslyn 编译器和现有 `RPG.Runtime.rsp` / `RPG.EditModeTests.rsp`，将新测试加入测试程序集。

Expected: 编译失败，明确报告 `InventoryHUD.Refresh` 不存在，或 `InventoryHUD` 尚未引用 `UnityEngine.UI.Image`。

- [ ] **Step 3: 为 asmdef 添加 UGUI 引用**

将运行时 asmdef 改为：

```json
{
  "name": "RPG.Runtime",
  "rootNamespace": "RPG",
  "references": [
    "UnityEngine.UI"
  ]
}
```

在 `RPG.EditModeTests.asmdef` 的 `references` 中保留已有项，并追加 `UnityEngine.UI`。

- [ ] **Step 4: 写入最小 InventoryHUD 实现**

删除 `[RequireComponent(typeof(PlayerInventory))]`、所有 `OnGUI` 布局常量与绘制方法，改为：

```csharp
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class InventoryHUD : MonoBehaviour
    {
        private const int SlotCount = 3;

        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private Image[] selectionBorders;
        [SerializeField] private Image[] itemIcons;

        private bool configurationErrorReported;

        private void Awake()
        {
            Refresh();
        }

        private void LateUpdate()
        {
            Refresh();
        }

        public bool Refresh()
        {
            if (!HasValidConfiguration())
            {
                if (!configurationErrorReported)
                {
                    Debug.LogError("InventoryHUD requires one inventory, three borders, and three icons.", this);
                    configurationErrorReported = true;
                }
                return false;
            }

            for (int index = 0; index < SlotCount; index++)
            {
                selectionBorders[index].enabled = inventory.SelectedSlotIndex == index;
                WorldItem item = inventory.GetSlotItem(index);
                itemIcons[index].enabled = item != null && item.Icon != null;
                if (itemIcons[index].enabled)
                {
                    itemIcons[index].sprite = item.Icon;
                    itemIcons[index].color = item.IconColor;
                }
            }

            return true;
        }

        private bool HasValidConfiguration()
        {
            if (inventory == null || selectionBorders == null || itemIcons == null
                || selectionBorders.Length != SlotCount || itemIcons.Length != SlotCount)
            {
                return false;
            }

            for (int index = 0; index < SlotCount; index++)
            {
                if (selectionBorders[index] == null || itemIcons[index] == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
```

- [ ] **Step 5: 运行 GREEN**

重新编译 `RPG.Runtime` 与 `RPG.EditModeTests`。

Expected: 两个程序集零错误；HUD 行为测试源码全部编译。若 Unity Test Runner 可启动，再运行 EditMode 测试；否则保留离线编译证据并在最终验收中明确人工 Play Mode 步骤。

---

### Task 2: 在 SampleScene 写入标准 Canvas 层级

**Files:**
- Modify: `Assets/Scenes/SampleScene.unity`
- Create: `work/canvas-inventory/verify_canvas_inventory_ui.ps1`
- Modify: `work/inventory/verify_inventory_scene.ps1`
- Modify: `work/classroom-scene/verify_classroom_scene.ps1`

**Interfaces:**
- Consumes: InventoryHUD 脚本 GUID `e14f780d5b0a42c890d7a9b7f0c11204`、PlayerInventory 组件 fileID `2110000000`。
- Produces: `Inventory Canvas` 根对象及三个可序列化 UGUI 槽位。

**固定 fileID：**

| 对象 | GameObject | RectTransform | 其他组件 |
|---|---:|---:|---|
| Inventory Canvas | 2120000000 | 2120000001 | Canvas 2120000002；CanvasScaler 2120000003；GraphicRaycaster 2120000004；InventoryHUD 2110000001 |
| Inventory Panel | 2120000010 | 2120000011 | 无 |
| Slot 1 | 2120000100 | 2120000101 | 无 |
| Slot 1 Border | 2120000110 | 2120000111 | CanvasRenderer 2120000112；Image 2120000113 |
| Slot 1 Background | 2120000120 | 2120000121 | CanvasRenderer 2120000122；Image 2120000123 |
| Slot 1 Icon | 2120000130 | 2120000131 | CanvasRenderer 2120000132；Image 2120000133 |
| Slot 1 Label | 2120000140 | 2120000141 | CanvasRenderer 2120000142；Text 2120000143 |
| Slot 2 | 2120000200 | 2120000201 | 子组件沿用 2120000210–2120000243 的同一十位布局 |
| Slot 3 | 2120000300 | 2120000301 | 子组件沿用 2120000310–2120000343 的同一十位布局 |

Slot 2 精确 ID：Border `2120000210/211/212/213`，Background `2120000220/221/222/223`，Icon `2120000230/231/232/233`，Label `2120000240/241/242/243`。Slot 3 分别为 `2120000310–313`、`2120000320–323`、`2120000330–333`、`2120000340–343`。

UGUI 脚本 GUID：Image `fe87c0e1cc204ed48ad3b37840f39efc`；Text `5f7201a12d95ffc409449d95f23cf332`；CanvasScaler `0cd44c1031e13a943bb63640046fad76`；GraphicRaycaster `dc42784cf147c0c48a680349fa168899`。

- [ ] **Step 1: 写入失败的 Canvas 场景契约**

`verify_canvas_inventory_ui.ps1` 必须解析真实 Unity 对象块并断言：

- Main Character 的组件列表不含 `2110000001`，但保留 `2110000000`。
- Canvas GameObject 绑定上述5个组件，Layer 为5，名称为 `Inventory Canvas`。
- Canvas `m_RenderMode: 0`、`m_SortingOrder: 100`。
- CanvasScaler `m_UiScaleMode: 1`、`m_ReferenceResolution: {x: 1920, y: 1080}`、`m_MatchWidthOrHeight: 0.5`。
- HUD 组件绑定 Canvas，且序列化引用：

```yaml
inventory: {fileID: 2110000000}
selectionBorders:
- {fileID: 2120000113}
- {fileID: 2120000213}
- {fileID: 2120000313}
itemIcons:
- {fileID: 2120000133}
- {fileID: 2120000233}
- {fileID: 2120000333}
```

- Panel RectTransform 锚点与轴心都是右上角，`m_AnchoredPosition: {x: -16, y: -16}`、`m_SizeDelta: {x: 208, y: 64}`。
- 三个 Slot 尺寸均为64×64，位置 X 分别为0、72、144。
- 3个 Border、3个 Background、3个 Icon、3个 Text 精确存在；Label 文本依次为1、2、3。
- 第1个 Border 初始 enabled，其余 Border disabled；3个 Icon 初始 disabled且 `m_PreserveAspect: 1`。
- 没有名为 `EventSystem` 的 GameObject；所有新增 fileID 唯一。

- [ ] **Step 2: 运行场景 RED**

Run: `work/canvas-inventory/verify_canvas_inventory_ui.ps1`

Expected: FAIL，报告缺少 `Inventory Canvas`、Panel、槽位和 HUD Canvas 绑定。

- [ ] **Step 3: 备份并修改场景**

备份 `SampleScene.unity` 到 `work/canvas-inventory/backup/SampleScene.before-canvas-ui.unity`。

场景修改必须严格限于：

1. 从 Main Character 组件列表移除 `2110000001`。
2. 将 MonoBehaviour `2110000001` 的 `m_GameObject` 改为 `2120000000`，并写入库存、边框和图标引用。
3. 追加上表中的 Canvas/Panel/Slot UGUI 对象块。
4. SceneRoots 在现有5个根 Transform 后追加 `{fileID: 2120000001}`。

外观序列化值：Border 颜色 `{r: 1, g: 0.82, b: 0.1, a: 1}`；Background `{r: 0.08, g: 0.08, b: 0.08, a: 0.65}`；图标初始白色、inactive by `Image.m_Enabled: 0`；所有 Graphic 的 `m_RaycastTarget: 0`。Border 填满64×64，Background 四边各内缩3像素，Icon 四边各内缩10像素。

数字标签使用内置 Arial：`m_Font: {fileID: 12800000, guid: 0000000000000000e000000000000000, type: 0}`，字号14、白色、文本分别为1/2/3，RectTransform 位于槽位左上角5像素处。

- [ ] **Step 4: 更新已有场景回归检查**

`verify_inventory_scene.ps1` 改为断言 InventoryHUD 绑定 Canvas 而非 Main Character，并调用或等价覆盖 Canvas 引用检查。

`verify_classroom_scene.ps1` 将 SceneRoots 期望从5个更新为6个，精确顺序：

```text
519420032,619394802,2000000002,1424282803,220157105,2120000001
```

教室可见 SpriteRenderer 数量、方形 Sprite 绑定和世界物件层级断言不得改变。

- [ ] **Step 5: 运行场景 GREEN**

Expected: Canvas、库存场景、教室结构验证全部退出码0；无重复 fileID；Main Character 与 Library Card 原数据保持不变。

---

### Task 3: 完整编译、回归与运行验收

**Files:**
- Verify: `Assets/Scripts/*.cs`
- Verify: `Assets/Tests/EditMode/*.cs`
- Verify: `Assets/Scenes/SampleScene.unity`
- Verify: `work/**/verify_*.ps1`

**Interfaces:**
- Consumes: Task 1 的 UGUI HUD 与 Task 2 的场景引用。
- Produces: 可由用户在 Game 窗口直接验收的标准 Canvas 三格物品栏。

- [ ] **Step 1: 编译运行时与测试程序集**

使用 Unity 2022.3.62f3c1 的 Roslyn `csc.dll` 和现有 rsp 编译：

- `RPG.Runtime` 加入全部 `Assets/Scripts/*.cs`。
- `RPG.EditModeTests` 加入 `InventorySlotsTests.cs`、`InventoryInteractionTests.cs`、`InventoryHUDTests.cs`。

Expected: 两个程序集零编译错误。

- [ ] **Step 2: 运行全部离线夹具与场景验证**

依次执行：

1. InventorySlots `4/4`。
2. InventoryInteraction `5/5`。
3. Movement `7/7`。
4. MovementPhysicsContract `2/2`。
5. `verify_canvas_inventory_ui.ps1`。
6. `verify_inventory_scene.ps1`。
7. `verify_classroom_collision.ps1`。
8. `verify_basic_unit_replacement.ps1`。
9. `verify_classroom_scene.ps1`。

Expected: 全部退出码0；20个教室非 Trigger BoxCollider2D、1个 Library Card Trigger、12把无碰撞椅子、隐藏 Basic Unit 均保持不变。

- [ ] **Step 3: 检查故障修复边界**

静态检查必须确认 `InventoryHUD.cs` 不再包含 `OnGUI`、`GUI.`、`Screen.width` 或 `Texture2D.whiteTexture`，并包含 `UnityEngine.UI` 与 `Refresh()`。

场景检查必须确认存在标准 `Canvas` classID 223、`RectTransform` classID 224、UGUI Image/Text 组件，并且 HUD 不再依赖与 PlayerInventory 同 GameObject。

- [ ] **Step 4: 手动 Play Mode 验收说明**

最终交付提示用户打开 Unity，并验证：

1. Hierarchy 中可见 `Inventory Canvas` 完整层级。
2. Game 窗口右上角始终显示三格。
3. 初始选中槽位1，数字键切换黄色边框。
4. 接触 Library Card 后按 E，蓝色图标进入最左空槽。
5. 选中该槽按 Q，图标消失且同一张卡在角色前方出现。

若 Unity 无界面 Test Runner 再次停在初始化前，应安全终止本次启动的进程，不宣称已执行 Unity 测试；以程序集编译、场景契约和用户 Play Mode 验收作为明确边界。
