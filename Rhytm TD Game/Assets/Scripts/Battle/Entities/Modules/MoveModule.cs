
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Передвижение
    /// </summary>
    public class MoveModule : IBattleModule
    {
        public Transform Transform { get; private set; }
        public float Speed { get; private set; }
        public float CurrentSpeed { get; private set; }
        public Vector3 MoveDirection { get; private set; }
        public bool IsMoving { get; private set; }

        public System.Action OnStartMove;
        public System.Action OnStopMove;

        public MoveModule(Transform transform, float speed)
        {
            Transform = transform;
            Speed = CurrentSpeed = speed;
        }

        public void StartMove(Vector3 direction)
        {
            if (IsMoving)
                return;

            MoveDirection = direction;
            IsMoving = true;

            OnStartMove?.Invoke();
        }

        public void Stop()
        {
            if (!IsMoving)
                return;

            IsMoving = false;
            MoveDirection = Vector3.zero;

            OnStopMove?.Invoke();
        }
    }
}
