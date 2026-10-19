using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public sealed class GangDogBossVisualAnimator : MonoBehaviour
    {
        private const float MovementThreshold = .001f;
        private Animator animator;
        private SpriteRenderer renderer;
        private GangDogBoss boss;
        private Vector2 lastPosition;
        private Vector2 facing = Vector2.down;
        private int currentHash;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            renderer = GetComponent<SpriteRenderer>();
            boss = GetComponentInParent<GangDogBoss>();
            lastPosition = transform.position;
        }

        private void LateUpdate()
        {
            Vector2 position = transform.position;
            Vector2 movement = position - lastPosition;
            bool moving = movement.sqrMagnitude > MovementThreshold * MovementThreshold;
            if (moving) facing = movement.normalized;
            CharacterAnimationState state = ResolveAnimation(
                boss != null && boss.IsCharging, boss != null && boss.IsStunned, facing, moving);
            renderer.flipX = false;
            if (state.Hash != currentHash)
            {
                currentHash = state.Hash;
                animator.Play(state.Hash);
            }
            lastPosition = position;
        }

        public static CharacterAnimationState ResolveAnimation(bool charging, bool stunned, Vector2 direction, bool moving)
        {
            string suffix;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) suffix = direction.x < 0f ? "Left" : "Right";
            else suffix = direction.y > 0f ? "Up" : "Down";
            string state = stunned ? "Stun_" + suffix : charging ? "Charge_" + suffix : moving ? "Walk_" + suffix : "Idle_" + suffix;
            string name = "GangDogBoss_" + state;
            return new CharacterAnimationState(Animator.StringToHash(name), name, false);
        }
    }
}
