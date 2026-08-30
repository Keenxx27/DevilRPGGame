# 三格物品栏、拾取与丢弃系统设计

## 目标

为试玩版加入显示在游戏画面右上角的三格初始物品栏。玩家使用数字键 `1`、`2`、`3` 选择当前手持槽位；接触世界物品时按 `E` 拾取；按 `Q` 将当前槽位中的物品丢弃到主角正前方。`Library Card` 是第一件支持完整拾取和丢弃流程的物品。

## 范围

- 物品栏固定三格，不扩容。
- 物品不堆叠，每格至多保存一个世界物品实例。
- 拾取时自动使用从左到右的第一个空槽，不覆盖现有物品。
- 三格已满时按 `E` 无操作，地面物品保持原状。
- 当前槽位为空时按 `Q` 无操作。
- 初始选择第1格；切换槽位不自动使用或丢弃物品。
- 本阶段不实现存档、物品使用效果、拾取提示、角色手持模型、物品数量或拖拽整理。

## 组件设计

### MainCharacterMovement

保留现有 `Rigidbody2D.MovePosition` 移动。新增只读的最近朝向状态：

- 初始朝向为 `Vector2.down`。
- 当当前输入向量非零时，将朝向更新为归一化输入方向。
- 停止移动时保留最近一次有效方向。
- `PlayerInventory` 通过只读属性取得该方向，不重复读取移动输入。

### PlayerInventory

挂载到 `Main Character`，拥有固定长度为3的 `WorldItem` 槽位数组和 `selectedSlotIndex`：

- `Alpha1/Keypad1`、`Alpha2/Keypad2`、`Alpha3/Keypad3` 选择对应槽位。
- `E` 从当前接触范围内选择距离主角最近的可拾取物，并尝试放入第一个空槽。
- 拾取成功后，将物品从附近集合移除并隐藏其 GameObject。
- `Q` 移除当前槽位物品，将其移动到 `playerPosition + facingDirection * 1.2` 后重新激活。
- 对满栏拾取、空槽丢弃和没有附近物品的拾取请求保持无副作用。

核心槽位操作与丢弃位置计算使用不依赖 Unity 帧循环的独立方法，便于 EditMode 测试。

### WorldItem

挂载到 `Library Card`：

- 保存显示名称、SpriteRenderer 与非 Trigger 世界表现。
- 添加 `BoxCollider2D` 并设为 Trigger，用于进入和离开主角拾取范围。
- `OnTriggerEnter2D`/`OnTriggerExit2D` 通过碰撞对象上的 `PlayerInventory` 注册或注销附近物品。
- 拾取时停用整个 GameObject；丢弃时先更新世界位置再激活。
- 同一个世界物品实例在地面和物品栏之间往返，不创建副本或 Prefab。

### InventoryHUD

挂载到 `Main Character`，使用 `OnGUI` 绘制测试阶段界面：

- 位于屏幕右上角，横向排列三格。
- 每格显示数字 `1`、`2`、`3`。
- 当前选中槽位绘制黄色边框。
- 空槽使用半透明深色背景。
- 有物品时读取 `WorldItem` 的 SpriteRenderer，将 Sprite 纹理及其颜色绘制为槽位图标。
- 界面仅展示状态，不接收鼠标交互。

## Library Card 场景配置

保留现有 `Library Card` GameObject、Sprite、颜色、位置 `(-4.86, 1.94)` 与缩放。新增：

- `WorldItem` 组件，显示名称为 `Library Card`。
- `BoxCollider2D`，`Is Trigger = true`，本地尺寸 `1 x 1`。

它不添加 Rigidbody2D；主角已有 Dynamic Rigidbody2D，因此 Trigger 事件能够产生。

## 交互流程

```text
主角进入 Library Card Trigger
        ↓
WorldItem 注册到 PlayerInventory 附近集合
        ↓
按 E → 选择最近物品 → 查找第一个空槽
        ↓
成功：槽位保存引用，Library Card inactive
失败：物品和槽位均不变化
        ↓
按 1/2/3 切换 selectedSlotIndex
        ↓
按 Q → 当前槽位引用清空
        ↓
物品移动到最近朝向前方1.2单位并重新 active
```

## 测试与验收

- 槽位测试：初始选择第1格；数字键映射到正确索引；物品进入第一个空槽；满栏拒绝且不覆盖；空槽丢弃无变化。
- 朝向测试：初始向下；有效单轴和斜向输入更新为归一化方向；零输入保留最近方向。
- 丢弃位置测试：位置精确等于玩家位置加归一化朝向乘以 `1.2`。
- 附近物品测试：同时接触多个物品时选择距离最近者。
- 场景测试：`Main Character` 绑定 `PlayerInventory` 和 `InventoryHUD`；`Library Card` 绑定 `WorldItem` 与 Trigger BoxCollider2D；原碰撞体数量和 `Basic Unit` 隐藏状态不变。
- 回归测试：主角移动、Rigidbody2D/MovePosition 契约、三格物品栏边界和现有教室结构继续通过。
- 手动 Play Mode 验收：右上角出现三格；按键高亮正确；接触借书卡后 `E` 可拾取；选中对应槽位后 `Q` 在主角最近朝向前方丢出；满栏和空槽操作不产生异常。

