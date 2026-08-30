# Main Character WASD 移动设计

## 目标

让场景 `Assets/Scenes/SampleScene.unity` 中的 `Main Character` 可通过 WASD 平滑移动：W 向上、A 向左、S 向下、D 向右。

## 范围

- 使用现有旧输入系统的 `Horizontal` 与 `Vertical` 轴。
- 直接修改 `Transform`，不添加 `Rigidbody2D` 或 `Collider2D`。
- 将斜向输入归一化，确保斜向速度不高于单轴速度。
- 移动速度作为 Inspector 可编辑字段，默认值为每秒 5 个 Unity 单位。
- 使用 `Time.deltaTime` 保证移动速度不依赖帧率。

## 结构与数据流

新增 `MainCharacterMovement` 组件并挂载到 `Main Character`。组件在每帧读取水平、垂直输入，构造二维方向向量，将长度限制为 1，再计算 `方向 × 速度 × 帧间隔` 并累加到角色位置。

## 异常与边界

- 无输入时位移为零。
- 同时按下相反方向时，对应轴输入互相抵消。
- 同时按下两个非相反方向时允许斜向移动，但总速度保持不变。
- 本功能不处理碰撞、动画、冲刺、移动边界或按键重映射。

## 验证

- EditMode 测试验证四个方向、无输入、相反输入抵消、斜向限速和帧时间缩放。
- Unity 批处理运行 EditMode 测试并确认全部通过。
- Unity 批处理打开项目并确认脚本、场景无编译或序列化错误。
- 检查场景 YAML，确认脚本组件挂载到 `Main Character`。
