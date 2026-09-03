using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MainCharacterMovement : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveSpeed = 5f;

        private Rigidbody2D body;
        private Animator animator;
        private SpriteRenderer characterRenderer;
        private Vector2 input;
        private int currentAnimationHash;

        private static readonly int IdleDownHash = Animator.StringToHash("Player_Idle_Down");
        private static readonly int RunDownHash = Animator.StringToHash("Player_Down_Run");
        private static readonly int IdleUpHash = Animator.StringToHash("Player_Idle_Up");
        private static readonly int RunUpHash = Animator.StringToHash("Player_Up_Run");
        private static readonly int IdleRightHash = Animator.StringToHash("Player_Idle_Right");
        private static readonly int RunRightHash = Animator.StringToHash("Player_Right_Run");

        public Vector2 FacingDirection { get; private set; } = Vector2.down;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
            characterRenderer = GetComponentInChildren<SpriteRenderer>();

            if (animator == null || characterRenderer == null)
            {
                if (Application.isPlaying)
                {
                    Debug.LogError(
                        "Main Character 的子对象上需要 Animator 和 SpriteRenderer。",
                        this);
                }

                enabled = false;
            }
        }

        private void Update()
        {
            input = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));
            FacingDirection = CalculateFacingDirection(FacingDirection, input);
            UpdateAnimation();
        }

        private void FixedUpdate()
        {
            Vector3 delta = CalculateDelta(input, moveSpeed, Time.fixedDeltaTime);
            body.MovePosition(body.position + new Vector2(delta.x, delta.y));
        }

        private void UpdateAnimation()
        {
            bool isMoving = input.sqrMagnitude > 0.01f;
            Vector2 direction = isMoving ? input : FacingDirection;
            int animationHash;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                characterRenderer.flipX = direction.x < 0f;
                animationHash = isMoving ? RunRightHash : IdleRightHash;
            }
            else if (direction.y > 0f)
            {
                characterRenderer.flipX = false;
                animationHash = isMoving ? RunUpHash : IdleUpHash;
            }
            else
            {
                characterRenderer.flipX = false;
                animationHash = isMoving ? RunDownHash : IdleDownHash;
            }

            PlayAnimation(animationHash);
        }

        private void PlayAnimation(int animationHash)
        {
            if (currentAnimationHash == animationHash)
            {
                return;
            }

            currentAnimationHash = animationHash;
            animator.Play(animationHash);
        }

        public static Vector3 CalculateDelta(Vector2 input, float speed, float deltaTime)
        {
            Vector2 direction = Vector2.ClampMagnitude(input, 1f);
            return direction * speed * deltaTime;
        }

        public static Vector2 CalculateFacingDirection(Vector2 current, Vector2 input)
        {
            if (input.sqrMagnitude > 0f)
            {
                return input.normalized;
            }

            return current.sqrMagnitude > 0f ? current.normalized : Vector2.down;
        }
    }
}
