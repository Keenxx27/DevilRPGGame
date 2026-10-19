using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public sealed class ClassroomStudentVisualAnimator : MonoBehaviour
    {
        private const float MovementThreshold = .001f;

        [SerializeField] private string animationPrefix;

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Vector2 lastPosition;
        private Vector2 facingDirection = Vector2.down;
        private int currentAnimationHash;

        public string AnimationPrefix
        {
            get => animationPrefix;
            set => animationPrefix = value;
        }

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

            CharacterAnimationState state = ResolveAnimation(animationPrefix, facingDirection, isMoving);
            spriteRenderer.flipX = state.FlipX;
            if (currentAnimationHash != state.Hash)
            {
                currentAnimationHash = state.Hash;
                animator.Play(state.Hash);
            }

            lastPosition = currentPosition;
        }

        public static CharacterAnimationState ResolveAnimation(string prefix, Vector2 direction, bool isMoving)
        {
            string stateName;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                stateName = direction.x < 0f
                    ? isMoving ? "Left_Run" : "Idle_Left"
                    : isMoving ? "Right_Run" : "Idle_Right";
            }
            else
            {
                stateName = direction.y > 0f
                    ? isMoving ? "Up_Run" : "Idle_Up"
                    : isMoving ? "Down_Run" : "Idle_Down";
            }

            string fullName = $"{prefix}_{stateName}";
            return new CharacterAnimationState(Animator.StringToHash(fullName), fullName, false);
        }
    }
}
