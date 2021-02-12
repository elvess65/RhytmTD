using CoreFramework;
using System.Collections.Generic;

namespace RhytmTD.Battle.Entities.Models
{
    public class BattleModel : BaseModel
    {
        public int ID;
        public int CurrentArea;
        public ICollection<BattleEntity> BattleEntities => m_BattleEntities.Values;

        private Dictionary<int, BattleEntity> m_BattleEntities = new Dictionary<int, BattleEntity>();
        private BattleEntity m_PlayerEntity;

        public System.Action<BattleEntity> OnPlayerEntityInitialized;
        public System.Action<bool> OnBattleFinished;
        public System.Action OnBattleStarted;
        public System.Action OnBattleInitialize;

        public BattleEntity PlayerEntity
        {
            get { return m_PlayerEntity; }
            set
            {
                m_PlayerEntity = value;

                if (value != null)
                    OnPlayerEntityInitialized?.Invoke(value);
            }
        }

        public void AddBattleEntity(BattleEntity battleEntity)
        {
            m_BattleEntities.Add(battleEntity.ID, battleEntity);
        }

        public void RemoveBattleEntity(int entityID)
        {
            m_BattleEntities.Remove(entityID);
        }

        public BattleEntity GetEntity(int entityID)
        {
            return m_BattleEntities[entityID];
        }
    }
}
