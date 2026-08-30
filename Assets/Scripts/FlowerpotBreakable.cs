using UnityEngine;

namespace RPG
{
    public sealed class FlowerpotBreakable : MonoBehaviour
    {
        [SerializeField] private GameObject intactVisual;
        [SerializeField] private GameObject brokenVisual;
        [SerializeField] private GameObject libraryCard;

        public bool IsBroken { get; private set; }
        public event System.Action Broken;

        public bool TryBreak(ThrowableChair chair)
        {
            if (IsBroken || chair == null || !chair.IsFlying)
            {
                return false;
            }

            IsBroken = true;
            if (intactVisual != null)
            {
                intactVisual.SetActive(false);
            }
            if (brokenVisual != null)
            {
                brokenVisual.SetActive(true);
            }
            if (libraryCard != null)
            {
                libraryCard.SetActive(true);
            }
            Broken?.Invoke();
            return true;
        }
    }
}
