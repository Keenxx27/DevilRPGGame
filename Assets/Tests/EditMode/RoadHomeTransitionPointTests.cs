using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Tests
{
    public class RoadHomeTransitionPointTests
    {
        [Test]
        public void RoadHomeExit_UsesApprovedInvisibleTriggerBounds()
        {
            Scene scene = EditorSceneManager.OpenScene("Assets/Scenes/RoadHome.unity", OpenSceneMode.Single);
            GameObject exit = Find(scene, "回到公寓外");

            Assert.That(exit, Is.Not.Null);
            Assert.That((Vector2)exit.transform.localPosition,
                Is.EqualTo(new Vector2(-9.5f, -0.5009f)));
            Assert.That((Vector2)exit.transform.localScale,
                Is.EqualTo(new Vector2(0.5f, 10.537516f)));
            Assert.That(exit.GetComponent<SpriteRenderer>().enabled, Is.False);
            BoxCollider2D trigger = exit.GetComponent<BoxCollider2D>();
            Assert.That(trigger, Is.Not.Null);
            Assert.That(trigger.isTrigger, Is.True);
        }

        [Test]
        public void TransitionPoint_UsesOptionalColliderAsActivationArea()
        {
            GameObject pointObject = new GameObject("Transition Point");
            pointObject.transform.localScale = new Vector3(0.5f, 10f, 1f);
            BoxCollider2D trigger = pointObject.AddComponent<BoxCollider2D>();
            trigger.isTrigger = true;
            MapTransitionPoint point = pointObject.AddComponent<MapTransitionPoint>();
            MethodInfo method = typeof(MapTransitionPoint).GetMethod("IsWithinActivationArea",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(method, Is.Not.Null);
            Assert.That((bool)method.Invoke(point, new object[] { new Vector2(0f, 4f) }), Is.True);
            Assert.That((bool)method.Invoke(point, new object[] { new Vector2(1f, 4f) }), Is.False);

            Object.DestroyImmediate(pointObject);
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
    }
}
