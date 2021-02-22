using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.Entities.Skills;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Views
{
    public class PlayerSkillView : BattleEntityView
    {
        private BattleModel m_BattleModel;
        private SkillsModel m_SkillsModel;
        private LoadoutModule m_LoadoutModule;
        private AnimationModule m_AnimationModule;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_AnimationModule = entity.GetModule<AnimationModule>();
            m_LoadoutModule = entity.GetModule<LoadoutModule>();
            m_SkillsModel = Dispatcher.GetModel<SkillsModel>();

            foreach (int skillID in m_LoadoutModule.SelectedSkillIDs)
            {
                BaseSkill skill = m_SkillsModel.GetSkill(skillID);
                SkillModule skillModule = skill.BattleEntity.GetModule<SkillModule>();

                skillModule.OnSkillUseStarted += SkillModule_OnSkillUseStarted;     
            }
        }

        private void SkillModule_OnSkillUseStarted(int senderID, int targetID)
        {
            m_AnimationModule.PlayAnimation(AnimationTypes.UseSkill);
        }

        private void OnDestroy()
        {
            foreach (int skillID in m_LoadoutModule.SelectedSkillIDs)
            {
                BaseSkill skill = m_SkillsModel.GetSkill(skillID);
                SkillModule skillModule = skill.BattleEntity.GetModule<SkillModule>();

                skillModule.OnSkillUseStarted -= SkillModule_OnSkillUseStarted;
            }
        }
    }
}
