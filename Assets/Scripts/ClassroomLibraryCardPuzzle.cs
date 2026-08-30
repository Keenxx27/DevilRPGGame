using UnityEngine;

namespace RPG
{
    public enum ClassroomPuzzleState
    {
        WaitingForOpening,
        PlayingSearchIntro,
        Searching,
        PlayingFlowerpotDiscovery,
        ChairsUnlocked,
        PotBroken,
        Completed
    }

    public sealed class ClassroomLibraryCardPuzzle : MonoBehaviour, IInvestigationHandler
    {
        [SerializeField] private DialogueController dialogueController;
        [SerializeField] private BroadcastDialogueTrigger openingBroadcast;
        [SerializeField] private DialoguePage[] searchIntroPages;
        [SerializeField] private DialoguePage[] storageCabinetPages;
        [SerializeField] private DialoguePage[] podiumPages;
        [SerializeField] private DialoguePage[] teacherDeskPages;
        [SerializeField] private DialoguePage[] blackboardPages;
        [SerializeField] private DialoguePage[] firstFlowerpotPages;
        [SerializeField] private DialoguePage[] repeatFlowerpotPages;
        [SerializeField] private DialoguePage[] flowerpotBrokenPages;
        [SerializeField] private DialoguePage[] cardPickedUpPages;
        [SerializeField] private ThrowableChair[] chairs;
        [SerializeField] private FlowerpotBreakable flowerpot;
        [SerializeField] private WorldItem libraryCard;

        public ClassroomPuzzleState State { get; private set; }

        private void OnEnable()
        {
            if (openingBroadcast == null)
            {
                Debug.LogError(
                    "ClassroomLibraryCardPuzzle requires an opening broadcast.", this);
                return;
            }

            openingBroadcast.Completed += HandleOpeningCompleted;
            if (flowerpot != null)
            {
                flowerpot.Broken += HandleFlowerpotBroken;
            }
            if (libraryCard != null)
            {
                libraryCard.PickedUp += HandleLibraryCardPickedUp;
            }

            if (State == ClassroomPuzzleState.WaitingForOpening)
            {
                SetChairPickupEnabled(false);
            }
        }

        private void OnDisable()
        {
            if (openingBroadcast != null)
            {
                openingBroadcast.Completed -= HandleOpeningCompleted;
            }
            if (flowerpot != null)
            {
                flowerpot.Broken -= HandleFlowerpotBroken;
            }
            if (libraryCard != null)
            {
                libraryCard.PickedUp -= HandleLibraryCardPickedUp;
            }
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            if (kind == InvestigationKind.Flowerpot)
            {
                return State == ClassroomPuzzleState.Searching
                    || State == ClassroomPuzzleState.ChairsUnlocked;
            }

            return IsOrdinaryInvestigationState() && PagesFor(kind) != null;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind) || dialogueController == null)
            {
                return false;
            }

            if (kind == InvestigationKind.Flowerpot)
            {
                return TryInvestigateFlowerpot();
            }

            return dialogueController.StartDialogue(PagesFor(kind));
        }

        private void HandleOpeningCompleted()
        {
            if (State != ClassroomPuzzleState.WaitingForOpening
                || dialogueController == null)
            {
                return;
            }

            State = ClassroomPuzzleState.PlayingSearchIntro;
            if (!dialogueController.StartDialogue(searchIntroPages, FinishSearchIntro)
                && State == ClassroomPuzzleState.PlayingSearchIntro)
            {
                State = ClassroomPuzzleState.WaitingForOpening;
                Debug.LogError("Failed to start the classroom search introduction.", this);
            }
        }

        private void FinishSearchIntro()
        {
            State = ClassroomPuzzleState.Searching;
        }

        private bool TryInvestigateFlowerpot()
        {
            if (State == ClassroomPuzzleState.ChairsUnlocked)
            {
                return dialogueController.StartDialogue(repeatFlowerpotPages);
            }

            State = ClassroomPuzzleState.PlayingFlowerpotDiscovery;
            if (dialogueController.StartDialogue(
                firstFlowerpotPages, FinishFlowerpotDiscovery))
            {
                return true;
            }

            State = ClassroomPuzzleState.Searching;
            return false;
        }

        private void FinishFlowerpotDiscovery()
        {
            SetChairPickupEnabled(true);
            State = ClassroomPuzzleState.ChairsUnlocked;
        }

        private void HandleFlowerpotBroken()
        {
            if (State == ClassroomPuzzleState.PotBroken
                || State == ClassroomPuzzleState.Completed)
            {
                return;
            }

            State = ClassroomPuzzleState.PotBroken;
            if (dialogueController != null
                && !dialogueController.StartDialogue(flowerpotBrokenPages))
            {
                Debug.LogError("Failed to start the flowerpot broken dialogue.", this);
            }
        }

        private void HandleLibraryCardPickedUp(WorldItem pickedUp)
        {
            if (pickedUp != libraryCard || State != ClassroomPuzzleState.PotBroken)
            {
                return;
            }

            State = ClassroomPuzzleState.Completed;
            if (dialogueController != null
                && !dialogueController.StartDialogue(cardPickedUpPages))
            {
                Debug.LogError("Failed to start the library card dialogue.", this);
            }
        }

        private void SetChairPickupEnabled(bool enabled)
        {
            if (chairs == null)
            {
                return;
            }

            foreach (ThrowableChair chair in chairs)
            {
                if (chair != null)
                {
                    chair.GetComponent<WorldItem>()?.SetPickupEnabled(enabled);
                }
            }
        }

        private bool IsOrdinaryInvestigationState()
        {
            return State == ClassroomPuzzleState.Searching
                || State == ClassroomPuzzleState.ChairsUnlocked
                || State == ClassroomPuzzleState.PotBroken
                || State == ClassroomPuzzleState.Completed;
        }

        private DialoguePage[] PagesFor(InvestigationKind kind)
        {
            switch (kind)
            {
                case InvestigationKind.StorageCabinet:
                    return storageCabinetPages;
                case InvestigationKind.Podium:
                    return podiumPages;
                case InvestigationKind.TeacherDesk:
                    return teacherDeskPages;
                case InvestigationKind.Blackboard:
                    return blackboardPages;
                default:
                    return null;
            }
        }
    }
}
