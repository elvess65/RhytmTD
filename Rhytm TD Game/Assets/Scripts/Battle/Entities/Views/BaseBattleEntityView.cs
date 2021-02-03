using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class BaseBattleEntityView : MonoBehaviour
    {
        public int ID => m_BattleEntity.ID;

        protected BattleEntity m_BattleEntity;
        
        public void Initialize(BattleEntity entity)
        {
            m_BattleEntity = entity;
        }
    }
}
