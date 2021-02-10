using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.StateMachine
{
    public class BattleState_Normal : BattleState_Abstract
    {
        private BattlefieldController m_BattlefieldController;
        private DamageController m_DamageController;
        private BattleEntity m_PlayerEntity;

        public BattleState_Normal() : base()
        {
            m_BattlefieldController = Dispatcher.GetController<BattlefieldController>();
            m_DamageController = Dispatcher.GetController<DamageController>();
        }

        public override void HandleTouch(Vector3 mouseScreenPos)
        {
            if (m_PlayerEntity == null)
                m_PlayerEntity = m_BattlefieldController.Dispatcher.GetModel<BattleModel>().PlayerEntity;

            BattleEntity targetEntity = m_BattlefieldController.FindClosestTo(m_PlayerEntity);

            if (targetEntity != null)
            {
                m_DamageController.DealDamage(m_PlayerEntity.ID, targetEntity.ID); //TODO: Should be moved on collision
            }
            
            //if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid())
            //    Debug.Log("Input is valid");

            base.HandleTouch(mouseScreenPos);
        }       
    }
}
