using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class BattleEntityView : BaseView
    {
        public int ID => m_BattleEntity.ID;

        protected BattleEntity m_BattleEntity;
        private HealthModule m_HealthModule;

        public virtual void Initialize(BattleEntity entity)
        {
            m_BattleEntity = entity;

            m_HealthModule = entity.GetModule<HealthModule>();
            m_HealthModule.OnHealthRemoved += EnemyView_OnHealthRemoved;
            m_HealthModule.OnDestroyed += EnemyView_OnDestroyed;
        }

        private void EnemyView_OnHealthRemoved(int amount, int senderID)
        {
            Debug.Log($"VIEW: Take {amount} damage from ID: {senderID}. {m_HealthModule.CurrentHealth}/{m_HealthModule.Health}");
        }

        private void EnemyView_OnDestroyed(int id)
        {
            Debug.Log($"VIEW: {ID} was destroyed");
            Destroy(gameObject);
        }
    }
}
