# 教室静物方形 Sprite 替换设计

## 目标

将 `SampleScene` 中现有的 37 个教室可见静态物件从 `Main Character` 使用的圆形 Sprite 替换为场景对象 `Basic Unit` 使用的方形 Sprite。继续通过各物件已有的 `Transform.m_LocalScale` 形成地面、墙体、黑板、桌椅、窗、门和储物柜等不同尺寸的矩形。

## 范围

- 仅修改 `Assets/Scenes/SampleScene.unity` 中 fileID `2000000000` 至 `2000000452` 所属教室生成节点的 37 个 SpriteRenderer。
- 方形 Sprite 必须取自 `Basic Unit`：fileID `7482667652216324306`，GUID `311925a002f4447b3a28927169b83ea6`。
- `Main Character` 保留圆形 Sprite：fileID `-2413806693520163455`，GUID `a86470a33a6bf42c4b3595704624658b`。
- 不删除、不移动或停用 `Basic Unit`；不修改 `Professor`、角色移动脚本、颜色、排序、层级、位置或缩放。

## 验证

- 修改前，资源检查应报告 37 个教室静物仍引用圆形 Sprite。
- 修改后，37 个教室静物全部引用 `Basic Unit` 方形 Sprite，且不再引用圆形 Sprite。
- `Main Character` 仍引用圆形 Sprite，`Basic Unit` 仍存在并引用方形 Sprite。
- 原教室结构验证、Unity 程序集编译和移动行为测试保持通过。

