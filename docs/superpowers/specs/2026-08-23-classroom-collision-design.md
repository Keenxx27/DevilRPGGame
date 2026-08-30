# 教室角色移动与静物碰撞设计

## 目标

在保留平滑 WASD 操作和 `Main Character` 圆形外观的前提下，将主角移动改为 Unity 2D 物理位移，使主角不能穿过教室墙壁、课桌椅及指定大型静物。同时隐藏场景中的 `Basic Unit` 参考对象。

## 移动架构

- `MainCharacterMovement.Update` 只读取 Horizontal/Vertical 输入并保存为当前输入向量。
- `MainCharacterMovement.FixedUpdate` 使用 `Rigidbody2D.MovePosition` 执行位移。
- 位移继续复用 `CalculateDelta`：输入先限制到单位长度，再乘以 `moveSpeed` 和 `Time.fixedDeltaTime`，保持斜向速度与单轴速度一致。
- 脚本通过 `RequireComponent(typeof(Rigidbody2D))` 声明依赖，并在 `Awake` 中缓存组件。

## 主角物理配置

`Main Character` 添加以下组件：

- `Rigidbody2D`
  - Body Type：Dynamic
  - Gravity Scale：0
  - Interpolate：Interpolate
  - Collision Detection：Continuous
  - Constraints：Freeze Rotation Z
- `CircleCollider2D`
  - 非 Trigger
  - 以当前圆形 Sprite 为边界，半径采用 0.5 个本地单位

Dynamic Rigidbody2D 会对静态 BoxCollider2D 产生物理解算；零重力与冻结旋转保证俯视角移动不会下落或因碰撞旋转。

## 静态碰撞范围

以下20个可见静物各添加一个非 Trigger `BoxCollider2D`，碰撞体使用对象的本地单位尺寸 `1 x 1`，由现有 Transform 缩放同步形成实际矩形范围：

- 四面墙：4个
- 学生桌：12个
- 教师桌：1个
- 讲台：1个
- 储物柜：1个
- 门：1个

12把学生椅不添加碰撞体，使角色可以经过椅子并保留原出生点。地板、黑板和三扇窗也不单独添加碰撞体；它们作为背景或位于墙体覆盖范围内，不增加新的阻挡面。

## Basic Unit

保留 `Basic Unit` 对象及其 Sprite 引用，将 GameObject 的 `m_IsActive` 从 `1` 改为 `0`。它不参与显示、移动或碰撞，但仍可作为方形 Sprite 的场景参考对象。

## 测试与验收

- TDD RED：验证缺少主角 Rigidbody2D、CircleCollider2D、20个目标 BoxCollider2D，或存在任何椅子碰撞体时失败；同时检查 `Basic Unit` 是否仍为激活状态。
- 脚本测试：四方向位移、无输入、斜向限速和 fixedDeltaTime 缩放继续通过。
- 场景测试：主角物理参数与组件绑定正确；20个目标静物各有且仅有一个 BoxCollider2D；12把椅子及其他非目标物件无碰撞体；`Basic Unit` 保留但 inactive。
- 回归检查：37个教室静物继续使用方形 Sprite，主角继续使用圆形 Sprite，场景层级、位置、颜色和排序不变。
- 编辑器手动验收：进入 Play Mode 后，主角可沿墙和桌椅边缘滑动，不能穿透目标静物，且画面中不显示 `Basic Unit`。
