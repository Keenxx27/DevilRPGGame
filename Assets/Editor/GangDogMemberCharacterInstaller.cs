using System;
using System.Collections.Generic;
using System.Linq;
using RPG;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GangDogMemberCharacterInstaller
{
    private const string LowArtwork = "Assets/Art/Characters/gang-dog-low-member-character-sheet-pixel-v1.png";
    private const string AdvancedArtwork = "Assets/Art/Characters/gang-dog-advanced-member-character-sheet-pixel-v1.png";
    private static readonly string[] Directions = { "Down", "Up", "Right", "Left" };

    [MenuItem("Tools/Devil RPG/Apply Gang Dog Member Character Visuals")]
    public static void Apply()
    {
        Dictionary<string, Sprite> lowSprites = ImportSheet(LowArtwork, 3, "GangDogLow");
        Dictionary<string, Sprite> advancedSprites = ImportSheet(AdvancedArtwork, 4, "GangDogAdvanced");
        AnimatorController lowController = BuildController("GangDogLowMember", lowSprites, false);
        AnimatorController advancedController = BuildController("GangDogAdvancedMember", advancedSprites, true);
        ApplyScene("Assets/Scenes/RoadToSuburbs1.unity", "丧家犬低级成员", lowSprites, lowController, 1f);
        ApplyScene("Assets/Scenes/RoadToSuburbs2.unity", "丧家犬高级成员", advancedSprites, advancedController, 1.05f);
        AssetDatabase.SaveAssets();
    }

    private static Dictionary<string, Sprite> ImportSheet(string path, int rows, string prefix)
    {
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到丧家犬成员精灵表：" + path);
        SpriteMetaData[] metadata = new SpriteMetaData[rows * 4];
        for (int row = 0; row < rows; row++) for (int column = 0; column < 4; column++)
        {
            string action = row == 0 ? "Idle" : row == 1 ? "WalkA" : row == 2 ? "WalkB" : "Swing";
            metadata[row * 4 + column] = new SpriteMetaData {
                name = prefix + "_" + action + "_" + Directions[column],
                rect = new Rect(column * 48, (rows - 1 - row) * 64, 48, 64),
                alignment = (int)SpriteAlignment.Custom, pivot = new Vector2(.5f, 0f)
            };
        }
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = 32;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.alphaIsTransparency = true;
        importer.spritesheet = metadata;
        EditorUtility.SetDirty(importer);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        return AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToDictionary(sprite => sprite.name, sprite => sprite);
    }

    private static AnimatorController BuildController(string controllerName, Dictionary<string, Sprite> sprites, bool advanced)
    {
        string prefix = advanced ? "GangDogAdvanced" : "GangDogLow";
        string path = "Assets/Character/" + controllerName + ".controller";
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path) ?? AnimatorController.CreateAnimatorControllerAtPath(path);
        if (controller.layers == null || controller.layers.Length == 0) controller.AddLayer("Base Layer");
        AnimatorStateMachine machine = controller.layers[0].stateMachine;
        AnimatorState defaultState = null;
        foreach (string direction in Directions)
        {
            CreateState(machine, prefix + "_Idle_" + direction, new[] { sprites[prefix + "_Idle_" + direction] }, ref defaultState);
            CreateState(machine, prefix + "_Walk_" + direction, new[] { sprites[prefix + "_WalkA_" + direction], sprites[prefix + "_WalkB_" + direction] }, ref defaultState);
            if (advanced) CreateState(machine, prefix + "_Swing_" + direction, new[] { sprites[prefix + "_Swing_" + direction] }, ref defaultState);
        }
        machine.defaultState = defaultState;
        EditorUtility.SetDirty(controller);
        return controller;
    }

    private static void CreateState(AnimatorStateMachine machine, string name, Sprite[] frames, ref AnimatorState defaultState)
    {
        string clipPath = "Assets/Character/" + name + ".anim";
        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
        if (clip == null) { clip = new AnimationClip { name = name }; AssetDatabase.CreateAsset(clip, clipPath); }
        EditorCurveBinding binding = new EditorCurveBinding { path = string.Empty, type = typeof(SpriteRenderer), propertyName = "m_Sprite" };
        ObjectReferenceKeyframe[] keys = frames.Select((frame, index) => new ObjectReferenceKeyframe { time = index / 12f, value = frame }).ToArray();
        AnimationUtility.SetObjectReferenceCurve(clip, binding, keys); clip.frameRate = 12f;
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip); settings.loopTime = !name.Contains("_Swing_"); AnimationUtility.SetAnimationClipSettings(clip, settings);
        AnimatorState state = machine.states.Select(child => child.state).FirstOrDefault(item => item.name == name) ?? machine.AddState(name);
        state.motion = clip;
        if (name.EndsWith("Idle_Down")) defaultState = state;
    }

    private static void ApplyScene(string path, string rootName, Dictionary<string, Sprite> sprites, AnimatorController controller, float scale)
    {
        Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
        foreach (Transform root in scene.GetRootGameObjects().SelectMany(item => item.GetComponentsInChildren<Transform>(true)).Where(item => item.name.StartsWith(rootName)))
        {
            SpriteRenderer legacy = root.GetComponent<SpriteRenderer>();
            if (legacy == null) throw new InvalidOperationException(root.name + " 缺少占位 SpriteRenderer。");
            Transform visual = root.Find("Gang Dog Member Visual");
            if (visual == null) { visual = new GameObject("Gang Dog Member Visual").transform; visual.SetParent(root, false); }
            SpriteRenderer renderer = visual.GetComponent<SpriteRenderer>(); if (renderer == null) renderer = visual.gameObject.AddComponent<SpriteRenderer>();
            Animator animator = visual.GetComponent<Animator>(); if (animator == null) animator = visual.gameObject.AddComponent<Animator>();
            if (visual.GetComponent<GangDogMemberVisualAnimator>() == null) visual.gameObject.AddComponent<GangDogMemberVisualAnimator>();
            animator.runtimeAnimatorController = controller;
            renderer.sprite = sprites[controller.name.Contains("Advanced") ? "GangDogAdvanced_Idle_Down" : "GangDogLow_Idle_Down"];
            renderer.color = Color.white; renderer.sortingOrder = legacy.sortingOrder; renderer.flipX = false;
            visual.localPosition = Vector3.zero; visual.localScale = Vector3.one * scale; legacy.enabled = false;
        }
        EditorSceneManager.SaveScene(scene);
    }
}
