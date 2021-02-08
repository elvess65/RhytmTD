using RhytmTD.Data.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Models
{
    public class CameraModel : BaseModel
    {
        private Vector3 m_Position;
        private Quaternion m_Rotation;

        public System.Action<Vector3> OnPositionChanged;
        public System.Action<Quaternion> OnRotationChanger;

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
                OnRotationChanger?.Invoke(value);
            }
        }
    }
}
