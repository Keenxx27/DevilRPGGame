# Main Character WASD Movement Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 让 `SampleScene` 中的 `Main Character` 可通过 WASD 以恒定最大速度平滑移动。

**Architecture:** 一个运行时 `MonoBehaviour` 读取旧输入系统的 `Horizontal`/`Vertical` 轴，调用可独立测试的位移计算函数，再更新角色 `Transform.position`。EditMode 测试覆盖方向、限速和帧率无关性；一次性 Editor 安装脚本负责把组件挂到场景对象并保存场景。

**Tech Stack:** Unity 2022.3.62f3c1、C#、Unity Test Framework 1.1.33、NUnit、旧 Input Manager、Transform。

## Global Constraints

- 使用现有旧输入系统，不安装新 Input System。
- 直接修改 `Transform`，不添加 `Rigidbody2D` 或 `Collider2D`。
- WASD 映射为 W 上、A 左、S 下、D 右。
- 斜向输入长度限制为 1，速度不高于单轴速度。
- 默认速度为每秒 5 个 Unity 单位，并允许在 Inspector 修改。
- 位移乘以 `Time.deltaTime`，不依赖帧率。
- 不实现碰撞、动画、冲刺、边界或按键重映射。
- 项目当前没有 Git 仓库，因此跳过提交步骤，不擅自初始化 Git。
- 执行 Unity 批处理命令前必须关闭当前打开的 Unity 项目，避免项目锁和场景覆盖。

---

## File Structure

- `Assets/Scripts/RPG.Runtime.asmdef`：定义运行时程序集。
- `Assets/Scripts/MainCharacterMovement.cs`：读取输入、计算位移并更新 Transform。
- `Assets/Tests/EditMode/RPG.EditModeTests.asmdef`：定义 EditMode 测试程序集并引用运行时程序集。
- `Assets/Tests/EditMode/MainCharacterMovementTests.cs`：验证位移计算行为。
- `Assets/Editor/MainCharacterMovementInstaller.cs`：一次性场景安装工具；执行成功后删除。
- `Assets/Scenes/SampleScene.unity`：挂载 `MainCharacterMovement` 后由 Unity 保存。

### Task 1: 以测试驱动实现移动计算与运行时组件

**Files:**
- Create: `Assets/Scripts/RPG.Runtime.asmdef`
- Create: `Assets/Scripts/MainCharacterMovement.cs`
- Create: `Assets/Tests/EditMode/RPG.EditModeTests.asmdef`
- Test: `Assets/Tests/EditMode/MainCharacterMovementTests.cs`

**Interfaces:**
- Consumes: Unity `Input.GetAxis(string)`、`Transform.position`、`Time.deltaTime`。
- Produces: `MainCharacterMovement.CalculateDelta(Vector2 input, float speed, float deltaTime) -> Vector3`，以及可挂载的 `MainCharacterMovement` 组件。

- [ ] **Step 1: 创建测试程序集和失败测试**

`Assets/Scripts/RPG.Runtime.asmdef`：

```json
{
  "name": "RPG.Runtime",
  "rootNamespace": "RPG"
}
```

`Assets/Tests/EditMode/RPG.EditModeTests.asmdef`：

```json
{
  "name": "RPG.EditModeTests",
  "rootNamespace": "RPG.Tests",
  "references": [
    "RPG.Runtime",
    "UnityEngine.TestRunner",
    "UnityEditor.TestRunner"
  ],
  "includePlatforms": [
    "Editor"
  ],
  "overrideReferences": true,
  "precompiledReferences": [
    "nunit.framework.dll"
  ],
  "autoReferenced": false,
  "defineConstraints": [
    "UNITY_INCLUDE_TESTS"
  ]
}
```

`Assets/Tests/EditMode/MainCharacterMovementTests.cs`：

```csharp
using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class MainCharacterMovementTests
    {
        [TestCase(0f, 1f, 0f, 5f)]
        [TestCase(-1f, 0f, -5f, 0f)]
        [TestCase(0f, -1f, 0f, -5f)]
        [TestCase(1f, 0f, 5f, 0f)]
        public void CalculateDelta_MovesInRequestedCardinalDirection(
            float inputX, float inputY, float expectedX, float expectedY)
        {
            Vector3 delta = MainCharacterMovement.CalculateDelta(
                new Vector2(inputX, inputY), 5f, 1f);

            Assert.That(delta.x, Is.EqualTo(expectedX).Within(0.0001f));
            Assert.That(delta.y, Is.EqualTo(expectedY).Within(0.0001f));
            Assert.That(delta.z, Is.Zero);
        }

        [Test]
        public void CalculateDelta_ProducesNoMovementForNoInput()
        {
            Vector3 delta = MainCharacterMovement.CalculateDelta(Vector2.zero, 5f, 1f);

            Assert.That(delta, Is.EqualTo(Vector3.zero));
        }

        [Test]
        public void CalculateDelta_LimitsDiagonalMovementToConfiguredSpeed()
        {
            Vector3 delta = MainCharacterMovement.CalculateDelta(Vector2.one, 5f, 1f);

            Assert.That(delta.magnitude, Is.EqualTo(5f).Within(0.0001f));
        }

        [Test]
        public void CalculateDelta_ScalesMovementByDeltaTime()
        {
            Vector3 delta = MainCharacterMovement.CalculateDelta(Vector2.right, 5f, 0.25f);

            Assert.That(delta, Is.EqualTo(new Vector3(1.25f, 0f, 0f))); 
        }
    }
}
```

- [ ] **Step 2: 运行测试并确认 RED**

Run：

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\RPG project 1' -runTests -testPlatform EditMode -testResults 'D:\RPG project 1\Temp\main-character-editmode-results.xml' -logFile 'D:\RPG project 1\Logs\main-character-editmode-red.log'
```

Expected：失败，编译器明确报告 `MainCharacterMovement` 类型不存在。失败原因必须是生产功能尚未实现，而不是测试语法或程序集配置错误。

- [ ] **Step 3: 添加最小运行时实现**

`Assets/Scripts/MainCharacterMovement.cs`：

```csharp
using UnityEngine;

namespace RPG
{
    public class MainCharacterMovement : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveSpeed = 5f;

        private void Update()
        {
            Vector2 input = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));

            transform.position += CalculateDelta(input, moveSpeed, Time.deltaTime);
        }

        public static Vector3 CalculateDelta(Vector2 input, float speed, float deltaTime)
        {
            Vector2 direction = Vector2.ClampMagnitude(input, 1f);
            return direction * speed * deltaTime;
        }
    }
}
```

- [ ] **Step 4: 运行测试并确认 GREEN**

再次运行 Step 2 的 Unity 命令。

Expected：进程退出码为 0；结果 XML 中所有测试通过，失败数为 0；日志无 C# 编译错误。

### Task 2: 将组件挂载到 Main Character 并验证场景

**Files:**
- Create temporarily: `Assets/Editor/MainCharacterMovementInstaller.cs`
- Modify: `Assets/Scenes/SampleScene.unity`
- Delete after execution: `Assets/Editor/MainCharacterMovementInstaller.cs` 及 Unity 生成的同名 `.meta`

**Interfaces:**
- Consumes: `RPG.MainCharacterMovement`、场景对象名称 `Main Character`。
- Produces: 已保存且包含 `MainCharacterMovement` 组件的 `SampleScene.unity`。

- [ ] **Step 1: 创建一次性安装器**

`Assets/Editor/MainCharacterMovementInstaller.cs`：

```csharp
using RPG;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MainCharacterMovementInstaller
{
    public static void Install()
    {
        const string scenePath = "Assets/Scenes/SampleScene.unity";
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        GameObject character = GameObject.Find("Main Character");

        if (character == null)
        {
            throw new MissingReferenceException("SampleScene 中找不到 Main Character。");
        }

        if (character.GetComponent<MainCharacterMovement>() == null)
        {
            character.AddComponent<MainCharacterMovement>();
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }
}
```

- [ ] **Step 2: 运行安装器并保存场景**

Run：

```powershell
& 'C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\RPG project 1' -executeMethod MainCharacterMovementInstaller.Install -logFile 'D:\RPG project 1\Logs\main-character-installer.log'
```

Expected：退出码为 0，日志中没有异常；`SampleScene.unity` 的 `Main Character` 组件列表出现 `MainCharacterMovement`，序列化速度为 `5`。

- [ ] **Step 3: 删除一次性安装器**

在确认以下两个目标都位于 `D:\RPG project 1\Assets\Editor` 后，仅删除：

```text
Assets/Editor/MainCharacterMovementInstaller.cs
Assets/Editor/MainCharacterMovementInstaller.cs.meta
```

如果 `Assets/Editor` 因此为空，可保留空目录；不删除其他文件。

- [ ] **Step 4: 运行最终 EditMode 测试与编译验证**

再次运行 Task 1 Step 2 的测试命令，并读取结果 XML 与日志。

Expected：Unity 退出码为 0；全部测试通过；无编译错误或异常。

- [ ] **Step 5: 核对最终范围**

检查最终文件列表与场景序列化内容：只保留运行时脚本、两个程序集定义、测试脚本及场景挂载改动；没有物理组件、新输入系统、临时安装器或超出设计的功能。
