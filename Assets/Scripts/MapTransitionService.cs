using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace RPG
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MapTransitionService : MonoBehaviour
    {
        private const string ApartmentEntranceFromSchoolId = "ApartmentEntranceFromSchool";
        private static MapTransitionService instance;
        public static event Action<Transform> PlayerArrivedAtDestination;
        private Rigidbody2D playerBody;
        private bool isTransitioning;
        private bool schoolInventoryCleared;
        private readonly Dictionary<int, List<GameObject>> hiddenSceneRoots =
            new Dictionary<int, List<GameObject>>();

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            playerBody = GetComponent<Rigidbody2D>();
            PreserveRuntimeObjects();
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        public void RequestTransition(string sceneName, string destinationId)
        {
            if (!isTransitioning)
            {
                StartCoroutine(TransitionRoutine(sceneName, destinationId));
            }
        }

        private IEnumerator TransitionRoutine(string sceneName, string destinationId)
        {
            isTransitioning = true;

            if (string.IsNullOrWhiteSpace(destinationId))
            {
                Fail("Map transition requires a destination id.");
                yield break;
            }

            if (!string.IsNullOrWhiteSpace(sceneName)
                && !SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                AsyncOperation loadOperation = null;
                try
                {
                    loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                }
                catch (Exception exception)
                {
                    Fail("Unable to load scene " + sceneName + ": " + exception.Message);
                    yield break;
                }

                if (loadOperation == null)
                {
                    Fail("Unable to load scene " + sceneName + ".");
                    yield break;
                }

                while (!loadOperation.isDone)
                {
                    yield return null;
                }
            }

            Scene destinationScene = string.IsNullOrWhiteSpace(sceneName)
                ? gameObject.scene
                : SceneManager.GetSceneByName(sceneName);
            RestoreSceneRoots(destinationScene);

            if (!MapSpawnPoint.TryFind(destinationId, out MapSpawnPoint destination))
            {
                Fail("Map spawn point not found: " + destinationId);
                yield break;
            }

            Camera mainCamera = Camera.main;
            if (playerBody == null || mainCamera == null)
            {
                Fail("Map transition requires a player Rigidbody2D and Main Camera.");
                yield break;
            }

            GetComponent<PlayerInteraction>()?.ClearNearbyDoors();
            GetComponent<PlayerInventory>()?.ClearNearbyItems();
            ApplyDestination(playerBody, mainCamera, destination);
            if (!schoolInventoryCleared && destinationId == ApartmentEntranceFromSchoolId)
            {
                GetComponent<PlayerInventory>()?.ClearAllItems();
                schoolInventoryCleared = true;
            }
            DoorExitPortal.SuppressPortalsAtDestination(playerBody);
            PlayerArrivedAtDestination?.Invoke(transform);
            HideOtherSceneRoots(destinationScene);
            if (destinationScene.IsValid() && destinationScene.isLoaded)
            {
                SceneManager.SetActiveScene(destinationScene);
            }
            isTransitioning = false;
        }

        private void PreserveRuntimeObjects()
        {
            Scene sourceScene = gameObject.scene;

            transform.SetParent(null, true);
            DontDestroyOnLoad(gameObject);

            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                PreserveRoot(mainCamera.transform.root.gameObject, sourceScene);
            }

            foreach (Canvas canvas in FindObjectsOfType<Canvas>(true))
            {
                PreserveRoot(canvas.transform.root.gameObject, sourceScene);
            }

            EventSystem eventSystem = FindObjectOfType<EventSystem>(true);
            if (eventSystem != null)
            {
                PreserveRoot(eventSystem.transform.root.gameObject, sourceScene);
            }
        }

        private static void PreserveRoot(GameObject root, Scene sourceScene)
        {
            if (root != null && root.scene == sourceScene)
            {
                DontDestroyOnLoad(root);
            }
        }

        private void HideOtherSceneRoots(Scene destinationScene)
        {
            for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                Scene scene = SceneManager.GetSceneAt(sceneIndex);
                if (!scene.IsValid() || !scene.isLoaded || scene == destinationScene)
                {
                    continue;
                }

                if (!hiddenSceneRoots.TryGetValue(scene.handle, out List<GameObject> hiddenRoots))
                {
                    hiddenRoots = new List<GameObject>();
                    hiddenSceneRoots.Add(scene.handle, hiddenRoots);
                }

                foreach (GameObject root in scene.GetRootGameObjects())
                {
                    if (!root.activeSelf || hiddenRoots.Contains(root)) continue;
                    root.SetActive(false);
                    hiddenRoots.Add(root);
                }
            }
        }

        private void RestoreSceneRoots(Scene scene)
        {
            if (!scene.IsValid() || !scene.isLoaded
                || !hiddenSceneRoots.TryGetValue(scene.handle, out List<GameObject> hiddenRoots))
            {
                return;
            }

            foreach (GameObject root in hiddenRoots)
            {
                if (root != null) root.SetActive(true);
            }

            hiddenRoots.Clear();
        }

        public static void ApplyDestination(
            Rigidbody2D player, Camera mainCamera, MapSpawnPoint destination)
        {
            player.position = destination.transform.position;
            player.velocity = Vector2.zero;
            mainCamera.transform.position = destination.CameraPosition;
        }

        private void Fail(string message)
        {
            Debug.LogError(message, this);
            isTransitioning = false;
        }
    }
}
