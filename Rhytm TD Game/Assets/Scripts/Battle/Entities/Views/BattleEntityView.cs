using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class BattleEntityView : BaseView
    {
        public int ID => m_BattleEntity.ID;

        protected BattleEntity m_BattleEntity;

        private HealthModule m_HealthModule;
        private DestroyModule m_DestroyModule;

        public virtual void Initialize(BattleEntity entity)
        {
            m_BattleEntity = entity;

            m_HealthModule = entity.GetModule<HealthModule>();
            m_DestroyModule = entity.GetModule<DestroyModule>();

            m_HealthModule.OnHealthRemoved += OnHealthRemoved;
            m_DestroyModule.OnDestroyed += OnDestroyed;
        }

        private void OnHealthRemoved(int amount, int senderID)
        {
            Debug.Log($"VIEW: Take {amount} damage from ID: {senderID}. {m_HealthModule.CurrentHealth}/{m_HealthModule.Health}");

            transform.localScale = Vector3.one * 0.5f;
        }

        private void OnDestroyed(BattleEntity entity)
        {
            Debug.Log($"VIEW: {entity.ID} was destroyed");

            transform.localScale = Vector3.one * 0.1f;
        }
    }
}
