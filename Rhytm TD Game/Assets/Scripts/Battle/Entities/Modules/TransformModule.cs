using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    public class TransformModule : IBattleModule
    {
        private Vector3 m_Position;
        private Quaternion m_Rotation;

        public Vector3 Position
        {
            get { return m_Position; }
            set
            {
                m_Position = value;
                OnPositionChanged?.Invoke(value);
            }
        }

        public Quaternion Rotation
        {
            get { return m_Rotation; }
            set
            {
                m_Rotation = value;
                OnRotationChanged?.Invoke(value);
            }
        }

        public System.Action<Vector3> OnPositionChanged;
        public System.Action<Quaternion> OnRotationChanged;

        public TransformModule(Vector3 position, Quaternion rotation)
        {
            m_Position = position;
            m_Rotation = rotation;
        }
    }
}
