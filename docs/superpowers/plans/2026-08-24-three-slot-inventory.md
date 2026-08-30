# Three-Slot Inventory Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 在右上角加入三格物品栏，实现数字键选槽、接触后按 E 拾取 `Library Card`、按 Q 向最近朝向前方丢弃。

**Architecture:** 使用可独立测试的 `InventorySlots<T>` 管理固定容量状态；`PlayerInventory` 处理输入、附近物品和拾取/丢弃；`WorldItem` 管理地面实例；`InventoryHUD` 用 OnGUI 绘制测试阶段界面。`MainCharacterMovement` 作为唯一朝向来源，场景只新增组件与借书卡 Trigger，不创建物品副本。

**Tech Stack:** Unity 2022.3.62f3c1、C#、Unity 2D Physics、Legacy Input、OnGUI、NUnit、Unity Scene YAML、PowerShell。

## Global Constraints

- 物品栏容量固定为3，每格至多一个实例，不堆叠。
- 拾取进入从左到右第一个空槽；满栏不覆盖。
- 初始选择槽位1；Alpha1/2/3 与 Keypad1/2/3 均可选槽。
- E 拾取当前接触物品中距离最近者；Q 丢弃当前槽物品。
- 丢弃位置为玩家位置加归一化最近朝向乘以1.2；初始朝向向下，零输入保留最近方向。
- `Library Card` 复用同一个场景实例；拾取时 inactive，丢弃时重新定位并 active。
- UI 位于右上角，三格横排，当前槽黄色边框，借书卡显示现有蓝色 Sprite 图标。
- 不加入存档、堆叠、物品效果、拾取提示、角色手持模型或鼠标交互。
- 保留现有主角碰撞、20个教室静态碰撞体、12把无碰撞椅子以及隐藏的 `Basic Unit`。
- 项目不是 Git 仓库；使用工作区备份，不执行 commit。

---

### Task 1: 实现可测试的三格槽位核心

**Files:**
- Create: `Assets/Scripts/InventorySlots.cs`
- Create: `Assets/Scripts/InventorySlots.cs.meta`
- Create: `Assets/Tests/EditMode/InventorySlotsTests.cs`
- Create: `Assets/Tests/EditMode/InventorySlotsTests.cs.meta`
- Create: `work/inventory/InventorySlotsHarness.cs`

**Interfaces:**
- Produces: `InventorySlots<T>(int capacity)`、`Capacity`、`SelectedIndex`、`Select(int)`、`TryAdd(T, out int)`、`Get(int)`、`RemoveSelected()`，其中 `T : class`。
- Consumers: `PlayerInventory` 与 `InventoryHUD`。

- [ ] **Step 1: 编写失败的槽位测试**

NUnit 与离线夹具必须覆盖以下字面期望：

```csharp
var slots = new InventorySlots<object>(3);
Assert.That(slots.Capacity, Is.EqualTo(3));
Assert.That(slots.SelectedIndex, Is.Zero);
Assert.That(slots.TryAdd(first, out int firstIndex), Is.True);
Assert.That(firstIndex, Is.Zero);
slots.Select(2);
Assert.That(slots.SelectedIndex, Is.EqualTo(2));
Assert.That(slots.RemoveSelected(), Is.SameAs(third));
```

还要断言有空洞时使用最左空槽，三格全满时 `TryAdd` 返回 false 且内容不变，移除空槽返回 null，越界选择返回 false。

- [ ] **Step 2: 运行 RED**

使用 Unity Mono C# 编译器编译 `InventorySlotsHarness.cs + InventorySlots.cs`。

Expected: 在实现文件不存在时编译失败，明确报告 `InventorySlots` 缺失。

- [ ] **Step 3: 写入最小槽位实现**

实现固定数组，不提供扩容、移动或交换功能：

```csharp
public sealed class InventorySlots<T> where T : class
{
    private readonly T[] items;

    public int Capacity => items.Length;
    public int SelectedIndex { get; private set; }

    public InventorySlots(int capacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        items = new T[capacity];
    }

    public bool Select(int index)
    {
        if (index < 0 || index >= items.Length) return false;
        SelectedIndex = index;
        return true;
    }

    public bool TryAdd(T item, out int slotIndex)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        for (int index = 0; index < items.Length; index++)
        {
            if (items[index] != null) continue;
            items[index] = item;
            slotIndex = index;
            return true;
        }
        slotIndex = -1;
        return false;
    }

    public T Get(int index) => items[index];

    public T RemoveSelected()
    {
        T item = items[SelectedIndex];
        items[SelectedIndex] = null;
        return item;
    }
}
```

- [ ] **Step 4: 运行 GREEN**

Expected: 离线槽位夹具全部通过；Unity EditMode 测试程序集编译通过。

---

### Task 2: 增加朝向、世界物品和玩家交互

**Files:**
- Modify: `Assets/Scripts/MainCharacterMovement.cs`
- Modify: `Assets/Tests/EditMode/MainCharacterMovementTests.cs`
- Create: `Assets/Scripts/WorldItem.cs`
- Create: `Assets/Scripts/WorldItem.cs.meta`
- Create: `Assets/Scripts/PlayerInventory.cs`
- Create: `Assets/Scripts/PlayerInventory.cs.meta`
- Create: `Assets/Tests/EditMode/InventoryInteractionTests.cs`
- Create: `Assets/Tests/EditMode/InventoryInteractionTests.cs.meta`
- Create: `work/inventory/InventoryInteractionHarness.cs`

**Interfaces:**
- `MainCharacterMovement.FacingDirection -> Vector2`，初值 `Vector2.down`。
- `MainCharacterMovement.CalculateFacingDirection(Vector2 current, Vector2 input) -> Vector2`。
- `PlayerInventory.SelectedSlotIndex -> int`、`GetSlotItem(int) -> WorldItem`、`RegisterNearby(WorldItem)`、`UnregisterNearby(WorldItem)`。
- `PlayerInventory.CalculateDropPosition(Vector2 player, Vector2 facing, float distance) -> Vector2`。
- `WorldItem.PickUp()` 与 `WorldItem.Drop(Vector2)`。

- [ ] **Step 1: 编写失败的朝向和丢弃位置测试**

测试字面结果：初始/零朝向回退为下方；输入 `(1, 1)` 得到归一化方向；零输入保留当前方向；玩家 `(2,3)` 朝右、距离 `1.2` 时丢弃点为 `(3.2,3)`。

- [ ] **Step 2: 运行 RED**

Expected: 编译失败，报告 `CalculateFacingDirection`、`FacingDirection`、`PlayerInventory` 或 `CalculateDropPosition` 尚不存在。

- [ ] **Step 3: 实现朝向状态**

在 `MainCharacterMovement.Update` 采样输入后执行 `FacingDirection = CalculateFacingDirection(FacingDirection, input);`。纯函数规则：输入平方长度大于0时返回 `input.normalized`；否则当前方向非零就归一化保留；当前方向也为零时返回 `Vector2.down`。

- [ ] **Step 4: 实现 WorldItem**

使用 `[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]`。`Awake` 缓存 SpriteRenderer；Trigger 进入/离开时在碰撞对象上查找 `PlayerInventory` 并注册/注销。`PickUp` 停用 GameObject；`Drop` 先设置 `transform.position` 再激活。公开只读 `DisplayName`、`Icon` 和 `IconColor` 给 HUD 使用。

- [ ] **Step 5: 实现 PlayerInventory**

创建 `InventorySlots<WorldItem>(3)` 与不重复的附近物品列表。`Update` 依次处理选槽、E、Q。拾取时按世界距离平方选择最近 active 物品，成功加入后先从附近列表移除再停用。丢弃时从当前槽移除，通过 `CalculateDropPosition(transform.position, movement.FacingDirection, 1.2f)` 定位并激活。

- [ ] **Step 6: 运行 GREEN 与移动回归**

Expected: 朝向/丢弃夹具通过；Unity 运行时与 EditMode 测试程序集编译通过；原移动行为 `7/7`、Rigidbody2D/MovePosition 契约 `2/2` 保持通过。

---

### Task 3: 绘制右上角测试物品栏

**Files:**
- Create: `Assets/Scripts/InventoryHUD.cs`
- Create: `Assets/Scripts/InventoryHUD.cs.meta`

**Interfaces:**
- Consumes: 同 GameObject 上的 `PlayerInventory.SelectedSlotIndex` 与 `GetSlotItem(int)`。
- Produces: 右上角三个不可点击的 OnGUI 槽位。

- [ ] **Step 1: 编写 HUD 场景契约检查**

扩展场景验证目标，要求 `Main Character` 包含 `InventoryHUD` MonoBehaviour；脚本程序集必须可编译。

- [ ] **Step 2: 实现最小 HUD**

使用 `[RequireComponent(typeof(PlayerInventory))]`，在 `Awake` 缓存物品栏。`OnGUI` 按 `Screen.width - margin - totalWidth` 计算右上角起点，绘制三格深色半透明背景、槽位数字、黄色选中边框。物品图标通过 Sprite 的 `textureRect` 归一化为 UV，再用 `GUI.DrawTextureWithTexCoords` 和 `IconColor` 绘制。

- [ ] **Step 3: 编译验证**

Expected: RPG.Runtime 零编译错误；HUD 依赖契约通过。

---

### Task 4: 绑定场景中的主角与 Library Card

**Files:**
- Modify: `Assets/Scenes/SampleScene.unity`
- Create: `work/inventory/verify_inventory_scene.ps1`
- Modify: `work/classroom-collision/verify_classroom_collision.ps1`

**Interfaces:**
- Main Character GameObject fileID `1667414357`。
- Library Card GameObject fileID `220157103`。
- PlayerInventory 组件 fileID `2110000000`，脚本 GUID `e14f780d5b0a42c890d7a9b7f0c11202`。
- InventoryHUD 组件 fileID `2110000001`，脚本 GUID `e14f780d5b0a42c890d7a9b7f0c11204`。
- WorldItem 组件 fileID `2110000010`，脚本 GUID `e14f780d5b0a42c890d7a9b7f0c11203`。
- Library Card Trigger BoxCollider2D fileID `2110000011`。

- [ ] **Step 1: 编写失败的场景绑定检查**

验证器断言：主角拥有两个新 MonoBehaviour；借书卡拥有 WorldItem 和 Trigger BoxCollider2D；WorldItem 的 `displayName` 为 `Library Card`；Trigger 本地尺寸 `1 x 1`；Library Card 位置、缩放、Sprite 和颜色不变；所有 fileID 唯一。

- [ ] **Step 2: 运行场景 RED**

Expected: FAIL，报告主角缺少物品栏/HUD，Library Card 缺少 WorldItem/Trigger。

- [ ] **Step 3: 备份并写入组件**

为主角组件列表添加 `2110000000`、`2110000001` 并追加对应 MonoBehaviour 块。为 Library Card 添加 `2110000010`、`2110000011`；BoxCollider2D 为 `m_IsTrigger: 1`、Offset `(0,0)`、Size `(1,1)`、Edge Radius `0`。不添加 Rigidbody2D。

- [ ] **Step 4: 更新碰撞回归检查**

原教室验证继续要求20个非 Trigger BoxCollider2D 精确绑定批准的墙壁/桌子等目标；另外允许且仅允许 Library Card 的一个 Trigger BoxCollider2D。椅子碰撞体仍为0。

- [ ] **Step 5: 执行完整验证**

依次运行槽位夹具、交互夹具、Unity 运行时/测试程序集编译、移动 `7/7`、MovePosition `2/2`、库存场景、教室碰撞、方形 Sprite 和教室结构验证。

Expected: 全部退出码0；静态 BoxCollider2D `20/20`、Library Card Trigger `1/1`、椅子 `0/12`；`Basic Unit` inactive；主角和 Library Card 原有显示与位置不变。

- [ ] **Step 6: 手动 Play Mode 验收说明**

最终交付提示用户打开 Unity：确认右上角三格与黄色选框；数字键切换；接触借书卡后 E 拾取；选中槽后 Q 在最近朝向前方1.2单位丢出；满栏和空槽操作无副作用。

