using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class BattleEntityView : BaseView
    {
        public int ID => m_BattleEntity.ID;

        protected BattleEntity m_BattleEntity;
        
        public virtual void Initialize(BattleEntity entity)
        {
            m_BattleEntity = entity;

            entity.GetModule<HealthModule>().OnHealthRemoved += EnemyView_OnHealthRemoved;
        }

        private void EnemyView_OnHealthRemoved(int amount, int senderID)
        {
            Debug.Log("Take damage " + amount + " form " + senderID);
        }
    }
}
