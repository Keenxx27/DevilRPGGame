using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Tests
{
    public class SuburbsTransitionPointTests
    {
        [TestCase("Assets/Scenes/RoadToSuburbs1.unity", "Return To Apartment Sign", 9.73f, -4.57f, 0.32f, 2.2f)]
        [TestCase("Assets/Scenes/RoadToSuburbs1.unity", "Road To Suburbs 2 Transition", -9.7f, 1.0704f, 0.32f, 2.3991f)]
        [TestCase("Assets/Scenes/RoadToSuburbs2.unity", "Rubble Pile Transition", -9.6582f, -4.4535f, 0.50349f, 2.2929199f)]
        [TestCase("Assets/Scenes/RoadToSuburbs2.unity", "Return To Road 1 Sign", 9.63f, 1.3f, 0.32f, 2.2f)]
        public void Transition_UsesApprovedInvisibleTriggerBounds(string scenePath, string objectName,
            float positionX, float positionY, float scaleX, float scaleY)
        {
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            GameObject transition = Find(scene, objectName);

            Assert.That(transition, Is.Not.Null);
            Assert.That((Vector2)transition.transform.localPosition,
                Is.EqualTo(new Vector2(positionX, positionY)));
            Assert.That((Vector2)transition.transform.localScale,
                Is.EqualTo(new Vector2(scaleX, scaleY)));
            Assert.That(transition.GetComponent<SpriteRenderer>().enabled, Is.False);
            BoxCollider2D trigger = transition.GetComponent<BoxCollider2D>();
            Assert.That(trigger, Is.Not.Null);
            Assert.That(trigger.isTrigger, Is.True);
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
