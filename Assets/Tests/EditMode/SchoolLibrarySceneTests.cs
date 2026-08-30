using System.Linq;
using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Tests
{
    public class SchoolLibrarySceneTests
    {
        private const string LibraryScenePath = "Assets/Scenes/SchoolLibrary.unity";
        private const string CorridorScenePath = "Assets/Scenes/SchoolCorridor.unity";

        [Test]
        public void BuildSettings_ContainsEnabledSchoolLibraryScene()
        {
            Assert.That(EditorBuildSettings.scenes.Any(scene =>
                scene.enabled && scene.path == LibraryScenePath), Is.True);
        }

        [Test]
        public void SchoolLibrary_ConfiguresCompactBasicUnitLayoutAndExcludesGameplaySystems()
        {
            Scene scene = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);

            string[] requiredLayoutNames =
            {
                "Library Floor",
                "Library Wall Top",
                "Library Wall Bottom Left",
                "Library Wall Bottom Right",
                "Library Wall Left",
                "Library Wall Right",
                "Circulation Counter Horizontal",
                "Circulation Counter Vertical",
                "Reading Table Left",
                "Reading Table Right",
                "Bookcase Top 1",
                "Bookcase Top 2",
                "Bookcase Top 3",
                "Bookcase Top 4",
                "Bookcase Left 1",
                "Bookcase Left 2",
                "Bookcase Right 1",
                "Bookcase Right 2"
            };

            foreach (string layoutName in requiredLayoutNames)
            {
                Assert.That(Find(scene, layoutName), Is.Not.Null, layoutName);
            }

            string[] blockingNames =
            {
                "Library Wall Top",
                "Library Wall Bottom Left",
                "Library Wall Bottom Right",
                "Library Wall Left",
                "Library Wall Right",
                "Circulation Counter Horizontal",
                "Circulation Counter Vertical",
                "Reading Table Left",
                "Reading Table Right"
            };

            foreach (string blockingName in blockingNames)
            {
                AssertBlocking(Find(scene, blockingName));
            }

            AssertNoComponents<WorldItem>(scene);
            AssertNoComponents<ThrowableChair>(scene);
            AssertNoComponents<FlowerpotBreakable>(scene);
            AssertNoComponents<ClassroomLibraryCardPuzzle>(scene);
            AssertNoComponents<PlayerInteraction>(scene);
            AssertNoComponents<Camera>(scene);
        }

        [Test]
        public void CorridorAndLibrary_ConfigureMatchingBidirectionalDoorTransitions()
        {
            Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
            AssertDoor(corridor, "Library Door", "SchoolLibrary", "LibraryFromCorridor");
            GameObject libraryDoor = Find(corridor, "Library Door");
            Assert.That(libraryDoor.transform.position,
                Is.EqualTo(new Vector3(27f, 2.5f, 0f)), "Library Door");
            Assert.That(Find(corridor, "Decorative Door 1"), Is.Null,
                "Decorative Door 1");
            AssertSpawn(corridor, "CorridorFromLibrary", new Vector2(27f, 1.1f),
                new Vector3(29f, 0f, -10f));
            AssertDecorativeDoorUnchanged(corridor, "Decorative Door 2", 31f);
            AssertDecorativeDoorUnchanged(corridor, "Decorative Door 3", 35f);

            Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
            AssertDoor(library, "Library Exit Door", "SchoolCorridor", "CorridorFromLibrary");
            AssertSpawn(library, "LibraryFromCorridor", new Vector2(60f, -4.7f),
                new Vector3(60f, 0f, -10f));
        }

        private static GameObject Find(Scene scene, string objectName)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                Transform[] transforms = root.GetComponentsInChildren<Transform>(true);
                foreach (Transform transform in transforms)
                {
                    if (transform.name == objectName)
                    {
                        return transform.gameObject;
                    }
                }
            }

            return null;
        }

        private static void AssertBlocking(GameObject gameObject)
        {
            Assert.That(gameObject, Is.Not.Null);
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            Assert.That(collider, Is.Not.Null, gameObject.name);
            Assert.That(collider.enabled, Is.True, gameObject.name);
            Assert.That(collider.isTrigger, Is.False, gameObject.name);
        }

        private static void AssertDoor(
            Scene scene, string rootName, string destinationScene, string destinationId)
        {
            GameObject root = Find(scene, rootName);
            Assert.That(root, Is.Not.Null, rootName);

            DoorController door = root.GetComponent<DoorController>();
            Assert.That(door, Is.Not.Null, rootName);

            DoorExitPortal portal = root.GetComponentInChildren<DoorExitPortal>(true);
            Assert.That(portal, Is.Not.Null, rootName + " DoorExitPortal");
            Assert.That(portal.transform.parent, Is.SameAs(root.transform), rootName);
            Assert.That(GetField<string>(portal, "destinationScene"),
                Is.EqualTo(destinationScene), rootName);
            Assert.That(GetField<string>(portal, "destinationId"),
                Is.EqualTo(destinationId), rootName);
            Assert.That(GetField<DoorController>(portal, "door"), Is.SameAs(door), rootName);
        }

        private static void AssertSpawn(
            Scene scene, string id, Vector2 position, Vector3 cameraPosition)
        {
            MapSpawnPoint spawnPoint = Object.FindObjectsOfType<MapSpawnPoint>(true)
                .SingleOrDefault(point => point.gameObject.scene == scene
                    && GetField<string>(point, "id") == id);

            Assert.That(spawnPoint, Is.Not.Null, id);
            Assert.That((Vector2)spawnPoint.transform.position, Is.EqualTo(position), id);
            Assert.That(GetField<Vector3>(spawnPoint, "cameraPosition"),
                Is.EqualTo(cameraPosition), id);
        }

        private static void AssertDecorativeDoorUnchanged(
            Scene scene, string objectName, float expectedX)
        {
            GameObject door = Find(scene, objectName);
            Assert.That(door, Is.Not.Null, objectName);
            Assert.That(door.transform.position, Is.EqualTo(new Vector3(expectedX, 2.5f, 0f)),
                objectName);
            Assert.That(door.GetComponent<SpriteRenderer>(), Is.Not.Null, objectName);
            Assert.That(door.GetComponent<DoorController>(), Is.Null, objectName);
            Assert.That(door.GetComponentInChildren<DoorExitPortal>(true), Is.Null, objectName);
        }

        private static void AssertNoComponents<T>(Scene scene) where T : Component
        {
            Assert.That(Object.FindObjectsOfType<T>(true)
                    .Any(component => component.gameObject.scene == scene),
                Is.False, typeof(T).Name);
        }

        private static T GetField<T>(object target, string fieldName)
        {
            return (T)target.GetType().GetField(fieldName,
                    BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(target);
        }
    }
}
