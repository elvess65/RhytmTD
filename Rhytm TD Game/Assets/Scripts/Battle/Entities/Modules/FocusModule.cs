
using System;

namespace RhytmTD.Battle.Entities
{
    public class FocusModule : IBattleModule
    {
        private TransformModule m_TargetTransform;
        private bool m_IsFocusing;

        public TransformModule TargetTransform => m_TargetTransform;
        public bool IsFocusing => m_IsFocusing;

        public Action<int> OnFocusTargetChanged;

        public FocusModule()
        {
            m_IsFocusing = false;
        }

        public void StartFocusOnTarget(int targetID, TransformModule focusTarget)
        {
            m_TargetTransform = focusTarget;
            m_IsFocusing = true;

            OnFocusTargetChanged?.Invoke(targetID);
        }

        public void StopFocus()
        {
            m_TargetTransform = null;
            m_IsFocusing = false;
        }
    }
}
