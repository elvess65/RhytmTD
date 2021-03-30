using RhytmTD.Setup;

namespace CoreFramework.Network
{
    public partial class ConnectionController
    {
        partial void Setup(ConnectionSuccessResult connectionResult)
        {
            IGameSetup gameSetup = new GameSetup(new DataGameSetup(connectionResult.SerializedAccountData,
                                                                   connectionResult.SerializedEnviromentData,
                                                                   connectionResult.SerializedLevelingData,
                                                                   connectionResult.SerializedWorldData,
                                                                   connectionResult.SerializedAccountBaseParamsData,
                                                                   connectionResult.SerializedSkillSequennceData),
                                                new BattleGameSetup());
            gameSetup.Setup();
        }
    }
}
