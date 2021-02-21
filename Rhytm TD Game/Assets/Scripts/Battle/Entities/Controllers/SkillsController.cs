using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.Entities.Skills;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class SkillsController : BaseController
    {
        private SkillsModel m_SkillsModel;
        private ISkillFactory m_SkillFactory;

        public SkillsController(Dispatcher dispatcher) : base(dispatcher)
        {
            m_SkillFactory = new DefaultSkillFactory();
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_SkillsModel = Dispatcher.GetModel<SkillsModel>();
        }

        public void UseSkill(int skillID, int senderID, int targetID)
        {
            BaseSkill skill = m_SkillsModel.GetSkill(skillID);
            skill.UseSkill(senderID, targetID);
        }

        public BattleEntity CreateMeteoriteSkill()
        {
            BattleEntity battleEntity = m_SkillFactory.CreateMeteorite();

            SkillMeteorite skillMeteorite = new SkillMeteorite();
            skillMeteorite.Initialize(battleEntity);

            m_SkillsModel.AddSkill(skillMeteorite);
            m_SkillsModel.SkillCreated(battleEntity);

            return battleEntity;
        }

        public BattleEntity CreateFireballSkill()
        {
            BattleEntity battleEntity = m_SkillFactory.CreateFireball();

            SkillFireball skillFireball = new SkillFireball();
            skillFireball.Initialize(battleEntity);

            m_SkillsModel.AddSkill(skillFireball);
            m_SkillsModel.SkillCreated(battleEntity);

            return battleEntity;
        }
    }
}
