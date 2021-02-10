
namespace RhytmTD.Network
{
    public class ConnectionSeccessResult
    {
        public string SerializedAccountData { get; }
        public string SerializedEnviromentData { get; }
        public string SerializedLevelingData { get; }
        public string SerializedWorldData { get; }
        public string SerializedAccountBaseParamsData { get; }

        public ConnectionSeccessResult(string serializedAccountData,
                                       string serializedEnviromentData,
                                       string serializedLevelingData,
                                       string serializedWorldData,
                                       string serializedAccountBaseParamsData)
        {
            SerializedAccountData = serializedAccountData;
            SerializedEnviromentData = serializedEnviromentData;
            SerializedLevelingData = serializedLevelingData;
            SerializedWorldData = serializedWorldData;
            SerializedAccountBaseParamsData = serializedAccountBaseParamsData;
        }
    }
}
