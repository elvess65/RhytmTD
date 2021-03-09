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
        private FindTargetController m_FindTargetController;
        
        private AnimationModule m_PlayerAnimationModule;
        private BattleEntity m_TargetEntity;
        private Vector3 m_TargetDirection;


        public BattleState_Normal() : base()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_InputModel = Dispatcher.GetModel<InputModel>();
            m_ShootController = Dispatcher.GetController<ShootController>();
            m_FindTargetController = Dispatcher.GetController<FindTargetController>();
            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();

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
#if TARGET_BASED_ATTACK
            HandleTargetBasedTouch(mouseScreenPos);
#elif DIRECTION_BASED_ATTACK
            HandleDirectionBasedTouch(mouseScreenPos);
#endif
        }

        #region Direction Based Attack

        private GameObject ob;

        private void HandleDirectionBasedTouch(Vector3 mouseScreenPos)
        {
            Camera camera = Camera.main;
            Vector3 playerWorldPos = Dispatcher.GetModel<BattleModel>().PlayerEntity.GetModule<TransformModule>().Position;
            Vector3 playerScreenPos = camera.WorldToScreenPoint(playerWorldPos);
            Vector3 dir2MouseScreen = (mouseScreenPos - playerScreenPos).normalized;
            dir2MouseScreen.z = 0;

            //Make possible to attack only forward
            if (dir2MouseScreen.y > 0)
            {
                m_TargetDirection = new Vector3(dir2MouseScreen.x, 0, dir2MouseScreen.y);

                m_PlayerAnimationModule.OnAnimationMoment += BaseDirectionAttackAnimationMomentHandler;
                m_PlayerAnimationModule.PlayAnimation(EnumsCollection.AnimationTypes.Attack);

                //Debug 
                if (ob == null)
                    ob = GameObject.CreatePrimitive(PrimitiveType.Capsule);

                ob.transform.position = playerWorldPos + m_TargetDirection * 10;

                //if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid())
                //m_RhytmInputProxy.RegisterInput();
            }
        }

        private void BaseDirectionAttackAnimationMomentHandler()
        {
            m_PlayerAnimationModule.OnAnimationMoment -= BaseDirectionAttackAnimationMomentHandler;

            m_RhytmInputProxy.IsInputTickValid();
            m_ShootController.Shoot(m_BattleModel.PlayerEntity, m_TargetDirection);
        }

        #endregion

        #region Target Based Attack

        private void HandleTargetBasedTouch(Vector3 mouseScreenPos)
        {
            if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid())
            {
                m_TargetEntity = GetTargetForBaseAttack();

                if (m_TargetEntity != null)
                {
                    m_PlayerAnimationModule.OnAnimationMoment += BaseTargetAttackAnimationMomentHandler;
                    m_PlayerAnimationModule.PlayAnimation(EnumsCollection.AnimationTypes.Attack);
                }
            }

            m_RhytmInputProxy.RegisterInput();
        }

        private void BaseTargetAttackAnimationMomentHandler()
        {
            m_PlayerAnimationModule.OnAnimationMoment -= BaseTargetAttackAnimationMomentHandler;

            m_RhytmInputProxy.IsInputTickValid();
            m_ShootController.Shoot(m_BattleModel.PlayerEntity, m_TargetEntity);
        }

        private BattleEntity GetTargetForBaseAttack()
        {
            TargetModule targetModule = m_BattleModel.PlayerEntity.GetModule<TargetModule>();
            BattleEntity targetEntity;

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

            return targetEntity;
        }

        #endregion

        private void HandleKeyDown(KeyCode keyCode)
        {
        }

        private void PlayerInitializedHandlder(BattleEntity playerEntity)
        {
            m_BattleModel.OnPlayerEntityInitialized -= PlayerInitializedHandlder;

            m_PlayerAnimationModule = playerEntity.GetModule<AnimationModule>();
        }
    }
}
