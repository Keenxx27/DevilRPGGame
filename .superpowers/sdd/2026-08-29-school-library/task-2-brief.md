### Task 2: 创建 Basic Unit 图书馆布局

**Files:**
- Temporary Create: `Assets/Editor/SchoolLibraryInstaller.cs` and `.meta`
- Create: `Assets/Scenes/SchoolLibrary.unity` and Unity-generated `.meta`
- Modify: `ProjectSettings/EditorBuildSettings.asset`

**Scope:** 只创建静态图书馆布局并注册 Build Settings。本任务不要修改 `SchoolCorridor`，不要创建入口/出口门或 SpawnPoint；门测试应继续失败。安装器保留到 Task 3。

**Source:** 打开 `Assets/Scenes/SchoolClassroom.unity`，递归查找 inactive `Basic Unit`，保存其 `SpriteRenderer.sprite` 与 `sharedMaterial`；然后用 `EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single)` 新建空场景。

**Scene:** `Assets/Scenes/SchoolLibrary.unity`，root `School Library`，中心 `(60,0)`，无 Player、Camera、UI、WorldItem、ThrowableChair、FlowerpotBreakable、ClassroomLibraryCardPuzzle。

**Colors:**
- floor `(0.62,0.48,0.32,1)`
- wall `(0.82,0.76,0.62,1)`
- dark wood `(0.28,0.16,0.08,1)`
- counter `(0.38,0.22,0.11,1)`
- chair `(0.40,0.10,0.10,1)`

**CreateUnit contract:** Each visual uses the Basic Unit Sprite/material, exact name/world position/scale/color/sorting order. If blocking, add enabled non-trigger `BoxCollider2D`. Floor and chairs have no Collider.

**Exact objects:**
- `Library Floor` `(60,0)` scale `(18,12)`, floor, sorting -10, nonblocking.
- `Library Wall Top` `(60,6)` `(18,0.5)`; `Library Wall Bottom Left` `(55,-6)` `(8,0.5)`; `Library Wall Bottom Right` `(65,-6)` `(8,0.5)`; `Library Wall Left` `(51,0)` `(0.5,12)`; `Library Wall Right` `(69,0)` `(0.5,12)`. All wall color, sorting 0, blocking.
- `Circulation Counter Horizontal` `(54.5,-3.4)` `(4.2,0.8)` and `Circulation Counter Vertical` `(52.8,-2.2)` `(0.8,3.2)`, counter color, sorting 2, blocking.
- `Reading Table Left` `(56.3,1.0)` `(3.2,1.2)` and `Reading Table Right` `(63.7,1.0)` `(3.2,1.2)`, dark wood, sorting 2, blocking.
- Top shelves `Bookcase Top 1..4` at `(53.3,5.2)`, `(56.7,5.2)`, `(63.3,5.2)`, `(66.7,5.2)`, scale `(3.0,0.7)`, dark wood, sorting 2, blocking.
- Side shelves: `Bookcase Left 1` `(51.8,2.7)`, `Bookcase Left 2` `(51.8,0)`, `Bookcase Right 1` `(68.2,2.7)`, `Bookcase Right 2` `(68.2,0)`, scale `(0.7,2.2)`, dark wood, sorting 2, blocking.
- Nonblocking fixed chairs: `Reading Chair Left North` `(56.3,2.0)`, `Reading Chair Left South` `(56.3,0)`, `Reading Chair Right North` `(63.7,2.0)`, `Reading Chair Right South` `(63.7,0)`, scale `(0.75,0.45)`, chair color, sorting 3, no Collider or gameplay script.

**Build Settings:** append enabled `Assets/Scenes/SchoolLibrary.unity` only if absent; preserve existing classroom/corridor order and entries.

**Installer:** public static `SchoolLibraryInstaller.InstallLayout()`. Use Unity Editor APIs and save assets. Meta GUID: `7c4c9060f30a4f678e1f136202408120`.

**Run:**
```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\RPG project 1' -executeMethod SchoolLibraryInstaller.InstallLayout -logFile 'D:\RPG project 1\Logs\school-library-layout-installer.log'
```

Then run `RPG.Tests.SchoolLibrarySceneTests` into `Logs/school-library-layout-tests.xml`. Expected: Build Settings and layout tests PASS; bidirectional door test FAIL only because Task 3 is not implemented. Do not suppress unexpected logs.

**Report:** Write `.superpowers/sdd/2026-08-29-school-library/task-2-report.md` with files, installer result, targeted test counts/names, self-review, concerns. Return only status, one-line summary, concerns.
