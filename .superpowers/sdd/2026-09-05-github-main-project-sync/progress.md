# GitHub 正式主项目同步进度

- 工作树：`D:\RPG project 1\.worktrees\DevilRPGGame-sync`
- 分支：`codex/sync-school-library`
- 基线提交：`cf48db41452fea9ad6a586ef3223af2890411cd0`
- Unity：`2022.3.62f3c1`
- 基线报告：`Logs/sync-baseline-final.xml`
- 基线结果：94 total，91 passed，3 failed，0 skipped。
- 已知 RED：3 个失败全部属于 `RPG.Tests.SchoolLibrarySceneTests`，原因是图书馆场景、Build Settings 条目和双向门尚未迁入；非图书馆失败为 0。
- China Unity 环境配置保持未暂存：
  - `Packages/packages-lock.json` — `E3BBF3B15D3C59485ABA617DCC7895E9351B7E41E42873417A31E68DB8ED4B94`
  - `ProjectSettings/PackageManagerSettings.asset` — `73E9832BC4B326B061273755A7CA92DE6148E6565ADA17534788B00F875CF707`
  - `ProjectSettings/ProjectVersion.txt` — `B42279CFD794D9F1825F3B7C1F318B861FA9E2E2B3C6C146737BDBD41C01B389`
- 环境说明：China Unity 的全新 UPM resolve 会因空 `path` 失败；工作树使用被 Git 忽略的主项目 `Library` 快照，并将 `ProjectCache` 路径改为工作树。`ProjectCache.md5` 必须是无换行的 32 字节十六进制文本。
