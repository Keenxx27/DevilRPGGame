using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEngine;

namespace RPG.Tests
{
    public class MapTransitionTests
    {
        private readonly List<GameObject> createdObjects = new List<GameObject>();

        [TearDown]
        public void TearDown()
        {
            foreach (GameObject createdObject in createdObjects)
            {
                if (createdObject != null)
                {
                    Object.DestroyImmediate(createdObject);
                }
            }

            createdObjects.Clear();
        }

        [Test]
        public void SpawnPoint_RegistersByUniqueIdWhileEnabled()
        {
            MapSpawnPoint spawn = CreateSpawn(
                "CorridorFromClassroom", new Vector2(30f, 0f), new Vector3(30f, 0f, -10f));
            EditModeTestLifecycle.Invoke(spawn, "OnEnable");

            Assert.That(MapSpawnPoint.TryFind("CorridorFromClassroom", out MapSpawnPoint found),
                Is.True);
            Assert.That(found, Is.SameAs(spawn));

            spawn.gameObject.SetActive(false);
            EditModeTestLifecycle.Invoke(spawn, "OnDisable");
            Assert.That(MapSpawnPoint.TryFind("CorridorFromClassroom", out _), Is.False);
        }

        [Test]
        public void ApplyDestination_MovesPlayerAndCameraAndClearsVelocity()
        {
            GameObject playerObject = CreateObject("Player");
            Rigidbody2D player = playerObject.AddComponent<Rigidbody2D>();
            player.position = new Vector2(1f, 2f);
            player.velocity = new Vector2(3f, 4f);

            GameObject cameraObject = CreateObject("Camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            MapSpawnPoint spawn = CreateSpawn(
                "CorridorFromClassroom", new Vector2(30f, 1f), new Vector3(30f, 0f, -10f));

            MapTransitionService.ApplyDestination(player, camera, spawn);

            Assert.That(player.position.x, Is.EqualTo(30f).Within(0.0001f));
            Assert.That(player.position.y, Is.EqualTo(1f).Within(0.0001f));
            Assert.That(player.velocity, Is.EqualTo(Vector2.zero));
            Assert.That(camera.transform.position, Is.EqualTo(new Vector3(30f, 0f, -10f)));
        }

        [TestCase(false, true, false)]
        [TestCase(true, false, false)]
        [TestCase(true, true, true)]
        public void Portal_RequiresFullyOpenDoorAndPlayer(
            bool doorFullyOpen, bool isPlayer, bool expected)
        {
            Assert.That(DoorExitPortal.CanTransition(doorFullyOpen, isPlayer), Is.EqualTo(expected));
        }

        private MapSpawnPoint CreateSpawn(string id, Vector2 position, Vector3 cameraPosition)
        {
            GameObject spawnObject = CreateObject("Spawn", false);
            spawnObject.transform.position = position;
            MapSpawnPoint spawn = spawnObject.AddComponent<MapSpawnPoint>();
            SetPrivateField(spawn, "id", id);
            SetPrivateField(spawn, "cameraPosition", cameraPosition);
            spawnObject.SetActive(true);
            return spawn;
        }

        private GameObject CreateObject(string name, bool active = true)
        {
            var gameObject = new GameObject(name);
            gameObject.SetActive(active);
            createdObjects.Add(gameObject);
            return gameObject;
        }

        private static void SetPrivateField<T>(object target, string name, T value)
        {
            FieldInfo field = target.GetType().GetField(
                name, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(target, value);
        }
    }
}
