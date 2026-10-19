using System;
using System.Collections.Generic;
using System.Linq;
using RPG;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ClassroomStudentCharacterInstaller
{
    private const string ClassroomScenePath = "Assets/Scenes/SchoolClassroom.unity";
    private const float VisualScale = 1.1f;
    private const int ClassroomListeningSortingOrder = 4;
    private static readonly StudentDefinition[] Students =
    {
        new StudentDefinition("Classroom Student 1", "StudentA", "Assets/Art/Characters/classroom-student-a-male-pixel-v1.png", "Assets/Character/ClassroomStudentA.controller"),
        new StudentDefinition("Classroom Student 2", "StudentB", "Assets/Art/Characters/classroom-student-b-male-pixel-v1.png", "Assets/Character/ClassroomStudentB.controller"),
        new StudentDefinition("Classroom Student 3", "StudentC", "Assets/Art/Characters/classroom-student-a-female-pixel-v1.png", "Assets/Character/ClassroomStudentC.controller"),
        new StudentDefinition("Classroom Student 5", "StudentD", "Assets/Art/Characters/classroom-student-b-female-pixel-v1.png", "Assets/Character/ClassroomStudentD.controller")
    };

    [MenuItem("Tools/Devil RPG/Apply Classroom Student Character Visuals")]
    public static void Apply()
    {
        Dictionary<string, StudentAssets> assets = Students.ToDictionary(definition => definition.RootName, ConfigureAssets);
        ConfigureScene(assets);
        AssetDatabase.SaveAssets();
    }

    private static StudentAssets ConfigureAssets(StudentDefinition definition)
    {
        TextureImporter importer = AssetImporter.GetAtPath(definition.ArtworkPath) as TextureImporter;
        if (importer == null) throw new InvalidOperationException($"找不到学生素材：{definition.ArtworkPath}");
        FrameDefinition[] frames = BuildFrames(definition.Prefix);
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = 32;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.alphaIsTransparency = true;
        importer.spritesheet = frames.Select(frame => new SpriteMetaData
        {
            name = frame.Name,
            rect = new Rect(frame.Column * 48, (2 - frame.Row) * 64, 48, 64),
            alignment = (int)SpriteAlignment.Custom,
            pivot = new Vector2(.5f, 0f)
        }).ToArray();
        EditorUtility.SetDirty(importer);
        AssetDatabase.ImportAsset(definition.ArtworkPath, ImportAssetOptions.ForceUpdate);
        Dictionary<string, Sprite> sprites = AssetDatabase.LoadAllAssetsAtPath(definition.ArtworkPath)
            .OfType<Sprite>().ToDictionary(sprite => sprite.name, sprite => sprite);
        foreach (FrameDefinition frame in frames)
            if (!sprites.ContainsKey(frame.Name)) throw new InvalidOperationException($"缺少学生动画帧：{frame.Name}");

        ConfigureClip($"Assets/Character/{definition.Prefix}_Idle_Down.anim", sprites[$"{definition.Prefix}_Idle_Down"]);
        ConfigureClip($"Assets/Character/{definition.Prefix}_Idle_Up.anim", sprites[$"{definition.Prefix}_Idle_Up"]);
        ConfigureClip($"Assets/Character/{definition.Prefix}_Idle_Right.anim", sprites[$"{definition.Prefix}_Idle_Right"]);
        ConfigureClip($"Assets/Character/{definition.Prefix}_Idle_Left.anim", sprites[$"{definition.Prefix}_Idle_Left"]);
        ConfigureClip($"Assets/Character/{definition.Prefix}_Down_Run.anim", sprites[$"{definition.Prefix}_Walk_Down_A"], sprites[$"{definition.Prefix}_Walk_Down_B"]);
        ConfigureClip($"Assets/Character/{definition.Prefix}_Up_Run.anim", sprites[$"{definition.Prefix}_Walk_Up_A"], sprites[$"{definition.Prefix}_Walk_Up_B"]);
        ConfigureClip($"Assets/Character/{definition.Prefix}_Right_Run.anim", sprites[$"{definition.Prefix}_Walk_Right_A"], sprites[$"{definition.Prefix}_Walk_Right_B"]);
        ConfigureClip($"Assets/Character/{definition.Prefix}_Left_Run.anim", sprites[$"{definition.Prefix}_Walk_Left_A"], sprites[$"{definition.Prefix}_Walk_Left_B"]);
        return new StudentAssets(sprites, ConfigureController(definition));
    }

    private static FrameDefinition[] BuildFrames(string prefix) => new[]
    {
        new FrameDefinition($"{prefix}_Idle_Down", 0, 0), new FrameDefinition($"{prefix}_Walk_Down_A", 0, 1), new FrameDefinition($"{prefix}_Walk_Down_B", 0, 2),
        new FrameDefinition($"{prefix}_Idle_Up", 1, 0), new FrameDefinition($"{prefix}_Walk_Up_A", 1, 1), new FrameDefinition($"{prefix}_Walk_Up_B", 1, 2),
        new FrameDefinition($"{prefix}_Idle_Right", 2, 0), new FrameDefinition($"{prefix}_Walk_Right_A", 2, 1), new FrameDefinition($"{prefix}_Walk_Right_B", 2, 2),
        new FrameDefinition($"{prefix}_Idle_Left", 3, 0), new FrameDefinition($"{prefix}_Walk_Left_A", 3, 1), new FrameDefinition($"{prefix}_Walk_Left_B", 3, 2)
    };

    private static void ConfigureClip(string path, params Sprite[] frames)
    {
        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path) ?? new AnimationClip { name = System.IO.Path.GetFileNameWithoutExtension(path) };
        if (AssetDatabase.LoadAssetAtPath<AnimationClip>(path) == null) AssetDatabase.CreateAsset(clip, path);
        EditorCurveBinding binding = new EditorCurveBinding { path = string.Empty, type = typeof(SpriteRenderer), propertyName = "m_Sprite" };
        ObjectReferenceKeyframe[] keys = frames.Select((sprite, index) => new ObjectReferenceKeyframe { time = index / 12f, value = sprite })
            .Concat(frames.Length == 1 ? Enumerable.Empty<ObjectReferenceKeyframe>() : new[] { new ObjectReferenceKeyframe { time = frames.Length / 12f, value = frames[0] } }).ToArray();
        AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);
        clip.frameRate = 12f;
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);
        EditorUtility.SetDirty(clip);
    }

    private static AnimatorController ConfigureController(StudentDefinition definition)
    {
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(definition.ControllerPath) ?? AnimatorController.CreateAnimatorControllerAtPath(definition.ControllerPath);
        if (controller.layers == null || controller.layers.Length == 0) controller.AddLayer("Base Layer");
        AnimatorStateMachine machine = controller.layers[0].stateMachine;
        AnimatorState initial = null;
        foreach (string suffix in new[] { "Idle_Down", "Idle_Up", "Idle_Right", "Idle_Left", "Down_Run", "Up_Run", "Right_Run", "Left_Run" })
        {
            string name = $"{definition.Prefix}_{suffix}";
            AnimatorState state = machine.states.Select(child => child.state).FirstOrDefault(candidate => candidate.name == name) ?? machine.AddState(name);
            state.motion = AssetDatabase.LoadAssetAtPath<AnimationClip>($"Assets/Character/{name}.anim");
            if (suffix == "Idle_Up") initial = state;
        }
        machine.defaultState = initial;
        EditorUtility.SetDirty(controller);
        return controller;
    }

    private static void ConfigureScene(IReadOnlyDictionary<string, StudentAssets> assets)
    {
        Scene scene = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        Transform[] hierarchy = scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<Transform>(true)).ToArray();
        foreach (StudentDefinition definition in Students)
        {
            Transform root = hierarchy.Single(node => node.name == definition.RootName);
            SpriteRenderer legacy = root.GetComponent<SpriteRenderer>();
            if (legacy == null) throw new InvalidOperationException($"学生缺少原始 Renderer：{definition.RootName}");
            Transform visual = root.Find("Student Visual");
            if (visual == null) { visual = new GameObject("Student Visual").transform; visual.SetParent(root, false); }
            SpriteRenderer renderer = visual.GetComponent<SpriteRenderer>();
            if (renderer == null) renderer = visual.gameObject.AddComponent<SpriteRenderer>();
            Animator animator = visual.GetComponent<Animator>();
            if (animator == null) animator = visual.gameObject.AddComponent<Animator>();
            ClassroomStudentVisualAnimator driver = visual.GetComponent<ClassroomStudentVisualAnimator>();
            if (driver == null) driver = visual.gameObject.AddComponent<ClassroomStudentVisualAnimator>();
            StudentAssets studentAssets = assets[definition.RootName];
            animator.runtimeAnimatorController = studentAssets.Controller;
            renderer.sprite = studentAssets.Sprites[$"{definition.Prefix}_Idle_Up"];
            renderer.color = Color.white; renderer.sortingOrder = ClassroomListeningSortingOrder; renderer.flipX = false;
            visual.localPosition = Vector3.zero; visual.localScale = Vector3.one * VisualScale;
            driver.AnimationPrefix = definition.Prefix;
            legacy.enabled = false;
        }
        EditorSceneManager.SaveScene(scene);
    }

    private readonly struct StudentDefinition { public StudentDefinition(string rootName, string prefix, string artworkPath, string controllerPath) { RootName = rootName; Prefix = prefix; ArtworkPath = artworkPath; ControllerPath = controllerPath; } public string RootName { get; } public string Prefix { get; } public string ArtworkPath { get; } public string ControllerPath { get; } }
    private readonly struct StudentAssets { public StudentAssets(Dictionary<string, Sprite> sprites, AnimatorController controller) { Sprites = sprites; Controller = controller; } public Dictionary<string, Sprite> Sprites { get; } public AnimatorController Controller { get; } }
    private readonly struct FrameDefinition { public FrameDefinition(string name, int column, int row) { Name = name; Column = column; Row = row; } public string Name { get; } public int Column { get; } public int Row { get; } }
}
