# 教授 NPC 圆形外观实施计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 将教室讲台上的 `Professor` 改为灰色圆形 NPC，同时保持场景中其他对象不变。

**Architecture:** 这是一次单场景序列化数据的精准修改。先建立针对 `Professor` 的 YAML 结构校验，再只替换其 `SpriteRenderer` 的 Sprite GUID/fileID 与颜色值，最后运行新增校验和全部既有场景回归校验。

**Tech Stack:** Unity 2022.3.62f3c1、Unity Scene YAML、PowerShell 校验脚本。

## Global Constraints

- `Professor` 必须复用 `Main Character` 当前使用的圆形 Sprite。
- `Professor` 颜色必须为不透明中性灰色 `{r: 0.5, g: 0.5, b: 0.5, a: 1}`。
- 不修改教授的位置、缩放、旋转、排序层级、父对象或其他组件。
- 不修改 `Main Character`、`Basic Unit` 或其他教室物件。
- 项目不是 Git 仓库，不执行提交步骤。

---

### Task 1: 替换教授外观并验证场景回归

**Files:**
- Create: `work/professor-appearance/verify_professor_appearance.ps1`
- Modify: `Assets/Scenes/SampleScene.unity:142-208`
- Test: `work/professor-appearance/verify_professor_appearance.ps1`

**Interfaces:**
- Consumes: `Main Character` 的圆形 Sprite 引用 `{fileID: -2413806693520163455, guid: a86470a33a6bf42c4b3595704624658b, type: 3}`。
- Produces: 使用同一圆形 Sprite 且颜色为灰色的 `Professor` SpriteRenderer（fileID `35893606`）。

- [ ] **Step 1: 写入针对性场景校验脚本**

  校验脚本必须读取 `Assets/Scenes/SampleScene.unity` 中 fileID `35893606` 的 `SpriteRenderer` 块，并断言：

  ```powershell
  $expectedSprite = 'm_Sprite: {fileID: -2413806693520163455, guid: a86470a33a6bf42c4b3595704624658b, type: 3}'
  $expectedColor = 'm_Color: {r: 0.5, g: 0.5, b: 0.5, a: 1}'
  ```

  同时断言 `Main Character` 的 Sprite 与颜色仍为原值，并断言教授 Transform 块仍包含原位置、缩放和父对象引用。

- [ ] **Step 2: 运行校验并确认修改前失败**

  Run: `powershell -File work/professor-appearance/verify_professor_appearance.ps1`

  Expected: `PROFESSOR_APPEARANCE_VERIFICATION=FAIL`，原因是教授仍引用 `Basic Unit` 且颜色仍为白色。

- [ ] **Step 3: 最小化修改 Professor SpriteRenderer**

  在 `Assets/Scenes/SampleScene.unity` 的 fileID `35893606` 块中只进行以下替换：

  ```yaml
  m_Sprite: {fileID: -2413806693520163455, guid: a86470a33a6bf42c4b3595704624658b, type: 3}
  m_Color: {r: 0.5, g: 0.5, b: 0.5, a: 1}
  ```

- [ ] **Step 4: 运行针对性校验并确认通过**

  Run: `powershell -File work/professor-appearance/verify_professor_appearance.ps1`

  Expected: `PROFESSOR_APPEARANCE_VERIFICATION=PASS`。

- [ ] **Step 5: 运行全部既有场景回归校验**

  Run: 依次执行 Canvas UI、Inventory、Classroom Collision、Basic Unit Replacement、Classroom Scene 和 Door Corridor 六个现有 PowerShell 校验脚本。

  Expected: 六项均输出 `PASS`。如果依赖“所有可见对象都使用 Basic Unit”的旧断言因教授有意改为圆形而失败，只精准更新该断言，将 `Professor` 明确列为角色例外。

- [ ] **Step 6: 检查 Unity 进程状态**

  Run: `Get-Process -Name Unity -ErrorAction SilentlyContinue`

  Expected: 无 Unity 进程；场景文件修改完成后由用户下一次打开编辑器进行视觉确认。
