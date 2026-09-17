using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MapTransitionService : MonoBehaviour
    {
        private const string ApartmentEntranceFromSchoolId = "ApartmentEntranceFromSchool";
        private Rigidbody2D playerBody;
        private bool isTransitioning;
        private bool schoolInventoryCleared;

        private void Awake()
        {
            playerBody = GetComponent<Rigidbody2D>();
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
            isTransitioning = false;
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
