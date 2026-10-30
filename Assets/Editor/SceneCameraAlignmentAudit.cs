using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneCameraAlignmentAudit
{
    [MenuItem("Tools/Devil RPG/Audit Scene Camera Alignment")]
    public static void Audit()
    {
        string[] paths = {
            "Assets/Scenes/2_SchoolCorridor.unity", "Assets/Scenes/3_SchoolLibrary.unity", "Assets/Scenes/4_SchoolStorageRoom.unity",
            "Assets/Scenes/ApartmentEntrance.unity", "Assets/Scenes/ApartmentLivingRoom.unity", "Assets/Scenes/ApartmentKitchen.unity", "Assets/Scenes/ApartmentBathroom.unity", "Assets/Scenes/ApartmentLinChunbaoBedroom.unity", "Assets/Scenes/ApartmentProtagonistBedroom.unity",
            "Assets/Scenes/RoadToSuburbs1.unity", "Assets/Scenes/RoadToSuburbs2.unity", "Assets/Scenes/RubblePile.unity", "Assets/Scenes/AbyssEntrance.unity", "Assets/Scenes/FluorescentMushroomCave.unity", "Assets/Scenes/RoadHome.unity"
        };
        foreach (string path in paths)
        {
            Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            Camera camera = Object.FindObjectOfType<Camera>();
            SpriteRenderer[] renderers = Object.FindObjectsOfType<SpriteRenderer>();
            bool hasBounds = false; Bounds bounds = default;
            foreach (SpriteRenderer renderer in renderers)
            {
                if (!renderer.enabled || renderer.sprite == null) continue;
                if (!hasBounds) { bounds = renderer.bounds; hasBounds = true; } else bounds.Encapsulate(renderer.bounds);
            }
            Debug.Log($"CAMERA_AUDIT|{path}|camera={(camera == null ? "none" : camera.transform.position.ToString())}|center={(hasBounds ? bounds.center.ToString() : "none")}|size={(hasBounds ? bounds.size.ToString() : "none")}");
        }
    }
}
