# Task 1：图书馆场景验收测试（RED）

## 变更文件

- `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs`
- `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs.meta`
- `.superpowers/sdd/2026-08-29-school-library/task-1-report.md`

未修改生产脚本、场景或 Build Settings。

## 测试命令与结果

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -projectPath 'D:\RPG project 1' -runTests -testPlatform EditMode -testFilter 'RPG.Tests.SchoolLibrarySceneTests' -testResults 'D:\RPG project 1\Logs\school-library-red.xml' -logFile 'D:\RPG project 1\Logs\school-library-red.log'
```

结果：`school-library-red.xml` 显示 3 个测试、0 通过、3 失败；测试程序集已成功编译并执行。

- `BuildSettings_ContainsEnabledSchoolLibraryScene`：未找到已启用的 `Assets/Scenes/SchoolLibrary.unity`。
- `CorridorAndLibrary_ConfigureMatchingBidirectionalDoorTransitions`：走廊中不存在 `Library Door`。
- `SchoolLibrary_ConfiguresCompactBasicUnitLayoutAndExcludesGameplaySystems`：`Assets/Scenes/SchoolLibrary.unity` 尚不存在。

这些失败仅对应尚未实施的图书馆场景、Build Settings 条目与双向门配置，符合预期 RED 状态。

## 自审

- 三个测试覆盖了简报要求的 Build Settings、布局/阻挡碰撞体/排除游戏系统，以及双向门和出生点契约。
- 私有 `DoorExitPortal` 与 `MapSpawnPoint` 序列化字段通过 `BindingFlags.Instance | BindingFlags.NonPublic` 反射读取；门的 portal 明确验证其 `door` 引用指向根 `DoorController`。
- `SchoolLibrarySceneTests.cs.meta` 的 guid 和 timeCreated 与简报逐字一致。
- 当前 RED 失败来自缺失功能，不存在编译失败或未预期日志抑制。

## 修复轮次 1：走廊门来源契约

审查指出原测试只验证了存在名为 `Library Door` 的对象，不能排除保留
`Decorative Door 1` 并新增另一扇门的错误实现。

- 在 `CorridorAndLibrary_ConfigureMatchingBidirectionalDoorTransitions` 中新增
  `Library Door` 的精确世界坐标断言：`(27f, 2.5f, 0f)`。
- 新增 `Decorative Door 1` 不存在的断言，确保原对象被重命名/改造而非保留。

本轮测试命令：

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -projectPath 'D:\RPG project 1' -runTests -testPlatform EditMode -testFilter 'RPG.Tests.SchoolLibrarySceneTests' -testResults 'D:\RPG project 1\Logs\school-library-red-fix1.xml' -logFile 'D:\RPG project 1\Logs\school-library-red-fix1.log'
```

结果：`school-library-red-fix1.xml` 显示 3 个测试、0 通过、3 失败，且没有编译错误。失败仍仅为预期的未实现功能：未启用图书馆 Build Settings 条目、走廊不存在 `Library Door`、图书馆场景文件不存在。
