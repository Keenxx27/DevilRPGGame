using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(WorldItem), typeof(BoxCollider2D))]
    public sealed class ThrowableChair : MonoBehaviour
    {
        [SerializeField, Min(0.01f)] private float flightDuration = 0.3f;
        [SerializeField, Min(0.01f)] private float flightDistance = 2f;

        private WorldItem worldItem;
        private BoxCollider2D chairCollider;
        private Vector2 startPosition;
        private Vector2 direction;
        private float elapsed;

        public bool IsFlying { get; private set; }

        private void Awake()
        {
            worldItem = GetComponent<WorldItem>();
            chairCollider = GetComponent<BoxCollider2D>();
            chairCollider.isTrigger = true;
        }

        private void Update()
        {
            AdvanceFlight(Time.deltaTime);
        }

        public bool BeginThrow(Vector2 facing)
        {
            return BeginThrow(transform.position, facing);
        }

        public bool BeginThrow(Vector2 origin, Vector2 facing)
        {
            if (IsFlying || worldItem == null)
            {
                return false;
            }

            direction = facing.sqrMagnitude > 0f ? facing.normalized : Vector2.down;
            startPosition = origin;
            elapsed = 0f;
            transform.position = origin;
            worldItem.SetPickupEnabled(false);
            gameObject.SetActive(true);
            IsFlying = true;
            return true;
        }

        public void AdvanceFlight(float deltaTime)
        {
            if (!IsFlying || deltaTime <= 0f)
            {
                return;
            }

            elapsed = Mathf.Min(elapsed + deltaTime, flightDuration);
            float progress = flightDuration > 0f ? elapsed / flightDuration : 1f;
            Vector2 destination = startPosition + direction * (flightDistance * progress);

            if (TryFindCollision(transform.position, destination,
                out Vector2 stopPosition, out FlowerpotBreakable flowerpot))
            {
                transform.position = stopPosition;
                flowerpot?.TryBreak(this);
                FinishFlight();
                return;
            }

            transform.position = destination;
            if (elapsed >= flightDuration)
            {
                FinishFlight();
            }
        }

        private bool TryFindCollision(
            Vector2 from,
            Vector2 to,
            out Vector2 stopPosition,
            out FlowerpotBreakable flowerpot)
        {
            stopPosition = from;
            flowerpot = null;
            Vector2 delta = to - from;
            float distance = delta.magnitude;
            if (distance <= 0f || chairCollider == null)
            {
                return false;
            }

            Vector2 size = Vector2.Scale(chairCollider.size,
                new Vector2(Mathf.Abs(transform.lossyScale.x),
                    Mathf.Abs(transform.lossyScale.y)));
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(Physics2D.AllLayers);
            filter.useTriggers = true;
            RaycastHit2D[] hits = new RaycastHit2D[32];
            int hitCount = Physics2D.BoxCast(
                from, size, transform.eulerAngles.z, delta.normalized,
                filter, hits, distance);
            float nearestDistance = float.PositiveInfinity;

            for (int index = 0; index < hitCount; index++)
            {
                RaycastHit2D hit = hits[index];
                Collider2D other = hit.collider;
                if (other == null || other == chairCollider
                    || other.GetComponentInParent<ThrowableChair>() != null
                    || other.GetComponentInParent<PlayerInventory>() != null)
                {
                    continue;
                }

                FlowerpotBreakable hitFlowerpot =
                    other.GetComponentInParent<FlowerpotBreakable>();
                if (other.isTrigger && hitFlowerpot == null)
                {
                    continue;
                }

                if (hit.distance < nearestDistance)
                {
                    nearestDistance = hit.distance;
                    stopPosition = hit.centroid;
                    flowerpot = hitFlowerpot;
                }
            }

            Vector2 halfSize = size * 0.5f;
            foreach (Collider2D candidateCollider in
                Object.FindObjectsOfType<Collider2D>())
            {
                if (candidateCollider == chairCollider
                    || candidateCollider.GetComponentInParent<ThrowableChair>() != null
                    || candidateCollider.GetComponentInParent<PlayerInventory>() != null)
                {
                    continue;
                }

                FlowerpotBreakable candidateFlowerpot =
                    candidateCollider.GetComponentInParent<FlowerpotBreakable>();
                if (!candidateCollider.enabled
                    || (candidateCollider.isTrigger && candidateFlowerpot == null)
                    || !TryIntersectExpandedBounds(
                        from, to, candidateCollider.bounds, halfSize,
                        out float fraction))
                {
                    continue;
                }

                float candidateDistance = distance * fraction;
                if (candidateDistance < nearestDistance)
                {
                    nearestDistance = candidateDistance;
                    stopPosition = Vector2.Lerp(from, to, fraction);
                    flowerpot = candidateFlowerpot;
                }
            }

            return nearestDistance < float.PositiveInfinity;
        }

        private static bool TryIntersectExpandedBounds(
            Vector2 from,
            Vector2 to,
            Bounds bounds,
            Vector2 expansion,
            out float fraction)
        {
            Vector2 minimum = (Vector2)bounds.min - expansion;
            Vector2 maximum = (Vector2)bounds.max + expansion;
            Vector2 delta = to - from;
            float entry = 0f;
            float exit = 1f;

            if (!UpdateInterval(from.x, delta.x, minimum.x, maximum.x,
                    ref entry, ref exit)
                || !UpdateInterval(from.y, delta.y, minimum.y, maximum.y,
                    ref entry, ref exit))
            {
                fraction = 0f;
                return false;
            }

            fraction = entry;
            return true;
        }

        private static bool UpdateInterval(
            float origin,
            float delta,
            float minimum,
            float maximum,
            ref float entry,
            ref float exit)
        {
            if (Mathf.Abs(delta) < 0.0001f)
            {
                return origin >= minimum && origin <= maximum;
            }

            float first = (minimum - origin) / delta;
            float second = (maximum - origin) / delta;
            if (first > second)
            {
                float swap = first;
                first = second;
                second = swap;
            }

            entry = Mathf.Max(entry, first);
            exit = Mathf.Min(exit, second);
            return entry <= exit;
        }

        private void FinishFlight()
        {
            IsFlying = false;
            worldItem.SetPickupEnabled(true);
        }
    }
}
