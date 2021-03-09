using System.Collections.Generic;
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
        private TransformModule m_PlayerTransformModule;
        private BattleEntity m_TargetEntity;
        private Vector3 m_ShootDirection;
        private List<BattleEntity> m_PotentialTargets;

        public BattleState_Normal() : base()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_InputModel = Dispatcher.GetModel<InputModel>();
            m_ShootController = Dispatcher.GetController<ShootController>();
            m_FindTargetController = Dispatcher.GetController<FindTargetController>();
            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();

            m_PotentialTargets = new List<BattleEntity>();

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

        //Debug
        private GameObject ob;

        private void HandleDirectionBasedTouch(Vector3 mouseScreenPos)
        {
            if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid())
            {
                //Dispose cache from prev shoot
                m_TargetEntity = null;
                if (m_PotentialTargets.Count > 0)
                    m_PotentialTargets.Clear();

                //Temp
                Camera camera = Camera.main;

                Vector3 fromWorldPos = Dispatcher.GetModel<BattleModel>().PlayerEntity.GetModule<SlotModule>().ProjectileSlot.position;
                fromWorldPos.y = 0;

                Vector3 fromScreenPos = camera.WorldToScreenPoint(fromWorldPos);
                Vector3 shootDirScreen = (mouseScreenPos - fromScreenPos).normalized;
                shootDirScreen.z = 0;

                //Make possible to attack only forward
                if (shootDirScreen.y <= 0)
                    return;

                m_ShootDirection = new Vector3(shootDirScreen.x, 0, shootDirScreen.y);
                m_TargetEntity = GetTargetForDirectionBaseAttack(fromWorldPos);

                m_PlayerAnimationModule.OnAnimationMoment += BaseAttackAnimationMomentHandler;
                m_PlayerAnimationModule.PlayAnimation(EnumsCollection.AnimationTypes.Attack);

                //Debug 
                if (ob == null)
                    ob = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                ob.transform.position = fromWorldPos + m_ShootDirection * 10;
            }
            
            m_RhytmInputProxy.RegisterInput();
        }

        private BattleEntity GetTargetForDirectionBaseAttack(Vector3 fromWorldPos)
        {
            BattleEntity target = null;

            //Get pottential targets
            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (!entity.HasModule<EnemyBehaviourTag>())
                    continue;

                //Get from module
                float enemyColliderSize = 1.5f;
                float distance2ShootLine = GetDistanceToLine(m_ShootDirection, fromWorldPos, entity.GetModule<TransformModule>().Position);

                if (distance2ShootLine <= enemyColliderSize)
                    m_PotentialTargets.Add(entity);
            }

            if (m_PotentialTargets.Count > 0)
            {
                if (m_PotentialTargets.Count > 1)
                {
                    //Filter target by distance if mo than 1
                    float closestSqrDist2Enemy = float.MaxValue;
                    foreach (BattleEntity potentialTarget in m_PotentialTargets)
                    {
                        TransformModule potentialTargetTransformModule = potentialTarget.GetModule<TransformModule>();

                        float sqrDist = (potentialTargetTransformModule.Position - m_PlayerTransformModule.Position).sqrMagnitude;
                        if (sqrDist < closestSqrDist2Enemy)
                        {
                            closestSqrDist2Enemy = sqrDist;
                            target = potentialTarget;
                        }
                    }
                }
                else
                    target = m_PotentialTargets[0];
            }

            return target;
        }

        private float GetDistanceToLine(Vector3 dir, Vector3 origin, Vector3 point)
        {
            return Vector3.Cross(dir, point - origin).magnitude;
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
                    m_PlayerAnimationModule.OnAnimationMoment += BaseAttackAnimationMomentHandler;
                    m_PlayerAnimationModule.PlayAnimation(EnumsCollection.AnimationTypes.Attack);
                }
            }

            m_RhytmInputProxy.RegisterInput();
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

        private void BaseAttackAnimationMomentHandler()
        {
            m_PlayerAnimationModule.OnAnimationMoment -= BaseAttackAnimationMomentHandler;

            m_RhytmInputProxy.IsInputTickValid();

            if (m_TargetEntity != null)
                m_ShootController.Shoot(m_BattleModel.PlayerEntity, m_TargetEntity);
            else
                m_ShootController.Shoot(m_BattleModel.PlayerEntity, m_ShootDirection);

        }

        private void HandleKeyDown(KeyCode keyCode)
        {
        }

        private void PlayerInitializedHandlder(BattleEntity playerEntity)
        {
            m_BattleModel.OnPlayerEntityInitialized -= PlayerInitializedHandlder;

            m_PlayerAnimationModule = playerEntity.GetModule<AnimationModule>();
            m_PlayerTransformModule = playerEntity.GetModule<TransformModule>();
        }
    }
}
