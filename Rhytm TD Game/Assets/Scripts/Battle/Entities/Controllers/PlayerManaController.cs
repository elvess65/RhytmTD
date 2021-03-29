using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models.DataTableModels;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class PlayerManaController : BaseController
    {
        private BattleModel m_BattleModel;
        private SkillsModel m_SkillsModel;
        private SpellBookModel m_SpellBookModel;
        private AccountBaseParamsDataModel m_AccountBaseParamsDataModel;

        private RhytmController m_RhytmController;

        private ManaModule m_ManaModule;


        public PlayerManaController(Dispatcher dispatcher) : base(dispatcher)
        {
        }


        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_SkillsModel = Dispatcher.GetModel<SkillsModel>();
            m_SpellBookModel = Dispatcher.GetModel<SpellBookModel>();
            m_AccountBaseParamsDataModel = Dispatcher.GetModel<AccountBaseParamsDataModel>();

            m_RhytmController = Dispatcher.GetController<RhytmController>();

            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandlder;
            m_BattleModel.OnBattleStarted += BattleStartedHandler;
            m_BattleModel.OnBattleFinished += BattleFinishedHandler;

            m_SpellBookModel.OnSpellbookOpened += SpellBookOpenedHandler;
            m_SpellBookModel.OnSpellbookClosed += SpellBookClosedAndPostUsedHandler;
            m_SpellBookModel.OnSpellbookPostUsed += SpellBookClosedAndPostUsedHandler;

            m_SkillsModel.OnSkillUsed += SkillUsedHandler;
        }

        public bool IsEnoughManaForSkill(int skillTypeID)
        {
            return m_ManaModule.CurrentMana >= m_AccountBaseParamsDataModel.GetSkillBaseDataByID(skillTypeID).ManaCost;
        }

        public void AddMana()
        {
            AddMana(m_AccountBaseParamsDataModel.BaseCharacterData.ManaInputRestore);
        }


        private void AddMana(int amount)
        {
            if (m_ManaModule.CurrentMana < m_ManaModule.TotalMana)
            {
                m_ManaModule.AddMana(amount);
            }
        }

        private void RemoveMana(int skillTypeID)
        {
            m_ManaModule.RemoveMana(m_AccountBaseParamsDataModel.GetSkillBaseDataByID(skillTypeID).ManaCost);
        }

        #region Handlers

        private void SkillUsedHandler(int skillID, int skillTypeID)
        {
            RemoveMana(skillTypeID);
        }

        private void PlayerInitializedHandlder(BattleEntity playerEntity)
        {
            m_BattleModel.OnPlayerEntityInitialized -= PlayerInitializedHandlder;

            m_ManaModule = playerEntity.GetModule<ManaModule>();
        }

        private void BattleStartedHandler()
        {
            m_RhytmController.OnTick += TickHandler;
        }

        private void BattleFinishedHandler(bool isSuccess)
        {
            m_RhytmController.OnTick -= TickHandler;
        }

        private void SpellBookOpenedHandler()
        {
            m_RhytmController.OnTick -= TickHandler;
        }

        private void SpellBookClosedAndPostUsedHandler()
        {
            m_RhytmController.OnTick += TickHandler;
        }

        private void TickHandler(int ticksSinceStart)
        {
            AddMana(m_AccountBaseParamsDataModel.BaseCharacterData.ManaAutoRestore);
        }

        #endregion
    }
}
