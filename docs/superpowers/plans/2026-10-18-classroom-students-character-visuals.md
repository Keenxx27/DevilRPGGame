# 教室四名学生角色美术接入实施计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 为四名其他学生生成并接入独立的四向站立/行走像素角色视觉，不改变教室剧情内容。

**Architecture:** 每名学生使用独立 4×3 角色表、动画控制器与视觉子对象；四个相同模式的轻量 Animator 组件根据所属学生根对象的世界位移选择状态。单个编辑器安装器以对象名映射四名学生，并幂等地写入场景。

**Tech Stack:** Unity 2022.3.62f1、C#、UnityEditor Animation API、NUnit EditMode、透明 PNG。

## 全局约束

- 每张 PNG 为 192×192、透明背景、4×3、48×64 单元、32 PPU、Point、Uncompressed。
- 四个方向独立绘制，各行走方向两帧；不得添加排斥台词、特殊冷淡姿态、剧情或交互变化。
- 只改 `Assets/Scenes/SchoolClassroom.unity` 的可见学生视觉；保留原根对象和所有既有组件。

### Task 1: 生成四张确认过的学生角色表

**Files:**
- Create: `Assets/Art/Characters/classroom-student-a-male-pixel-v1.png`
- Create: `Assets/Art/Characters/classroom-student-b-male-pixel-v1.png`
- Create: `Assets/Art/Characters/classroom-student-a-female-pixel-v1.png`
- Create: `Assets/Art/Characters/classroom-student-b-female-pixel-v1.png`

- [ ] 分别生成男生 A、男生 B、女生 A、女生 B 的透明背景 4×3 预览。
- [ ] 用户确认各自外观后，以最近邻裁切为严格 192×192 PNG。
- [ ] 检查四张图均有 12 个完整角色单元、无背景像素或文字。

### Task 2: 先写失败测试，再实现可复用的学生视觉方向选择

**Files:**
- Create: `Assets/Tests/EditMode/ClassroomStudentVisualAnimatorTests.cs`
- Create: `Assets/Scripts/ClassroomStudentVisualAnimator.cs`

- [ ] 写测试，断言组件依照传入的 `prefix`、方向与移动状态返回 `<prefix>_Idle_Down`、`<prefix>_Left_Run` 等独立状态，且 `FlipX` 恒为 false。
- [ ] 运行测试，确认因组件不存在而失败。
- [ ] 实现 `ClassroomStudentVisualAnimator`：序列化状态前缀，`LateUpdate` 比较世界坐标位移并只在状态改变时播放；默认朝下。
- [ ] 重跑目标测试，确认通过。

### Task 3: 生成每名学生的动画资源并幂等接入教室

**Files:**
- Create: `Assets/Editor/ClassroomStudentCharacterInstaller.cs`
- Create: `Assets/Character/ClassroomStudent*.controller`
- Create: `Assets/Character/ClassroomStudent*.anim`
- Modify: `Assets/Scenes/SchoolClassroom.unity`

- [ ] 安装器以每张角色表切出 12 张命名精灵，生成 Idle 与 Run 八个动画片段，并建立各自的 Animator Controller。
- [ ] 在现有场景层级识别四个非主角、非教授的学生根对象；对每个根对象创建独立 `Student Visual` 子对象，写入 Renderer、Animator、`ClassroomStudentVisualAnimator`、初始 Sprite、继承排序及适当缩放，最后停用旧 Renderer。
- [ ] 执行安装器并以 Unity batchmode 编译；修复实际 C# 错误，但不重构无关逻辑。

### Task 4: 场景回归验证

**Files:**
- Create: `Assets/Tests/EditMode/ClassroomStudentCharacterSceneTests.cs`

- [ ] 测试 `SchoolClassroom` 中恰有四个新的学生视觉子对象，每个旧 Renderer 停用、子对象有 Animator 与 `ClassroomStudentVisualAnimator`，并保留根对象的位置与组件。
- [ ] 运行学生相关 EditMode 测试；若 batch runner 未产生 XML，记录该限制并用最新成功编译作为最低验证，不将其等同于测试通过。
- [ ] 最终核查四张 PNG、四套控制器/八帧动画和场景引用均存在。
