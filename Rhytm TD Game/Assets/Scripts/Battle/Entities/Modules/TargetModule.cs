
using System;

namespace RhytmTD.Battle.Entities
{
    public class TargetModule : IBattleModule
    {
        private int m_TargetID;
        private TransformModule m_TargetTransform;

        public int TargetID => m_TargetID;
        public TransformModule TargetTransform => m_TargetTransform;
        public bool HasTarget => m_TargetTransform != null;

        public Action<TransformModule> OnTargetChanged;

        public TargetModule()
        {
        }

        public void SetTarget(int targetID, TransformModule target)
        {
            m_TargetID = targetID;
            m_TargetTransform = target;

            OnTargetChanged?.Invoke(target);
        }

        public void ClearTarget()
        {
            m_TargetTransform = null;
        }
    }
}
