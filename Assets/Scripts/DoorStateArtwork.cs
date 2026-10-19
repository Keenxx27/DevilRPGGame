using UnityEngine;

namespace RPG
{
    public sealed class DoorStateArtwork : MonoBehaviour
    {
        [SerializeField] private DoorController door;
        [SerializeField] private SpriteRenderer artworkRenderer;
        [SerializeField] private Sprite closedSprite;
        [SerializeField] private Sprite openSprite;

        private bool showingOpen;

        private void Awake()
        {
            RefreshArtwork();
        }

        private void Update()
        {
            RefreshArtwork();
        }

        private void RefreshArtwork()
        {
            if (door == null || artworkRenderer == null || closedSprite == null || openSprite == null)
            {
                return;
            }

            bool shouldShowOpen = door.State == DoorState.Opening || door.State == DoorState.Open;
            if (artworkRenderer.sprite == (shouldShowOpen ? openSprite : closedSprite))
            {
                showingOpen = shouldShowOpen;
                return;
            }

            showingOpen = shouldShowOpen;
            artworkRenderer.sprite = showingOpen ? openSprite : closedSprite;
        }
    }
}
