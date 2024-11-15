﻿
using System;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Holds data about current entity's target and provides event on changing it
    /// </summary>
    public class TargetModule : IBattleModule
    {
        private BattleEntity m_Target;

        public BattleEntity Target => m_Target;
        public bool HasTarget => m_Target != null;

        public Action<BattleEntity> OnTargetChanged;

        /// <summary>
        /// Holds data about current entity's target and provides event on changing it
        /// </summary>
        public TargetModule()
        {
        }

        public void SetTarget(BattleEntity battleEntity)
        {
            m_Target = battleEntity;

            OnTargetChanged?.Invoke(battleEntity);
        }

        public void ClearTarget()
        {
            m_Target = null;
            OnTargetChanged?.Invoke(null);
        }
    }
}
