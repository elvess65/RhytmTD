
using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Skills
{
    public abstract class BaseSkill : ISkill
    {
        public int ID => BattleEntity.ID;
        public BattleEntity BattleEntity { get; private set; }

        protected SkillModule m_SkillModule { get; private set; }
        protected Dispatcher Dispatcher => Dispatcher.Instance;

        public virtual void Initialize(BattleEntity battleEntity)
        {
            BattleEntity = battleEntity;

            m_SkillModule = battleEntity.GetModule<SkillModule>();
        }

        public async virtual void UseSkill(int senderID)
        {
            SkillPrepareStarted(senderID);
            await new WaitForSeconds(m_SkillModule.ActivationTime);

            SkilUseStarted(senderID);
            await new WaitForSeconds(m_SkillModule.UseTime);

            FinishingSkillUseStarted(senderID);
            await new WaitForSeconds(m_SkillModule.FinishingTime);

            SkillUseFinished(senderID);
        }

        protected virtual void SkillPrepareStarted(int senderID)
        {
            m_SkillModule.SkillPrepareStarted(senderID);
        }

        protected virtual void SkilUseStarted(int senderID)
        {
            m_SkillModule.SkillUseStarted(senderID);
        }

        protected virtual void FinishingSkillUseStarted(int senderID)
        {
            m_SkillModule.FinishingSkillUseStarted(senderID);
        }

        protected virtual void SkillUseFinished(int senderID)
        {
            m_SkillModule.SkillUseFinished(senderID);
        }
    }
}
