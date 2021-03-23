using System;
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Holds current and max health as well as event of changing current health
    /// </summary>
    public class HealthModule : IBattleModule
    {
        public int TotalHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;

        private BattleEntity m_BattleEntity;

        public Action<int> OnHealthAdded;
        public Action<int, int> OnHealthRemoved;

        /// <summary>
        /// Holds current and max health as well as event of changing current health
        /// </summary>
        public HealthModule(BattleEntity battleEntity, int health)
        {
            m_BattleEntity = battleEntity;
            TotalHealth = CurrentHealth = health;
        }

        public void AddHealth(int health)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + health, TotalHealth);
            OnHealthAdded?.Invoke(CurrentHealth);
        }

        public void RemoveHealth(int health, int senderID)
        {
            if (CurrentHealth > 0)
            {
                CurrentHealth = Mathf.Max(CurrentHealth - health, 0);

                OnHealthRemoved?.Invoke(CurrentHealth, senderID);

                if (CurrentHealth <= 0 && m_BattleEntity.HasModule<DestroyModule>())
                {
                    DestroyModule destroyModule = m_BattleEntity.GetModule<DestroyModule>();
                    destroyModule.SetDestroyed(true);
                }
            }
        }
    }
}
