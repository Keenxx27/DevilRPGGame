# 学校图书馆地图设计规格

## 1. 目标

在现有学校场景体系中新增一张可探索的学校图书馆地图。玩家可以从学校走廊的一扇门进入图书馆，并通过图书馆出口返回同一扇走廊门前。

本阶段仅实现地图、家具陈设、阻挡碰撞和双向场景切换。暂不加入 NPC、对话、调查点、借阅功能、剧情事件或可拾取物。

## 2. 入口选择

- 将 `SchoolCorridor` 场景中距离教室最近的 `Decorative Door 1` 改造为 `Library Door`。
- `Decorative Door 2` 和 `Decorative Door 3` 保持装饰门状态，不添加交互或地图切换。
- 玩家靠近 `Library Door` 后按 E 开门；门完全打开并穿过门口后进入图书馆。

## 3. 图书馆场景

- 新场景路径：`Assets/Scenes/SchoolLibrary.unity`。
- 场景采用与 `SchoolClassroom`、`SchoolCorridor` 一致的 Basic Unit 色块风格。
- 场景放置在与教室、走廊错开的世界坐标区域，防止 additive 加载后地图相互重叠或被同一相机同时看到。
- 图书馆不创建新的玩家、主相机、对话 UI 或物品栏；继续使用现有持久对象。
- 将 `SchoolLibrary` 加入 Unity Build Settings。

## 4. 地图布局

图书馆为约 18×12 世界单位的单层矩形房间。

### 4.1 出入口

- 图书馆出口位于房间下方中央。
- 出口设置可开合的 `Library Exit Door`。
- 玩家必须按 E 打开门，并在门完全打开后穿过传送区域返回走廊。
- 门内侧保留足够空间，确保出生点不会与门、墙壁或家具碰撞。

### 4.2 家具区域

- 左下区域：L 形借阅柜台，仅作为不可交互陈设。
- 上侧墙边：连续靠墙书架。
- 左侧和右侧墙边：靠墙书架，并避开入口通道。
- 中央区域：两组阅读桌椅。
- 房间中央保留横向与纵向通道，使玩家能够绕行所有家具并到达房间主要区域。

### 4.3 临时配色

- 地板：浅棕色。
- 墙体：米色。
- 书架与桌面：深棕色。
- 借阅柜台：中深棕色。
- 椅子：暗红色。

所有视觉对象使用 Basic Unit 组合，并保持独立、语义明确的对象名称，方便以后替换正式美术资源或添加交互。

## 5. 碰撞规则

- 四周墙体使用 `BoxCollider2D` 阻挡玩家。
- 书架、借阅柜台和阅读桌使用 `BoxCollider2D` 阻挡玩家。
- 图书馆椅子是固定陈设，不添加 `WorldItem`、`ThrowableChair` 或教室解谜组件。
- 椅子默认不设置阻挡碰撞，以减少狭窄区域卡位，并与项目现有简化家具表现保持一致。
- 门口、出生点、中央横向通道和中央纵向通道不得被碰撞体封死。

## 6. 双向地图切换

沿用现有组件，不新增地图切换架构：

- `DoorController`
- `DoorExitPortal`
- `MapTransitionService`
- `MapSpawnPoint`

### 6.1 走廊到图书馆

- 走廊门：`Library Door`。
- 目标场景：`SchoolLibrary`。
- 目标出生点 ID：`LibraryFromCorridor`。
- 图书馆出生点放在出口门内侧，面向可探索区域。
- 图书馆相机目标位置与图书馆房间中心对齐。

### 6.2 图书馆到走廊

- 图书馆门：`Library Exit Door`。
- 目标场景：`SchoolCorridor`。
- 目标出生点 ID：`CorridorFromLibrary`。
- 走廊出生点放在 `Library Door` 的走廊侧，且不与门板或墙体重叠。
- 走廊相机目标位置沿用当前走廊视野设置。

### 6.3 场景加载约束

- 使用现有 additive 加载行为。
- 若目标场景已加载，`MapTransitionService` 直接查找对应出生点并移动玩家和相机。
- 两个出生点 ID 必须在所有已加载场景中保持唯一。

## 7. 明确排除范围

本阶段不实现：

- 图书管理员或其他 NPC。
- 借书、还书或柜台交互。
- 可调查书架、桌椅或其他调查点。
- 图书馆剧情、对话或任务状态。
- 可拾取、投掷或消耗物品。
- 正式贴图、动画或音效。
- 教室花盆、椅子投掷或借书卡解谜组件在图书馆中的复用。

## 8. 验收标准

### 8.1 场景结构

- `SchoolLibrary.unity` 能够无异常加载。
- 场景包含地板、四周墙体、三侧书架、L 形借阅柜台和两组阅读桌椅。
- 场景包含 `Library Exit Door`、`LibraryFromCorridor` 和必要的出口传送区域。
- 图书馆不包含新的玩家、主相机、对话 UI 或物品栏。
- 图书馆不包含 `WorldItem`、`ThrowableChair`、`FlowerpotBreakable` 或 `ClassroomLibraryCardPuzzle`。

### 8.2 走廊改造

- `Decorative Door 1` 已改名并改造为可操作的 `Library Door`。
- `Decorative Door 2` 和 `Decorative Door 3` 保持不变。
- 走廊包含唯一的 `CorridorFromLibrary` 出生点。

### 8.3 切换配置

- 走廊入口的目标场景和出生点分别为 `SchoolLibrary`、`LibraryFromCorridor`。
- 图书馆出口的目标场景和出生点分别为 `SchoolCorridor`、`CorridorFromLibrary`。
- 两扇门都必须完全打开后才允许穿过传送区域。
- 双向出生点均不与阻挡碰撞体重叠。

### 8.4 移动与碰撞

- 玩家能够从入口到达柜台前、两组阅读桌周围和主要书架区域。
- 中央横向与纵向通道保持连通。
- 玩家不能穿过墙壁、书架、柜台或桌子。
- 固定椅子不会阻挡玩家，也不能拾取或投掷。

### 8.5 回归验证

- 新场景已加入 Build Settings。
- 新增自动测试验证场景结构、双向传送配置、出生点 ID 和关键碰撞设置。
- 项目原有完整 EditMode 测试继续通过。

