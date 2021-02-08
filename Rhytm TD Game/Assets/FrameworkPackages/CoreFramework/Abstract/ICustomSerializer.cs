

namespace CoreFramework
{
    public interface ICustomSerializer
    {
        string Serialize<T>(T val);
        T Deserialize<T>(string data);
    }
}
