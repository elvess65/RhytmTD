using System;
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    public class HealthModule : IBattleModule
    {
        public int Health { get; private set; }
        public int CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;

        private BattleEntity m_BattleEntity;

        public Action<int, int> OnHealthRemoved;

        public HealthModule(BattleEntity battleEntity, int health)
        {
            m_BattleEntity = battleEntity;
            Health = CurrentHealth = health;
        }

        public void AddHealth(int health)
        {
            CurrentHealth += health;
        }

        public void RemoveHealth(int health, int senderID)
        {
            if (CurrentHealth > 0)
            {
                CurrentHealth = Mathf.Max(CurrentHealth - health, 0);

                OnHealthRemoved?.Invoke(health, senderID);

                if (CurrentHealth <= 0 && m_BattleEntity.HasModule<DestroyModule>())
                {
                    DestroyModule destroyModule = m_BattleEntity.GetModule<DestroyModule>();
                    destroyModule.SetDestroyed(true);
                }
            }
        }
    }
}
