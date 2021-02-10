using CoreFramework;
using RhytmTD.Data.Controllers;
using RhytmTD.Data.Models;

namespace RhytmTD.Battle.Core
{
    public class GameSetup : IGameSetup
    {
        private IGameSetup m_MetaSetup;
        private IGameSetup m_BattleSetup;

        public GameSetup(IGameSetup metaSetup, IGameSetup battleSetup)
        {
            m_MetaSetup = metaSetup;
            m_BattleSetup = battleSetup;
        }

        public void Setup()
        {
            Dispatcher dispatcher = Dispatcher.Instance;

            // Models
            dispatcher.CreateModel<UpdateModel>();

            // Controllers
            dispatcher.CreateController<UpdateController>();

            m_MetaSetup.Setup();
            m_BattleSetup.Setup();

            dispatcher.InitializeComplete();
        }
    }
}
