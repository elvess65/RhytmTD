
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Передвижение
    /// </summary>
    public class MoveModule : IBattleModule
    {
        public float Speed { get; }
        public float CurrentSpeed { get; }

        public Vector3 MoveDirection { get; private set; }
        public bool IsMoving { get; private set; }

        public Vector3 Position
        {
            get { return m_Position; }
            set 
            {
                m_Position = value;
                OnPositionChanged?.Invoke(value);
            }
        }

        private Vector3 m_Position;

        public System.Action<Vector3> OnPositionChanged;

        public MoveModule(Vector3 position, float speed)
        {
            Position = position;
            Speed = CurrentSpeed = speed;
        }

        public void StartMove(Vector3 direction)
        {
            if (IsMoving)
                return;

            MoveDirection = direction;
            IsMoving = true;
        }

        public void Stop()
        {
            if (!IsMoving)
                return;

            IsMoving = false;
            MoveDirection = Vector3.zero;
        }
    }
}
