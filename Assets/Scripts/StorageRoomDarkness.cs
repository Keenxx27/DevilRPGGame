using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace RPG
{
    public sealed class StorageRoomDarkness : MonoBehaviour
    {
        [Header("Player Light")]
        [Tooltip("Object followed by the light. Leave empty to find the active player automatically.")]
        [SerializeField] private Transform lightTarget;
        [SerializeField, Min(0.1f)] private float lightRadius = 2.2f;
        [SerializeField, Min(0f)] private float lightIntensity = 1f;
        [SerializeField, Range(0.01f, 1f)] private float edgeSoftness = 0.35f;
        [SerializeField, Range(0f, 1f)] private float darknessOpacity = 0.92f;

        private readonly Dictionary<Light2D, float> globalLightIntensities =
            new Dictionary<Light2D, float>();
        private PlayerInteraction player;
        private Light2D playerLight;
        private bool roomLightingActive;
        private bool openingPlayed;
        private DialoguePage[] openingPages;

        private void Awake()
        {
            openingPages = new[]
            {
                new DialoguePage("主角", GameLocalization.Get("storage.opening.1")),
                new DialoguePage("主角", GameLocalization.Get("storage.opening.2"))
            };
        }

        private void LateUpdate()
        {
            ResolveLightTarget();
            if (lightTarget == null)
            {
                SetRoomLighting(false);
                return;
            }

            Vector2 position = lightTarget.position;
            bool inside = position.x > -4f && position.x < 4f
                && position.y > -7f && position.y < 7f;
            SetRoomLighting(inside);
            if (!inside) return;

            EnsurePlayerLight();
            UpdatePlayerLight();

            if (!openingPlayed)
            {
                DialogueController dialogue = lightTarget.GetComponent<DialogueController>();
                if (dialogue != null && dialogue.StartDialogue(openingPages)) openingPlayed = true;
            }
        }

        private void ResolveLightTarget()
        {
            if (lightTarget != null && lightTarget.gameObject.activeInHierarchy) return;

            lightTarget = null;
            player = null;
            foreach (PlayerInteraction candidate in FindObjectsOfType<PlayerInteraction>())
            {
                if (candidate == null || !candidate.isActiveAndEnabled) continue;

                Vector2 candidatePosition = candidate.transform.position;
                bool insideRoom = candidatePosition.x > -4f && candidatePosition.x < 4f
                    && candidatePosition.y > -7f && candidatePosition.y < 7f;
                if (player == null || insideRoom)
                {
                    player = candidate;
                    if (insideRoom) break;
                }
            }

            if (player != null) lightTarget = player.transform;
        }

        private void EnsurePlayerLight()
        {
            if (playerLight != null || lightTarget == null) return;

            GameObject lightObject = new GameObject("Storage Player Light 2D");
            lightObject.transform.SetParent(lightTarget, false);
            lightObject.transform.localPosition = Vector3.zero;
            playerLight = lightObject.AddComponent<Light2D>();
            playerLight.lightType = Light2D.LightType.Point;
            playerLight.color = Color.white;
            playerLight.overlapOperation = Light2D.OverlapOperation.Additive;
        }

        private void UpdatePlayerLight()
        {
            if (playerLight == null) return;

            playerLight.transform.localPosition = Vector3.zero;
            playerLight.pointLightOuterRadius = lightRadius;
            playerLight.pointLightInnerRadius = lightRadius * (1f - edgeSoftness);
            playerLight.falloffIntensity = edgeSoftness;
            playerLight.intensity = lightIntensity;
            playerLight.enabled = roomLightingActive;
        }

        private void SetRoomLighting(bool active)
        {
            if (roomLightingActive == active)
            {
                if (active) ApplyGlobalDarkness();
                return;
            }

            roomLightingActive = active;
            if (active)
            {
                CaptureGlobalLights();
                ApplyGlobalDarkness();
            }
            else
            {
                RestoreGlobalLights();
            }

            if (playerLight != null) playerLight.enabled = active;
        }

        private void CaptureGlobalLights()
        {
            foreach (Light2D light in FindObjectsOfType<Light2D>())
            {
                if (light == null || light == playerLight
                    || light.lightType != Light2D.LightType.Global
                    || globalLightIntensities.ContainsKey(light)) continue;

                globalLightIntensities.Add(light, light.intensity);
            }
        }

        private void ApplyGlobalDarkness()
        {
            float remainingLight = 1f - darknessOpacity;
            foreach (KeyValuePair<Light2D, float> entry in globalLightIntensities)
            {
                if (entry.Key != null) entry.Key.intensity = entry.Value * remainingLight;
            }
        }

        private void RestoreGlobalLights()
        {
            foreach (KeyValuePair<Light2D, float> entry in globalLightIntensities)
            {
                if (entry.Key != null) entry.Key.intensity = entry.Value;
            }

            globalLightIntensities.Clear();
        }

        private void RemovePlayerLight()
        {
            if (playerLight == null) return;

            // The player is DontDestroyOnLoad, so its child light would otherwise survive the
            // storage-room scene transition. Disable it immediately, then remove the object.
            playerLight.enabled = false;
            GameObject lightObject = playerLight.gameObject;
            playerLight = null;
            if (Application.isPlaying)
            {
                Destroy(lightObject);
            }
            else
            {
                DestroyImmediate(lightObject);
            }
        }

        private void OnDisable()
        {
            RestoreGlobalLights();
            RemovePlayerLight();
        }

        private void OnDestroy()
        {
            RestoreGlobalLights();
            RemovePlayerLight();
        }
    }
}
