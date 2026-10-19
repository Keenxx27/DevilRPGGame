using System;
using System.Collections.Generic;
using System.Linq;
using RPG;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GangDogBossCharacterInstaller
{
    private const string ArtworkPath = "Assets/Art/Characters/gang-dog-boss-character-sheet-pixel-v1.png";
    private const string ControllerPath = "Assets/Character/GangDogBoss.controller";
    private const string ScenePath = "Assets/Scenes/RubblePile.unity";
    private static readonly string[] Directions = { "Down", "Up", "Right", "Left" };

    [MenuItem("Tools/Devil RPG/Apply Gang Dog Boss Character Visuals")]
    public static void Apply()
    {
        TextureImporter importer = AssetImporter.GetAtPath(ArtworkPath) as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到丧家犬老大精灵表。");
        SpriteMetaData[] metadata = new SpriteMetaData[20];
        for (int row = 0; row < 5; row++) for (int col = 0; col < 4; col++)
        {
            string action = row == 0 ? "Idle" : row == 1 ? "WalkA" : row == 2 ? "WalkB" : row == 3 ? "Charge" : "Stun";
            metadata[row * 4 + col] = new SpriteMetaData { name = "GangDogBoss_" + action + "_" + Directions[col], rect = new Rect(col * 48, (4 - row) * 64, 48, 64), alignment = (int)SpriteAlignment.Custom, pivot = new Vector2(.5f, 0f) };
        }
        importer.textureType = TextureImporterType.Sprite; importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = 32; importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed; importer.alphaIsTransparency = true;
        importer.spritesheet = metadata; EditorUtility.SetDirty(importer); AssetDatabase.ImportAsset(ArtworkPath, ImportAssetOptions.ForceUpdate);
        Dictionary<string, Sprite> sprites = AssetDatabase.LoadAllAssetsAtPath(ArtworkPath).OfType<Sprite>().ToDictionary(sprite => sprite.name, sprite => sprite);
        foreach (string direction in Directions)
        {
            Clip("GangDogBoss_Idle_" + direction, sprites["GangDogBoss_Idle_" + direction]);
            Clip("GangDogBoss_Walk_" + direction, sprites["GangDogBoss_WalkA_" + direction], sprites["GangDogBoss_WalkB_" + direction]);
            Clip("GangDogBoss_Charge_" + direction, sprites["GangDogBoss_Charge_" + direction]);
            Clip("GangDogBoss_Stun_" + direction, sprites["GangDogBoss_Stun_" + direction]);
        }
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath) ?? AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
        if (controller.layers == null || controller.layers.Length == 0) controller.AddLayer("Base Layer");
        AnimatorStateMachine machine = controller.layers[0].stateMachine;
        AnimatorState defaultState = null;
        foreach (string direction in Directions) foreach (string action in new[] { "Idle", "Walk", "Charge", "Stun" })
        {
            string stateName = "GangDogBoss_" + action + "_" + direction;
            AnimatorState state = machine.states.Select(child => child.state).FirstOrDefault(item => item.name == stateName) ?? machine.AddState(stateName);
            state.motion = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Character/" + stateName + ".anim");
            if (stateName == "GangDogBoss_Idle_Down") defaultState = state;
        }
        machine.defaultState = defaultState; EditorUtility.SetDirty(controller);
        Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        Transform boss = scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<Transform>(true)).Single(node => node.name == "丧家犬老大");
        SpriteRenderer legacy = boss.GetComponent<SpriteRenderer>();
        if (legacy == null || boss.GetComponent<GangDogBoss>() == null) throw new InvalidOperationException("丧家犬老大缺少原有组件。");
        Transform visual = boss.Find("Gang Dog Boss Visual");
        if (visual == null) { visual = new GameObject("Gang Dog Boss Visual").transform; visual.SetParent(boss, false); }
        SpriteRenderer renderer = visual.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = visual.gameObject.AddComponent<SpriteRenderer>();
        Animator animator = visual.GetComponent<Animator>();
        if (animator == null) animator = visual.gameObject.AddComponent<Animator>();
        if (visual.GetComponent<GangDogBossVisualAnimator>() == null) visual.gameObject.AddComponent<GangDogBossVisualAnimator>();
        animator.runtimeAnimatorController = controller; renderer.sprite = sprites["GangDogBoss_Idle_Down"]; renderer.color = Color.white; renderer.sortingOrder = legacy.sortingOrder; renderer.flipX = false;
        visual.localPosition = Vector3.zero; visual.localScale = Vector3.one * 1.25f; legacy.enabled = false;
        EditorSceneManager.SaveScene(scene); AssetDatabase.SaveAssets();
    }

    private static void Clip(string name, params Sprite[] frames)
    {
        string path = "Assets/Character/" + name + ".anim";
        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
        if (clip == null) { clip = new AnimationClip { name = name }; AssetDatabase.CreateAsset(clip, path); }
        EditorCurveBinding binding = new EditorCurveBinding { path = string.Empty, type = typeof(SpriteRenderer), propertyName = "m_Sprite" };
        ObjectReferenceKeyframe[] keys = frames.Select((sprite, index) => new ObjectReferenceKeyframe { time = index / 12f, value = sprite }).Concat(frames.Length == 1 ? Enumerable.Empty<ObjectReferenceKeyframe>() : new[] { new ObjectReferenceKeyframe { time = frames.Length / 12f, value = frames[0] } }).ToArray();
        AnimationUtility.SetObjectReferenceCurve(clip, binding, keys); clip.frameRate = 12f;
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip); settings.loopTime = true; AnimationUtility.SetAnimationClipSettings(clip, settings); EditorUtility.SetDirty(clip);
    }
}
