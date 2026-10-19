using System.Linq;
using NUnit.Framework;
using RPG;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Tests
{
    public class LinChunbaoCharacterSceneTests
    {
        private const string KitchenScenePath = "Assets/Scenes/ApartmentKitchen.unity";

        [Test]
        public void ApartmentKitchen_SeparatesLinChunbaoVisualFromDialogueObject()
        {
            Scene scene = EditorSceneManager.OpenScene(KitchenScenePath, OpenSceneMode.Single);
            Transform linChunbao = scene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Transform>(true))
                .Single(transform => transform.name == "Lin Chunbao");

            SpriteRenderer legacyRenderer = linChunbao.GetComponent<SpriteRenderer>();
            Transform visual = linChunbao.Find("Lin Chunbao Visual");

            Assert.That(linChunbao.GetComponent<LinChunbaoLivingRoomDialogue>(), Is.Not.Null);
            Assert.That(legacyRenderer, Is.Not.Null);
            Assert.That(legacyRenderer.enabled, Is.False);
            Assert.That(visual, Is.Not.Null);
            Assert.That(visual.localScale, Is.EqualTo(Vector3.one * 1.2f));
            Assert.That(visual.GetComponent<SpriteRenderer>().sortingOrder, Is.EqualTo(10));
            Assert.That(visual.GetComponent<Animator>(), Is.Not.Null);
            Assert.That(visual.GetComponent<LinChunbaoVisualAnimator>(), Is.Not.Null);
        }
    }
}
