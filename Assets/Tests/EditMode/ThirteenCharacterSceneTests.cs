using System.Linq;
using NUnit.Framework;
using RPG;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Tests
{
    public class ThirteenCharacterSceneTests
    {
        private const string CaveScenePath = "Assets/Scenes/FluorescentMushroomCave.unity";

        [Test]
        public void MushroomCave_SeparatesThirteenVisualFromPersistentFollower()
        {
            Scene scene = EditorSceneManager.OpenScene(CaveScenePath, OpenSceneMode.Single);
            Transform thirteen = scene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Transform>(true))
                .Single(transform => transform.name == "？？？");

            SpriteRenderer legacyRenderer = thirteen.GetComponent<SpriteRenderer>();
            Transform visual = thirteen.Find("Thirteen Visual");

            Assert.That(thirteen.GetComponent<ThirteenFollower>(), Is.Not.Null);
            Assert.That(legacyRenderer, Is.Not.Null);
            Assert.That(legacyRenderer.enabled, Is.False);
            Assert.That(visual, Is.Not.Null);
            Assert.That(visual.localScale, Is.EqualTo(Vector3.one));
            Assert.That(visual.GetComponent<SpriteRenderer>().sortingOrder, Is.EqualTo(5));
            Assert.That(visual.GetComponent<Animator>(), Is.Not.Null);
            Assert.That(visual.GetComponent<ThirteenVisualAnimator>(), Is.Not.Null);
        }
    }
}
