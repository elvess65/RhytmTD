using System.Collections.Generic;
using CoreFramework.Abstract;

namespace CoreFramework
{
    /// <summary>
    /// Updates updatables
    /// </summary>
    public class UpdatablesManager : iUpdatable
    {
        private List<iUpdatable> m_Updateables;

        public UpdatablesManager()
        {
            m_Updateables = new List<iUpdatable>();
        }

        public void Add(iUpdatable updatable)
        {
            m_Updateables.Add(updatable);
        }

        public void Remove(iUpdatable updatable)
        {
            if (m_Updateables.Contains(updatable))
                m_Updateables.Remove(updatable);
        }

        public virtual void PerformUpdate(float deltaTime)
        {
            if (m_Updateables != null)
            {
                for (int i = 0; i < m_Updateables.Count; i++)
                    m_Updateables[i].PerformUpdate(deltaTime);
            }
        }
    }
}
