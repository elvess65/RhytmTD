namespace CoreFramework.Network
{
    public partial class ConnectionSuccessResult
    {
        public string SerializedAccountData { get; }
        public string SerializedEnviromentData { get; }
        public string SerializedLevelingData { get; }
        public string SerializedWorldData { get; }
        public string SerializedAccountBaseParamsData { get; }
        public string SerializedSkillSequennceData { get; }

        public ConnectionSuccessResult(string serializedAccountData,
                                       string serializedEnviromentData,
                                       string serializedLevelingData,
                                       string serializedWorldData,
                                       string serializedAccountBaseParamsData,
                                       string serializedSkillSequennceData)
        {
            SerializedAccountData = serializedAccountData;
            SerializedEnviromentData = serializedEnviromentData;
            SerializedLevelingData = serializedLevelingData;
            SerializedWorldData = serializedWorldData;
            SerializedAccountBaseParamsData = serializedAccountBaseParamsData;
            SerializedSkillSequennceData = serializedSkillSequennceData;
        }
    }
}
