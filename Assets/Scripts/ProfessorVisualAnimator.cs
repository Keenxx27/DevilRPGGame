using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public sealed class ProfessorVisualAnimator : MonoBehaviour
    {
        private const float MovementThreshold = .001f;

        private static readonly int IdleDownHash = Animator.StringToHash("Professor_Idle_Down");
        private static readonly int RunDownHash = Animator.StringToHash("Professor_Down_Run");
        private static readonly int IdleUpHash = Animator.StringToHash("Professor_Idle_Up");
        private static readonly int RunUpHash = Animator.StringToHash("Professor_Up_Run");
        private static readonly int IdleRightHash = Animator.StringToHash("Professor_Idle_Right");
        private static readonly int RunRightHash = Animator.StringToHash("Professor_Right_Run");
        private static readonly int IdleLeftHash = Animator.StringToHash("Professor_Idle_Left");
        private static readonly int RunLeftHash = Animator.StringToHash("Professor_Left_Run");

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Vector2 lastPosition;
        private Vector2 facingDirection = Vector2.down;
        private int currentAnimationHash;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            lastPosition = transform.position;
        }

        private void LateUpdate()
        {
            Vector2 currentPosition = transform.position;
            Vector2 movement = currentPosition - lastPosition;
            bool isMoving = movement.sqrMagnitude > MovementThreshold * MovementThreshold;
            if (isMoving)
            {
                facingDirection = movement.normalized;
            }

            CharacterAnimationState state = ResolveAnimation(facingDirection, isMoving);
            spriteRenderer.flipX = state.FlipX;
            if (currentAnimationHash != state.Hash)
            {
                currentAnimationHash = state.Hash;
                animator.Play(state.Hash);
            }

            lastPosition = currentPosition;
        }

        public static CharacterAnimationState ResolveAnimation(Vector2 direction, bool isMoving)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                return direction.x < 0f
                    ? new CharacterAnimationState(
                        isMoving ? RunLeftHash : IdleLeftHash,
                        isMoving ? "Professor_Left_Run" : "Professor_Idle_Left", false)
                    : new CharacterAnimationState(
                        isMoving ? RunRightHash : IdleRightHash,
                        isMoving ? "Professor_Right_Run" : "Professor_Idle_Right", false);
            }

            return direction.y > 0f
                ? new CharacterAnimationState(
                    isMoving ? RunUpHash : IdleUpHash,
                    isMoving ? "Professor_Up_Run" : "Professor_Idle_Up", false)
                : new CharacterAnimationState(
                    isMoving ? RunDownHash : IdleDownHash,
                    isMoving ? "Professor_Down_Run" : "Professor_Idle_Down", false);
        }
    }
}
