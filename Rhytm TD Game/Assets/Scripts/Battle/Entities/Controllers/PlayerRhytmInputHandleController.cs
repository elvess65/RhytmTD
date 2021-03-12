using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Factory;
using RhytmTD.Data.Models;
using RhytmTD.Data.Models.DataTableModels;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class PlayerRhytmInputHandleController : BaseController
    {
        private BattleModel m_BattleModel;
        private WorldDataModel m_WorldDataModel;
        private AccountDataModel m_AccountDataModel;

        private DamageController m_DamageController;
        
        private LevelDataFactory m_CurrentLevel => m_WorldDataModel.Areas[m_AreaIndex].LevelsData[m_LevelIndex];
        private int m_AreaIndex;
        private int m_LevelIndex;

        private int m_CorrectInputsCounter = 0;


        public PlayerRhytmInputHandleController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_WorldDataModel = Dispatcher.GetModel<WorldDataModel>();
            m_AccountDataModel = Dispatcher.GetModel<AccountDataModel>();

            m_DamageController = Dispatcher.GetController<DamageController>();

            m_AreaIndex = m_AccountDataModel.CompletedAreas;
            m_LevelIndex = m_AccountDataModel.CompletedLevels;
        }

        public void HandleCorrectRhytmInput()
        {
            m_CorrectInputsCounter++;
        }

        public void HandleWrongRhytmInput()
        {
            m_CorrectInputsCounter = 0;
            m_DamageController.DealDamage(m_BattleModel.PlayerEntity.ID, m_BattleModel.PlayerEntity.ID, m_CurrentLevel.DamageForMissRhytm);
        }
    }
}
