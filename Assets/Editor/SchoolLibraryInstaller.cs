using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SchoolLibraryInstaller
{
    private const string ClassroomScenePath = "Assets/Scenes/SchoolClassroom.unity";
    private const string LibraryScenePath = "Assets/Scenes/SchoolLibrary.unity";

    public static void InstallLayout()
    {
        EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        SpriteRenderer basicUnit = FindBasicUnit();
        Sprite sprite = basicUnit.sprite;
        Material material = basicUnit.sharedMaterial;

        if (sprite == null || material == null)
        {
            throw new InvalidOperationException("Basic Unit 必须同时包含 Sprite 与 sharedMaterial。");
        }

        Scene libraryScene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject library = new GameObject("School Library");
        library.transform.position = new Vector3(60f, 0f, 0f);

        CreateUnit(library.transform, "Library Floor", new Vector2(60f, 0f),
            new Vector2(18f, 12f), new Color(0.62f, 0.48f, 0.32f, 1f), -10,
            false, sprite, material);

        Color wall = new Color(0.82f, 0.76f, 0.62f, 1f);
        CreateUnit(library.transform, "Library Wall Top", new Vector2(60f, 6f),
            new Vector2(18f, 0.5f), wall, 0, true, sprite, material);
        CreateUnit(library.transform, "Library Wall Bottom Left", new Vector2(55f, -6f),
            new Vector2(8f, 0.5f), wall, 0, true, sprite, material);
        CreateUnit(library.transform, "Library Wall Bottom Right", new Vector2(65f, -6f),
            new Vector2(8f, 0.5f), wall, 0, true, sprite, material);
        CreateUnit(library.transform, "Library Wall Left", new Vector2(51f, 0f),
            new Vector2(0.5f, 12f), wall, 0, true, sprite, material);
        CreateUnit(library.transform, "Library Wall Right", new Vector2(69f, 0f),
            new Vector2(0.5f, 12f), wall, 0, true, sprite, material);

        Color counter = new Color(0.38f, 0.22f, 0.11f, 1f);
        CreateUnit(library.transform, "Circulation Counter Horizontal",
            new Vector2(54.5f, -3.4f), new Vector2(4.2f, 0.8f), counter, 2, true,
            sprite, material);
        CreateUnit(library.transform, "Circulation Counter Vertical",
            new Vector2(52.8f, -2.2f), new Vector2(0.8f, 3.2f), counter, 2, true,
            sprite, material);

        Color darkWood = new Color(0.28f, 0.16f, 0.08f, 1f);
        CreateUnit(library.transform, "Reading Table Left", new Vector2(56.3f, 1f),
            new Vector2(3.2f, 1.2f), darkWood, 2, true, sprite, material);
        CreateUnit(library.transform, "Reading Table Right", new Vector2(63.7f, 1f),
            new Vector2(3.2f, 1.2f), darkWood, 2, true, sprite, material);
        CreateUnit(library.transform, "Bookcase Top 1", new Vector2(53.3f, 5.2f),
            new Vector2(3f, 0.7f), darkWood, 2, true, sprite, material);
        CreateUnit(library.transform, "Bookcase Top 2", new Vector2(56.7f, 5.2f),
            new Vector2(3f, 0.7f), darkWood, 2, true, sprite, material);
        CreateUnit(library.transform, "Bookcase Top 3", new Vector2(63.3f, 5.2f),
            new Vector2(3f, 0.7f), darkWood, 2, true, sprite, material);
        CreateUnit(library.transform, "Bookcase Top 4", new Vector2(66.7f, 5.2f),
            new Vector2(3f, 0.7f), darkWood, 2, true, sprite, material);
        CreateUnit(library.transform, "Bookcase Left 1", new Vector2(51.8f, 2.7f),
            new Vector2(0.7f, 2.2f), darkWood, 2, true, sprite, material);
        CreateUnit(library.transform, "Bookcase Left 2", new Vector2(51.8f, 0f),
            new Vector2(0.7f, 2.2f), darkWood, 2, true, sprite, material);
        CreateUnit(library.transform, "Bookcase Right 1", new Vector2(68.2f, 2.7f),
            new Vector2(0.7f, 2.2f), darkWood, 2, true, sprite, material);
        CreateUnit(library.transform, "Bookcase Right 2", new Vector2(68.2f, 0f),
            new Vector2(0.7f, 2.2f), darkWood, 2, true, sprite, material);

        Color chair = new Color(0.40f, 0.10f, 0.10f, 1f);
        CreateUnit(library.transform, "Reading Chair Left North", new Vector2(56.3f, 2f),
            new Vector2(0.75f, 0.45f), chair, 3, false, sprite, material);
        CreateUnit(library.transform, "Reading Chair Left South", new Vector2(56.3f, 0f),
            new Vector2(0.75f, 0.45f), chair, 3, false, sprite, material);
        CreateUnit(library.transform, "Reading Chair Right North", new Vector2(63.7f, 2f),
            new Vector2(0.75f, 0.45f), chair, 3, false, sprite, material);
        CreateUnit(library.transform, "Reading Chair Right South", new Vector2(63.7f, 0f),
            new Vector2(0.75f, 0.45f), chair, 3, false, sprite, material);

        EditorSceneManager.SaveScene(libraryScene, LibraryScenePath);
        AddLibraryToBuildSettings();
        AssetDatabase.SaveAssets();
    }

    private static SpriteRenderer FindBasicUnit()
    {
        foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach (SpriteRenderer renderer in root.GetComponentsInChildren<SpriteRenderer>(true))
            {
                if (renderer.name == "Basic Unit")
                {
                    return renderer;
                }
            }
        }

        throw new InvalidOperationException("SchoolClassroom 中找不到 inactive Basic Unit。");
    }

    private static void CreateUnit(Transform parent, string name, Vector2 position,
        Vector2 scale, Color color, int sortingOrder, bool blocking, Sprite sprite,
        Material material)
    {
        GameObject unit = new GameObject(name);
        unit.transform.SetParent(parent, false);
        unit.transform.position = new Vector3(position.x, position.y, 0f);
        unit.transform.localScale = new Vector3(scale.x, scale.y, 1f);

        SpriteRenderer renderer = unit.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sharedMaterial = material;
        renderer.color = color;
        renderer.sortingOrder = sortingOrder;

        if (blocking)
        {
            BoxCollider2D collider = unit.AddComponent<BoxCollider2D>();
            collider.enabled = true;
            collider.isTrigger = false;
        }
    }

    private static void AddLibraryToBuildSettings()
    {
        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.path == LibraryScenePath)
            {
                return;
            }
        }

        scenes.Add(new EditorBuildSettingsScene(LibraryScenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }
}
