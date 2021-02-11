using CoreFramework.Input;
using CoreFramework.Rhytm;
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
        private InputController m_InputController;
        private RhytmInputProxy m_RhytmInputProxy;
        private BattleEntity m_PlayerEntity;

        public BattleState_Normal() : base()
        {
            m_BattlefieldController = Dispatcher.GetController<BattlefieldController>();
            m_DamageController = Dispatcher.GetController<DamageController>();
            m_InputController = Dispatcher.GetController<InputController>();
            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();
        }

        public override void EnterState()
        {
            base.EnterState();

            m_InputController.OnTouch += HandleTouch;
        }

        public override void ExitState()
        {
            base.ExitState();

            m_InputController.OnTouch -= HandleTouch;
        }

        private void HandleTouch(Vector3 mouseScreenPos)
        {
            BattleEntity targetEntity = m_FindTargetController.GetNearestTarget(m_BattleModel.PlayerEntity);

            if (targetEntity != null)
            {
                m_ShootController.Shoot(m_BattleModel.PlayerEntity, targetEntity);
            }

            //if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid())
            //    Debug.Log("Input is valid");

            m_RhytmInputProxy.RegisterInput();
        }       
    }
}
