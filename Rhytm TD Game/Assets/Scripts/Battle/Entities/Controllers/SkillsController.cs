using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.Entities.Skills;
using RhytmTD.Data.Models.DataTableModels;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class SkillsController : BaseController
    {
        private SkillsModel m_SkillsModel;
        private AccountBaseParamsDataModel m_AccountBaseParamsDataModel;

        private ISkillEntityFactory m_SkillFactory;

        public SkillsController(Dispatcher dispatcher) : base(dispatcher)
        {
            m_SkillFactory = new DefaultSkilEntityFactory();
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_SkillsModel = Dispatcher.GetModel<SkillsModel>();
            m_AccountBaseParamsDataModel = Dispatcher.GetModel<AccountBaseParamsDataModel>();
        }

        public void UseSkill(int skillID, int senderID, int targetID)
        {
            BaseSkill skill = m_SkillsModel.GetSkill(skillID);
            skill.UseSkill(senderID, targetID);
        }

        public BattleEntity CreateMeteoriteSkillEntity()
        {
            AccountBaseParamsDataModel.MeteoriteSkillBaseData data = m_AccountBaseParamsDataModel.BaseMeteoriteData;
            BattleEntity battleEntity = m_SkillFactory.CreateMeteoriteEntity(data.TypeID, data.ActivationTime,
                                                                             data.UseTime, data.FinishingTime,
                                                                             data.CooldownTime,
                                                                             data.FlyTime, data.DamageRadius, data.Damage,
                                                                             data.EffectOffset);

            SkillMeteorite skillMeteorite = new SkillMeteorite();
            skillMeteorite.Initialize(battleEntity);

            m_SkillsModel.AddSkill(skillMeteorite);
            m_SkillsModel.OnSkillCreated?.Invoke(battleEntity);

            return battleEntity;
        }

        public BattleEntity CreateFireballSkillEntity()
        {
            AccountBaseParamsDataModel.FireballSkillBaseData data = m_AccountBaseParamsDataModel.BaseFireballData;
            BattleEntity battleEntity = m_SkillFactory.CreateFireballEntity(data.TypeID, data.ActivationTime,
                                                                            data.UseTime, data.FinishingTime,
                                                                            data.CooldownTime,
                                                                            data.MoveSpeed, data.Damage);

            SkillFireball skillFireball = new SkillFireball();
            skillFireball.Initialize(battleEntity);

            m_SkillsModel.AddSkill(skillFireball);
            m_SkillsModel.OnSkillCreated?.Invoke(battleEntity);

            return battleEntity;
        }
    }
}
