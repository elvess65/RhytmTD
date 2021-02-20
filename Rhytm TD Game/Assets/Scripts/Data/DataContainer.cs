

using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Data
{
    public class DataContainer
    {
        private Dictionary<string, float> m_Floats;
        private Dictionary<string, int> m_Ints;
        private Dictionary<string, string> m_Strings;
        private Dictionary<string, Vector3> m_Vectors;

        public void AddFloat(string key, float value)
        {
            if (m_Floats == null)
            {
                m_Floats = new Dictionary<string, float>();
            }

            m_Floats.Add(key, value);
        }

        public void RemoveFloat(string key)
        {
            m_Floats.Remove(key);
        }

        public float GetFloat(string key)
        {
            return m_Floats[key];
        }

        public void AddInt(string key, int value)
        {
            if (m_Ints == null)
            {
                m_Ints = new Dictionary<string, int>();
            }

            m_Ints.Add(key, value);
        }

        public void RemoveInt(string key)
        {
            m_Ints.Remove(key);
        }

        public int GetInt(string key)
        {
            return m_Ints[key];
        }

        public void AddString(string key, string value)
        {
            if (m_Strings == null)
            {
                m_Strings = new Dictionary<string, string>();
            }

            m_Strings.Add(key, value);
        }

        public void RemoveString(string key)
        {
            m_Strings.Remove(key);
        }

        public string GetString(string key)
        {
            return m_Strings[key];
        }

        public void AddVector(string key, Vector3 value)
        {
            if (m_Vectors == null)
            {
                m_Vectors = new Dictionary<string, Vector3>();
            }

            m_Vectors.Add(key, value);
        }

        public void RemoveVector(string key)
        {
            m_Vectors.Remove(key);
        }

        public Vector3 GetVector(string key)
        {
            return m_Vectors[key];
        }
    }
}
