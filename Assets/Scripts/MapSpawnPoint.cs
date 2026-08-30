using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class MapSpawnPoint : MonoBehaviour
    {
        private static readonly Dictionary<string, MapSpawnPoint> SpawnPoints =
            new Dictionary<string, MapSpawnPoint>();

        [SerializeField] private string id;
        [SerializeField] private Vector3 cameraPosition;

        public string Id => id;
        public Vector3 CameraPosition => cameraPosition;

        private void OnEnable()
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Debug.LogError("MapSpawnPoint requires a non-empty id.", this);
                enabled = false;
                return;
            }

            if (SpawnPoints.TryGetValue(id, out MapSpawnPoint existing) && existing != this)
            {
                Debug.LogError("Duplicate MapSpawnPoint id: " + id, this);
                enabled = false;
                return;
            }

            SpawnPoints[id] = this;
        }

        private void OnDisable()
        {
            if (!string.IsNullOrWhiteSpace(id)
                && SpawnPoints.TryGetValue(id, out MapSpawnPoint existing)
                && existing == this)
            {
                SpawnPoints.Remove(id);
            }
        }

        public static bool TryFind(string spawnId, out MapSpawnPoint spawnPoint)
        {
            if (string.IsNullOrWhiteSpace(spawnId))
            {
                spawnPoint = null;
                return false;
            }

            return SpawnPoints.TryGetValue(spawnId, out spawnPoint) && spawnPoint != null;
        }
    }
}
