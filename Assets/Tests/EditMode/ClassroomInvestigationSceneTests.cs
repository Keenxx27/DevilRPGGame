using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Tests
{
    public class ClassroomInvestigationSceneTests
    {
        private const string ScenePath = "Assets/Scenes/SchoolClassroom.unity";

        [Test]
        public void SchoolClassroom_ConfiguresAllInvestigationsAndHiddenCard()
        {
            Scene scene = OpenSampleScene();
            ClassroomLibraryCardPuzzle[] puzzles =
                Object.FindObjectsOfType<ClassroomLibraryCardPuzzle>(true);
            InvestigationPoint[] points = Object.FindObjectsOfType<InvestigationPoint>(true);

            Assert.That(puzzles.Length, Is.EqualTo(1));
            Assert.That(points.Length, Is.EqualTo(5));

            AssertPoint(scene, "Storage Cabinet Investigation", "Storage Cabinet",
                InvestigationKind.StorageCabinet, puzzles[0]);
            AssertPoint(scene, "Podium Investigation", "Podium",
                InvestigationKind.Podium, puzzles[0]);
            AssertPoint(scene, "Teacher Desk Investigation", "Teacher Desk",
                InvestigationKind.TeacherDesk, puzzles[0]);
            AssertPoint(scene, "Blackboard Investigation", "Blackboard",
                InvestigationKind.Blackboard, puzzles[0]);
            AssertPoint(scene, "Flowerpot Investigation", "Flowerpot",
                InvestigationKind.Flowerpot, puzzles[0]);

            Assert.That(Find(scene, "Flowerpot"), Is.Not.Null);
            GameObject card = Find(scene, "Library Card");
            Assert.That(card, Is.Not.Null);
            Assert.That(card.activeSelf, Is.False);
        }

        [Test]
        public void SchoolClassroom_ConfiguresAllApprovedDialogueText()
        {
            OpenSampleScene();
            ClassroomLibraryCardPuzzle puzzle =
                Object.FindObjectOfType<ClassroomLibraryCardPuzzle>(true);

            Assert.That(puzzle, Is.Not.Null);
            AssertPages(puzzle, "searchIntroPages",
                "肯定不是放在别的同学的课桌。");
            AssertPages(puzzle, "storageCabinetPages",
                "储物柜里没有。看来也没人把它随手塞进来。");
            AssertPages(puzzle, "podiumPages",
                "教授的桌子上也没有。最好别在这里乱翻。");
            AssertPages(puzzle, "teacherDeskPages", "讲台下面空空的。");
            AssertPages(puzzle, "blackboardPages",
                "黑板附近也没有。总不可能夹在粉笔槽里吧。");
            AssertPages(puzzle, "firstFlowerpotPages",
                "花盆下面……好像压着什么。",
                "是我的借书卡。",
                "花盆纹丝不动。真够沉的。",
                "把这种东西藏在这里，很好玩吗？",
                "就因为我脸上的痕迹，他们就觉得可以随便欺负我。",
                "生气又能怎么样……我总不能在教室里和所有人打一架。",
                "得找个够结实的东西，把花盆砸开。");
            AssertPages(puzzle, "repeatFlowerpotPages",
                "还是推不动。得找个东西把它砸开。");
            AssertPages(puzzle, "flowerpotBrokenPages",
                "碎了。借书卡就在下面。");
            AssertPages(puzzle, "cardPickedUpPages",
                "终于拿回来了。该去图书馆了。");

            Assert.That(GetField<DialogueController>(puzzle, "dialogueController"),
                Is.SameAs(Object.FindObjectOfType<DialogueController>(true)));
            Assert.That(GetField<BroadcastDialogueTrigger>(puzzle, "openingBroadcast"),
                Is.SameAs(Object.FindObjectOfType<BroadcastDialogueTrigger>(true)));
        }

        [Test]
        public void SchoolClassroom_ConfiguresTwelveThrowableNonBlockingChairsAndBasicUnitPot()
        {
            Scene scene = OpenSampleScene();
            ClassroomLibraryCardPuzzle puzzle =
                Object.FindObjectOfType<ClassroomLibraryCardPuzzle>(true);
            ThrowableChair[] chairs = GetField<ThrowableChair[]>(puzzle, "chairs");

            Assert.That(chairs, Has.Length.EqualTo(12));
            foreach (ThrowableChair chair in chairs)
            {
                Assert.That(chair, Is.Not.Null);
                Assert.That(chair.name, Does.StartWith("Chair R"));
                Assert.That(chair.GetComponent<WorldItem>(), Is.Not.Null);
                Assert.That(chair.GetComponent<BoxCollider2D>().isTrigger, Is.True);
                Assert.That(chair.GetComponent<WorldItem>().CanPickUp, Is.False);
            }

            GameObject flowerpot = Find(scene, "Flowerpot");
            FlowerpotBreakable breakable = flowerpot.GetComponent<FlowerpotBreakable>();
            Assert.That(breakable, Is.Not.Null);
            Assert.That(GetField<GameObject>(breakable, "libraryCard"),
                Is.SameAs(Find(scene, "Library Card")));

            Sprite basicUnit = Find(scene, "Basic Unit").GetComponent<SpriteRenderer>().sprite;
            AssertBasicUnitPart(scene, "Flowerpot Pot Body", basicUnit,
                new Color(0.45f, 0.23f, 0.10f, 1f));
            AssertBasicUnitPart(scene, "Flowerpot Plant", basicUnit,
                new Color(0.18f, 0.52f, 0.20f, 1f));
            AssertBasicUnitPart(scene, "Flowerpot Shard 1", basicUnit,
                new Color(0.45f, 0.23f, 0.10f, 1f));
        }

        private static Scene OpenSampleScene()
        {
            return EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        }

        private static void AssertPoint(
            Scene scene,
            string pointName,
            string parentName,
            InvestigationKind expectedKind,
            ClassroomLibraryCardPuzzle expectedHandler)
        {
            GameObject pointObject = Find(scene, pointName);
            Assert.That(pointObject, Is.Not.Null, pointName);
            Assert.That(pointObject.transform.parent, Is.Not.Null, pointName);
            Assert.That(pointObject.transform.parent.name, Is.EqualTo(parentName), pointName);

            InvestigationPoint point = pointObject.GetComponent<InvestigationPoint>();
            BoxCollider2D collider = pointObject.GetComponent<BoxCollider2D>();
            Assert.That(point, Is.Not.Null, pointName);
            Assert.That(collider, Is.Not.Null, pointName);
            Assert.That(collider.isTrigger, Is.True, pointName);
            Assert.That(point.Kind, Is.EqualTo(expectedKind), pointName);
            Assert.That(GetField<MonoBehaviour>(point, "handler"),
                Is.SameAs(expectedHandler), pointName);
        }

        private static void AssertPages(
            ClassroomLibraryCardPuzzle puzzle, string fieldName, params string[] expectedText)
        {
            DialoguePage[] pages = GetField<DialoguePage[]>(puzzle, fieldName);
            Assert.That(pages, Has.Length.EqualTo(expectedText.Length), fieldName);
            for (int index = 0; index < expectedText.Length; index++)
            {
                Assert.That(pages[index].Speaker, Is.EqualTo("主角"), fieldName);
                Assert.That(pages[index].Text, Is.EqualTo(expectedText[index]), fieldName);
            }
        }

        private static void AssertBasicUnitPart(
            Scene scene, string objectName, Sprite expectedSprite, Color expectedColor)
        {
            GameObject part = Find(scene, objectName);
            Assert.That(part, Is.Not.Null, objectName);
            SpriteRenderer renderer = part.GetComponent<SpriteRenderer>();
            Assert.That(renderer, Is.Not.Null, objectName);
            Assert.That(renderer.sprite, Is.SameAs(expectedSprite), objectName);
            Assert.That(renderer.color.r, Is.EqualTo(expectedColor.r).Within(0.01f), objectName);
            Assert.That(renderer.color.g, Is.EqualTo(expectedColor.g).Within(0.01f), objectName);
            Assert.That(renderer.color.b, Is.EqualTo(expectedColor.b).Within(0.01f), objectName);
        }

        private static T GetField<T>(object target, string fieldName)
        {
            return (T)target.GetType().GetField(
                    fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(target);
        }

        private static GameObject Find(Scene scene, string objectName)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                GameObject found = FindRecursive(root.transform, objectName);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        private static GameObject FindRecursive(Transform current, string objectName)
        {
            if (current.name == objectName)
            {
                return current.gameObject;
            }

            for (int index = 0; index < current.childCount; index++)
            {
                GameObject found = FindRecursive(current.GetChild(index), objectName);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }
    }
}
