using System;
using System.Collections.Generic;
using System.Linq;
using RPG;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LinJiantaiCharacterInstaller
{
    private const string ArtworkPath = "Assets/Art/Characters/lin-jiantai-character-sheet-pixel-v1.png";
    private const string ControllerPath = "Assets/Player_Down_Run_01.controller";
    private const string ClassroomScenePath = "Assets/Scenes/SchoolClassroom.unity";
    private const float VisualScale = 1.2f;

    private static readonly FrameDefinition[] Frames =
    {
        new FrameDefinition("LinJiantai_Idle_Down", 0, 0),
        new FrameDefinition("LinJiantai_Walk_Down_A", 0, 1),
        new FrameDefinition("LinJiantai_Walk_Down_B", 0, 2),
        new FrameDefinition("LinJiantai_Idle_Up", 1, 0),
        new FrameDefinition("LinJiantai_Walk_Up_A", 1, 1),
        new FrameDefinition("LinJiantai_Walk_Up_B", 1, 2),
        new FrameDefinition("LinJiantai_Idle_Right", 2, 0),
        new FrameDefinition("LinJiantai_Walk_Right_A", 2, 1),
        new FrameDefinition("LinJiantai_Walk_Right_B", 2, 2),
        new FrameDefinition("LinJiantai_Idle_Left", 3, 0),
        new FrameDefinition("LinJiantai_Walk_Left_A", 3, 1),
        new FrameDefinition("LinJiantai_Walk_Left_B", 3, 2)
    };

    [MenuItem("Tools/Devil RPG/Apply Lin Jiantai Character Visuals")]
    public static void Apply()
    {
        ConfigureArtworkImporter();
        Dictionary<string, Sprite> sprites = LoadSprites();
        ConfigureAnimationClips(sprites);
        ConfigureAnimatorController();
        ConfigurePlayerVisual();
        AssetDatabase.SaveAssets();
    }

    private static void ConfigureArtworkImporter()
    {
        TextureImporter importer = AssetImporter.GetAtPath(ArtworkPath) as TextureImporter;
        if (importer == null)
        {
            throw new InvalidOperationException("找不到林健太角色精灵表。");
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
                throw new InvalidOperationException($"缺少林健太动画帧：{frame.Name}。");
            }
        }

        return sprites;
    }

    private static void ConfigureAnimationClips(IReadOnlyDictionary<string, Sprite> sprites)
    {
        ConfigureClip("Assets/Character/Player_Idle_Down.anim", sprites["LinJiantai_Idle_Down"]);
        ConfigureClip("Assets/Character/Player_Idle_Up.anim", sprites["LinJiantai_Idle_Up"]);
        ConfigureClip("Assets/Character/Player_Idle_Right.anim", sprites["LinJiantai_Idle_Right"]);
        ConfigureClip("Assets/Character/Player_Idle_Left.anim", sprites["LinJiantai_Idle_Left"]);
        ConfigureClip("Assets/Character/Player_Down_Run.anim", sprites["LinJiantai_Walk_Down_A"],
            sprites["LinJiantai_Walk_Down_B"]);
        ConfigureClip("Assets/Character/Player_Up_Run.anim", sprites["LinJiantai_Walk_Up_A"],
            sprites["LinJiantai_Walk_Up_B"]);
        ConfigureClip("Assets/Character/Player_Right_Run.anim", sprites["LinJiantai_Walk_Right_A"],
            sprites["LinJiantai_Walk_Right_B"]);
        ConfigureClip("Assets/Character/Player_Left_Run.anim", sprites["LinJiantai_Walk_Left_A"],
            sprites["LinJiantai_Walk_Left_B"]);
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
            .Select((sprite, index) => new ObjectReferenceKeyframe
            {
                time = index / 12f,
                value = sprite
            })
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

    private static void ConfigureAnimatorController()
    {
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
        if (controller == null)
        {
            throw new InvalidOperationException("找不到主角 Animator Controller。");
        }

        AnimatorStateMachine machine = controller.layers[0].stateMachine;
        SetStateMotion(machine, "Player_Idle_Down", "Assets/Character/Player_Idle_Down.anim");
        SetStateMotion(machine, "Player_Idle_Up", "Assets/Character/Player_Idle_Up.anim");
        SetStateMotion(machine, "Player_Idle_Right", "Assets/Character/Player_Idle_Right.anim");
        SetStateMotion(machine, "Player_Idle_Left", "Assets/Character/Player_Idle_Left.anim");
        SetStateMotion(machine, "Player_Down_Run", "Assets/Character/Player_Down_Run.anim");
        SetStateMotion(machine, "Player_Up_Run", "Assets/Character/Player_Up_Run.anim");
        SetStateMotion(machine, "Player_Right_Run", "Assets/Character/Player_Right_Run.anim");
        SetStateMotion(machine, "Player_Left_Run", "Assets/Character/Player_Left_Run.anim");
        EditorUtility.SetDirty(controller);
    }

    private static void SetStateMotion(AnimatorStateMachine machine, string stateName, string clipPath)
    {
        AnimatorState state = machine.states
            .Select(child => child.state)
            .FirstOrDefault(candidate => candidate.name == stateName);
        if (state == null)
        {
            state = machine.AddState(stateName);
        }

        state.motion = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
        state.speed = 1f;
    }

    private static void ConfigurePlayerVisual()
    {
        Scene scene = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        MainCharacterMovement movement = UnityEngine.Object.FindObjectOfType<MainCharacterMovement>(true);
        if (movement == null)
        {
            throw new InvalidOperationException("起始教室中找不到主角移动组件。");
        }

        SpriteRenderer legacyRenderer = movement.GetComponent<SpriteRenderer>();
        CircleCollider2D physicalCollider = movement.GetComponent<CircleCollider2D>();
        if (legacyRenderer == null || physicalCollider == null)
        {
            throw new InvalidOperationException("主角缺少旧 SpriteRenderer 或 CircleCollider2D。");
        }

        Transform visual = movement.transform.Find("Player Visual");
        if (visual == null)
        {
            visual = new GameObject("Player Visual").transform;
            visual.SetParent(movement.transform, false);
        }

        SpriteRenderer renderer = visual.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = visual.gameObject.AddComponent<SpriteRenderer>();
        Animator animator = visual.GetComponent<Animator>();
        if (animator == null) animator = visual.gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
        renderer.sprite = LoadSprites()["LinJiantai_Idle_Down"];
        renderer.sortingOrder = legacyRenderer.sortingOrder;
        visual.localPosition = Vector3.zero;
        visual.localScale = Vector3.one * VisualScale;
        renderer.flipX = false;
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
