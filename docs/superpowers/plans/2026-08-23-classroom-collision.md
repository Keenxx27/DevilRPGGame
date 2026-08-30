# Classroom Collision Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 让 `Main Character` 使用 Rigidbody2D 平滑移动并被20个指定教室静物阻挡，同时隐藏 `Basic Unit`。

**Architecture:** 输入在 `Update` 中采样，物理位移在 `FixedUpdate` 中通过 `Rigidbody2D.MovePosition` 执行。主角使用 Dynamic Rigidbody2D 与 CircleCollider2D，指定静物使用无 Rigidbody2D 的静态 BoxCollider2D；场景组件直接序列化到 `SampleScene.unity`。

**Tech Stack:** Unity 2022.3.62f3c1、C#、Unity 2D Physics、Unity Scene YAML、NUnit、PowerShell。

## Global Constraints

- Unity 版本为 2022.3.62f3c1。
- 主角继续使用圆形 Sprite，37个教室静物继续使用 `Basic Unit` 方形 Sprite。
- Rigidbody2D 为 Dynamic、Gravity Scale 0、Interpolate、Continuous、Freeze Rotation Z。
- 仅四面墙、12张学生桌、教师桌、讲台、储物柜和门具有 BoxCollider2D，共20个。
- 12把椅子、地板、黑板、窗户、Professor 和 `Basic Unit` 不添加碰撞体。
- `Basic Unit` 保留在场景中，但 GameObject inactive。
- 项目不是 Git 仓库；以场景和脚本备份代替工作树，不执行 commit。

---

### Task 1: 将主角移动迁移到 Rigidbody2D

**Files:**
- Modify: `Assets/Scripts/MainCharacterMovement.cs`
- Modify: `Assets/Tests/EditMode/MainCharacterMovementTests.cs`
- Create: `work/classroom-collision/MovementPhysicsContractHarness.cs`

**Interfaces:**
- Consumes: Unity Legacy Input 的 `Horizontal`、`Vertical` 轴。
- Produces: `MainCharacterMovement.CalculateDelta(Vector2, float, float) -> Vector3` 保持不变；新增 Rigidbody2D 必需组件契约；`FixedUpdate` 通过 `MovePosition` 执行位移。

- [x] **Step 1: 编写失败的 Rigidbody2D 依赖测试**

在 `MainCharacterMovementTests.cs` 添加真实 GameObject 测试：

```csharp
[Test]
public void AddingMovementAlsoAddsRequiredRigidbody2D()
{
    GameObject gameObject = new GameObject("Movement Test");
    try
    {
        gameObject.AddComponent<MainCharacterMovement>();
        Assert.That(gameObject.GetComponent<Rigidbody2D>(), Is.Not.Null);
    }
    finally
    {
        Object.DestroyImmediate(gameObject);
    }
}
```

同时创建 `MovementPhysicsContractHarness.cs`，通过反射断言 `MainCharacterMovement` 存在 `RequireComponent` 且其必需类型包含 `Rigidbody2D`。这个可在关闭 Unity 时运行，用于证明 RED-GREEN；NUnit 测试留给 Unity Test Runner 执行真实 GameObject 行为。

- [x] **Step 2: 编译并运行 RED 契约检查**

使用 Unity `Library/Bee/.../RPG.Runtime.rsp` 编译当前运行时程序集，再编译并运行契约夹具。

Expected: 契约夹具 FAIL，报告缺少 `RequireComponent(Rigidbody2D)`；新增 NUnit 测试程序集可编译。

- [x] **Step 3: 实现最小 Rigidbody2D 移动**

将运行时代码改为：

```csharp
using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MainCharacterMovement : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveSpeed = 5f;

        private Rigidbody2D body;
        private Vector2 input;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            input = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));
        }

        private void FixedUpdate()
        {
            Vector3 delta = CalculateDelta(input, moveSpeed, Time.fixedDeltaTime);
            body.MovePosition(body.position + new Vector2(delta.x, delta.y));
        }

        public static Vector3 CalculateDelta(Vector2 input, float speed, float deltaTime)
        {
            Vector2 direction = Vector2.ClampMagnitude(input, 1f);
            return direction * speed * deltaTime;
        }
    }
}
```

- [x] **Step 4: 运行 GREEN 与移动回归**

重新编译运行时、EditMode 测试和两个夹具。

Expected: Rigidbody2D 契约通过；原移动行为夹具 `7/7` 通过；运行时和 EditMode 测试程序集零编译错误。

---

### Task 2: 向场景写入碰撞组件并隐藏 Basic Unit

**Files:**
- Modify: `Assets/Scenes/SampleScene.unity`
- Create: `work/classroom-collision/verify_classroom_collision.ps1`

**Interfaces:**
- Consumes: `Main Character` GameObject fileID `1667414357`；`Basic Unit` GameObject fileID `1424282801`；教室生成对象 fileID 范围 `2000000000` 至 `2000000453`。
- Produces: 主角 Rigidbody2D fileID `2100000000`、CircleCollider2D fileID `2100000001`；20个目标静物的 BoxCollider2D 使用其 GameObject fileID 加3；椅子不含碰撞体。

- [x] **Step 1: 编写失败的场景物理检查**

`verify_classroom_collision.ps1` 必须解析真实 YAML 并断言：

```text
Main Character组件: Rigidbody2D 2100000000、CircleCollider2D 2100000001
Rigidbody2D: BodyType 0、GravityScale 0、Interpolate 1、CollisionDetection 1、Constraints 4
CircleCollider2D: IsTrigger 0、Offset (0,0)、Radius 0.5
BoxCollider2D数量: 20
BoxCollider2D目标GO: 四面墙、教师桌、讲台、12张学生桌、门和储物柜（按本文列出的离散集合）
Basic Unit: m_IsActive 0
```

还要断言非目标对象没有 Collider2D，场景 fileID 不重复。

- [x] **Step 2: 运行场景 RED 检查**

Run: `powershell -File work/classroom-collision/verify_classroom_collision.ps1`

Expected: FAIL，报告缺少主角 Rigidbody2D/Collider2D、BoxCollider2D 实际数量不是20、存在椅子碰撞体，或 `Basic Unit` 仍激活。

- [x] **Step 3: 备份并写入主角物理组件**

备份修改前的 `SampleScene.unity`。在 `Main Character.m_Component` 添加 `2100000000` 和 `2100000001`，序列化 Dynamic Rigidbody2D 与半径0.5的非 Trigger CircleCollider2D。组件参数严格匹配 Global Constraints。

- [x] **Step 4: 写入20个静态 BoxCollider2D**

目标 GameObject fileID 精确为：

```text
2000000100, 2000000110, 2000000120, 2000000130,
2000000150, 2000000160,
2000000170, 2000000190, 2000000210,
2000000230, 2000000250, 2000000270,
2000000290, 2000000310, 2000000330,
2000000350, 2000000370, 2000000390,
2000000440, 2000000450
```

每个 GameObject 的 `m_Component` 添加 fileID `GameObject + 3`。对应 BoxCollider2D 使用 `m_IsTrigger: 0`、`m_Offset: {x: 0, y: 0}`、`m_Size: {x: 1, y: 1}`、`m_EdgeRadius: 0`；实际边界由已有 Transform 缩放形成。

- [x] **Step 5: 隐藏 Basic Unit**

仅将 `Basic Unit` GameObject 块的 `m_IsActive: 1` 改为 `m_IsActive: 0`，不删除对象或修改其 Transform/SpriteRenderer。

- [x] **Step 6: 运行场景 GREEN 与完整回归**

依次运行碰撞检查、方形 Sprite 检查、教室结构检查、Unity Roslyn 运行时与 EditMode 测试程序集编译、移动行为夹具和 Rigidbody2D 契约夹具。

Expected: 所有命令退出码0；碰撞体 `20/20`，椅子碰撞体 `0/12`；移动行为 `7/7`；`Basic Unit` inactive；仅计划内脚本、测试和场景文件发生变化。

- [x] **Step 7: 手动 Play Mode 验收说明**

由于实施期间 Unity 保持关闭，最终交付明确提示用户打开编辑器进入 Play Mode，检查主角不能穿墙或穿过桌椅、能够沿障碍边缘移动、画面中不显示 `Basic Unit`。
