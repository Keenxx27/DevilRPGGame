using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Tests
{
    public class FluorescentMushroomCaveGeneratorTests
    {
        private const string ScenePath = "Assets/Scenes/FluorescentMushroomCave.unity";

        [Test]
        public void MushroomArtworkGenerator_PreservesApprovedLayout()
        {
            Type installer = Type.GetType("SchoolLibraryInstaller, Assembly-CSharp-Editor");
            Assert.That(installer, Is.Not.Null);
            installer.GetMethod("ApplyFluorescentMushroomCaveArtwork",
                BindingFlags.Static | BindingFlags.Public)?.Invoke(null, null);
            Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            AssertLayout(scene, "最大荧光蘑菇丛 Artwork", new Vector2(-0.37f, 0.6f),
                new Vector2(0.095231265f, 0.15515949f));
            AssertLayout(scene, "荧光蘑菇丛 左 Artwork", new Vector2(0f, 0.8682f),
                new Vector2(0.06510417f, 0.26722416f));
            AssertLayout(scene, "荧光蘑菇丛 右 Artwork", new Vector2(0.07f, -0.26f),
                new Vector2(0.07251953f, 0.18821289f));
        }

        [Test]
        public void ThirteenArtworkGenerator_PreservesApprovedVisualOffset()
        {
            Dictionary<string, Sprite> sprites = AssetDatabase
                .LoadAllAssetsAtPath("Assets/Art/Characters/thirteen-character-sheet-pixel-v1.png")
                .OfType<Sprite>()
                .ToDictionary(sprite => sprite.name, sprite => sprite);
            RuntimeAnimatorController controller = AssetDatabase
                .LoadAssetAtPath<RuntimeAnimatorController>("Assets/Character/Thirteen.controller");
            Type installer = Type.GetType("ThirteenCharacterInstaller, Assembly-CSharp-Editor");
            Assert.That(installer, Is.Not.Null);
            MethodInfo configure = installer.GetMethod("ConfigureCaveVisual",
                BindingFlags.Static | BindingFlags.NonPublic);

            Assert.That(configure, Is.Not.Null);
            configure.Invoke(null, new object[] { sprites, controller });
            Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            AssertLayout(scene, "Thirteen Visual", new Vector2(-1.1f, -0.83f), Vector2.one);
        }

        private static void AssertLayout(Scene scene, string objectName, Vector2 position, Vector2 scale)
        {
            Transform target = scene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Transform>(true))
                .Single(transform => transform.name == objectName);

            Assert.That((Vector2)target.localPosition, Is.EqualTo(position));
            Assert.That((Vector2)target.localScale, Is.EqualTo(scale));
        }
    }
}
