using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MunicipalIsolationOfficerInstaller
{
    [MenuItem("Tools/Devil RPG/Apply Municipal Isolation Officers")]
    public static void Apply()
    {
        ApplyOfficer("Assets/Art/Characters/municipal-isolation-officer-a-pixel-v1.png", "MunicipalOfficerA", 4, "猎魔机构行政人员 1", 1.05f);
        ApplyOfficer("Assets/Art/Characters/municipal-isolation-officer-b-pixel-v1.png", "MunicipalOfficerB", 3, "猎魔机构行政人员 2", 1f);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyOfficer(string artPath, string prefix, int rows, string rootName, float scale)
    {
        TextureImporter importer = AssetImporter.GetAtPath(artPath) as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到素材：" + artPath);
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(artPath);
        string[] directions = { "Down", "Up", "Right", "Left" };
        SpriteMetaData[] slices = new SpriteMetaData[rows * 4];
        for (int row = 0; row < rows; row++) for (int col = 0; col < 4; col++)
        {
            int x0 = col * texture.width / 4, x1 = (col + 1) * texture.width / 4;
            int y0 = (rows - 1 - row) * texture.height / rows, y1 = (rows - row) * texture.height / rows;
            string action = row == 0 ? "Idle" : row == 1 ? "WalkA" : row == 2 ? "WalkB" : "Notice";
            slices[row * 4 + col] = new SpriteMetaData { name = prefix + "_" + action + "_" + directions[col], rect = new Rect(x0, y0, x1 - x0, y1 - y0), pivot = new Vector2(.5f, 0f), alignment = (int)SpriteAlignment.Custom };
        }
        importer.textureType = TextureImporterType.Sprite; importer.spriteImportMode = SpriteImportMode.Multiple; importer.spritePixelsPerUnit = 32; importer.filterMode = FilterMode.Point; importer.textureCompression = TextureImporterCompression.Uncompressed; importer.spritesheet = slices; AssetDatabase.ImportAsset(artPath, ImportAssetOptions.ForceUpdate);
        var sprites = AssetDatabase.LoadAllAssetsAtPath(artPath).OfType<Sprite>().ToDictionary(s => s.name);
        string controllerPath = "Assets/Character/" + prefix + ".controller";
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath) ?? AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
        if (controller.layers.Length == 0) controller.AddLayer("Base Layer");
        AnimatorStateMachine machine = controller.layers[0].stateMachine; AnimatorState fallback = null;
        foreach (string direction in directions) { Add(machine, prefix + "_Idle_" + direction, sprites[prefix + "_Idle_" + direction], ref fallback); Add(machine, prefix + "_Walk_" + direction, sprites[prefix + "_WalkA_" + direction], sprites[prefix + "_WalkB_" + direction], ref fallback); }
        if (rows == 4) Add(machine, prefix + "_Notice_Down", sprites[prefix + "_Notice_Down"], ref fallback); machine.defaultState = fallback;
        Scene scene = EditorSceneManager.OpenScene("Assets/Scenes/ApartmentLivingRoom.unity", OpenSceneMode.Single); Transform root = scene.GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<Transform>(true)).Single(x => x.name == rootName); SpriteRenderer old = root.GetComponent<SpriteRenderer>(); Transform visual = root.Find("Municipal Officer Visual") ?? new GameObject("Municipal Officer Visual").transform; visual.SetParent(root, false); SpriteRenderer renderer = visual.GetComponent<SpriteRenderer>(); if (renderer == null) renderer = visual.gameObject.AddComponent<SpriteRenderer>(); Animator animator = visual.GetComponent<Animator>(); if (animator == null) animator = visual.gameObject.AddComponent<Animator>(); animator.runtimeAnimatorController = controller; renderer.sprite = sprites[prefix + "_Idle_Down"]; renderer.sortingOrder = old.sortingOrder; visual.localScale = Vector3.one * scale; old.enabled = false; EditorSceneManager.SaveScene(scene);
    }

    private static void Add(AnimatorStateMachine machine, string name, Sprite a, ref AnimatorState fallback) { Add(machine, name, new[] { a }, ref fallback); }
    private static void Add(AnimatorStateMachine machine, string name, Sprite a, Sprite b, ref AnimatorState fallback) { Add(machine, name, new[] { a, b }, ref fallback); }
    private static void Add(AnimatorStateMachine machine, string name, Sprite[] frames, ref AnimatorState fallback) { string path = "Assets/Character/" + name + ".anim"; AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path) ?? new AnimationClip { name = name }; if (AssetDatabase.LoadAssetAtPath<AnimationClip>(path) == null) AssetDatabase.CreateAsset(clip, path); AnimationUtility.SetObjectReferenceCurve(clip, new EditorCurveBinding { type = typeof(SpriteRenderer), propertyName = "m_Sprite" }, frames.Select((s, i) => new ObjectReferenceKeyframe { time = i / 12f, value = s }).ToArray()); AnimatorState state = machine.states.Select(x => x.state).FirstOrDefault(x => x.name == name) ?? machine.AddState(name); state.motion = clip; if (name.EndsWith("Idle_Down")) fallback = state; }
}
