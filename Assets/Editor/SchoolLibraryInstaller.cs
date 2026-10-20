using System;
using System.Collections.Generic;
using System.Linq;
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
    private const string ApartmentProtagonistBedroomScenePath = "Assets/Scenes/ApartmentProtagonistBedroom.unity";
    private const string RoadToSuburbsOneScenePath = "Assets/Scenes/RoadToSuburbs1.unity";
    private const string RoadToSuburbsTwoScenePath = "Assets/Scenes/RoadToSuburbs2.unity";
    private const string RubblePileScenePath = "Assets/Scenes/RubblePile.unity";
    private const string AbyssEntranceScenePath = "Assets/Scenes/AbyssEntrance.unity";
    private const string FluorescentMushroomCaveScenePath = "Assets/Scenes/FluorescentMushroomCave.unity";
    private const string RoadHomeScenePath = "Assets/Scenes/RoadHome.unity";
    private const string LanguageSelectionScenePath = "Assets/Scenes/LanguageSelection.unity";
    private const string GameInstructionsScenePath = "Assets/Scenes/GameInstructions.unity";
    private const string NameEntryScenePath = "Assets/Scenes/NameEntry.unity";
    private const string ClassroomChairArtworkPath =
        "Assets/Art/Sprites/classroom-chair-selected-pixel-v1.png";
    private const string ClassroomChairPrefabPath =
        "Assets/Prefabs/ClassroomStudentChair.prefab";
    private const string ClassroomDeskPrefabPath =
        "Assets/Prefabs/ClassroomStudentDesk.prefab";
    private const string ClassroomDeskArtworkPath =
        "Assets/Art/Sprites/classroom-student-desk-pixel-v1.png";
    private const string ClassroomBlackboardArtworkPath =
        "Assets/Art/Sprites/classroom-blackboard-pixel-v1.png";
    private const string ClassroomPodiumArtworkPath =
        "Assets/Art/Sprites/classroom-podium-pixel-v1.png";
    private const string ClassroomProfessorDeskArtworkPath =
        "Assets/Art/Sprites/classroom-professor-desk-pixel-v1.png";
    private const string ClassroomStorageCabinetArtworkPath =
        "Assets/Art/Sprites/classroom-storage-cabinet-pixel-v1.png";
    private const string LibraryCardArtworkPath =
        "Assets/Art/Sprites/library-card-pixel-v1.png";
    private const string GlueBottleArtworkPath =
        "Assets/Art/Sprites/glue-bottle-pixel-v1.png";
    private const string JanitorKeyRingArtworkPath =
        "Assets/Art/Sprites/janitor-key-ring-pixel-v1.png";
    private const string StorageBroomArtworkPath =
        "Assets/Art/Sprites/storage-broom-pixel-v1.png";
    private const string StorageShelfLeftArtworkPath =
        "Assets/Art/Sprites/storage-shelf-left-pixel-v1.png";
    private const string StorageShelfRightArtworkPath =
        "Assets/Art/Sprites/storage-shelf-right-pixel-v1.png";
    private const string StorageCrateNorthArtworkPath =
        "Assets/Art/Sprites/storage-crate-north-pixel-v1.png";
    private const string StorageCrateSouthDiscardedExamsArtworkPath =
        "Assets/Art/Sprites/storage-crate-south-discarded-exams-pixel-v1.png";
    private const string KitchenKnifeArtworkPath =
        "Assets/Art/Sprites/kitchen-knife-pixel-v1.png";
    private const string KitchenStoveCounterArtworkPath =
        "Assets/Art/Sprites/kitchen-stove-counter-pixel-v1.png";
    private const string KitchenSinkCounterArtworkPath =
        "Assets/Art/Sprites/kitchen-sink-counter-pixel-v1.png";
    private const string KitchenRefrigeratorArtworkPath =
        "Assets/Art/Sprites/kitchen-refrigerator-pixel-v1.png";
    private const string KitchenDiningTableArtworkPath =
        "Assets/Art/Sprites/kitchen-dining-table-pixel-v1.png";
    private const string LockedFoodCabinetArtworkPath =
        "Assets/Art/Sprites/locked-food-cabinet-pixel-v1.png";
    private const string BathroomSinkMedicineCabinetArtworkPath =
        "Assets/Art/Sprites/bathroom-sink-medicine-cabinet-pixel-v1.png";
    private const string BathroomToiletArtworkPath =
        "Assets/Art/Sprites/bathroom-toilet-pixel-v1.png";
    private const string BathroomVentArtworkPath =
        "Assets/Art/Sprites/bathroom-vent-pixel-v1.png";
    private const string LinBedroomDeskArtworkPath =
        "Assets/Art/Sprites/lin-bedroom-desk-pixel-v1.png";
    private const string LinBedroomChairArtworkPath =
        "Assets/Art/Sprites/lin-bedroom-chair-pixel-v1.png";
    private const string LinBedroomBedArtworkPath =
        "Assets/Art/Sprites/lin-bedroom-bed-pixel-v1.png";
    private const string LinBedroomWardrobeArtworkPath =
        "Assets/Art/Sprites/lin-bedroom-wardrobe-pixel-v1.png";
    private const string ProtagonistBedroomBedArtworkPath =
        "Assets/Art/Sprites/protagonist-bedroom-bed-pixel-v1.png";
    private const string ProtagonistPlasterBoxesArtworkPath =
        "Assets/Art/Sprites/protagonist-plaster-boxes-pixel-v1.png";
    private const string ProtagonistBedsideTableClosedArtworkPath =
        "Assets/Art/Sprites/protagonist-bedside-table-closed-pixel-v1.png";
    private const string ProtagonistBedsideTableOpenArtworkPath =
        "Assets/Art/Sprites/protagonist-bedside-table-open-pixel-v1.png";
    private const string RubbleIronGateClosedArtworkPath =
        "Assets/Art/Sprites/rubble-iron-gate-closed-pixel-v1.png";
    private const string RubbleIronGateOpenArtworkPath =
        "Assets/Art/Sprites/rubble-iron-gate-open-pixel-v1.png";
    private const string RubbleSwitchPrefabPath =
        "Assets/Prefabs/RubbleSwitch.prefab";
    private const string RubbleSwitchIdleArtworkPath =
        "Assets/Art/Sprites/rubble-switch-idle-pixel-v1.png";
    private const string RubbleSwitchPressedArtworkPath =
        "Assets/Art/Sprites/rubble-switch-pressed-pixel-v1.png";
    private const string RoadHomeStreetlightArtworkPath =
        "Assets/Art/Sprites/road-home-streetlight-pixel-v1.png";
    private const string RoadHomeCustomRoadArtworkPath =
        "Assets/Scenes/ChatGPT 图像 2026年10月4日 12_14_45.png";
    private const string RubbleTuitionReceiptArtworkPath =
        "Assets/Art/Sprites/rubble-tuition-receipt-pixel-v1.png";
    private const string RubbleDebtNoticeArtworkPath =
        "Assets/Art/Sprites/rubble-debt-notice-pixel-v1.png";
    private const string ThirteenCrystalGiftArtworkPath =
        "Assets/Art/Sprites/thirteen-crystal-gift-pixel-v1.png";
    private const string RubbleDroppedElectricBatonArtworkPath =
        "Assets/Art/Sprites/rubble-dropped-electric-baton-pixel-v1.png";
    private const string RubbleInjuredSmallDemonArtworkPath =
        "Assets/Art/Sprites/rubble-injured-small-demon-pixel-v1.png";
    private static readonly Vector2 RubbleSpawnPosition = new Vector2(0f, -5f);
    private static readonly Vector2 InjuredDemonIntroPosition = new Vector2(299.45f, -3.65f);
    private static readonly Vector2 DroppedBatonIntroPosition = new Vector2(300.15f, -3.95f);
    private const string MedicineCabinetKeyArtworkPath =
        "Assets/Art/Sprites/medicine-cabinet-key-pixel-v1.png";
    private static readonly string[] LibraryFragmentArtworkPaths =
    {
        "Assets/Art/Sprites/library-page-fragment-1-pixel-v1.png",
        "Assets/Art/Sprites/library-page-fragment-2-pixel-v1.png",
        "Assets/Art/Sprites/library-page-fragment-3-pixel-v1.png"
    };
    private const string CompleteLibraryPageArtworkPath =
        "Assets/Art/Sprites/complete-library-page-pixel-v1.png";
    private const string BoxingGlovesArtworkPath =
        "Assets/Art/Sprites/boxing-gloves-pixel-v1.png";
    private const string LinChunbaoPhoneArtworkPath =
        "Assets/Art/Sprites/lin-chunbao-phone-pixel-v1.png";
    private const string BoxingPasswordNoteArtworkPath =
        "Assets/Art/Sprites/boxing-password-note-6-pixel-v1.png";
    private const string BathroomPasswordNoteArtworkPath =
        "Assets/Art/Sprites/bathroom-password-note-8-pixel-v1.png";
    private const string LivingRoomSandbagArtworkPath =
        "Assets/Art/Sprites/living-room-sandbag-pixel-v1.png";
    private const string BrokenLivingRoomSandbagArtworkPath =
        "Assets/Art/Sprites/living-room-sandbag-broken-pixel-v1.png";
    private const string LivingRoomSofaArtworkPath =
        "Assets/Art/Sprites/living-room-sofa-pixel-v1.png";
    private const string LivingRoomCoffeeTableArtworkPath =
        "Assets/Art/Sprites/living-room-coffee-table-pixel-v1.png";
    private const string LivingRoomLowCabinetArtworkPath =
        "Assets/Art/Sprites/living-room-low-cabinet-pixel-v1.png";
    private const string LivingRoomLowCabinetOpenArtworkPath =
        "Assets/Art/Sprites/living-room-low-cabinet-open-pixel-v1.png";
    private const string BathroomPlasterArtworkPath =
        "Assets/Art/Sprites/bathroom-plaster-pixel-v1.png";
    private const string DriedMeatArtworkPath =
        "Assets/Art/Sprites/dried-meat-pixel-v1.png";
    private const string ClassroomWoodFloorTexturePath =
        "Assets/Art/Textures/classroom-wood-floor-pixel-v1.png";
    private const string CorridorCeramicTileFloorTexturePath =
        "Assets/Art/Textures/corridor-ceramic-tile-floor-pixel-v1.png";
    private const string LibraryWoodFloorTexturePath =
        "Assets/Art/Textures/library-wood-floor-pixel-v1.png";
    private const string StorageRoomConcreteFloorTexturePath =
        "Assets/Art/Textures/storage-room-concrete-floor-pixel-v1.png";
    private const string RubblePileGroundTexturePath =
        "Assets/Art/Textures/rubble-pile-ground-pixel-v1.png";
    private const string ApartmentEntranceAsphaltRoadTexturePath =
        "Assets/Art/Textures/apartment-entrance-asphalt-road-pixel-v1.png";
    private const string ApartmentEntranceBusStopSignArtworkPath =
        "Assets/Art/Sprites/apartment-entrance-bus-stop-sign-pixel-v1.png";
    private const string ApartmentEntranceUpperSidewalkTexturePath =
        "Assets/Art/Textures/apartment-entrance-upper-sidewalk-pixel-v1.png";
    private const string ApartmentEntranceLowerSidewalkTexturePath =
        "Assets/Art/Textures/apartment-entrance-lower-sidewalk-pixel-v1.png";
    private const string ApartmentLivingRoomCarpetFloorTexturePath =
        "Assets/Art/Textures/apartment-living-room-carpet-floor-pixel-v1.png";
    private const string ApartmentProtagonistBedroomCarpetFloorTexturePath =
        "Assets/Art/Textures/apartment-protagonist-bedroom-carpet-floor-pixel-v1.png";
    private const string ApartmentLinBedroomCarpetFloorTexturePath =
        "Assets/Art/Textures/apartment-lin-bedroom-carpet-floor-pixel-v1.png";
    private const string ApartmentKitchenCheckerFloorTexturePath =
        "Assets/Art/Textures/apartment-kitchen-checker-floor-pixel-v1.png";
    private const string ApartmentBathroomCheckerFloorTexturePath =
        "Assets/Art/Textures/apartment-bathroom-checker-floor-pixel-v1.png";
    private const string SuburbRoadGuardrailArtworkPath =
        "Assets/Art/Sprites/suburb-road-guardrail-pixel-v1.png";
    private const string SuburbMazeBarricadeHorizontalShortArtworkPath =
        "Assets/Art/Sprites/suburbs-maze-barricade-horizontal-short-pixel-v1.png";
    private const string SuburbMazeBarricadeHorizontalMediumArtworkPath =
        "Assets/Art/Sprites/suburbs-maze-barricade-horizontal-medium-pixel-v1.png";
    private const string SuburbMazeBarricadeHorizontalLongArtworkPath =
        "Assets/Art/Sprites/suburbs-maze-barricade-horizontal-long-pixel-v1.png";
    private const string SuburbMazeBarricadeHorizontalBoundaryArtworkPath =
        "Assets/Art/Sprites/suburbs-maze-barricade-horizontal-boundary-pixel-v1.png";
    private const string SuburbMazeBarricadeVerticalShortArtworkPath =
        "Assets/Art/Sprites/suburbs-maze-barricade-vertical-short-pixel-v1.png";
    private const string SuburbMazeBarricadeVerticalMediumArtworkPath =
        "Assets/Art/Sprites/suburbs-maze-barricade-vertical-medium-pixel-v1.png";
    private const string RubbleBarricadeHorizontalShortArtworkPath =
        "Assets/Art/Sprites/rubble-barricade-horizontal-short-pixel-v1.png";
    private const string RubbleBarricadeHorizontalMediumArtworkPath =
        "Assets/Art/Sprites/rubble-barricade-horizontal-medium-pixel-v1.png";
    private const string RubbleBarricadeHorizontalLongArtworkPath =
        "Assets/Art/Sprites/rubble-barricade-horizontal-long-pixel-v1.png";
    private const string RubbleBarricadeHorizontalBoundaryArtworkPath =
        "Assets/Art/Sprites/rubble-barricade-horizontal-boundary-pixel-v1.png";
    private const string RubbleBarricadeVerticalShortArtworkPath =
        "Assets/Art/Sprites/rubble-barricade-vertical-short-pixel-v1.png";
    private const string RubbleBarricadeVerticalMediumArtworkPath =
        "Assets/Art/Sprites/rubble-barricade-vertical-medium-pixel-v1.png";
    private const string AbyssEntranceCaveMudFloorTexturePath =
        "Assets/Art/Textures/abyss-entrance-cave-mud-floor-pixel-v1.png";
    private const string AbyssEntranceCaveWallTopArtworkPath =
        "Assets/Art/Sprites/abyss-entrance-cave-wall-top-pixel-v1.png";
    private const string AbyssEntranceCaveWallLeftArtworkPath =
        "Assets/Art/Sprites/abyss-entrance-cave-wall-left-pixel-v1.png";
    private const string AbyssEntranceCaveWallRightArtworkPath =
        "Assets/Art/Sprites/abyss-entrance-cave-wall-right-pixel-v1.png";
    private const string AbyssEntranceCaveWallBottomArtworkPath =
        "Assets/Art/Sprites/abyss-entrance-cave-wall-bottom-pixel-v1.png";
    private const string AbyssEntranceRockProtrusionArtworkPath =
        "Assets/Art/Sprites/abyss-entrance-rock-protrusion-pixel-v1.png";
    private const string AbyssEntranceRockyMossArtworkPath =
        "Assets/Art/Sprites/abyss-entrance-glowing-moss-rocky-pixel-v1.png";
    private const string AbyssEntranceGlowingMossArtworkPath =
        "Assets/Art/Sprites/abyss-entrance-glowing-moss-pixel-v1.png";
    private const string AbyssEntranceWarmCrystalClusterArtworkPath =
        "Assets/Art/Sprites/abyss-entrance-warm-crystal-cluster-pixel-v1.png";
    private const string AbyssEntranceSideCavePassageArtworkPath =
        "Assets/Art/Sprites/abyss-entrance-side-cave-passage-pixel-v1.png";
    private const string FluorescentMushroomClusterLargeArtworkPath =
        "Assets/Art/Sprites/fluorescent-mushroom-cluster-large-pixel-v1.png";
    private const string FluorescentMushroomClusterLeftArtworkPath =
        "Assets/Art/Sprites/fluorescent-mushroom-cluster-left-pixel-v1.png";
    private const string FluorescentMushroomClusterRightArtworkPath =
        "Assets/Art/Sprites/fluorescent-mushroom-cluster-right-pixel-v1.png";
    private const string ApartmentEntranceBuildingFacadeArtworkPath =
        "Assets/Art/Sprites/apartment-entrance-building-facade-pixel-v1.png";
    private const string ApartmentEntranceDoorArtworkPath =
        "Assets/Art/Sprites/apartment-entrance-door-pixel-v1.png";
    private const string ApartmentEntranceDoorOpenArtworkPath =
        "Assets/Art/Sprites/apartment-entrance-door-open-pixel-v1.png";
    private const string ApartmentBedroomDoorClosedArtworkPath =
        "Assets/Art/Sprites/apartment-bedroom-door-closed-pixel-v1.png";
    private const string ApartmentBedroomDoorOpenArtworkPath =
        "Assets/Art/Sprites/apartment-bedroom-door-open-pixel-v1.png";
    private const string LivingRoomUpperWallArtworkPath =
        "Assets/Art/Sprites/living-room-upper-wall-pixel-v1.png";
    private const string LivingRoomLeftWallArtworkPath =
        "Assets/Art/Sprites/living-room-left-wall-pixel-v1.png";
    private const string LivingRoomRightWallArtworkPath =
        "Assets/Art/Sprites/living-room-right-wall-pixel-v1.png";
    private const string LivingRoomBottomWallArtworkPath =
        "Assets/Art/Sprites/living-room-bottom-wall-pixel-v1.png";
    private const string SchoolCorridorWallArtworkPath =
        "Assets/Art/Sprites/school-corridor-wall-pixel-v1.png";
    private const string LibraryHorizontalBookcaseArtworkPath =
        "Assets/Art/Sprites/library-bookcase-horizontal-pixel-v1.png";
    private const string LibraryVerticalBookcaseArtworkPath =
        "Assets/Art/Sprites/library-bookcase-vertical-pixel-v1.png";
    private const string LibraryCounterArtworkPath =
        "Assets/Art/Sprites/library-circulation-counter-pixel-v1.png";
    private const string LibraryReadingChairFrontArtworkPath =
        "Assets/Art/Sprites/library-reading-chair-front-pixel-v1.png";
    private const string LibraryReadingChairBackArtworkPath =
        "Assets/Art/Sprites/library-reading-chair-back-pixel-v1.png";
    private const string LibraryReadingTableLeftArtworkPath =
        "Assets/Art/Sprites/library-reading-table-left-pixel-v1.png";
    private const string LibraryReadingTableRightArtworkPath =
        "Assets/Art/Sprites/library-reading-table-right-pixel-v1.png";
    private const string LibraryDoorArtworkPath =
        "Assets/Art/Sprites/library-door-pixel-v1.png";
    private const string JanitorRestRoomDoorArtworkPath =
        "Assets/Art/Sprites/janitor-rest-room-door-pixel-v1.png";
    private const string StorageRoomDoorArtworkPath =
        "Assets/Art/Sprites/storage-room-door-pixel-v1.png";
    private const string LibraryDoorOpenArtworkPath =
        "Assets/Art/Sprites/library-door-open-pixel-v1.png";
    private const string LibraryWallTopArtworkPath =
        "Assets/Art/Sprites/library-wall-top-pixel-v1.png";
    private const string LibraryWallSideArtworkPath =
        "Assets/Art/Sprites/library-wall-side-pixel-v1.png";
    private const string LibraryWallBottomArtworkPath =
        "Assets/Art/Sprites/library-wall-bottom-pixel-v1.png";
    private const string LibraryLooseWallPanelArtworkPath =
        "Assets/Art/Sprites/library-loose-wall-panel-pixel-v1.png";
    private const string StorageRoomDoorOpenArtworkPath =
        "Assets/Art/Sprites/storage-room-door-open-pixel-v1.png";
    private const string ClassroomFrontWallArtworkPath =
        "Assets/Art/Sprites/classroom-front-wall-pixel-v1.png";
    private const string ClassroomCactusPotIntactArtworkPath =
        "Assets/Art/Sprites/classroom-cactus-pot-intact-pixel-v1.png";
    private const string ClassroomCactusPotBrokenArtworkPath =
        "Assets/Art/Sprites/classroom-cactus-pot-broken-pixel-v1.png";
    // 椅子略小于课桌，且不改变椅子根节点的互动碰撞范围。
    private const float ClassroomChairArtworkScale = 0.10f;

    [MenuItem("Tools/Devil RPG/Apply Classroom Chair Artwork")]
    public static void ApplyClassroomChairArtwork()
    {
        Sprite chairArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(ClassroomChairArtworkPath);
        if (chairArtwork == null)
        {
            throw new InvalidOperationException("找不到教室椅子美术素材。");
        }

        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        for (int row = 1; row <= 4; row++)
        {
            for (int column = 1; column <= 3; column++)
            {
                GameObject chair = FindInScene(classroom, $"Chair R{row}C{column}");
                SpriteRenderer baseRenderer = chair.GetComponent<SpriteRenderer>();
                if (baseRenderer == null)
                {
                    throw new InvalidOperationException($"{chair.name} 缺少精灵渲染器。");
                }

                Transform artwork = chair.transform.Find("Chair Artwork");
                if (artwork == null)
                {
                    artwork = new GameObject("Chair Artwork").transform;
                    artwork.SetParent(chair.transform, false);
                }

                SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
                if (artworkRenderer == null)
                {
                    artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
                }

                artwork.localPosition = Vector3.zero;
                artwork.localRotation = Quaternion.identity;
                artwork.localScale = Vector3.one * ClassroomChairArtworkScale;
                artworkRenderer.sprite = chairArtwork;
                artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
                artworkRenderer.color = Color.white;
                artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
                artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

                // 根对象继续保存互动、碰撞与物品栏图标所需的组件；仅隐藏旧占位图。
                baseRenderer.sprite = chairArtwork;
                baseRenderer.enabled = false;
            }
        }

        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Resize Classroom Chair Artwork")]
    public static void ResizeClassroomChairArtwork()
    {
        GameObject prefab = PrefabUtility.LoadPrefabContents(ClassroomChairPrefabPath);
        try
        {
            Transform artwork = prefab.transform.Find("Chair Artwork");
            if (artwork == null)
            {
                throw new InvalidOperationException("学生椅子预制体缺少美术子对象。");
            }

            artwork.localScale = Vector3.one * ClassroomChairArtworkScale;
            PrefabUtility.SaveAsPrefabAsset(prefab, ClassroomChairPrefabPath);
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefab);
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Create Classroom Chair Prefab")]
    public static void CreateClassroomChairPrefab()
    {
        const int chairCount = 12;
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject source = FindInScene(classroom, "Chair R1C1");
        GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(source,
            ClassroomChairPrefabPath, InteractionMode.AutomatedAction);
        if (prefab == null)
        {
            throw new InvalidOperationException("无法创建学生椅子预制体。");
        }

        List<ThrowableChair> chairs = new List<ThrowableChair>(chairCount)
        {
            source.GetComponent<ThrowableChair>()
        };
        for (int row = 1; row <= 4; row++)
        {
            for (int column = 1; column <= 3; column++)
            {
                if (row == 1 && column == 1) continue;

                GameObject oldChair = FindInScene(classroom, $"Chair R{row}C{column}");
                Transform oldTransform = oldChair.transform;
                Transform parent = oldTransform.parent;
                string name = oldChair.name;
                Vector3 localPosition = oldTransform.localPosition;
                Quaternion localRotation = oldTransform.localRotation;
                Vector3 localScale = oldTransform.localScale;
                int siblingIndex = oldTransform.GetSiblingIndex();

                GameObject chair = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
                chair.name = name;
                chair.transform.localPosition = localPosition;
                chair.transform.localRotation = localRotation;
                chair.transform.localScale = localScale;
                chair.transform.SetSiblingIndex(siblingIndex);
                UnityEngine.Object.DestroyImmediate(oldChair);
                chairs.Add(chair.GetComponent<ThrowableChair>());
            }
        }

        ClassroomLibraryCardPuzzle puzzle =
            UnityEngine.Object.FindObjectOfType<ClassroomLibraryCardPuzzle>(true);
        if (puzzle == null)
        {
            throw new InvalidOperationException("找不到教室椅子谜题控制器。");
        }

        SerializedObject serialized = new SerializedObject(puzzle);
        SerializedProperty chairList = serialized.FindProperty("chairs");
        chairList.arraySize = chairs.Count;
        for (int index = 0; index < chairs.Count; index++)
        {
            chairList.GetArrayElementAtIndex(index).objectReferenceValue = chairs[index];
        }
        serialized.ApplyModifiedPropertiesWithoutUndo();

        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Create Classroom Desk Prefab")]
    public static void CreateClassroomDeskPrefab()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject source = FindInScene(classroom, "Student Desk R1C1");
        GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(source,
            ClassroomDeskPrefabPath, InteractionMode.AutomatedAction);
        if (prefab == null)
        {
            throw new InvalidOperationException("无法创建学生桌子预制体。");
        }

        for (int row = 1; row <= 4; row++)
        {
            for (int column = 1; column <= 3; column++)
            {
                if (row == 1 && column == 1) continue;

                GameObject oldDesk = FindInScene(classroom, $"Student Desk R{row}C{column}");
                Transform oldTransform = oldDesk.transform;
                Transform parent = oldTransform.parent;
                string name = oldDesk.name;
                Vector3 localPosition = oldTransform.localPosition;
                Quaternion localRotation = oldTransform.localRotation;
                Vector3 localScale = oldTransform.localScale;
                int siblingIndex = oldTransform.GetSiblingIndex();

                GameObject desk = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
                desk.name = name;
                desk.transform.localPosition = localPosition;
                desk.transform.localRotation = localRotation;
                desk.transform.localScale = localScale;
                desk.transform.SetSiblingIndex(siblingIndex);
                UnityEngine.Object.DestroyImmediate(oldDesk);
            }
        }

        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Shrink Classroom Furniture Colliders")]
    public static void ShrinkClassroomFurnitureColliders()
    {
        SetPrefabColliderSize(ClassroomDeskPrefabPath, new Vector2(.7f, .7f));
        SetPrefabColliderSize(ClassroomChairPrefabPath, new Vector2(.7f, .7f));

        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        for (int row = 1; row <= 4; row++)
        {
            for (int column = 1; column <= 3; column++)
            {
                FindInScene(classroom, $"Student Desk R{row}C{column}")
                    .GetComponent<BoxCollider2D>().size = new Vector2(.7f, .7f);
                FindInScene(classroom, $"Chair R{row}C{column}")
                    .GetComponent<BoxCollider2D>().size = new Vector2(.7f, .7f);
            }
        }

        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Classroom Desk Artwork")]
    public static void ApplyClassroomDeskArtwork()
    {
        Sprite deskArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(ClassroomDeskArtworkPath);
        if (deskArtwork == null)
        {
            throw new InvalidOperationException("找不到学生桌子美术素材。");
        }

        GameObject prefab = PrefabUtility.LoadPrefabContents(ClassroomDeskPrefabPath);
        try
        {
            SpriteRenderer baseRenderer = prefab.GetComponent<SpriteRenderer>();
            if (baseRenderer == null)
            {
                throw new InvalidOperationException("学生桌子预制体缺少精灵渲染器。");
            }

            Transform artwork = prefab.transform.Find("Desk Artwork");
            if (artwork == null)
            {
                artwork = new GameObject("Desk Artwork").transform;
                artwork.SetParent(prefab.transform, false);
            }

            SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
            if (artworkRenderer == null)
            {
                artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
            }

            // 视觉层比原碰撞范围略小，避免压到后方椅子，同时保留完整桌架轮廓。
            Vector2 targetSize = new Vector2(1.4f, 0.75f);
            Vector3 parentScale = prefab.transform.localScale;
            artwork.localPosition = Vector3.zero;
            artwork.localRotation = Quaternion.identity;
            artwork.localScale = new Vector3(
                targetSize.x / (deskArtwork.bounds.size.x * parentScale.x),
                targetSize.y / (deskArtwork.bounds.size.y * parentScale.y), 1f);
            artworkRenderer.sprite = deskArtwork;
            artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
            artworkRenderer.color = Color.white;
            artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
            artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

            baseRenderer.sprite = deskArtwork;
            baseRenderer.enabled = false;
            PrefabUtility.SaveAsPrefabAsset(prefab, ClassroomDeskPrefabPath);
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefab);
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Classroom Blackboard Artwork")]
    public static void ApplyClassroomBlackboardArtwork()
    {
        Sprite blackboardArtwork =
            AssetDatabase.LoadAssetAtPath<Sprite>(ClassroomBlackboardArtworkPath);
        if (blackboardArtwork == null)
        {
            throw new InvalidOperationException("找不到教室黑板美术素材。");
        }

        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject surface = FindInScene(classroom, "Blackboard Surface");
        SpriteRenderer baseRenderer = surface.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("黑板缺少精灵渲染器。");
        }

        Transform artwork = surface.transform.Find("Blackboard Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Blackboard Artwork").transform;
            artwork.SetParent(surface.transform, false);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector2 targetSize = new Vector2(6.5f, 1.4f);
        Vector3 parentScale = surface.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            targetSize.x / (blackboardArtwork.bounds.size.x * parentScale.x),
            targetSize.y / (blackboardArtwork.bounds.size.y * parentScale.y), 1f);
        artworkRenderer.sprite = blackboardArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

        baseRenderer.sprite = blackboardArtwork;
        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Classroom Podium Artwork")]
    public static void ApplyClassroomPodiumArtwork()
    {
        Sprite podiumArtwork =
            AssetDatabase.LoadAssetAtPath<Sprite>(ClassroomPodiumArtworkPath);
        if (podiumArtwork == null)
        {
            throw new InvalidOperationException("找不到教室讲台美术素材。");
        }

        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject podium = FindInScene(classroom, "Podium");
        GameObject teacherDesk = FindInScene(classroom, "Teacher Desk");
        SpriteRenderer teacherDeskRenderer = teacherDesk.GetComponent<SpriteRenderer>();
        if (teacherDeskRenderer == null)
        {
            throw new InvalidOperationException("教师讲台缺少精灵渲染器。");
        }

        Transform previousArtwork = podium.transform.Find("Podium Artwork");
        if (previousArtwork != null)
        {
            UnityEngine.Object.DestroyImmediate(previousArtwork.gameObject);
        }

        SpriteRenderer podiumRenderer = podium.GetComponent<SpriteRenderer>();
        if (podiumRenderer != null)
        {
            podiumRenderer.sprite = teacherDeskRenderer.sprite;
            podiumRenderer.enabled = true;
        }

        SpriteRenderer baseRenderer = teacherDeskRenderer;
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("讲台缺少精灵渲染器。");
        }

        Transform artwork = teacherDesk.transform.Find("Podium Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Podium Artwork").transform;
            artwork.SetParent(teacherDesk.transform, false);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector2 targetSize = new Vector2(2.6f, 1.1f);
        Vector3 parentScale = teacherDesk.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            targetSize.x / (podiumArtwork.bounds.size.x * parentScale.x),
            targetSize.y / (podiumArtwork.bounds.size.y * parentScale.y), 1f);
        artworkRenderer.sprite = podiumArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = 8;

        baseRenderer.sprite = podiumArtwork;
        baseRenderer.enabled = false;
        SpriteRenderer professorRenderer = FindInScene(classroom, "Professor")
            .GetComponent<SpriteRenderer>();
        if (professorRenderer != null)
        {
            professorRenderer.sortingOrder = 6;
        }
        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Classroom Professor Desk Artwork")]
    public static void ApplyClassroomProfessorDeskArtwork()
    {
        Sprite professorDeskArtwork =
            AssetDatabase.LoadAssetAtPath<Sprite>(ClassroomProfessorDeskArtworkPath);
        if (professorDeskArtwork == null)
        {
            throw new InvalidOperationException("找不到教授桌子美术素材。");
        }

        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject podium = FindInScene(classroom, "Podium");
        SpriteRenderer baseRenderer = podium.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("教授桌子缺少精灵渲染器。");
        }

        Transform artwork = podium.transform.Find("Professor Desk Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Professor Desk Artwork").transform;
            artwork.SetParent(podium.transform, false);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector2 targetSize = new Vector2(0.95f, 1.15f);
        Vector3 parentScale = podium.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            targetSize.x / (professorDeskArtwork.bounds.size.x * parentScale.x),
            targetSize.y / (professorDeskArtwork.bounds.size.y * parentScale.y), 1f);
        artworkRenderer.sprite = professorDeskArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = 7;

        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Classroom Storage Cabinet Artwork")]
    public static void ApplyClassroomStorageCabinetArtwork()
    {
        Sprite cabinetArtwork =
            AssetDatabase.LoadAssetAtPath<Sprite>(ClassroomStorageCabinetArtworkPath);
        if (cabinetArtwork == null)
        {
            throw new InvalidOperationException("找不到教室储物柜美术素材。");
        }

        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject cabinetBody = FindInScene(classroom, "Storage Cabinet Body");
        SpriteRenderer baseRenderer = cabinetBody.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("教室储物柜缺少精灵渲染器。");
        }

        Transform artwork = cabinetBody.transform.Find("Storage Cabinet Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Storage Cabinet Artwork").transform;
            artwork.SetParent(cabinetBody.transform, false);
            Vector3 parentScale = cabinetBody.transform.lossyScale;
            artwork.localScale = new Vector3(
                1.25f / (cabinetArtwork.bounds.size.x * parentScale.x),
                1.8f / (cabinetArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = cabinetArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Shrink Classroom Storage Cabinet Collider")]
    public static void ShrinkClassroomStorageCabinetCollider()
    {
        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject cabinetBody = FindInScene(classroom, "Storage Cabinet Body");
        BoxCollider2D collider = cabinetBody.GetComponent<BoxCollider2D>();
        if (collider == null) throw new InvalidOperationException("教室储物柜缺少碰撞箱。");

        collider.size = new Vector2(.8f, .85f);
        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Library Card Artwork")]
    public static void ApplyLibraryCardArtwork()
    {
        Sprite cardArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryCardArtworkPath);
        if (cardArtwork == null)
        {
            throw new InvalidOperationException("找不到借书卡美术素材。");
        }

        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject libraryCard = FindInScene(classroom, "Library Card");
        SpriteRenderer baseRenderer = libraryCard.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("借书卡缺少精灵渲染器。");
        }

        Transform artwork = libraryCard.transform.Find("Library Card Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Library Card Artwork").transform;
            artwork.SetParent(libraryCard.transform, false);
            Vector3 parentScale = libraryCard.transform.lossyScale;
            artwork.localScale = new Vector3(
                .7f / (cardArtwork.bounds.size.x * parentScale.x),
                .3f / (cardArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = cardArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

        baseRenderer.sprite = cardArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Library Glue Bottle Artwork")]
    public static void ApplyLibraryGlueBottleArtwork()
    {
        Sprite glueArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(GlueBottleArtworkPath);
        if (glueArtwork == null)
        {
            throw new InvalidOperationException("找不到胶水瓶美术素材。");
        }

        Scene libraryScene = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        GameObject glueBottle = FindInScene(libraryScene, "Library Glue Bottle");
        SpriteRenderer baseRenderer = glueBottle.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("胶水瓶缺少精灵渲染器。");
        }

        Transform artwork = glueBottle.transform.Find("Glue Bottle Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Glue Bottle Artwork").transform;
            artwork.SetParent(glueBottle.transform, false);
            Vector3 parentScale = glueBottle.transform.lossyScale;
            artwork.localScale = new Vector3(
                .8f / (glueArtwork.bounds.size.x * parentScale.x),
                .35f / (glueArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = glueArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

        baseRenderer.sprite = glueArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(libraryScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Janitor Key Ring Artwork")]
    public static void ApplyJanitorKeyRingArtwork()
    {
        Sprite keyRingArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(JanitorKeyRingArtworkPath);
        if (keyRingArtwork == null)
        {
            throw new InvalidOperationException("找不到校工钥匙串美术素材。");
        }

        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        GameObject keyRing = FindInScene(corridor, "Janitor Key Ring");
        SpriteRenderer baseRenderer = keyRing.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("校工钥匙串缺少精灵渲染器。");
        }

        Transform artwork = keyRing.transform.Find("Janitor Key Ring Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Janitor Key Ring Artwork").transform;
            artwork.SetParent(keyRing.transform, false);
            Vector3 parentScale = keyRing.transform.lossyScale;
            artwork.localScale = new Vector3(
                .55f / (keyRingArtwork.bounds.size.x * parentScale.x),
                .45f / (keyRingArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = keyRingArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

        baseRenderer.sprite = keyRingArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(corridor);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Storage Broom Artwork")]
    public static void ApplyStorageBroomArtwork()
    {
        Sprite broomArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(StorageBroomArtworkPath);
        if (broomArtwork == null)
        {
            throw new InvalidOperationException("找不到储藏室扫帚美术素材。");
        }

        Scene storageScene = EditorSceneManager.OpenScene(StorageRoomScenePath, OpenSceneMode.Single);
        GameObject broom = FindInScene(storageScene, "Storage Broom");
        SpriteRenderer baseRenderer = broom.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("储藏室扫帚缺少精灵渲染器。");
        }

        Transform artwork = broom.transform.Find("Storage Broom Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Storage Broom Artwork").transform;
            artwork.SetParent(broom.transform, false);
            Vector3 parentScale = broom.transform.lossyScale;
            artwork.localScale = new Vector3(
                .45f / (broomArtwork.bounds.size.x * parentScale.x),
                1.5f / (broomArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = broomArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

        baseRenderer.sprite = broomArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(storageScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Storage Shelf Left Artwork")]
    public static void ApplyStorageShelfLeftArtwork()
    {
        Sprite shelfArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(StorageShelfLeftArtworkPath);
        if (shelfArtwork == null)
        {
            throw new InvalidOperationException("找不到左侧储物架美术素材。");
        }

        Scene storageScene = EditorSceneManager.OpenScene(StorageRoomScenePath, OpenSceneMode.Single);
        GameObject shelf = FindInScene(storageScene, "Storage Shelf Left");
        SpriteRenderer baseRenderer = shelf.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("左侧储物架缺少精灵渲染器。");
        }

        Transform artwork = shelf.transform.Find("Storage Shelf Left Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Storage Shelf Left Artwork").transform;
            artwork.SetParent(shelf.transform, false);
        }

        Vector3 parentScale = shelf.transform.lossyScale;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            4.6f / (shelfArtwork.bounds.size.x * parentScale.x),
            .95f / (shelfArtwork.bounds.size.y * parentScale.y), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = shelfArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(storageScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Storage Shelf Right Artwork")]
    public static void ApplyStorageShelfRightArtwork()
    {
        Sprite shelfArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(StorageShelfRightArtworkPath);
        if (shelfArtwork == null)
        {
            throw new InvalidOperationException("找不到右侧储物架美术素材。");
        }

        Scene storageScene = EditorSceneManager.OpenScene(StorageRoomScenePath, OpenSceneMode.Single);
        GameObject shelf = FindInScene(storageScene, "Storage Shelf Right");
        SpriteRenderer baseRenderer = shelf.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("右侧储物架缺少精灵渲染器。");
        }

        Transform artwork = shelf.transform.Find("Storage Shelf Right Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Storage Shelf Right Artwork").transform;
            artwork.SetParent(shelf.transform, false);
        }

        Vector3 parentScale = shelf.transform.lossyScale;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            4.6f / (shelfArtwork.bounds.size.x * parentScale.x),
            .95f / (shelfArtwork.bounds.size.y * parentScale.y), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = shelfArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(storageScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Repair Storage Shelf Artwork Scale")]
    public static void RepairStorageShelfArtworkScale()
    {
        ApplyStorageShelfLeftArtwork();
        ApplyStorageShelfRightArtwork();
    }

    [MenuItem("Tools/Devil RPG/Resize Storage Broom Investigation")]
    public static void ResizeStorageBroomInvestigation()
    {
        Scene storageScene = EditorSceneManager.OpenScene(StorageRoomScenePath, OpenSceneMode.Single);
        BoxCollider2D investigationCollider = FindInScene(storageScene,
            "Storage Broom Investigation").GetComponent<BoxCollider2D>();
        if (investigationCollider == null)
        {
            throw new InvalidOperationException("扫帚调查点缺少触发器。");
        }

        investigationCollider.size = new Vector2(.7f, 1.6f);
        EditorSceneManager.SaveScene(storageScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Storage Crate North Artwork")]
    public static void ApplyStorageCrateNorthArtwork()
    {
        Sprite crateArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(StorageCrateNorthArtworkPath);
        if (crateArtwork == null)
        {
            throw new InvalidOperationException("找不到北侧木箱美术素材。");
        }

        Scene storageScene = EditorSceneManager.OpenScene(StorageRoomScenePath, OpenSceneMode.Single);
        GameObject crate = FindInScene(storageScene, "Storage Crate North");
        SpriteRenderer baseRenderer = crate.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("北侧木箱缺少精灵渲染器。");
        }

        Transform artwork = crate.transform.Find("Storage Crate North Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Storage Crate North Artwork").transform;
            artwork.SetParent(crate.transform, false);
        }

        Vector3 parentScale = crate.transform.lossyScale;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            2f / (crateArtwork.bounds.size.x * parentScale.x),
            1.8f / (crateArtwork.bounds.size.y * parentScale.y), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = crateArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(storageScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Storage Crate South Exam Papers Artwork")]
    public static void ApplyStorageCrateSouthExamPapersArtwork()
    {
        Sprite papersArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(StorageCrateSouthDiscardedExamsArtworkPath);
        if (papersArtwork == null)
        {
            throw new InvalidOperationException("找不到南侧报废试卷美术素材。");
        }

        Scene storageScene = EditorSceneManager.OpenScene(StorageRoomScenePath, OpenSceneMode.Single);
        GameObject crate = FindInScene(storageScene, "Storage Crate South");
        SpriteRenderer baseRenderer = crate.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("南侧木箱缺少精灵渲染器。");
        }

        Transform artwork = crate.transform.Find("Storage Crate South Exam Papers Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Storage Crate South Exam Papers Artwork").transform;
            artwork.SetParent(crate.transform, false);
        }

        Vector3 parentScale = crate.transform.lossyScale;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1.7f / (papersArtwork.bounds.size.x * parentScale.x),
            1.25f / (papersArtwork.bounds.size.y * parentScale.y), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = papersArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(storageScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Kitchen Knife Artwork")]
    public static void ApplyKitchenKnifeArtwork()
    {
        Sprite knifeArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(KitchenKnifeArtworkPath);
        if (knifeArtwork == null)
        {
            throw new InvalidOperationException("找不到菜刀美术素材。");
        }

        Scene kitchenScene = EditorSceneManager.OpenScene(ApartmentKitchenScenePath, OpenSceneMode.Single);
        GameObject knife = FindInScene(kitchenScene, "Kitchen Knife");
        SpriteRenderer baseRenderer = knife.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("菜刀缺少精灵渲染器。");
        }

        Transform artwork = knife.transform.Find("Kitchen Knife Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Kitchen Knife Artwork").transform;
            artwork.SetParent(knife.transform, false);
            Vector3 parentScale = knife.transform.lossyScale;
            artwork.localScale = new Vector3(
                .9f / (knifeArtwork.bounds.size.x * parentScale.x),
                .28f / (knifeArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = knifeArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

        baseRenderer.sprite = knifeArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(kitchenScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Medicine Cabinet Key Artwork")]
    public static void ApplyMedicineCabinetKeyArtwork()
    {
        Sprite keyArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(MedicineCabinetKeyArtworkPath);
        if (keyArtwork == null)
        {
            throw new InvalidOperationException("找不到药柜钥匙美术素材。");
        }

        Scene bathroomScene = EditorSceneManager.OpenScene(ApartmentBathroomScenePath, OpenSceneMode.Single);
        GameObject key = FindInScene(bathroomScene, "Bathroom Medicine Cabinet Key");
        SpriteRenderer baseRenderer = key.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("药柜钥匙缺少精灵渲染器。");
        }

        Transform artwork = key.transform.Find("Medicine Cabinet Key Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Medicine Cabinet Key Artwork").transform;
            artwork.SetParent(key.transform, false);
            Vector3 parentScale = key.transform.lossyScale;
            artwork.localScale = new Vector3(
                .45f / (keyArtwork.bounds.size.x * parentScale.x),
                .25f / (keyArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = keyArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;

        baseRenderer.sprite = keyArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(bathroomScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Library Fragment Artwork")]
    public static void ApplyLibraryFragmentArtwork()
    {
        Sprite[] fragments = new Sprite[3];
        for (int index = 0; index < fragments.Length; index++)
        {
            fragments[index] = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryFragmentArtworkPaths[index]);
            if (fragments[index] == null)
            {
                throw new InvalidOperationException("找不到第 " + (index + 1) + " 张残页碎片美术素材。");
            }
        }

        Scene libraryScene = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        for (int index = 0; index < fragments.Length; index++)
        {
            int number = index + 1;
            ApplyFragmentVisual(FindInScene(libraryScene, "Library Fragment " + number),
                fragments[index]);
            ApplyFragmentVisual(FindInScene(libraryScene, "Placed Library Fragment " + number),
                fragments[index]);
        }

        EditorSceneManager.SaveScene(libraryScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Complete Library Page Artwork")]
    public static void ApplyCompleteLibraryPageArtwork()
    {
        Sprite pageArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(CompleteLibraryPageArtworkPath);
        if (pageArtwork == null)
        {
            throw new InvalidOperationException("找不到完整残页美术素材。");
        }

        Scene libraryScene = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        GameObject page = FindInScene(libraryScene, "Complete Library Page");
        SpriteRenderer baseRenderer = page.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException("完整残页缺少精灵渲染器。");
        }

        Transform artwork = page.transform.Find("Complete Page Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Complete Page Artwork").transform;
            artwork.SetParent(page.transform, false);
            Vector3 parentScale = page.transform.lossyScale;
            artwork.localScale = new Vector3(
                1.7f / (pageArtwork.bounds.size.x * parentScale.x),
                .65f / (pageArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = pageArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.sprite = pageArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(libraryScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Boxing Gloves Artwork")]
    public static void ApplyBoxingGlovesArtwork()
    {
        Sprite glovesArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(BoxingGlovesArtworkPath);
        if (glovesArtwork == null) throw new InvalidOperationException("找不到拳击手套美术素材。");

        Scene livingRoomScene = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject gloves = FindInScene(livingRoomScene, "Boxing Gloves");
        SpriteRenderer baseRenderer = gloves.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("拳击手套缺少精灵渲染器。");

        Transform artwork = gloves.transform.Find("Boxing Gloves Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Boxing Gloves Artwork").transform;
            artwork.SetParent(gloves.transform, false);
            Vector3 parentScale = gloves.transform.lossyScale;
            artwork.localScale = new Vector3(
                .8f / (glovesArtwork.bounds.size.x * parentScale.x),
                .45f / (glovesArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = glovesArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.sprite = glovesArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(livingRoomScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Lin Chunbao Phone Artwork")]
    public static void ApplyLinChunbaoPhoneArtwork()
    {
        Sprite phoneArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LinChunbaoPhoneArtworkPath);
        if (phoneArtwork == null) throw new InvalidOperationException("找不到林春宝手机美术素材。");

        Scene bedroomScene = EditorSceneManager.OpenScene(ApartmentLinChunbaoBedroomScenePath, OpenSceneMode.Single);
        GameObject phone = FindInScene(bedroomScene, "Lin Chunbao Phone");
        SpriteRenderer baseRenderer = phone.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("林春宝的手机缺少精灵渲染器。");

        Transform artwork = phone.transform.Find("Lin Chunbao Phone Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Lin Chunbao Phone Artwork").transform;
            artwork.SetParent(phone.transform, false);
            Vector3 parentScale = phone.transform.lossyScale;
            artwork.localScale = new Vector3(
                .45f / (phoneArtwork.bounds.size.x * parentScale.x),
                .28f / (phoneArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = phoneArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.sprite = phoneArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(bedroomScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Boxing Password Note Artwork")]
    public static void ApplyBoxingPasswordNoteArtwork()
    {
        Sprite noteArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(BoxingPasswordNoteArtworkPath);
        if (noteArtwork == null) throw new InvalidOperationException("找不到沙袋密码纸条美术素材。");

        Scene livingRoomScene = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject sandbag = FindInScene(livingRoomScene, "Living Room Sandbag");
        SandbagBoxingMinigame boxing = sandbag.GetComponent<SandbagBoxingMinigame>();
        if (boxing == null) throw new InvalidOperationException("沙袋缺少拳击小游戏组件。");
        SetReference(boxing, "passwordNoteSprite", noteArtwork);

        EditorSceneManager.SaveScene(livingRoomScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Bathroom Password Note Artwork")]
    public static void ApplyBathroomPasswordNoteArtwork()
    {
        Sprite noteArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(BathroomPasswordNoteArtworkPath);
        if (noteArtwork == null) throw new InvalidOperationException("找不到洗手间密码纸条美术素材。");

        Scene bathroomScene = EditorSceneManager.OpenScene(ApartmentBathroomScenePath, OpenSceneMode.Single);
        GameObject note = FindInScene(bathroomScene, "Bathroom Password Note 8");
        SpriteRenderer baseRenderer = note.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("洗手间密码纸条缺少精灵渲染器。");

        Transform artwork = note.transform.Find("Bathroom Password Note Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Bathroom Password Note Artwork").transform;
            artwork.SetParent(note.transform, false);
            Vector3 parentScale = note.transform.lossyScale;
            artwork.localScale = new Vector3(
                .8f / (noteArtwork.bounds.size.x * parentScale.x),
                .48f / (noteArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = noteArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.sprite = noteArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;

        Transform number = note.transform.Find("Bathroom Password Number 8");
        if (number != null) number.gameObject.SetActive(false);

        EditorSceneManager.SaveScene(bathroomScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Living Room Sandbag Artwork")]
    public static void ApplyLivingRoomSandbagArtwork()
    {
        Sprite sandbagArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomSandbagArtworkPath);
        if (sandbagArtwork == null) throw new InvalidOperationException("找不到客厅沙袋美术素材。");

        Scene livingRoomScene = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject sandbag = FindInScene(livingRoomScene, "Living Room Sandbag");
        SpriteRenderer baseRenderer = sandbag.GetComponent<SpriteRenderer>();
        SandbagBoxingMinigame boxing = sandbag.GetComponent<SandbagBoxingMinigame>();
        if (baseRenderer == null || boxing == null) throw new InvalidOperationException("客厅沙袋缺少必要组件。");

        Transform artwork = sandbag.transform.Find("Living Room Sandbag Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Living Room Sandbag Artwork").transform;
            artwork.SetParent(sandbag.transform, false);
            Vector3 parentScale = sandbag.transform.lossyScale;
            artwork.localScale = new Vector3(
                1.35f / (sandbagArtwork.bounds.size.x * parentScale.x),
                2.25f / (sandbagArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = sandbagArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = new Color(.42f, .12f, .1f, 1f);
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.sprite = sandbagArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;
        SetReference(boxing, "sandbagArtworkRenderer", artworkRenderer);

        EditorSceneManager.SaveScene(livingRoomScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Broken Living Room Sandbag Artwork")]
    public static void ApplyBrokenLivingRoomSandbagArtwork()
    {
        Sprite brokenArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(BrokenLivingRoomSandbagArtworkPath);
        if (brokenArtwork == null) throw new InvalidOperationException("找不到破损客厅沙袋美术素材。");

        Scene livingRoomScene = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject sandbag = FindInScene(livingRoomScene, "Living Room Sandbag");
        SandbagBoxingMinigame boxing = sandbag.GetComponent<SandbagBoxingMinigame>();
        if (boxing == null) throw new InvalidOperationException("客厅沙袋缺少拳击小游戏组件。");
        SetReference(boxing, "brokenSandbagSprite", brokenArtwork);

        EditorSceneManager.SaveScene(livingRoomScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Bathroom Plaster Artwork")]
    public static void ApplyBathroomPlasterArtwork()
    {
        Sprite plasterArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(BathroomPlasterArtworkPath);
        if (plasterArtwork == null) throw new InvalidOperationException("找不到膏药美术素材。");

        Scene bathroomScene = EditorSceneManager.OpenScene(ApartmentBathroomScenePath, OpenSceneMode.Single);
        GameObject plaster = FindInScene(bathroomScene, "Bathroom Plaster");
        SpriteRenderer baseRenderer = plaster.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("膏药缺少精灵渲染器。");

        Transform artwork = plaster.transform.Find("Bathroom Plaster Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Bathroom Plaster Artwork").transform;
            artwork.SetParent(plaster.transform, false);
            Vector3 parentScale = plaster.transform.lossyScale;
            artwork.localScale = new Vector3(
                .7f / (plasterArtwork.bounds.size.x * parentScale.x),
                .38f / (plasterArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = plasterArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.sprite = plasterArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(bathroomScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Dried Meat Artwork")]
    public static void ApplyDriedMeatArtwork()
    {
        Sprite meatArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(DriedMeatArtworkPath);
        if (meatArtwork == null) throw new InvalidOperationException("找不到肉干美术素材。");

        Scene kitchenScene = EditorSceneManager.OpenScene(ApartmentKitchenScenePath, OpenSceneMode.Single);
        GameObject driedMeat = FindInScene(kitchenScene, "Dried Meat");
        SpriteRenderer baseRenderer = driedMeat.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("肉干缺少精灵渲染器。");

        Transform artwork = driedMeat.transform.Find("Dried Meat Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Dried Meat Artwork").transform;
            artwork.SetParent(driedMeat.transform, false);
            Vector3 parentScale = driedMeat.transform.lossyScale;
            artwork.localScale = new Vector3(
                .75f / (meatArtwork.bounds.size.x * parentScale.x),
                .35f / (meatArtwork.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = meatArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.sprite = meatArtwork;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(kitchenScene);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Apartment Carpet Floor Artwork")]
    public static void ApplyApartmentCarpetFloorArtwork()
    {
        ConfigureFloorTextureImporter(ApartmentLivingRoomCarpetFloorTexturePath, "客厅地毯地板贴图");
        ConfigureFloorTextureImporter(ApartmentProtagonistBedroomCarpetFloorTexturePath,
            "主角卧室地毯地板贴图");
        ConfigureFloorTextureImporter(ApartmentLinBedroomCarpetFloorTexturePath,
            "林春宝卧室地毯地板贴图");

        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath,
            OpenSceneMode.Single);
        ApplyLivingRoomCarpetFloorArtwork(livingRoom);
        EditorSceneManager.SaveScene(livingRoom);

        Scene protagonistBedroom = EditorSceneManager.OpenScene(ApartmentProtagonistBedroomScenePath,
            OpenSceneMode.Single);
        ApplyProtagonistBedroomCarpetFloorArtwork(protagonistBedroom);
        EditorSceneManager.SaveScene(protagonistBedroom);

        Scene linBedroom = EditorSceneManager.OpenScene(ApartmentLinChunbaoBedroomScenePath,
            OpenSceneMode.Single);
        ApplyLinBedroomCarpetFloorArtwork(linBedroom);
        EditorSceneManager.SaveScene(linBedroom);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyLivingRoomCarpetFloorArtwork(Scene scene)
    {
        ApplySceneFloorVisual(scene, "Living Room Floor",
            ApartmentLivingRoomCarpetFloorTexturePath, "客厅地毯地板贴图", new Vector2(15f, 9f));
    }

    private static void ApplyProtagonistBedroomCarpetFloorArtwork(Scene scene)
    {
        ApplySceneFloorVisual(scene, "Protagonist Bedroom Floor",
            ApartmentProtagonistBedroomCarpetFloorTexturePath, "主角卧室地毯地板贴图",
            new Vector2(7f, 5f));
    }

    private static void ApplyLinBedroomCarpetFloorArtwork(Scene scene)
    {
        ApplySceneFloorVisual(scene, "Lin Bedroom Floor",
            ApartmentLinBedroomCarpetFloorTexturePath, "林春宝卧室地毯地板贴图",
            new Vector2(7f, 5f));
    }

    private static void ApplySceneFloorVisual(Scene scene, string floorName,
        string artworkPath, string artworkLabel, Vector2 floorSize)
    {
        Sprite artwork = AssetDatabase.LoadAssetAtPath<Sprite>(artworkPath);
        if (artwork == null) throw new InvalidOperationException(artworkLabel + "导入失败。");

        GameObject floor = FindInScene(scene, floorName);
        SpriteRenderer renderer = floor.GetComponent<SpriteRenderer>();
        if (renderer == null) throw new InvalidOperationException(floorName + " 缺少精灵渲染器。");

        floor.transform.localScale = new Vector3(
            floorSize.x / artwork.bounds.size.x,
            floorSize.y / artwork.bounds.size.y,
            1f);
        renderer.sprite = artwork;
        renderer.drawMode = SpriteDrawMode.Simple;
        renderer.color = Color.white;
        renderer.sortingOrder = -10;
    }

    [MenuItem("Tools/Devil RPG/Apply Kitchen And Bathroom Checker Floor Artwork")]
    public static void ApplyKitchenAndBathroomCheckerFloorArtwork()
    {
        ConfigureFloorTextureImporter(ApartmentKitchenCheckerFloorTexturePath, "厨房格子地板贴图");
        ConfigureFloorTextureImporter(ApartmentBathroomCheckerFloorTexturePath, "洗手间格子地板贴图");

        Scene kitchen = EditorSceneManager.OpenScene(ApartmentKitchenScenePath, OpenSceneMode.Single);
        ApplyKitchenCheckerFloorArtwork(kitchen);
        EditorSceneManager.SaveScene(kitchen);

        Scene bathroom = EditorSceneManager.OpenScene(ApartmentBathroomScenePath, OpenSceneMode.Single);
        ApplyBathroomCheckerFloorArtwork(bathroom);
        EditorSceneManager.SaveScene(bathroom);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyKitchenCheckerFloorArtwork(Scene scene)
    {
        ApplySceneFloorVisual(scene, "Kitchen Floor", ApartmentKitchenCheckerFloorTexturePath,
            "厨房格子地板贴图", new Vector2(10f, 7f));
    }

    private static void ApplyBathroomCheckerFloorArtwork(Scene scene)
    {
        ApplySceneFloorVisual(scene, "Bathroom Floor", ApartmentBathroomCheckerFloorTexturePath,
            "洗手间格子地板贴图", new Vector2(7f, 5f));
    }

    [MenuItem("Tools/Devil RPG/Apply Suburb Road Guardrail Artwork")]
    public static void ApplySuburbRoadGuardrailArtwork()
    {
        ConfigureFloorTextureImporter(SuburbRoadGuardrailArtworkPath, "郊区道路护栏素材");
        Scene road = EditorSceneManager.OpenScene(RoadToSuburbsOneScenePath, OpenSceneMode.Single);
        ApplySuburbRoadGuardrailArtwork(road);
        EditorSceneManager.SaveScene(road);
        AssetDatabase.SaveAssets();
    }

    private static void ApplySuburbRoadGuardrailArtwork(Scene road)
    {
        Sprite artwork = AssetDatabase.LoadAssetAtPath<Sprite>(SuburbRoadGuardrailArtworkPath);
        if (artwork == null) throw new InvalidOperationException("郊区道路护栏素材导入失败。");

        GameObject guardrail = FindInScene(road, "Suburbs Road Guardrail");
        SpriteRenderer baseRenderer = guardrail.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("郊区道路护栏缺少精灵渲染器。");

        Transform visual = guardrail.transform.Find("Suburbs Road Guardrail Artwork");
        if (visual == null)
        {
            visual = new GameObject("Suburbs Road Guardrail Artwork").transform;
            visual.SetParent(guardrail.transform, false);
        }

        visual.localPosition = Vector3.zero;
        visual.localRotation = Quaternion.identity;
        visual.localScale = new Vector3(
            1f / artwork.bounds.size.x,
            1f / artwork.bounds.size.y, 1f);
        SpriteRenderer visualRenderer = visual.GetComponent<SpriteRenderer>();
        if (visualRenderer == null) visualRenderer = visual.gameObject.AddComponent<SpriteRenderer>();
        visualRenderer.sprite = artwork;
        visualRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        visualRenderer.color = Color.white;
        visualRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        visualRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Apply Suburban Maze Barricade Artwork")]
    public static void ApplySuburbanMazeBarricadeArtwork()
    {
        foreach (string path in new[]
                 {
                     SuburbMazeBarricadeHorizontalShortArtworkPath,
                     SuburbMazeBarricadeHorizontalMediumArtworkPath,
                     SuburbMazeBarricadeHorizontalLongArtworkPath,
                     SuburbMazeBarricadeHorizontalBoundaryArtworkPath,
                     SuburbMazeBarricadeVerticalShortArtworkPath,
                     SuburbMazeBarricadeVerticalMediumArtworkPath
                 })
        {
            ConfigureFloorTextureImporter(path, "郊区迷宫路障素材");
        }

        Scene roadOne = EditorSceneManager.OpenScene(RoadToSuburbsOneScenePath, OpenSceneMode.Single);
        ApplySuburbanMazeBarricadeArtwork(roadOne);
        EditorSceneManager.SaveScene(roadOne);

        Scene roadTwo = EditorSceneManager.OpenScene(RoadToSuburbsTwoScenePath, OpenSceneMode.Single);
        ApplySuburbanMazeBarricadeArtwork(roadTwo);
        EditorSceneManager.SaveScene(roadTwo);
        AssetDatabase.SaveAssets();
    }

    private static void ApplySuburbanMazeBarricadeArtwork(Scene scene)
    {
        Sprite horizontalShort = LoadBarricadeArtwork(SuburbMazeBarricadeHorizontalShortArtworkPath);
        Sprite horizontalMedium = LoadBarricadeArtwork(SuburbMazeBarricadeHorizontalMediumArtworkPath);
        Sprite horizontalLong = LoadBarricadeArtwork(SuburbMazeBarricadeHorizontalLongArtworkPath);
        Sprite horizontalBoundary = LoadBarricadeArtwork(SuburbMazeBarricadeHorizontalBoundaryArtworkPath);
        Sprite verticalShort = LoadBarricadeArtwork(SuburbMazeBarricadeVerticalShortArtworkPath);
        Sprite verticalMedium = LoadBarricadeArtwork(SuburbMazeBarricadeVerticalMediumArtworkPath);

        foreach (Transform candidate in scene.GetRootGameObjects()
                     .SelectMany(root => root.GetComponentsInChildren<Transform>(true)))
        {
            if (!candidate.name.StartsWith("Road Maze ") && !candidate.name.StartsWith("Road 2 "))
            {
                continue;
            }

            if (candidate.name.Contains("Artwork")) continue;
            SpriteRenderer baseRenderer = candidate.GetComponent<SpriteRenderer>();
            BoxCollider2D collider = candidate.GetComponent<BoxCollider2D>();
            if (baseRenderer == null || collider == null) continue;

            Vector2 wallSize = new Vector2(
                Mathf.Abs(candidate.lossyScale.x), Mathf.Abs(candidate.lossyScale.y));
            bool horizontal = wallSize.x >= wallSize.y;
            float longSide = horizontal ? wallSize.x : wallSize.y;
            Sprite artwork = horizontal
                ? longSide > 12f ? horizontalBoundary
                    : longSide > 6f ? horizontalLong
                    : longSide > 3f ? horizontalMedium : horizontalShort
                : longSide > 2f ? verticalMedium : verticalShort;

            ApplyMazeBarricadeVisual(candidate, baseRenderer, artwork, horizontal, wallSize);
        }
    }

    private static Sprite LoadBarricadeArtwork(string path)
    {
        Sprite artwork = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (artwork == null) throw new InvalidOperationException("迷宫路障素材导入失败：" + path);
        return artwork;
    }

    [MenuItem("Tools/Devil RPG/Apply Rubble Pile Barricade Artwork")]
    public static void ApplyRubblePileBarricadeArtwork()
    {
        foreach (string path in new[]
                 {
                     RubbleBarricadeHorizontalShortArtworkPath,
                     RubbleBarricadeHorizontalMediumArtworkPath,
                     RubbleBarricadeHorizontalLongArtworkPath,
                     RubbleBarricadeHorizontalBoundaryArtworkPath,
                     RubbleBarricadeVerticalShortArtworkPath,
                     RubbleBarricadeVerticalMediumArtworkPath
                 })
        {
            ConfigureFloorTextureImporter(path, "瓦砾堆障碍物素材");
        }

        Scene rubble = EditorSceneManager.OpenScene(RubblePileScenePath, OpenSceneMode.Single);
        ApplyRubblePileBarricadeArtwork(rubble);
        EditorSceneManager.SaveScene(rubble);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyRubblePileBarricadeArtwork(Scene scene)
    {
        Sprite horizontalShort = LoadBarricadeArtwork(RubbleBarricadeHorizontalShortArtworkPath);
        Sprite horizontalMedium = LoadBarricadeArtwork(RubbleBarricadeHorizontalMediumArtworkPath);
        Sprite horizontalLong = LoadBarricadeArtwork(RubbleBarricadeHorizontalLongArtworkPath);
        Sprite horizontalBoundary = LoadBarricadeArtwork(RubbleBarricadeHorizontalBoundaryArtworkPath);
        Sprite verticalShort = LoadBarricadeArtwork(RubbleBarricadeVerticalShortArtworkPath);
        Sprite verticalMedium = LoadBarricadeArtwork(RubbleBarricadeVerticalMediumArtworkPath);

        foreach (Transform wall in scene.GetRootGameObjects()
                     .SelectMany(root => root.GetComponentsInChildren<Transform>(true))
                     .Where(candidate => candidate.name.StartsWith("Rubble ") && candidate.name.Contains("Wall")))
        {
            SpriteRenderer baseRenderer = wall.GetComponent<SpriteRenderer>();
            BoxCollider2D collider = wall.GetComponent<BoxCollider2D>();
            if (baseRenderer == null || collider == null) continue;

            Vector2 wallSize = new Vector2(Mathf.Abs(wall.lossyScale.x), Mathf.Abs(wall.lossyScale.y));
            bool horizontal = wallSize.x >= wallSize.y;
            float longSide = horizontal ? wallSize.x : wallSize.y;
            Sprite artwork = horizontal
                ? longSide > 12f ? horizontalBoundary
                    : longSide > 6f ? horizontalLong
                    : longSide > 3f ? horizontalMedium : horizontalShort
                : longSide > 2f ? verticalMedium : verticalShort;

            ApplyRubbleBarricadeVisual(wall, baseRenderer, artwork, horizontal, wallSize);
        }
    }

    private static void ApplyRubbleBarricadeVisual(Transform wall, SpriteRenderer baseRenderer,
        Sprite artwork, bool horizontal, Vector2 wallSize)
    {
        const float visualThickness = .8f;
        Transform visual = wall.Find("Rubble Barricade Artwork");
        if (visual == null)
        {
            visual = new GameObject("Rubble Barricade Artwork").transform;
            visual.SetParent(wall, false);
            visual.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector2 visualSize = horizontal
            ? new Vector2(wallSize.x, visualThickness)
            : new Vector2(visualThickness, wallSize.y);
        Vector3 parentScale = wall.lossyScale;
        visual.localPosition = Vector3.zero;
        visual.localRotation = Quaternion.identity;
        visual.localScale = new Vector3(
            visualSize.x / (artwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            visualSize.y / (artwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer visualRenderer = visual.GetComponent<SpriteRenderer>();
        visualRenderer.sprite = artwork;
        visualRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        visualRenderer.color = Color.white;
        visualRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        visualRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Apply Library Wall Artwork")]
    public static void ApplyLibraryWallArtwork()
    {
        foreach (string path in new[]
                 {
                     LibraryWallTopArtworkPath, LibraryWallSideArtworkPath, LibraryWallBottomArtworkPath
                 })
        {
            ConfigureFloorTextureImporter(path, "图书馆墙体素材");
        }

        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        ApplyLibraryWallArtwork(library);
        EditorSceneManager.SaveScene(library);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyLibraryWallArtwork(Scene library)
    {
        Sprite topArtwork = LoadBarricadeArtwork(LibraryWallTopArtworkPath);
        Sprite sideArtwork = LoadBarricadeArtwork(LibraryWallSideArtworkPath);
        Sprite bottomArtwork = LoadBarricadeArtwork(LibraryWallBottomArtworkPath);

        ApplyLibraryWallVisual(FindInScene(library, "Library Wall Top"), "Library Wall Top Artwork",
            topArtwork, false);
        ApplyLibraryWallVisual(FindInScene(library, "Library Wall Left"), "Library Wall Side Artwork",
            sideArtwork, false);
        ApplyLibraryWallVisual(FindInScene(library, "Library Wall Right"), "Library Wall Side Artwork",
            sideArtwork, true);
        ApplyLibraryWallVisual(FindInScene(library, "Library Wall Bottom Left"),
            "Library Wall Bottom Artwork", bottomArtwork, false);
        ApplyLibraryWallVisual(FindInScene(library, "Library Wall Bottom Right"),
            "Library Wall Bottom Artwork", bottomArtwork, false);
    }

    private static void ApplyLibraryWallVisual(GameObject wall, string artworkName, Sprite artwork,
        bool flipX)
    {
        SpriteRenderer baseRenderer = wall.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(wall.name + " 缺少精灵渲染器。");

        Transform visual = wall.transform.Find(artworkName);
        if (visual == null)
        {
            visual = new GameObject(artworkName).transform;
            visual.SetParent(wall.transform, false);
            visual.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector2 targetSize = baseRenderer.bounds.size;
        Vector3 parentScale = wall.transform.lossyScale;
        visual.localPosition = Vector3.zero;
        visual.localRotation = Quaternion.identity;
        visual.localScale = new Vector3(
            targetSize.x / (artwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            targetSize.y / (artwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer visualRenderer = visual.GetComponent<SpriteRenderer>();
        visualRenderer.sprite = artwork;
        visualRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        visualRenderer.color = Color.white;
        visualRenderer.flipX = flipX;
        visualRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        visualRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Apply Library Loose Wall Panel Artwork")]
    public static void ApplyLibraryLooseWallPanelArtwork()
    {
        ConfigureFloorTextureImporter(LibraryLooseWallPanelArtworkPath, "图书馆松动墙面嵌板素材");
        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        ApplyLibraryLooseWallPanelArtwork(library);
        EditorSceneManager.SaveScene(library);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyLibraryLooseWallPanelArtwork(Scene library)
    {
        Sprite artwork = LoadBarricadeArtwork(LibraryLooseWallPanelArtworkPath);
        GameObject wallPanel = FindInScene(library, "Library Loose Wall Brick");
        SpriteRenderer baseRenderer = wallPanel.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("图书馆松动墙面嵌板缺少精灵渲染器。");

        Transform visual = wallPanel.transform.Find("Library Loose Wall Panel Artwork");
        if (visual == null)
        {
            visual = new GameObject("Library Loose Wall Panel Artwork").transform;
            visual.SetParent(wallPanel.transform, false);
            visual.gameObject.AddComponent<SpriteRenderer>();
        }

        // 生成图四周保留较宽透明边距；按实际内容边界放大，视觉尺寸仍与原调查点一致。
        Vector3 parentScale = wallPanel.transform.lossyScale;
        visual.localPosition = Vector3.zero;
        visual.localRotation = Quaternion.identity;
        visual.localScale = new Vector3(
            2.35f / (artwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            2.82f / (artwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer visualRenderer = visual.GetComponent<SpriteRenderer>();
        visualRenderer.sprite = artwork;
        visualRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        visualRenderer.color = Color.white;
        visualRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        visualRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    private static void ApplyMazeBarricadeVisual(Transform wall, SpriteRenderer baseRenderer,
        Sprite artwork, bool horizontal, Vector2 wallSize)
    {
        const float visualThickness = .8f;
        Transform visual = wall.Find("Maze Barricade Artwork");
        if (visual == null)
        {
            visual = new GameObject("Maze Barricade Artwork").transform;
            visual.SetParent(wall, false);
            visual.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector2 visualSize = horizontal
            ? new Vector2(wallSize.x, visualThickness)
            : new Vector2(visualThickness, wallSize.y);
        Vector3 parentScale = wall.lossyScale;
        visual.localPosition = Vector3.zero;
        visual.localRotation = Quaternion.identity;
        visual.localScale = new Vector3(
            visualSize.x / (artwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            visualSize.y / (artwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer visualRenderer = visual.GetComponent<SpriteRenderer>();
        visualRenderer.sprite = artwork;
        visualRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        visualRenderer.color = Color.white;
        visualRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        visualRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Apply Abyss Entrance Cave Mud Floor Artwork")]
    public static void ApplyAbyssEntranceCaveMudFloorArtwork()
    {
        ConfigureFloorTextureImporter(AbyssEntranceCaveMudFloorTexturePath, "归墟入口洞穴泥土地板贴图");
        Scene abyss = EditorSceneManager.OpenScene(AbyssEntranceScenePath, OpenSceneMode.Single);
        ApplyAbyssEntranceCaveMudFloorArtwork(abyss);
        EditorSceneManager.SaveScene(abyss);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Abyss Entrance Cave Wall Artwork")]
    public static void ApplyAbyssEntranceCaveWallArtwork()
    {
        ConfigureFloorTextureImporter(AbyssEntranceCaveWallTopArtworkPath, "归墟入口顶岩壁素材");
        ConfigureFloorTextureImporter(AbyssEntranceCaveWallLeftArtworkPath, "归墟入口左岩壁素材");
        ConfigureFloorTextureImporter(AbyssEntranceCaveWallRightArtworkPath, "归墟入口右岩壁素材");
        ConfigureFloorTextureImporter(AbyssEntranceCaveWallBottomArtworkPath, "归墟入口底岩壁素材");

        Scene abyss = EditorSceneManager.OpenScene(AbyssEntranceScenePath, OpenSceneMode.Single);
        ApplyAbyssEntranceCaveWallArtwork(abyss);
        EditorSceneManager.SaveScene(abyss);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Abyss Entrance Rock Protrusion Artwork")]
    public static void ApplyAbyssEntranceRockProtrusionArtwork()
    {
        ConfigureFloorTextureImporter(AbyssEntranceRockProtrusionArtworkPath, "归墟入口岩壁凸起素材");

        Sprite protrusionArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceRockProtrusionArtworkPath);
        if (protrusionArtwork == null) throw new InvalidOperationException("归墟入口岩壁凸起素材导入失败。");

        Scene abyss = EditorSceneManager.OpenScene(AbyssEntranceScenePath, OpenSceneMode.Single);
        ApplyAbyssEntranceRockProtrusionArtwork(abyss, protrusionArtwork);
        EditorSceneManager.SaveScene(abyss);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Abyss Entrance Glowing Moss Artwork")]
    public static void ApplyAbyssEntranceGlowingMossArtwork()
    {
        ConfigureFloorTextureImporter(AbyssEntranceRockyMossArtworkPath, "归墟入口岩缝荧光苔藓素材");
        ConfigureFloorTextureImporter(AbyssEntranceGlowingMossArtworkPath, "归墟入口荧光苔藓素材");

        Sprite rockyMossArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceRockyMossArtworkPath);
        Sprite mossArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceGlowingMossArtworkPath);
        if (rockyMossArtwork == null || mossArtwork == null)
        {
            throw new InvalidOperationException("归墟入口荧光苔藓素材导入失败。");
        }

        Scene abyss = EditorSceneManager.OpenScene(AbyssEntranceScenePath, OpenSceneMode.Single);
        ApplyAbyssEntranceGlowingMossArtwork(abyss, rockyMossArtwork, mossArtwork);
        EditorSceneManager.SaveScene(abyss);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Abyss Entrance Warm Crystal Artwork")]
    public static void ApplyAbyssEntranceWarmCrystalArtwork()
    {
        ConfigureFloorTextureImporter(AbyssEntranceWarmCrystalClusterArtworkPath, "归墟入口温热晶簇素材");

        Sprite crystalArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceWarmCrystalClusterArtworkPath);
        if (crystalArtwork == null) throw new InvalidOperationException("归墟入口温热晶簇素材导入失败。");

        Scene abyss = EditorSceneManager.OpenScene(AbyssEntranceScenePath, OpenSceneMode.Single);
        ApplyAbyssEntranceWarmCrystalArtwork(abyss, crystalArtwork);
        EditorSceneManager.SaveScene(abyss);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Abyss Entrance Side Cave Passage Artwork")]
    public static void ApplyAbyssEntranceSideCavePassageArtwork()
    {
        ConfigureFloorTextureImporter(AbyssEntranceSideCavePassageArtworkPath, "归墟入口侧向岩洞通道素材");

        Sprite passageArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceSideCavePassageArtworkPath);
        if (passageArtwork == null) throw new InvalidOperationException("归墟入口侧向岩洞通道素材导入失败。");

        Scene abyss = EditorSceneManager.OpenScene(AbyssEntranceScenePath, OpenSceneMode.Single);
        ApplyAbyssEntranceSideCavePassageArtwork(abyss, passageArtwork);
        EditorSceneManager.SaveScene(abyss);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Abyss Entrance Drop In")]
    public static void InstallAbyssEntranceDropIn()
    {
        Scene abyss = EditorSceneManager.OpenScene(AbyssEntranceScenePath, OpenSceneMode.Single);
        GameObject root = FindInScene(abyss, "归墟入口");
        if (root.GetComponent<AbyssEntranceDropIn>() == null)
        {
            root.AddComponent<AbyssEntranceDropIn>();
        }
        EditorSceneManager.SaveScene(abyss);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Abyss Entrance Interactions")]
    public static void InstallAbyssEntranceInteractions()
    {
        Scene abyss = EditorSceneManager.OpenScene(AbyssEntranceScenePath, OpenSceneMode.Single);
        GameObject root = FindInScene(abyss, "归墟入口");
        AbyssEntranceInteraction interaction = root.GetComponent<AbyssEntranceInteraction>();
        if (interaction == null) interaction = root.AddComponent<AbyssEntranceInteraction>();

        CreateAbyssEntranceInvestigationPoints(root.transform, interaction);
        EditorSceneManager.SaveScene(abyss);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Fluorescent Mushroom Cave Environment Artwork")]
    public static void ApplyFluorescentMushroomCaveEnvironmentArtwork()
    {
        ConfigureFloorTextureImporter(AbyssEntranceCaveMudFloorTexturePath, "洞穴泥土地板贴图");
        ConfigureFloorTextureImporter(AbyssEntranceCaveWallTopArtworkPath, "洞穴顶岩壁素材");
        ConfigureFloorTextureImporter(AbyssEntranceCaveWallLeftArtworkPath, "洞穴左岩壁素材");
        ConfigureFloorTextureImporter(AbyssEntranceCaveWallRightArtworkPath, "洞穴右岩壁素材");
        ConfigureFloorTextureImporter(AbyssEntranceCaveWallBottomArtworkPath, "洞穴底岩壁素材");

        Scene cave = EditorSceneManager.OpenScene(FluorescentMushroomCaveScenePath, OpenSceneMode.Single);
        ApplyFluorescentMushroomCaveEnvironmentArtwork(cave);
        EditorSceneManager.SaveScene(cave);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Fluorescent Mushroom Cave Artwork")]
    public static void ApplyFluorescentMushroomCaveArtwork()
    {
        ConfigureFloorTextureImporter(FluorescentMushroomClusterLargeArtworkPath, "最大荧光蘑菇丛素材");
        ConfigureFloorTextureImporter(FluorescentMushroomClusterLeftArtworkPath, "左侧荧光蘑菇丛素材");
        ConfigureFloorTextureImporter(FluorescentMushroomClusterRightArtworkPath, "右侧荧光蘑菇丛素材");

        Scene cave = EditorSceneManager.OpenScene(FluorescentMushroomCaveScenePath, OpenSceneMode.Single);
        ApplyFluorescentMushroomCaveArtwork(cave);
        EditorSceneManager.SaveScene(cave);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyAbyssEntranceCaveMudFloorArtwork(Scene abyss)
    {
        ApplySceneFloorVisual(abyss, "湿润的洞穴泥土", AbyssEntranceCaveMudFloorTexturePath,
            "归墟入口洞穴泥土地板贴图", new Vector2(19.4f, 7.5f));
        FindInScene(abyss, "湿润的洞穴泥土").GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    private static void ApplyAbyssEntranceCaveWallArtwork(Scene abyss)
    {
        Sprite topArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceCaveWallTopArtworkPath);
        Sprite leftArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceCaveWallLeftArtworkPath);
        Sprite rightArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceCaveWallRightArtworkPath);
        Sprite bottomArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceCaveWallBottomArtworkPath);
        if (topArtwork == null || leftArtwork == null || rightArtwork == null || bottomArtwork == null)
        {
            throw new InvalidOperationException("归墟入口岩壁美术素材导入失败。");
        }

        ApplyAbyssEntranceCaveWallVisual(FindInScene(abyss, "洞顶岩壁"), topArtwork);
        ApplyAbyssEntranceCaveWallVisual(FindInScene(abyss, "左侧岩壁"), leftArtwork);
        ApplyAbyssEntranceCaveWallVisual(FindInScene(abyss, "右侧岩壁"), rightArtwork);
        ApplyAbyssEntranceCaveWallVisual(FindInScene(abyss, "洞底岩壁"), bottomArtwork);
    }

    private static void ApplyFluorescentMushroomCaveEnvironmentArtwork(Scene cave)
    {
        GameObject bottomWall = FindInScene(cave, "菌洞底岩壁");
        bottomWall.transform.position = new Vector2(361.6f, -5.65f);
        bottomWall.transform.localScale = new Vector3(16.2f, .65f, 1f);

        ApplySceneFloorVisual(cave, "荧光菌洞地面", AbyssEntranceCaveMudFloorTexturePath,
            "荧光菌洞泥土地板贴图", new Vector2(20f, 7.7f));
        FindInScene(cave, "荧光菌洞地面").GetComponent<SpriteRenderer>().sortingOrder = 0;

        Sprite topArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceCaveWallTopArtworkPath);
        Sprite leftArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceCaveWallLeftArtworkPath);
        Sprite rightArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceCaveWallRightArtworkPath);
        Sprite bottomArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceCaveWallBottomArtworkPath);
        if (topArtwork == null || leftArtwork == null || rightArtwork == null || bottomArtwork == null)
        {
            throw new InvalidOperationException("荧光菌洞岩壁美术素材导入失败。");
        }

        ApplyAbyssEntranceCaveWallVisual(FindInScene(cave, "菌洞顶岩壁"), topArtwork);
        ApplyAbyssEntranceCaveWallVisual(FindInScene(cave, "菌洞左岩壁"), leftArtwork);
        ApplyAbyssEntranceCaveWallVisual(FindInScene(cave, "菌洞右岩壁"), rightArtwork);
        ApplyAbyssEntranceCaveWallVisual(FindInScene(cave, "菌洞底岩壁"), bottomArtwork);
    }

    private static void ApplyFluorescentMushroomCaveArtwork(Scene cave)
    {
        Sprite largeArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(FluorescentMushroomClusterLargeArtworkPath);
        Sprite leftArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(FluorescentMushroomClusterLeftArtworkPath);
        Sprite rightArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(FluorescentMushroomClusterRightArtworkPath);
        if (largeArtwork == null || leftArtwork == null || rightArtwork == null)
        {
            throw new InvalidOperationException("荧光菌洞蘑菇美术素材导入失败。");
        }

        ApplyFluorescentMushroomVisual(FindInScene(cave, "最大荧光蘑菇丛"), largeArtwork);
        ApplyFluorescentMushroomVisual(FindInScene(cave, "荧光蘑菇丛 左"), leftArtwork);
        ApplyFluorescentMushroomVisual(FindInScene(cave, "荧光蘑菇丛 右"), rightArtwork);
    }

    private static void ApplyFluorescentMushroomVisual(GameObject mushroom, Sprite artworkSprite)
    {
        SpriteRenderer baseRenderer = mushroom.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(mushroom.name + " 缺少精灵渲染器。");

        Transform artwork = mushroom.transform.Find(mushroom.name + " Artwork");
        if (artwork == null)
        {
            artwork = new GameObject(mushroom.name + " Artwork").transform;
            artwork.SetParent(mushroom.transform, false);
        }

        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1f / artworkSprite.bounds.size.x, 1f / artworkSprite.bounds.size.y, 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    private static void ApplyAbyssEntranceCaveWallVisual(GameObject wall, Sprite wallArtwork)
    {
        SpriteRenderer baseRenderer = wall.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(wall.name + " 缺少精灵渲染器。");

        Transform artwork = wall.transform.Find(wall.name + " Artwork");
        if (artwork == null)
        {
            artwork = new GameObject(wall.name + " Artwork").transform;
            artwork.SetParent(wall.transform, false);
        }

        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1f / wallArtwork.bounds.size.x,
            1f / wallArtwork.bounds.size.y, 1f);
        ApplyAbyssEntranceArtworkLayout(artwork, wall.name);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = wallArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = 2;
        baseRenderer.enabled = false;
    }

    private static void ApplyAbyssEntranceRockProtrusionArtwork(Scene abyss, Sprite protrusionArtwork)
    {
        ApplyAbyssEntranceRockProtrusionVisual(FindInScene(abyss, "岩壁突起一"), protrusionArtwork);
        ApplyAbyssEntranceRockProtrusionVisual(FindInScene(abyss, "岩壁突起二"), protrusionArtwork);
    }

    private static void ApplyAbyssEntranceRockProtrusionVisual(GameObject protrusion, Sprite protrusionArtwork)
    {
        SpriteRenderer baseRenderer = protrusion.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(protrusion.name + " 缺少精灵渲染器。");

        Transform artwork = protrusion.transform.Find(protrusion.name + " Artwork");
        if (artwork == null)
        {
            artwork = new GameObject(protrusion.name + " Artwork").transform;
            artwork.SetParent(protrusion.transform, false);
        }

        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1f / protrusionArtwork.bounds.size.x,
            1f / protrusionArtwork.bounds.size.y, 1f);
        ApplyAbyssEntranceArtworkLayout(artwork, protrusion.name);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = protrusionArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    private static void ApplyAbyssEntranceGlowingMossArtwork(Scene abyss, Sprite rockyMossArtwork,
        Sprite mossArtwork)
    {
        ApplyAbyssEntranceGlowingMossVisual(FindInScene(abyss, "荧光苔藓 左上"), rockyMossArtwork);
        ApplyAbyssEntranceGlowingMossVisual(FindInScene(abyss, "荧光苔藓 右上"), rockyMossArtwork);
        ApplyAbyssEntranceGlowingMossVisual(FindInScene(abyss, "荧光苔藓 左壁"), mossArtwork);
        ApplyAbyssEntranceGlowingMossVisual(FindInScene(abyss, "荧光苔藓 右壁"), mossArtwork);
    }

    private static void ApplyAbyssEntranceGlowingMossVisual(GameObject moss, Sprite mossArtwork)
    {
        SpriteRenderer baseRenderer = moss.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(moss.name + " 缺少精灵渲染器。");

        Transform artwork = moss.transform.Find(moss.name + " Artwork");
        if (artwork == null)
        {
            artwork = new GameObject(moss.name + " Artwork").transform;
            artwork.SetParent(moss.transform, false);
        }

        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1f / mossArtwork.bounds.size.x,
            1f / mossArtwork.bounds.size.y, 1f);
        ApplyAbyssEntranceArtworkLayout(artwork, moss.name);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = mossArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    private static void ApplyAbyssEntranceWarmCrystalArtwork(Scene abyss, Sprite crystalArtwork)
    {
        ApplyAbyssEntranceWarmCrystalVisual(FindInScene(abyss, "温热晶体 一"), crystalArtwork);
        ApplyAbyssEntranceWarmCrystalVisual(FindInScene(abyss, "温热晶体 二"), crystalArtwork);
        ApplyAbyssEntranceWarmCrystalVisual(FindInScene(abyss, "温热晶体 三"), crystalArtwork);
    }

    private static void ApplyAbyssEntranceWarmCrystalVisual(GameObject crystal, Sprite crystalArtwork)
    {
        SpriteRenderer baseRenderer = crystal.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(crystal.name + " 缺少精灵渲染器。");

        Transform artwork = crystal.transform.Find(crystal.name + " Artwork");
        if (artwork == null)
        {
            artwork = new GameObject(crystal.name + " Artwork").transform;
            artwork.SetParent(crystal.transform, false);
        }

        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1f / crystalArtwork.bounds.size.x,
            1f / crystalArtwork.bounds.size.y, 1f);
        ApplyAbyssEntranceArtworkLayout(artwork, crystal.name);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = crystalArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    private static void ApplyAbyssEntranceSideCavePassageArtwork(Scene abyss, Sprite passageArtwork)
    {
        GameObject passage = FindInScene(abyss, "荧光菌洞通道");
        SpriteRenderer baseRenderer = passage.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("荧光菌洞通道缺少精灵渲染器。");

        Transform artwork = passage.transform.Find("荧光菌洞通道 Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("荧光菌洞通道 Artwork").transform;
            artwork.SetParent(passage.transform, false);
        }

        artwork.localPosition = new Vector3(.22f, -.043f, 0f);
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(.35270506f, .098790534f, 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = passageArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    private static void ApplyAbyssEntranceArtworkLayout(Transform artwork, string ownerName)
    {
        switch (ownerName)
        {
            case "洞顶岩壁":
                artwork.localPosition = new Vector3(-.00132f, 1.3802f, 0f);
                artwork.localScale = new Vector3(.04672859f, .72137946f, 1f);
                break;
            case "岩壁突起一":
                artwork.localPosition = new Vector3(.2141f, -.6161f, 0f);
                artwork.localScale = new Vector3(.06575967f, .30816442f, 1f);
                break;
            case "岩壁突起二":
                artwork.localPosition = new Vector3(-.1323f, -.5776f, 0f);
                artwork.localScale = new Vector3(.058227442f, .2975205f, 1f);
                break;
            case "荧光苔藓 左上":
                artwork.localPosition = new Vector3(-.016f, .8f, 0f);
                artwork.localScale = new Vector3(.084880784f, .64626855f, 1f);
                break;
            case "荧光苔藓 右上":
                artwork.localPosition = new Vector3(-2.892f, -12.19f, 0f);
                artwork.localScale = new Vector3(.07856771f, .59814453f, 1f);
                break;
            case "荧光苔藓 左壁":
                artwork.localPosition = new Vector3(76f, 1.06f, 0f);
                artwork.localScale = new Vector3(.90729105f, .061984215f, 1f);
                break;
            case "荧光苔藓 右壁":
                artwork.localPosition = new Vector3(-7.8f, -.81f, 0f);
                artwork.localScale = new Vector3(1.2559259f, .07662909f, 1f);
                break;
            case "温热晶体 一":
                artwork.localPosition = new Vector3(2.617f, 1.3143f, 0f);
                artwork.localScale = new Vector3(.4058594f, .35435545f, 1f);
                break;
            case "温热晶体 二":
                artwork.localPosition = new Vector3(-.04f, .69f, 0f);
                artwork.localScale = new Vector3(.3269751f, .3535286f, 1f);
                break;
            case "温热晶体 三":
                artwork.localPosition = new Vector3(-2.94f, 1.18f, 0f);
                artwork.localScale = new Vector3(.46588546f, .26847658f, 1f);
                break;
        }
    }

    [MenuItem("Tools/Devil RPG/Apply Classroom Wood Floor Artwork")]
    public static void ApplyClassroomWoodFloorArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(ClassroomWoodFloorTexturePath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到教室木地板贴图。");
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(ClassroomWoodFloorTexturePath, ImportAssetOptions.ForceUpdate);

        Sprite floorArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(ClassroomWoodFloorTexturePath);
        if (floorArtwork == null) throw new InvalidOperationException("教室木地板贴图导入失败。");

        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject floor = FindOptionalInScene(classroom, "Classroom Floor Artwork")
            ?? FindInScene(classroom, "ChatGPT 图像 2026年10月2日 11_59_18");
        SpriteRenderer renderer = floor.GetComponent<SpriteRenderer>();
        if (renderer == null) throw new InvalidOperationException("教室地板缺少精灵渲染器。");

        const float floorWidth = 15.36f;
        const float floorHeight = 10.24f;
        floor.name = "Classroom Floor Artwork";
        floor.transform.localScale = new Vector3(
            floorWidth / floorArtwork.bounds.size.x,
            floorHeight / floorArtwork.bounds.size.y,
            1f);
        renderer.sprite = floorArtwork;
        renderer.drawMode = SpriteDrawMode.Simple;
        renderer.sortingOrder = -10;
        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Corridor Ceramic Tile Floor Artwork")]
    public static void ApplyCorridorCeramicTileFloorArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(CorridorCeramicTileFloorTexturePath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到走廊瓷砖地板贴图。");
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.wrapMode = TextureWrapMode.Repeat;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(CorridorCeramicTileFloorTexturePath, ImportAssetOptions.ForceUpdate);

        Sprite floorArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(CorridorCeramicTileFloorTexturePath);
        if (floorArtwork == null) throw new InvalidOperationException("走廊瓷砖地板贴图导入失败。");

        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        GameObject floor = FindOptionalInScene(corridor, "Corridor Floor Artwork");
        if (floor == null)
        {
            floor = new GameObject("Corridor Floor Artwork");
            floor.transform.SetParent(FindInScene(corridor, "School Corridor").transform, false);
            floor.transform.localPosition = new Vector3(30f, 0f, 0f);
            floor.AddComponent<SpriteRenderer>();
        }
        SpriteRenderer renderer = floor.GetComponent<SpriteRenderer>();
        if (renderer == null) throw new InvalidOperationException("走廊地板缺少精灵渲染器。");

        const float corridorWidth = 15.36f;
        const float corridorHeight = 3.84f;
        renderer.sprite = floorArtwork;
        renderer.drawMode = SpriteDrawMode.Simple;
        floor.transform.localScale = new Vector3(
            corridorWidth / floorArtwork.bounds.size.x,
            corridorHeight / floorArtwork.bounds.size.y,
            1f);
        renderer.color = Color.white;
        renderer.sortingOrder = -10;
        EditorSceneManager.SaveScene(corridor);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Library Wood Floor Artwork")]
    public static void ApplyLibraryWoodFloorArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(LibraryWoodFloorTexturePath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到图书馆木地板贴图。");
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(LibraryWoodFloorTexturePath, ImportAssetOptions.ForceUpdate);

        Sprite floorArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryWoodFloorTexturePath);
        if (floorArtwork == null) throw new InvalidOperationException("图书馆木地板贴图导入失败。");

        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        GameObject floor = FindOptionalInScene(library, "Library Floor Artwork")
            ?? FindInScene(library, "Library Floor");
        SpriteRenderer renderer = floor.GetComponent<SpriteRenderer>();
        if (renderer == null) throw new InvalidOperationException("图书馆地板缺少精灵渲染器。");

        const float floorWidth = 18f;
        const float floorHeight = 12f;
        floor.name = "Library Floor Artwork";
        floor.transform.position = new Vector3(60f, 0f, 0f);
        floor.transform.localScale = new Vector3(
            floorWidth / floorArtwork.bounds.size.x,
            floorHeight / floorArtwork.bounds.size.y,
            1f);
        renderer.sprite = floorArtwork;
        renderer.drawMode = SpriteDrawMode.Simple;
        renderer.color = Color.white;
        renderer.sortingOrder = -10;
        EditorSceneManager.SaveScene(library);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Storage Room Concrete Floor Artwork")]
    public static void ApplyStorageRoomConcreteFloorArtwork()
    {
        Scene storageScene = EditorSceneManager.OpenScene(StorageRoomScenePath, OpenSceneMode.Single);
        ApplyStorageRoomConcreteFloorArtwork(storageScene);
        EditorSceneManager.SaveScene(storageScene);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyStorageRoomConcreteFloorArtwork(Scene storageScene)
    {
        TextureImporter importer = AssetImporter.GetAtPath(StorageRoomConcreteFloorTexturePath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到储藏室水泥地板贴图。");
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(StorageRoomConcreteFloorTexturePath, ImportAssetOptions.ForceUpdate);

        Sprite floorArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(StorageRoomConcreteFloorTexturePath);
        if (floorArtwork == null) throw new InvalidOperationException("储藏室水泥地板贴图导入失败。");

        GameObject storage = FindInScene(storageScene, "School Storage Room");
        GameObject floor = FindInScene(storageScene, "Storage Floor");
        SpriteRenderer baseRenderer = floor.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("储藏室基础地板缺少精灵渲染器。");

        GameObject artwork = FindOptionalInScene(storageScene, "Storage Concrete Floor Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Storage Concrete Floor Artwork");
            artwork.transform.SetParent(storage.transform, false);
            artwork.AddComponent<SpriteRenderer>();
        }

        artwork.transform.position = floor.transform.position;
        artwork.transform.localRotation = Quaternion.identity;
        artwork.transform.localScale = new Vector3(
            8f / floorArtwork.bounds.size.x,
            14f / floorArtwork.bounds.size.y,
            1f);

        SpriteRenderer renderer = artwork.GetComponent<SpriteRenderer>();
        renderer.sprite = floorArtwork;
        renderer.sharedMaterial = baseRenderer.sharedMaterial;
        renderer.color = Color.white;
        renderer.drawMode = SpriteDrawMode.Simple;
        renderer.sortingOrder = -10;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Apply Apartment Entrance Asphalt Road Artwork")]
    public static void ApplyApartmentEntranceAsphaltRoadArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(ApartmentEntranceAsphaltRoadTexturePath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到公寓入口柏油马路贴图。");
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(ApartmentEntranceAsphaltRoadTexturePath,
            ImportAssetOptions.ForceUpdate);

        Scene apartment = EditorSceneManager.OpenScene(ApartmentEntranceScenePath,
            OpenSceneMode.Single);
        ApplyApartmentEntranceAsphaltRoadArtwork(apartment);
        EditorSceneManager.SaveScene(apartment);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyApartmentEntranceAsphaltRoadArtwork(Scene apartment)
    {
        Sprite roadArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(
            ApartmentEntranceAsphaltRoadTexturePath);
        if (roadArtwork == null) throw new InvalidOperationException("公寓入口柏油马路贴图导入失败。");

        GameObject root = FindInScene(apartment, "Apartment Entrance");
        GameObject street = FindInScene(apartment, "Apartment Street");
        SpriteRenderer baseRenderer = street.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("公寓入口马路缺少精灵渲染器。");

        GameObject artwork = FindOptionalInScene(apartment, "Apartment Entrance Asphalt Road Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Apartment Entrance Asphalt Road Artwork");
            artwork.transform.SetParent(root.transform, false);
            artwork.AddComponent<SpriteRenderer>();
        }

        artwork.transform.position = street.transform.position;
        artwork.transform.localRotation = Quaternion.identity;
        artwork.transform.localScale = new Vector3(
            18f / roadArtwork.bounds.size.x,
            3.6f / roadArtwork.bounds.size.y,
            1f);

        SpriteRenderer renderer = artwork.GetComponent<SpriteRenderer>();
        renderer.sprite = roadArtwork;
        renderer.sharedMaterial = baseRenderer.sharedMaterial;
        renderer.color = Color.white;
        renderer.drawMode = SpriteDrawMode.Simple;
        renderer.sortingOrder = -11;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Fix Apartment Entrance Ground Sorting")]
    public static void FixApartmentEntranceGroundSorting()
    {
        Scene apartment = EditorSceneManager.OpenScene(ApartmentEntranceScenePath,
            OpenSceneMode.Single);
        SpriteRenderer nightSky = FindInScene(apartment, "Apartment Night Sky")
            .GetComponent<SpriteRenderer>();
        if (nightSky == null) throw new InvalidOperationException("公寓入口夜空缺少精灵渲染器。");

        nightSky.sortingOrder = -12;
        EditorSceneManager.SaveScene(apartment);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Apartment Entrance Bus Stop Sign Artwork")]
    public static void ApplyApartmentEntranceBusStopSignArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(ApartmentEntranceBusStopSignArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到公寓入口公交站牌素材。");
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(ApartmentEntranceBusStopSignArtworkPath,
            ImportAssetOptions.ForceUpdate);

        Scene apartment = EditorSceneManager.OpenScene(ApartmentEntranceScenePath,
            OpenSceneMode.Single);
        ApplyApartmentEntranceBusStopSignArtwork(apartment);
        EditorSceneManager.SaveScene(apartment);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyApartmentEntranceBusStopSignArtwork(Scene apartment)
    {
        Sprite signArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(
            ApartmentEntranceBusStopSignArtworkPath);
        if (signArtwork == null) throw new InvalidOperationException("公寓入口公交站牌素材导入失败。");

        GameObject sign = FindInScene(apartment, "Apartment Bus Stop Sign");
        SpriteRenderer baseRenderer = sign.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("公交站牌缺少精灵渲染器。");

        Transform artwork = sign.transform.Find("Apartment Bus Stop Sign Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Apartment Bus Stop Sign Artwork").transform;
            artwork.SetParent(sign.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = sign.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            .6f / (signArtwork.bounds.size.x * parentScale.x),
            2.1f / (signArtwork.bounds.size.y * parentScale.y),
            1f);

        SpriteRenderer renderer = artwork.GetComponent<SpriteRenderer>();
        renderer.sprite = signArtwork;
        renderer.sharedMaterial = baseRenderer.sharedMaterial;
        renderer.color = Color.white;
        renderer.sortingLayerID = baseRenderer.sortingLayerID;
        renderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Apply Apartment Entrance Sidewalk Artwork")]
    public static void ApplyApartmentEntranceSidewalkArtwork()
    {
        ConfigureFloorTextureImporter(ApartmentEntranceUpperSidewalkTexturePath,
            "公寓入口上侧人行道贴图");
        ConfigureFloorTextureImporter(ApartmentEntranceLowerSidewalkTexturePath,
            "公寓入口下侧人行道贴图");

        Scene apartment = EditorSceneManager.OpenScene(ApartmentEntranceScenePath,
            OpenSceneMode.Single);
        ApplyApartmentEntranceSidewalkArtwork(apartment);
        EditorSceneManager.SaveScene(apartment);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyApartmentEntranceSidewalkArtwork(Scene apartment)
    {
        ApplyApartmentEntranceSidewalkVisual(apartment, "Apartment Sidewalk",
            "Apartment Entrance Upper Sidewalk Artwork", ApartmentEntranceUpperSidewalkTexturePath,
            18f, 1.8f);
        ApplyApartmentEntranceSidewalkVisual(apartment, "Apartment Opposite Sidewalk",
            "Apartment Entrance Lower Sidewalk Artwork", ApartmentEntranceLowerSidewalkTexturePath,
            18f, 1.1f);
    }

    private static void ApplyApartmentEntranceSidewalkVisual(Scene apartment,
        string sidewalkName, string artworkName, string artworkPath, float width, float height)
    {
        Sprite artworkSprite = AssetDatabase.LoadAssetAtPath<Sprite>(artworkPath);
        if (artworkSprite == null) throw new InvalidOperationException($"{sidewalkName} 贴图导入失败。");

        GameObject root = FindInScene(apartment, "Apartment Entrance");
        GameObject sidewalk = FindInScene(apartment, sidewalkName);
        SpriteRenderer baseRenderer = sidewalk.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException($"{sidewalkName} 缺少精灵渲染器。");

        GameObject artwork = FindOptionalInScene(apartment, artworkName);
        if (artwork == null)
        {
            artwork = new GameObject(artworkName);
            artwork.transform.SetParent(root.transform, false);
            artwork.AddComponent<SpriteRenderer>();
        }

        artwork.transform.position = sidewalk.transform.position;
        artwork.transform.localRotation = Quaternion.identity;
        artwork.transform.localScale = new Vector3(
            width / artworkSprite.bounds.size.x,
            height / artworkSprite.bounds.size.y,
            1f);

        SpriteRenderer renderer = artwork.GetComponent<SpriteRenderer>();
        renderer.sprite = artworkSprite;
        renderer.sharedMaterial = baseRenderer.sharedMaterial;
        renderer.color = Color.white;
        renderer.drawMode = SpriteDrawMode.Simple;
        renderer.sortingOrder = -11;
        baseRenderer.enabled = false;
    }

    private static void ConfigureFloorTextureImporter(string assetPath, string assetLabel)
    {
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer == null) throw new InvalidOperationException($"找不到{assetLabel}。");
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
    }

    [MenuItem("Tools/Devil RPG/Apply Apartment Entrance Building Facade Artwork")]
    public static void ApplyApartmentEntranceBuildingFacadeArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(ApartmentEntranceBuildingFacadeArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到公寓入口建筑立面素材。");
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(ApartmentEntranceBuildingFacadeArtworkPath,
            ImportAssetOptions.ForceUpdate);

        Scene apartment = EditorSceneManager.OpenScene(ApartmentEntranceScenePath,
            OpenSceneMode.Single);
        ApplyApartmentEntranceBuildingFacadeArtwork(apartment);
        EditorSceneManager.SaveScene(apartment);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyApartmentEntranceBuildingFacadeArtwork(Scene apartment)
    {
        Sprite facadeArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(
            ApartmentEntranceBuildingFacadeArtworkPath);
        if (facadeArtwork == null) throw new InvalidOperationException("公寓入口建筑立面素材导入失败。");

        GameObject building = FindInScene(apartment, "Apartment Building");
        SpriteRenderer baseRenderer = building.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("公寓建筑缺少精灵渲染器。");

        Transform artwork = building.transform.Find("Apartment Building Facade Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Apartment Building Facade Artwork").transform;
            artwork.SetParent(building.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = building.transform.lossyScale;
        artwork.position = new Vector3(120f, 3.1f, 0f);
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            14f / (facadeArtwork.bounds.size.x * parentScale.x),
            7.5f / (facadeArtwork.bounds.size.y * parentScale.y),
            1f);

        SpriteRenderer renderer = artwork.GetComponent<SpriteRenderer>();
        renderer.sprite = facadeArtwork;
        renderer.sharedMaterial = baseRenderer.sharedMaterial;
        renderer.color = Color.white;
        renderer.sortingLayerID = baseRenderer.sortingLayerID;
        renderer.sortingOrder = 0;
        baseRenderer.enabled = false;

        SpriteRenderer stepsRenderer = FindInScene(apartment, "Apartment Steps")
            .GetComponent<SpriteRenderer>();
        if (stepsRenderer != null) stepsRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Apply Apartment Entrance Door Artwork")]
    public static void ApplyApartmentEntranceDoorArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(ApartmentEntranceDoorArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到公寓入口大门素材。");
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(ApartmentEntranceDoorArtworkPath, ImportAssetOptions.ForceUpdate);
        ConfigureFloorTextureImporter(ApartmentEntranceDoorOpenArtworkPath,
            "公寓入口大门开启素材");

        Scene apartment = EditorSceneManager.OpenScene(ApartmentEntranceScenePath,
            OpenSceneMode.Single);
        ApplyApartmentEntranceDoorArtwork(apartment);
        EditorSceneManager.SaveScene(apartment);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyApartmentEntranceDoorArtwork(Scene apartment)
    {
        Sprite doorArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(ApartmentEntranceDoorArtworkPath);
        Sprite openDoorArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(ApartmentEntranceDoorOpenArtworkPath);
        if (doorArtwork == null || openDoorArtwork == null)
        {
            throw new InvalidOperationException("公寓入口大门状态素材导入失败。");
        }

        GameObject panel = FindInScene(apartment, "Apartment Entrance Door Panel");
        SpriteRenderer baseRenderer = panel.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("公寓入口门板缺少精灵渲染器。");

        Transform artwork = panel.transform.Find("Apartment Entrance Door Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Apartment Entrance Door Artwork").transform;
            artwork.SetParent(panel.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = panel.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1.5f / (doorArtwork.bounds.size.x * parentScale.x),
            2.2f / (doorArtwork.bounds.size.y * parentScale.y),
            1f);

        SpriteRenderer renderer = artwork.GetComponent<SpriteRenderer>();
        renderer.sprite = doorArtwork;
        renderer.sharedMaterial = baseRenderer.sharedMaterial;
        renderer.color = Color.white;
        renderer.sortingLayerID = baseRenderer.sortingLayerID;
        renderer.sortingOrder = 5;
        baseRenderer.enabled = false;

        DoorController door = FindInScene(apartment, "Apartment Entrance Door")
            .GetComponent<DoorController>();
        if (door == null) throw new InvalidOperationException("公寓入口门缺少门控制器。");
        DoorStateArtwork stateArtwork = artwork.GetComponent<DoorStateArtwork>();
        if (stateArtwork == null) stateArtwork = artwork.gameObject.AddComponent<DoorStateArtwork>();
        SetReference(stateArtwork, "door", door);
        SetReference(stateArtwork, "artworkRenderer", renderer);
        SetReference(stateArtwork, "closedSprite", doorArtwork);
        SetReference(stateArtwork, "openSprite", openDoorArtwork);
        renderer.sprite = doorArtwork;
    }

    [MenuItem("Tools/Devil RPG/Apply Apartment Bedroom Door Artwork")]
    public static void ApplyApartmentBedroomDoorArtwork()
    {
        ConfigureFloorTextureImporter(ApartmentBedroomDoorClosedArtworkPath, "公寓卧室门关闭素材");
        ConfigureFloorTextureImporter(ApartmentBedroomDoorOpenArtworkPath, "公寓卧室门开启素材");

        Sprite closedArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(ApartmentBedroomDoorClosedArtworkPath);
        Sprite openArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(ApartmentBedroomDoorOpenArtworkPath);
        if (closedArtwork == null || openArtwork == null)
        {
            throw new InvalidOperationException("公寓卧室门状态素材导入失败。");
        }

        ApplyDoorArtwork(ApartmentLivingRoomScenePath, "Protagonist Bedroom Door Panel",
            "Protagonist Bedroom Door Artwork", closedArtwork, 2.2f);
        ApplyDoorArtwork(ApartmentLivingRoomScenePath, "Lin Chunbao Bedroom Door Panel",
            "Lin Chunbao Bedroom Door Artwork", closedArtwork, 2.2f);
        ConfigureDoorStateArtwork(ApartmentLivingRoomScenePath, "Protagonist Bedroom Door",
            "Protagonist Bedroom Door Artwork", closedArtwork, openArtwork);
        ConfigureDoorStateArtwork(ApartmentLivingRoomScenePath, "Lin Chunbao Bedroom Door",
            "Lin Chunbao Bedroom Door Artwork", closedArtwork, openArtwork);

        ConfigureSpriteStateDoor(ApartmentLivingRoomScenePath, "Protagonist Bedroom Door");
        ConfigureSpriteStateDoor(ApartmentLivingRoomScenePath, "Lin Chunbao Bedroom Door");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Replace Apartment Exit Door With Doorway")]
    public static void ReplaceApartmentExitDoorWithDoorway()
    {
        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        Transform root = FindInScene(livingRoom, "Apartment Living Room").transform;
        RemoveSceneObject(livingRoom, "Apartment Exit Door");
        RemoveSceneObject(livingRoom, "Apartment Exit Door Portal");
        CreateAutomaticTransitionPoint(root, "Apartment Exit Doorway", new Vector2(140f, -5f),
            "ApartmentEntrance", "ApartmentEntranceFromLivingRoom");
        EditorSceneManager.SaveScene(livingRoom);
        ConfigureSpriteStateDoor(ApartmentLivingRoomScenePath, "Protagonist Bedroom Door");
        ConfigureSpriteStateDoor(ApartmentLivingRoomScenePath, "Lin Chunbao Bedroom Door");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Replace Apartment Bedroom Exit Doors With Doorways")]
    public static void ReplaceApartmentBedroomExitDoorsWithDoorways()
    {
        ReplaceBedroomExitDoorWithDoorway(ApartmentLinChunbaoBedroomScenePath,
            "Apartment Lin Chunbao Bedroom", "Lin Bedroom Exit Door", "Lin Bedroom Exit Doorway",
            new Vector2(200f, -2.5f), "ApartmentLivingRoom", "ApartmentLivingRoomFromLinBedroom");
        ReplaceBedroomExitDoorWithDoorway(ApartmentProtagonistBedroomScenePath,
            "Apartment Protagonist Bedroom", "Protagonist Bedroom Exit Door",
            "Protagonist Bedroom Exit Doorway", new Vector2(220f, -2.5f),
            "ApartmentLivingRoom", "ApartmentLivingRoomFromProtagonistBedroom");
        AssetDatabase.SaveAssets();
    }

    private static void ReplaceBedroomExitDoorWithDoorway(string scenePath, string rootName,
        string doorName, string doorwayName, Vector2 position, string destinationScene,
        string destinationId)
    {
        Scene bedroom = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        Transform root = FindInScene(bedroom, rootName).transform;
        RemoveSceneObject(bedroom, doorName);
        RemoveSceneObject(bedroom, doorName + " Portal");
        CreateAutomaticTransitionPoint(root, doorwayName, position, destinationScene, destinationId);
        EditorSceneManager.SaveScene(bedroom);
    }

    [MenuItem("Tools/Devil RPG/Apply Living Room Upper Wall Artwork")]
    public static void ApplyLivingRoomUpperWallArtwork()
    {
        ConfigureFloorTextureImporter(LivingRoomUpperWallArtworkPath, "客厅上部墙壁素材");
        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath,
            OpenSceneMode.Single);
        ApplyLivingRoomUpperWallArtwork(livingRoom);
        EditorSceneManager.SaveScene(livingRoom);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyLivingRoomUpperWallArtwork(Scene livingRoom)
    {
        Sprite wallArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomUpperWallArtworkPath);
        if (wallArtwork == null) throw new InvalidOperationException("客厅上部墙壁素材导入失败。");

        ApplyLivingRoomUpperWallVisual(FindInScene(livingRoom, "Living Room Wall Top Left"),
            wallArtwork, Vector2.zero + new Vector2(0f, .49f),
            new Vector2(.046040516f, 1.0048409f));
        ApplyLivingRoomUpperWallVisual(FindInScene(livingRoom, "Living Room Wall Top Center"),
            wallArtwork, new Vector2(.00128f, .403f),
            new Vector2(.04709606f, .9810679f));
        ApplyLivingRoomUpperWallVisual(FindInScene(livingRoom, "Living Room Wall Top Right"),
            wallArtwork, new Vector2(0f, .351f),
            new Vector2(.046040516f, .9665723f));
    }

    private static void ApplyLivingRoomUpperWallVisual(GameObject wall, Sprite wallArtwork,
        Vector2 localPosition, Vector2 localScale)
    {
        SpriteRenderer baseRenderer = wall.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(wall.name + " 缺少精灵渲染器。");

        Transform artwork = wall.transform.Find("Living Room Upper Wall Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Living Room Upper Wall Artwork").transform;
            artwork.SetParent(wall.transform, false);
        }

        artwork.localPosition = localPosition;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(localScale.x, localScale.y, 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = wallArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = 3;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Apply Living Room Side And Bottom Wall Artwork")]
    public static void ApplyLivingRoomSideAndBottomWallArtwork()
    {
        ConfigureFloorTextureImporter(LivingRoomLeftWallArtworkPath, "客厅左侧墙壁素材");
        ConfigureFloorTextureImporter(LivingRoomRightWallArtworkPath, "客厅右侧墙壁素材");
        ConfigureFloorTextureImporter(LivingRoomBottomWallArtworkPath, "客厅底边墙壁素材");

        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath,
            OpenSceneMode.Single);
        ApplyLivingRoomSideAndBottomWallArtwork(livingRoom);
        EditorSceneManager.SaveScene(livingRoom);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyLivingRoomSideAndBottomWallArtwork(Scene livingRoom)
    {
        Sprite leftWallArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomLeftWallArtworkPath);
        Sprite rightWallArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomRightWallArtworkPath);
        Sprite bottomWallArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomBottomWallArtworkPath);
        if (leftWallArtwork == null || rightWallArtwork == null || bottomWallArtwork == null)
        {
            throw new InvalidOperationException("客厅侧边或底边墙壁素材导入失败。");
        }

        ApplyLivingRoomWallVisual(FindInScene(livingRoom, "Living Room Wall Left"),
            "Living Room Left Wall Artwork", leftWallArtwork);
        ApplyLivingRoomWallVisual(FindInScene(livingRoom, "Living Room Wall Right Top"),
            "Living Room Right Wall Artwork", rightWallArtwork);
        ApplyLivingRoomWallVisual(FindInScene(livingRoom, "Living Room Wall Right Middle"),
            "Living Room Right Wall Artwork", rightWallArtwork);
        ApplyLivingRoomWallVisual(FindInScene(livingRoom, "Living Room Wall Right Bottom"),
            "Living Room Right Wall Artwork", rightWallArtwork);
        ApplyLivingRoomWallVisual(FindInScene(livingRoom, "Living Room Wall Bottom Left"),
            "Living Room Bottom Wall Artwork", bottomWallArtwork);
        ApplyLivingRoomWallVisual(FindInScene(livingRoom, "Living Room Wall Bottom Right"),
            "Living Room Bottom Wall Artwork", bottomWallArtwork);
    }

    private static void ApplyLivingRoomWallVisual(GameObject wall, string artworkName,
        Sprite wallArtwork)
    {
        SpriteRenderer baseRenderer = wall.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(wall.name + " 缺少精灵渲染器。");

        Transform artwork = wall.transform.Find(artworkName);
        if (artwork == null)
        {
            artwork = new GameObject(artworkName).transform;
            artwork.SetParent(wall.transform, false);
        }

        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1f / wallArtwork.bounds.size.x,
            1f / wallArtwork.bounds.size.y, 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = wallArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = 3;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Apply Apartment Room Wall Artwork")]
    public static void ApplyApartmentRoomWallArtwork()
    {
        ConfigureFloorTextureImporter(LivingRoomUpperWallArtworkPath, "公寓上部墙壁素材");
        ConfigureFloorTextureImporter(LivingRoomLeftWallArtworkPath, "公寓左侧墙壁素材");
        ConfigureFloorTextureImporter(LivingRoomRightWallArtworkPath, "公寓右侧墙壁素材");
        ConfigureFloorTextureImporter(LivingRoomBottomWallArtworkPath, "公寓底边墙壁素材");

        ApplyApartmentRoomWallArtwork(ApartmentKitchenScenePath,
            new[] { "Kitchen Wall Top" },
            new[] { "Kitchen Wall Left Top", "Kitchen Wall Left Bottom" },
            new[] { "Kitchen Wall Right" },
            new[] { "Kitchen Wall Bottom" });
        ApplyApartmentRoomWallArtwork(ApartmentBathroomScenePath,
            new[] { "Bathroom Wall Top" },
            new[] { "Bathroom Wall Left Top", "Bathroom Wall Left Bottom" },
            new[] { "Bathroom Wall Right" },
            new[] { "Bathroom Wall Bottom" });
        ApplyApartmentRoomWallArtwork(ApartmentLinChunbaoBedroomScenePath,
            new[] { "Lin Bedroom Wall Top" },
            new[] { "Lin Bedroom Wall Left" },
            new[] { "Lin Bedroom Wall Right" },
            new[] { "Lin Bedroom Wall Bottom Left", "Lin Bedroom Wall Bottom Right" });
        ApplyApartmentRoomWallArtwork(ApartmentProtagonistBedroomScenePath,
            new[] { "Protagonist Bedroom Wall Top" },
            new[] { "Protagonist Bedroom Wall Left" },
            new[] { "Protagonist Bedroom Wall Right" },
            new[] { "Protagonist Bedroom Wall Bottom Left", "Protagonist Bedroom Wall Bottom Right" });
        AssetDatabase.SaveAssets();
    }

    private static void ApplyApartmentRoomWallArtwork(string scenePath, string[] topWalls,
        string[] leftWalls, string[] rightWalls, string[] bottomWalls)
    {
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        ApplyApartmentRoomWallArtwork(scene, topWalls, leftWalls, rightWalls, bottomWalls);
        EditorSceneManager.SaveScene(scene);
    }

    private static void ApplyApartmentRoomWallArtwork(Scene scene, string[] topWalls,
        string[] leftWalls, string[] rightWalls, string[] bottomWalls)
    {
        Sprite topArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomUpperWallArtworkPath);
        Sprite leftArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomLeftWallArtworkPath);
        Sprite rightArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomRightWallArtworkPath);
        Sprite bottomArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomBottomWallArtworkPath);
        if (topArtwork == null || leftArtwork == null || rightArtwork == null || bottomArtwork == null)
        {
            throw new InvalidOperationException("公寓墙壁美术素材导入失败。");
        }

        foreach (string wallName in topWalls)
        {
            ApplyApartmentRoomWallVisual(FindInScene(scene, wallName), topArtwork);
        }
        foreach (string wallName in leftWalls)
        {
            ApplyApartmentRoomWallVisual(FindInScene(scene, wallName), leftArtwork);
        }
        foreach (string wallName in rightWalls)
        {
            ApplyApartmentRoomWallVisual(FindInScene(scene, wallName), rightArtwork);
        }
        foreach (string wallName in bottomWalls)
        {
            ApplyApartmentRoomWallVisual(FindInScene(scene, wallName), bottomArtwork);
        }
    }

    private static void ApplyApartmentRoomWallVisual(GameObject wall, Sprite wallArtwork)
    {
        SpriteRenderer baseRenderer = wall.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(wall.name + " 缺少精灵渲染器。");

        Transform artwork = wall.transform.Find(wall.name + " Artwork");
        if (artwork == null)
        {
            artwork = new GameObject(wall.name + " Artwork").transform;
            artwork.SetParent(wall.transform, false);
        }

        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1f / wallArtwork.bounds.size.x,
            1f / wallArtwork.bounds.size.y, 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = wallArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = 1;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Apply Living Room Sofa Artwork")]
    public static void ApplyLivingRoomSofaArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(LivingRoomSofaArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到客厅沙发美术素材。");

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(LivingRoomSofaArtworkPath, ImportAssetOptions.ForceUpdate);

        Sprite sofaArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomSofaArtworkPath);
        if (sofaArtwork == null) throw new InvalidOperationException("客厅沙发美术素材导入失败。");

        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath,
            OpenSceneMode.Single);
        GameObject sofa = FindInScene(livingRoom, "Living Room Sofa");
        SpriteRenderer baseRenderer = sofa.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("客厅沙发缺少精灵渲染器。");

        Transform artwork = sofa.transform.Find("Living Room Sofa Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Living Room Sofa Artwork").transform;
            artwork.SetParent(sofa.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artwork.localPosition = new Vector3(-.002f, -.03f, 0f);
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(.07250853f, .1605254f, 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = sofaArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(livingRoom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Living Room Coffee Table Artwork")]
    public static void ApplyLivingRoomCoffeeTableArtwork()
    {
        ConfigureFloorTextureImporter(LivingRoomCoffeeTableArtworkPath, "客厅茶几美术素材");

        Sprite tableArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomCoffeeTableArtworkPath);
        if (tableArtwork == null) throw new InvalidOperationException("客厅茶几美术素材导入失败。");

        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath,
            OpenSceneMode.Single);
        ApplyLivingRoomCoffeeTableArtwork(livingRoom, tableArtwork);
        EditorSceneManager.SaveScene(livingRoom);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyLivingRoomCoffeeTableArtwork(Scene livingRoom, Sprite tableArtwork)
    {
        GameObject table = FindInScene(livingRoom, "Living Room Coffee Table");
        SpriteRenderer baseRenderer = table.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("客厅茶几缺少精灵渲染器。");

        Transform artwork = table.transform.Find("Living Room Coffee Table Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Living Room Coffee Table Artwork").transform;
            artwork.SetParent(table.transform, false);
        }

        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1f / tableArtwork.bounds.size.x,
            1f / tableArtwork.bounds.size.y, 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = tableArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Apply Living Room Low Cabinet Artwork")]
    public static void ApplyLivingRoomLowCabinetArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(LivingRoomLowCabinetArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到客厅矮柜美术素材。");

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(LivingRoomLowCabinetArtworkPath, ImportAssetOptions.ForceUpdate);
        ConfigureFloorTextureImporter(LivingRoomLowCabinetOpenArtworkPath,
            "客厅矮柜开启美术素材");

        Sprite cabinetArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomLowCabinetArtworkPath);
        Sprite openCabinetArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomLowCabinetOpenArtworkPath);
        if (cabinetArtwork == null || openCabinetArtwork == null)
        {
            throw new InvalidOperationException("客厅矮柜状态美术素材导入失败。");
        }

        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath,
            OpenSceneMode.Single);
        GameObject cabinet = FindInScene(livingRoom, "Living Room Low Cabinet");
        SpriteRenderer baseRenderer = cabinet.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("客厅矮柜缺少精灵渲染器。");

        Transform artwork = cabinet.transform.Find("Living Room Low Cabinet Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Living Room Low Cabinet Artwork").transform;
            artwork.SetParent(cabinet.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artwork.localPosition = new Vector3(-.02f, -.92f, 0f);
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(.07370913f, .21776775f, 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = cabinetArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;

        LockedGloveCabinet gloveCabinet = cabinet.GetComponent<LockedGloveCabinet>();
        if (gloveCabinet == null) throw new InvalidOperationException("客厅矮柜缺少解谜组件。");
        SetReference(gloveCabinet, "cabinetArtworkRenderer", artworkRenderer);
        SetReference(gloveCabinet, "openArtwork", openCabinetArtwork);

        EditorSceneManager.SaveScene(livingRoom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Kitchen Stove Counter Artwork")]
    public static void ApplyKitchenStoveCounterArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(KitchenStoveCounterArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到厨房灶台桌美术素材。");

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(KitchenStoveCounterArtworkPath, ImportAssetOptions.ForceUpdate);

        Sprite stoveArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(KitchenStoveCounterArtworkPath);
        if (stoveArtwork == null) throw new InvalidOperationException("厨房灶台桌美术素材导入失败。");

        Scene kitchen = EditorSceneManager.OpenScene(ApartmentKitchenScenePath,
            OpenSceneMode.Single);
        GameObject stove = FindInScene(kitchen, "Kitchen Stove");
        SpriteRenderer baseRenderer = stove.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("厨房灶台缺少精灵渲染器。");

        Transform artwork = stove.transform.Find("Kitchen Stove Counter Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Kitchen Stove Counter Artwork").transform;
            artwork.SetParent(stove.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = stove.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            2.3f / (stoveArtwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            .8f / (stoveArtwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = stoveArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(kitchen);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Kitchen Sink Counter Artwork")]
    public static void ApplyKitchenSinkCounterArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(KitchenSinkCounterArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到厨房水池柜美术素材。");

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(KitchenSinkCounterArtworkPath, ImportAssetOptions.ForceUpdate);

        Sprite sinkArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(KitchenSinkCounterArtworkPath);
        if (sinkArtwork == null) throw new InvalidOperationException("厨房水池柜美术素材导入失败。");

        Scene kitchen = EditorSceneManager.OpenScene(ApartmentKitchenScenePath,
            OpenSceneMode.Single);
        GameObject sink = FindInScene(kitchen, "Kitchen Sink");
        SpriteRenderer baseRenderer = sink.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("厨房水池缺少精灵渲染器。");

        Transform artwork = sink.transform.Find("Kitchen Sink Counter Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Kitchen Sink Counter Artwork").transform;
            artwork.SetParent(sink.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = sink.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1.6f / (sinkArtwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            .8f / (sinkArtwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = sinkArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(kitchen);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Kitchen Refrigerator Artwork")]
    public static void ApplyKitchenRefrigeratorArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(KitchenRefrigeratorArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到厨房冰箱美术素材。");

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(KitchenRefrigeratorArtworkPath, ImportAssetOptions.ForceUpdate);

        Sprite refrigeratorArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(KitchenRefrigeratorArtworkPath);
        if (refrigeratorArtwork == null) throw new InvalidOperationException("厨房冰箱美术素材导入失败。");

        Scene kitchen = EditorSceneManager.OpenScene(ApartmentKitchenScenePath,
            OpenSceneMode.Single);
        GameObject refrigerator = FindInScene(kitchen, "Kitchen Refrigerator");
        SpriteRenderer baseRenderer = refrigerator.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("厨房冰箱缺少精灵渲染器。");

        Transform artwork = refrigerator.transform.Find("Kitchen Refrigerator Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Kitchen Refrigerator Artwork").transform;
            artwork.SetParent(refrigerator.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = refrigerator.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1.1f / (refrigeratorArtwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            2f / (refrigeratorArtwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = refrigeratorArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(kitchen);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Kitchen Dining Table Artwork")]
    public static void ApplyKitchenDiningTableArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(KitchenDiningTableArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到厨房餐桌美术素材。");

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(KitchenDiningTableArtworkPath, ImportAssetOptions.ForceUpdate);

        Sprite tableArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(KitchenDiningTableArtworkPath);
        if (tableArtwork == null) throw new InvalidOperationException("厨房餐桌美术素材导入失败。");

        Scene kitchen = EditorSceneManager.OpenScene(ApartmentKitchenScenePath,
            OpenSceneMode.Single);
        GameObject table = FindInScene(kitchen, "Kitchen Dining Table");
        SpriteRenderer baseRenderer = table.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("厨房餐桌缺少精灵渲染器。");

        Transform artwork = table.transform.Find("Kitchen Dining Table Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Kitchen Dining Table Artwork").transform;
            artwork.SetParent(table.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = table.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            2.1f / (tableArtwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            1f / (tableArtwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = tableArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(kitchen);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Locked Food Cabinet Artwork")]
    public static void ApplyLockedFoodCabinetArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(LockedFoodCabinetArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到上锁食品柜美术素材。");

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(LockedFoodCabinetArtworkPath, ImportAssetOptions.ForceUpdate);

        Sprite cabinetArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LockedFoodCabinetArtworkPath);
        if (cabinetArtwork == null) throw new InvalidOperationException("上锁食品柜美术素材导入失败。");

        Scene kitchen = EditorSceneManager.OpenScene(ApartmentKitchenScenePath,
            OpenSceneMode.Single);
        GameObject cabinet = FindInScene(kitchen, "Locked Food Cabinet");
        SpriteRenderer baseRenderer = cabinet.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("食品柜缺少精灵渲染器。");

        Transform artwork = cabinet.transform.Find("Locked Food Cabinet Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Locked Food Cabinet Artwork").transform;
            artwork.SetParent(cabinet.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = cabinet.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1.5f / (cabinetArtwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            1.3f / (cabinetArtwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = cabinetArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(kitchen);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Bathroom Sink Medicine Cabinet Artwork")]
    public static void ApplyBathroomSinkMedicineCabinetArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(BathroomSinkMedicineCabinetArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到洗手间水池药柜美术素材。");

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(BathroomSinkMedicineCabinetArtworkPath,
            ImportAssetOptions.ForceUpdate);

        Sprite fixtureArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(BathroomSinkMedicineCabinetArtworkPath);
        if (fixtureArtwork == null) throw new InvalidOperationException("洗手间水池药柜美术素材导入失败。");

        Scene bathroom = EditorSceneManager.OpenScene(ApartmentBathroomScenePath,
            OpenSceneMode.Single);
        GameObject sink = FindInScene(bathroom, "Bathroom Sink");
        SpriteRenderer baseRenderer = sink.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("洗手间水池缺少精灵渲染器。");

        Transform artwork = sink.transform.Find("Bathroom Sink Medicine Cabinet Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Bathroom Sink Medicine Cabinet Artwork").transform;
            artwork.SetParent(sink.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = sink.transform.lossyScale;
        artwork.localPosition = new Vector3(0f, .35f, 0f);
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1.75f / (fixtureArtwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            1.8f / (fixtureArtwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = fixtureArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = 3;
        baseRenderer.enabled = false;

        SpriteRenderer medicineCabinetRenderer = FindInScene(bathroom, "Bathroom Medicine Cabinet")
            .GetComponent<SpriteRenderer>();
        if (medicineCabinetRenderer != null) medicineCabinetRenderer.enabled = false;

        EditorSceneManager.SaveScene(bathroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Bathroom Toilet Artwork")]
    public static void ApplyBathroomToiletArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(BathroomToiletArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到洗手间马桶美术素材。");

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(BathroomToiletArtworkPath, ImportAssetOptions.ForceUpdate);

        Sprite toiletArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(BathroomToiletArtworkPath);
        if (toiletArtwork == null) throw new InvalidOperationException("洗手间马桶美术素材导入失败。");

        Scene bathroom = EditorSceneManager.OpenScene(ApartmentBathroomScenePath,
            OpenSceneMode.Single);
        GameObject toilet = FindInScene(bathroom, "Bathroom Toilet");
        SpriteRenderer baseRenderer = toilet.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("洗手间马桶缺少精灵渲染器。");

        Transform artwork = toilet.transform.Find("Bathroom Toilet Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Bathroom Toilet Artwork").transform;
            artwork.SetParent(toilet.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = toilet.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            1.4f / (toiletArtwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            1f / (toiletArtwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = toiletArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(bathroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Bathroom Vent Artwork")]
    public static void ApplyBathroomVentArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(BathroomVentArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到洗手间通风口美术素材。");

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(BathroomVentArtworkPath, ImportAssetOptions.ForceUpdate);

        Sprite ventArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(BathroomVentArtworkPath);
        if (ventArtwork == null) throw new InvalidOperationException("洗手间通风口美术素材导入失败。");

        Scene bathroom = EditorSceneManager.OpenScene(ApartmentBathroomScenePath,
            OpenSceneMode.Single);
        GameObject vent = FindInScene(bathroom, "Bathroom Vent");
        SpriteRenderer baseRenderer = vent.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("洗手间通风口缺少精灵渲染器。");

        Transform artwork = vent.transform.Find("Bathroom Vent Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Bathroom Vent Artwork").transform;
            artwork.SetParent(vent.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = vent.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            .75f / (ventArtwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            .35f / (ventArtwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = ventArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(bathroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Lin Bedroom Desk Chair Artwork")]
    public static void ApplyLinBedroomDeskChairArtwork()
    {
        ConfigureFloorTextureImporter(LinBedroomDeskArtworkPath, "林春宝书桌美术素材");
        ConfigureFloorTextureImporter(LinBedroomChairArtworkPath, "林春宝椅子美术素材");

        Sprite deskArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LinBedroomDeskArtworkPath);
        Sprite chairArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LinBedroomChairArtworkPath);
        if (deskArtwork == null || chairArtwork == null)
        {
            throw new InvalidOperationException("林春宝书桌或椅子美术素材导入失败。");
        }

        Scene bedroom = EditorSceneManager.OpenScene(ApartmentLinChunbaoBedroomScenePath,
            OpenSceneMode.Single);
        ApplyLinBedroomFurnitureArtwork(FindInScene(bedroom, "Lin Bedroom Desk"),
            "Lin Bedroom Desk Artwork", deskArtwork, new Vector2(1.45f, .72f));
        ApplyLinBedroomFurnitureArtwork(FindInScene(bedroom, "Lin Bedroom Chair"),
            "Lin Bedroom Chair Artwork", chairArtwork, new Vector2(.7f, .65f));

        EditorSceneManager.SaveScene(bedroom);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyLinBedroomFurnitureArtwork(GameObject furniture, string artworkName,
        Sprite artworkSprite, Vector2 worldSize)
    {
        SpriteRenderer baseRenderer = furniture.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(furniture.name + " 缺少精灵渲染器。");

        Transform artwork = furniture.transform.Find(artworkName);
        if (artwork == null)
        {
            artwork = new GameObject(artworkName).transform;
            artwork.SetParent(furniture.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = furniture.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            worldSize.x / (artworkSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            worldSize.y / (artworkSprite.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Apply Lin Bedroom Bed Artwork")]
    public static void ApplyLinBedroomBedArtwork()
    {
        ConfigureFloorTextureImporter(LinBedroomBedArtworkPath, "林春宝床铺美术素材");
        Sprite bedArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LinBedroomBedArtworkPath);
        if (bedArtwork == null) throw new InvalidOperationException("林春宝床铺美术素材导入失败。");

        Scene bedroom = EditorSceneManager.OpenScene(ApartmentLinChunbaoBedroomScenePath,
            OpenSceneMode.Single);
        ApplyLinBedroomFurnitureArtwork(FindInScene(bedroom, "Lin Bedroom Bed"),
            "Lin Bedroom Bed Artwork", bedArtwork, new Vector2(2.45f, 1.25f));

        EditorSceneManager.SaveScene(bedroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Lin Bedroom Wardrobe Artwork")]
    public static void ApplyLinBedroomWardrobeArtwork()
    {
        ConfigureFloorTextureImporter(LinBedroomWardrobeArtworkPath, "林春宝衣柜美术素材");
        Sprite wardrobeArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LinBedroomWardrobeArtworkPath);
        if (wardrobeArtwork == null) throw new InvalidOperationException("林春宝衣柜美术素材导入失败。");

        Scene bedroom = EditorSceneManager.OpenScene(ApartmentLinChunbaoBedroomScenePath,
            OpenSceneMode.Single);
        ApplyLinBedroomFurnitureArtwork(FindInScene(bedroom, "Lin Bedroom Wardrobe"),
            "Lin Bedroom Wardrobe Artwork", wardrobeArtwork, new Vector2(1.45f, 1.95f));

        EditorSceneManager.SaveScene(bedroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Protagonist Bedroom Bed Artwork")]
    public static void ApplyProtagonistBedroomBedArtwork()
    {
        ConfigureFloorTextureImporter(ProtagonistBedroomBedArtworkPath, "主角房间床铺美术素材");
        Sprite bedArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(ProtagonistBedroomBedArtworkPath);
        if (bedArtwork == null) throw new InvalidOperationException("主角房间床铺美术素材导入失败。");

        Scene bedroom = EditorSceneManager.OpenScene(ApartmentProtagonistBedroomScenePath,
            OpenSceneMode.Single);
        ApplyLinBedroomFurnitureArtwork(FindInScene(bedroom, "Protagonist Bedroom Bed"),
            "Protagonist Bedroom Bed Artwork", bedArtwork, new Vector2(2.55f, 1.35f));

        EditorSceneManager.SaveScene(bedroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Protagonist Plaster Boxes Artwork")]
    public static void ApplyProtagonistPlasterBoxesArtwork()
    {
        ConfigureFloorTextureImporter(ProtagonistPlasterBoxesArtworkPath, "主角房间膏药盒美术素材");
        Sprite boxesArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(ProtagonistPlasterBoxesArtworkPath);
        if (boxesArtwork == null) throw new InvalidOperationException("主角房间膏药盒美术素材导入失败。");

        Scene bedroom = EditorSceneManager.OpenScene(ApartmentProtagonistBedroomScenePath,
            OpenSceneMode.Single);
        RemoveSceneObject(bedroom, "Plaster Box Top");
        RemoveSceneObject(bedroom, "Plaster Box Middle");
        ApplyLinBedroomFurnitureArtwork(FindInScene(bedroom, "Stacked Plaster Boxes"),
            "Protagonist Plaster Boxes Artwork", boxesArtwork, new Vector2(1.1f, 1.1f));

        EditorSceneManager.SaveScene(bedroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Protagonist Bedside Table Artwork")]
    public static void ApplyProtagonistBedsideTableArtwork()
    {
        ConfigureFloorTextureImporter(ProtagonistBedsideTableClosedArtworkPath, "主角床头柜关闭美术素材");
        ConfigureFloorTextureImporter(ProtagonistBedsideTableOpenArtworkPath, "主角床头柜打开美术素材");
        Sprite closedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(ProtagonistBedsideTableClosedArtworkPath);
        Sprite openSprite = AssetDatabase.LoadAssetAtPath<Sprite>(ProtagonistBedsideTableOpenArtworkPath);
        if (closedSprite == null || openSprite == null)
        {
            throw new InvalidOperationException("主角床头柜美术素材导入失败。");
        }

        Scene bedroom = EditorSceneManager.OpenScene(ApartmentProtagonistBedroomScenePath,
            OpenSceneMode.Single);
        GameObject table = FindInScene(bedroom, "Protagonist Bedroom Bedside Table");
        GameObject closedArtwork = CreateProtagonistBedsideArtwork(table, "Protagonist Bedside Table Closed Artwork",
            closedSprite, new Vector2(.82f, .82f));
        GameObject openArtwork = CreateProtagonistBedsideArtwork(table, "Protagonist Bedside Table Open Artwork",
            openSprite, new Vector2(.82f, .98f));
        openArtwork.SetActive(false);

        Transform drawer = table.transform.Find("Protagonist Bedside Table Open Drawer");
        if (drawer != null)
        {
            foreach (Renderer renderer in drawer.GetComponentsInChildren<Renderer>(true)) renderer.enabled = false;
        }

        ProtagonistBedsideTable bedsidePuzzle = table.GetComponent<ProtagonistBedsideTable>();
        if (bedsidePuzzle == null)
        {
            bedsidePuzzle = table.AddComponent<ProtagonistBedsideTable>();
            if (drawer != null) bedsidePuzzle.Configure(drawer.gameObject);
            InvestigationPoint investigation = FindInScene(bedroom, "Protagonist Bedside Table Investigation")
                .GetComponent<InvestigationPoint>();
            if (investigation == null) throw new InvalidOperationException("主角床头柜缺少调查点组件。");
            SetReference(investigation, "handler", bedsidePuzzle);
        }
        bedsidePuzzle.ConfigureArtwork(closedArtwork, openArtwork);

        SpriteRenderer baseRenderer = table.GetComponent<SpriteRenderer>();
        if (baseRenderer != null) baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(bedroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Rubble Iron Gate Artwork")]
    public static void ApplyRubbleIronGateArtwork()
    {
        ConfigureFloorTextureImporter(RubbleIronGateClosedArtworkPath, "瓦砾堆铁门关闭美术素材");
        ConfigureFloorTextureImporter(RubbleIronGateOpenArtworkPath, "瓦砾堆铁门打开美术素材");
        Sprite closedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(RubbleIronGateClosedArtworkPath);
        Sprite openSprite = AssetDatabase.LoadAssetAtPath<Sprite>(RubbleIronGateOpenArtworkPath);
        if (closedSprite == null || openSprite == null)
        {
            throw new InvalidOperationException("瓦砾堆铁门美术素材导入失败。");
        }

        Scene rubble = EditorSceneManager.OpenScene(RubblePileScenePath, OpenSceneMode.Single);
        GameObject gate = FindInScene(rubble, "锈蚀铁门");
        SpriteRenderer baseRenderer = gate.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("锈蚀铁门缺少精灵渲染器。");
        GameObject closedArtwork = CreateRubbleIronGateArtwork(gate, "Rubble Iron Gate Closed Artwork",
            closedSprite, new Vector2(2.1f, 2.25f), baseRenderer);
        GameObject openArtwork = CreateRubbleIronGateArtwork(gate, "Rubble Iron Gate Open Artwork",
            openSprite, new Vector2(2.1f, 2.25f), baseRenderer);
        RubbleSwitchPuzzle puzzle = FindInScene(rubble, "瓦砾堆").GetComponent<RubbleSwitchPuzzle>();
        if (puzzle == null) throw new InvalidOperationException("瓦砾堆缺少开关解谜组件。");
        puzzle.ConfigureArtwork(closedArtwork, openArtwork);
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(rubble);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Create Rubble Switch Prefab")]
    public static void CreateRubbleSwitchPrefab()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        Scene rubble = EditorSceneManager.OpenScene(RubblePileScenePath, OpenSceneMode.Single);
        string[] switchNames =
        {
            "瓦砾开关 左上", "瓦砾开关 右上", "瓦砾开关 左下", "瓦砾开关 右下"
        };
        GameObject source = FindInScene(rubble, switchNames[0]);
        SpriteRenderer sourceRenderer = source.GetComponent<SpriteRenderer>();
        if (sourceRenderer == null) throw new InvalidOperationException("瓦砾开关缺少精灵渲染器。");
        GameObject template = new GameObject("Rubble Switch");
        SpriteRenderer templateRenderer = template.AddComponent<SpriteRenderer>();
        templateRenderer.sprite = sourceRenderer.sprite;
        templateRenderer.sharedMaterial = sourceRenderer.sharedMaterial;
        templateRenderer.color = sourceRenderer.color;
        templateRenderer.sortingLayerID = sourceRenderer.sortingLayerID;
        templateRenderer.sortingOrder = sourceRenderer.sortingOrder;
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(template, RubbleSwitchPrefabPath);
        UnityEngine.Object.DestroyImmediate(template);
        if (prefab == null) throw new InvalidOperationException("无法创建瓦砾开关预制体。");

        RubbleSwitch[] switches = new RubbleSwitch[switchNames.Length];
        for (int index = 0; index < switchNames.Length; index++)
        {
            GameObject oldSwitch = FindInScene(rubble, switchNames[index]);
            Transform oldTransform = oldSwitch.transform;
            Transform parent = oldTransform.parent;
            Vector3 localPosition = oldTransform.localPosition;
            Quaternion localRotation = oldTransform.localRotation;
            Vector3 localScale = oldTransform.localScale;
            int siblingIndex = oldTransform.GetSiblingIndex();

            GameObject replacement = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            replacement.name = switchNames[index];
            replacement.transform.localPosition = localPosition;
            replacement.transform.localRotation = localRotation;
            replacement.transform.localScale = localScale;
            replacement.transform.SetSiblingIndex(siblingIndex);
            UnityEngine.Object.DestroyImmediate(oldSwitch);
            RubbleSwitch behavior = replacement.AddComponent<RubbleSwitch>();
            behavior.Configure(null, replacement.GetComponent<SpriteRenderer>());
            switches[index] = behavior;
        }

        GameObject root = FindInScene(rubble, "瓦砾堆");
        RubbleSwitchPuzzle puzzle = root.GetComponent<RubbleSwitchPuzzle>();
        if (puzzle == null) throw new InvalidOperationException("瓦砾堆缺少开关解谜组件。");
        GameObject gate = FindInScene(rubble, "锈蚀铁门");
        puzzle.Configure(gate.GetComponent<Collider2D>(), gate.GetComponent<SpriteRenderer>(), switches);
        for (int index = 0; index < switchNames.Length; index++)
        {
            InvestigationPoint point = FindInScene(rubble, switchNames[index] + " Investigation")
                .GetComponent<InvestigationPoint>();
            if (point == null) throw new InvalidOperationException(switchNames[index] + " 缺少调查点组件。");
            SetReference(point, "handler", switches[index]);
        }

        EditorSceneManager.SaveScene(rubble);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Rubble Switch Artwork")]
    public static void ApplyRubbleSwitchArtwork()
    {
        ConfigureFloorTextureImporter(RubbleSwitchIdleArtworkPath, "瓦砾开关常态美术素材");
        ConfigureFloorTextureImporter(RubbleSwitchPressedArtworkPath, "瓦砾开关按下美术素材");
        Sprite idleSprite = AssetDatabase.LoadAssetAtPath<Sprite>(RubbleSwitchIdleArtworkPath);
        Sprite pressedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(RubbleSwitchPressedArtworkPath);
        if (idleSprite == null || pressedSprite == null)
        {
            throw new InvalidOperationException("瓦砾开关美术素材导入失败。");
        }

        GameObject prefab = PrefabUtility.LoadPrefabContents(RubbleSwitchPrefabPath);
        try
        {
            SpriteRenderer baseRenderer = prefab.GetComponent<SpriteRenderer>();
            if (baseRenderer == null) throw new InvalidOperationException("瓦砾开关预制体缺少精灵渲染器。");
            CreateRubbleSwitchArtwork(prefab, "Rubble Switch Idle Artwork", idleSprite, baseRenderer, true);
            CreateRubbleSwitchArtwork(prefab, "Rubble Switch Pressed Artwork", pressedSprite, baseRenderer, false);
            baseRenderer.enabled = false;
            PrefabUtility.SaveAsPrefabAsset(prefab, RubbleSwitchPrefabPath);
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefab);
        }

        Scene rubble = EditorSceneManager.OpenScene(RubblePileScenePath, OpenSceneMode.Single);
        string[] switchNames = { "瓦砾开关 左上", "瓦砾开关 右上", "瓦砾开关 左下", "瓦砾开关 右下" };
        RubbleSwitch[] switches = new RubbleSwitch[switchNames.Length];
        for (int index = 0; index < switchNames.Length; index++)
        {
            GameObject switchObject = FindInScene(rubble, switchNames[index]);
            RubbleSwitch switchBehavior = switchObject.GetComponent<RubbleSwitch>();
            if (switchBehavior == null)
            {
                switchBehavior = switchObject.AddComponent<RubbleSwitch>();
                switchBehavior.Configure(null, switchObject.GetComponent<SpriteRenderer>());
            }
            switchBehavior.ConfigureArtwork(
                switchObject.transform.Find("Rubble Switch Idle Artwork").gameObject,
                switchObject.transform.Find("Rubble Switch Pressed Artwork").gameObject);
            switches[index] = switchBehavior;
        }

        GameObject root = FindInScene(rubble, "瓦砾堆");
        RubbleSwitchPuzzle puzzle = root.GetComponent<RubbleSwitchPuzzle>();
        if (puzzle == null) throw new InvalidOperationException("瓦砾堆缺少开关解谜组件。");
        GameObject gate = FindInScene(rubble, "锈蚀铁门");
        puzzle.Configure(gate.GetComponent<Collider2D>(), gate.GetComponent<SpriteRenderer>(), switches);
        for (int index = 0; index < switchNames.Length; index++)
        {
            InvestigationPoint point = FindInScene(rubble, switchNames[index] + " Investigation")
                .GetComponent<InvestigationPoint>();
            if (point == null) throw new InvalidOperationException(switchNames[index] + " 缺少调查点组件。");
            SetReference(point, "handler", switches[index]);
        }

        EditorSceneManager.SaveScene(rubble);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Road Home Streetlight Artwork")]
    public static void ApplyRoadHomeStreetlightArtwork()
    {
        ConfigureFloorTextureImporter(RoadHomeStreetlightArtworkPath, "回家路路灯美术素材");
        Sprite streetlightSprite = AssetDatabase.LoadAssetAtPath<Sprite>(RoadHomeStreetlightArtworkPath);
        if (streetlightSprite == null) throw new InvalidOperationException("回家路路灯美术素材导入失败。");

        Scene roadHome = EditorSceneManager.OpenScene(RoadHomeScenePath, OpenSceneMode.Single);
        ApplyRoadHomeStreetlightArtwork(roadHome, streetlightSprite);
        EditorSceneManager.SaveScene(roadHome);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyRoadHomeStreetlightArtwork(Scene roadHome, Sprite streetlightSprite)
    {
        foreach (string name in new[] { "路灯 一", "路灯 二", "路灯 三" })
        {
            GameObject streetlight = FindInScene(roadHome, name);
            SpriteRenderer baseRenderer = streetlight.GetComponent<SpriteRenderer>();
            if (baseRenderer == null) throw new InvalidOperationException(name + " 缺少精灵渲染器。");

            Transform artwork = streetlight.transform.Find("Road Home Streetlight Artwork");
            if (artwork == null)
            {
                artwork = new GameObject("Road Home Streetlight Artwork").transform;
                artwork.SetParent(streetlight.transform, false);
                artwork.gameObject.AddComponent<SpriteRenderer>();
            }

            artwork.localPosition = name switch
            {
                "路灯 一" => new Vector3(6f, .05f, 0f),
                "路灯 二" => new Vector3(5f, .05f, 0f),
                _ => new Vector3(4f, .05f, 0f)
            };
            artwork.localRotation = Quaternion.identity;
            artwork.localScale = new Vector3(1.26f, .12f, 1f);
            SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
            artworkRenderer.sprite = streetlightSprite;
            artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
            artworkRenderer.color = Color.white;
            artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
            artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
            baseRenderer.enabled = false;
        }
    }

    [MenuItem("Tools/Devil RPG/Apply Road Home Ground Artwork")]
    public static void ApplyRoadHomeGroundArtwork()
    {
        ConfigureFloorTextureImporter(RoadHomeCustomRoadArtworkPath, "回家路自定义马路贴图");
        ConfigureFloorTextureImporter(ApartmentEntranceUpperSidewalkTexturePath, "人行道贴图");

        Scene roadHome = EditorSceneManager.OpenScene(RoadHomeScenePath, OpenSceneMode.Single);
        ApplyRoadHomeCustomRoadArtwork(roadHome);
        ApplyGroundArtwork(roadHome, "人行道", "Road Home Sidewalk Artwork",
            ApartmentEntranceUpperSidewalkTexturePath);
        SetSpriteSortingOrder(roadHome, "Road Home Custom Road Artwork", -10);
        SetSpriteSortingOrder(roadHome, "Road Home Sidewalk Artwork", -10);
        SetSpriteSortingOrder(roadHome, "回家路夜空", -11);
        EditorSceneManager.SaveScene(roadHome);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyRoadHomeCustomRoadArtwork(Scene roadHome)
    {
        Sprite roadArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(RoadHomeCustomRoadArtworkPath);
        if (roadArtwork == null) throw new InvalidOperationException("回家路自定义马路贴图导入失败。");

        GameObject road = FindInScene(roadHome, "马路");
        SpriteRenderer baseRenderer = road.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("马路缺少精灵渲染器。");

        GameObject legacyArtwork = FindOptionalInScene(roadHome, "Road Home Asphalt Road Artwork");
        if (legacyArtwork != null) UnityEngine.Object.DestroyImmediate(legacyArtwork);

        GameObject artwork = FindOptionalInScene(roadHome, "Road Home Custom Road Artwork");
        if (artwork == null)
        {
            artwork = FindOptionalInScene(roadHome, "ChatGPT 图像 2026年10月4日 12_14_45")
                ?? new GameObject("Road Home Custom Road Artwork");
            artwork.name = "Road Home Custom Road Artwork";
            artwork.transform.SetParent(FindInScene(roadHome, "回家的路").transform, false);
            if (artwork.GetComponent<SpriteRenderer>() == null) artwork.AddComponent<SpriteRenderer>();
        }

        artwork.transform.localPosition = new Vector3(.0208f, -3.8228f, 0f);
        artwork.transform.localRotation = Quaternion.identity;
        artwork.transform.localScale = new Vector3(1.1224172f, .50406235f, 1f);
        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = roadArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = -10;
        baseRenderer.enabled = false;

        foreach (string name in new[] { "道路标线 一", "道路标线 二", "道路标线 三" })
        {
            GameObject line = FindOptionalInScene(roadHome, name);
            if (line != null) line.SetActive(false);
        }
    }

    private static void ApplyGroundArtwork(Scene scene, string groundName, string artworkName,
        string artworkPath)
    {
        Sprite artworkSprite = AssetDatabase.LoadAssetAtPath<Sprite>(artworkPath);
        if (artworkSprite == null) throw new InvalidOperationException(groundName + " 贴图导入失败。");

        GameObject ground = FindInScene(scene, groundName);
        SpriteRenderer baseRenderer = ground.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(groundName + " 缺少精灵渲染器。");

        Transform artwork = ground.transform.parent.Find(artworkName);
        if (artwork == null)
        {
            artwork = new GameObject(artworkName).transform;
            artwork.SetParent(ground.transform.parent, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector2 targetSize = baseRenderer.bounds.size;
        Vector3 parentScale = artwork.parent.lossyScale;
        artwork.position = ground.transform.position;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            targetSize.x / (artworkSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            targetSize.y / (artworkSprite.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);
        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = -11;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Fix Outdoor Scene Sorting")]
    public static void FixOutdoorSceneSorting()
    {
        Scene apartment = EditorSceneManager.OpenScene(ApartmentEntranceScenePath, OpenSceneMode.Single);
        SetSpriteSortingOrder(apartment, "Apartment Night Sky", -12);
        SetSpriteSortingOrder(apartment, "Apartment Entrance Asphalt Road Artwork", -11);
        SetSpriteSortingOrder(apartment, "Apartment Entrance Upper Sidewalk Artwork", -11);
        SetSpriteSortingOrder(apartment, "Apartment Entrance Lower Sidewalk Artwork", -11);
        SetSpriteSortingOrder(apartment, "ChatGPT 图像 2026年10月4日 12_14_45", -11);
        EditorSceneManager.SaveScene(apartment);

        Scene roadOne = EditorSceneManager.OpenScene(RoadToSuburbsOneScenePath, OpenSceneMode.Single);
        SetSpriteSortingOrder(roadOne, "Suburbs Road Night Sky", -12);
        SetSpriteSortingOrder(roadOne, "Suburbs Road 1 Asphalt Artwork", -11);
        SetSpriteSortingOrder(roadOne, "Suburbs Road 1 Sidewalk Artwork", -11);
        SetSpriteSortingOrder(roadOne, "Suburbs Road 1 Lower Sidewalk Artwork", -11);
        EditorSceneManager.SaveScene(roadOne);

        Scene roadTwo = EditorSceneManager.OpenScene(RoadToSuburbsTwoScenePath, OpenSceneMode.Single);
        SetSpriteSortingOrder(roadTwo, "Suburbs Road 2 Night Sky", -12);
        SetSpriteSortingOrder(roadTwo, "Suburbs Road 2 Asphalt Artwork", -11);
        SetSpriteSortingOrder(roadTwo, "Suburbs Road 2 Sidewalk Artwork", -11);
        SetSpriteSortingOrder(roadTwo, "Suburbs Road 2 Lower Sidewalk Artwork", -11);
        EditorSceneManager.SaveScene(roadTwo);

        Scene rubble = EditorSceneManager.OpenScene(RubblePileScenePath, OpenSceneMode.Single);
        SetSpriteSortingOrder(rubble, "Rubble Night Sky", -12);
        SetSpriteSortingOrder(rubble, "Rubble Ground", -11);
        SetSpriteSortingOrder(rubble, "Rubble Ground Artwork", -11);
        EditorSceneManager.SaveScene(rubble);

        Scene roadHome = EditorSceneManager.OpenScene(RoadHomeScenePath, OpenSceneMode.Single);
        SetSpriteSortingOrder(roadHome, "回家路夜空", -11);
        SetSpriteSortingOrder(roadHome, "Road Home Custom Road Artwork", -10);
        SetSpriteSortingOrder(roadHome, "Road Home Sidewalk Artwork", -10);
        EditorSceneManager.SaveScene(roadHome);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Widen Suburban Maze Passages")]
    public static void WidenSuburbanMazePassages()
    {
        // 主角 CircleCollider2D 的世界直径为 1.0；所有实际开口保留至少 1.25 的净宽。
        Scene roadOne = EditorSceneManager.OpenScene(RoadToSuburbsOneScenePath, OpenSceneMode.Single);
        SetMazeWallLayout(roadOne, "Road Maze Upper Divider Left", new Vector2(232.875f, -.3f),
            new Vector2(5.75f, .35f));
        EditorSceneManager.SaveScene(roadOne);

        Scene roadTwo = EditorSceneManager.OpenScene(RoadToSuburbsTwoScenePath, OpenSceneMode.Single);
        SetMazeWallLayout(roadTwo, "Road 2 Upper Divider Mid Left", new Vector2(265.225f, 0f),
            new Vector2(3.05f, .35f));
        SetMazeWallLayout(roadTwo, "Road 2 Lower Divider Mid Left", new Vector2(264.6f, -3f),
            new Vector2(2.8f, .35f));

        // A、D 改为实心隔断，避免留下小于主角碰撞直径的伪开口；
        // B、C 各保留一条 1.275 宽的通道。
        SetMazeWallLayout(roadTwo, "Road 2 Vertical Block A", new Vector2(276.2f, 1.25f),
            new Vector2(.35f, 2.5f));
        SetMazeWallLayout(roadTwo, "Road 2 Vertical Block B", new Vector2(272.2f, -.675f),
            new Vector2(.35f, 1.75f));
        SetMazeWallLayout(roadTwo, "Road 2 Vertical Block C", new Vector2(268.2f, -5.125f),
            new Vector2(.35f, 1.35f));
        SetMazeWallLayout(roadTwo, "Road 2 Vertical Block D", new Vector2(264.2f, 1.3f),
            new Vector2(.35f, 2.5f));
        EditorSceneManager.SaveScene(roadTwo);

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Suburban Road Ground Artwork")]
    public static void ApplySuburbanRoadGroundArtwork()
    {
        ConfigureFloorTextureImporter(ApartmentEntranceAsphaltRoadTexturePath, "柏油马路贴图");
        ConfigureFloorTextureImporter(ApartmentEntranceUpperSidewalkTexturePath, "上侧人行道贴图");
        ConfigureFloorTextureImporter(ApartmentEntranceLowerSidewalkTexturePath, "下侧人行道贴图");

        Scene roadOne = EditorSceneManager.OpenScene(RoadToSuburbsOneScenePath, OpenSceneMode.Single);
        ApplySuburbanRoadThreeBandBackground(roadOne, "通往郊区的路（1）", "Suburbs Road",
            "Suburbs Road Pavement", "Suburbs Road Lower Pavement", "Suburbs Road 1");
        EditorSceneManager.SaveScene(roadOne);

        Scene roadTwo = EditorSceneManager.OpenScene(RoadToSuburbsTwoScenePath, OpenSceneMode.Single);
        ApplySuburbanRoadThreeBandBackground(roadTwo, "通往郊区的路（2）", "Suburbs Road 2",
            "Suburbs Road 2 Pavement", "Suburbs Road 2 Lower Pavement", "Suburbs Road 2");
        EditorSceneManager.SaveScene(roadTwo);
        AssetDatabase.SaveAssets();
    }

    private static void ApplySuburbanRoadThreeBandBackground(Scene scene, string rootName,
        string roadName, string upperSidewalkName, string lowerSidewalkName, string artworkPrefix)
    {
        GameObject road = FindInScene(scene, roadName);
        GameObject upperSidewalk = FindInScene(scene, upperSidewalkName);
        SpriteRenderer source = road.GetComponent<SpriteRenderer>();
        if (source == null) throw new InvalidOperationException(roadName + " 缺少精灵渲染器。");

        road.transform.position = new Vector3(road.transform.position.x, -1.35f, 0f);
        road.transform.localScale = new Vector3(20f, 3.8f, 1f);
        upperSidewalk.transform.position = new Vector3(upperSidewalk.transform.position.x, 1.55f, 0f);
        upperSidewalk.transform.localScale = new Vector3(20f, 2f, 1f);

        GameObject root = FindInScene(scene, rootName);
        GameObject lowerSidewalk = FindOrCreateObject(root.transform, lowerSidewalkName,
            new Vector2(root.transform.position.x, -4.525f));
        lowerSidewalk.transform.localScale = new Vector3(20f, 2.55f, 1f);
        SpriteRenderer lowerRenderer = lowerSidewalk.GetComponent<SpriteRenderer>();
        if (lowerRenderer == null) lowerRenderer = lowerSidewalk.AddComponent<SpriteRenderer>();
        lowerRenderer.sprite = source.sprite;
        lowerRenderer.sharedMaterial = source.sharedMaterial;
        lowerRenderer.color = Color.white;
        lowerRenderer.sortingOrder = 0;

        ApplyGroundArtwork(scene, roadName, artworkPrefix + " Asphalt Artwork",
            ApartmentEntranceAsphaltRoadTexturePath);
        ApplyGroundArtwork(scene, upperSidewalkName, artworkPrefix + " Sidewalk Artwork",
            ApartmentEntranceUpperSidewalkTexturePath);
        ApplyGroundArtwork(scene, lowerSidewalkName, artworkPrefix + " Lower Sidewalk Artwork",
            ApartmentEntranceLowerSidewalkTexturePath);
    }

    [MenuItem("Tools/Devil RPG/Apply Rubble Pile Ground Artwork")]
    public static void ApplyRubblePileGroundArtwork()
    {
        ConfigureFloorTextureImporter(RubblePileGroundTexturePath, "瓦砾堆地面贴图");
        Scene rubble = EditorSceneManager.OpenScene(RubblePileScenePath, OpenSceneMode.Single);
        ApplyRubblePileGroundArtwork(rubble);
        EditorSceneManager.SaveScene(rubble);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyRubblePileGroundArtwork(Scene rubble)
    {
        ApplyGroundArtwork(rubble, "Rubble Ground", "Rubble Ground Artwork",
            RubblePileGroundTexturePath);
    }

    [MenuItem("Tools/Devil RPG/Apply Rubble Lore Note Artwork")]
    public static void ApplyRubbleLoreNoteArtwork()
    {
        ConfigureFloorTextureImporter(RubbleTuitionReceiptArtworkPath, "学生缴费单美术素材");
        ConfigureFloorTextureImporter(RubbleDebtNoticeArtworkPath, "欠款条美术素材");
        Sprite tuitionReceipt = AssetDatabase.LoadAssetAtPath<Sprite>(RubbleTuitionReceiptArtworkPath);
        Sprite debtNotice = AssetDatabase.LoadAssetAtPath<Sprite>(RubbleDebtNoticeArtworkPath);
        if (tuitionReceipt == null || debtNotice == null)
        {
            throw new InvalidOperationException("瓦砾堆纸条美术素材导入失败。");
        }

        Scene rubble = EditorSceneManager.OpenScene(RubblePileScenePath, OpenSceneMode.Single);
        ApplyRubbleLoreNoteArtwork(rubble, "学生缴费单", "Rubble Tuition Receipt Artwork",
            tuitionReceipt, new Vector2(.8f, .52f));
        ApplyRubbleLoreNoteArtwork(rubble, "欠款条", "Rubble Debt Notice Artwork",
            debtNotice, new Vector2(.68f, .56f));
        EditorSceneManager.SaveScene(rubble);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Thirteen Crystal Gift Artwork")]
    public static void ApplyThirteenCrystalGiftArtwork()
    {
        ConfigureFloorTextureImporter(ThirteenCrystalGiftArtworkPath, "十三晶石礼物美术素材");
        Sprite crystalSprite = AssetDatabase.LoadAssetAtPath<Sprite>(ThirteenCrystalGiftArtworkPath);
        if (crystalSprite == null) throw new InvalidOperationException("十三晶石礼物美术素材导入失败。");

        Scene cave = EditorSceneManager.OpenScene(FluorescentMushroomCaveScenePath, OpenSceneMode.Single);
        GameObject crystal = FindInScene(cave, "十三的晶石礼物");
        SpriteRenderer baseRenderer = crystal.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("十三的晶石礼物缺少精灵渲染器。");

        Transform artwork = crystal.transform.Find("Thirteen Crystal Gift Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Thirteen Crystal Gift Artwork").transform;
            artwork.SetParent(crystal.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = crystal.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            .68f / (crystalSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            .68f / (crystalSprite.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);
        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = crystalSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(cave);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Rubble Dropped Electric Baton Artwork")]
    public static void ApplyRubbleDroppedElectricBatonArtwork()
    {
        ConfigureFloorTextureImporter(RubbleDroppedElectricBatonArtworkPath, "遗落电棍美术素材");
        Sprite batonSprite = AssetDatabase.LoadAssetAtPath<Sprite>(RubbleDroppedElectricBatonArtworkPath);
        if (batonSprite == null) throw new InvalidOperationException("遗落电棍美术素材导入失败。");

        Scene rubble = EditorSceneManager.OpenScene(RubblePileScenePath, OpenSceneMode.Single);
        GameObject baton = FindInScene(rubble, "遗落的电棍");
        SpriteRenderer baseRenderer = baton.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("遗落的电棍缺少精灵渲染器。");

        Transform artwork = baton.transform.Find("Rubble Dropped Electric Baton Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Rubble Dropped Electric Baton Artwork").transform;
            artwork.SetParent(baton.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = baton.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            .72f / (batonSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            .36f / (batonSprite.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);
        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = batonSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(rubble);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Rubble Injured Demon Artwork")]
    public static void ApplyRubbleInjuredDemonArtwork()
    {
        ConfigureFloorTextureImporter(RubbleInjuredSmallDemonArtworkPath, "受伤小恶魔美术素材");
        Scene rubble = EditorSceneManager.OpenScene(RubblePileScenePath, OpenSceneMode.Single);
        ApplyRubbleInjuredDemonArtwork(rubble);
        EditorSceneManager.SaveScene(rubble);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyRubbleInjuredDemonArtwork(Scene rubble)
    {
        Sprite demonSprite = AssetDatabase.LoadAssetAtPath<Sprite>(RubbleInjuredSmallDemonArtworkPath);
        if (demonSprite == null) throw new InvalidOperationException("受伤小恶魔美术素材导入失败。");

        GameObject demon = FindInScene(rubble, "受伤的小恶魔");
        SpriteRenderer baseRenderer = demon.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("受伤的小恶魔缺少精灵渲染器。");

        Transform artwork = demon.transform.Find("Rubble Injured Small Demon Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Rubble Injured Small Demon Artwork").transform;
            artwork.SetParent(demon.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = demon.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            .92f / (demonSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            .52f / (demonSprite.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);
        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = demonSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    [MenuItem("Tools/Devil RPG/Position Rubble Intro Props")]
    public static void PositionRubbleIntroProps()
    {
        Scene rubble = EditorSceneManager.OpenScene(RubblePileScenePath, OpenSceneMode.Single);
        GameObject injuredDemon = FindInScene(rubble, "受伤的小恶魔");
        GameObject droppedBaton = FindInScene(rubble, "遗落的电棍");
        injuredDemon.transform.position = InjuredDemonIntroPosition;
        droppedBaton.transform.position = DroppedBatonIntroPosition;
        EditorSceneManager.SaveScene(rubble);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Widen Rubble Lore Note Passages")]
    public static void WidenRubbleLoreNotePassages()
    {
        Scene rubble = EditorSceneManager.OpenScene(RubblePileScenePath, OpenSceneMode.Single);
        GameObject westWall = FindInScene(rubble, "Rubble Vertical Wall West");
        SpriteRenderer renderer = westWall.GetComponent<SpriteRenderer>();
        if (renderer == null) throw new InvalidOperationException("瓦砾堆竖墙缺少精灵渲染器。");

        CreateRubbleLoreNotePassageWalls(FindInScene(rubble, "瓦砾堆").transform,
            renderer.sprite, renderer.sharedMaterial, true);
        EditorSceneManager.SaveScene(rubble);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Persistent Thirteen Companion")]
    public static void InstallPersistentThirteenCompanion()
    {
        Scene cave = EditorSceneManager.OpenScene(FluorescentMushroomCaveScenePath, OpenSceneMode.Single);
        GameObject thirteen = FindInScene(cave, "？？？");
        if (thirteen.GetComponent<ThirteenFollower>() == null)
        {
            thirteen.AddComponent<ThirteenFollower>();
        }
        EditorSceneManager.SaveScene(cave);

        Scene roadHome = EditorSceneManager.OpenScene(RoadHomeScenePath, OpenSceneMode.Single);
        GameObject roadHomeCopy = FindOptionalInScene(roadHome, "十三");
        if (roadHomeCopy != null) UnityEngine.Object.DestroyImmediate(roadHomeCopy);
        EditorSceneManager.SaveScene(roadHome);

        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject livingRoomCopy = FindOptionalInScene(livingRoom, "十三（客厅）");
        if (livingRoomCopy != null) UnityEngine.Object.DestroyImmediate(livingRoomCopy);
        ApartmentConfrontation confrontation = FindInScene(livingRoom, "Apartment Living Room")
            .GetComponent<ApartmentConfrontation>();
        if (confrontation != null) SetReference(confrontation, "thirteen", null);
        EditorSceneManager.SaveScene(livingRoom);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyRubbleLoreNoteArtwork(Scene scene, string noteName, string artworkName,
        Sprite artworkSprite, Vector2 worldSize)
    {
        GameObject note = FindInScene(scene, noteName);
        SpriteRenderer baseRenderer = note.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(noteName + " 缺少精灵渲染器。");

        Transform artwork = note.transform.Find(artworkName);
        if (artwork == null)
        {
            artwork = new GameObject(artworkName).transform;
            artwork.SetParent(note.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = note.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            worldSize.x / (artworkSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            worldSize.y / (artworkSprite.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);
        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    private static void CreateRubbleSwitchArtwork(GameObject switchObject, string artworkName,
        Sprite artworkSprite, SpriteRenderer baseRenderer, bool active)
    {
        Transform artwork = switchObject.transform.Find(artworkName);
        if (artwork == null)
        {
            artwork = new GameObject(artworkName).transform;
            artwork.SetParent(switchObject.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(1f / artworkSprite.bounds.size.x,
            1f / artworkSprite.bounds.size.y, 1f);
        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        artwork.gameObject.SetActive(active);
    }

    private static GameObject CreateRubbleIronGateArtwork(GameObject gate, string artworkName,
        Sprite artworkSprite, Vector2 worldSize, SpriteRenderer baseRenderer)
    {
        Transform artwork = gate.transform.Find(artworkName);
        if (artwork == null)
        {
            artwork = new GameObject(artworkName).transform;
            artwork.SetParent(gate.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        Vector3 parentScale = gate.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            worldSize.x / (artworkSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            worldSize.y / (artworkSprite.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);
        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        return artwork.gameObject;
    }

    private static GameObject CreateProtagonistBedsideArtwork(GameObject table, string artworkName,
        Sprite artworkSprite, Vector2 worldSize)
    {
        Transform artwork = table.transform.Find(artworkName);
        if (artwork == null)
        {
            artwork = new GameObject(artworkName).transform;
            artwork.SetParent(table.transform, false);
            artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        SpriteRenderer baseRenderer = table.GetComponent<SpriteRenderer>();
        Vector3 parentScale = table.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            worldSize.x / (artworkSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            worldSize.y / (artworkSprite.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        return artwork.gameObject;
    }

    [MenuItem("Tools/Devil RPG/Apply School Corridor Wall Artwork")]
    public static void ApplySchoolCorridorWallArtwork()
    {
        TextureImporter importer = AssetImporter.GetAtPath(SchoolCorridorWallArtworkPath)
            as TextureImporter;
        if (importer == null) throw new InvalidOperationException("找不到走廊墙壁美术素材。");
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;
        AssetDatabase.ImportAsset(SchoolCorridorWallArtworkPath, ImportAssetOptions.ForceUpdate);

        Sprite wallArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(SchoolCorridorWallArtworkPath);
        if (wallArtwork == null) throw new InvalidOperationException("走廊墙壁美术素材导入失败。");

        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        ApplyCorridorWallVisual(FindInScene(corridor, "Corridor Wall Top"), wallArtwork, false);
        RemoveCorridorWallVisual(FindInScene(corridor, "Corridor Wall Bottom"));
        SetSpriteSortingOrder(corridor, "Corridor Floor Artwork", -10);
        SetSpriteSortingOrder(corridor, "Corridor Wall Top", 0);
        SetSpriteSortingOrder(corridor, "School Gate Wall Upper", 0);
        SetSpriteSortingOrder(corridor, "School Gate Wall Lower", 0);
        SetSpriteSortingOrder(corridor, "Library Door", 2);
        SetSpriteSortingOrder(corridor, "Janitor Rest Room Door", 2);
        SetSpriteSortingOrder(corridor, "Storage Room Door", 2);
        SetSpriteSortingOrder(corridor, "School Gate", 2);
        SetSpriteSortingOrder(corridor, "Stairwell Door", 2);
        EditorSceneManager.SaveScene(corridor);
        AssetDatabase.SaveAssets();
    }

    private static void ApplyCorridorWallVisual(GameObject wall, Sprite wallArtwork, bool flipY)
    {
        SpriteRenderer baseRenderer = wall.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(wall.name + " 缺少精灵渲染器。");

        string artworkName = wall.name + " Artwork";
        Transform artwork = wall.transform.Find(artworkName);
        if (artwork == null)
        {
            artwork = new GameObject(artworkName).transform;
            artwork.SetParent(wall.transform, false);
        }

        Vector3 parentScale = wall.transform.lossyScale;
        artwork.localPosition = new Vector3(0f, flipY ? 1.4f : -1.4f, 0f);
        artwork.localRotation = Quaternion.identity;
        artwork.localScale = new Vector3(
            15.36f / (wallArtwork.bounds.size.x * parentScale.x),
            1.1f / (wallArtwork.bounds.size.y * parentScale.y), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = wallArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.flipY = flipY;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = 0;
        baseRenderer.enabled = false;
    }

    private static void RemoveCorridorWallVisual(GameObject wall)
    {
        Transform artwork = wall.transform.Find(wall.name + " Artwork");
        if (artwork != null) UnityEngine.Object.DestroyImmediate(artwork.gameObject);

        SpriteRenderer baseRenderer = wall.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(wall.name + " 缺少精灵渲染器。");
        baseRenderer.enabled = true;
    }

    [MenuItem("Tools/Devil RPG/Apply Library Bookcase Artwork")]
    public static void ApplyLibraryBookcaseArtwork()
    {
        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        ApplyLibraryBookcaseArtwork(library);
        ShrinkLibraryBookcaseColliders(library);
        EditorSceneManager.SaveScene(library);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Shrink Library Bookcase Colliders")]
    public static void ShrinkLibraryBookcaseColliders()
    {
        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        ShrinkLibraryBookcaseColliders(library);
        EditorSceneManager.SaveScene(library);
        AssetDatabase.SaveAssets();
    }

    private static void ShrinkLibraryBookcaseColliders(Scene library)
    {
        GameObject root = FindInScene(library, "School Library");
        foreach (Transform bookcase in root.GetComponentsInChildren<Transform>(true))
        {
            if (!bookcase.name.StartsWith("Bookcase ")) continue;
            BoxCollider2D collider = bookcase.GetComponent<BoxCollider2D>();
            if (collider == null) continue;
            collider.size = new Vector2(.8f, .8f);
        }
    }

    [MenuItem("Tools/Devil RPG/Apply Library Counter Artwork")]
    public static void ApplyLibraryCounterArtwork()
    {
        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        ApplyLibraryCounterArtwork(library);
        EditorSceneManager.SaveScene(library);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Library Reading Chair Artwork")]
    public static void ApplyLibraryReadingChairArtwork()
    {
        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        ApplyLibraryReadingChairArtwork(library);
        EditorSceneManager.SaveScene(library);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Library Reading Table Artwork")]
    public static void ApplyLibraryReadingTableArtwork()
    {
        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        ApplyLibraryReadingTableArtwork(library);
        EditorSceneManager.SaveScene(library);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Fix Library Floor Sorting")]
    public static void FixLibraryFloorSorting()
    {
        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        GameObject floor = FindInScene(library, "Library Floor Artwork");
        SpriteRenderer floorRenderer = floor.GetComponent<SpriteRenderer>();
        if (floorRenderer == null) throw new InvalidOperationException("图书馆地板缺少精灵渲染器。");
        floorRenderer.sortingOrder = -10;

        GameObject libraryRoot = FindInScene(library, "School Library");
        foreach (SpriteRenderer renderer in libraryRoot.GetComponentsInChildren<SpriteRenderer>(true))
        {
            if (renderer != floorRenderer && renderer.sortingOrder < 0)
            {
                renderer.sortingOrder = 0;
            }
        }

        EditorSceneManager.SaveScene(library);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Classroom Front Wall Artwork")]
    public static void ApplyClassroomFrontWallArtwork()
    {
        Sprite wallArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(ClassroomFrontWallArtworkPath);
        if (wallArtwork == null) throw new InvalidOperationException("找不到教室正前方墙面素材。");

        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject wall = FindInScene(classroom, "Wall Top");
        SpriteRenderer baseRenderer = wall.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException("Wall Top 缺少精灵渲染器。");

        Transform artwork = wall.transform.Find("Classroom Front Wall Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Classroom Front Wall Artwork").transform;
            artwork.SetParent(wall.transform, false);
        }

        Vector3 parentScale = wall.transform.lossyScale;
        artwork.localPosition = new Vector3(0f, -1.5f, 0f);
        artwork.localScale = new Vector3(
            15.6f / (wallArtwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            3.2f / (wallArtwork.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = wallArtwork;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = -6;
        baseRenderer.enabled = false;

        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply Classroom Cactus Pot Artwork")]
    public static void ApplyClassroomCactusPotArtwork()
    {
        Sprite intactArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(ClassroomCactusPotIntactArtworkPath);
        Sprite brokenArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(ClassroomCactusPotBrokenArtworkPath);
        if (intactArtwork == null || brokenArtwork == null)
        {
            throw new InvalidOperationException("找不到教室仙人掌盆栽素材。");
        }

        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        ApplyFlowerpotVisual(FindInScene(classroom, "Flowerpot Intact Visual").transform,
            "Cactus Pot Intact Artwork", intactArtwork, new Vector2(1.25f, 1.25f));
        ApplyFlowerpotVisual(FindInScene(classroom, "Flowerpot Broken Visual").transform,
            "Cactus Pot Broken Artwork", brokenArtwork, new Vector2(1.55f, 1.1f));
        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Replace Classroom Doors With Doorways")]
    public static void ReplaceClassroomDoorsWithDoorways()
    {
        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        RemoveSceneObject(classroom, "Door");
        RemoveSceneObject(classroom, "Exit To School Corridor Portal");
        RemoveSceneObject(classroom, "Doorway Blocker");
        CreateAutomaticTransitionPoint(FindInScene(classroom, "Classroom").transform,
            "Classroom To Corridor Doorway", new Vector2(8.35f, -3.25f),
            "SchoolCorridor", "CorridorFromClassroom");
        EditorSceneManager.SaveScene(classroom);

        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        RemoveSceneObject(corridor, "Classroom Door");
        RemoveSceneObject(corridor, "Return To Classroom Portal");
        RemoveSceneObject(corridor, "Doorway Blocker");
        CreateAutomaticTransitionPoint(FindInScene(corridor, "School Corridor").transform,
            "Corridor To Classroom Doorway", new Vector2(21.6f, 0f),
            "SchoolClassroom", "ClassroomFromCorridor");
        EditorSceneManager.SaveScene(corridor);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Replace Library Exit Door With Doorway")]
    public static void ReplaceLibraryExitDoorWithDoorway()
    {
        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        RemoveSceneObject(library, "Library Exit Door");
        CreateAutomaticTransitionPoint(FindInScene(library, "School Library").transform,
            "Library To Corridor Doorway", new Vector2(60f, -5.8f),
            "SchoolCorridor", "CorridorFromLibrary");
        EditorSceneManager.SaveScene(library);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Replace Storage Exit Door With Doorway")]
    public static void ReplaceStorageExitDoorWithDoorway()
    {
        Scene storage = EditorSceneManager.OpenScene(StorageRoomScenePath, OpenSceneMode.Single);
        RemoveSceneObject(storage, "Storage Room Exit Door");
        CreateAutomaticTransitionPoint(FindInScene(storage, "School Storage Room").transform,
            "Storage Room To Corridor Doorway", new Vector2(90f, -6.8f),
            "SchoolCorridor", "CorridorFromStorage");
        EditorSceneManager.SaveScene(storage);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Remove Classroom Top Left Student")]
    public static void RemoveClassroomTopLeftStudent()
    {
        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject student = FindInScene(classroom, "Classroom Student 3");
        UnityEngine.Object.DestroyImmediate(student);
        if (FindOptionalInScene(classroom, "Classroom Student 3") != null)
        {
            throw new InvalidOperationException("无法删除教室左上角的学生对象。");
        }
        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Move Top Row Student To Left")]
    public static void MoveTopRowStudentToLeft()
    {
        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject rightStudent = FindOptionalInScene(classroom, "Classroom Student 4");
        if (rightStudent == null)
        {
            throw new InvalidOperationException("教室右上角学生不存在。");
        }

        GameObject leftStudent = FindOptionalInScene(classroom, "Classroom Student 3");
        if (leftStudent == null)
        {
            leftStudent = UnityEngine.Object.Instantiate(rightStudent, rightStudent.transform.parent);
            leftStudent.name = "Classroom Student 3";
        }

        leftStudent.transform.localPosition = new Vector3(-4f, 1.48f, 0f);
        UnityEngine.Object.DestroyImmediate(rightStudent);
        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Fix School Wall Sorting")]
    public static void FixSchoolWallSorting()
    {
        SetWallSortingOrder(ClassroomScenePath, "Wall Top", "Wall Bottom", "Wall Left",
            "Wall Right Upper", "Wall Right Lower");
        SetWallSortingOrder(CorridorScenePath, "Corridor Wall Top", "Corridor Wall Bottom",
            "Corridor Wall Left Upper", "Corridor Wall Left Lower", "Corridor Wall Right");
        SetWallSortingOrder(LibraryScenePath, "Library Wall Top", "Library Wall Bottom Left",
            "Library Wall Bottom Right", "Library Wall Left", "Library Wall Right");
        SetWallSortingOrder(StorageRoomScenePath, "Storage Wall Top", "Storage Wall Bottom Left",
            "Storage Wall Bottom Right", "Storage Wall Left", "Storage Wall Right");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Fix School Door Sorting")]
    public static void FixSchoolDoorSorting()
    {
        // 学校场景统一遵守：地板 < 墙壁 < 门。
        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        SetSpriteSortingOrder(corridor, "Corridor Floor Artwork", -10);
        SetSpriteSortingOrder(corridor, "Corridor Wall Top", 0);
        SetSpriteSortingOrder(corridor, "Corridor Wall Bottom", 0);
        SetSpriteSortingOrder(corridor, "Corridor Wall Left Upper", 0);
        SetSpriteSortingOrder(corridor, "Corridor Wall Left Lower", 0);
        SetSpriteSortingOrder(corridor, "Corridor Wall Right", 0);
        SetSpriteSortingOrder(corridor, "School Gate Wall Upper", 0);
        SetSpriteSortingOrder(corridor, "School Gate Wall Lower", 0);
        SetSpriteSortingOrder(corridor, "Library Door", 2);
        SetSpriteSortingOrder(corridor, "Janitor Rest Room Door", 2);
        SetSpriteSortingOrder(corridor, "Storage Room Door", 2);
        SetSpriteSortingOrder(corridor, "School Gate", 2);
        SetSpriteSortingOrder(corridor, "Stairwell Door", 2);
        EditorSceneManager.SaveScene(corridor);

        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        SetSpriteSortingOrder(library, "Library Floor Artwork", -10);
        SetSpriteSortingOrder(library, "Library Wall Top", 0);
        SetSpriteSortingOrder(library, "Library Wall Bottom Left", 0);
        SetSpriteSortingOrder(library, "Library Wall Bottom Right", 0);
        SetSpriteSortingOrder(library, "Library Wall Left", 0);
        SetSpriteSortingOrder(library, "Library Wall Right", 0);
        SetSpriteSortingOrder(library, "Library Exit Door", 2);
        EditorSceneManager.SaveScene(library);

        Scene storageRoom = EditorSceneManager.OpenScene(StorageRoomScenePath, OpenSceneMode.Single);
        SetSpriteSortingOrder(storageRoom, "Storage Wall Top", 0);
        SetSpriteSortingOrder(storageRoom, "Storage Wall Bottom Left", 0);
        SetSpriteSortingOrder(storageRoom, "Storage Wall Bottom Right", 0);
        SetSpriteSortingOrder(storageRoom, "Storage Wall Left", 0);
        SetSpriteSortingOrder(storageRoom, "Storage Wall Right", 0);
        SetSpriteSortingOrder(storageRoom, "Storage Room Exit Door", 2);
        EditorSceneManager.SaveScene(storageRoom);

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Remove Classroom Windows")]
    public static void RemoveClassroomWindows()
    {
        Scene classroomScene = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        GameObject windows = FindOptionalInScene(classroomScene, "Windows");
        if (windows != null)
        {
            UnityEngine.Object.DestroyImmediate(windows);
            EditorSceneManager.SaveScene(classroomScene);
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply School Door Artwork")]
    public static void ApplySchoolDoorArtwork()
    {
        Sprite libraryArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryDoorArtworkPath);
        Sprite janitorArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(JanitorRestRoomDoorArtworkPath);
        Sprite storageArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(StorageRoomDoorArtworkPath);
        if (libraryArtwork == null || janitorArtwork == null || storageArtwork == null)
        {
            throw new InvalidOperationException("找不到学校门美术素材。");
        }

        ApplyDoorArtwork(CorridorScenePath, "Library Door Panel", "Library Door Artwork", libraryArtwork, 2.1f);
        ApplyDoorArtwork(CorridorScenePath, "Janitor Rest Room Door", "Janitor Rest Room Door Artwork", janitorArtwork, 2f);
        ApplyDoorArtwork(CorridorScenePath, "Storage Room Door Panel", "Storage Room Door Artwork", storageArtwork, 2f);
        ApplyDoorArtwork(StorageRoomScenePath, "Storage Room Exit Door Panel", "Storage Room Door Artwork", storageArtwork, 2f);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Disable School Door Visual Rotation")]
    public static void DisableSchoolDoorVisualRotation()
    {
        DisableDoorVisualRotation(CorridorScenePath, "Library Door");
        DisableDoorVisualRotation(CorridorScenePath, "Storage Room Door");
        DisableDoorVisualRotation(StorageRoomScenePath, "Storage Room Exit Door");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Apply School Door Open Artwork")]
    public static void ApplySchoolDoorOpenArtwork()
    {
        Sprite libraryClosed = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryDoorArtworkPath);
        Sprite libraryOpen = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryDoorOpenArtworkPath);
        Sprite storageClosed = AssetDatabase.LoadAssetAtPath<Sprite>(StorageRoomDoorArtworkPath);
        Sprite storageOpen = AssetDatabase.LoadAssetAtPath<Sprite>(StorageRoomDoorOpenArtworkPath);
        if (libraryClosed == null || libraryOpen == null || storageClosed == null || storageOpen == null)
        {
            throw new InvalidOperationException("找不到学校门开启状态美术素材。");
        }

        ConfigureDoorStateArtwork(CorridorScenePath, "Library Door", "Library Door Artwork", libraryClosed, libraryOpen);
        ConfigureDoorStateArtwork(CorridorScenePath, "Storage Room Door", "Storage Room Door Artwork", storageClosed, storageOpen);
        ConfigureDoorStateArtwork(StorageRoomScenePath, "Storage Room Exit Door", "Storage Room Door Artwork", storageClosed, storageOpen);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Remove School Gate And Side Wall Artwork")]
    public static void RemoveSchoolGateAndSideWallArtwork()
    {
        Scene corridor = EditorSceneManager.OpenScene(CorridorScenePath, OpenSceneMode.Single);
        RemoveSceneObject(corridor, "School Gate Artwork");
        RemoveSceneObject(corridor, "School Gate Side Wall Artwork");

        GameObject gatePanel = FindInScene(corridor, "School Gate Glass Panel");
        DoorController door = gatePanel.GetComponent<DoorController>();
        if (door == null) throw new InvalidOperationException("学校大门缺少门控制器。");
        SetBool(door, "animateVisual", false);
        SpriteRenderer gateRenderer = gatePanel.GetComponent<SpriteRenderer>();
        if (gateRenderer != null) gateRenderer.enabled = false;

        SpriteRenderer sideWallRenderer = FindInScene(corridor, "Corridor Wall Right")
            .GetComponent<SpriteRenderer>();
        if (sideWallRenderer != null) sideWallRenderer.enabled = false;

        EditorSceneManager.SaveScene(corridor);
        AssetDatabase.DeleteAsset("Assets/Art/Sprites/school-gate-glass-door-pixel-v1.png");
        AssetDatabase.DeleteAsset("Assets/Art/Sprites/school-gate-glass-door-open-pixel-v1.png");
        AssetDatabase.DeleteAsset("Assets/Art/Sprites/school-gate-side-wall-pixel-v1.png");
        AssetDatabase.SaveAssets();
    }

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
        int nameEntryIndex = scenes.FindIndex(scene => scene.path == NameEntryScenePath);
        int instructionsIndex = nameEntryIndex >= 0 ? nameEntryIndex + 1 : 0;
        scenes.Insert(instructionsIndex, new EditorBuildSettingsScene(GameInstructionsScenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Name Entry")]
    public static void InstallNameEntry()
    {
        Scene nameEntry = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        new GameObject("Name Entry").AddComponent<NameEntryStart>();
        EditorSceneManager.SaveScene(nameEntry, NameEntryScenePath);

        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        scenes.RemoveAll(scene => scene.path == NameEntryScenePath);
        int nameEntryIndex = scenes.Exists(scene => scene.path == LanguageSelectionScenePath) ? 1 : 0;
        scenes.Insert(nameEntryIndex, new EditorBuildSettingsScene(NameEntryScenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Language Selection")]
    public static void InstallLanguageSelection()
    {
        Scene languageSelection =
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        new GameObject("Language Selection").AddComponent<LanguageSelectionStart>();
        EditorSceneManager.SaveScene(languageSelection, LanguageSelectionScenePath);

        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        scenes.RemoveAll(scene => scene.path == LanguageSelectionScenePath);
        scenes.Insert(0, new EditorBuildSettingsScene(LanguageSelectionScenePath, true));
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
            new Vector2(0f, -0.02f), new Vector2(-4f, -1.52f),
            new Vector2(-4f, 1.48f), new Vector2(4f, 1.48f),
            new Vector2(4f, -3.02f)
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
        float[] chairRows = { 1.48f, -0.02f, -1.52f, -3.02f };
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

    [MenuItem("Tools/Devil RPG/Move Classroom Chairs Closer")]
    public static void MoveClassroomChairsCloser()
    {
        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        ArrangeClassroomSeating(classroom);

        Vector2[] seats =
        {
            new Vector2(0f, -0.02f), new Vector2(-4f, -1.52f),
            new Vector2(-4f, 1.48f), new Vector2(4f, 1.48f),
            new Vector2(4f, -3.02f)
        };
        for (int index = 0; index < seats.Length; index++)
        {
            FindInScene(classroom, "Classroom Student " + (index + 1)).transform.position =
                seats[index];
        }

        FindInScene(classroom, "Main Character").transform.position = new Vector2(-4f, -3.02f);
        EditorSceneManager.SaveScene(classroom);
        AssetDatabase.SaveAssets();
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
        ApplyLibraryLooseWallPanelArtwork(libraryScene);
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

    [MenuItem("Tools/Devil RPG/Realign Library Counter Glue Point")]
    public static void RealignLibraryCounterGluePoint()
    {
        Scene library = EditorSceneManager.OpenScene(LibraryScenePath, OpenSceneMode.Single);
        AlignLibraryCounterGluePoint(library);
        EditorSceneManager.SaveScene(library);
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
        SetString(door, "requiredItemId", "library.card");
        SetString(door, "missingItemDialogue", "图书馆门需要刷借书卡入内，我得先拿到借书卡。");
        SetString(door, "missingItemDialogueKey", "corridor.libraryDoor.missingCard");
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
        ConfigureDoorGroup(CorridorScenePath, "Library Door", "library-corridor");
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
        ApplyApartmentEntranceDoorArtwork(entrance);
        EditorSceneManager.SaveScene(entrance);

        Scene livingRoom = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject root = new GameObject("Apartment Living Room");
        root.transform.position = new Vector3(140f, 0f, 0f);
        CreateUnit(root.transform, "Living Room Floor", new Vector2(140f, 0f),
            new Vector2(15f, 9f), new Color(0.45f, 0.36f, 0.29f, 1f), -1,
            false, sprite, material);
        ApplyLivingRoomCarpetFloorArtwork(livingRoom);
        CreateUnit(root.transform, "Living Room Back Wall", new Vector2(140f, 0f),
            new Vector2(18f, 12f), new Color(0.24f, 0.22f, 0.2f, 1f), -2,
            false, sprite, material);
        CreateLivingRoomWalls(root.transform, sprite, material);
        ApplyLivingRoomUpperWallArtwork(livingRoom);
        ApplyLivingRoomSideAndBottomWallArtwork(livingRoom);

        CreateUnit(root.transform, "Living Room Sofa", new Vector2(140f, 1.25f),
            new Vector2(3.6f, 1.2f), new Color(0.22f, 0.33f, 0.36f, 1f), 2,
            true, sprite, material);
        CreateUnit(root.transform, "Living Room Coffee Table", new Vector2(140f, -0.35f),
            new Vector2(1.7f, 0.8f), new Color(0.27f, 0.16f, 0.09f), 3,
            true, sprite, material);
        Sprite coffeeTableArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LivingRoomCoffeeTableArtworkPath);
        if (coffeeTableArtwork != null)
        {
            ApplyLivingRoomCoffeeTableArtwork(livingRoom, coffeeTableArtwork);
        }
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
        CreateAutomaticTransitionPoint(root.transform, "Apartment Exit Doorway", new Vector2(140f, -5f),
            "ApartmentEntrance", "ApartmentEntranceFromLivingRoom");
        CreateInteriorDoor(root.transform, "Protagonist Bedroom Door", new Vector2(136f, 5f), sprite, material);
        CreateInteriorDoor(root.transform, "Lin Chunbao Bedroom Door", new Vector2(144f, 5f), sprite, material);
        CreateInteriorDoor(root.transform, "Bathroom Door", new Vector2(148f, 1.55f), sprite, material);
        GameObject kitchenDoor = CreateDoorWithPortal(root.transform, "Kitchen Door", new Vector2(148f, -2.05f),
            sprite, material, "ApartmentKitchen", "ApartmentKitchenFromLivingRoom");
        SetString(kitchenDoor.GetComponent<DoorController>(), "doorGroup", "apartment-kitchen");
        CreateSpawn(root.transform, "ApartmentLivingRoomFromEntrance", new Vector2(0f, -3.85f),
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
        ApplyKitchenCheckerFloorArtwork(kitchen);
        Color wall = new Color(0.28f, 0.26f, 0.23f, 1f);
        CreateUnit(root.transform, "Kitchen Wall Top", new Vector2(160f, 3.75f), new Vector2(11f, .5f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Wall Bottom", new Vector2(160f, -3.75f), new Vector2(11f, .5f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Wall Right", new Vector2(165.25f, 0f), new Vector2(.5f, 8f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Wall Left Top", new Vector2(154.75f, 2.2f), new Vector2(.5f, 3.1f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Wall Left Bottom", new Vector2(154.75f, -2.2f), new Vector2(.5f, 3.1f), wall, 4, true, sprite, material);
        ApplyApartmentRoomWallArtwork(kitchen,
            new[] { "Kitchen Wall Top" },
            new[] { "Kitchen Wall Left Top", "Kitchen Wall Left Bottom" },
            new[] { "Kitchen Wall Right" },
            new[] { "Kitchen Wall Bottom" });
        CreateUnit(root.transform, "Kitchen Stove", new Vector2(157.1f, 2.8f), new Vector2(2.3f, .8f), new Color(.2f,.2f,.21f,1), 2, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Sink", new Vector2(160.2f, 2.8f), new Vector2(1.6f, .8f), new Color(.4f,.45f,.47f,1), 2, true, sprite, material);
        CreateUnit(root.transform, "Kitchen Refrigerator", new Vector2(163.8f, -2.2f), new Vector2(1.1f, 2f), new Color(.62f,.64f,.64f,1), 2, true, sprite, material);
        GameObject cabinet = CreateUnit(root.transform, "Locked Food Cabinet", new Vector2(163.6f, 2.55f), new Vector2(1.5f, 1.3f), new Color(.48f,.2f,.12f,1), 3, true, sprite, material);
        LockedFoodCabinet locked = cabinet.AddComponent<LockedFoodCabinet>();
        CreateInvestigationPoint(root.transform, "Locked Food Cabinet Investigation", cabinet.transform.position, new Vector2(1.8f,1.6f), InvestigationKind.KitchenFoodCabinet, locked);
        GameObject exit = CreateDoorWithPortal(root.transform, "Kitchen Exit Door", new Vector2(154.5f, 0f), sprite, material, "ApartmentLivingRoom", "ApartmentLivingRoomFromKitchen");
        SetString(exit.GetComponent<DoorController>(), "doorGroup", "apartment-kitchen");
        CreateSpawn(root.transform, "ApartmentKitchenFromLivingRoom", new Vector2(-4.3f, 0f), new Vector3(160f,0f,-10f));
        EditorSceneManager.SaveScene(kitchen, ApartmentKitchenScenePath);
        AddSceneToBuildSettings(ApartmentKitchenScenePath);
        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        CreateSpawn(FindInScene(livingRoom, "Apartment Living Room").transform, "ApartmentLivingRoomFromKitchen", new Vector2(6.8f, -2.05f), new Vector3(140f,0f,-10f));
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
        CreateSpawn(livingRoot.transform, "ApartmentLivingRoomFromBathroom", new Vector2(6.7f, 1.55f),
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
        ApplyBathroomCheckerFloorArtwork(bathroom);
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
        ApplyApartmentRoomWallArtwork(bathroom,
            new[] { "Bathroom Wall Top" },
            new[] { "Bathroom Wall Left Top", "Bathroom Wall Left Bottom" },
            new[] { "Bathroom Wall Right" },
            new[] { "Bathroom Wall Bottom" });

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
        CreateSpawn(root.transform, "ApartmentBathroomFromLivingRoom", new Vector2(-2.65f, 0f),
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
        CreateSpawn(livingRoot.transform, "ApartmentLivingRoomFromLinBedroom", new Vector2(4f, 3.85f),
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
        ApplyLinBedroomCarpetFloorArtwork(bedroom);
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
        ApplyApartmentRoomWallArtwork(bedroom,
            new[] { "Lin Bedroom Wall Top" },
            new[] { "Lin Bedroom Wall Left" },
            new[] { "Lin Bedroom Wall Right" },
            new[] { "Lin Bedroom Wall Bottom Left", "Lin Bedroom Wall Bottom Right" });

        CreateUnit(root.transform, "Lin Bedroom Bed", new Vector2(197.95f, -.94f), new Vector2(2.45f, 1.25f),
            new Color(.42f, .5f, .66f, 1f), 2, true, sprite, material);
        CreateUnit(root.transform, "Lin Bedroom Desk", new Vector2(202.15f, 1.5f), new Vector2(1.45f, .72f),
            new Color(.31f, .18f, .1f, 1f), 2, true, sprite, material);
        CreateUnit(root.transform, "Lin Bedroom Chair", new Vector2(202.15f, .55f), new Vector2(.7f, .65f),
            new Color(.24f, .16f, .12f, 1f), 2, true, sprite, material);
        CreateUnit(root.transform, "Lin Bedroom Wardrobe", new Vector2(197.98f, 1.32f), new Vector2(1.45f, 1.95f),
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

        CreateAutomaticTransitionPoint(root.transform, "Lin Bedroom Exit Doorway", new Vector2(200f, -2.5f),
            "ApartmentLivingRoom", "ApartmentLivingRoomFromLinBedroom");
        CreateSpawn(root.transform, "ApartmentLinBedroomFromLivingRoom", new Vector2(0f, -1.7f),
            new Vector3(200f, 0f, -10f));
        EditorSceneManager.SaveScene(bedroom, ApartmentLinChunbaoBedroomScenePath);
        AddSceneToBuildSettings(ApartmentLinChunbaoBedroomScenePath);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Sync Lin Chunbao Bedroom Furniture Layout")]
    public static void SyncLinChunbaoBedroomFurnitureLayout()
    {
        Scene bedroom = EditorSceneManager.OpenScene(ApartmentLinChunbaoBedroomScenePath,
            OpenSceneMode.Single);
        GameObject bed = FindInScene(bedroom, "Lin Bedroom Bed");
        GameObject wardrobe = FindInScene(bedroom, "Lin Bedroom Wardrobe");

        FindInScene(bedroom, "Lin Bedroom Bed Investigation").transform.position = bed.transform.position;
        FindInScene(bedroom, "Lin Bedroom Wardrobe Investigation").transform.position = wardrobe.transform.position;

        EditorSceneManager.SaveScene(bedroom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Protagonist Bedroom")]
    public static void InstallProtagonistBedroom()
    {
        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject livingRoot = FindInScene(livingRoom, "Apartment Living Room");
        GameObject livingDoor = FindInScene(livingRoom, "Protagonist Bedroom Door");
        DoorController livingDoorController = livingDoor.GetComponent<DoorController>();
        SetString(livingDoorController, "doorGroup", "protagonist-bedroom");
        GameObject livingPortal = FindOrCreateObject(livingDoor.transform,
            "Protagonist Bedroom Door Portal", Vector2.zero);
        livingPortal.transform.localPosition = new Vector3(0f, -.7f, 0f);
        BoxCollider2D portalCollider = livingPortal.GetComponent<BoxCollider2D>();
        if (portalCollider == null) portalCollider = livingPortal.AddComponent<BoxCollider2D>();
        portalCollider.isTrigger = true;
        portalCollider.size = new Vector2(1.5f, 1.3f);
        DoorExitPortal portal = livingPortal.GetComponent<DoorExitPortal>();
        if (portal == null) portal = livingPortal.AddComponent<DoorExitPortal>();
        SetReference(portal, "door", livingDoorController);
        SetString(portal, "destinationScene", "ApartmentProtagonistBedroom");
        SetString(portal, "destinationId", "ApartmentProtagonistBedroomFromLivingRoom");
        CreateSpawn(livingRoot.transform, "ApartmentLivingRoomFromProtagonistBedroom",
            new Vector2(-4f, 3.85f), new Vector3(140f, 0f, -10f));
        SpriteRenderer source = FindInScene(livingRoom, "Protagonist Bedroom Door Panel")
            .GetComponent<SpriteRenderer>();
        Sprite sprite = source.sprite;
        Material material = source.sharedMaterial;
        EditorSceneManager.SaveScene(livingRoom);

        Scene bedroom = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject root = new GameObject("Apartment Protagonist Bedroom");
        root.transform.position = new Vector3(220f, 0f, 0f);
        CreateUnit(root.transform, "Protagonist Bedroom Floor", new Vector2(220f, 0f),
            new Vector2(7f, 5f), new Color(.42f, .35f, .3f, 1f), -1, false, sprite, material);
        ApplyProtagonistBedroomCarpetFloorArtwork(bedroom);
        Color wall = new Color(.28f, .25f, .24f, 1f);
        CreateUnit(root.transform, "Protagonist Bedroom Wall Top", new Vector2(220f, 2.75f),
            new Vector2(8f, .5f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Protagonist Bedroom Wall Bottom Left", new Vector2(217.75f, -2.75f),
            new Vector2(3.5f, .5f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Protagonist Bedroom Wall Bottom Right", new Vector2(222.25f, -2.75f),
            new Vector2(3.5f, .5f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Protagonist Bedroom Wall Left", new Vector2(216.25f, 0f),
            new Vector2(.5f, 6f), wall, 4, true, sprite, material);
        CreateUnit(root.transform, "Protagonist Bedroom Wall Right", new Vector2(223.75f, 0f),
            new Vector2(.5f, 6f), wall, 4, true, sprite, material);
        ApplyApartmentRoomWallArtwork(bedroom,
            new[] { "Protagonist Bedroom Wall Top" },
            new[] { "Protagonist Bedroom Wall Left" },
            new[] { "Protagonist Bedroom Wall Right" },
            new[] { "Protagonist Bedroom Wall Bottom Left", "Protagonist Bedroom Wall Bottom Right" });

        CreateUnit(root.transform, "Protagonist Bedroom Bed", new Vector2(218.25f, 1.2f),
            new Vector2(2.55f, 1.35f), new Color(.38f, .45f, .59f, 1f), 2, true, sprite, material);
        GameObject bedsideTable = CreateUnit(root.transform, "Protagonist Bedroom Bedside Table", new Vector2(220.1f, 1.55f),
            new Vector2(.7f, .7f), new Color(.28f, .16f, .1f, 1f), 2, true, sprite, material);
        GameObject openDrawer = CreateUnit(bedsideTable.transform, "Protagonist Bedside Table Open Drawer",
            new Vector2(220.1f, .92f), new Vector2(.8f, .3f), new Color(.19f, .1f, .06f, 1f),
            3, false, sprite, material);
        GameObject note = CreateUnit(openDrawer.transform, "Protagonist Password Note 7",
            new Vector2(220.1f, .94f), new Vector2(.45f, .18f), Color.white, 4, false, sprite, material);
        GameObject number = new GameObject("Protagonist Password Number 7");
        number.transform.SetParent(note.transform, false);
        TextMesh numberText = number.AddComponent<TextMesh>();
        numberText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        numberText.text = "7";
        numberText.fontSize = 72;
        numberText.characterSize = .11f;
        numberText.anchor = TextAnchor.MiddleCenter;
        numberText.alignment = TextAlignment.Center;
        numberText.color = Color.black;
        numberText.GetComponent<MeshRenderer>().sortingOrder = 5;
        GameObject plasterBoxes = CreateUnit(root.transform, "Stacked Plaster Boxes", new Vector2(222.35f, -1.55f),
            new Vector2(.9f, 1.1f), new Color(.72f, .63f, .48f, 1f), 2, true, sprite, material);

        ConfigureProtagonistBedroomDialogue(FindInScene(bedroom, "Protagonist Bedroom Bed"),
            InvestigationKind.ProtagonistBedroomBed, new[]
            {
                new DialoguePage("主角", "我房间的陈设比春宝简单得多。毕竟照顾好她是我的责任。\n平时写作业我也会在餐桌上写。"),
                new DialoguePage("主角", "而且简单的房间整理起来也很方便。")
            }, root.transform, new Vector2(2.8f, 1.7f));
        ConfigureProtagonistBedroomDialogue(plasterBoxes, InvestigationKind.ProtagonistPlasterBoxes,
            new[]
            {
                new DialoguePage("主角", "这些是我一直以来用来掩盖脸上这痕迹的膏药剩下的包装。"),
                new DialoguePage("主角", "……"),
                new DialoguePage("主角", "……人们不接纳我，因为我是一半的恶魔。"),
                new DialoguePage("主角", "恶魔会因为我是一半的人类而和人一样不接纳我吗？"),
                new DialoguePage("主角", "所以有没有人能告诉我我到底是什么！")
            }, root.transform, new Vector2(1.4f, 1.5f));
        bedsideTable = FindInScene(bedroom, "Protagonist Bedroom Bedside Table");
        ProtagonistBedsideTable bedsidePuzzle = bedsideTable.GetComponent<ProtagonistBedsideTable>();
        if (bedsidePuzzle == null) bedsidePuzzle = bedsideTable.AddComponent<ProtagonistBedsideTable>();
        bedsidePuzzle.Configure(openDrawer);
        CreateInvestigationPoint(root.transform, "Protagonist Bedside Table Investigation",
            bedsideTable.transform.position, new Vector2(1.2f, 1.2f),
            InvestigationKind.ProtagonistBedsideTable, bedsidePuzzle);

        CreateAutomaticTransitionPoint(root.transform, "Protagonist Bedroom Exit Doorway",
            new Vector2(220f, -2.5f), "ApartmentLivingRoom",
            "ApartmentLivingRoomFromProtagonistBedroom");
        CreateSpawn(root.transform, "ApartmentProtagonistBedroomFromLivingRoom", new Vector2(220f, -1.7f),
            new Vector3(220f, 0f, -10f));
        EditorSceneManager.SaveScene(bedroom, ApartmentProtagonistBedroomScenePath);
        AddSceneToBuildSettings(ApartmentProtagonistBedroomScenePath);
        AssetDatabase.SaveAssets();
        ConfigureSpriteStateDoor(ApartmentLivingRoomScenePath, "Protagonist Bedroom Door");
    }

    [MenuItem("Tools/Devil RPG/Install Apartment Departure Preparation")]
    public static void InstallApartmentDeparturePreparation()
    {
        Scene kitchen = EditorSceneManager.OpenScene(ApartmentKitchenScenePath, OpenSceneMode.Single);
        GameObject kitchenRoot = FindInScene(kitchen, "Apartment Kitchen");
        GameObject cabinet = FindInScene(kitchen, "Locked Food Cabinet");
        SpriteRenderer source = cabinet.GetComponent<SpriteRenderer>();
        WorldItem driedMeat = CreateWorldItem(kitchenRoot.transform, "Dried Meat", new Vector2(163.3f, 1.95f),
            new Vector2(.75f, .35f), new Color(.48f, .16f, .08f, 1f), "肉干", true,
            source.sprite, source.sharedMaterial);
        LockedFoodCabinet foodCabinet = cabinet.GetComponent<LockedFoodCabinet>();
        foodCabinet.Configure(driedMeat);
        EditorSceneManager.SaveScene(kitchen);

        Scene livingRoom = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath, OpenSceneMode.Single);
        GameObject livingRoot = FindInScene(livingRoom, "Apartment Living Room");
        if (livingRoot.GetComponent<ApartmentDepartureDialogue>() == null)
        {
            livingRoot.AddComponent<ApartmentDepartureDialogue>();
        }
        EditorSceneManager.SaveScene(livingRoom);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Devil RPG/Install Road To Suburbs 1")]
    public static void InstallRoadToSuburbsOne()
    {
        Scene classroom = EditorSceneManager.OpenScene(ClassroomScenePath, OpenSceneMode.Single);
        SpriteRenderer enemySource = FindInScene(classroom, "Professor").GetComponent<SpriteRenderer>();
        Sprite enemySprite = enemySource.sprite;
        Material enemyMaterial = enemySource.sharedMaterial;
        Scene apartment = EditorSceneManager.OpenScene(ApartmentEntranceScenePath, OpenSceneMode.Single);
        GameObject apartmentRoot = FindInScene(apartment, "Apartment Entrance");
        if (apartmentRoot.GetComponent<ApartmentEntranceRightSideHint>() == null)
        {
            apartmentRoot.AddComponent<ApartmentEntranceRightSideHint>();
        }
        SpriteRenderer source = FindInScene(apartment, "Apartment Street").GetComponent<SpriteRenderer>();
        Sprite sprite = source.sprite;
        Material material = source.sharedMaterial;
        MapTransitionPoint apartmentTransition = FindOrCreateObject(apartmentRoot.transform,
            "Apartment Entrance Left Transition", new Vector2(111.5f, -3.3f))
            .GetComponent<MapTransitionPoint>();
        if (apartmentTransition == null)
        {
            apartmentTransition = FindInScene(apartment, "Apartment Entrance Left Transition")
                .AddComponent<MapTransitionPoint>();
        }
        apartmentTransition.Configure("RoadToSuburbs1", "RoadToSuburbs1FromApartment");
        CreateInvestigationPoint(apartmentRoot.transform, "Apartment Left Transition Investigation",
            new Vector2(111.5f, -3.3f), new Vector2(1.8f, 5.5f),
            InvestigationKind.SuburbanRoadTransition, apartmentTransition);
        EditorSceneManager.SaveScene(apartment);

        Scene road = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject roadRoot = new GameObject("通往郊区的路（1）");
        roadRoot.transform.position = new Vector3(240f, 0f, 0f);
        roadRoot.AddComponent<RoadToSuburbsOneIntro>();
        roadRoot.AddComponent<RoadToSuburbsOneEndpointHint>();
        CreateUnit(roadRoot.transform, "Suburbs Road Night Sky", new Vector2(240f, 0f),
            new Vector2(20f, 12f), new Color(.05f, .08f, .14f, 1f), -12, false,
            sprite, material);
        CreateUnit(roadRoot.transform, "Suburbs Road Pavement", new Vector2(240f, 1.55f),
            new Vector2(20f, 2f), new Color(.34f, .34f, .38f, 1f), 0, false,
            sprite, material);
        CreateUnit(roadRoot.transform, "Suburbs Road", new Vector2(240f, -1.35f),
            new Vector2(20f, 3.8f), new Color(.11f, .12f, .15f, 1f), 0, false,
            sprite, material);
        CreateUnit(roadRoot.transform, "Suburbs Road Lower Pavement", new Vector2(240f, -4.525f),
            new Vector2(20f, 2.55f), new Color(.34f, .34f, .38f, 1f), 0, false,
            sprite, material);
        CreateUnit(roadRoot.transform, "Suburbs Road Guardrail", new Vector2(240f, 2.2f),
            new Vector2(20f, .28f), new Color(.36f, .38f, .42f, 1f), 2, true,
            sprite, material);
        ApplySuburbRoadGuardrailArtwork(road);
        CreateRoadMazeWall(roadRoot.transform, "Road Maze Top Wall", new Vector2(240f, 2.55f),
            new Vector2(20f, .35f), sprite, material);
        CreateRoadMazeWall(roadRoot.transform, "Road Maze Bottom Wall", new Vector2(240f, -5.8f),
            new Vector2(20f, .35f), sprite, material);
        CreateRoadMazeWall(roadRoot.transform, "Road Maze Lower Divider Left", new Vector2(231.2f, -3.2f),
            new Vector2(2.4f, .35f), sprite, material);
        CreateRoadMazeWall(roadRoot.transform, "Road Maze Lower Divider Center", new Vector2(240f, -3.2f),
            new Vector2(10.8f, .35f), sprite, material);
        CreateRoadMazeWall(roadRoot.transform, "Road Maze Lower Divider Right", new Vector2(248.8f, -3.2f),
            new Vector2(2.4f, .35f), sprite, material);
        // 主角的 CircleCollider2D 直径为 1.0；所有可通行缝隙至少保留 1.25 单位。
        CreateRoadMazeWall(roadRoot.transform, "Road Maze Upper Divider Left", new Vector2(232.875f, -.3f),
            new Vector2(5.75f, .35f), sprite, material);
        CreateRoadMazeWall(roadRoot.transform, "Road Maze Upper Divider Center", new Vector2(242f, -.3f),
            new Vector2(10f, .35f), sprite, material);
        CreateRoadMazeWall(roadRoot.transform, "Road Maze Upper Divider Right", new Vector2(249.2f, -.3f),
            new Vector2(1.6f, .35f), sprite, material);
        ApplySuburbanMazeBarricadeArtwork(road);
        CreateGangDogMember(roadRoot.transform, "丧家犬低级成员 1", new Vector2(242.5f, -1.75f),
            new Vector2(239f, -2.75f), new Vector2(245.5f, -1f), enemySprite, enemyMaterial);
        CreateGangDogMember(roadRoot.transform, "丧家犬低级成员 2", new Vector2(235.2f, 1.1f),
            new Vector2(232.2f, .3f), new Vector2(239f, 1.9f), enemySprite, enemyMaterial);
        GameObject returnSign = CreateUnit(roadRoot.transform, "Return To Apartment Sign",
            new Vector2(248.7f, -4.5f), new Vector2(.32f, 2.2f), new Color(.28f, .62f, .8f, 1f),
            3, false, sprite, material);
        MapTransitionPoint returnTransition = returnSign.AddComponent<MapTransitionPoint>();
        returnTransition.Configure("ApartmentEntrance", "ApartmentEntranceFromRoadToSuburbs1");
        CreateInvestigationPoint(roadRoot.transform, "Road To Apartment Transition Investigation",
            returnSign.transform.position, new Vector2(1.8f, 4.5f),
            InvestigationKind.SuburbanRoadTransition, returnTransition);
        CreateSpawn(roadRoot.transform, "RoadToSuburbs1FromApartment", new Vector2(8f, -4.5f),
            new Vector3(240f, -1.5f, -10f));
        GameObject roadTwoTransition = CreateUnit(roadRoot.transform, "Road To Suburbs 2 Transition",
            new Vector2(231.3f, 1.2f), new Vector2(.32f, 2.2f), new Color(.28f, .62f, .8f, 1f),
            3, false, sprite, material);
        MapTransitionPoint roadTwoPoint = roadTwoTransition.AddComponent<MapTransitionPoint>();
        roadTwoPoint.Configure("RoadToSuburbs2", "RoadToSuburbs2FromRoadOne");
        CreateInvestigationPoint(roadRoot.transform, "Road To Suburbs 2 Transition Investigation",
            roadTwoTransition.transform.position, new Vector2(1.8f, 4.5f),
            InvestigationKind.SuburbanRoadTransition, roadTwoPoint);
        EditorSceneManager.SaveScene(road, RoadToSuburbsOneScenePath);
        AddSceneToBuildSettings(RoadToSuburbsOneScenePath);

        Scene roadTwo = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject roadTwoRoot = new GameObject("通往郊区的路（2）");
        roadTwoRoot.transform.position = new Vector3(270f, 0f, 0f);
        roadTwoRoot.AddComponent<RoadToSuburbsTwoIntro>();
        roadTwoRoot.AddComponent<RoadToSuburbsTwoEndpointHint>();
        CreateUnit(roadTwoRoot.transform, "Suburbs Road 2 Night Sky", new Vector2(270f, 0f),
            new Vector2(20f, 12f), new Color(.045f, .07f, .12f, 1f), -12, false, sprite, material);
        CreateUnit(roadTwoRoot.transform, "Suburbs Road 2 Pavement", new Vector2(270f, 1.55f),
            new Vector2(20f, 2f), new Color(.31f, .31f, .35f, 1f), 0, false, sprite, material);
        CreateUnit(roadTwoRoot.transform, "Suburbs Road 2", new Vector2(270f, -1.35f),
            new Vector2(20f, 3.8f), new Color(.1f, .11f, .14f, 1f), 0, false, sprite, material);
        CreateUnit(roadTwoRoot.transform, "Suburbs Road 2 Lower Pavement", new Vector2(270f, -4.525f),
            new Vector2(20f, 2.55f), new Color(.31f, .31f, .35f, 1f), 0, false, sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Top Wall", new Vector2(270f, 2.55f),
            new Vector2(20f, .35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Bottom Wall", new Vector2(270f, -5.8f),
            new Vector2(20f, .35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Upper Divider Left", new Vector2(261.5f, 0f),
            new Vector2(2f, .35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Upper Divider Mid Left", new Vector2(265.225f, 0f),
            new Vector2(3.05f, .35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Upper Divider Mid", new Vector2(269.5f, 0f),
            new Vector2(3f, .35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Upper Divider Right", new Vector2(274.5f, 0f),
            new Vector2(4f, .35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Upper Divider End", new Vector2(279f, 0f),
            new Vector2(1f, .35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Lower Divider Left", new Vector2(261.2f, -3f),
            new Vector2(1.4f, .35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Lower Divider Mid Left", new Vector2(264.6f, -3f),
            new Vector2(2.8f, .35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Lower Divider Mid", new Vector2(269f, -3f),
            new Vector2(3.5f, .35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Lower Divider Mid Right", new Vector2(273.8f, -3f),
            new Vector2(3.6f, .35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Lower Divider Right", new Vector2(278.8f, -3f),
            new Vector2(1.4f, .35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Vertical Block A", new Vector2(276.2f, 1.25f),
            new Vector2(.35f, 2.5f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Vertical Block B", new Vector2(272.2f, -.675f),
            new Vector2(.35f, 1.75f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Vertical Block C", new Vector2(268.2f, -5.125f),
            new Vector2(.35f, 1.35f), sprite, material);
        CreateRoadMazeWall(roadTwoRoot.transform, "Road 2 Vertical Block D", new Vector2(264.2f, 1.3f),
            new Vector2(.35f, 2.5f), sprite, material);
        ApplySuburbanMazeBarricadeArtwork(roadTwo);
        CreateGangDogAdvancedMember(roadTwoRoot.transform, "丧家犬高级成员 1", new Vector2(275f, -1.4f),
            new Vector2(273.2f, -2.65f), new Vector2(277.8f, -.35f), enemySprite, enemyMaterial);
        CreateGangDogAdvancedMember(roadTwoRoot.transform, "丧家犬高级成员 2", new Vector2(269.6f, 1.2f),
            new Vector2(266.2f, .35f), new Vector2(273.8f, 1.9f), enemySprite, enemyMaterial);
        CreateGangDogAdvancedMember(roadTwoRoot.transform, "丧家犬高级成员 3", new Vector2(264.8f, -4.25f),
            new Vector2(262f, -5.25f), new Vector2(267.5f, -3.35f), enemySprite, enemyMaterial);
        GameObject rubbleTransition = CreateUnit(roadTwoRoot.transform, "Rubble Pile Transition",
            new Vector2(261.2f, -5.05f), new Vector2(1.3f, .7f), new Color(.5f, .38f, .26f, 1f),
            3, false, sprite, material);
        MapTransitionPoint rubblePoint = rubbleTransition.AddComponent<MapTransitionPoint>();
        rubblePoint.Configure("RubblePile", "RubblePileFromRoadTwo");
        CreateInvestigationPoint(roadTwoRoot.transform, "Rubble Pile Transition Investigation",
            rubbleTransition.transform.position, new Vector2(1.8f, 2f),
            InvestigationKind.SuburbanRoadTransition, rubblePoint);
        GameObject returnToRoadOne = CreateUnit(roadTwoRoot.transform, "Return To Road 1 Sign",
            new Vector2(278.7f, 1.3f), new Vector2(.32f, 2.2f), new Color(.28f, .62f, .8f, 1f),
            3, false, sprite, material);
        MapTransitionPoint roadOnePoint = returnToRoadOne.AddComponent<MapTransitionPoint>();
        roadOnePoint.Configure("RoadToSuburbs1", "RoadToSuburbs1FromRoadTwo");
        CreateInvestigationPoint(roadTwoRoot.transform, "Road To Suburbs 1 Transition Investigation",
            returnToRoadOne.transform.position, new Vector2(1.8f, 4.5f),
            InvestigationKind.SuburbanRoadTransition, roadOnePoint);
        CreateSpawn(roadTwoRoot.transform, "RoadToSuburbs2FromRoadOne", new Vector2(7.6f, 1.35f),
            new Vector3(270f, -1.5f, -10f));
        EditorSceneManager.SaveScene(roadTwo, RoadToSuburbsTwoScenePath);
        AddSceneToBuildSettings(RoadToSuburbsTwoScenePath);

        Scene rubble = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject rubbleRoot = new GameObject("瓦砾堆");
        rubbleRoot.transform.position = new Vector3(300f, 0f, 0f);
        CreateUnit(rubbleRoot.transform, "Rubble Night Sky", new Vector2(300f, 0f),
            new Vector2(20f, 12f), new Color(.075f, .065f, .055f, 1f), -12, false, sprite, material);
        CreateUnit(rubbleRoot.transform, "Rubble Ground", new Vector2(300f, -1.6f),
            new Vector2(20f, 8.4f), new Color(.22f, .19f, .16f, 1f), -11, false, sprite, material);
        ApplyRubblePileGroundArtwork(rubble);
        CreateRoadMazeWall(rubbleRoot.transform, "Rubble Top Wall", new Vector2(300f, 2.55f),
            new Vector2(20f, .35f), sprite, material);
        CreateRoadMazeWall(rubbleRoot.transform, "Rubble Bottom Wall", new Vector2(300f, -5.8f),
            new Vector2(20f, .35f), sprite, material);
        CreateRoadMazeWall(rubbleRoot.transform, "Rubble Upper Wall Left", new Vector2(294f, .15f),
            new Vector2(7.5f, .35f), sprite, material);
        CreateRoadMazeWall(rubbleRoot.transform, "Rubble Upper Wall Right", new Vector2(306f, .15f),
            new Vector2(7.5f, .35f), sprite, material);
        CreateRoadMazeWall(rubbleRoot.transform, "Rubble Lower Wall Left", new Vector2(294f, -3.25f),
            new Vector2(7.5f, .35f), sprite, material);
        CreateRoadMazeWall(rubbleRoot.transform, "Rubble Lower Wall Right", new Vector2(306f, -3.25f),
            new Vector2(7.5f, .35f), sprite, material);
        CreateRubbleLoreNotePassageWalls(rubbleRoot.transform, sprite, material, false);
        CreateRoadMazeWall(rubbleRoot.transform, "Rubble Vertical Wall North", new Vector2(290.8f, 1.25f),
            new Vector2(.35f, 2.2f), sprite, material);
        CreateRoadMazeWall(rubbleRoot.transform, "Rubble Vertical Wall South", new Vector2(309.2f, -4.35f),
            new Vector2(.35f, 2.2f), sprite, material);
        ApplyRubblePileBarricadeArtwork(rubble);
        CreateUnit(rubbleRoot.transform, "受伤的小恶魔", InjuredDemonIntroPosition,
            new Vector2(.75f, .45f), new Color(.24f, .48f, .3f, 1f), 3, false, sprite, material);
        ApplyRubbleInjuredDemonArtwork(rubble);
        CreateUnit(rubbleRoot.transform, "遗落的电棍", DroppedBatonIntroPosition,
            new Vector2(.16f, .7f), new Color(.82f, .76f, .26f, 1f), 3, false, sprite, material);
        GameObject gate = CreateUnit(rubbleRoot.transform, "锈蚀铁门", new Vector2(300f, -1.55f),
            new Vector2(.35f, 2.25f), new Color(.34f, .2f, .12f, 1f), 3, true, sprite, material);
        MapTransitionPoint abyssEntrancePoint = gate.AddComponent<MapTransitionPoint>();
        abyssEntrancePoint.Configure("AbyssEntrance", "AbyssEntranceFromRubblePile");
        abyssEntrancePoint.enabled = false;
        RubbleSwitchPuzzle rubblePuzzle = rubbleRoot.AddComponent<RubbleSwitchPuzzle>();
        RubbleSwitch[] rubbleSwitches =
        {
            CreateRubbleSwitch(rubbleRoot.transform, "瓦砾开关 左上", new Vector2(291.6f, 1.55f), sprite, material),
            CreateRubbleSwitch(rubbleRoot.transform, "瓦砾开关 右上", new Vector2(308.4f, 1.55f), sprite, material),
            CreateRubbleSwitch(rubbleRoot.transform, "瓦砾开关 左下", new Vector2(291.6f, -4.8f), sprite, material),
            CreateRubbleSwitch(rubbleRoot.transform, "瓦砾开关 右下", new Vector2(308.4f, -4.8f), sprite, material)
        };
        rubblePuzzle.Configure(gate.GetComponent<Collider2D>(), gate.GetComponent<SpriteRenderer>(), rubbleSwitches);
        GameObject boss = CreateUnit(rubbleRoot.transform, "丧家犬老大", new Vector2(300f, -5f),
            new Vector2(1.25f, 1.25f), new Color(.22f, .82f, .32f, 1f), 5, false, enemySprite, enemyMaterial);
        CircleCollider2D bossCollider = boss.AddComponent<CircleCollider2D>();
        bossCollider.radius = .5f;
        GangDogBoss bossBehavior = boss.AddComponent<GangDogBoss>();
        RubbleBossEncounter bossEncounter = rubbleRoot.AddComponent<RubbleBossEncounter>();
        bossEncounter.Configure(bossBehavior, rubblePuzzle, new Vector2(297f, -5f),
            new Vector2(297f, -5f), new Vector2(300f, -5f));
        CreateRubbleLoreNote(rubbleRoot.transform, "学生缴费单", new Vector2(292.2f, -1.8f),
            true, bossEncounter, sprite, material);
        CreateRubbleLoreNote(rubbleRoot.transform, "欠款条", new Vector2(307.8f, -1.8f),
            false, bossEncounter, sprite, material);
        boss.SetActive(false);
        CreateSpawn(rubbleRoot.transform, "RubblePileFromRoadTwo", RubbleSpawnPosition,
            new Vector3(300f, -1.5f, -10f));
        EditorSceneManager.SaveScene(rubble, RubblePileScenePath);
        AddSceneToBuildSettings(RubblePileScenePath);

        Scene abyssEntrance = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject abyssRoot = new GameObject("归墟入口");
        abyssRoot.transform.position = new Vector3(330f, 0f, 0f);
        abyssRoot.AddComponent<AbyssEntranceDropIn>();
        CreateUnit(abyssRoot.transform, "Abyss Entrance Night", new Vector2(330f, 0f),
            new Vector2(20f, 12f), new Color(.025f, .035f, .06f, 1f), -10, false, sprite, material);
        CreateUnit(abyssRoot.transform, "湿润的洞穴泥土", new Vector2(330f, -2.1f),
            new Vector2(19.4f, 7.5f), new Color(.13f, .1f, .14f, 1f), 0, false, sprite, material);
        ApplyAbyssEntranceCaveMudFloorArtwork(abyssEntrance);
        CreateUnit(abyssRoot.transform, "洞顶岩壁", new Vector2(330f, 2.5f),
            new Vector2(20f, 1.1f), new Color(.12f, .15f, .19f, 1f), 2, true, sprite, material);
        CreateUnit(abyssRoot.transform, "洞底岩壁", new Vector2(330f, -5.65f),
            new Vector2(20f, .7f), new Color(.1f, .12f, .15f, 1f), 2, true, sprite, material);
        CreateUnit(abyssRoot.transform, "左侧岩壁", new Vector2(320.3f, -1.55f),
            new Vector2(.7f, 8.2f), new Color(.12f, .15f, .19f, 1f), 2, true, sprite, material);
        CreateUnit(abyssRoot.transform, "右侧岩壁", new Vector2(339.7f, -1.55f),
            new Vector2(.7f, 8.2f), new Color(.12f, .15f, .19f, 1f), 2, true, sprite, material);
        ApplyAbyssEntranceCaveWallArtwork(abyssEntrance);
        CreateUnit(abyssRoot.transform, "岩壁突起一", new Vector2(324.5f, .85f),
            new Vector2(3.3f, .75f), new Color(.16f, .18f, .22f, 1f), 2, true, sprite, material);
        CreateUnit(abyssRoot.transform, "岩壁突起二", new Vector2(334.9f, -3.65f),
            new Vector2(3.8f, .8f), new Color(.15f, .17f, .2f, 1f), 2, true, sprite, material);
        Sprite rockProtrusionArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceRockProtrusionArtworkPath);
        if (rockProtrusionArtwork != null)
        {
            ApplyAbyssEntranceRockProtrusionArtwork(abyssEntrance, rockProtrusionArtwork);
        }
        CreateUnit(abyssRoot.transform, "荧光苔藓 左上", new Vector2(323.1f, 1.95f),
            new Vector2(3.4f, .3f), new Color(.12f, .67f, .9f, .9f), 3, false, sprite, material);
        CreateUnit(abyssRoot.transform, "荧光苔藓 右上", new Vector2(335.8f, 1.8f),
            new Vector2(4.6f, .35f), new Color(.11f, .55f, .85f, .88f), 3, false, sprite, material);
        CreateUnit(abyssRoot.transform, "荧光苔藓 左壁", new Vector2(320.7f, -1.1f),
            new Vector2(.18f, 3.2f), new Color(.1f, .62f, .9f, .88f), 3, false, sprite, material);
        CreateUnit(abyssRoot.transform, "荧光苔藓 右壁", new Vector2(339.3f, -2.4f),
            new Vector2(.18f, 2.8f), new Color(.13f, .6f, .92f, .88f), 3, false, sprite, material);
        Sprite rockyMossArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceRockyMossArtworkPath);
        Sprite mossArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceGlowingMossArtworkPath);
        if (rockyMossArtwork != null && mossArtwork != null)
        {
            ApplyAbyssEntranceGlowingMossArtwork(abyssEntrance, rockyMossArtwork, mossArtwork);
        }
        GameObject crystalOne = CreateUnit(abyssRoot.transform, "温热晶体 一", new Vector2(326.7f, -3.95f),
            new Vector2(.38f, .82f), new Color(.55f, .82f, 1f, .72f), 4, false, sprite, material);
        crystalOne.transform.rotation = Quaternion.Euler(0f, 0f, -18f);
        GameObject crystalTwo = CreateUnit(abyssRoot.transform, "温热晶体 二", new Vector2(327.25f, -3.9f),
            new Vector2(.25f, .56f), new Color(.46f, .75f, 1f, .68f), 4, false, sprite, material);
        crystalTwo.transform.rotation = Quaternion.Euler(0f, 0f, 23f);
        GameObject crystalThree = CreateUnit(abyssRoot.transform, "温热晶体 三", new Vector2(334.2f, -.85f),
            new Vector2(.35f, .78f), new Color(.62f, .84f, 1f, .72f), 4, false, sprite, material);
        crystalThree.transform.rotation = Quaternion.Euler(0f, 0f, 16f);
        Sprite warmCrystalArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceWarmCrystalClusterArtworkPath);
        if (warmCrystalArtwork != null)
        {
            ApplyAbyssEntranceWarmCrystalArtwork(abyssEntrance, warmCrystalArtwork);
        }
        GameObject mushroomTransition = CreateUnit(abyssRoot.transform, "荧光菌洞通道", new Vector2(338.5f, -1.6f),
            new Vector2(.6f, 2.1f), new Color(.12f, .55f, .78f, 1f), 3, false, sprite, material);
        AbyssEntranceInteraction abyssInteraction = abyssRoot.AddComponent<AbyssEntranceInteraction>();
        CreateAbyssEntranceInvestigationPoints(abyssRoot.transform, abyssInteraction);
        Sprite sideCavePassageArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(AbyssEntranceSideCavePassageArtworkPath);
        if (sideCavePassageArtwork != null)
        {
            ApplyAbyssEntranceSideCavePassageArtwork(abyssEntrance, sideCavePassageArtwork);
        }
        CreateSpawn(abyssRoot.transform, "AbyssEntranceFromRubblePile", new Vector2(0f, -2.1f),
            new Vector3(330f, -1.5f, -10f));
        EditorSceneManager.SaveScene(abyssEntrance, AbyssEntranceScenePath);
        AddSceneToBuildSettings(AbyssEntranceScenePath);

        Scene mushroomCave = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject mushroomRoot = new GameObject("荧光菌洞");
        mushroomRoot.transform.position = new Vector3(360f, 0f, 0f);
        CreateUnit(mushroomRoot.transform, "荧光菌洞背景", new Vector2(360f, 0f),
            new Vector2(20f, 12f), new Color(.02f, .045f, .07f, 1f), -10, false, sprite, material);
        CreateUnit(mushroomRoot.transform, "荧光菌洞地面", new Vector2(360f, -2f),
            new Vector2(20f, 7.7f), new Color(.1f, .12f, .14f, 1f), 0, false, sprite, material);
        CreateRoadMazeWall(mushroomRoot.transform, "菌洞顶岩壁", new Vector2(360f, 2.5f),
            new Vector2(20f, .65f), sprite, material);
        CreateRoadMazeWall(mushroomRoot.transform, "菌洞底岩壁", new Vector2(361.6f, -5.65f),
            new Vector2(16.2f, .65f), sprite, material);
        CreateRoadMazeWall(mushroomRoot.transform, "菌洞左岩壁", new Vector2(350.3f, -1.55f),
            new Vector2(.65f, 8.1f), sprite, material);
        CreateRoadMazeWall(mushroomRoot.transform, "菌洞右岩壁", new Vector2(369.7f, -1.55f),
            new Vector2(.65f, 8.1f), sprite, material);
        CreateUnit(mushroomRoot.transform, "最大荧光蘑菇丛", new Vector2(365f, -.9f),
            new Vector2(2.6f, 1.5f), new Color(.13f, .58f, .91f, .9f), 2, false, sprite, material);
        CreateUnit(mushroomRoot.transform, "荧光蘑菇丛 左", new Vector2(355.1f, -3.55f),
            new Vector2(2.1f, .72f), new Color(.1f, .43f, .75f, .85f), 2, false, sprite, material);
        CreateUnit(mushroomRoot.transform, "荧光蘑菇丛 右", new Vector2(367.2f, -3.9f),
            new Vector2(1.9f, .7f), new Color(.12f, .47f, .78f, .85f), 2, false, sprite, material);
        GameObject thirteen = CreateUnit(mushroomRoot.transform, "？？？", new Vector2(365f, -.55f),
            new Vector2(.72f, .72f), Color.black, 5, false, sprite, material);
        GameObject giftCrystal = CreateUnit(mushroomRoot.transform, "十三的晶石礼物", new Vector2(363.7f, -1.7f),
            new Vector2(.68f, .68f), new Color(.62f, .85f, 1f, .78f), 5, false, sprite, material);
        ThirteenEncounter thirteenEncounter = mushroomRoot.AddComponent<ThirteenEncounter>();
        thirteenEncounter.Configure(thirteen.transform, giftCrystal, new Vector2(363.9f, -1.1f),
            new Vector2(363.2f, -1.35f));
        CreateSpawn(mushroomRoot.transform, "FluorescentMushroomCaveFromAbyss", new Vector2(-8.5f, -2f),
            new Vector3(360f, -1.5f, -10f));
        EditorSceneManager.SaveScene(mushroomCave, FluorescentMushroomCaveScenePath);
        AddSceneToBuildSettings(FluorescentMushroomCaveScenePath);

        Scene roadHome = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject roadHomeRoot = new GameObject("回家的路");
        roadHomeRoot.transform.position = new Vector3(390f, 0f, 0f);
        RoadHomeJourney journey = roadHomeRoot.AddComponent<RoadHomeJourney>();
        journey.Configure(398.5f, 381.2f);
        CreateUnit(roadHomeRoot.transform, "回家路夜空", new Vector2(390f, 0f),
            new Vector2(20f, 12f), new Color(.035f, .045f, .075f, 1f), -11, false, sprite, material);
        CreateUnit(roadHomeRoot.transform, "人行道", new Vector2(390f, .2f),
            new Vector2(20f, 3.4f), new Color(.42f, .42f, .44f, 1f), 0, false, sprite, material);
        CreateUnit(roadHomeRoot.transform, "马路", new Vector2(390f, -3.15f),
            new Vector2(20f, 3.4f), new Color(.13f, .14f, .17f, 1f), 0, false, sprite, material);
        CreateUnit(roadHomeRoot.transform, "路缘", new Vector2(390f, -1.5f),
            new Vector2(20f, .18f), new Color(.78f, .75f, .62f, 1f), 2, false, sprite, material);
        CreateUnit(roadHomeRoot.transform, "道路标线 一", new Vector2(394.5f, -3.15f),
            new Vector2(2f, .12f), new Color(.86f, .8f, .46f, 1f), 2, false, sprite, material);
        CreateUnit(roadHomeRoot.transform, "道路标线 二", new Vector2(387.5f, -3.15f),
            new Vector2(2f, .12f), new Color(.86f, .8f, .46f, 1f), 2, false, sprite, material);
        CreateUnit(roadHomeRoot.transform, "道路标线 三", new Vector2(382.5f, -3.15f),
            new Vector2(1.3f, .12f), new Color(.86f, .8f, .46f, 1f), 2, false, sprite, material);
        CreateUnit(roadHomeRoot.transform, "路灯 一", new Vector2(396f, .95f),
            new Vector2(.16f, 2.7f), new Color(.38f, .4f, .45f, 1f), 2, false, sprite, material);
        CreateUnit(roadHomeRoot.transform, "路灯 二", new Vector2(389f, .95f),
            new Vector2(.16f, 2.7f), new Color(.38f, .4f, .45f, 1f), 2, false, sprite, material);
        CreateUnit(roadHomeRoot.transform, "路灯 三", new Vector2(382f, .95f),
            new Vector2(.16f, 2.7f), new Color(.38f, .4f, .45f, 1f), 2, false, sprite, material);
        GameObject thirteenFollower = CreateUnit(roadHomeRoot.transform, "十三", new Vector2(397.2f, -.7f),
            new Vector2(.72f, .72f), Color.black, 5, false, sprite, material);
        thirteenFollower.AddComponent<ThirteenFollower>();
        GameObject homeExit = CreateUnit(roadHomeRoot.transform, "回到公寓外", new Vector2(380.8f, -.3f),
            new Vector2(.5f, 2.2f), new Color(.28f, .62f, .8f, 1f), 3, false, sprite, material);
        MapTransitionPoint homePoint = homeExit.AddComponent<MapTransitionPoint>();
        homePoint.Configure("ApartmentEntrance", "ApartmentEntranceFromRoadHome");
        CreateSpawn(roadHomeRoot.transform, "RoadHomeFromMushroomCave", new Vector2(8.5f, -.3f),
            new Vector3(390f, -1.5f, -10f));
        Sprite roadHomeStreetlightSprite = AssetDatabase.LoadAssetAtPath<Sprite>(RoadHomeStreetlightArtworkPath);
        if (roadHomeStreetlightSprite == null)
        {
            throw new InvalidOperationException("回家路路灯美术素材导入失败。");
        }
        ApplyRoadHomeStreetlightArtwork(roadHome, roadHomeStreetlightSprite);
        ApplyRoadHomeCustomRoadArtwork(roadHome);
        EditorSceneManager.SaveScene(roadHome, RoadHomeScenePath);
        AddSceneToBuildSettings(RoadHomeScenePath);

        apartment = EditorSceneManager.OpenScene(ApartmentEntranceScenePath, OpenSceneMode.Single);
        CreateSpawn(FindInScene(apartment, "Apartment Entrance").transform,
            "ApartmentEntranceFromRoadToSuburbs1", new Vector2(112.8f, -3.3f),
            new Vector3(120f, -1.5f, -10f));
        CreateSpawn(FindInScene(apartment, "Apartment Entrance").transform,
            "ApartmentEntranceFromRoadHome", new Vector2(129.5f, -3.3f),
            new Vector3(120f, -1.5f, -10f));
        EditorSceneManager.SaveScene(apartment);
        Scene livingForConfrontation = EditorSceneManager.OpenScene(ApartmentLivingRoomScenePath,
            OpenSceneMode.Single);
        GameObject livingRootForConfrontation = FindInScene(livingForConfrontation, "Apartment Living Room");
        GameObject confrontationLin = FindOptionalInScene(livingForConfrontation, "Lin Chunbao Confrontation")
            ?? CreateUnit(livingRootForConfrontation.transform, "Lin Chunbao Confrontation",
                new Vector2(140f, 1.25f), Vector2.one, new Color(.94f, .56f, .56f, 1f), 5, false,
                enemySprite, enemyMaterial);
        GameObject officialOne = FindOptionalInScene(livingForConfrontation, "猎魔机构行政人员 1")
            ?? CreateUnit(livingRootForConfrontation.transform, "猎魔机构行政人员 1",
                new Vector2(139.15f, -.15f), Vector2.one, new Color(.62f, .62f, .64f, 1f), 5, false,
                enemySprite, enemyMaterial);
        GameObject officialTwo = FindOptionalInScene(livingForConfrontation, "猎魔机构行政人员 2")
            ?? CreateUnit(livingRootForConfrontation.transform, "猎魔机构行政人员 2",
                new Vector2(140.85f, -.15f), Vector2.one, new Color(.62f, .62f, .64f, 1f), 5, false,
                enemySprite, enemyMaterial);
        GameObject apartmentThirteen = FindOptionalInScene(livingForConfrontation, "十三（客厅）")
            ?? CreateUnit(livingRootForConfrontation.transform, "十三（客厅）", new Vector2(142.8f, -.7f),
                new Vector2(.72f, .72f), Color.black, 5, false, enemySprite, enemyMaterial);
        confrontationLin.transform.position = new Vector2(140f, 1.25f);
        officialOne.transform.position = new Vector2(139.15f, -.15f);
        officialTwo.transform.position = new Vector2(140.85f, -.15f);
        apartmentThirteen.transform.position = new Vector2(142.8f, -.7f);
        if (apartmentThirteen.GetComponent<ThirteenFollower>() == null)
        {
            apartmentThirteen.AddComponent<ThirteenFollower>();
        }
        ApartmentConfrontation confrontation = livingRootForConfrontation.GetComponent<ApartmentConfrontation>();
        if (confrontation == null) confrontation = livingRootForConfrontation.AddComponent<ApartmentConfrontation>();
        confrontation.Configure(confrontationLin, officialOne, officialTwo, apartmentThirteen);
        confrontationLin.SetActive(false);
        officialOne.SetActive(false);
        officialTwo.SetActive(false);
        apartmentThirteen.SetActive(false);
        EditorSceneManager.SaveScene(livingForConfrontation);
        road = EditorSceneManager.OpenScene(RoadToSuburbsOneScenePath, OpenSceneMode.Single);
        CreateSpawn(FindInScene(road, "通往郊区的路（1）").transform,
            "RoadToSuburbs1FromRoadTwo", new Vector2(-7.7f, 1.2f),
            new Vector3(240f, -1.5f, -10f));
        EditorSceneManager.SaveScene(road);
        AssetDatabase.SaveAssets();
    }

    private static void CreateAbyssEntranceInvestigationPoints(Transform root,
        AbyssEntranceInteraction interaction)
    {
        foreach (string mossName in new[]
                 {
                     "荧光苔藓 左上", "荧光苔藓 右上", "荧光苔藓 左壁", "荧光苔藓 右壁"
                 })
        {
            Transform moss = FindInScene(SceneManager.GetActiveScene(), mossName).transform;
            CreateInvestigationPoint(root, mossName + " 调查", moss.position,
                new Vector2(1.5f, 1.2f), InvestigationKind.AbyssGlowingMoss, interaction);
        }

        foreach (string crystalName in new[] { "温热晶体 一", "温热晶体 二", "温热晶体 三" })
        {
            Transform crystal = FindInScene(SceneManager.GetActiveScene(), crystalName).transform;
            CreateInvestigationPoint(root, crystalName + " 调查", crystal.position,
                new Vector2(1.2f, 1.2f), InvestigationKind.AbyssWarmCrystal, interaction);
        }

        GameObject passage = FindInScene(SceneManager.GetActiveScene(), "荧光菌洞通道");
        MapTransitionPoint automaticTransition = passage.GetComponent<MapTransitionPoint>();
        if (automaticTransition != null) UnityEngine.Object.DestroyImmediate(automaticTransition);
        CreateInvestigationPoint(root, "荧光菌洞通道 调查", passage.transform.position,
            new Vector2(1.4f, 2.4f), InvestigationKind.AbyssCavePassage, interaction);
    }

    private static void CreateRoadMazeWall(Transform parent, string name, Vector2 position,
        Vector2 scale, Sprite sprite, Material material)
    {
        CreateUnit(parent, name, position, scale, new Color(.19f, .22f, .25f, 1f), 2, true,
            sprite, material);
    }

    private static void CreateRubbleLoreNotePassageWalls(Transform parent, Sprite sprite,
        Material material, bool replaceExisting)
    {
        string[] wallNames =
        {
            "Rubble Vertical Wall West", "Rubble Vertical Wall East",
            "Rubble Vertical Wall West Upper", "Rubble Vertical Wall West Lower",
            "Rubble Vertical Wall East Upper", "Rubble Vertical Wall East Lower"
        };
        if (replaceExisting)
        {
            foreach (string wallName in wallNames)
            {
                Transform wall = parent.Find(wallName);
                if (wall != null) UnityEngine.Object.DestroyImmediate(wall.gameObject);
            }
        }

        CreateRoadMazeWall(parent, "Rubble Vertical Wall West Upper", new Vector2(296.2f, -.525f),
            new Vector2(.35f, .65f), sprite, material);
        CreateRoadMazeWall(parent, "Rubble Vertical Wall West Lower", new Vector2(296.2f, -2.575f),
            new Vector2(.35f, .65f), sprite, material);
        CreateRoadMazeWall(parent, "Rubble Vertical Wall East Upper", new Vector2(303.8f, -.525f),
            new Vector2(.35f, .65f), sprite, material);
        CreateRoadMazeWall(parent, "Rubble Vertical Wall East Lower", new Vector2(303.8f, -2.575f),
            new Vector2(.35f, .65f), sprite, material);
    }

    private static RubbleSwitch CreateRubbleSwitch(Transform parent, string name, Vector2 position,
        Sprite sprite, Material material)
    {
        GameObject switchObject = CreateUnit(parent, name, position, new Vector2(.65f, .65f),
            new Color(.82f, .32f, .12f, 1f), 4, false, sprite, material);
        RubbleSwitch rubbleSwitch = switchObject.AddComponent<RubbleSwitch>();
        rubbleSwitch.Configure(null, switchObject.GetComponent<SpriteRenderer>());
        CreateInvestigationPoint(parent, name + " Investigation", position, new Vector2(1.4f, 1.4f),
            InvestigationKind.RubbleSwitch, rubbleSwitch);
        return rubbleSwitch;
    }

    private static void CreateRubbleLoreNote(Transform parent, string name, Vector2 position,
        bool isTuitionReceipt, RubbleBossEncounter encounter, Sprite sprite, Material material)
    {
        GameObject note = CreateUnit(parent, name, position, new Vector2(.72f, .48f),
            new Color(.88f, .86f, .76f, 1f), 4, false, sprite, material);
        RubbleLoreNote handler = note.AddComponent<RubbleLoreNote>();
        handler.Configure(encounter, isTuitionReceipt);
        CreateInvestigationPoint(parent, name + " Investigation", position, new Vector2(1.4f, 1.4f),
            InvestigationKind.RubbleLoreNote, handler);
    }

    private static void CreateGangDogMember(Transform parent, string name, Vector2 position,
        Vector2 patrolMin, Vector2 patrolMax, Sprite sprite, Material material)
    {
        GameObject member = CreateUnit(parent, name, position, Vector2.one,
            new Color(.2f, .48f, .88f, 1f), 4, false, sprite, material);
        CircleCollider2D collider = member.AddComponent<CircleCollider2D>();
        collider.radius = .48f;
        GangDogLowMember behavior = member.AddComponent<GangDogLowMember>();
        behavior.Configure(patrolMin, patrolMax);
    }

    private static void CreateGangDogAdvancedMember(Transform parent, string name, Vector2 position,
        Vector2 patrolMin, Vector2 patrolMax, Sprite sprite, Material material)
    {
        GameObject member = CreateUnit(parent, name, position, Vector2.one,
            new Color(.62f, .3f, .82f, 1f), 4, false, sprite, material);
        CircleCollider2D collider = member.AddComponent<CircleCollider2D>();
        collider.radius = .48f;
        GangDogAdvancedMember behavior = member.AddComponent<GangDogAdvancedMember>();
        behavior.Configure(patrolMin, patrolMax, "RoadToSuburbs2FromRoadOne");
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

        // 右侧墙改为上下两段，中央保留真实的门洞，而不是以门图覆盖整面墙。
        GameObject oldRightWall = FindInScene(corridor, "Corridor Wall Right");
        SpriteRenderer oldRightWallRenderer = oldRightWall.GetComponent<SpriteRenderer>();
        SpriteRenderer shapeSource = oldRightWallRenderer != null ? oldRightWallRenderer : source;
        if (oldRightWallRenderer != null) oldRightWallRenderer.enabled = false;
        Collider2D oldRightWallCollider = oldRightWall.GetComponent<Collider2D>();
        if (oldRightWallCollider != null) oldRightWallCollider.enabled = false;

        ConfigureSchoolGateWallSegment(corridorRoot.transform, "School Gate Wall Upper",
            new Vector2(37.85f, 2.1f), new Vector2(.3f, 1.8f), shapeSource);
        ConfigureSchoolGateWallSegment(corridorRoot.transform, "School Gate Wall Lower",
            new Vector2(37.85f, -2.1f), new Vector2(.3f, 1.8f), shapeSource);

        GameObject gate = FindOrCreateObject(corridorRoot.transform, "School Gate",
            new Vector2(37.55f, 0f));
        gate.transform.localScale = Vector3.one;

        GameObject panel = FindOrCreateObject(gate.transform, "School Gate Glass Panel", Vector2.zero);
        panel.transform.localPosition = Vector3.zero;
        panel.transform.localScale = Vector3.one;
        SpriteRenderer renderer = panel.GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;
        BoxCollider2D blocking = panel.GetComponent<BoxCollider2D>();
        if (blocking == null) blocking = panel.AddComponent<BoxCollider2D>();
        blocking.isTrigger = false;
        blocking.size = new Vector2(.7f, 2.4f);

        DoorController door = panel.GetComponent<DoorController>();
        if (door == null) door = panel.AddComponent<DoorController>();
        SetReference(door, "rotatingTransform", panel.transform);
        SetReference(door, "blockingCollider", blocking);
        SetBool(door, "animateVisual", false);

        GameObject closedDoorVisual = FindOrCreateObject(panel.transform,
            "School Gate Closed Glass Doors", Vector2.zero);
        closedDoorVisual.transform.localPosition = Vector3.zero;
        closedDoorVisual.transform.localScale = Vector3.one;
        ConfigureSchoolGateGlassLeaf(closedDoorVisual.transform, "Left Glass Door",
            new Vector2(0f, .55f), new Vector2(.7f, 1.05f), shapeSource);
        ConfigureSchoolGateGlassLeaf(closedDoorVisual.transform, "Right Glass Door",
            new Vector2(0f, -.55f), new Vector2(.7f, 1.05f), shapeSource);

        SchoolGateDoorVisual visual = gate.GetComponent<SchoolGateDoorVisual>();
        if (visual == null) visual = gate.AddComponent<SchoolGateDoorVisual>();
        SetReference(visual, "door", door);
        SetReference(visual, "closedDoorVisual", closedDoorVisual);

        GameObject portalObject = FindOrCreateObject(gate.transform, "School Gate Portal", Vector2.zero);
        portalObject.transform.localPosition = new Vector3(.75f, 0f, 0f);
        portalObject.transform.localScale = Vector3.one;
        BoxCollider2D portalCollider = portalObject.GetComponent<BoxCollider2D>();
        if (portalCollider == null) portalCollider = portalObject.AddComponent<BoxCollider2D>();
        portalCollider.isTrigger = true;
        portalCollider.size = new Vector2(.6f, 1.8f);
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

    [MenuItem("Tools/Devil RPG/Rebuild School Gate Doorway")]
    public static void RebuildSchoolGateDoorway()
    {
        InstallSchoolGate();
        AssetDatabase.SaveAssets();
    }

    private static void ConfigureSchoolGateWallSegment(Transform parent, string name,
        Vector2 position, Vector2 scale, SpriteRenderer source)
    {
        GameObject segment = FindOrCreateObject(parent, name, position);
        segment.transform.position = new Vector3(position.x, position.y, 0f);
        segment.transform.localScale = new Vector3(scale.x, scale.y, 1f);

        SpriteRenderer renderer = segment.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = segment.AddComponent<SpriteRenderer>();
        renderer.sprite = source.sprite;
        renderer.sharedMaterial = source.sharedMaterial;
        renderer.color = new Color(.19f, .22f, .25f, 1f);
        renderer.sortingOrder = 0;
        renderer.enabled = true;

        BoxCollider2D collider = segment.GetComponent<BoxCollider2D>();
        if (collider == null) collider = segment.AddComponent<BoxCollider2D>();
        collider.isTrigger = false;
        collider.enabled = true;
    }

    private static void ConfigureSchoolGateGlassLeaf(Transform parent, string name,
        Vector2 position, Vector2 scale, SpriteRenderer source)
    {
        GameObject leaf = FindOrCreateObject(parent, name, position);
        leaf.transform.localPosition = new Vector3(position.x, position.y, 0f);
        leaf.transform.localScale = new Vector3(scale.x, scale.y, 1f);

        SpriteRenderer renderer = leaf.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = leaf.AddComponent<SpriteRenderer>();
        renderer.sprite = source.sprite;
        renderer.sharedMaterial = source.sharedMaterial;
        renderer.color = new Color(.2f, .65f, .92f, .78f);
        renderer.sortingOrder = 2;
        renderer.enabled = true;
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
            new Vector2(18f, 12f), new Color(0.06f, 0.09f, 0.18f, 1f), -12,
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
        ApplyApartmentEntranceBuildingFacadeArtwork(apartment);
        CreateUnit(root.transform, "Apartment Sidewalk", new Vector2(120f, -1.4f),
            new Vector2(18f, 1.8f), new Color(0.38f, 0.38f, 0.42f, 1f), 0,
            false, sprite, material);
        CreateUnit(root.transform, "Apartment Street", new Vector2(120f, -4.1f),
            new Vector2(18f, 3.6f), new Color(0.12f, 0.13f, 0.16f, 1f), 0,
            false, sprite, material);
        ApplyApartmentEntranceAsphaltRoadArtwork(apartment);
        CreateUnit(root.transform, "Apartment Opposite Sidewalk", new Vector2(120f, -5.35f),
            new Vector2(18f, 1.1f), new Color(0.35f, 0.35f, 0.39f, 1f), 1,
            false, sprite, material);
        ApplyApartmentEntranceSidewalkArtwork(apartment);
        CreateUnit(root.transform, "Apartment Bus Stop Sign", new Vector2(116.4f, -4.7f),
            new Vector2(0.22f, 1.8f), new Color(0.18f, 0.62f, 0.82f, 1f), 3,
            false, sprite, material);
        ApplyApartmentEntranceBusStopSignArtwork(apartment);
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
        SetString(storageDoor, "requiredItemId", "corridor.janitorKeyRing");
        SetString(storageDoor, "missingItemDialogue", "储藏室门锁着。没有钥匙进不去。");
        SetString(storageDoor, "missingItemDialogueKey", "corridor.storageDoor.missingKey");
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
            broom.transform.position, new Vector2(.7f, 1.6f), InvestigationKind.StorageBroom, puzzle);
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

        CreateLibraryFloor(library.transform, material);

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
        ApplyLibraryWallArtwork(libraryScene);

        Color counter = new Color(0.38f, 0.22f, 0.11f, 1f);
        CreateUnit(library.transform, "Circulation Counter Horizontal",
            new Vector2(54.5f, -3.4f), new Vector2(4.2f, 0.8f), counter, 2, true,
            sprite, material);
        CreateUnit(library.transform, "Circulation Counter Vertical",
            new Vector2(52.8f, -2.2f), new Vector2(0.8f, 3.2f), counter, 2, true,
            sprite, material);
        ApplyLibraryCounterArtwork(libraryScene);

        Color darkWood = new Color(0.28f, 0.16f, 0.08f, 1f);
        CreateUnit(library.transform, "Reading Table Left", new Vector2(56.3f, 1f),
            new Vector2(3.2f, 1.2f), darkWood, 2, true, sprite, material);
        CreateUnit(library.transform, "Reading Table Right", new Vector2(63.7f, 1f),
            new Vector2(3.2f, 1.2f), darkWood, 2, true, sprite, material);
        ApplyLibraryReadingTableArtwork(libraryScene);
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
        ApplyLibraryBookcaseArtwork(libraryScene);
        ShrinkLibraryBookcaseColliders(libraryScene);

        Color chair = new Color(0.40f, 0.10f, 0.10f, 1f);
        CreateUnit(library.transform, "Reading Chair Left North", new Vector2(56.3f, 2f),
            new Vector2(0.75f, 0.45f), chair, 3, false, sprite, material);
        CreateUnit(library.transform, "Reading Chair Left South", new Vector2(56.3f, 0f),
            new Vector2(0.75f, 0.45f), chair, 3, false, sprite, material);
        CreateUnit(library.transform, "Reading Chair Right North", new Vector2(63.7f, 2f),
            new Vector2(0.75f, 0.45f), chair, 3, false, sprite, material);
        CreateUnit(library.transform, "Reading Chair Right South", new Vector2(63.7f, 0f),
            new Vector2(0.75f, 0.45f), chair, 3, false, sprite, material);
        ApplyLibraryReadingChairArtwork(libraryScene);

        CreateSpawn(library.transform, "LibraryFromCorridor", new Vector2(0f, -4.7f),
            new Vector3(0f, 0f, -10f));
        CreateAutomaticTransitionPoint(library.transform, "Library To Corridor Doorway",
            new Vector2(60f, -5.8f), "SchoolCorridor", "CorridorFromLibrary");

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

        CreateSpawn(storage.transform, "StorageRoomFromCorridor", new Vector2(0f, -5.2f),
            new Vector3(90f, 0f, -10f));
        CreateDoor(storage.transform, "Storage Room Exit Door", new Vector2(90f, -6.8f),
            sprite, material, "SchoolCorridor", "CorridorFromStorage");

        ApplyStorageRoomConcreteFloorArtwork(storageScene);

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

        GameObject counter = FindInScene(libraryScene, "Circulation Counter Horizontal");
        SpriteRenderer source = counter.GetComponent<SpriteRenderer>();
        Vector2 counterPosition = counter.transform.position;
        CreateWorldItem(library.transform, "Library Glue Bottle", counterPosition + new Vector2(.2f, -.55f),
            new Vector2(0.8f, 0.35f), new Color(0.8f, 0.08f, 0.08f, 1f),
            "胶水", false, source.sprite, source.sharedMaterial).gameObject.SetActive(false);
        CreateInvestigationPoint(library.transform, "Library Counter Investigation",
            counterPosition + new Vector2(.2f, -.65f), new Vector2(3.8f, 1.4f),
            InvestigationKind.LibraryCounter, puzzle);
        EditorSceneManager.SaveScene(libraryScene);
    }

    private static void AlignLibraryCounterGluePoint(Scene library)
    {
        Transform counter = FindInScene(library, "Circulation Counter Horizontal").transform;
        FindInScene(library, "Library Glue Bottle").transform.position =
            counter.position + new Vector3(.2f, -.55f, 0f);
        FindInScene(library, "Library Counter Investigation").transform.position =
            counter.position + new Vector3(.2f, -.65f, 0f);
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

    private static void ConfigureSpriteStateDoor(string scenePath, string doorName)
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

        panel.transform.SetParent(doorObject.transform, false);
        panel.transform.localPosition = Vector3.zero;
        panel.transform.localRotation = Quaternion.identity;
        panel.transform.localScale = new Vector3(1.5f, .35f, 1f);
        Transform hinge = doorObject.transform.Find(doorName + " Hinge");
        if (hinge != null) UnityEngine.Object.DestroyImmediate(hinge.gameObject);
        SetReference(door, "rotatingTransform", panel.transform);
        SetReference(door, "blockingCollider", blockingCollider);
        SetBool(door, "animateVisual", false);
        EditorSceneManager.SaveScene(scene);
    }

    private static void CreateLivingRoomWalls(Transform parent, Sprite sprite, Material material)
    {
        Color wall = new Color(0.32f, 0.29f, 0.26f, 1f);
        CreateUnit(parent, "Living Room Wall Left", new Vector2(132.5f, 0f),
            new Vector2(0.5f, 10f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Top Left", new Vector2(134f, 4.75f),
            new Vector2(3f, 0.5f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Top Center", new Vector2(139.9908f, 4.75f),
            new Vector2(6.7917123f, 0.5f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Top Right", new Vector2(146f, 4.75f),
            new Vector2(3f, 0.5f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Bottom Left", new Vector2(135.3421f, -4.75f),
            new Vector2(5.316f, 0.5f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Bottom Right", new Vector2(144.8445f, -4.75f),
            new Vector2(5.689f, 0.5f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Right Top", new Vector2(147.5f, 3.2f),
            new Vector2(0.5f, 3.1f), wall, 4, true, sprite, material);
        CreateUnit(parent, "Living Room Wall Right Middle", new Vector2(147.45f, -0.44f),
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

    private static void ApplyFragmentVisual(GameObject fragmentObject, Sprite artworkSprite)
    {
        SpriteRenderer baseRenderer = fragmentObject.GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            throw new InvalidOperationException(fragmentObject.name + " 缺少精灵渲染器。");
        }

        Transform artwork = fragmentObject.transform.Find("Fragment Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Fragment Artwork").transform;
            artwork.SetParent(fragmentObject.transform, false);
            Vector3 parentScale = fragmentObject.transform.lossyScale;
            artwork.localScale = new Vector3(
                .55f / (artworkSprite.bounds.size.x * parentScale.x),
                .4f / (artworkSprite.bounds.size.y * parentScale.y), 1f);
        }

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null)
        {
            artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        }

        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.sprite = artworkSprite;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;
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

    private static void ConfigureProtagonistBedroomDialogue(GameObject furniture,
        InvestigationKind kind, DialoguePage[] pages, Transform root, Vector2 size)
    {
        ProtagonistBedroomDialogue dialogue = furniture.GetComponent<ProtagonistBedroomDialogue>();
        if (dialogue == null) dialogue = furniture.AddComponent<ProtagonistBedroomDialogue>();
        dialogue.Configure(kind, pages);
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

    private static void SetWallSortingOrder(string scenePath, params string[] wallNames)
    {
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        foreach (string wallName in wallNames)
        {
            SpriteRenderer renderer = FindInScene(scene, wallName).GetComponent<SpriteRenderer>();
            if (renderer == null) throw new InvalidOperationException(wallName + " 缺少精灵渲染器。");
            renderer.sortingOrder = 0;
        }
        EditorSceneManager.SaveScene(scene);
    }

    private static void SetPrefabColliderSize(string prefabPath, Vector2 size)
    {
        GameObject prefab = PrefabUtility.LoadPrefabContents(prefabPath);
        try
        {
            BoxCollider2D collider = prefab.GetComponent<BoxCollider2D>();
            if (collider == null) throw new InvalidOperationException(prefabPath + " 缺少碰撞箱。");
            collider.size = size;
            PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefab);
        }
    }

    private static void SetSpriteSortingOrder(Scene scene, string objectName, int sortingOrder)
    {
        GameObject sceneObject = FindOptionalInScene(scene, objectName);
        if (sceneObject == null) return;

        foreach (SpriteRenderer renderer in sceneObject.GetComponentsInChildren<SpriteRenderer>(true))
        {
            renderer.sortingOrder = sortingOrder;
        }
    }

    private static void SetMazeWallLayout(Scene scene, string wallName, Vector2 position,
        Vector2 scale)
    {
        GameObject wall = FindInScene(scene, wallName);
        wall.transform.position = new Vector3(position.x, position.y, wall.transform.position.z);
        wall.transform.localScale = new Vector3(scale.x, scale.y, 1f);
    }

    private static void ApplyFlowerpotVisual(Transform visualRoot, string artworkName,
        Sprite artworkSprite, Vector2 worldSize)
    {
        SpriteRenderer[] existingRenderers = visualRoot.GetComponentsInChildren<SpriteRenderer>(true);
        Material material = existingRenderers.Length > 0 ? existingRenderers[0].sharedMaterial : null;
        foreach (SpriteRenderer renderer in existingRenderers)
        {
            renderer.enabled = false;
        }

        Transform artwork = visualRoot.Find(artworkName);
        if (artwork == null)
        {
            artwork = new GameObject(artworkName).transform;
            artwork.SetParent(visualRoot, false);
        }

        Vector3 parentScale = visualRoot.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localScale = new Vector3(
            worldSize.x / (artworkSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            worldSize.y / (artworkSprite.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);
        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = material;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingOrder = 3;
        artworkRenderer.enabled = true;
    }

    private static void CreateAutomaticTransitionPoint(Transform parent, string name,
        Vector2 position, string destinationScene, string destinationId)
    {
        GameObject pointObject = FindOptionalInScene(parent.gameObject.scene, name);
        if (pointObject == null)
        {
            pointObject = new GameObject(name);
            pointObject.transform.SetParent(parent, false);
        }

        pointObject.transform.position = new Vector3(position.x, position.y, 0f);
        MapTransitionPoint point = pointObject.GetComponent<MapTransitionPoint>();
        if (point == null) point = pointObject.AddComponent<MapTransitionPoint>();
        point.Configure(destinationScene, destinationId);
    }

    private static void RemoveSceneObject(Scene scene, string objectName)
    {
        GameObject item = FindOptionalInScene(scene, objectName);
        if (item != null) UnityEngine.Object.DestroyImmediate(item);
    }

    private static void ApplyDoorArtwork(string scenePath, string hostName, string artworkName,
        Sprite artworkSprite, float worldHeight)
    {
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        GameObject host = FindInScene(scene, hostName);
        SpriteRenderer baseRenderer = host.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(hostName + " 缺少精灵渲染器。");

        Transform artwork = host.transform.Find(artworkName);
        if (artwork == null)
        {
            artwork = new GameObject(artworkName).transform;
            artwork.SetParent(host.transform, false);
        }

        Vector3 parentScale = host.transform.lossyScale;
        float worldWidth = worldHeight * artworkSprite.bounds.size.x / artworkSprite.bounds.size.y;
        artwork.localScale = new Vector3(
            worldWidth / (artworkSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            worldHeight / (artworkSprite.bounds.size.y * Mathf.Abs(parentScale.y)), 1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.sprite = artworkSprite;
        baseRenderer.color = Color.white;
        baseRenderer.enabled = false;
        EditorSceneManager.SaveScene(scene);
    }

    private static void DisableDoorVisualRotation(string scenePath, string doorName)
    {
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        DoorController door = FindInScene(scene, doorName).GetComponent<DoorController>();
        if (door == null) throw new InvalidOperationException(doorName + " 缺少门控制器。");
        SetBool(door, "animateVisual", false);
        EditorSceneManager.SaveScene(scene);
    }

    private static void ConfigureDoorStateArtwork(string scenePath, string doorName, string artworkName,
        Sprite closedSprite, Sprite openSprite)
    {
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        DoorController door = FindInScene(scene, doorName).GetComponent<DoorController>();
        GameObject artworkObject = FindInScene(scene, artworkName);
        SpriteRenderer artworkRenderer = artworkObject.GetComponent<SpriteRenderer>();
        if (door == null || artworkRenderer == null)
        {
            throw new InvalidOperationException(doorName + " 缺少门状态美术所需组件。");
        }

        DoorStateArtwork artwork = artworkObject.GetComponent<DoorStateArtwork>();
        if (artwork == null) artwork = artworkObject.AddComponent<DoorStateArtwork>();
        SetReference(artwork, "door", door);
        SetReference(artwork, "artworkRenderer", artworkRenderer);
        SetReference(artwork, "closedSprite", closedSprite);
        SetReference(artwork, "openSprite", openSprite);
        artworkRenderer.sprite = closedSprite;
        EditorSceneManager.SaveScene(scene);
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

    private static void CreateLibraryFloor(Transform parent, Material material)
    {
        Sprite floorArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryWoodFloorTexturePath);
        if (floorArtwork == null) throw new InvalidOperationException("找不到图书馆木地板贴图。");

        GameObject floor = new GameObject("Library Floor Artwork");
        floor.transform.SetParent(parent, false);
        floor.transform.position = new Vector3(60f, 0f, 0f);
        floor.transform.localScale = new Vector3(
            18f / floorArtwork.bounds.size.x,
            12f / floorArtwork.bounds.size.y,
            1f);
        SpriteRenderer renderer = floor.AddComponent<SpriteRenderer>();
        renderer.sprite = floorArtwork;
        renderer.sharedMaterial = material;
        renderer.color = Color.white;
        renderer.sortingOrder = -10;
    }

    private static void ApplyLibraryBookcaseArtwork(Scene library)
    {
        Sprite horizontalArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryHorizontalBookcaseArtworkPath);
        Sprite verticalArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryVerticalBookcaseArtworkPath);
        if (horizontalArtwork == null || verticalArtwork == null)
        {
            throw new InvalidOperationException("找不到图书馆书架美术素材。");
        }

        for (int index = 1; index <= 4; index++)
        {
            ApplyLibraryBookcaseVisual(FindInScene(library, "Bookcase Top " + index),
                horizontalArtwork, new Vector2(3f, .7f));
        }

        for (int index = 1; index <= 2; index++)
        {
            ApplyLibraryBookcaseVisual(FindInScene(library, "Bookcase Left " + index),
                verticalArtwork, new Vector2(.7f, 2.2f));
            ApplyLibraryBookcaseVisual(FindInScene(library, "Bookcase Right " + index),
                verticalArtwork, new Vector2(.7f, 2.2f));
        }
    }

    private static void ApplyLibraryBookcaseVisual(GameObject bookcase, Sprite artworkSprite,
        Vector2 worldSize)
    {
        SpriteRenderer baseRenderer = bookcase.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(bookcase.name + " 缺少精灵渲染器。");

        Transform artwork = bookcase.transform.Find("Bookcase Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Bookcase Artwork").transform;
            artwork.SetParent(bookcase.transform, false);
        }

        Vector3 parentScale = bookcase.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localScale = new Vector3(
            worldSize.x / (artworkSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            worldSize.y / (artworkSprite.bounds.size.y * Mathf.Abs(parentScale.y)),
            1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    private static void ApplyLibraryCounterArtwork(Scene library)
    {
        Sprite counterArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryCounterArtworkPath);
        if (counterArtwork == null) throw new InvalidOperationException("找不到图书馆柜台美术素材。");

        GameObject horizontalCounter = FindInScene(library, "Circulation Counter Horizontal");
        GameObject verticalCounter = FindInScene(library, "Circulation Counter Vertical");
        SpriteRenderer horizontalRenderer = horizontalCounter.GetComponent<SpriteRenderer>();
        SpriteRenderer verticalRenderer = verticalCounter.GetComponent<SpriteRenderer>();
        BoxCollider2D horizontalCollider = horizontalCounter.GetComponent<BoxCollider2D>();
        BoxCollider2D verticalCollider = verticalCounter.GetComponent<BoxCollider2D>();
        if (horizontalRenderer == null || verticalRenderer == null
            || horizontalCollider == null || verticalCollider == null)
        {
            throw new InvalidOperationException("图书馆柜台缺少渲染器或碰撞箱。");
        }

        Bounds counterBounds = horizontalCollider.bounds;
        counterBounds.Encapsulate(verticalCollider.bounds);
        Transform artwork = horizontalCounter.transform.Find("Circulation Counter Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Circulation Counter Artwork").transform;
            artwork.SetParent(horizontalCounter.transform, false);
        }

        Vector3 parentScale = horizontalCounter.transform.lossyScale;
        artwork.position = counterBounds.center;
        artwork.localScale = new Vector3(
            counterBounds.size.x / (counterArtwork.bounds.size.x * Mathf.Abs(parentScale.x)),
            counterBounds.size.y / (counterArtwork.bounds.size.y * Mathf.Abs(parentScale.y)),
            1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = counterArtwork;
        artworkRenderer.sharedMaterial = horizontalRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = horizontalRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = horizontalRenderer.sortingOrder;
        horizontalRenderer.enabled = false;
        verticalRenderer.enabled = false;
    }

    private static void ApplyLibraryReadingChairArtwork(Scene library)
    {
        Sprite frontArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryReadingChairFrontArtworkPath);
        Sprite backArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryReadingChairBackArtworkPath);
        if (frontArtwork == null || backArtwork == null)
        {
            throw new InvalidOperationException("找不到图书馆阅读椅美术素材。");
        }

        ApplyLibraryReadingChairVisual(FindInScene(library, "Reading Chair Left North"),
            frontArtwork);
        ApplyLibraryReadingChairVisual(FindInScene(library, "Reading Chair Right North"),
            frontArtwork);
        ApplyLibraryReadingChairVisual(FindInScene(library, "Reading Chair Left South"),
            backArtwork);
        ApplyLibraryReadingChairVisual(FindInScene(library, "Reading Chair Right South"),
            backArtwork);
    }

    private static void ApplyLibraryReadingChairVisual(GameObject chair, Sprite artworkSprite)
    {
        SpriteRenderer baseRenderer = chair.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(chair.name + " 缺少精灵渲染器。");

        Transform artwork = chair.transform.Find("Reading Chair Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Reading Chair Artwork").transform;
            artwork.SetParent(chair.transform, false);
        }

        Vector3 parentScale = chair.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localScale = new Vector3(
            .75f / (artworkSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            .45f / (artworkSprite.bounds.size.y * Mathf.Abs(parentScale.y)),
            1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
    }

    private static void ApplyLibraryReadingTableArtwork(Scene library)
    {
        Sprite leftArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryReadingTableLeftArtworkPath);
        Sprite rightArtwork = AssetDatabase.LoadAssetAtPath<Sprite>(LibraryReadingTableRightArtworkPath);
        if (leftArtwork == null || rightArtwork == null)
        {
            throw new InvalidOperationException("找不到图书馆阅读桌美术素材。");
        }

        ApplyLibraryReadingTableVisual(FindInScene(library, "Reading Table Left"), leftArtwork);
        ApplyLibraryReadingTableVisual(FindInScene(library, "Reading Table Right"), rightArtwork);
    }

    private static void ApplyLibraryReadingTableVisual(GameObject table, Sprite artworkSprite)
    {
        SpriteRenderer baseRenderer = table.GetComponent<SpriteRenderer>();
        if (baseRenderer == null) throw new InvalidOperationException(table.name + " 缺少精灵渲染器。");

        Transform artwork = table.transform.Find("Reading Table Artwork");
        if (artwork == null)
        {
            artwork = new GameObject("Reading Table Artwork").transform;
            artwork.SetParent(table.transform, false);
        }

        Vector3 parentScale = table.transform.lossyScale;
        artwork.localPosition = Vector3.zero;
        artwork.localScale = new Vector3(
            3.2f / (artworkSprite.bounds.size.x * Mathf.Abs(parentScale.x)),
            1.2f / (artworkSprite.bounds.size.y * Mathf.Abs(parentScale.y)),
            1f);

        SpriteRenderer artworkRenderer = artwork.GetComponent<SpriteRenderer>();
        if (artworkRenderer == null) artworkRenderer = artwork.gameObject.AddComponent<SpriteRenderer>();
        artworkRenderer.sprite = artworkSprite;
        artworkRenderer.sharedMaterial = baseRenderer.sharedMaterial;
        artworkRenderer.color = Color.white;
        artworkRenderer.sortingLayerID = baseRenderer.sortingLayerID;
        artworkRenderer.sortingOrder = baseRenderer.sortingOrder;
        baseRenderer.enabled = false;
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
