using UnityEngine;

namespace RPG
{
    public sealed class ThirteenFollower : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float followSpeed = 4.2f;
        [SerializeField, Min(0f)] private float minimumDistance = 1.05f;

        private PlayerInteraction player;

        private void Update()
        {
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
    }
}
