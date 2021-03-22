using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class SkillsCooldownController : BaseController
    {
        private UpdateModel m_UpdateModel;
        private BattleModel m_BattleModel;
        private SkillsModel m_SkillsModel;
        private SpellBookModel m_SpellBookModel;
        private SkillsCooldownModel m_SkillsCooldownModel;
        private RhytmController m_RhytmController;

        private bool m_IsActive = false;


        public SkillsCooldownController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_UpdateModel = Dispatcher.GetModel<UpdateModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_SkillsModel = Dispatcher.GetModel<SkillsModel>();
            m_SpellBookModel = Dispatcher.GetModel<SpellBookModel>();

            m_SkillsCooldownModel = Dispatcher.GetModel<SkillsCooldownModel>();

            m_RhytmController = Dispatcher.GetController<RhytmController>();

            m_BattleModel.OnBattleStarted += BattleStartedHandler;
            m_BattleModel.OnBattleFinished += OnBattleFinishedHandler;

            m_SpellBookModel.OnSpellbookOpened += SpellBookOpenedHandler;
            m_SpellBookModel.OnSpellbookPostUsed += SpellBookClosedAndPostUsedHandler;
            m_SpellBookModel.OnSpellbookClosed += SpellBookClosedAndPostUsedHandler;

            m_SkillsModel.OnSkillUsed += SkillUsedHandler;
        }

        public (float remainTime, float totalTime) GetSkillCooldownTime(int skillID)
        {
            return m_SkillsCooldownModel.GetSkillCooldownTime(skillID);
        }


        private void UpdateHandler(float deltaTime)
        {
            if (m_IsActive)
                m_SkillsCooldownModel.UpdateSkillsCooldownTime(deltaTime);
        }

        private void BattleStartedHandler()
        {
            m_UpdateModel.OnUpdate += UpdateHandler;
        }

        private void OnBattleFinishedHandler(bool isSuccess)
        {
            m_UpdateModel.OnUpdate -= UpdateHandler;
        }

        private void SpellBookOpenedHandler()
        {
            m_IsActive = false;
        }

        private void SpellBookClosedAndPostUsedHandler()
        {
            m_IsActive = true;
        }

        private void SkillUsedHandler(int skillID)
        {
            int cooldownTicks = m_SkillsModel.GetSkill(skillID).BattleEntity.GetModule<SkillModule>().CooldownTicks;
            float cooldownSeconds = (float)(cooldownTicks * m_RhytmController.TickDurationSeconds);

            m_SkillsCooldownModel.AddSkillToCooldown(skillID, cooldownSeconds);
        }
    }
}
