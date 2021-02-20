
using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Skills
{
    public abstract class BaseSkill : ISkill
    {
        public int ID => m_BattleEntity.ID;

        protected SkillsModel m_SkillsModel { get; private set; }
        protected BattleEntity m_BattleEntity { get; private set; }
        protected SkillModule m_SkillModule { get; private set; }

        public Dispatcher Dispatcher => Dispatcher.Instance;

        public virtual void Initialize(BattleEntity battleEntity)
        {
            m_BattleEntity = battleEntity;

            m_SkillModule = battleEntity.GetModule<SkillModule>();
            m_SkillsModel = Dispatcher.GetModel<SkillsModel>();
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
            m_SkillsModel.SkillPrepareStarted(ID, senderID, targetID, m_SkillModule.ActivationTime);
        }

        protected virtual void SkilUseStarted(int senderID, int targetID)
        {
            m_SkillsModel.SkillUseStarted(ID, senderID, targetID, m_SkillModule.UseTime);
        }

        protected virtual void FinishingSkillUseStarted(int senderID, int targetID)
        {
            m_SkillsModel.FinishingSkillUseStarted(ID, senderID, targetID, m_SkillModule.FinishingTime);
        }

        protected virtual void SkillUseFinished(int senderID, int targetID)
        {
            m_SkillsModel.SkillUseFinished(ID, senderID, targetID, m_SkillModule.CooldownTime);
        }
    }
}
