
using System;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Holds data about current focusing target and provides method for starting/stopping focusing
    /// </summary>
    public class FocusModule : IBattleModule
    {
        private TransformModule m_TargetTransform;
        private bool m_IsFocusing;

        public TransformModule TargetTransform => m_TargetTransform;
        public bool IsFocusing => m_IsFocusing;

        public Action<int> OnFocusTargetChanged;

        /// <summary>
        /// Holds data about current focusing target and provides method for starting/stopping focusing
        /// </summary>
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
