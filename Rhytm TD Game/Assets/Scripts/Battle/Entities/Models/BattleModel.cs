using CoreFramework;
using System;
using System.Collections.Generic;

namespace RhytmTD.Battle.Entities.Models
{
    public class BattleModel : BaseModel, IDisposable
    {
        public int ID;
        public int CurrentArea;
        public ICollection<BattleEntity> BattleEntities => m_BattleEntities.Values;

        private Dictionary<int, BattleEntity> m_BattleEntities = new Dictionary<int, BattleEntity>();
        private BattleEntity m_PlayerEntity;

        public Action<BattleEntity> OnPlayerEntityInitialized;
        public Action<bool> OnBattleFinished;
        public Action OnBattleStarted;
        public Action OnBattleInitialize;

        public BattleModel()
        {
            Dispatcher.Instance.AddDisposable(this);
        }

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

        public bool HasEntity(int entityID)
        {
            return m_BattleEntities.ContainsKey(entityID);
        }

        public void Dispose()
        {
            m_PlayerEntity = null;
            m_BattleEntities.Clear();
        }
    }
}
