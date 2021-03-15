using CoreFramework;
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
        private InputModel m_InputModel;
        private BattleModel m_BattleModel;

        private RhytmInputProxy m_RhytmInputProxy;
        private ShootController m_ShootController;
        private TargetingController m_TargetingController;
        private PlayerRhytmInputHandleController m_PlayerRhytmInputHandleController;

        private AnimationModule m_PlayerAnimationModule;
        private TransformModule m_PlayerTransformModule;
        private SlotModule m_PlayerSlotModule;
        private BattleEntity m_TargetEntity;
        private Vector3 m_ShootDirection;


        public BattleState_Normal() : base()
        {
            m_InputModel = Dispatcher.GetModel<InputModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_ShootController = Dispatcher.GetController<ShootController>();
            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();
            m_TargetingController = Dispatcher.GetController<TargetingController>();
            m_PlayerRhytmInputHandleController = Dispatcher.GetController<PlayerRhytmInputHandleController>();

            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandlder;
        }

        public override void EnterState()
        {
            base.EnterState();

            m_InputModel.OnTouch += HandleTouch;
            m_InputModel.OnKeyDown += HandleKeyDown;
        }

        public override void ExitState()
        {
            base.ExitState();

            m_InputModel.OnTouch -= HandleTouch;
            m_InputModel.OnKeyDown -= HandleKeyDown;
        }


        private void HandleTouch(Vector3 mouseScreenPos)
        {   
#if DIRECTION_BASED_ATTACK
            HandleDirectionBasedTouch(mouseScreenPos);
#else 
            HandleTargetBasedTouch(mouseScreenPos);
#endif
        }

        private void HandleDirectionBasedTouch(Vector3 mouseScreenPos)
        {
            if (m_RhytmInputProxy.IsInputAllowed()) 
            {
                if (m_RhytmInputProxy.IsInputTickValid())
                {
                    m_ShootDirection = m_TargetingController.GetDirection(mouseScreenPos, m_PlayerSlotModule.ProjectileSlot.position, out Vector3 hitPoint);

                    //Make possible to attack only forward
                    if (m_ShootDirection.z <= 0)
                    {
                        m_RhytmInputProxy.RegisterInput();
                        return;
                    }

                    m_TargetEntity = m_TargetingController.GetTargetForDirectionBaseAttack(m_PlayerSlotModule.ProjectileSlot.position, m_PlayerTransformModule.Position, m_ShootDirection);

                    m_PlayerAnimationModule.OnAnimationMoment += BaseAttackAnimationMomentHandler;
                    m_PlayerAnimationModule.PlayAnimation(EnumsCollection.AnimationTypes.Attack);

                    m_PlayerRhytmInputHandleController.HandleCorrectRhytmInput();
                }
                else
                {
                    m_PlayerRhytmInputHandleController.HandleWrongRhytmInput();
                }
            }
            
            m_RhytmInputProxy.RegisterInput();
        }

        private void HandleTargetBasedTouch(Vector3 mouseScreenPos)
        {
            if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid())
            {
                m_TargetEntity = m_TargetingController.GetTargetForTargetBaseAttack(); 

                if (m_TargetEntity != null)
                {


                    m_PlayerAnimationModule.OnAnimationMoment += BaseAttackAnimationMomentHandler;
                    m_PlayerAnimationModule.PlayAnimation(EnumsCollection.AnimationTypes.Attack);
                }
            }

            m_RhytmInputProxy.RegisterInput();
        }


        private void BaseAttackAnimationMomentHandler()
        {
            m_PlayerAnimationModule.OnAnimationMoment -= BaseAttackAnimationMomentHandler;

            m_RhytmInputProxy.IsInputTickValid();

            if (m_TargetEntity != null)
                m_ShootController.Shoot(m_BattleModel.PlayerEntity, m_TargetEntity);
            else
                m_ShootController.Shoot(m_BattleModel.PlayerEntity, m_ShootDirection);

        }

        private void PlayerInitializedHandlder(BattleEntity playerEntity)
        {
            m_BattleModel.OnPlayerEntityInitialized -= PlayerInitializedHandlder;

            m_PlayerAnimationModule = playerEntity.GetModule<AnimationModule>();
            m_PlayerTransformModule = playerEntity.GetModule<TransformModule>();
            m_PlayerSlotModule = playerEntity.GetModule<SlotModule>();
        }

        private void HandleKeyDown(KeyCode keyCode)
        {
        }
    }
}
