using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Incapsulates move speed and direction as well as events on start/stop movement
    /// Could be applies for each entity that requires movement
    /// </summary>
    public class MoveModule : IBattleModule
    {
        public System.Action OnMoveStarted;
        public System.Action OnMoveStopped;

        public float Speed { get; }
        public float CurrentSpeed { get; }

        public Vector3 MoveDirection { get; private set; }
        public bool IsMoving { get; private set; }

        /// <summary>
        /// Incapsulates move speed and direction as well as events on start/stop movement
        /// Could be applies for each entity that requires movement
        /// </summary>
        public MoveModule(float speed)
        {
            Speed = CurrentSpeed = speed;
        }

        public void StartMove(Vector3 direction)
        {
            MoveDirection = direction;
            IsMoving = true;

            OnMoveStarted?.Invoke();
        }

        public void Stop()
        {
            IsMoving = false;
            MoveDirection = Vector3.zero;

            OnMoveStopped?.Invoke();
        }
    }
}
