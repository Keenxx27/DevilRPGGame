### Task 1: 写入图书馆场景验收测试

**Files:**
- Create: `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs`
- Create: `Assets/Tests/EditMode/SchoolLibrarySceneTests.cs.meta`

**Goal:** 先写失败的 EditMode 场景验收测试，固定图书馆布局、Build Settings、双向门与出生点契约。此任务不得创建或修改生产场景、Build Settings 或生产脚本。

**Exact constants:**
- Library scene: `Assets/Scenes/SchoolLibrary.unity`
- Corridor scene: `Assets/Scenes/SchoolCorridor.unity`
- Library destination: `SchoolLibrary` / `LibraryFromCorridor`
- Corridor destination: `SchoolCorridor` / `CorridorFromLibrary`
- Library spawn: `(60, -4.7)`, camera `(60, 0, -10)`
- Corridor spawn: `(27, 1.1)`, camera `(29, 0, -10)`

**Required layout objects:**
`Library Floor`, `Library Wall Top`, `Library Wall Bottom Left`, `Library Wall Bottom Right`, `Library Wall Left`, `Library Wall Right`, `Circulation Counter Horizontal`, `Circulation Counter Vertical`, `Reading Table Left`, `Reading Table Right`, `Bookcase Top 1..4`, `Bookcase Left 1..2`, `Bookcase Right 1..2`.

**Tests to add:**
1. `BuildSettings_ContainsEnabledSchoolLibraryScene`: use `EditorBuildSettings.scenes.Any` and require the enabled library path.
2. `SchoolLibrary_ConfiguresCompactBasicUnitLayoutAndExcludesGameplaySystems`: open library scene, require all layout names, require non-trigger BoxCollider2D on five walls, two tables and two counter parts; assert no `WorldItem`, `ThrowableChair`, `FlowerpotBreakable`, `ClassroomLibraryCardPuzzle`, `PlayerInteraction`, or `Camera`.
3. `CorridorAndLibrary_ConfigureMatchingBidirectionalDoorTransitions`: open corridor, require `Library Door` with `DoorController` and child `DoorExitPortal`, exact destination fields, `CorridorFromLibrary` position/camera, and unchanged Decorative Door 2/3. Open library and require matching `Library Exit Door` and `LibraryFromCorridor`.

**Helpers:**
- Recursive `Find(Scene, string)`.
- `AssertBlocking` requires `BoxCollider2D`, enabled, non-trigger.
- `AssertDoor` reads private `DoorExitPortal` fields by reflection and requires the portal's `door` reference to match the root `DoorController`.
- `AssertSpawn(Scene, string, Vector2, Vector3)` reads private `MapSpawnPoint.id`/`cameraPosition` by reflection and checks exact position.
- `GetField<T>` uses `BindingFlags.Instance | BindingFlags.NonPublic`.

**Meta:**
```yaml
fileFormatVersion: 2
guid: 7c4c9060f30a4f678e1f136202408119
timeCreated: 1787961600
```

**RED command:**
```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -projectPath 'D:\RPG project 1' -runTests -testPlatform EditMode -testFilter 'RPG.Tests.SchoolLibrarySceneTests' -testResults 'D:\RPG project 1\Logs\school-library-red.xml' -logFile 'D:\RPG project 1\Logs\school-library-red.log'
```

Expected RED only because the library scene, Build Settings entry, and functional door do not exist. Fix any test compile errors. Do not weaken assertions or suppress unexpected logs.

**Report:** Write `.superpowers/sdd/2026-08-29-school-library/task-1-report.md` with changed files, exact command/result, expected failure causes, and self-review. Return only status, one-line test summary, and concerns.
