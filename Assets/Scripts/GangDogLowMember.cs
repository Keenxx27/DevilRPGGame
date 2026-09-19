using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public sealed class GangDogLowMember : MonoBehaviour
    {
        private static readonly DialoguePage[] BlockPages =
        {
            new DialoguePage("丧家犬", "这是我们的地盘，没你过去的份！")
        };

        private static bool hasShownBlockDialogue;
        [SerializeField] private Vector2 patrolMin;
        [SerializeField] private Vector2 patrolMax;
        [SerializeField, Min(0f)] private float moveSpeed = 1.35f;
        [SerializeField, Min(0f)] private float stopDistance = 2.1f;

        private Rigidbody2D body;
        private Vector2 target;
        private float retargetTimer;
        private bool blocksPlayer;
        private bool blockedLastFrame;

        public void Configure(Vector2 minimum, Vector2 maximum)
        {
            patrolMin = minimum;
            patrolMax = maximum;
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
            PlayerInteraction player = FindObjectOfType<PlayerInteraction>();
            blocksPlayer = player != null
                && Vector2.Distance(transform.position, player.transform.position) <= stopDistance;
            if (blocksPlayer)
            {
                body.velocity = Vector2.zero;
                if (!blockedLastFrame && !hasShownBlockDialogue)
                {
                    DialogueController dialogue = player.GetComponent<DialogueController>();
                    if (dialogue != null && !dialogue.IsPlaying
                        && dialogue.StartDialogue(BlockPages))
                    {
                        hasShownBlockDialogue = true;
                    }
                }
                blockedLastFrame = true;
                return;
            }

            blockedLastFrame = false;

            retargetTimer -= Time.deltaTime;
            if (Vector2.Distance(body.position, target) < .12f || retargetTimer <= 0f)
            {
                SetNewTarget();
            }
        }

        private void FixedUpdate()
        {
            if (blocksPlayer || Time.timeScale <= 0f) return;
            body.MovePosition(Vector2.MoveTowards(body.position, target,
                moveSpeed * Time.fixedDeltaTime));
        }

        private void SetNewTarget()
        {
            target = new Vector2(Random.Range(patrolMin.x, patrolMax.x),
                Random.Range(patrolMin.y, patrolMax.y));
            retargetTimer = Random.Range(1.4f, 3.2f);
        }
    }
}
