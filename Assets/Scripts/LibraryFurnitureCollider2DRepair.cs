using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    /// <summary>
    /// Converts the accidentally-added 3D furniture colliders in the library
    /// into matching 2D colliders at runtime. Each object keeps its own center
    /// and X/Y size; investigation triggers are not touched.
    /// </summary>
    public sealed class LibraryFurnitureCollider2DRepair : MonoBehaviour
    {
        private const string SceneName = "3_SchoolLibrary";
        private const string Prefix = "[LibraryFurnitureCollider2DRepair]";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (FindObjectOfType<LibraryFurnitureCollider2DRepair>() != null) return;
            GameObject host = new GameObject("Library Furniture Collider 2D Repair");
            DontDestroyOnLoad(host);
            host.AddComponent<LibraryFurnitureCollider2DRepair>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
            RepairActiveScene();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SceneName) RepairScene(scene);
        }

        private static void RepairActiveScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == SceneName) RepairScene(scene);
        }

        private static void RepairScene(Scene scene)
        {
            int converted = 0;
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (BoxCollider source in root.GetComponentsInChildren<BoxCollider>(true))
                {
                    if (!IsFurnitureArtwork(source.gameObject.name)) continue;

                    BoxCollider2D target = source.GetComponent<BoxCollider2D>();
                    if (target == null) target = source.gameObject.AddComponent<BoxCollider2D>();
                    target.offset = new Vector2(source.center.x, source.center.y);
                    target.size = new Vector2(source.size.x, source.size.y);
                    target.isTrigger = source.isTrigger;
                    target.enabled = source.enabled;
                    source.enabled = false;
                    converted++;
                }
            }

            Debug.Log($"{Prefix} Converted {converted} furniture colliders from 3D to 2D.");
        }

        private static bool IsFurnitureArtwork(string objectName)
        {
            return objectName.StartsWith("Reading Chair Artwork")
                || objectName.StartsWith("Reading Table Artwork")
                || objectName.StartsWith("Bookcase Artwork")
                || objectName.StartsWith("Circulation Counter Artwork");
        }
    }
}
