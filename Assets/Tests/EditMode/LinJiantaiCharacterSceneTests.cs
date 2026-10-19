using System.Linq;
using NUnit.Framework;
using RPG;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Tests
{
    public class LinJiantaiCharacterSceneTests
    {
        private const string ClassroomScenePath = "Assets/Scenes/SchoolClassroom.unity";

        [Test]
        public void SchoolClassroom_SeparatesLinJiantaiVisualFromPhysicalCollider()
        {
            Scene scene = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
            MainCharacterMovement player = Object.FindObjectsOfType<MainCharacterMovement>(true)
                .Single(character => character.gameObject.scene == scene);

            SpriteRenderer legacyRenderer = player.GetComponent<SpriteRenderer>();
            CircleCollider2D physicalCollider = player.GetComponent<CircleCollider2D>();
            Transform visual = player.transform.Find("Player Visual");

            Assert.That(legacyRenderer, Is.Not.Null);
            Assert.That(legacyRenderer.enabled, Is.False);
            Assert.That(physicalCollider, Is.Not.Null);
            Assert.That(physicalCollider.radius, Is.EqualTo(0.5f));
            Assert.That(visual, Is.Not.Null);
            Assert.That(visual.localScale, Is.EqualTo(Vector3.one * 1.2f));

            SpriteRenderer visualRenderer = visual.GetComponent<SpriteRenderer>();
            Animator visualAnimator = visual.GetComponent<Animator>();
            Assert.That(visualRenderer, Is.Not.Null);
            Assert.That(visualRenderer.enabled, Is.True);
            Assert.That(visualRenderer.sortingOrder, Is.EqualTo(10));
            Assert.That(visualAnimator, Is.Not.Null);
            Assert.That(visualAnimator.runtimeAnimatorController, Is.Not.Null);
        }
    }
}
