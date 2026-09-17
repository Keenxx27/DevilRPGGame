using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG;

public static class SchoolLibraryInstaller
{
    private const string ClassroomScenePath = "Assets/Scenes/SchoolClassroom.unity";
    private const string CorridorScenePath = "Assets/Scenes/SchoolCorridor.unity";
    private const string LibraryScenePath = "Assets/Scenes/SchoolLibrary.unity";
    private const string StorageRoomScenePath = "Assets/Scenes/SchoolStorageRoom.unity";
    private const string ApartmentEntranceScenePath = "Assets/Scenes/ApartmentEntrance.unity";
    private const string ApartmentLivingRoomScenePath = "Assets/Scenes/ApartmentLivingRoom.unity";
    private const string ApartmentKitchenScenePath = "Assets/Scenes/ApartmentKitchen.unity";
    private const string ApartmentBathroomScenePath = "Assets/Scenes/ApartmentBathroom.unity";
    private const string ApartmentLinChunbaoBedroomScenePath = "Assets/Scenes/ApartmentLinChunbaoBedroom.unity";
    private const string GameInstructionsScenePath = "Assets/Scenes/GameInstructions.unity";

    [MenuItem("Tools/Devil RPG/Install Game Instructions")]
    public static void InstallGameInstructions()
    {
        Scene instructions = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject root = new GameObject("Game Instructions");
        root.AddComponent<GameInstructionsStart>();
        EditorSceneManager.SaveScene(instructions, GameInstructionsScenePath);

        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        scenes.RemoveAll(scene => scene.path == GameInstructionsScenePath);
        scenes.Insert(0, new EditorBuildSettingsScene(GameInstructionsScenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install School Library")]
    public static void Install()
    {
        InstallLayout();
        InstallCorridorDoor();
        InstallLibraryExit();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Repair Library Door Visuals")]
    public static void RepairDoorVisuals()
    {
        RepairDoorVisual(CorridorScenePath, "Library Door", "Library Door Panel");
        RepairDoorVisual(LibraryScenePath, "Library Exit Door", "Library Exit Door Panel");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Classroom Dismissal")]
    public static void InstallClassroomDismissal()
    {
        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject professor = FindInScene(classroom, "Professor");
        BroadcastDialogueTrigger broadcast = professor.GetComponent<BroadcastDialogueTrigger>();
        if (broadcast == null)
        {
            throw new InvalidOperationException("Professor 缺少开场对话触发器。");
        }

        SetBool(broadcast, "autoTrigger", false);
        SetDialoguePages(broadcast, new[]
        {
            new DialoguePage("主角", "（左脸被归墟侵蚀的痕迹……正在发热、灼痛。）"),
            new DialoguePage("主角", "如果它们只是动物。"),
            new DialoguePage("主角", "如果它们中，有比动物更复杂的存在。"),
            new DialoguePage("主角", "我们到底在做什么？"),
            new DialoguePage("主角", "也许我该去图书馆看看……"),
            new DialoguePage("主角", "不知道我的借书卡去哪里了，"),
            new DialoguePage("主角", "可能是被哪个家伙藏起来了。")
        });

        Transform classroomRoot = professor.transform.parent;
        SpriteRenderer source = professor.GetComponent<SpriteRenderer>();
        ArrangeClassroomSeating(classroom);
        Vector2[] seats =
        {
            new Vector2(0f, -0.35f), new Vector2(-4f, -1.85f),
            new Vector2(-4f, 1.15f), new Vector2(4f, 1.15f),
            new Vector2(4f, -3.35f)
        };

        GameObject extraStudent = FindOptionalInScene(classroom, "Classroom Student 6");
        if (extraStudent != null) UnityEngine.Object.DestroyImmediate(extraStudent);
        for (int index = 0; index < seats.Length; index++)
        {
            GameObject student = FindOrCreateObject(classroomRoot, "Classroom Student " + (index + 1),
                seats[index]);
            student.transform.localScale = Vector3.one;
            SpriteRenderer renderer = student.GetComponent<SpriteRenderer>();
            if (renderer == null) renderer = student.AddComponent<SpriteRenderer>();
            renderer.sprite = source.sprite;
            renderer.sharedMaterial = source.sharedMaterial;
            renderer.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            renderer.sortingOrder = source.sortingOrder;
        }

        GameObject sequenceObject = FindOrCreateObject(classroomRoot,
            "Classroom Dismissal Sequence", Vector2.zero);
        if (sequenceObject.GetComponent<ClassroomDismissalSequence>() == null)
        {
            sequenceObject.AddComponent<ClassroomDismissalSequence>();
        }
        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    private static void ArrangeClassroomSeating(Scene classroom)
    {
        float[] deskRows = { 2.1f, 0.6f, -0.9f, -2.4f };
        float[] chairRows = { 1.15f, -0.35f, -1.85f, -3.35f };
        for (int row = 0; row < 4; row++)
        {
            for (int column = 1; column <= 3; column++)
            {
                FindInScene(classroom, $"Student Desk R{row + 1}C{column}").transform.position =
                    new Vector2((column - 2) * 4f, deskRows[row]);
                FindInScene(classroom, $"Chair R{row + 1}C{column}").transform.position =
                    new Vector2((column - 2) * 4f, chairRows[row]);
            }
        }
    }

    [MenuItem("Tools/Devil RPG/Install Library Opening Puzzle")]
    public static void InstallLibraryOpeningPuzzle()
    {
        Scene libraryScene = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        GameObject library = FindInScene(libraryScene, "School Library");
        GameObject puzzleObject = FindOptionalInScene(libraryScene, "Library Book Puzzle");
        if (puzzleObject == null)
        {
            puzzleObject = new GameObject("Library Book Puzzle");
            puzzleObject.transform.SetParent(library.transform, false);
        }

        LibraryBookPuzzle puzzle = puzzleObject.GetComponent<LibraryBookPuzzle>();
        if (puzzle == null)
        {
            puzzle = puzzleObject.AddComponent<LibraryBookPuzzle>();
        }

        foreach (Transform item in library.GetComponentsInChildren<Transform>(true))
        {
            if (!item.name.StartsWith("Bookcase ")
                || item.GetComponent<SpriteRenderer>() == null)
            {
                continue;
            }

            GameObject pointObject = FindOrCreateInvestigationPoint(library.transform, item);
            BoxCollider2D collider = pointObject.GetComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(item.lossyScale.x + 0.5f, item.lossyScale.y + 0.5f);

            InvestigationPoint point = pointObject.GetComponent<InvestigationPoint>();
            SetEnum(point, "kind", (int)InvestigationKind.LibraryBookcase);
            SetReference(point, "handler", puzzle);
        }

        EditorSceneManager.SaveScene(libraryScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Library Complete Puzzle")]
    public static void InstallLibraryCompletePuzzle()
    {
        Scene libraryScene = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        GameObject library = FindInScene(libraryScene, "School Library");
        GameObject puzzleObject = FindOptionalInScene(libraryScene, "Library Book Puzzle");
        if (puzzleObject == null)
        {
            puzzleObject = new GameObject("Library Book Puzzle");
            puzzleObject.transform.SetParent(library.transform, false);
        }

        LibraryBookPuzzle puzzle = puzzleObject.GetComponent<LibraryBookPuzzle>()
            ?? puzzleObject.AddComponent<LibraryBookPuzzle>();
        if (puzzleObject.GetComponent<LibraryPushHint>() == null)
        {
            puzzleObject.AddComponent<LibraryPushHint>();
        }

        SpriteRenderer source = FindInScene(libraryScene, "Bookcase Top 1")
            .GetComponent<SpriteRenderer>();
        CreateFragment(library.transform, "Library Fragment 1", new Vector2(60f, 3.4f),
            source, false);
        CreateFragment(library.transform, "Library Fragment 2", new Vector2(56.3f, 0f),
            source, false);
        CreateFragment(library.transform, "Library Fragment 3", new Vector2(68.25f, -3.4f),
            source, false);

        CreatePlacedFragment(library.transform, "Placed Library Fragment 1", source);
        CreatePlacedFragment(library.transform, "Placed Library Fragment 2", source);
        CreatePlacedFragment(library.transform, "Placed Library Fragment 3", source);
        CreateCompletePage(library.transform, source, puzzle);

        Transform chair = FindInScene(libraryScene, "Reading Chair Left South").transform;
        CreateInvestigationPoint(library.transform, "Library Chair Investigation",
            chair.position, new Vector2(1.5f, 1.1f), InvestigationKind.LibraryChair, puzzle);

        GameObject wallBrick = FindOrCreateObject(library.transform, "Library Loose Wall Brick",
            new Vector2(68.75f, -3.4f));
        wallBrick.transform.localScale = new Vector3(0.35f, 0.7f, 1f);
        SpriteRenderer wallRenderer = wallBrick.GetComponent<SpriteRenderer>();
        if (wallRenderer == null) wallRenderer = wallBrick.AddComponent<SpriteRenderer>();
        wallRenderer.sprite = source.sprite;
        wallRenderer.sharedMaterial = source.sharedMaterial;
        wallRenderer.color = new Color(0.72f, 0.66f, 0.53f, 1f);
        wallRenderer.sortingOrder = 1;
        CreateInvestigationPoint(library.transform, "Library Wall Brick Investigation",
            wallBrick.transform.position, new Vector2(1.1f, 1.6f),
            InvestigationKind.LibraryWallBrick, puzzle);

        foreach (string tableName in new[] { "Reading Table Left", "Reading Table Right" })
        {
            Transform table = FindInScene(libraryScene, tableName).transform;
            CreateInvestigationPoint(library.transform, tableName + " Investigation",
                table.position, new Vector2(3.8f, 1.8f),
                InvestigationKind.LibraryReadingTable, puzzle);
        }

        EditorSceneManager.SaveScene(libraryScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Library Counter And Broom")]
    public static void InstallLibraryCounterAndBroom()
    {
        InstallLibraryCounterGlue();
        InstallStorageRoomBroom();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Configure Library Door Requirement")]
    public static void ConfigureLibraryDoorRequirement()
    {
        Scene corridorScene = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        DoorController door = FindInScene(corridorScene, "Library Door")
            .GetComponent<DoorController>();
        if (door == null)
        {
            throw new InvalidOperationException("Library Door 缺少 DoorController。");
        }

        SetString(door, "requiredItemDisplayName", "Library Card");
        SetString(door, "missingItemDialogue", "图书馆门需要刷借书卡入内，我得先拿到借书卡。");
        EditorSceneManager.SaveScene(corridorScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Storage Room")]
    public static void InstallStorageRoom()
    {
        InstallStorageRoomLayout();
        InstallStorageRoomDoor();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Configure Storage Room Return Camera")]
    public static void ConfigureStorageRoomReturnCamera()
    {
        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        MapSpawnPoint spawn = FindInScene(corridor, "CorridorFromStorage")
            .GetComponent<MapSpawnPoint>();
        if (spawn == null) throw new InvalidOperationException("CorridorFromStorage 缺少出生点组件。");
        SetVector3(spawn, "cameraPosition", new Vector3(30f, 0f, -10f));
        EditorSceneManager.SaveScene(corridor);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Configure Paired Door Groups")]
    public static void ConfigurePairedDoorGroups()
    {
        ConfigureDoorGroup(ClassroomScenePath, "Door", "classroom-corridor");
        ConfigureDoorGroup(CorridorScenePath, "Classroom Door", "classroom-corridor");
        ConfigureDoorGroup(CorridorScenePath, "Library Door", "library-corridor");
        ConfigureDoorGroup(LibraryScenePath, "Library Exit Door", "library-corridor");
        ConfigureDoorGroup(CorridorScenePath, "Storage Room Door", "storage-corridor");
        ConfigureDoorGroup(StorageRoomScenePath, "Storage Room Exit Door", "storage-corridor");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Janitor Rest Room Door")]
    public static void InstallJanitorRestRoomDoor()
    {
        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        GameObject doorObject = FindOptionalInScene(corridor, "Janitor Rest Room Door")
            ?? FindInScene(corridor, "Decorative Door 2");
        doorObject.name = "Janitor Rest Room Door";
        SpriteRenderer source = doorObject.GetComponent<SpriteRenderer>();
        if (source == null || source.sprite == null)
        {
            throw new InvalidOperationException("Janitor Rest Room Door 缺少 SpriteRenderer。");
        }

        JanitorRestRoomDoor door = doorObject.GetComponent<JanitorRestRoomDoor>();
        if (door == null) door = doorObject.AddComponent<JanitorRestRoomDoor>();
        Transform corridorRoot = doorObject.transform.parent;
        WorldItem keyRing = CreateWorldItem(corridorRoot, "Janitor Key Ring",
            (Vector2)doorObject.transform.position + new Vector2(0f, -0.55f),
            new Vector2(0.55f, 0.45f), new Color(0.48f, 0.48f, 0.5f, 1f),
            "校工钥匙串", true, source.sprite, source.sharedMaterial);
        keyRing.gameObject.SetActive(false);
        SetReference(door, "keyRing", keyRing);
        CreateInvestigationPoint(corridorRoot, "Janitor Rest Room Door Investigation",
            doorObject.transform.position, new Vector2(2.2f, 1.7f),
            InvestigationKind.JanitorRestRoomDoor, door);
        EditorSceneManager.SaveScene(corridor);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Stairwell Door")]
    public static void InstallStairwellDoor()
    {
        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        GameObject corridorRoot = FindInScene(corridor, "School Corridor");
        SpriteRenderer source = FindInScene(corridor, "Janitor Rest Room Door")
            .GetComponent<SpriteRenderer>();
        GameObject doorObject = FindOrCreateObject(corridorRoot.transform, "Stairwell Door",
            new Vector2(24.5f, -2.5f));
        doorObject.transform.localScale = new Vector3(1.5f, 0.35f, 1f);
        SpriteRenderer renderer = doorObject.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = doorObject.AddComponent<SpriteRenderer>();
        renderer.sprite = source.sprite;
        renderer.sharedMaterial = source.sharedMaterial;
        renderer.color = source.color;
        renderer.sortingOrder = source.sortingOrder;

        StairwellDoor door = doorObject.GetComponent<StairwellDoor>();
        if (door == null) door = doorObject.AddComponent<StairwellDoor>();
        CreateInvestigationPoint(corridorRoot.transform, "Stairwell Door Investigation",
            doorObject.transform.position, new Vector2(2.2f, 1.7f),
            InvestigationKind.StairwellDoor, door);
        EditorSceneManager.SaveScene(corridor);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install School Gate And Apartment Entrance")]
    public static void InstallSchoolGateAndApartmentEntrance()
    {
        InstallApartmentEntrance();
        InstallSchoolGate();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Apartment Living Room")]
    public static void InstallApartmentLivingRoom()
    {
        Scene entrance = EditorSceneManager.OpenScene(ApartmentEntranceScenePath, OpenSceneMode.Single);
        GameObject exteriorDoor = FindInScene(entrance, "Apartment Door");
        SpriteRenderer source = exteriorDoor.GetComponent<SpriteRenderer>();
        Sprite sprite = source.sprite;
        Material material = source.sharedMaterial;

        source.enabled = false;
        Collider2D oldCollider = exteriorDoor.GetComponent<Collider2D>();
        if (oldCollider != null) oldCollider.enabled = false;
        ApartmentDoor oldPuzzle = exteriorDoor.GetComponent<ApartmentDoor>();
        if (oldPuzzle != null) oldPuzzle.enabled = false;
        Collider2D buildingCollider = FindInScene(entrance, "Apartment Building")
            .GetComponent<Collider2D>();
        if (buildingCollider != null) buildingCollider.enabled = false;
        Collider2D stepsCollider = FindInScene(entrance, "Apartment Steps")
            .GetComponent<Collider2D>();
        if (stepsCollider != null) stepsCollider.enabled = false;

        GameObject exteriorDoorRoot = FindOptionalInScene(entrance, "Apartment Entrance Door");
        if (exteriorDoorRoot == null)
        {
            exteriorDoorRoot = CreateDoorWithPortal(exteriorDoor.transform.parent, "Apartment Entrance Door",
                exteriorDoor.transform.position, sprite, material, "ApartmentLivingRoom",
                "ApartmentLivingRoomFromEntrance");
        }
        DoorController exteriorController = exteriorDoorRoot.GetComponent<DoorController>();
        SetString(exteriorController, "doorGroup", "apartment-entrance");
        SetBool(exteriorController, "animateVisual", false);
        BoxCollider2D exteriorInteraction = exteriorDoorRoot.GetComponent<BoxCollider2D>();
        exteriorInteraction.enabled = false;
        ApartmentDoor entryPuzzle = exteriorDoorRoot.GetComponent<ApartmentDoor>();
        if (entryPuzzle == null) entryPuzzle = exteriorDoorRoot.AddComponent<ApartmentDoor>();
        SetReference(entryPuzzle, "door", exteriorController);
        SetReference(entryPuzzle, "doorInteraction", exteriorInteraction);
        CreateInvestigationPoint(exteriorDoor.transform.parent, "Apartment Door Investigation",
            new Vector2(120f, 0.35f), new Vector2(2.6f, 1.7f),
            InvestigationKind.ApartmentDoor, entryPuzzle);
        CreateSpawn(exteriorDoor.transform.parent, "ApartmentEntranceFromLivingRoom",
            new Vector2(120f, 0.25f), new Vector3(120f, -1.5f, -10f));
        EditorSceneManager.SaveScene(entrance);

        Scene livingRoom = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject root = new GameObject("Apartment Living Room");
        root.transform.position = new Vector3(140f, 0f, 0f);
        CreateUnit(root.transform, "Living Room Floor", new Vector2(140f, 0f),
            new Vector2(15f, 9f), new Color(0.45f, 0.36f, 0.29f, 1f), -1,
            false, sprite, material);
        CreateUnit(root.transform, "Living Room Back Wall", new Vector2(140f, 0f),
            new Vector2(18f, 12f), new Color(0.24f, 0.22f, 0.2f, 1f), -2,
            false, sprite, material);
        CreateLivingRoomWalls(root.transform, sprite, material);

        CreateUnit(root.transform, "Living Room Sofa", new Vector2(140f, 1.25f),
            new Vector2(3.6f, 1.2f), new Color(0.22f, 0.33f, 0.36f, 1f), 2,
            true, sprite, material);
        CreateUnit(root.transform, "Living Room Coffee Table", new Vector2(140f, -0.35f),
            new Vector2(1.7f, 0.8f), new Color(0.27f, 0.16f, 0.09f, 1f), 3,
            true, sprite, material);
        CreateUnit(root.transform, "Living Room Low Cabinet", new Vector2(145.1f, 3.45f),
            new Vector2(1.8f, 0.65f), new Color(0.31f, 0.2f, 0.12f, 1f), 2,
            true, sprite, material);
        GameObject sandbag = CreateUnit(root.transform, "Living Room Sandbag", new Vector2(133.8f, -2.55f),
            new Vector2(0.75f, 1.7f), new Color(0.42f, 0.12f, 0.1f, 1f), 3,
            true, sprite, material);
        LivingRoomSandbag sandbagDialogue = sandbag.GetComponent<LivingRoomSandbag>();
        if (sandbagDialogue == null) sandbagDialogue = sandbag.AddComponent<LivingRoomSandbag>();
        CreateInvestigationPoint(root.transform, "Living Room Sandbag Investigation",
            sandbag.transform.position, new Vector2(1.7f, 2.2f),
            InvestigationKind.LivingRoomSandbag, sandbagDialogue);
        CreateUnit(root.transform, "Living Room Sandbag Stand", new Vector2(133.8f, -1.35f),
            new Vector2(1.1f, 0.16f), new Color(0.18f, 0.18f, 0.19f, 1f), 3,
            true, sprite, material);

        GameObject exitDoor = CreateDoorWithPortal(root.transform, "Apartment Exit Door",
            new Vector2(140f, -5f), sprite, material, "ApartmentEntrance",
            "ApartmentEntranceFromLivingRoom");
        SetString(exitDoor.GetComponent<DoorController>(), "doorGroup", "apartment-entrance");
        SetBool(exitDoor.GetComponent<DoorController>(), "animateVisual", false);
        CreateInteriorDoor(root.transform, "Protagonist Bedroom Door", new Vector2(136f, 5f), sprite, material);
        CreateInteriorDoor(root.transform, "Lin Chunbao Bedroom Door", new Vector2(144f, 5f), sprite, material);
        CreateInteriorDoor(root.transform, "Bathroom Door", new Vector2(148f, 1.55f), sprite, material);
        GameObject kitchenDoor = CreateDoorWithPortal(root.transform, "Kitchen Door", new Vector2(148f, -2.05f),
            sprite, material, "ApartmentKitchen", "ApartmentKitchenFromLivingRoom");
        SetString(kitchenDoor.GetComponent<DoorController>(), "doorGroup", "apartment-kitchen");
        CreateSpawn(root.transform, "ApartmentLivingRoomFromEntrance", new Vector2(140f, -3.85f),
            new Vector3(140f, 0f, -10f));
        EditorSceneManager.SaveScene(livingRoom, ApartmentLivingRoomScenePath);
        AddSceneToBuildSettings(ApartmentLivingRoomScenePath);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Apartment Kitchen")]
    public static void InstallApartmentKitchen()
    {
        Scene living = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        SpriteRenderer source = FindInScene(living, "Kitchen Door Panel").GetComponent<SpriteRenderer>();
        Sprite sprite = source.sprite;
        Material material = source.sharedMaterial;
        Scene kitchen = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject root = new GameObject("Apartment Kitchen");
        root.transform.position = new Vector3(160f, 0f, 0f);
        CreateUnit(root.transform, "Kitchen Floor", new Vector2(160f, 0f), new Vector2(10f, 7f),
            new Color(0.42f, 0.38f, 0.31f, 1f), -1, false, sprite, material);
        Color wall = new Color(0.28f, 0.26f, 0.23f, 1f);
        CreateUnit(root.transform, "Kitchen Wall Top", new Vector2(160f, 3.75f), new Vector2(11f, .5f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Wall Bottom", new Vector2(160f, -3.75f), new Vector2(11f, .5f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Wall Right", new Vector2(165.25f, 0f), new Vector2(.5f, 8f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Wall Left Top", new Vector2(154.75f, 2.2f), new Vector2(.5f, 3.1f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Wall Left Bottom", new Vector2(154.75f, -2.2f), new Vector2(.5f, 3.1f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Stove", new Vector2(157.1f, 2.8f), new Vector2(2.3f, .8f), new Color(.2f,.2f,.21f,1), 2, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Sink", new Vector2(160.2f, 2.8f), new Vector2(1.6f, .8f), new Color(.4f,.45f,.47f,1), 2, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Refrigerator", new Vector2(163.8f, -2.2f), new Vector2(1.1f, 2f), new Color(.62f,.64f,.64f,1), 2, true, sprite, material);
        GameObject cabinet = CreateUnit(root.transform, "Locked Food Cabinet", new Vector2(163.6f, 2.55f), new Vector2(1.5f, 1.3f), new Color(.48f,.2f,.12f,1), 3, true, sprite, material);
        LockedFoodCabinet locked = cabinet.AddComponent<LockedFoodCabinet>();
        CreateInvestigationPoint(root.transform, "Locked Food Cabinet Investigation", cabinet.transform.position, new Vector2(1.8f,1.6f), InvestigationKind.KitchenFoodCabinet, locked);
        GameObject exit = CreateDoorWithPortal(root.transform, "Kitchen Exit Door", new Vector2(154.5f, 0f), sprite, material, "ApartmentLivingRoom", "ApartmentLivingRoomFromKitchen");
        SetString(exit.GetComponent<DoorController>(), "doorGroup", "apartment-kitchen");
        CreateSpawn(root.transform, "ApartmentKitchenFromLivingRoom", new Vector2(155.7f, 0f), new Vector3(160f,0f,-10f));
        EditorSceneManager.SaveScene(kitchen, ApartmentKitchenScenePath);
        AddSceneToBuildSettings(ApartmentKitchenScenePath);
        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        CreateSpawn(FindInScene(livingRoom, "Apartment Living Room").transform, "ApartmentLivingRoomFromKitchen", new Vector2(146.8f, -2.05f), new Vector3(140f,0f,-10f));
        EditorSceneManager.SaveScene(livingRoom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Repair Kitchen Door Visuals")]
    public static void RepairKitchenDoorVisuals()
    {
        RepairVerticalHingedDoor(ApartmentLivingRoomScenePath, "Kitchen Door");
        RepairVerticalHingedDoor(ApartmentKitchenScenePath, "Kitchen Exit Door");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Repair Bathroom Door Visuals")]
    public static void RepairBathroomDoorVisuals()
    {
        RepairVerticalHingedDoor(ApartmentLivingRoomScenePath, "Bathroom Door");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Repair Lin Chunbao Bedroom Door Visuals")]
    public static void RepairLinChunbaoBedroomDoorVisuals()
    {
        RepairHorizontalHingedDoor(ApartmentLivingRoomScenePath, "Lin Chunbao Bedroom Door");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Apartment Bathroom")]
    public static void InstallApartmentBathroom()
    {
        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject livingRoot = FindInScene(livingRoom, "Apartment Living Room");
        GameObject bathroomDoor = FindInScene(livingRoom, "Bathroom Door");
        DoorController livingDoorController = bathroomDoor.GetComponent<DoorController>();
        SetString(livingDoorController, "doorGroup", "apartment-bathroom");
        GameObject livingPortal = FindOrCreateObject(bathroomDoor.transform, "Bathroom Door Portal", Vector2.zero);
        livingPortal.transform.localPosition = Vector3.zero;
        BoxCollider2D livingPortalCollider = livingPortal.GetComponent<BoxCollider2D>();
        if (livingPortalCollider == null) livingPortalCollider = livingPortal.AddComponent<BoxCollider2D>();
        livingPortalCollider.isTrigger = true;
        livingPortalCollider.size = new Vector2(1.4f, 1.8f);
        DoorExitPortal livingExit = livingPortal.GetComponent<DoorExitPortal>();
        if (livingExit == null) livingExit = livingPortal.AddComponent<DoorExitPortal>();
        SetReference(livingExit, "door", livingDoorController);
        SetString(livingExit, "destinationScene", "ApartmentBathroom");
        SetString(livingExit, "destinationId", "ApartmentBathroomFromLivingRoom");
        CreateSpawn(livingRoot.transform, "ApartmentLivingRoomFromBathroom", new Vector2(146.7f, 1.55f),
            new Vector3(140f, 0f, -10f));
        SpriteRenderer source = FindInScene(livingRoom, "Bathroom Door Panel").GetComponent<SpriteRenderer>();
        Sprite sprite = source.sprite;
        Material material = source.sharedMaterial;
        EditorSceneManager.SaveScene(livingRoom);

        Scene bathroom = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject root = new GameObject("Apartment Bathroom");
        root.transform.position = new Vector3(180f, 0f, 0f);
        CreateUnit(root.transform, "Bathroom Floor", new Vector2(180f, 0f), new Vector2(7f, 5f),
            new Color(.42f, .44f, .43f, 1f), -1, false, sprite, material);
        Color wall = new Color(.25f, .27f, .27f, 1f);
        CreateUnit(root.transform, "Bathroom Wall Top", new Vector2(180f, 2.75f), new Vector2(8f, .5f),
            wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Bathroom Wall Bottom", new Vector2(180f, -2.75f), new Vector2(8f, .5f),
            wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Bathroom Wall Right", new Vector2(183.75f, 0f), new Vector2(.5f, 6f),
            wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Bathroom Wall Left Top", new Vector2(176.25f, 1.75f), new Vector2(.5f, 2f),
            wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Bathroom Wall Left Bottom", new Vector2(176.25f, -1.75f), new Vector2(.5f, 2f),
            wall, 4, true, sprite, material);

        GameObject sinkUnit = CreateUnit(root.transform, "Bathroom Sink", new Vector2(178.3f, 1.45f),
            new Vector2(1.75f, .7f), new Color(.62f, .67f, .68f, 1f), 2, false,
            sprite, material);
        GameObject medicineCabinet = CreateUnit(sinkUnit.transform, "Bathroom Medicine Cabinet",
            new Vector2(178.3f, 2.05f), new Vector2(1.45f, .55f), new Color(.78f, .8f, .8f, 1f),
            3, false, sprite, material);
        medicineCabinet.transform.position = new Vector3(178.3f, 2.05f, 0f);
        BathroomMedicineCabinet medicineCabinetPuzzle = sinkUnit.GetComponent<BathroomMedicineCabinet>();
        if (medicineCabinetPuzzle == null) medicineCabinetPuzzle = sinkUnit.AddComponent<BathroomMedicineCabinet>();
        MedicineCabinetSortingPuzzle sortingPuzzle = sinkUnit.GetComponent<MedicineCabinetSortingPuzzle>();
        if (sortingPuzzle == null) sortingPuzzle = sinkUnit.AddComponent<MedicineCabinetSortingPuzzle>();
        SetReference(medicineCabinetPuzzle, "sortingPuzzle", sortingPuzzle);
        CreateInvestigationPoint(root.transform, "Bathroom Medicine Cabinet Investigation",
            new Vector2(178.3f, 1.75f), new Vector2(2.2f, 1.5f),
            InvestigationKind.BathroomMedicineCabinet, medicineCabinetPuzzle);
        GameObject toilet = CreateUnit(root.transform, "Bathroom Toilet", new Vector2(182.15f, -1.45f),
            new Vector2(1.15f, 1.25f), new Color(.84f, .84f, .8f, 1f), 2, true,
            sprite, material);
        WorldItem medicineCabinetKey = CreateWorldItem(root.transform, "Bathroom Medicine Cabinet Key",
            new Vector2(182.15f, -2.2f), new Vector2(.45f, .25f), new Color(.55f, .38f, .16f, 1f),
            "药柜钥匙", true, sprite, material);
        medicineCabinetKey.gameObject.SetActive(false);
        BathroomToiletPuzzle toiletPuzzle = toilet.GetComponent<BathroomToiletPuzzle>();
        if (toiletPuzzle == null) toiletPuzzle = toilet.AddComponent<BathroomToiletPuzzle>();
        SetReference(toiletPuzzle, "medicineCabinetKey", medicineCabinetKey);
        CreateInvestigationPoint(root.transform, "Bathroom Toilet Investigation", toilet.transform.position,
            new Vector2(1.8f, 1.8f), InvestigationKind.BathroomToilet, toiletPuzzle);
        GameObject vent = CreateUnit(root.transform, "Bathroom Vent", new Vector2(182.8f, 1.9f),
            new Vector2(.75f, .35f), new Color(.16f, .18f, .18f, 1f), 3, false,
            sprite, material);
        BathroomVentPuzzle ventPuzzle = vent.GetComponent<BathroomVentPuzzle>();
        if (ventPuzzle == null) ventPuzzle = vent.AddComponent<BathroomVentPuzzle>();
        CreateInvestigationPoint(root.transform, "Bathroom Vent Investigation", vent.transform.position,
            new Vector2(1.6f, 1.2f), InvestigationKind.BathroomVent, ventPuzzle);
        WorldItem bathroomPasswordNote = CreateWorldItem(root.transform, "Bathroom Password Note 8",
            new Vector2(182.8f, 1.2f), new Vector2(.8f, .48f), Color.white, "密码纸条 8", false,
            sprite, material);
        bathroomPasswordNote.gameObject.SetActive(false);
        BathroomPasswordNote passwordNotePuzzle = bathroomPasswordNote.GetComponent<BathroomPasswordNote>();
        if (passwordNotePuzzle == null) passwordNotePuzzle = bathroomPasswordNote.gameObject.AddComponent<BathroomPasswordNote>();
        InvestigationPoint passwordNotePoint = bathroomPasswordNote.GetComponent<InvestigationPoint>();
        if (passwordNotePoint == null) passwordNotePoint = bathroomPasswordNote.gameObject.AddComponent<InvestigationPoint>();
        passwordNotePoint.Configure(InvestigationKind.PasswordNote, passwordNotePuzzle);
        GameObject passwordNumber = FindOrCreateObject(bathroomPasswordNote.transform, "Bathroom Password Number 8", Vector2.zero);
        passwordNumber.transform.localPosition = Vector3.zero;
        TextMesh numberText = passwordNumber.GetComponent<TextMesh>();
        if (numberText == null) numberText = passwordNumber.AddComponent<TextMesh>();
        numberText.text = "8";
        numberText.fontSize = 64;
        numberText.characterSize = .18f;
        numberText.anchor = TextAnchor.MiddleCenter;
        numberText.alignment = TextAlignment.Center;
        numberText.color = Color.black;
        numberText.GetComponent<MeshRenderer>().sortingOrder = 6;
        SetReference(ventPuzzle, "passwordNote", bathroomPasswordNote);
        WorldItem plaster = CreateWorldItem(root.transform, "Bathroom Plaster", new Vector2(180.2f, .7f),
            new Vector2(.7f, .38f), new Color(.92f, .78f, .58f, 1f), "膏药", true, sprite, material);
        plaster.gameObject.SetActive(false);
        SetReference(sortingPuzzle, "plaster", plaster);

        GameObject exitDoor = CreateDoorWithPortal(root.transform, "Bathroom Exit Door", new Vector2(176.5f, 0f),
            sprite, material, "ApartmentLivingRoom", "ApartmentLivingRoomFromBathroom");
        SetString(exitDoor.GetComponent<DoorController>(), "doorGroup", "apartment-bathroom");
        CreateSpawn(root.transform, "ApartmentBathroomFromLivingRoom", new Vector2(177.35f, 0f),
            new Vector3(180f, 0f, -10f));
        EditorSceneManager.SaveScene(bathroom, ApartmentBathroomScenePath);
        AddSceneToBuildSettings(ApartmentBathroomScenePath);
        AssetDatabase.SaveAssets();
        RepairVerticalHingedDoor(ApartmentBathroomScenePath, "Bathroom Exit Door");
    }

    [MenuItem("Tools/Devil RPG/Install Lin Chunbao Bedroom")]
    public static void InstallLinChunbaoBedroom()
    {
        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject livingRoot = FindInScene(livingRoom, "Apartment Living Room");
        GameObject livingDoor = FindInScene(livingRoom, "Lin Chunbao Bedroom Door");
        DoorController livingDoorController = livingDoor.GetComponent<DoorController>();
        SetString(livingDoorController, "doorGroup", "lin-chunbao-bedroom");
        GameObject livingPortal = FindOrCreateObject(livingDoor.transform, "Lin Chunbao Bedroom Door Portal", Vector2.zero);
        livingPortal.transform.localPosition = new Vector3(0f, -.7f, 0f);
        BoxCollider2D livingPortalCollider = livingPortal.GetComponent<BoxCollider2D>();
        if (livingPortalCollider == null) livingPortalCollider = livingPortal.AddComponent<BoxCollider2D>();
        livingPortalCollider.isTrigger = true;
        livingPortalCollider.size = new Vector2(1.5f, 1.3f);
        DoorExitPortal livingExit = livingPortal.GetComponent<DoorExitPortal>();
        if (livingExit == null) livingExit = livingPortal.AddComponent<DoorExitPortal>();
        SetReference(livingExit, "door", livingDoorController);
        SetString(livingExit, "destinationScene", "ApartmentLinChunbaoBedroom");
        SetString(livingExit, "destinationId", "ApartmentLinBedroomFromLivingRoom");
        CreateSpawn(livingRoot.transform, "ApartmentLivingRoomFromLinBedroom", new Vector2(144f, 3.85f),
            new Vector3(140f, 0f, -10f));
        SpriteRenderer source = FindInScene(livingRoom, "Lin Chunbao Bedroom Door Panel").GetComponent<SpriteRenderer>();
        Sprite sprite = source.sprite;
        Material material = source.sharedMaterial;
        EditorSceneManager.SaveScene(livingRoom);

        Scene bedroom = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject root = new GameObject("Apartment Lin Chunbao Bedroom");
        root.transform.position = new Vector3(200f, 0f, 0f);
        CreateUnit(root.transform, "Lin Bedroom Floor", new Vector2(200f, 0f), new Vector2(7f, 5f),
            new Color(.48f, .39f, .33f, 1f), -1, false, sprite, material);
        Color wall = new Color(.3f, .27f, .25f, 1f);
        CreateUnit(root.transform, "Lin Bedroom Wall Top", new Vector2(200f, 2.75f), new Vector2(8f, .5f),
            wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Lin Bedroom Wall Bottom Left", new Vector2(197.75f, -2.75f), new Vector2(3.5f, .5f),
            wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Lin Bedroom Wall Bottom Right", new Vector2(202.25f, -2.75f), new Vector2(3.5f, .5f),
            wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Lin Bedroom Wall Left", new Vector2(196.25f, 0f), new Vector2(.5f, 6f),
            wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Lin Bedroom Wall Right", new Vector2(203.75f, 0f), new Vector2(.5f, 6f),
            wall, 4, true, sprite, material);

        CreateUnit(root.transform, "Lin Bedroom Bed", new Vector2(198.1f, 1.3f), new Vector2(2.45f, 1.25f),
            new Color(.42f, .5f, .66f, 1f), 2, true, sprite, material);
        CreateUnit(root.transform, "Lin Bedroom Desk", new Vector2(202.15f, 1.5f), new Vector2(1.45f, .72f),
            new Color(.31f, .18f, .1f, 1f), 2, true, sprite, material);
        CreateUnit(root.transform, "Lin Bedroom Chair", new Vector2(202.15f, .55f), new Vector2(.7f, .65f),
            new Color(.24f, .16f, .12f, 1f), 2, true, sprite, material);
        CreateUnit(root.transform, "Lin Bedroom Wardrobe", new Vector2(198.05f, -.6f), new Vector2(1.45f, 1.95f),
            new Color(.28f, .17f, .11f, 1f), 2, true, sprite, material);

        DialoguePage[] deskAndChairPages =
        {
            new DialoguePage("主角", "上面放着春宝小时候最喜欢的玩具，红熊可米。"),
            new DialoguePage("主角", "是十来年前一个一段时间内人气火爆迅速就绝版了的潮流玩偶。"),
            new DialoguePage("主角", "……"),
            new DialoguePage("主角", "也是爸爸妈妈在我六岁时那次意外前留给她的唯一纪念品……"),
            new DialoguePage("主角", "嗯，她的作业都写完收好了。")
        };
        ConfigureFurnitureDialogue(FindInScene(bedroom, "Lin Bedroom Desk"), InvestigationKind.LinBedroomDesk,
            deskAndChairPages, root.transform, new Vector2(1.9f, 1.25f));
        ConfigureFurnitureDialogue(FindInScene(bedroom, "Lin Bedroom Chair"), InvestigationKind.LinBedroomChair,
            deskAndChairPages, root.transform, new Vector2(1.25f, 1.2f));
        ConfigureFurnitureDialogue(FindInScene(bedroom, "Lin Bedroom Wardrobe"), InvestigationKind.LinBedroomWardrobe,
            new[]
            {
                new DialoguePage("主角", "衣柜里都是些朴素耐脏的平时衣物和校服。"),
                new DialoguePage("主角", "毕竟我们没有条件置办些更好的衣服，更别提给春宝买什么化妆品了……"),
                new DialoguePage("主角", "我在说些什么……她根本不在意这些。")
            }, root.transform, new Vector2(1.85f, 2.35f));

        GameObject bed = FindInScene(bedroom, "Lin Bedroom Bed");
        WorldItem phone = CreateWorldItem(root.transform, "Lin Chunbao Phone", new Vector2(199.4f, .85f),
            new Vector2(.45f, .28f), new Color(.12f, .14f, .16f, 1f), "林春宝的手机", false, sprite, material);
        phone.gameObject.SetActive(false);
        LinChunbaoBedroomBed bedPuzzle = bed.GetComponent<LinChunbaoBedroomBed>();
        if (bedPuzzle == null) bedPuzzle = bed.AddComponent<LinChunbaoBedroomBed>();
        SetReference(bedPuzzle, "phone", phone);
        CreateInvestigationPoint(root.transform, "Lin Bedroom Bed Investigation", bed.transform.position,
            new Vector2(2.9f, 1.7f), InvestigationKind.LinBedroomBed, bedPuzzle);
        LinChunbaoPhone phonePuzzle = phone.GetComponent<LinChunbaoPhone>();
        if (phonePuzzle == null) phonePuzzle = phone.gameObject.AddComponent<LinChunbaoPhone>();
        InvestigationPoint phonePoint = phone.GetComponent<InvestigationPoint>();
        if (phonePoint == null) phonePoint = phone.gameObject.AddComponent<InvestigationPoint>();
        phonePoint.Configure(InvestigationKind.LinBedroomPhone, phonePuzzle);

        GameObject exitDoor = CreateDoorWithPortal(root.transform, "Lin Bedroom Exit Door", new Vector2(200f, -2.5f),
            sprite, material, "ApartmentLivingRoom", "ApartmentLivingRoomFromLinBedroom");
        SetString(exitDoor.GetComponent<DoorController>(), "doorGroup", "lin-chunbao-bedroom");
        CreateSpawn(root.transform, "ApartmentLinBedroomFromLivingRoom", new Vector2(200f, -1.7f),
            new Vector3(200f, 0f, -10f));
        EditorSceneManager.SaveScene(bedroom, ApartmentLinChunbaoBedroomScenePath);
        AddSceneToBuildSettings(ApartmentLinChunbaoBedroomScenePath);
        AssetDatabase.SaveAssets();
        RepairHorizontalHingedDoor(ApartmentLinChunbaoBedroomScenePath, "Lin Bedroom Exit Door");
    }

    [MenuItem("Tools/Devil RPG/Install Lin Chunbao Bedroom Puzzle")]
    public static void InstallLinChunbaoBedroomPuzzle()
    {
        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject bedroomDoor = FindInScene(livingRoom, "Lin Chunbao Bedroom Door");
        if (bedroomDoor.GetComponent<LinChunbaoBedroomDoor>() == null)
        {
            bedroomDoor.AddComponent<LinChunbaoBedroomDoor>();
        }
        EditorSceneManager.SaveScene(livingRoom);

        Scene kitchen = EditorSceneManager.OpenScene(ApartmentKitchenScenePath, OpenSceneMode.Single);
        GameObject linChunbao = FindInScene(kitchen, "Lin Chunbao");
        if (linChunbao.GetComponent<LinChunbaoLivingRoomDialogue>() == null)
        {
            linChunbao.AddComponent<LinChunbaoLivingRoomDialogue>();
        }
        EditorSceneManager.SaveScene(kitchen);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Lin Chunbao Kitchen Encounter")]
    public static void InstallLinChunbaoKitchenEncounter()
    {
        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        SpriteRenderer studentSource = FindInScene(classroom, "Professor").GetComponent<SpriteRenderer>();
        Sprite sprite = studentSource.sprite;
        Material material = studentSource.sharedMaterial;
        int sortingOrder = studentSource.sortingOrder;

        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject livingRoot = FindInScene(livingRoom, "Apartment Living Room");
        if (livingRoot.GetComponent<ApartmentLivingRoomIntro>() == null)
        {
            livingRoot.AddComponent<ApartmentLivingRoomIntro>();
        }
        EditorSceneManager.SaveScene(livingRoom);

        Scene kitchen = EditorSceneManager.OpenScene(ApartmentKitchenScenePath, OpenSceneMode.Single);
        GameObject kitchenRoot = FindInScene(kitchen, "Apartment Kitchen");
        SpriteRenderer furnitureSource = FindInScene(kitchen, "Kitchen Stove").GetComponent<SpriteRenderer>();
        GameObject diningTable = CreateUnit(kitchenRoot.transform, "Kitchen Dining Table",
            new Vector2(159f, -1.45f), new Vector2(2.1f, 1f), new Color(.35f, .2f, .1f, 1f),
            2, true, furnitureSource.sprite, furnitureSource.sharedMaterial);
        GameObject linChunbao = FindOrCreateObject(kitchenRoot.transform, "Lin Chunbao",
            new Vector2(157.1f, 1.75f));
        linChunbao.transform.localScale = Vector3.one;
        SpriteRenderer renderer = linChunbao.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = linChunbao.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sharedMaterial = material;
        renderer.color = new Color(.94f, .56f, .56f, 1f);
        renderer.sortingOrder = sortingOrder;
        LinChunbaoEncounter encounter = kitchenRoot.GetComponent<LinChunbaoEncounter>();
        if (encounter == null)
        {
            encounter = kitchenRoot.AddComponent<LinChunbaoEncounter>();
        }
        SetReference(encounter, "linChunbao", linChunbao.transform);
        SetReference(encounter, "diningTable", diningTable.transform);
        LinChunbaoLivingRoomDialogue livingDialogue = linChunbao.GetComponent<LinChunbaoLivingRoomDialogue>();
        if (livingDialogue == null) livingDialogue = linChunbao.AddComponent<LinChunbaoLivingRoomDialogue>();
        CreateInvestigationPoint(linChunbao.transform, "Lin Chunbao Investigation",
            linChunbao.transform.position, new Vector2(1.6f, 1.6f),
            InvestigationKind.LivingRoomLinChunbao, livingDialogue);
        EditorSceneManager.SaveScene(kitchen);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Living Room Food Cabinet Follow-up")]
    public static void InstallLivingRoomFoodCabinetFollowUp()
    {
        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject sandbag = FindInScene(livingRoom, "Living Room Sandbag");
        LivingRoomSandbag sandbagDialogue = sandbag.GetComponent<LivingRoomSandbag>();
        if (sandbagDialogue == null) sandbagDialogue = sandbag.AddComponent<LivingRoomSandbag>();
        CreateInvestigationPoint(FindInScene(livingRoom, "Apartment Living Room").transform,
            "Living Room Sandbag Investigation", sandbag.transform.position, new Vector2(1.7f, 2.2f),
            InvestigationKind.LivingRoomSandbag, sandbagDialogue);
        EditorSceneManager.SaveScene(livingRoom);

        Scene kitchen = EditorSceneManager.OpenScene(ApartmentKitchenScenePath, OpenSceneMode.Single);
        GameObject linChunbao = FindInScene(kitchen, "Lin Chunbao");
        LinChunbaoLivingRoomDialogue livingDialogue = linChunbao.GetComponent<LinChunbaoLivingRoomDialogue>();
        if (livingDialogue == null) livingDialogue = linChunbao.AddComponent<LinChunbaoLivingRoomDialogue>();
        CreateInvestigationPoint(linChunbao.transform, "Lin Chunbao Investigation",
            linChunbao.transform.position, new Vector2(1.6f, 1.6f),
            InvestigationKind.LivingRoomLinChunbao, livingDialogue);
        EditorSceneManager.SaveScene(kitchen);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Apartment Boxing Puzzle")]
    public static void InstallApartmentBoxingPuzzle()
    {
        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject root = FindInScene(livingRoom, "Apartment Living Room");
        SpriteRenderer source = FindInScene(livingRoom, "Living Room Floor").GetComponent<SpriteRenderer>();

        GameObject cabinet = FindInScene(livingRoom, "Living Room Low Cabinet");
        LockedGloveCabinet gloveCabinet = cabinet.GetComponent<LockedGloveCabinet>();
        if (gloveCabinet == null) gloveCabinet = cabinet.AddComponent<LockedGloveCabinet>();
        WorldItem gloves = CreateWorldItem(root.transform, "Boxing Gloves", new Vector2(145.1f, 3.05f),
            new Vector2(.8f, .45f), new Color(.18f, .12f, .1f, 1f), "拳击手套", true,
            source.sprite, source.sharedMaterial);
        gloves.gameObject.SetActive(false);
        SetReference(gloveCabinet, "boxingGloves", gloves);
        CreateInvestigationPoint(root.transform, "Living Room Low Cabinet Investigation",
            cabinet.transform.position, new Vector2(2.1f, 1.2f),
            InvestigationKind.LivingRoomLowCabinet, gloveCabinet);

        ConfigureFurnitureDialogue(FindInScene(livingRoom, "Living Room Sofa"),
            InvestigationKind.LivingRoomSofa, new[]
            {
                new DialoguePage("主角", "这张沙发有点旧了，坐垫却收拾得很整齐。"),
                new DialoguePage("主角", "春宝总说不需要添置什么，\n但她比谁都在意这个家。")
            }, root.transform, new Vector2(4.2f, 1.7f));
        ConfigureFurnitureDialogue(FindInScene(livingRoom, "Living Room Coffee Table"),
            InvestigationKind.LivingRoomCoffeeTable, new[]
            {
                new DialoguePage("主角", "茶几上还留着几道杯底印，怎么擦也擦不掉。"),
                new DialoguePage("主角", "日子过得乱七八糟，\n至少这里还有一点有人生活过的痕迹。")
            }, root.transform, new Vector2(2.3f, 1.4f));

        GameObject sandbag = FindInScene(livingRoom, "Living Room Sandbag");
        LivingRoomSandbag sandbagDialogue = sandbag.GetComponent<LivingRoomSandbag>();
        SandbagBoxingMinigame boxing = sandbag.GetComponent<SandbagBoxingMinigame>();
        if (boxing == null) boxing = sandbag.AddComponent<SandbagBoxingMinigame>();
        SetReference(sandbagDialogue, "boxing", boxing);
        EditorSceneManager.SaveScene(livingRoom);

        Scene kitchen = EditorSceneManager.OpenScene(ApartmentKitchenScenePath, OpenSceneMode.Single);
        GameObject kitchenRoot = FindInScene(kitchen, "Apartment Kitchen");
        GameObject stove = FindInScene(kitchen, "Kitchen Stove");
        KitchenStoveKnife stoveKnife = stove.GetComponent<KitchenStoveKnife>();
        if (stoveKnife == null) stoveKnife = stove.AddComponent<KitchenStoveKnife>();
        SpriteRenderer stoveSprite = stove.GetComponent<SpriteRenderer>();
        WorldItem knife = CreateWorldItem(kitchenRoot.transform, "Kitchen Knife", new Vector2(156.7f, 2.35f),
            new Vector2(.9f, .18f), new Color(.72f, .72f, .75f, 1f), "菜刀", true,
            stoveSprite.sprite, stoveSprite.sharedMaterial);
        knife.gameObject.SetActive(false);
        SetReference(stoveKnife, "knife", knife);
        CreateInvestigationPoint(kitchenRoot.transform, "Kitchen Stove Investigation",
            stove.transform.position, new Vector2(2.8f, 1.4f), InvestigationKind.KitchenStove, stoveKnife);
        EditorSceneManager.SaveScene(kitchen);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Configure Static Gate Visuals")]
    public static void ConfigureStaticGateVisuals()
    {
        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        SetBool(FindInScene(corridor, "School Gate Glass Panel").GetComponent<DoorController>(),
            "animateVisual", false);
        EditorSceneManager.SaveScene(corridor);

        Scene entrance = EditorSceneManager.OpenScene(ApartmentEntranceScenePath, OpenSceneMode.Single);
        SetBool(FindInScene(entrance, "Apartment Entrance Door").GetComponent<DoorController>(),
            "animateVisual", false);
        EditorSceneManager.SaveScene(entrance);

        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        SetBool(FindInScene(livingRoom, "Apartment Exit Door").GetComponent<DoorController>(),
            "animateVisual", false);
        EditorSceneManager.SaveScene(livingRoom);
        AssetDatabase.SaveAssets();
    }

    private static void InstallSchoolGate()
    {
        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        GameObject corridorRoot = FindInScene(corridor, "School Corridor");
        SpriteRenderer source = FindInScene(corridor, "Janitor Rest Room Door")
            .GetComponent<SpriteRenderer>();
        GameObject gate = FindOrCreateObject(corridorRoot.transform, "School Gate",
            new Vector2(37.2f, 0f));
        gate.transform.localScale = Vector3.one;

        GameObject panel = FindOrCreateObject(gate.transform, "School Gate Glass Panel", Vector2.zero);
        panel.transform.localPosition = Vector3.zero;
        panel.transform.localScale = new Vector3(0.35f, 2.2f, 1f);
        SpriteRenderer renderer = panel.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = panel.AddComponent<SpriteRenderer>();
        renderer.sprite = source.sprite;
        renderer.sharedMaterial = source.sharedMaterial;
        renderer.color = new Color(0.28f, 0.72f, 0.9f, 0.62f);
        renderer.sortingOrder = 4;
        BoxCollider2D blocking = panel.GetComponent<BoxCollider2D>();
        if (blocking == null) blocking = panel.AddComponent<BoxCollider2D>();
        blocking.isTrigger = false;

        DoorController door = panel.GetComponent<DoorController>();
        if (door == null) door = panel.AddComponent<DoorController>();
        SetReference(door, "rotatingTransform", panel.transform);
        SetReference(door, "blockingCollider", blocking);
        SetBool(door, "animateVisual", false);

        GameObject portalObject = FindOrCreateObject(gate.transform, "School Gate Portal", Vector2.zero);
        portalObject.transform.localPosition = new Vector3(-0.7f, 0f, 0f);
        portalObject.transform.localScale = Vector3.one;
        BoxCollider2D portalCollider = portalObject.GetComponent<BoxCollider2D>();
        if (portalCollider == null) portalCollider = portalObject.AddComponent<BoxCollider2D>();
        portalCollider.isTrigger = true;
        portalCollider.size = new Vector2(1.2f, 1.8f);
        DoorExitPortal portal = portalObject.GetComponent<DoorExitPortal>();
        if (portal == null) portal = portalObject.AddComponent<DoorExitPortal>();
        SetReference(portal, "door", door);
        SetString(portal, "destinationScene", "ApartmentEntrance");
        SetString(portal, "destinationId", "ApartmentEntranceFromSchool");

        SchoolGate gatePuzzle = gate.GetComponent<SchoolGate>();
        if (gatePuzzle == null) gatePuzzle = gate.AddComponent<SchoolGate>();
        SetReference(gatePuzzle, "door", door);
        CreateInvestigationPoint(corridorRoot.transform, "School Gate Investigation",
            new Vector2(36.7f, 0f), new Vector2(1.8f, 2.4f),
            InvestigationKind.SchoolGate, gatePuzzle);
        EditorSceneManager.SaveScene(corridor);
    }

    private static void InstallApartmentEntrance()
    {
        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        SpriteRenderer source = FindInScene(corridor, "Janitor Rest Room Door")
            .GetComponent<SpriteRenderer>();
        Sprite sprite = source.sprite;
        Material material = source.sharedMaterial;
        Scene apartment = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject root = new GameObject("Apartment Entrance");
        root.transform.position = new Vector3(120f, 0f, 0f);
        if (root.GetComponent<ApartmentEntranceIntro>() == null)
        {
            root.AddComponent<ApartmentEntranceIntro>();
        }
        CreateUnit(root.transform, "Apartment Night Sky", new Vector2(120f, 0f),
            new Vector2(18f, 12f), new Color(0.06f, 0.09f, 0.18f, 1f), -10,
            false, sprite, material);
        CreateUnit(root.transform, "Apartment Building", new Vector2(120f, 3.5f),
            new Vector2(14f, 4.5f), new Color(0.18f, 0.2f, 0.28f, 1f), 0,
            true, sprite, material);
        CreateUnit(root.transform, "Apartment Door", new Vector2(120f, 1.6f),
            new Vector2(1.8f, 2.2f), new Color(0.36f, 0.5f, 0.62f, 1f), 2,
            true, sprite, material);
        CreateUnit(root.transform, "Apartment Steps", new Vector2(120f, 0.15f),
            new Vector2(4.2f, 0.8f), new Color(0.48f, 0.48f, 0.52f, 1f), 1,
            true, sprite, material);
        CreateUnit(root.transform, "Apartment Sidewalk", new Vector2(120f, -1.4f),
            new Vector2(18f, 1.8f), new Color(0.38f, 0.38f, 0.42f, 1f), 0,
            false, sprite, material);
        CreateUnit(root.transform, "Apartment Street", new Vector2(120f, -4.1f),
            new Vector2(18f, 3.6f), new Color(0.12f, 0.13f, 0.16f, 1f), 0,
            false, sprite, material);
        CreateUnit(root.transform, "Apartment Opposite Sidewalk", new Vector2(120f, -5.35f),
            new Vector2(18f, 1.1f), new Color(0.35f, 0.35f, 0.39f, 1f), 1,
            false, sprite, material);
        CreateUnit(root.transform, "Apartment Bus Stop Sign", new Vector2(116.4f, -4.7f),
            new Vector2(0.22f, 1.8f), new Color(0.18f, 0.62f, 0.82f, 1f), 3,
            false, sprite, material);
        GameObject apartmentDoor = FindInScene(apartment, "Apartment Door");
        ApartmentDoor doorPuzzle = apartmentDoor.GetComponent<ApartmentDoor>();
        if (doorPuzzle == null) doorPuzzle = apartmentDoor.AddComponent<ApartmentDoor>();
        CreateInvestigationPoint(root.transform, "Apartment Door Investigation",
            new Vector2(120f, 0.35f), new Vector2(2.6f, 1.7f),
            InvestigationKind.ApartmentDoor, doorPuzzle);
        CreateSpawn(root.transform, "ApartmentEntranceFromSchool", new Vector2(117f, -4.7f),
            new Vector3(120f, -1.5f, -10f));
        EditorSceneManager.SaveScene(apartment, ApartmentEntranceScenePath);
        AddSceneToBuildSettings(ApartmentEntranceScenePath);
    }

    [MenuItem("Tools/Devil RPG/Install Storage Room Puzzle")]
    public static void InstallStorageRoomPuzzle()
    {
        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        DoorController storageDoor = FindInScene(corridor, "Storage Room Door")
            .GetComponent<DoorController>();
        if (storageDoor == null) throw new InvalidOperationException("Storage Room Door 缺少门控制器。");
        SetString(storageDoor, "requiredItemDisplayName", "校工钥匙串");
        SetString(storageDoor, "missingItemDialogue", "储藏室门锁着。没有钥匙进不去。");
        SetBool(storageDoor, "requireSelectedItem", true);
        SetBool(storageDoor, "consumeRequiredItem", true);
        SetBool(storageDoor, "keepOpenWhenUnlocked", true);
        EditorSceneManager.SaveScene(corridor);

        Scene storageScene = EditorSceneManager.OpenScene(StorageRoomScenePath, OpenSceneMode.Single);
        GameObject storage = FindInScene(storageScene, "School Storage Room");
        GameObject puzzleObject = FindOrCreateObject(storage.transform, "Storage Room Puzzle", Vector2.zero);
        StorageRoomPuzzle puzzle = puzzleObject.GetComponent<StorageRoomPuzzle>();
        if (puzzle == null) puzzle = puzzleObject.AddComponent<StorageRoomPuzzle>();
        if (puzzleObject.GetComponent<StorageRoomDarkness>() == null)
        {
            puzzleObject.AddComponent<StorageRoomDarkness>();
        }

        WorldItem broom = FindInScene(storageScene, "Storage Broom").GetComponent<WorldItem>();
        broom.transform.position = new Vector3(88.5f, 2.4f, 0f);
        broom.SetPickupEnabled(false);
        SetReference(puzzle, "broom", broom);
        CreateInvestigationPoint(storage.transform, "Storage Shelf Left Investigation",
            FindInScene(storageScene, "Storage Shelf Left").transform.position,
            new Vector2(1.7f, 5.4f), InvestigationKind.StorageShelfLeft, puzzle);
        CreateInvestigationPoint(storage.transform, "Storage Shelf Right Investigation",
            FindInScene(storageScene, "Storage Shelf Right").transform.position,
            new Vector2(1.7f, 5.4f), InvestigationKind.StorageShelfRight, puzzle);
        CreateInvestigationPoint(storage.transform, "Storage Crate North Investigation",
            FindInScene(storageScene, "Storage Crate North").transform.position,
            new Vector2(2.5f, 1.5f), InvestigationKind.StorageCrateNorth, puzzle);
        CreateInvestigationPoint(storage.transform, "Storage Crate South Investigation",
            FindInScene(storageScene, "Storage Crate South").transform.position,
            new Vector2(1.9f, 1.6f), InvestigationKind.StorageCrateSouth, puzzle);
        CreateInvestigationPoint(storage.transform, "Storage Broom Investigation",
            broom.transform.position, new Vector2(1.2f, 2f), InvestigationKind.StorageBroom, puzzle);
        EditorSceneManager.SaveScene(storageScene);
        AssetDatabase.SaveAssets();
    }

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

        CreateSpawn(library.transform, "LibraryFromCorridor", new Vector2(60f, -4.7f),
            new Vector3(60f, 0f, -10f));
        CreateDoor(library.transform, "Library Exit Door", new Vector2(60f, -5.8f),
            sprite, material, "SchoolCorridor", "CorridorFromLibrary");

        EditorSceneManager.SaveScene(libraryScene, LibraryScenePath);
        AddLibraryToBuildSettings();
        AssetDatabase.SaveAssets();
    }

    private static void InstallStorageRoomLayout()
    {
        EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        SpriteRenderer basicUnit = FindBasicUnit();
        Sprite sprite = basicUnit.sprite;
        Material material = basicUnit.sharedMaterial;
        if (sprite == null || material == null)
        {
            throw new InvalidOperationException("Basic Unit 必须同时包含 Sprite 与 sharedMaterial。");
        }

        Scene storageScene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject storage = new GameObject("School Storage Room");
        storage.transform.position = new Vector3(90f, 0f, 0f);

        Color floor = new Color(0.12f, 0.13f, 0.15f, 1f);
        Color wall = new Color(0.24f, 0.25f, 0.28f, 1f);
        Color crate = new Color(0.26f, 0.16f, 0.09f, 1f);
        CreateUnit(storage.transform, "Storage Floor", new Vector2(90f, 0f),
            new Vector2(8f, 14f), floor, -10, false, sprite, material);
        CreateUnit(storage.transform, "Storage Wall Top", new Vector2(90f, 7f),
            new Vector2(8f, 0.5f), wall, 0, true, sprite, material);
        CreateUnit(storage.transform, "Storage Wall Bottom Left", new Vector2(87.5f, -7f),
            new Vector2(3f, 0.5f), wall, 0, true, sprite, material);
        CreateUnit(storage.transform, "Storage Wall Bottom Right", new Vector2(92.5f, -7f),
            new Vector2(3f, 0.5f), wall, 0, true, sprite, material);
        CreateUnit(storage.transform, "Storage Wall Left", new Vector2(86f, 0f),
            new Vector2(0.5f, 14f), wall, 0, true, sprite, material);
        CreateUnit(storage.transform, "Storage Wall Right", new Vector2(94f, 0f),
            new Vector2(0.5f, 14f), wall, 0, true, sprite, material);

        CreateUnit(storage.transform, "Storage Shelf Left", new Vector2(87.3f, 3.5f),
            new Vector2(1.1f, 5f), crate, 2, true, sprite, material);
        CreateUnit(storage.transform, "Storage Shelf Right", new Vector2(92.7f, 3.5f),
            new Vector2(1.1f, 5f), crate, 2, true, sprite, material);
        CreateUnit(storage.transform, "Storage Crate North", new Vector2(90f, 5.2f),
            new Vector2(2.1f, 1.1f), crate, 2, true, sprite, material);
        CreateUnit(storage.transform, "Storage Crate South", new Vector2(88.2f, -1.7f),
            new Vector2(1.5f, 1.2f), crate, 2, true, sprite, material);
        CreateWorldItem(storage.transform, "Storage Broom", new Vector2(88.5f, 2.4f),
            new Vector2(0.3f, 1.5f), new Color(0.48f, 0.29f, 0.12f, 1f),
            "扫帚", false, sprite, material);

        CreateSpawn(storage.transform, "StorageRoomFromCorridor", new Vector2(90f, -5.2f),
            new Vector3(90f, 0f, -10f));
        CreateDoor(storage.transform, "Storage Room Exit Door", new Vector2(90f, -6.8f),
            sprite, material, "SchoolCorridor", "CorridorFromStorage");

        EditorSceneManager.SaveScene(storageScene, StorageRoomScenePath);
        AddSceneToBuildSettings(StorageRoomScenePath);
    }

    private static void InstallCorridorDoor()
    {
        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        GameObject doorObject = FindInScene(corridor, "Decorative Door 1");
        doorObject.name = "Library Door";

        SpriteRenderer source = doorObject.GetComponent<SpriteRenderer>();
        if (source == null || source.sprite == null)
        {
            throw new InvalidOperationException("Decorative Door 1 requires a SpriteRenderer.");
        }

        CircleCollider2D interaction = doorObject.GetComponent<CircleCollider2D>();
        if (interaction == null)
        {
            interaction = doorObject.AddComponent<CircleCollider2D>();
        }
        interaction.isTrigger = true;
        interaction.offset = new Vector2(0f, 0.7f);
        interaction.radius = 1.6f;

        GameObject panel = new GameObject("Library Door Panel");
        panel.transform.SetParent(doorObject.transform, false);
        panel.transform.localScale = new Vector3(1f, 1f, 1f);
        SpriteRenderer panelRenderer = panel.AddComponent<SpriteRenderer>();
        panelRenderer.sprite = source.sprite;
        panelRenderer.sharedMaterial = source.sharedMaterial;
        panelRenderer.color = source.color;
        panelRenderer.sortingOrder = source.sortingOrder;
        BoxCollider2D blockingCollider = panel.AddComponent<BoxCollider2D>();
        blockingCollider.isTrigger = false;
        source.enabled = false;

        DoorController door = doorObject.GetComponent<DoorController>()
            ?? doorObject.AddComponent<DoorController>();
        SetReference(door, "rotatingTransform", panel.transform);
        SetReference(door, "blockingCollider", blockingCollider);

        GameObject portalObject = new GameObject("Library Door Portal");
        portalObject.transform.SetParent(doorObject.transform, false);
        portalObject.transform.localPosition = new Vector3(0f, 0.7f, 0f);
        BoxCollider2D portalCollider = portalObject.AddComponent<BoxCollider2D>();
        portalCollider.isTrigger = true;
        portalCollider.size = new Vector2(1.6f, 1.2f);
        DoorExitPortal portal = portalObject.AddComponent<DoorExitPortal>();
        SetReference(portal, "door", door);
        SetString(portal, "destinationScene", "SchoolLibrary");
        SetString(portal, "destinationId", "LibraryFromCorridor");

        CreateSpawn(null, "CorridorFromLibrary", new Vector2(27f, 1.1f),
            new Vector3(29f, 0f, -10f));
        EditorSceneManager.SaveScene(corridor);
    }

    private static void InstallStorageRoomDoor()
    {
        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        GameObject doorObject = FindOptionalInScene(corridor, "Storage Room Door")
            ?? FindInScene(corridor, "Decorative Door 3");
        doorObject.name = "Storage Room Door";
        doorObject.transform.localScale = Vector3.one;

        SpriteRenderer source = doorObject.GetComponent<SpriteRenderer>();
        if (source == null || source.sprite == null)
        {
            throw new InvalidOperationException("Storage Room Door requires a SpriteRenderer.");
        }

        CircleCollider2D interaction = doorObject.GetComponent<CircleCollider2D>();
        if (interaction == null) interaction = doorObject.AddComponent<CircleCollider2D>();
        interaction.isTrigger = true;
        interaction.offset = new Vector2(0f, 0.7f);
        interaction.radius = 1.6f;

        GameObject hinge = FindOrCreateObject(doorObject.transform, "Storage Room Door Hinge",
            new Vector2(0f, 0f));
        hinge.transform.localPosition = new Vector3(-0.75f, 0f, 0f);
        GameObject panel = FindOrCreateObject(hinge.transform, "Storage Room Door Panel",
            new Vector2(0f, 0f));
        panel.transform.localPosition = new Vector3(0.75f, 0f, 0f);
        panel.transform.localScale = new Vector3(1.5f, 0.35f, 1f);
        SpriteRenderer panelRenderer = panel.GetComponent<SpriteRenderer>();
        if (panelRenderer == null) panelRenderer = panel.AddComponent<SpriteRenderer>();
        panelRenderer.sprite = source.sprite;
        panelRenderer.sharedMaterial = source.sharedMaterial;
        panelRenderer.color = source.color;
        panelRenderer.sortingOrder = source.sortingOrder;
        BoxCollider2D blockingCollider = panel.GetComponent<BoxCollider2D>();
        if (blockingCollider == null) blockingCollider = panel.AddComponent<BoxCollider2D>();
        blockingCollider.isTrigger = false;
        source.enabled = false;

        DoorController door = doorObject.GetComponent<DoorController>()
            ?? doorObject.AddComponent<DoorController>();
        SetReference(door, "rotatingTransform", hinge.transform);
        SetReference(door, "blockingCollider", blockingCollider);

        GameObject portalObject = FindOrCreateObject(doorObject.transform,
            "Storage Room Door Portal", new Vector2(0f, 0f));
        portalObject.transform.localPosition = new Vector3(0f, 0.7f, 0f);
        BoxCollider2D portalCollider = portalObject.GetComponent<BoxCollider2D>();
        if (portalCollider == null) portalCollider = portalObject.AddComponent<BoxCollider2D>();
        portalCollider.isTrigger = true;
        portalCollider.size = new Vector2(1.6f, 1.2f);
        DoorExitPortal portal = portalObject.GetComponent<DoorExitPortal>()
            ?? portalObject.AddComponent<DoorExitPortal>();
        SetReference(portal, "door", door);
        SetString(portal, "destinationScene", "SchoolStorageRoom");
        SetString(portal, "destinationId", "StorageRoomFromCorridor");

        GameObject spawnObject = FindOptionalInScene(corridor, "CorridorFromStorage");
        if (spawnObject == null)
        {
            CreateSpawn(null, "CorridorFromStorage", new Vector2(35f, 1.1f),
                new Vector3(30f, 0f, -10f));
        }

        EditorSceneManager.SaveScene(corridor);
    }

    private static void InstallLibraryCounterGlue()
    {
        Scene libraryScene = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        GameObject library = FindInScene(libraryScene, "School Library");
        GameObject puzzleObject = FindInScene(libraryScene, "Library Book Puzzle");
        LibraryBookPuzzle puzzle = puzzleObject.GetComponent<LibraryBookPuzzle>();
        if (puzzle == null) throw new InvalidOperationException("Library Book Puzzle 缺少谜题脚本。");
        if (puzzleObject.GetComponent<LibraryPushHint>() == null)
        {
            puzzleObject.AddComponent<LibraryPushHint>();
        }

        SpriteRenderer source = FindInScene(libraryScene, "Circulation Counter Horizontal")
            .GetComponent<SpriteRenderer>();
        CreateWorldItem(library.transform, "Library Glue Bottle", new Vector2(54.7f, -3.95f),
            new Vector2(0.8f, 0.35f), new Color(0.8f, 0.08f, 0.08f, 1f),
            "胶水", false, source.sprite, source.sharedMaterial).gameObject.SetActive(false);
        CreateInvestigationPoint(library.transform, "Library Counter Investigation",
            new Vector2(54.7f, -4.05f), new Vector2(3.8f, 1.4f),
            InvestigationKind.LibraryCounter, puzzle);
        EditorSceneManager.SaveScene(libraryScene);
    }

    private static void InstallStorageRoomBroom()
    {
        Scene storageScene = EditorSceneManager.OpenScene(StorageRoomScenePath, OpenSceneMode.Single);
        GameObject storage = FindInScene(storageScene, "School Storage Room");
        SpriteRenderer source = FindInScene(storageScene, "Storage Floor").GetComponent<SpriteRenderer>();
        CreateWorldItem(storage.transform, "Storage Broom", new Vector2(88.5f, 2.4f),
            new Vector2(0.3f, 1.5f), new Color(0.48f, 0.29f, 0.12f, 1f),
            "扫帚", false, source.sprite, source.sharedMaterial);
        EditorSceneManager.SaveScene(storageScene);
    }

    private static void InstallLibraryExit()
    {
        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        EditorSceneManager.SaveScene(library);
    }

    private static void RepairDoorVisual(string scenePath, string doorName, string panelName)
    {
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        GameObject doorObject = FindInScene(scene, doorName);
        GameObject panel = FindInScene(scene, panelName);
        DoorController door = doorObject.GetComponent<DoorController>();
        BoxCollider2D blockingCollider = panel.GetComponent<BoxCollider2D>();
        if (door == null || blockingCollider == null)
        {
            throw new InvalidOperationException(doorName + " is missing required door components.");
        }

        doorObject.transform.localScale = Vector3.one;
        GameObject hinge = new GameObject(doorName + " Hinge");
        hinge.transform.SetParent(doorObject.transform, false);
        hinge.transform.localPosition = new Vector3(-0.75f, 0f, 0f);
        panel.transform.SetParent(hinge.transform, false);
        panel.transform.localPosition = new Vector3(0.75f, 0f, 0f);
        panel.transform.localRotation = Quaternion.identity;
        panel.transform.localScale = new Vector3(1.5f, 0.35f, 1f);
        SetReference(door, "rotatingTransform", hinge.transform);
        SetReference(door, "blockingCollider", blockingCollider);
        EditorSceneManager.SaveScene(scene);
    }

    private static void RepairVerticalHingedDoor(string scenePath, string doorName)
    {
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        GameObject doorObject = FindInScene(scene, doorName);
        GameObject panel = FindInScene(scene, doorName + " Panel");
        DoorController door = doorObject.GetComponent<DoorController>();
        BoxCollider2D blockingCollider = panel.GetComponent<BoxCollider2D>();
        if (door == null || blockingCollider == null)
        {
            throw new InvalidOperationException(doorName + " 缺少门控制器或门板碰撞器。");
        }

        GameObject hinge = FindOrCreateObject(doorObject.transform, doorName + " Hinge", Vector2.zero);
        hinge.transform.localPosition = new Vector3(0f, -.75f, 0f);
        panel.transform.SetParent(hinge.transform, false);
        panel.transform.localPosition = new Vector3(0f, .75f, 0f);
        panel.transform.localScale = new Vector3(.35f, 1.5f, 1f);
        SetReference(door, "rotatingTransform", hinge.transform);
        SetReference(door, "blockingCollider", blockingCollider);
        SetBool(door, "animateVisual", true);
        EditorSceneManager.SaveScene(scene);
    }

    private static void RepairHorizontalHingedDoor(string scenePath, string doorName)
    {
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        GameObject doorObject = FindInScene(scene, doorName);
        GameObject panel = FindInScene(scene, doorName + " Panel");
        DoorController door = doorObject.GetComponent<DoorController>();
        BoxCollider2D blockingCollider = panel.GetComponent<BoxCollider2D>();
        if (door == null || blockingCollider == null)
        {
            throw new InvalidOperationException(doorName + " 缺少门控制器或门板碰撞器。");
        }

        GameObject hinge = FindOrCreateObject(doorObject.transform, doorName + " Hinge", Vector2.zero);
        hinge.transform.localPosition = new Vector3(-.75f, 0f, 0f);
        panel.transform.SetParent(hinge.transform, false);
        panel.transform.localPosition = new Vector3(.75f, 0f, 0f);
        panel.transform.localRotation = Quaternion.identity;
        panel.transform.localScale = new Vector3(1.5f, .35f, 1f);
        SetReference(door, "rotatingTransform", hinge.transform);
        SetReference(door, "blockingCollider", blockingCollider);
        SetBool(door, "animateVisual", true);
        EditorSceneManager.SaveScene(scene);
    }

    private static void CreateLivingRoomWalls(Transform parent, Sprite sprite, Material material)
    {
        Color wall = new Color(0.32f, 0.29f, 0.26f, 1f);
        CreateUnit(parent, "Living Room Wall Left", new Vector2(132.5f, 0f),
            new Vector2(0.5f, 10f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Top Left", new Vector2(134f, 4.75f),
            new Vector2(3f, 0.5f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Top Center", new Vector2(140f, 4.75f),
            new Vector2(4f, 0.5f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Top Right", new Vector2(146f, 4.75f),
            new Vector2(3f, 0.5f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Bottom Left", new Vector2(135.5f, -4.75f),
            new Vector2(5f, 0.5f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Bottom Right", new Vector2(144.5f, -4.75f),
            new Vector2(5f, 0.5f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Right Top", new Vector2(147.5f, 3.2f),
            new Vector2(0.5f, 3.1f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Right Middle", new Vector2(147.5f, -0.2f),
            new Vector2(0.5f, 1.2f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Right Bottom", new Vector2(147.5f, -3.55f),
            new Vector2(0.5f, 1.9f), wall, 4, true, sprite, material);
    }

    private static void CreateInteriorDoor(Transform parent, string name, Vector2 position,
        Sprite sprite, Material material)
    {
        GameObject root = CreateDoorRoot(parent, name, position, sprite, material);
        root.GetComponent<DoorController>();
    }

    private static GameObject CreateDoorWithPortal(Transform parent, string name, Vector2 position,
        Sprite sprite, Material material, string destinationScene, string destinationId)
    {
        GameObject root = CreateDoorRoot(parent, name, position, sprite, material);
        DoorController door = root.GetComponent<DoorController>();
        GameObject portalObject = new GameObject(name + " Portal");
        portalObject.transform.SetParent(root.transform, false);
        portalObject.transform.localPosition = new Vector3(0f, 0.7f, 0f);
        BoxCollider2D portalCollider = portalObject.AddComponent<BoxCollider2D>();
        portalCollider.isTrigger = true;
        portalCollider.size = new Vector2(1.6f, 1.2f);
        DoorExitPortal portal = portalObject.AddComponent<DoorExitPortal>();
        SetReference(portal, "door", door);
        SetString(portal, "destinationScene", destinationScene);
        SetString(portal, "destinationId", destinationId);
        return root;
    }

    private static GameObject CreateDoorRoot(Transform parent, string name, Vector2 position,
        Sprite sprite, Material material)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent, false);
        root.transform.position = new Vector3(position.x, position.y, 0f);
        BoxCollider2D interaction = root.AddComponent<BoxCollider2D>();
        interaction.isTrigger = true;
        interaction.size = new Vector2(1.6f, 2.2f);

        GameObject panel = new GameObject(name + " Panel");
        panel.transform.SetParent(root.transform, false);
        panel.transform.localScale = new Vector3(1.5f, 0.35f, 1f);
        SpriteRenderer renderer = panel.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sharedMaterial = material;
        renderer.color = new Color(0.42f, 0.24f, 0.12f, 1f);
        renderer.sortingOrder = 4;
        BoxCollider2D blocking = panel.AddComponent<BoxCollider2D>();
        DoorController door = root.AddComponent<DoorController>();
        SetReference(door, "rotatingTransform", panel.transform);
        SetReference(door, "blockingCollider", blocking);
        return root;
    }

    private static void CreateDoor(Transform parent, string name, Vector2 position,
        Sprite sprite, Material material, string destinationScene, string destinationId)
    {
        CreateDoorWithPortal(parent, name, position, sprite, material, destinationScene, destinationId);
    }

    private static void CreateSpawn(Transform parent, string id, Vector2 position,
        Vector3 cameraPosition)
    {
        GameObject spawnObject = null;
        if (parent != null)
        {
            foreach (Transform item in parent.GetComponentsInChildren<Transform>(true))
            {
                if (item.name != id) continue;
                if (spawnObject == null) spawnObject = item.gameObject;
                else UnityEngine.Object.DestroyImmediate(item.gameObject);
            }
        }

        if (spawnObject == null) spawnObject = new GameObject(id);
        if (parent != null) spawnObject.transform.SetParent(parent, false);
        spawnObject.transform.position = new Vector3(position.x, position.y, 0f);
        MapSpawnPoint spawn = spawnObject.GetComponent<MapSpawnPoint>();
        if (spawn == null) spawn = spawnObject.AddComponent<MapSpawnPoint>();
        SetString(spawn, "id", id);
        SetVector3(spawn, "cameraPosition", cameraPosition);
    }

    private static GameObject FindInScene(Scene scene, string name)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform item in root.GetComponentsInChildren<Transform>(true))
            {
                if (item.name == name) return item.gameObject;
            }
        }

        throw new InvalidOperationException(scene.path + " is missing " + name + ".");
    }

    private static GameObject FindOptionalInScene(Scene scene, string name)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform item in root.GetComponentsInChildren<Transform>(true))
            {
                if (item.name == name) return item.gameObject;
            }
        }

        return null;
    }

    private static GameObject FindOrCreateObject(Transform parent, string name, Vector2 position)
    {
            foreach (Transform item in parent.GetComponentsInChildren<Transform>(true))
            {
            if (item.name == name)
            {
                item.position = new Vector3(position.x, position.y, 0f);
                return item.gameObject;
            }
        }

        GameObject itemObject = new GameObject(name);
        itemObject.transform.SetParent(parent, false);
        itemObject.transform.position = new Vector3(position.x, position.y, 0f);
        return itemObject;
    }

    private static void CreateFragment(Transform parent, string name, Vector2 position,
        SpriteRenderer source, bool active)
    {
        GameObject fragmentObject = FindOrCreateObject(parent, name, position);
        fragmentObject.transform.localScale = new Vector3(0.55f, 0.4f, 1f);
        SpriteRenderer renderer = fragmentObject.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = fragmentObject.AddComponent<SpriteRenderer>();
        renderer.sprite = source.sprite;
        renderer.sharedMaterial = source.sharedMaterial;
        renderer.color = new Color(0.92f, 0.84f, 0.63f, 1f);
        renderer.sortingOrder = 5;
        BoxCollider2D collider = fragmentObject.GetComponent<BoxCollider2D>();
        if (collider == null) collider = fragmentObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        WorldItem item = fragmentObject.GetComponent<WorldItem>();
        if (item == null) item = fragmentObject.AddComponent<WorldItem>();
        SetString(item, "displayName", "残页碎片");
        fragmentObject.SetActive(active);
    }

    private static WorldItem CreateWorldItem(Transform parent, string name, Vector2 position,
        Vector2 scale, Color color, string displayName, bool pickupEnabled, Sprite sprite,
        Material material)
    {
        GameObject itemObject = FindOrCreateObject(parent, name, position);
        itemObject.transform.localScale = new Vector3(scale.x, scale.y, 1f);
        SpriteRenderer renderer = itemObject.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = itemObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sharedMaterial = material;
        renderer.color = color;
        renderer.sortingOrder = 5;
        BoxCollider2D collider = itemObject.GetComponent<BoxCollider2D>();
        if (collider == null) collider = itemObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        WorldItem item = itemObject.GetComponent<WorldItem>();
        if (item == null) item = itemObject.AddComponent<WorldItem>();
        SetString(item, "displayName", displayName);
        item.SetPickupEnabled(pickupEnabled);
        return item;
    }

    private static void CreatePlacedFragment(Transform parent, string name,
        SpriteRenderer source)
    {
        GameObject fragmentObject = FindOrCreateObject(parent, name, Vector2.zero);
        fragmentObject.transform.localScale = new Vector3(0.55f, 0.4f, 1f);
        SpriteRenderer renderer = fragmentObject.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = fragmentObject.AddComponent<SpriteRenderer>();
        renderer.sprite = source.sprite;
        renderer.sharedMaterial = source.sharedMaterial;
        renderer.color = new Color(0.92f, 0.84f, 0.63f, 1f);
        renderer.sortingOrder = 5;
        fragmentObject.SetActive(false);
    }

    private static void CreateCompletePage(Transform parent, SpriteRenderer source,
        LibraryBookPuzzle puzzle)
    {
        WorldItem page = CreateWorldItem(parent, "Complete Library Page", Vector2.zero,
            new Vector2(1.7f, 0.65f), new Color(0.92f, 0.84f, 0.63f, 1f),
            "完整残页", false, source.sprite, source.sharedMaterial);
        InvestigationPoint point = page.GetComponent<InvestigationPoint>();
        if (point == null) point = page.gameObject.AddComponent<InvestigationPoint>();
        SetEnum(point, "kind", (int)InvestigationKind.LibraryCompletePage);
        SetReference(point, "handler", puzzle);
        page.gameObject.SetActive(false);
    }

    private static void CreateInvestigationPoint(Transform parent, string name,
        Vector2 position, Vector2 size, InvestigationKind kind, MonoBehaviour handler)
    {
        GameObject pointObject = FindOrCreateObject(parent, name, position);
        pointObject.transform.localScale = Vector3.one;
        BoxCollider2D collider = pointObject.GetComponent<BoxCollider2D>();
        if (collider == null) collider = pointObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = size;
        InvestigationPoint point = pointObject.GetComponent<InvestigationPoint>();
        if (point == null) point = pointObject.AddComponent<InvestigationPoint>();
        SetEnum(point, "kind", (int)kind);
        SetReference(point, "handler", handler);
    }

    private static void ConfigureFurnitureDialogue(GameObject furniture, InvestigationKind kind,
        DialoguePage[] pages, Transform root, Vector2 size)
    {
        LivingRoomFurnitureDialogue dialogue = furniture.GetComponent<LivingRoomFurnitureDialogue>();
        if (dialogue == null) dialogue = furniture.AddComponent<LivingRoomFurnitureDialogue>();
        SetEnum(dialogue, "kind", (int)kind);
        SetDialoguePages((UnityEngine.Object)dialogue, pages);
        CreateInvestigationPoint(root, furniture.name + " Investigation", furniture.transform.position,
            size, kind, dialogue);
    }

    private static void SetReference(UnityEngine.Object target, string field,
        UnityEngine.Object value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(field);
        if (property == null) throw new InvalidOperationException(field);
        property.objectReferenceValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetString(UnityEngine.Object target, string field, string value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(field);
        if (property == null) throw new InvalidOperationException(field);
        property.stringValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void ConfigureDoorGroup(string scenePath, string doorName, string group)
    {
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        DoorController door = FindInScene(scene, doorName).GetComponent<DoorController>();
        if (door == null) throw new InvalidOperationException(doorName + " 缺少 DoorController。");
        SetString(door, "doorGroup", group);
        EditorSceneManager.SaveScene(scene);
    }

    private static void SetBool(UnityEngine.Object target, string field, bool value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(field);
        if (property == null) throw new InvalidOperationException(field);
        property.boolValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetDialoguePages(BroadcastDialogueTrigger target, DialoguePage[] pages)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty("pages");
        if (property == null) throw new InvalidOperationException("pages");
        property.arraySize = pages.Length;
        for (int index = 0; index < pages.Length; index++)
        {
            SerializedProperty page = property.GetArrayElementAtIndex(index);
            page.FindPropertyRelative("speaker").stringValue = pages[index].Speaker;
            page.FindPropertyRelative("text").stringValue = pages[index].Text;
        }
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetDialoguePages(UnityEngine.Object target, DialoguePage[] pages)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty("pages");
        if (property == null) throw new InvalidOperationException("pages");
        property.arraySize = pages.Length;
        for (int index = 0; index < pages.Length; index++)
        {
            SerializedProperty page = property.GetArrayElementAtIndex(index);
            page.FindPropertyRelative("speaker").stringValue = pages[index].Speaker;
            page.FindPropertyRelative("text").stringValue = pages[index].Text;
        }
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetEnum(UnityEngine.Object target, string field, int value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(field);
        if (property == null) throw new InvalidOperationException(field);
        property.enumValueIndex = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetVector3(UnityEngine.Object target, string field, Vector3 value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(field);
        if (property == null) throw new InvalidOperationException(field);
        property.vector3Value = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
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

    private static GameObject CreateUnit(Transform parent, string name, Vector2 position,
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

        return unit;
    }

    private static void AddLibraryToBuildSettings()
    {
        AddSceneToBuildSettings(LibraryScenePath);
    }

    private static void AddSceneToBuildSettings(string scenePath)
    {
        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.path == scenePath)
            {
                return;
            }
        }

        scenes.Add(new EditorBuildSettingsScene(scenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }

    private static GameObject FindOrCreateInvestigationPoint(Transform library,
        Transform bookcase)
    {
        string pointName = bookcase.name + " Investigation";
        foreach (Transform item in library.GetComponentsInChildren<Transform>(true))
        {
            if (item.name == pointName)
            {
                return item.gameObject;
            }
        }

        GameObject pointObject = new GameObject(pointName);
        pointObject.transform.SetParent(library, false);
        pointObject.transform.position = bookcase.position;
        pointObject.transform.localScale = Vector3.one;
        pointObject.AddComponent<BoxCollider2D>();
        pointObject.AddComponent<InvestigationPoint>();
        return pointObject;
    }
}
