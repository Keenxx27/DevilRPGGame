using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MainCharacterMovement : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveSpeed = 5f;

        private Rigidbody2D body;
        private Vector2 input;

        public Vector2 FacingDirection { get; private set; } = Vector2.down;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            input = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));
            FacingDirection = CalculateFacingDirection(FacingDirection, input);
        }

        private void FixedUpdate()
        {
            Vector3 delta = CalculateDelta(input, moveSpeed, Time.fixedDeltaTime);
            body.MovePosition(body.position + new Vector2(delta.x, delta.y));
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
