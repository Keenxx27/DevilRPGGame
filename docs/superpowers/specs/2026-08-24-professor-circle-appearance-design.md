# 教授 NPC 圆形外观设计

## 目标

让教室讲台上的 `Professor` 与方形静态物件形成明确区别，同时与玩家角色保持颜色差异。

## 场景修改

- 修改场景：`Assets/Scenes/SampleScene.unity`。
- 将 `Professor` 的 `SpriteRenderer.m_Sprite` 从 `Basic Unit` 方形 Sprite 改为 `Main Character` 当前使用的圆形 Sprite。
- 将 `Professor` 的 `SpriteRenderer.m_Color` 设置为中性灰色 `{r: 0.5, g: 0.5, b: 0.5, a: 1}`。
- 保持教授的位置、缩放、旋转、排序层级、父对象和其他组件不变。
- 不修改 `Main Character`、`Basic Unit` 或其他教室物件。

## 验收标准

- `Professor` 使用圆形 Sprite，且不再引用 `Basic Unit` Sprite。
- `Professor` 显示为不透明灰色。
- `Main Character` 仍保持原圆形 Sprite 和原颜色。
- 教室静态物件仍使用 `Basic Unit` 方形 Sprite。
- 现有场景结构验证继续通过。
