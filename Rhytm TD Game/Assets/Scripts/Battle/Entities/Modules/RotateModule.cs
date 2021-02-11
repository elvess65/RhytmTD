
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    public class RotateModule : IBattleModule
    {
        public float Speed { get; }
        public float CurrentSpeed { get; }

        public Quaternion Destination { get; private set; }
        public bool IsRotating { get; private set; }

        public RotateModule(float speed)
        {
            Speed = CurrentSpeed = speed;
        }

        public void StartRotate(Quaternion destination)
        {
            Destination = destination;
            IsRotating = true;
        }

        public void Stop()
        {
            IsRotating = false;
        }
    }
}
