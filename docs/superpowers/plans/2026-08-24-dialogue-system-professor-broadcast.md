# 对话系统与教授广播实施计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 建立带底部 Canvas 对话框、逐字显示、广播/私聊触发、剧情输入锁定和跳过功能的通用对话系统，并配置教授与主角的10页课堂剧情。

**Architecture:** 使用纯 C# `DialoguePlayback` 管理逐字显示与翻页状态，`DialogueController` 作为唯一的 `R`/空格输入入口并管理剧情锁定，`DialogueHUD` 只负责 Canvas 显示。`BroadcastDialogueTrigger` 自动请求一次播放，`PrivateDialogueTrigger` 通过 Trigger 向玩家控制器登记可交谈来源。

**Tech Stack:** Unity 2022.3.62f3c1、C#、UnityEngine.UI、2D Physics、Unity Scene YAML、NUnit EditMode 测试、独立 C# 行为 Harness、PowerShell 场景校验。

## Global Constraints

- 每页以每秒12个显示字符推进，标点计入字符数。
- 逐字显示期间 `R` 无效；页面完整显示后 `R` 只负责翻页或结束。
- 空格在任何播放阶段跳过当前整段对话。
- 对话期间禁用移动、开门/拾取交互、丢弃和物品栏槽位切换。
- 正常结束、跳过和失败都必须隐藏对话框并恢复开始前的输入组件状态。
- 对话 UI 使用独立底部 Canvas，深色背景、金色边框、金色名称、白色正文，排序高于 Inventory Canvas 的100。
- 教授广播只在本次 `SampleScene` 实例中自动请求一次；返回教室不得重播。
- 教授广播必须严格使用设计文档中的10页名称、文字和顺序。
- 不复制玩家、主摄像机、物品栏或对话 Canvas 到 `SchoolCorridor`。
- 不引入 TextMeshPro、ScriptableObject 或第三方依赖。
- 项目不是 Git 仓库；每个任务的提交步骤均记录为跳过，不执行 Git 命令。

---

## 文件结构

**新增运行时代码：**

- `Assets/Scripts/DialoguePage.cs`：可序列化的发言者/正文数据。
- `Assets/Scripts/DialoguePlayback.cs`：纯状态机，负责逐字推进、翻页、完成和跳过。
- `Assets/Scripts/DialogueController.cs`：输入、私聊来源选择、HUD 刷新和输入锁定。
- `Assets/Scripts/DialogueHUD.cs`：标准 Canvas 文本与面板显示。
- `Assets/Scripts/BroadcastDialogueTrigger.cs`：单实例自动广播一次。
- `Assets/Scripts/PrivateDialogueTrigger.cs`：角色接触范围注册/注销。

**新增测试与校验：**

- `Assets/Tests/EditMode/DialoguePlaybackTests.cs`
- `Assets/Tests/EditMode/DialogueControllerTests.cs`
- `Assets/Tests/EditMode/DialogueHUDTests.cs`
- `Assets/Tests/EditMode/DialogueTriggerTests.cs`
- `work/dialogue/DialoguePlaybackHarness.cs`
- `work/dialogue/DialogueControllerHarness.cs`
- `work/dialogue/verify_dialogue_scene.ps1`

**修改场景：**

- `Assets/Scenes/SampleScene.unity`

---

### Task 1: 对话页面与逐字播放状态机

**Files:**
- Create: `Assets/Scripts/DialoguePage.cs`
- Create: `Assets/Scripts/DialoguePlayback.cs`
- Create: `Assets/Tests/EditMode/DialoguePlaybackTests.cs`
- Create: `work/dialogue/DialoguePlaybackHarness.cs`

**Interfaces:**
- Produces: `DialoguePage(string speaker, string text)`、`Speaker`、`Text`。
- Produces: `DialoguePlayback(IReadOnlyList<DialoguePage> pages)`。
- Produces: `DialogueAdvanceResult { Blocked, Advanced, Completed }`。
- Produces: `PageIndex`、`Speaker`、`VisibleText`、`IsPageComplete`、`IsComplete`。
- Produces: `void Step(float charactersPerSecond, float deltaTime)`、`DialogueAdvanceResult TryAdvance()`、`void Skip()`。

- [ ] **Step 1: 写入失败的 EditMode 测试**

  测试至少包含以下真实状态行为：

  ```csharp
  [Test]
  public void Step_RevealsTwelveCharactersPerSecond()
  {
      var playback = new DialoguePlayback(new[] {
          new DialoguePage("教授", "一二三四五六七八九十甲乙丙")
      });

      playback.Step(12f, 0.5f);

      Assert.That(playback.VisibleText, Is.EqualTo("一二三四五六"));
      Assert.That(playback.IsPageComplete, Is.False);
  }

  [Test]
  public void TryAdvance_IsBlockedUntilPageCompletes()
  {
      var playback = CreateTwoPagePlayback();
      Assert.That(playback.TryAdvance(), Is.EqualTo(DialogueAdvanceResult.Blocked));
      Assert.That(playback.PageIndex, Is.Zero);
  }

  [Test]
  public void TryAdvance_MovesPageThenCompletesOnLastPage()
  {
      var playback = CreateTwoPagePlayback();
      playback.Step(100f, 1f);
      Assert.That(playback.TryAdvance(), Is.EqualTo(DialogueAdvanceResult.Advanced));
      Assert.That(playback.PageIndex, Is.EqualTo(1));
      Assert.That(playback.VisibleText, Is.Empty);
      playback.Step(100f, 1f);
      Assert.That(playback.TryAdvance(), Is.EqualTo(DialogueAdvanceResult.Completed));
      Assert.That(playback.IsComplete, Is.True);
  }

  [Test]
  public void Skip_CompletesWholeSequenceImmediately()
  {
      var playback = CreateTwoPagePlayback();
      playback.Skip();
      Assert.That(playback.IsComplete, Is.True);
  }
  ```

- [ ] **Step 2: 运行 RED 编译/测试**

  Run: 使用 Unity Roslyn 编译上述测试或 `DialoguePlaybackHarness.cs` 与待测脚本列表。

  Expected: 编译失败，明确报告 `DialoguePage`、`DialoguePlayback` 或 `DialogueAdvanceResult` 不存在。

- [ ] **Step 3: 实现最小播放状态机**

  `DialoguePage` 使用 `[Serializable]` 和 `[SerializeField]` 字段，同时提供供测试与运行时读取的属性。`DialoguePlayback` 必须：

  ```csharp
  public void Step(float charactersPerSecond, float deltaTime)
  {
      if (IsComplete || IsPageComplete || charactersPerSecond <= 0f || deltaTime <= 0f)
      {
          return;
      }

      revealedCharacters += charactersPerSecond * deltaTime;
      visibleCharacterCount = Math.Min(
          CurrentPage.Text.Length,
          (int)Math.Floor(revealedCharacters));
  }
  ```

  `TryAdvance()` 在未显示完成时返回 `Blocked`，切换到下一页时清零字符进度，最后一页结束时返回 `Completed`。

- [ ] **Step 4: 运行 GREEN 状态机测试**

  Run: 编译并执行 `DialoguePlaybackHarness.exe`。

  Expected: 逐字速度、翻页门槛、下一页清零、最后结束和跳过全部通过，至少 `5/5 passed`。

- [ ] **Step 5: 跳过提交**

  记录：`SKIP_COMMIT_NON_GIT_PROJECT`。

---

### Task 2: 对话控制器、私聊选择与剧情输入锁定

**Files:**
- Create: `Assets/Scripts/DialogueController.cs`
- Create: `Assets/Scripts/PrivateDialogueTrigger.cs`
- Create: `Assets/Tests/EditMode/DialogueControllerTests.cs`
- Create: `work/dialogue/DialogueControllerHarness.cs`

**Interfaces:**
- Consumes: Task 1 的 `DialoguePage[]`、`DialoguePlayback` 和 `DialogueAdvanceResult`。
- Consumes: Task 3 将提供的 `DialogueHUD.TryShow(...)`、`SetPage(...)`、`Hide()`。
- Produces: `bool IsPlaying`。
- Produces: `bool StartDialogue(DialoguePage[] pages)`、`DialogueAdvanceResult TryAdvanceDialogue()`、`void SkipDialogue()`、`void Tick(float deltaTime)`。
- Produces: `void RegisterPrivateDialogue(PrivateDialogueTrigger source)`、`void UnregisterPrivateDialogue(PrivateDialogueTrigger source)`、`bool TryStartNearestPrivateDialogue()`、`void ClearNearbyPrivateDialogues()`。
- Produces: `PrivateDialogueTrigger.Pages`。

- [ ] **Step 1: 写入失败的控制器测试**

  使用真实 `GameObject`、`MainCharacterMovement`、`PlayerInteraction`、`PlayerInventory` 与禁用状态断言：

  ```csharp
  [Test]
  public void StartAndSkip_LocksThenRestoresGameplayComponents()
  {
      DialogueController controller = CreateConfiguredController(out Behaviour[] gameplay);

      Assert.That(controller.StartDialogue(OnePage()), Is.True);
      Assert.That(gameplay.All(component => !component.enabled), Is.True);

      controller.SkipDialogue();

      Assert.That(controller.IsPlaying, Is.False);
      Assert.That(gameplay.All(component => component.enabled), Is.True);
  }

  [Test]
  public void StartDialogue_DoesNotOverwriteActiveSequence()
  {
      DialogueController controller = CreateConfiguredController(out _);
      Assert.That(controller.StartDialogue(OnePage("教授")), Is.True);
      Assert.That(controller.StartDialogue(OnePage("主角")), Is.False);
      Assert.That(controller.CurrentSpeaker, Is.EqualTo("教授"));
  }

  [Test]
  public void NearestPrivateDialogue_IsChosen()
  {
      DialogueController controller = CreateConfiguredController(out _);
      PrivateDialogueTrigger far = CreateSource(new Vector2(3f, 0f), "远处");
      PrivateDialogueTrigger near = CreateSource(new Vector2(1f, 0f), "近处");
      controller.RegisterPrivateDialogue(far);
      controller.RegisterPrivateDialogue(near);

      Assert.That(controller.TryStartNearestPrivateDialogue(), Is.True);
      Assert.That(controller.CurrentSpeaker, Is.EqualTo("近处"));
  }
  ```

  另测：空页面/空正文拒绝且不锁输入、跳过恢复原本为禁用的组件状态、播放期间 `TryAdvanceDialogue()` 在逐字未完成时返回 `Blocked`。

- [ ] **Step 2: 运行 RED 编译/测试**

  Run: 编译 `DialogueControllerTests.cs` 或控制器 Harness。

  Expected: `DialogueController` 与 `PrivateDialogueTrigger` 缺失导致失败。

- [ ] **Step 3: 实现唯一输入入口与锁定恢复**

  `DialogueController.Update()` 采用明确互斥分支：

  ```csharp
  private void Update()
  {
      if (IsPlaying)
      {
          if (Input.GetKeyDown(KeyCode.Space))
          {
              SkipDialogue();
          }
          else if (Input.GetKeyDown(KeyCode.R))
          {
              TryAdvanceDialogue();
          }
          else
          {
              Tick(Time.deltaTime);
          }
          return;
      }

      if (Input.GetKeyDown(KeyCode.R))
      {
          TryStartNearestPrivateDialogue();
      }
  }
  ```

  开始时分别保存三个 gameplay 组件的 `enabled` 值，再禁用它们；结束时恢复保存值。`PrivateDialogueTrigger` 的 Enter/Exit 只调用注册方法，不读取按键。

- [ ] **Step 4: 运行 GREEN 控制器测试**

  Run: 编译并执行 `DialogueControllerHarness.exe`。

  Expected: 输入锁定/恢复、冲突拒绝、最近私聊、无效页面拒绝、逐字期间禁止翻页全部通过，至少 `6/6 passed`。

- [ ] **Step 5: 跳过提交**

  记录：`SKIP_COMMIT_NON_GIT_PROJECT`。

---

### Task 3: 标准 Canvas HUD 与中文字体

**Files:**
- Create: `Assets/Scripts/DialogueHUD.cs`
- Create: `Assets/Tests/EditMode/DialogueHUDTests.cs`

**Interfaces:**
- Produces: `bool TryShow(string speaker, string visibleText, bool canAdvance)`。
- Produces: `bool SetPage(string speaker, string visibleText, bool canAdvance)`。
- Produces: `void Hide()`。
- Produces: `bool IsVisible`。
- Consumes: Canvas 面板、speaker `Text`、body `Text`、hint `Text` 的序列化引用。

- [ ] **Step 1: 写入失败的 HUD 测试**

  ```csharp
  [Test]
  public void TryShow_SetsContentAndActivatesPanel()
  {
      DialogueHUD hud = CreateHud(out GameObject panel, out Text speaker, out Text body, out Text hint);

      Assert.That(hud.TryShow("教授", "所以同学们要明确，", false), Is.True);
      Assert.That(panel.activeSelf, Is.True);
      Assert.That(speaker.text, Is.EqualTo("教授"));
      Assert.That(body.text, Is.EqualTo("所以同学们要明确，"));
      Assert.That(hint.text, Does.Contain("R 继续"));
      Assert.That(hint.color.a, Is.LessThan(1f));
  }

  [Test]
  public void Hide_DeactivatesPanel()
  {
      DialogueHUD hud = CreateHud(out GameObject panel, out _, out _, out _);
      hud.TryShow("主角", "测试", true);
      hud.Hide();
      Assert.That(panel.activeSelf, Is.False);
  }
  ```

  另测缺少任一引用时 `TryShow` 返回 false 且不抛异常。

- [ ] **Step 2: 运行 RED 编译/测试**

  Expected: `DialogueHUD` 不存在导致失败。

- [ ] **Step 3: 实现 HUD 和系统字体回退**

  `Awake()` 隐藏面板，并为三个 Text 尝试设置：

  ```csharp
  Font font = Font.CreateDynamicFontFromOSFont(
      new[] { "Microsoft YaHei", "SimHei", "Arial" }, 28);
  ```

  字体创建失败时保留场景内字体并记录一次警告。`SetPage` 根据 `canAdvance` 将提示颜色 alpha 设置为 `1f` 或 `0.45f`。

- [ ] **Step 4: 运行 GREEN HUD 测试与运行时代码编译**

  Expected: HUD 测试通过；包含所有新增脚本的 runtime assembly 使用 Unity 2022.3.62f3c1 Roslyn 编译通过。

- [ ] **Step 5: 跳过提交**

  记录：`SKIP_COMMIT_NON_GIT_PROJECT`。

---

### Task 4: 单次广播触发与教授10页数据

**Files:**
- Create: `Assets/Scripts/BroadcastDialogueTrigger.cs`
- Create: `Assets/Tests/EditMode/DialogueTriggerTests.cs`
- Modify: `Assets/Scenes/SampleScene.unity`

**Interfaces:**
- Consumes: `DialogueController.StartDialogue(DialoguePage[] pages)`。
- Produces: `bool Trigger()`、`bool HasTriggered`。
- Produces: `PrivateDialogueTrigger` 的真实 Physics2D Enter/Exit 注册路径。

- [ ] **Step 1: 写入失败的广播测试**

  ```csharp
  [Test]
  public void Trigger_RequestsSequenceOnlyOnce()
  {
      BroadcastDialogueTrigger trigger = CreateTrigger(out DialogueController controller);

      Assert.That(trigger.Trigger(), Is.True);
      controller.SkipDialogue();
      Assert.That(trigger.Trigger(), Is.False);
      Assert.That(trigger.HasTriggered, Is.True);
  }
  ```

  另测私聊 Trigger 对非玩家 Collider 不注册、对带 `DialogueController` 的玩家注册并在 Exit 注销。

- [ ] **Step 2: 运行 RED 编译/测试**

  Expected: `BroadcastDialogueTrigger` 缺失导致失败。

- [ ] **Step 3: 实现最小单次广播组件**

  `Start()` 调用 `Trigger()`；`Trigger()` 在首次调用时先设置 `hasTriggered = true`，再验证 controller/pages 并请求播放，之后所有调用均返回 false。

- [ ] **Step 4: 在教授组件中配置精确10页**

  `Professor` 的 GameObject fileID `35893605` 添加广播组件，页面必须依次为：

  ```text
  教授|所以同学们要明确，
  教授|恶魔是归墟侵蚀具象化的、充满攻击性的副产物。
  教授|恶魔猎人的任务，就是像清除癌细胞一样，将它们从地球的肌体上彻底清除。
  主角|（左脸被归墟侵蚀的痕迹……正在发热、灼痛。）
  主角|如果它们只是动物。
  主角|如果它们中，有比动物更复杂的存在。
  主角|我们到底在做什么？
  主角|也许我该去图书馆看看......
  主角|不知道我的借书卡去哪里了，
  主角|可能是被哪个家伙藏起来了。
  ```

- [ ] **Step 5: 运行 GREEN 触发器测试**

  Expected: 单次广播、非玩家忽略、玩家私聊注册/注销全部通过。

- [ ] **Step 6: 跳过提交**

  记录：`SKIP_COMMIT_NON_GIT_PROJECT`。

---

### Task 5: Canvas 场景集成与完整回归

**Files:**
- Modify: `Assets/Scenes/SampleScene.unity`
- Create: `work/dialogue/verify_dialogue_scene.ps1`
- Modify only if intentional character exception requires it: existing classroom scene verifiers under `work/classroom-scene/`

**Interfaces:**
- Consumes: Task 2 的 `DialogueController`、Task 3 的 `DialogueHUD`、Task 4 的广播组件。
- Produces: SampleScene 中唯一的 `Dialogue Canvas` 与默认隐藏的 `Dialogue Panel`。

- [ ] **Step 1: 写入失败的场景结构校验**

  校验必须断言：

  ```text
  Main Character 含 DialogueController，且引用 movement/playerInteraction/playerInventory/HUD
  Professor 含 BroadcastDialogueTrigger，且 pages 数量为10
  Dialogue Canvas 为 Screen Space Overlay，sortingOrder=200
  Dialogue Panel 锚定底部且 m_IsActive=0
  外框 Image 为金色，内层 Image 为高不透明深色
  Speaker Text 为金色，Body Text 为白色
  Hint Text 初始内容包含 R 继续 与 Space 跳过
  SchoolCorridor 不包含 Dialogue Canvas、DialogueController 或第二个玩家
  所有新脚本 GUID、组件引用与 fileID 唯一
  ```

- [ ] **Step 2: 运行场景校验并确认 RED**

  Expected: 报告对话 Canvas、HUD、控制器和广播组件缺失。

- [ ] **Step 3: 写入标准 Unity Canvas UI**

  使用新的连续 fileID 区间，创建：

  ```text
  Dialogue Canvas（Canvas sortingOrder 200 + CanvasScaler + DialogueHUD）
    Dialogue Panel（金色 Image，底部锚点，默认隐藏）
      Dialogue Background（深色 Image，四边内缩形成边框）
        Speaker Text（金色，左上）
        Body Text（白色，多行，上方/中部）
        Hint Text（右下，R 继续　Space 跳过）
  ```

  CanvasScaler 使用 `1920x1080`、Scale With Screen Size、Match `0.5`。面板横向覆盖屏幕宽度的90%，底部边距24，设计高度260。

- [ ] **Step 4: 连接玩家与广播引用**

  `Main Character` 增加 `DialogueController`，连接：

  ```text
  hud -> DialogueHUD
  movement -> MainCharacterMovement fileID 1667414360
  playerInteraction -> PlayerInteraction fileID 2130000001
  playerInventory -> PlayerInventory fileID 2110000000
  charactersPerSecond -> 12
  ```

  教授广播组件连接该控制器并序列化10页。

- [ ] **Step 5: 运行针对性场景校验至 GREEN**

  Expected: `DIALOGUE_SCENE_VERIFICATION=PASS`。

- [ ] **Step 6: 完整重新编译与行为 Harness**

  使用 Unity Roslyn 与以下引用重新编译全部 `Assets/Scripts/*.cs`：

  ```text
  netstandard.dll
  UnityEngine.CoreModule.dll
  UnityEngine.InputLegacyModule.dll
  UnityEngine.Physics2DModule.dll
  UnityEngine.UI.dll
  ```

  Expected: runtime 编译退出码0；对话状态机和控制器 Harness 全部通过。

- [ ] **Step 7: 运行全部既有回归**

  依次执行：

  ```text
  verify_professor_appearance.ps1
  verify_canvas_inventory_ui.ps1
  verify_inventory_scene.ps1
  verify_classroom_collision.ps1
  verify_basic_unit_replacement.ps1
  verify_classroom_scene.ps1
  verify_door_corridor_scenes.ps1
  ```

  再执行既有 InventorySlots、InventoryInteraction、Movement、MovementPhysicsContract、HudLifecycle、DoorMotion、PlayerInteraction 和 MapTransition Harness。

  Expected: 所有校验与 Harness 均通过。

- [ ] **Step 8: 代码审查与审查修正**

  审查重点：唯一 `R` 输入入口、逐字期间禁止翻页、所有退出路径恢复输入、单次广播、10页精确内容、Canvas 中文可见性、Additive 场景不重复创建持久对象。

- [ ] **Step 9: Unity 人工验收清单**

  Unity 批处理此前无法在本机稳定初始化，因此不得用静态校验冒充 Play Mode 结果。用户下次打开 Unity 后验证：

  ```text
  进入 Play Mode 后教授广播自动弹出一次
  中文名称与正文无方框/乱码
  文字约12字符/秒逐字出现
  逐字期间 R 不翻页，显示完后 R 翻页
  对话期间 WASD/E/Q/1/2/3 均无效
  空格从任意页面直接结束全部10页
  正常翻完10页后面板隐藏且全部游戏输入恢复
  进入走廊再返回教室不重播广播
  ```

- [ ] **Step 10: 跳过提交**

  记录：`SKIP_COMMIT_NON_GIT_PROJECT`。
