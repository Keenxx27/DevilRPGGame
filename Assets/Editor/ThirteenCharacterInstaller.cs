using System;
using System.Collections.Generic;
using System.Linq;
using RPG;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ThirteenCharacterInstaller
{
    private const string ArtworkPath = "Assets/Art/Characters/thirteen-character-sheet-pixel-v1.png";
    private const string ControllerPath = "Assets/Character/Thirteen.controller";
    private const string CaveScenePath = "Assets/Scenes/FluorescentMushroomCave.unity";

    private static readonly FrameDefinition[] Frames =
    {
        new FrameDefinition("Thirteen_Idle_Down", 0, 0),
        new FrameDefinition("Thirteen_Walk_Down_A", 0, 1),
        new FrameDefinition("Thirteen_Walk_Down_B", 0, 2),
        new FrameDefinition("Thirteen_Idle_Up", 1, 0),
        new FrameDefinition("Thirteen_Walk_Up_A", 1, 1),
        new FrameDefinition("Thirteen_Walk_Up_B", 1, 2),
        new FrameDefinition("Thirteen_Idle_Right", 2, 0),
        new FrameDefinition("Thirteen_Walk_Right_A", 2, 1),
        new FrameDefinition("Thirteen_Walk_Right_B", 2, 2),
        new FrameDefinition("Thirteen_Idle_Left", 3, 0),
        new FrameDefinition("Thirteen_Walk_Left_A", 3, 1),
        new FrameDefinition("Thirteen_Walk_Left_B", 3, 2)
    };

    [MenuItem("Tools/Devil RPG/Apply Thirteen Character Visuals")]
    public static void Apply()
    {
        ConfigureArtworkImporter();
        Dictionary<string, Sprite> sprites = LoadSprites();
        ConfigureAnimationClips(sprites);
        AnimatorController controller = ConfigureAnimatorController();
        ConfigureCaveVisual(sprites, controller);
        AssetDatabase.SaveAssets();
    }

    private static void ConfigureArtworkImporter()
    {
        TextureImporter importer = AssetImporter.GetAtPath(ArtworkPath) as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到十三角色精灵表。");

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = 32;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.alphaIsTransparency = true;
        importer.spritesheet = Frames.Select(frame => new SpriteMetaData
        {
            name = frame.Name,
            rect = new Rect(frame.Column * 48, (2 - frame.Row) * 64, 48, 64),
            alignment = (int)SpriteAlignment.Custom,
            pivot = new Vector2(.5f, 0f)
        }).ToArray();
        EditorUtility.SetDirty(importer);
        AssetDatabase.ImportAsset(ArtworkPath, ImportAssetOptions.ForceUpdate);
    }

    private static Dictionary<string, Sprite> LoadSprites()
    {
        Dictionary<string, Sprite> sprites = AssetDatabase.LoadAllAssetsAtPath(ArtworkPath)
            .OfType<Sprite>()
            .ToDictionary(sprite => sprite.name, sprite => sprite);
        foreach (FrameDefinition frame in Frames)
        {
            if (!sprites.ContainsKey(frame.Name))
            {
                throw new InvalidOperationException($"缺少十三动画帧：{frame.Name}。");
            }
        }

        return sprites;
    }

    private static void ConfigureAnimationClips(IReadOnlyDictionary<string, Sprite> sprites)
    {
        ConfigureClip("Assets/Character/Thirteen_Idle_Down.anim", sprites["Thirteen_Idle_Down"]);
        ConfigureClip("Assets/Character/Thirteen_Idle_Up.anim", sprites["Thirteen_Idle_Up"]);
        ConfigureClip("Assets/Character/Thirteen_Idle_Right.anim", sprites["Thirteen_Idle_Right"]);
        ConfigureClip("Assets/Character/Thirteen_Idle_Left.anim", sprites["Thirteen_Idle_Left"]);
        ConfigureClip("Assets/Character/Thirteen_Down_Run.anim", sprites["Thirteen_Walk_Down_A"], sprites["Thirteen_Walk_Down_B"]);
        ConfigureClip("Assets/Character/Thirteen_Up_Run.anim", sprites["Thirteen_Walk_Up_A"], sprites["Thirteen_Walk_Up_B"]);
        ConfigureClip("Assets/Character/Thirteen_Right_Run.anim", sprites["Thirteen_Walk_Right_A"], sprites["Thirteen_Walk_Right_B"]);
        ConfigureClip("Assets/Character/Thirteen_Left_Run.anim", sprites["Thirteen_Walk_Left_A"], sprites["Thirteen_Walk_Left_B"]);
    }

    private static void ConfigureClip(string path, params Sprite[] frames)
    {
        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
        if (clip == null)
        {
            clip = new AnimationClip { name = System.IO.Path.GetFileNameWithoutExtension(path) };
            AssetDatabase.CreateAsset(clip, path);
        }

        EditorCurveBinding binding = new EditorCurveBinding
        {
            path = string.Empty,
            type = typeof(SpriteRenderer),
            propertyName = "m_Sprite"
        };
        ObjectReferenceKeyframe[] keys = frames
            .Select((sprite, index) => new ObjectReferenceKeyframe { time = index / 12f, value = sprite })
            .Concat(frames.Length == 1
                ? Enumerable.Empty<ObjectReferenceKeyframe>()
                : new[] { new ObjectReferenceKeyframe { time = frames.Length / 12f, value = frames[0] } })
            .ToArray();
        AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);
        clip.frameRate = 12f;
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);
        EditorUtility.SetDirty(clip);
    }

    private static AnimatorController ConfigureAnimatorController()
    {
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
        if (controller == null) controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);

        AnimatorStateMachine machine = controller.layers[0].stateMachine;
        AnimatorState idleDown = SetStateMotion(machine, "Thirteen_Idle_Down", "Assets/Character/Thirteen_Idle_Down.anim");
        SetStateMotion(machine, "Thirteen_Idle_Up", "Assets/Character/Thirteen_Idle_Up.anim");
        SetStateMotion(machine, "Thirteen_Idle_Right", "Assets/Character/Thirteen_Idle_Right.anim");
        SetStateMotion(machine, "Thirteen_Idle_Left", "Assets/Character/Thirteen_Idle_Left.anim");
        SetStateMotion(machine, "Thirteen_Down_Run", "Assets/Character/Thirteen_Down_Run.anim");
        SetStateMotion(machine, "Thirteen_Up_Run", "Assets/Character/Thirteen_Up_Run.anim");
        SetStateMotion(machine, "Thirteen_Right_Run", "Assets/Character/Thirteen_Right_Run.anim");
        SetStateMotion(machine, "Thirteen_Left_Run", "Assets/Character/Thirteen_Left_Run.anim");
        machine.defaultState = idleDown;
        EditorUtility.SetDirty(controller);
        return controller;
    }

    private static AnimatorState SetStateMotion(AnimatorStateMachine machine, string stateName, string clipPath)
    {
        AnimatorState state = machine.states.Select(child => child.state)
            .FirstOrDefault(candidate => candidate.name == stateName);
        if (state == null) state = machine.AddState(stateName);
        state.motion = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
        state.speed = 1f;
        return state;
    }

    private static void ConfigureCaveVisual(IReadOnlyDictionary<string, Sprite> sprites, AnimatorController controller)
    {
        Scene scene = EditorSceneManager.OpenScene(CaveScenePath, OpenSceneMode.Single);
        Transform thirteen = scene.GetRootGameObjects()
            .SelectMany(root => root.GetComponentsInChildren<Transform>(true))
            .Single(transform => transform.name == "？？？");
        SpriteRenderer legacyRenderer = thirteen.GetComponent<SpriteRenderer>();
        ThirteenFollower follower = thirteen.GetComponent<ThirteenFollower>();
        if (legacyRenderer == null || follower == null)
        {
            throw new InvalidOperationException("十三缺少原有 SpriteRenderer 或 ThirteenFollower。");
        }

        Transform visual = thirteen.Find("Thirteen Visual");
        if (visual == null)
        {
            visual = new GameObject("Thirteen Visual").transform;
            visual.SetParent(thirteen, false);
        }

        SpriteRenderer renderer = visual.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = visual.gameObject.AddComponent<SpriteRenderer>();
        Animator animator = visual.GetComponent<Animator>();
        if (animator == null) animator = visual.gameObject.AddComponent<Animator>();
        if (visual.GetComponent<ThirteenVisualAnimator>() == null)
        {
            visual.gameObject.AddComponent<ThirteenVisualAnimator>();
        }

        animator.runtimeAnimatorController = controller;
        renderer.sprite = sprites["Thirteen_Idle_Down"];
        renderer.color = Color.white;
        renderer.sortingOrder = legacyRenderer.sortingOrder;
        renderer.flipX = false;
        visual.localPosition = Vector3.zero;
        visual.localScale = Vector3.one;
        legacyRenderer.enabled = false;
        EditorSceneManager.SaveScene(scene);
    }

    private readonly struct FrameDefinition
    {
        public FrameDefinition(string name, int column, int row)
        {
            Name = name;
            Column = column;
            Row = row;
        }

        public string Name { get; }
        public int Column { get; }
        public int Row { get; }
    }
}
