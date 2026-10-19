using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public sealed class GangDogMemberVisualAnimator : MonoBehaviour
    {
        private const float MovementThreshold = .001f;
        private Animator animator;
        private SpriteRenderer renderer;
        private GangDogAdvancedMember advancedMember;
        private Vector2 lastPosition;
        private Vector2 facing = Vector2.down;
        private int currentHash;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            renderer = GetComponent<SpriteRenderer>();
            advancedMember = GetComponentInParent<GangDogAdvancedMember>();
            lastPosition = transform.position;
        }

        private void LateUpdate()
        {
            Vector2 position = transform.position;
            Vector2 movement = position - lastPosition;
            bool moving = movement.sqrMagnitude > MovementThreshold * MovementThreshold;
            bool advanced = advancedMember != null;
            bool swinging = advanced && advancedMember.IsSwinging;
            if (swinging) facing = advancedMember.FacingDirection;
            else if (moving) facing = movement.normalized;

            CharacterAnimationState state = ResolveAnimation(advanced, swinging, facing, moving);
            renderer.flipX = false;
            if (state.Hash != currentHash)
            {
                currentHash = state.Hash;
                animator.Play(state.Hash);
            }

            lastPosition = position;
        }

        public static CharacterAnimationState ResolveAnimation(bool advanced, bool swinging, Vector2 direction, bool moving)
        {
            string suffix;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) suffix = direction.x < 0f ? "Left" : "Right";
            else suffix = direction.y > 0f ? "Up" : "Down";
            string prefix = advanced ? "GangDogAdvanced_" : "GangDogLow_";
            string action = advanced && swinging ? "Swing" : moving ? "Walk" : "Idle";
            string name = prefix + action + "_" + suffix;
            return new CharacterAnimationState(Animator.StringToHash(name), name, false);
        }
    }
}
