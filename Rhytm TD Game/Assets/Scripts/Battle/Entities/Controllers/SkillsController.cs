using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.Entities.Skills;
using RhytmTD.Data.Models.DataTableModels;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class SkillsController : BaseController
    {
        private SkillsModel m_SkillsModel;
        private AccountBaseParamsDataModel m_AccountBaseParamsDataModel;

        private RhytmController m_RhytmController;

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

            m_RhytmController = Dispatcher.GetController<RhytmController>();
        }


        public void UseSkill(int skillID, int senderID, int targetID)
        {
            BaseSkill skill = m_SkillsModel.GetSkill(skillID);
            skill.UseSkill(senderID, targetID);

            m_SkillsModel.UpdateSkillUsageRecord(skillID, m_RhytmController.CurrentTick);
        }

        public bool IsSkillInCooldown(int skillID, out float remainsCooldownSeconds)
        {
            remainsCooldownSeconds = 0;
            (int skillID, int usageTick) result = m_SkillsModel.GetSkillUsageRecord(skillID);
            UnityEngine.Debug.Log("GET RECORD FOR " + skillID + " " + result.usageTick);
            if (skillID >= 0)
            {
                int cooldownFinishTick = result.usageTick + m_SkillsModel.GetSkill(skillID).BattleEntity.GetModule<SkillModule>().CooldownTicks;
                int ticksInCooldown = cooldownFinishTick - m_RhytmController.CurrentTick;
                if (ticksInCooldown > 0)
                {
                    remainsCooldownSeconds = (float)(ticksInCooldown * m_RhytmController.TickDurationSeconds);
                    return true;
                }
            }

            return false;
        }


        public BattleEntity CreateMeteoriteSkillEntity()
        {
            AccountBaseParamsDataModel.MeteoriteSkillBaseData data = m_AccountBaseParamsDataModel.BaseMeteoriteData;
            BattleEntity battleEntity = m_SkillFactory.CreateMeteoriteEntity((int)data.TypeID, data.ActivationTime,
                                                                             data.UseTime, data.FinishingTime,
                                                                             data.CooldownTicks,data.DamageRadius, data.Damage,
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
            BattleEntity battleEntity = m_SkillFactory.CreateFireballEntity((int)data.TypeID, data.ActivationTime,
                                                                            data.UseTime, data.FinishingTime,
                                                                            data.CooldownTicks,
                                                                            data.MoveSpeed, data.Damage);

            SkillFireball skillFireball = new SkillFireball();
            skillFireball.Initialize(battleEntity);

            m_SkillsModel.AddSkill(skillFireball);
            m_SkillsModel.OnSkillCreated?.Invoke(battleEntity);

            return battleEntity;
        }

        public BattleEntity CreateHealthSkillEntity()
        {
            AccountBaseParamsDataModel.HealthSkillBaseData data = m_AccountBaseParamsDataModel.BaseHealthData;
            BattleEntity battleEntity = m_SkillFactory.CreateHealthEntity(ConstsCollection.SkillConsts.HEALTH_ID, data.ActivationTime,
                                                                            data.UseTime, data.FinishingTime,
                                                                            data.CooldownTicks,
                                                                            data.HealthPercent);

            SkillHealth skillHealth = new SkillHealth();
            skillHealth.Initialize(battleEntity);

            m_SkillsModel.AddSkill(skillHealth);
            m_SkillsModel.OnSkillCreated?.Invoke(battleEntity);

            return battleEntity;
        }
    }
}
