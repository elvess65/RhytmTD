
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    public class MoveModule : IBattleModule
    {
        public Transform Transform { get; private set; }
        public float Speed { get; private set; }
        public float CurrentSpeed { get; private set; }
        public Vector3 MoveDirection { get; private set; }
        public bool IsMoving { get; private set; }

        public MoveModule(Transform transform, float speed)
        {
            Transform = transform;
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
