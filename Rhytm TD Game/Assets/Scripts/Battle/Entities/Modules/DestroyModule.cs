

using System;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Incapsulates data whether entity is destroyed and provides event 
    /// </summary>
    public class DestroyModule : IBattleModule
    {
        public Action<BattleEntity> OnDestroyed;

        private BattleEntity m_Owner;
        private bool m_IsDestroyed = false;

        public bool IsDestroyed => m_IsDestroyed;

        public DestroyModule(BattleEntity owner)
        {
            m_Owner = owner;
        }

        public void SetDestroyed(bool value)
        {
            m_IsDestroyed = value;

            if (value)
            {
                OnDestroyed?.Invoke(m_Owner);
            }
        }

        public void SetDestroyed()
        {
            SetDestroyed(true);
        }
    }
}
