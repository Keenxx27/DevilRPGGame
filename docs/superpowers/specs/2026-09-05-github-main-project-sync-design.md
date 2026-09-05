# GitHub 正式主项目同步设计

## 目标

将 `D:\Documents\GitHub\DevilRPGGame` 确立为唯一正式主项目，在完整保留其现有像素美术、角色动画及资源 GUID 的前提下，迁入 `D:\RPG project 1` 已完成并验证的学校图书馆功能，同时修复正式教室场景中动画版主角缺少 `Animator` 子对象而导致的运行时报错。

## 权威来源

- 正式代码库与美术基线：`D:\Documents\GitHub\DevilRPGGame` 当前 `main` 的已提交状态。
- 图书馆功能来源：`D:\RPG project 1`。
- GitHub 主项目中的 `Assets/Character`、`Assets/Pixel2D`、动画片段、Animator Controller、Sprite、材质、Prefab 及其 `.meta` 全部保留，不由旧项目中的同名或缺失内容覆盖。
- 图书馆本轮继续使用已验收的 Basic Unit 占位布局；将像素美术应用到图书馆不属于本次同步范围。

## 同步策略

从 GitHub 主项目 `main` 的当前提交创建独立工作树和分支 `codex/sync-school-library`。原主目录中未提交的 `Packages/packages-lock.json`、`ProjectSettings/PackageManagerSettings.asset` 与 `ProjectSettings/ProjectVersion.txt` 保持原样，不由同步过程修改。为使中国版 Unity 能在隔离工作树中恢复包并运行测试，工作树可以镜像这三个本机环境配置，但它们始终保持未暂存、未提交，不进入功能分支历史或最终合并。

同步采用按功能边界精确迁移，而非目录覆盖：

1. 原样迁入 `Assets/Scenes/SchoolLibrary.unity` 及其 `.meta`，保持场景 GUID。
2. 以 GitHub 版本的 `Assets/Scenes/SchoolCorridor.unity` 为底，只加入 `Library Door`、`CorridorFromLibrary` 和必要的双向传送组件；不覆盖主项目的美术对象或资源引用。
3. 在 `ProjectSettings/EditorBuildSettings.asset` 现有教室、走廊条目之后追加启用的 `SchoolLibrary`，不重排既有场景。
4. 合并增强后的 `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs`，保留固定椅子、中央通道、装饰门、排除组件和双向传送验收。
5. 安装完成后删除 GitHub 主项目中已跟踪但不应交付的 `Assets/Editor/SchoolLibraryInstaller.cs` 及其 `.meta`。
6. 不同步 `Library`、`Temp`、`Logs`、`UserSettings`、解决方案文件或旧项目的 `Packages` 内容。

## 主角动画配置修复

正式启动场景继续使用 `Assets/Scenes/SchoolClassroom.unity`，不以 `Classroom_1.unity` 整体替换它。

保留 `Main Character` 根对象上现有的 `MainCharacterMovement`、`Rigidbody2D`、交互、背包、对话和根级玩家碰撞组件。参考 `Classroom_1.unity` 中已经接入的动画视觉，在根对象下建立纯视觉子对象：

- 子对象包含 `Transform`、`Animator` 与 `SpriteRenderer`。
- `Animator` 继续引用 GitHub 主项目现有的 Player Animator Controller GUID。
- `SpriteRenderer` 使用 GitHub 主项目现有角色 Sprite，并保持适合角色显示的排序层级。
- 移除或禁用根对象旧的静态 `SpriteRenderer`，确保角色只显示一次。
- 不把 `Classroom_1` 视觉子对象上的额外 `BoxCollider2D` 复制进正式场景，避免与根级玩家碰撞体叠加。

该结构满足动画版 `MainCharacterMovement.GetComponentInChildren<Animator>()` 与 `GetComponentInChildren<SpriteRenderer>()` 的运行时依赖，同时不改变移动和交互物理边界。

## 测试与验收

同步遵循 RED-GREEN 顺序：

1. 在独立工作树先运行完整 EditMode 测试，记录 GitHub 主项目基线；若存在与本次同步无关的既有失败，先停止并报告。
2. 增加正式教室角色结构测试，断言 `Main Character` 能找到启用的 `Animator` 和 `SpriteRenderer`、Animator Controller 非空、根对象不存在重复可见 SpriteRenderer，并且视觉子对象没有 Collider。
3. 运行新测试确认它因当前角色结构缺失而失败。
4. 迁入图书馆测试并确认缺少场景、Build Settings 条目和双向门配置时按预期失败。
5. 实施最小场景修改后，运行角色结构测试与全部 `SchoolLibrarySceneTests`，要求全部通过且日志无未预期错误。
6. 运行完整 EditMode 回归，要求 `failed=0`、`skipped=0`。
7. 用正式 `SchoolClassroom` 进入 Play Mode 做冒烟验证：角色可移动、方向动画可切换、交互仍工作，Console 不再出现“Main Character 的子对象上需要 Animator 和 SpriteRenderer”。随后验证走廊进入图书馆并能返回。

## 冲突与停止条件

出现以下任一情况时停止同步并保留工作树供检查，不使用覆盖或放宽测试绕过：

- 主项目所需 Sprite、Animator Controller 或脚本 GUID 无法解析。
- 精确迁移的走廊对象与主项目已有对象名、出生点 ID 或 YAML fileID 冲突。
- GitHub 主项目基线测试存在未解释失败。
- 同步导致现有美术资源、`.meta`、Packages 或非目标场景发生变化。
- 完整回归或 Play Mode 冒烟验证出现新的异常。

## 交付与合并

所有实现提交只存在于 `codex/sync-school-library`，每个提交限定为可独立审查的角色修复、图书馆迁移或验收测试变更。验证完成后先展示差异、测试报告和主目录未提交文件状态；只有得到明确批准，才将该分支合并回 `main`。不自动推送远程仓库。

同步完成后，`D:\Documents\GitHub\DevilRPGGame` 成为后续开发的唯一工作项目；`D:\RPG project 1` 仅作为历史来源保留，不再接受新功能修改。
