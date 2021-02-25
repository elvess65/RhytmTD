
using System.Collections.Generic;

namespace RhytmTD.Data
{
    public class DataContainer
    {
        private Dictionary<string, object> m_Objects = new Dictionary<string, object>();

        public void AddObject(string key, object data)
        {
            m_Objects.Add(key, data);
        }

        public bool HasObject(string key)
        {
            return m_Objects.ContainsKey(key);
        }

        public T GetObject<T>(string key)
        {
            return (T)m_Objects[key];
        }

        public void RemoveObject(string key)
        {
            m_Objects.Remove(key);
        }
    }
}
