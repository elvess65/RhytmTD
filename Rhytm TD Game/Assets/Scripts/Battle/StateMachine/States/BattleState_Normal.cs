using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.StateMachine
{
    public class BattleState_Normal : BattleState_Abstract
    {
        private BattleModel m_BattleModel;
        private ShootController m_ShootController;
        private FindTargetController m_FindTargetController;

        public BattleState_Normal() : base()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_ShootController = Dispatcher.GetController<ShootController>();
            m_FindTargetController = Dispatcher.GetController<FindTargetController>();
        }

        public override void HandleTouch(Vector3 mouseScreenPos)
        {
            BattleEntity targetEntity = m_FindTargetController.GetNearestTarget(m_BattleModel.PlayerEntity);

            if (targetEntity != null)
            {
                m_ShootController.Shoot(m_BattleModel.PlayerEntity, targetEntity);
            }
            
            //if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid())
            //    Debug.Log("Input is valid");

            base.HandleTouch(mouseScreenPos);
        }       
    }
}
