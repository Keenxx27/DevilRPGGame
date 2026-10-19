# 教室离场学生对白实施计划

**Goal:** 在不停止离场移动的前提下播放两段学生私语。

### Task 1: 测试与实现

- [ ] 在 `Assets/Tests/EditMode/ClassroomDismissalSequenceTests.cs` 写失败测试，断言离场学生对白数组按学生 1、学生 2 与确认文本排序。
- [ ] 在 `Assets/Scripts/ClassroomDismissalSequence.cs` 新增静态 `StudentDismissalPages`，并在启动全部学生离场协程后等待 0.2 秒调用 `dialogueController.StartDialogue(StudentDismissalPages, null)`。
- [ ] 运行目标测试与 Unity 编译，确认无 C# 错误；不把缺失 batch XML 当作测试通过。
