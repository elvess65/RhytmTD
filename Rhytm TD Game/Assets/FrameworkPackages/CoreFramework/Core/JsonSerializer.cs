using UnityEngine;

namespace CoreFramework
{
    public class JsonSerializer : ICustomSerializer
    {
        public T Deserialize<T>(string data)
        {
            return JsonUtility.FromJson<T>(data);
        }

        public string Serialize<T>(T val)
        {
            return JsonUtility.ToJson(val);
        }
    }
}