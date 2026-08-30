using UnityEngine;

namespace RPG
{
    public enum DoorState
    {
        Closed,
        Opening,
        Open,
        Closing
    }

    public sealed class DoorMotion
    {
        private readonly float closedAngle;
        private readonly float openAngle;

        public DoorMotion(float closedAngle, float openAngle)
        {
            this.closedAngle = closedAngle;
            this.openAngle = openAngle;
            CurrentAngle = closedAngle;
            State = DoorState.Closed;
        }

        public float CurrentAngle { get; private set; }
        public DoorState State { get; private set; }
        public bool IsFullyOpen => State == DoorState.Open;
        public bool BlocksPassage => !IsFullyOpen;

        public bool Toggle()
        {
            if (State == DoorState.Closed)
            {
                State = DoorState.Opening;
            }
            else if (State == DoorState.Open)
            {
                State = DoorState.Closing;
            }
            else
            {
                return false;
            }

            return true;
        }

        public void Step(float degreesPerSecond, float deltaTime)
        {
            if (State != DoorState.Opening && State != DoorState.Closing)
            {
                return;
            }

            float target = State == DoorState.Opening ? openAngle : closedAngle;
            float maximumDelta = Mathf.Max(0f, degreesPerSecond) * Mathf.Max(0f, deltaTime);
            CurrentAngle = Mathf.MoveTowardsAngle(CurrentAngle, target, maximumDelta);
            if (Mathf.Abs(Mathf.DeltaAngle(CurrentAngle, target)) > 0.001f)
            {
                return;
            }

            CurrentAngle = target;
            State = State == DoorState.Opening ? DoorState.Open : DoorState.Closed;
        }
    }
}
