using System.Linq;
using NUnit.Framework;
using RPG;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Tests
{
    public class ProfessorCharacterSceneTests
    {
        private const string ClassroomScenePath = "Assets/Scenes/SchoolClassroom.unity";

        [Test]
        public void SchoolClassroom_SeparatesProfessorVisualFromExistingProfessorLogic()
        {
            Scene scene = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
            Transform professor = scene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Transform>(true))
                .Single(node => node.name == "Professor");

            SpriteRenderer legacyRenderer = professor.GetComponent<SpriteRenderer>();
            Transform visual = professor.Find("Professor Visual");

            Assert.That(legacyRenderer, Is.Not.Null);
            Assert.That(legacyRenderer.enabled, Is.False);
            Assert.That(visual, Is.Not.Null);
            Assert.That(visual.localScale, Is.EqualTo(Vector3.one * 1.25f));
            Assert.That(visual.GetComponent<Animator>(), Is.Not.Null);
            Assert.That(visual.GetComponent<ProfessorVisualAnimator>(), Is.Not.Null);
            Assert.That(visual.GetComponent<SpriteRenderer>().sortingOrder,
                Is.EqualTo(legacyRenderer.sortingOrder));
            Assert.That(professor.GetComponent<BroadcastDialogueTrigger>(), Is.Not.Null);
        }
    }
}
