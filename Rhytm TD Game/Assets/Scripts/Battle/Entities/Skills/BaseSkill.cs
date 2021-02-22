
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

        public async virtual void UseSkill(int senderID, int targetID)
        {
            SkillPrepareStarted(senderID, targetID);
            await new WaitForSeconds(m_SkillModule.ActivationTime);

            SkilUseStarted(senderID, targetID);
            await new WaitForSeconds(m_SkillModule.UseTime);

            FinishingSkillUseStarted(senderID, targetID);
            await new WaitForSeconds(m_SkillModule.FinishingTime);

            SkillUseFinished(senderID, targetID);
        }

        protected virtual void SkillPrepareStarted(int senderID, int targetID)
        {
            m_SkillModule.SkillPrepareStarted(senderID, targetID);
        }

        protected virtual void SkilUseStarted(int senderID, int targetID)
        {
            m_SkillModule.SkillUseStarted(senderID, targetID);
        }

        protected virtual void FinishingSkillUseStarted(int senderID, int targetID)
        {
            m_SkillModule.FinishingSkillUseStarted(senderID, targetID);
        }

        protected virtual void SkillUseFinished(int senderID, int targetID)
        {
            m_SkillModule.SkillUseFinished(senderID, targetID);
        }
    }
}
