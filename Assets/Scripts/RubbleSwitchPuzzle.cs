using UnityEngine;

namespace RPG
{
    public sealed class RubbleSwitchPuzzle : MonoBehaviour
    {
        private static bool gatePermanentlyOpen;

        [SerializeField] private Collider2D ironGateCollider;
        [SerializeField] private SpriteRenderer ironGateRenderer;
        [SerializeField] private RubbleSwitch[] switches;
        [SerializeField] private GameObject closedGateArtwork;
        [SerializeField] private GameObject openGateArtwork;

        public void Configure(Collider2D gateCollider, SpriteRenderer gateRenderer,
            RubbleSwitch[] puzzleSwitches)
        {
            ironGateCollider = gateCollider;
            ironGateRenderer = gateRenderer;
            switches = puzzleSwitches;
            if (switches == null) return;
            foreach (RubbleSwitch puzzleSwitch in switches)
            {
                if (puzzleSwitch != null) puzzleSwitch.SetPuzzle(this);
            }
        }

        public void ConfigureArtwork(GameObject closedArtwork, GameObject openArtwork)
        {
            closedGateArtwork = closedArtwork;
            openGateArtwork = openArtwork;
            SetGateArtwork(gatePermanentlyOpen);
        }

        private void Start()
        {
            if (gatePermanentlyOpen) OpenGate();
        }

        public bool Activate(RubbleSwitch puzzleSwitch)
        {
            if (puzzleSwitch == null || puzzleSwitch.IsActivated) return false;
            puzzleSwitch.SetActivated(true);
            UiAudio.PlaySwitchPressed();
            GetComponent<RubbleBossEncounter>()?.RefreshProgress();
            if (AllSwitchesActivated())
            {
                gatePermanentlyOpen = true;
                OpenGate();
                UiAudio.PlayIronGateOpen();
                GetComponent<RubbleBossEncounter>()?.CompleteBattle();
            }

            return true;
        }

        public void ResetForNewRound()
        {
            if (gatePermanentlyOpen) return;
            if (ironGateCollider != null) ironGateCollider.enabled = true;
            MapTransitionPoint exit = ironGateCollider != null
                ? ironGateCollider.GetComponent<MapTransitionPoint>() : null;
            if (exit != null) exit.enabled = false;
            if (ironGateRenderer != null) ironGateRenderer.color = new Color(.34f, .2f, .12f, 1f);
            SetGateArtwork(false);
            if (switches == null) return;
            foreach (RubbleSwitch puzzleSwitch in switches)
            {
                if (puzzleSwitch != null) puzzleSwitch.SetActivated(false);
            }
        }

        public int ActivatedCount
        {
            get
            {
                if (switches == null) return 0;
                int count = 0;
                foreach (RubbleSwitch puzzleSwitch in switches)
                {
                    if (puzzleSwitch != null && puzzleSwitch.IsActivated) count++;
                }

                return count;
            }
        }

        private bool AllSwitchesActivated()
        {
            if (switches == null || switches.Length != 4) return false;
            foreach (RubbleSwitch puzzleSwitch in switches)
            {
                if (puzzleSwitch == null || !puzzleSwitch.IsActivated) return false;
            }

            return true;
        }

        private void OpenGate()
        {
            if (ironGateCollider != null) ironGateCollider.enabled = false;
            MapTransitionPoint exit = ironGateCollider != null
                ? ironGateCollider.GetComponent<MapTransitionPoint>() : null;
            if (exit != null) exit.enabled = true;
            if (ironGateRenderer != null) ironGateRenderer.color = new Color(.34f, .2f, .12f, .5f);
            SetGateArtwork(true);
            if (switches == null) return;
            foreach (RubbleSwitch puzzleSwitch in switches)
            {
                if (puzzleSwitch != null) puzzleSwitch.SetActivated(true);
            }
        }

        private void SetGateArtwork(bool isOpen)
        {
            if (closedGateArtwork != null) closedGateArtwork.SetActive(!isOpen);
            if (openGateArtwork != null) openGateArtwork.SetActive(isOpen);
        }
    }

    public sealed class RubbleSwitch : MonoBehaviour, IInvestigationHandler
    {
        [SerializeField] private RubbleSwitchPuzzle puzzle;
        [SerializeField] private SpriteRenderer visual;
        [SerializeField] private bool activated;
        [SerializeField] private GameObject idleArtwork;
        [SerializeField] private GameObject pressedArtwork;

        public bool IsActivated => activated;

        public void Configure(RubbleSwitchPuzzle owner, SpriteRenderer renderer)
        {
            puzzle = owner;
            visual = renderer;
            SetActivated(false);
        }

        public void SetPuzzle(RubbleSwitchPuzzle owner)
        {
            puzzle = owner;
        }

        public void ConfigureArtwork(GameObject idleStateArtwork, GameObject pressedStateArtwork)
        {
            idleArtwork = idleStateArtwork;
            pressedArtwork = pressedStateArtwork;
            ApplyArtworkState();
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.RubbleSwitch && !activated && puzzle != null
                && dialogue != null && !dialogue.IsPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            return CanInvestigate(kind) && puzzle.Activate(this);
        }

        public void SetActivated(bool value)
        {
            activated = value;
            if (visual != null)
            {
                visual.color = activated
                    ? new Color(.32f, .92f, .45f, 1f)
                    : new Color(.82f, .32f, .12f, 1f);
            }
            ApplyArtworkState();
        }

        private void ApplyArtworkState()
        {
            if (idleArtwork != null) idleArtwork.SetActive(!activated);
            if (pressedArtwork != null) pressedArtwork.SetActive(activated);
        }
    }
}
