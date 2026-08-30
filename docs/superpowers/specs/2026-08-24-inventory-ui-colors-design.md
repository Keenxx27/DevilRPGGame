# 物品栏 UI 高可见度配色设计

## 目标

提高右上角三格物品栏在游戏画面中的可见度，同时保持现有布局和全部交互不变。

## 视觉规则

- `Inventory Panel` 新增标准 UGUI `Image`，设为不透明黄色：`RGBA(1, 0.82, 0.1, 1)`。
- `Background 1`、`Background 2`、`Background 3` 改为不透明白色：`RGBA(1, 1, 1, 1)`。
- `Border 1`、`Border 2`、`Border 3` 改为不透明绿色：`RGBA(0, 1, 0, 1)`。
- `InventoryHUD` 继续只启用当前选中槽位的边框，因此绿色边框仍随数字键 `1`、`2`、`3` 切换。

## 实现边界

只修改 `Assets/Scenes/SampleScene.unity`：为当前只有 `RectTransform` 的 `Inventory Panel` 增加 `CanvasRenderer` 与黄色 `Image`，并修改六个槽位 `Image` 的颜色字段。不得修改物品栏位置、尺寸、父子层级、脚本、拾取与丢弃逻辑。

## 验证标准

- 场景结构校验确认面板为不透明黄色。
- 三个槽位背景均为不透明白色。
- 三个选中边框均为不透明绿色。
- 现有 Canvas、物品栏、教室碰撞与移动回归校验继续通过。
