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
        private PlayerRhytmInputHandleModel m_PlayerRhytmInputHandleModel;

        private DamageController m_DamageController;
        
        private LevelDataFactory m_CurrentLevel => m_WorldDataModel.Areas[m_AreaIndex].LevelsData[m_LevelIndex];
        private int m_AreaIndex;
        private int m_LevelIndex;


        public PlayerRhytmInputHandleController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_WorldDataModel = Dispatcher.GetModel<WorldDataModel>();
            m_AccountDataModel = Dispatcher.GetModel<AccountDataModel>();
            m_PlayerRhytmInputHandleModel = Dispatcher.GetModel<PlayerRhytmInputHandleModel>();

            m_DamageController = Dispatcher.GetController<DamageController>();

            m_AreaIndex = m_AccountDataModel.CompletedAreas;
            m_LevelIndex = m_AccountDataModel.CompletedLevels;
        }

        public void HandleCorrectRhytmInput()
        {
            m_PlayerRhytmInputHandleModel.CorrectInputsCounter += ConstsCollection.DDRP_INPUT_INCREASE;
        }

        public void HandleWrongRhytmInput()
        {
            m_PlayerRhytmInputHandleModel.CorrectInputsCounter = UnityEngine.Mathf.Clamp(m_PlayerRhytmInputHandleModel.CorrectInputsCounter - ConstsCollection.DDRP_INPUT_DECREASE,
                                                                                         0, m_PlayerRhytmInputHandleModel.CorrectInputsCounter);

            m_DamageController.DealDamage(m_BattleModel.PlayerEntity.ID, m_BattleModel.PlayerEntity.ID, m_CurrentLevel.DamageForMissRhytm);
        }
    }
}
