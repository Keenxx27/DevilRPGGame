using System;
using System.Collections;
using UnityEngine;

namespace RPG
{
    public sealed class AbyssEntranceDropIn : MonoBehaviour
    {
        public static event Action<Transform> PlayerFinishedDropping;
        [SerializeField] private string spawnId = "AbyssEntranceFromRubblePile";
        [SerializeField, Min(0.1f)] private float dropDuration = 1.1f;
        [SerializeField, Min(0f)] private float dropHeight = 6f;

        private bool isDropping;

        private void OnEnable()
        {
            MapTransitionService.PlayerArrivedAtDestination += HandlePlayerArrival;
        }

        private void OnDisable()
        {
            MapTransitionService.PlayerArrivedAtDestination -= HandlePlayerArrival;
        }

        private void HandlePlayerArrival(Transform player)
        {
            if (isDropping || player == null) return;
            if (!MapSpawnPoint.TryFind(spawnId, out MapSpawnPoint spawn)) return;
            if (Vector2.Distance(player.position, spawn.transform.position) > .05f) return;
            StartCoroutine(DropRoutine(player, spawn.transform.position));
        }

        private IEnumerator DropRoutine(Transform player, Vector3 destination)
        {
            isDropping = true;
            Rigidbody2D body = player.GetComponent<Rigidbody2D>();
            MainCharacterMovement movement = player.GetComponent<MainCharacterMovement>();
            PlayerInteraction interaction = player.GetComponent<PlayerInteraction>();
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();

            bool movementWasEnabled = movement != null && movement.enabled;
            bool interactionWasEnabled = interaction != null && interaction.enabled;
            bool inventoryWasEnabled = inventory != null && inventory.enabled;
            bool wasSimulated = body != null && body.simulated;
            if (movement != null) movement.enabled = false;
            if (interaction != null) interaction.enabled = false;
            if (inventory != null) inventory.enabled = false;
            if (body != null)
            {
                body.velocity = Vector2.zero;
                body.simulated = false;
            }

            Vector3 start = destination + Vector3.up * dropHeight;
            player.position = start;
            float elapsed = 0f;
            while (elapsed < dropDuration)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / dropDuration);
                player.position = Vector3.Lerp(start, destination, progress * progress);
                yield return null;
            }

            player.position = destination;
            if (body != null)
            {
                body.position = destination;
                body.velocity = Vector2.zero;
                body.simulated = wasSimulated;
            }
            if (movement != null) movement.enabled = movementWasEnabled;
            if (interaction != null) interaction.enabled = interactionWasEnabled;
            if (inventory != null) inventory.enabled = inventoryWasEnabled;
            UiAudio.PlayLanding();
            isDropping = false;
            PlayerFinishedDropping?.Invoke(player);
        }
    }
}
