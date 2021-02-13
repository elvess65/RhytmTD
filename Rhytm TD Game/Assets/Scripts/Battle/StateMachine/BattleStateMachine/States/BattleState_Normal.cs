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
        private BattleModel m_BattleModel;
        private ShootController m_ShootController;
        private FindTargetController m_FindTargetController;
        private InputController m_InputController;
        private RhytmInputProxy m_RhytmInputProxy;

        public BattleState_Normal() : base()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_ShootController = Dispatcher.GetController<ShootController>();
            m_FindTargetController = Dispatcher.GetController<FindTargetController>();
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
            TargetModule targetModule = m_BattleModel.PlayerEntity.GetModule<TargetModule>();
            BattleEntity targetEntity;

            m_RhytmInputProxy.IsInputTickValid();

            if (!targetModule.HasTarget)
            {
                targetEntity = m_FindTargetController.GetNearestTarget(m_BattleModel.PlayerEntity);

                if (targetEntity != null)
                {
                    targetModule.SetTarget(targetEntity);
                }
            }
            else
            {
                targetEntity = targetModule.Target;
            }

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
