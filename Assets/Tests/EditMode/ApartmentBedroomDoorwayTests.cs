using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Tests
{
    public class ApartmentBedroomDoorwayTests
    {
        [TestCase("Assets/Scenes/ApartmentLinChunbaoBedroom.unity", "Lin Bedroom Exit Door",
            "Lin Bedroom Exit Doorway", 200f, -2.5f, "ApartmentLivingRoom",
            "ApartmentLivingRoomFromLinBedroom")]
        [TestCase("Assets/Scenes/ApartmentProtagonistBedroom.unity", "Protagonist Bedroom Exit Door",
            "Protagonist Bedroom Exit Doorway", 220f, -2.5f, "ApartmentLivingRoom",
            "ApartmentLivingRoomFromProtagonistBedroom")]
        public void BedroomExit_UsesInvisibleDoorwayTransition(
            string scenePath, string removedDoorName, string doorwayName, float x, float y,
            string destinationScene, string destinationId)
        {
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            Assert.That(Find(scene, removedDoorName), Is.Null, removedDoorName);

            GameObject doorway = Find(scene, doorwayName);
            Assert.That(doorway, Is.Not.Null, doorwayName);
            Assert.That((Vector2)doorway.transform.position, Is.EqualTo(new Vector2(x, y)), doorwayName);
            Assert.That(doorway.GetComponent<SpriteRenderer>(), Is.Null, doorwayName);
            Assert.That(doorway.GetComponent<Collider2D>(), Is.Null, doorwayName);

            MapTransitionPoint transition = doorway.GetComponent<MapTransitionPoint>();
            Assert.That(transition, Is.Not.Null, doorwayName);
            Assert.That(GetField<string>(transition, "destinationScene"), Is.EqualTo(destinationScene));
            Assert.That(GetField<string>(transition, "destinationId"), Is.EqualTo(destinationId));
        }

        private static GameObject Find(Scene scene, string objectName)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
                {
                    if (transform.name == objectName) return transform.gameObject;
                }
            }

            return null;
        }

        private static T GetField<T>(object target, string fieldName)
        {
            return (T)target.GetType().GetField(fieldName,
                    BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(target);
        }
    }
}
