using System;
using System.Collections.Generic;
using System.Linq;
using RPG;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ProfessorCharacterInstaller
{
    private const string ArtworkPath = "Assets/Art/Characters/professor-character-sheet-pixel-v1.png";
    private const string ControllerPath = "Assets/Character/Professor.controller";
    private const string ClassroomScenePath = "Assets/Scenes/SchoolClassroom.unity";
    private const float VisualScale = 1.25f;

    private static readonly FrameDefinition[] Frames =
    {
        new FrameDefinition("Professor_Idle_Down", 0, 0),
        new FrameDefinition("Professor_Walk_Down_A", 0, 1),
        new FrameDefinition("Professor_Walk_Down_B", 0, 2),
        new FrameDefinition("Professor_Idle_Up", 1, 0),
        new FrameDefinition("Professor_Walk_Up_A", 1, 1),
        new FrameDefinition("Professor_Walk_Up_B", 1, 2),
        new FrameDefinition("Professor_Idle_Right", 2, 0),
        new FrameDefinition("Professor_Walk_Right_A", 2, 1),
        new FrameDefinition("Professor_Walk_Right_B", 2, 2),
        new FrameDefinition("Professor_Idle_Left", 3, 0),
        new FrameDefinition("Professor_Walk_Left_A", 3, 1),
        new FrameDefinition("Professor_Walk_Left_B", 3, 2)
    };

    [MenuItem("Tools/Devil RPG/Apply Professor Character Visuals")]
    public static void Apply()
    {
        ConfigureArtworkImporter();
        Dictionary<string, Sprite> sprites = LoadSprites();
        ConfigureAnimationClips(sprites);
        AnimatorController controller = ConfigureAnimatorController();
        ConfigureClassroomVisual(sprites, controller);
        AssetDatabase.SaveAssets();
    }

    private static void ConfigureArtworkImporter()
    {
        TextureImporter importer = AssetImporter.GetAtPath(ArtworkPath) as TextureImporter;
        if (importer == null)
        {
            throw new InvalidOperationException("找不到教授角色精灵表。");
        }

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
                throw new InvalidOperationException($"缺少教授动画帧：{frame.Name}。");
            }
        }

        return sprites;
    }

    private static void ConfigureAnimationClips(IReadOnlyDictionary<string, Sprite> sprites)
    {
        ConfigureClip("Assets/Character/Professor_Idle_Down.anim", sprites["Professor_Idle_Down"]);
        ConfigureClip("Assets/Character/Professor_Idle_Up.anim", sprites["Professor_Idle_Up"]);
        ConfigureClip("Assets/Character/Professor_Idle_Right.anim", sprites["Professor_Idle_Right"]);
        ConfigureClip("Assets/Character/Professor_Idle_Left.anim", sprites["Professor_Idle_Left"]);
        ConfigureClip("Assets/Character/Professor_Down_Run.anim", sprites["Professor_Walk_Down_A"], sprites["Professor_Walk_Down_B"]);
        ConfigureClip("Assets/Character/Professor_Up_Run.anim", sprites["Professor_Walk_Up_A"], sprites["Professor_Walk_Up_B"]);
        ConfigureClip("Assets/Character/Professor_Right_Run.anim", sprites["Professor_Walk_Right_A"], sprites["Professor_Walk_Right_B"]);
        ConfigureClip("Assets/Character/Professor_Left_Run.anim", sprites["Professor_Walk_Left_A"], sprites["Professor_Walk_Left_B"]);
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
        if (controller == null)
        {
            controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
        }

        AnimatorStateMachine machine = controller.layers[0].stateMachine;
        AnimatorState idleDown = SetStateMotion(machine, "Professor_Idle_Down", "Assets/Character/Professor_Idle_Down.anim");
        SetStateMotion(machine, "Professor_Idle_Up", "Assets/Character/Professor_Idle_Up.anim");
        SetStateMotion(machine, "Professor_Idle_Right", "Assets/Character/Professor_Idle_Right.anim");
        SetStateMotion(machine, "Professor_Idle_Left", "Assets/Character/Professor_Idle_Left.anim");
        SetStateMotion(machine, "Professor_Down_Run", "Assets/Character/Professor_Down_Run.anim");
        SetStateMotion(machine, "Professor_Up_Run", "Assets/Character/Professor_Up_Run.anim");
        SetStateMotion(machine, "Professor_Right_Run", "Assets/Character/Professor_Right_Run.anim");
        SetStateMotion(machine, "Professor_Left_Run", "Assets/Character/Professor_Left_Run.anim");
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

    private static void ConfigureClassroomVisual(IReadOnlyDictionary<string, Sprite> sprites, AnimatorController controller)
    {
        Scene scene = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        Transform professor = scene.GetRootGameObjects()
            .SelectMany(root => root.GetComponentsInChildren<Transform>(true))
            .Single(transform => transform.name == "Professor");
        SpriteRenderer legacyRenderer = professor.GetComponent<SpriteRenderer>();
        BroadcastDialogueTrigger broadcast = professor.GetComponent<BroadcastDialogueTrigger>();
        if (legacyRenderer == null || broadcast == null)
        {
            throw new InvalidOperationException("教授缺少原有 SpriteRenderer 或课堂广播组件。");
        }

        Transform visual = professor.Find("Professor Visual");
        if (visual == null)
        {
            visual = new GameObject("Professor Visual").transform;
            visual.SetParent(professor, false);
        }

        SpriteRenderer renderer = visual.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = visual.gameObject.AddComponent<SpriteRenderer>();
        Animator animator = visual.GetComponent<Animator>();
        if (animator == null) animator = visual.gameObject.AddComponent<Animator>();
        if (visual.GetComponent<ProfessorVisualAnimator>() == null)
        {
            visual.gameObject.AddComponent<ProfessorVisualAnimator>();
        }

        animator.runtimeAnimatorController = controller;
        renderer.sprite = sprites["Professor_Idle_Down"];
        renderer.color = Color.white;
        renderer.sortingOrder = 6;
        renderer.flipX = false;
        visual.localPosition = Vector3.zero;
        visual.localScale = Vector3.one * VisualScale;
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
