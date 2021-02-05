using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class BattleEntityView : BaseView
    {
        public int ID => m_BattleEntity.ID;

        protected BattleEntity m_BattleEntity;

        private HealthModule m_HealthModule;
        private TransformModule m_TransformModule;

        public virtual void Initialize(BattleEntity entity)
        {
            m_BattleEntity = entity;

            m_HealthModule = entity.GetModule<HealthModule>();
            m_HealthModule.OnHealthRemoved += OnHealthRemoved;
            m_HealthModule.OnDestroyed += OnDestroyed;

            m_TransformModule = entity.GetModule<TransformModule>();
            m_TransformModule.OnStartFocus += OnStartFocus;
            m_TransformModule.OnStopFocus += OnStopFocus;
        }

        private void OnHealthRemoved(int amount, int senderID)
        {
            Debug.Log($"VIEW: Take {amount} damage from ID: {senderID}. {m_HealthModule.CurrentHealth}/{m_HealthModule.Health}");
        }

        private void OnDestroyed(int id)
        {
            Debug.Log($"VIEW: {ID} was destroyed");
            Destroy(gameObject);
        }

        private void OnStartFocus()
        {
            Debug.Log($"VIEW: Start focus");
        }

        private void OnStopFocus()
        {
            Debug.Log($"VIEW: Stop move");
        }
    }
}
