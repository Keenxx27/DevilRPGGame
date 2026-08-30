# 标准 Canvas 物品栏 UI 设计

## 目标

将当前通过 `OnGUI` 即时绘制的三格物品栏替换为保存在 `SampleScene` 层级中的标准 Unity UGUI Canvas。物品栏在编辑状态下可从 Hierarchy 查看和调整，并在运行时稳定显示于 Game 窗口右上角。

## 范围

- 只替换物品栏的显示层，不改变拾取、丢弃、选槽和角色朝向逻辑。
- 保留三格容量、初始选择槽位 1、数字键与小键盘 `1/2/3` 选槽。
- 保留 `E` 拾取最近物品、`Q` 向最近移动方向前方 1.2 单位丢弃。
- 保留 `Library Card` 的蓝色 Sprite 图标。
- 不加入鼠标交互、拖拽、堆叠、提示文字、物品数量或动画。

## 场景层级

在 `SampleScene` 新增一个根对象 `Inventory Canvas`：

```text
Inventory Canvas
└── Inventory Panel
    ├── Slot 1
    │   ├── Selection Border
    │   ├── Background
    │   ├── Item Icon
    │   └── Number Label
    ├── Slot 2
    │   ├── Selection Border
    │   ├── Background
    │   ├── Item Icon
    │   └── Number Label
    └── Slot 3
        ├── Selection Border
        ├── Background
        ├── Item Icon
        └── Number Label
```

`Inventory Canvas` 包含：

- `Canvas`，Render Mode 为 `Screen Space - Overlay`。
- `CanvasScaler`，Scale Mode 为 `Scale With Screen Size`，Reference Resolution 为 `1920×1080`，Match 为 `0.5`。
- `GraphicRaycaster`，保留标准 UGUI 结构；物品栏自身不接收点击。
- `InventoryHUD`，引用主角物品栏与三个槽位的边框、背景和图标。

不创建 `EventSystem`，因为界面没有鼠标、触控或导航交互。

## 布局与外观

- `Inventory Panel` 锚定右上角，距右侧和顶部各 16 像素。
- 三个槽位横向排列，每格 `64×64` 像素，间距 8 像素。
- Panel 总尺寸为 `208×64` 像素。
- 槽位背景为深色半透明，颜色与现有测试 UI 接近。
- 当前槽位显示黄色 3 像素视觉边框。
- 数字标签 `1`、`2`、`3` 固定在每格左上角。
- 物品图标位于槽位内部并保留 Sprite 宽高比例。
- 空槽隐藏 `Item Icon`，不显示占位图。

## 脚本结构与数据流

`PlayerInventory` 继续作为唯一库存状态来源，不做行为变更。

`InventoryHUD` 不再使用 `OnGUI`。它改为标准 UGUI 控制器：

- 序列化引用一个 `PlayerInventory`。
- 序列化引用三个选择边框和三个 `Image` 图标。
- 每帧读取 `SelectedSlotIndex` 与 `GetSlotItem(index)`。
- 只有当前槽位启用黄色边框。
- 空槽禁用图标；有物品时设置图标 Sprite、颜色并启用。
- Canvas 引用缺失或数组长度不是 3 时，组件记录明确错误并停止刷新，避免连续空引用异常。

`InventoryHUD` 从 `Main Character` 移到 `Inventory Canvas`。主角仅保留 `PlayerInventory`。

## 故障修复依据

旧实现的脚本和场景绑定存在，Unity 日志也没有编译错误，但显示完全依赖 `OnGUI` 回调，Hierarchy 中没有任何可检查的 UI 对象。本次修改用可序列化、可见的 Canvas 层级替换这一显示边界，使布局、组件启用状态和引用都能在编辑器中直接检查。

## 测试与验收

自动检查必须覆盖：

- 场景存在且只存在一个 `Inventory Canvas` 根对象。
- Canvas Render Mode、CanvasScaler 参考分辨率及右上角锚点正确。
- 三个槽位、三个数字标签、三个边框和三个图标均存在。
- `InventoryHUD` 绑定 `Main Character` 的 `PlayerInventory` 与全部 UI 引用。
- `Main Character` 不再挂载 `InventoryHUD`。
- `InventoryHUD` 运行时程序集和 EditMode 测试程序集可编译。
- HUD 刷新逻辑测试覆盖选中边框、空槽隐藏和有物品显示。
- 原物品栏、移动、碰撞、教室结构和 `Library Card` 场景检查保持通过。

手动 Play Mode 验收：

1. Game 窗口右上角始终显示三格物品栏。
2. 初始黄色边框位于槽位 1。
3. 数字键切换黄色边框。
4. 拾取借书卡后，对应槽位显示蓝色图标。
5. 丢弃后图标消失，地面借书卡在角色前方重新出现。

## 非目标

- 不制作美术 Sprite、九宫格边框或字体资源。
- 不把库存状态搬进 UI 脚本。
- 不改变 Main Character、Library Card 或教室物件的世界坐标。
- 不创建预制体，不加入跨场景持久化。
