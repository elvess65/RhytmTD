using System.Collections.Generic;

namespace RhytmTD.Battle.Entities
{
    public class BattleEntity
    {
        public int ID { get; }

        private Dictionary<System.Type, IBattleModule> m_BattleModules = new Dictionary<System.Type, IBattleModule>();

        public BattleEntity(int id)
        {
            ID = id;
        }

        public void AddNodule(IBattleModule module)
        {
            m_BattleModules.Add(module.GetType(), module);
        }

        public void RemoveModule<T>()
        {
            m_BattleModules.Remove(typeof(T));
        }

        public T GetModule<T>() where T : IBattleModule
        {
            return (T)m_BattleModules[typeof(T)];
        }

        public bool HasModule<T>()
        {
            return m_BattleModules.ContainsKey(typeof(T));
        }
    }
}
