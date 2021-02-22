using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.Entities.Skills;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class SkillsController : BaseController
    {
        private SkillsModel m_SkillsModel;
        private ISkillEntityFactory m_SkillFactory;

        public SkillsController(Dispatcher dispatcher) : base(dispatcher)
        {
            m_SkillFactory = new DefaultSkilEntityFactory();
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

        public BattleEntity CreateMeteoriteSkillEntity()
        {
            BattleEntity battleEntity = m_SkillFactory.CreateMeteoriteEntity();

            SkillMeteorite skillMeteorite = new SkillMeteorite();
            skillMeteorite.Initialize(battleEntity);

            m_SkillsModel.AddSkill(skillMeteorite);
            m_SkillsModel.OnSkillCreated?.Invoke(battleEntity);

            return battleEntity;
        }

        public BattleEntity CreateFireballSkillEntity()
        {
            BattleEntity battleEntity = m_SkillFactory.CreateFireballEntity();

            SkillFireball skillFireball = new SkillFireball();
            skillFireball.Initialize(battleEntity);

            m_SkillsModel.AddSkill(skillFireball);
            m_SkillsModel.OnSkillCreated?.Invoke(battleEntity);

            return battleEntity;
        }
    }
}
