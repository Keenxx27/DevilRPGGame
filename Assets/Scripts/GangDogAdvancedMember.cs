using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public sealed class GangDogAdvancedMember : MonoBehaviour
    {
        public const float SwingDistance = 1.1f;
        public const float SwingDuration = .35f;
        private const float CaptureDistance = .8f;
        private const float SwingMissCooldown = .3f;
        private static bool hasShownFirstCaptureDialogue;
        private static bool captureInProgress;
        [SerializeField] private Vector2 patrolMin;
        [SerializeField] private Vector2 patrolMax;
        [SerializeField] private string resetSpawnId;
        [SerializeField, Min(0f)] private float patrolSpeed = 1.55f;
        [SerializeField, Min(0f)] private float chaseSpeed = 3f;
        [SerializeField, Min(0f)] private float detectionDistance = 4.8f;

        private Rigidbody2D body;
        private Vector2 target;
        private float retargetTimer;
        private float resetCooldown;
        private float swingTime;
        private float swingCooldown;
        private Vector2 facingDirection = Vector2.down;
        private PlayerInteraction player;

        public bool IsSwinging => swingTime > 0f;
        public Vector2 FacingDirection => facingDirection;

        public void Configure(Vector2 minimum, Vector2 maximum, string spawnId)
        {
            patrolMin = minimum;
            patrolMax = maximum;
            resetSpawnId = spawnId;
        }

        public static bool ShouldBeginSwing(float distance, float detectionRange)
        {
            return distance <= SwingDistance && distance <= detectionRange;
        }

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            SetNewTarget();
        }

        private void Update()
        {
            if (player == null) player = FindObjectOfType<PlayerInteraction>();
            resetCooldown = Mathf.Max(0f, resetCooldown - Time.deltaTime);
            swingCooldown = Mathf.Max(0f, swingCooldown - Time.deltaTime);
            if (player == null) return;

            float distance = Vector2.Distance(body.position, player.transform.position);
            Vector2 playerOffset = (Vector2)player.transform.position - body.position;
            if (!IsSwinging && playerOffset.sqrMagnitude > .0001f)
            {
                facingDirection = playerOffset.normalized;
            }

            if (IsSwinging)
            {
                swingTime = Mathf.Max(0f, swingTime - Time.deltaTime);
                if (!IsSwinging)
                {
                    if (distance <= CaptureDistance && resetCooldown <= 0f && !captureInProgress)
                    {
                        BeginCapture();
                    }
                    else
                    {
                        swingCooldown = SwingMissCooldown;
                    }
                }
                return;
            }

            if (swingCooldown <= 0f && resetCooldown <= 0f && !captureInProgress
                && ShouldBeginSwing(distance, detectionDistance))
            {
                swingTime = SwingDuration;
                return;
            }

            if (distance > detectionDistance)
            {
                retargetTimer -= Time.deltaTime;
                if (Vector2.Distance(body.position, target) < .12f || retargetTimer <= 0f)
                {
                    SetNewTarget();
                }
            }
        }

        private void FixedUpdate()
        {
            if (player == null || IsSwinging || Time.timeScale <= 0f) return;
            float distance = Vector2.Distance(body.position, player.transform.position);
            Vector2 destination = distance <= detectionDistance ? (Vector2)player.transform.position : target;
            float speed = distance <= detectionDistance ? chaseSpeed : patrolSpeed;
            body.MovePosition(Vector2.MoveTowards(body.position, destination, speed * Time.fixedDeltaTime));
        }

        private void BeginCapture()
        {
            DialogueController dialogue = player.GetComponent<DialogueController>();
            if (!hasShownFirstCaptureDialogue && dialogue != null)
            {
                if (dialogue.IsPlaying)
                {
                    return;
                }

                if (dialogue.StartDialogue(new[]
                    {
                        new DialoguePage(GameLocalization.Get("suburbs.speaker.strayDogs"),
                            GameLocalization.Get("suburbs.road2.advancedMember.capture"))
                    }, BeginBlackoutAndReset))
                {
                    hasShownFirstCaptureDialogue = true;
                    captureInProgress = true;
                    return;
                }
            }

            captureInProgress = true;
            BeginBlackoutAndReset();
        }

        private void BeginBlackoutAndReset()
        {
            StartCoroutine(BlackoutAndReset());
        }

        private IEnumerator BlackoutAndReset()
        {
            MainCharacterMovement movement = player.GetComponent<MainCharacterMovement>();
            PlayerInteraction interaction = player.GetComponent<PlayerInteraction>();
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            bool movementWasEnabled = movement != null && movement.enabled;
            bool interactionWasEnabled = interaction != null && interaction.enabled;
            bool inventoryWasEnabled = inventory != null && inventory.enabled;
            if (movement != null) movement.enabled = false;
            if (interaction != null) interaction.enabled = false;
            if (inventory != null) inventory.enabled = false;

            GameObject blackout = CreateBlackout();
            yield return new WaitForSecondsRealtime(.55f);

            if (MapSpawnPoint.TryFind(resetSpawnId, out MapSpawnPoint start))
            {
                Rigidbody2D playerBody = player.GetComponent<Rigidbody2D>();
                if (playerBody != null && Camera.main != null)
                {
                    MapTransitionService.ApplyDestination(playerBody, Camera.main, start);
                }
            }

            Destroy(blackout);
            if (movement != null) movement.enabled = movementWasEnabled;
            if (interaction != null) interaction.enabled = interactionWasEnabled;
            if (inventory != null) inventory.enabled = inventoryWasEnabled;
            resetCooldown = 1.2f;
            captureInProgress = false;
        }

        private static GameObject CreateBlackout()
        {
            GameObject canvasObject = new GameObject("Road Two Capture Blackout");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 60;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());

            GameObject panel = new GameObject("Blackout");
            panel.transform.SetParent(canvasObject.transform, false);
            Image image = panel.AddComponent<Image>();
            image.color = Color.black;
            RectTransform rect = image.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return canvasObject;
        }

        private void SetNewTarget()
        {
            target = new Vector2(Random.Range(patrolMin.x, patrolMax.x),
                Random.Range(patrolMin.y, patrolMax.y));
            retargetTimer = Random.Range(1.2f, 2.8f);
        }
    }
}
