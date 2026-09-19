using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public sealed class GangDogBoss : MonoBehaviour
    {
        private Rigidbody2D body;
        private PlayerInteraction player;
        private RubbleBossEncounter encounter;
        private bool active;
        private bool temporarilyPaused;
        private float chargeCooldown;
        private float prepareTime;
        private float chargeTime;
        private float stunTime;
        private Vector2 chargeDirection;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
        }

        public void BeginBattle(RubbleBossEncounter owner, Vector2 startPosition)
        {
            encounter = owner;
            active = true;
            ResetForRound(startPosition);
        }

        public void ResetForRound(Vector2 startPosition)
        {
            body.position = startPosition;
            body.velocity = Vector2.zero;
            chargeCooldown = 1.5f;
            prepareTime = 0f;
            chargeTime = 0f;
            stunTime = 0f;
        }

        public void EndBattle()
        {
            active = false;
            body.velocity = Vector2.zero;
        }

        public void SetTemporarilyPaused(bool paused)
        {
            temporarilyPaused = paused;
            if (paused) body.velocity = Vector2.zero;
        }

        private void Update()
        {
            if (player == null) player = FindObjectOfType<PlayerInteraction>();
            if (!active || temporarilyPaused || player == null) return;
            chargeCooldown = Mathf.Max(0f, chargeCooldown - Time.deltaTime);
            stunTime = Mathf.Max(0f, stunTime - Time.deltaTime);
            if (prepareTime > 0f)
            {
                prepareTime -= Time.deltaTime;
                if (prepareTime <= 0f) chargeTime = .58f;
                return;
            }

            if (chargeTime > 0f)
            {
                chargeTime -= Time.deltaTime;
                return;
            }

            if (stunTime > 0f) return;
            Vector2 delta = (Vector2)player.transform.position - body.position;
            bool linedUp = Mathf.Abs(delta.x) < .42f || Mathf.Abs(delta.y) < .42f;
            if (chargeCooldown <= 0f && linedUp && delta.magnitude > 1.15f)
            {
                chargeDirection = delta.normalized;
                prepareTime = .42f;
                chargeCooldown = 2.7f;
            }
        }

        private void FixedUpdate()
        {
            if (!active || temporarilyPaused || player == null || stunTime > 0f || prepareTime > 0f) return;
            Vector2 destination;
            if (chargeTime > 0f)
            {
                destination = body.position + chargeDirection * 9.5f * Time.fixedDeltaTime;
            }
            else
            {
                destination = Vector2.MoveTowards(body.position, player.transform.position,
                    3.05f * Time.fixedDeltaTime);
            }

            body.MovePosition(destination);
            if (Vector2.Distance(body.position, player.transform.position) <= .78f)
            {
                encounter?.OnBossCapture();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!active || chargeTime <= 0f) return;
            if (collision.collider.GetComponentInParent<PlayerInteraction>() != null) return;
            chargeTime = 0f;
            stunTime = .85f;
        }
    }
}
