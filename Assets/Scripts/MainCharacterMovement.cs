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
        private static readonly int IdleLeftHash = Animator.StringToHash("Player_Idle_Left");
        private static readonly int RunLeftHash = Animator.StringToHash("Player_Left_Run");

        public Vector2 FacingDirection { get; private set; } = Vector2.down;

        private void Awake()
        {
            if (GetComponent<FootstepAudio>() == null) gameObject.AddComponent<FootstepAudio>();
            body = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
            characterRenderer = animator != null
                ? animator.GetComponent<SpriteRenderer>()
                : GetComponentInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            input = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));
            FacingDirection = CalculateFacingDirection(FacingDirection, input);
            if (animator != null && characterRenderer != null)
            {
                UpdateAnimation();
            }
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
            CharacterAnimationState state = ResolveAnimation(direction, isMoving);
            characterRenderer.flipX = state.FlipX;
            PlayAnimation(state.Hash);
        }

        public static CharacterAnimationState ResolveAnimation(Vector2 direction, bool isMoving)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                return direction.x < 0f
                    ? new CharacterAnimationState(
                        isMoving ? RunLeftHash : IdleLeftHash,
                        isMoving ? "Player_Left_Run" : "Player_Idle_Left", false)
                    : new CharacterAnimationState(
                        isMoving ? RunRightHash : IdleRightHash,
                        isMoving ? "Player_Right_Run" : "Player_Idle_Right", false);
            }

            return direction.y > 0f
                ? new CharacterAnimationState(
                    isMoving ? RunUpHash : IdleUpHash,
                    isMoving ? "Player_Up_Run" : "Player_Idle_Up", false)
                : new CharacterAnimationState(
                    isMoving ? RunDownHash : IdleDownHash,
                    isMoving ? "Player_Down_Run" : "Player_Idle_Down", false);
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

    public readonly struct CharacterAnimationState
    {
        public CharacterAnimationState(int hash, string name, bool flipX)
        {
            Hash = hash;
            Name = name;
            FlipX = flipX;
        }

        public int Hash { get; }
        public string Name { get; }
        public bool FlipX { get; }
    }
}
