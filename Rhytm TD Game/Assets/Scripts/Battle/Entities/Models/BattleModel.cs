using RhytmTD.Data.Models;
using System.Collections.Generic;

namespace RhytmTD.Battle.Entities.Models
{
    public class BattleModel : BaseModel
    {
        public int ID;
        public int CurrentArea;
        public ICollection<BattleEntity> BattleEntities => m_BattleEntities.Values;

        private Dictionary<int, BattleEntity> m_BattleEntities = new Dictionary<int, BattleEntity>();

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
