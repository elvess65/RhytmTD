
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

        public MoveModule(float speed)
        {
            Speed = CurrentSpeed = speed;
        }

        public void StartMove(Vector3 direction)
        {
            MoveDirection = direction;
            IsMoving = true;
        }

        public void Stop()
        {
            IsMoving = false;
            MoveDirection = Vector3.zero;
        }
    }
}
