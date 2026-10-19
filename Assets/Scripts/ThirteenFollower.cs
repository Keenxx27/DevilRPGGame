using UnityEngine;

namespace RPG
{
    public sealed class ThirteenFollower : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float followSpeed = 4.2f;
        [SerializeField, Min(0f)] private float minimumDistance = 1.05f;
        [SerializeField] private bool isPersistentCompanion;

        private PlayerInteraction player;

        public static ThirteenFollower PersistentCompanion { get; private set; }

        private void Awake()
        {
            if (isPersistentCompanion) PersistentCompanion = this;
        }

        private void OnEnable()
        {
            MapTransitionService.PlayerArrivedAtDestination += RejoinPlayerAfterTransition;
        }

        private void OnDisable()
        {
            MapTransitionService.PlayerArrivedAtDestination -= RejoinPlayerAfterTransition;
        }

        private void OnDestroy()
        {
            if (PersistentCompanion == this) PersistentCompanion = null;
        }

        public void BeginPersistentFollowing()
        {
            if (PersistentCompanion != null && PersistentCompanion != this)
            {
                Destroy(gameObject);
                return;
            }

            isPersistentCompanion = true;
            PersistentCompanion = this;
            gameObject.name = "十三";
            transform.SetParent(null, true);
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!ThirteenCompanionState.IsTravelingWithPlayer) return;
            if (player == null) player = FindObjectOfType<PlayerInteraction>();
            if (player == null) return;
            DialogueController dialogue = player.GetComponent<DialogueController>();
            if (dialogue != null && dialogue.IsPlaying) return;

            Vector2 delta = (Vector2)player.transform.position - (Vector2)transform.position;
            if (delta.magnitude <= minimumDistance) return;
            Vector2 destination = (Vector2)player.transform.position - delta.normalized * minimumDistance;
            transform.position = Vector2.MoveTowards(transform.position, destination,
                followSpeed * Time.deltaTime);
        }

        private void RejoinPlayerAfterTransition(Transform playerTransform)
        {
            if (!isPersistentCompanion || !ThirteenCompanionState.IsTravelingWithPlayer
                || playerTransform == null) return;

            player = playerTransform.GetComponent<PlayerInteraction>();
            transform.position = (Vector2)playerTransform.position + Vector2.left * minimumDistance;
        }
    }
}
