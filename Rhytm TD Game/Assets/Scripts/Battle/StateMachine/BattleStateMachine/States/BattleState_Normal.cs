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
        private BattleModel m_BattleModel;
        private InputModel m_InputModel;
        private ShootController m_ShootController;
        private FindTargetController m_FindTargetController;
        private SkillsController m_SkillsController;
        private RhytmInputProxy m_RhytmInputProxy;
        private AnimationModule m_PlayerAnimationModule;
        private BattleEntity m_TargetEntity;
        private int m_SkillID;


        public BattleState_Normal() : base()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_InputModel = Dispatcher.GetModel<InputModel>();
            m_ShootController = Dispatcher.GetController<ShootController>();
            m_FindTargetController = Dispatcher.GetController<FindTargetController>();
            m_SkillsController = Dispatcher.GetController<SkillsController>();
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
            if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid())
            {
                m_TargetEntity = GetTargetForBaseAttack();

                if (m_TargetEntity != null)
                {
                    m_PlayerAnimationModule.OnAnimationMoment += BaseAttackAnimationMomentHandler;
                    m_PlayerAnimationModule.PlayAnimation(CoreFramework.EnumsCollection.AnimationTypes.Attack);
                }
            }

            m_RhytmInputProxy.RegisterInput();
        }

        private void BaseAttackAnimationMomentHandler()
        {
            m_PlayerAnimationModule.OnAnimationMoment -= BaseAttackAnimationMomentHandler;

            m_RhytmInputProxy.IsInputTickValid();
            m_ShootController.Shoot(m_BattleModel.PlayerEntity, m_TargetEntity);
        }

        private void HandleKeyDown(KeyCode keyCode)
        {
            if (keyCode == KeyCode.Space)
            {
                m_TargetEntity = m_FindTargetController.GetNearestTarget(m_BattleModel.PlayerEntity);
                if (m_TargetEntity != null)
                {
                    LoadoutModule loadoutModule = m_BattleModel.PlayerEntity.GetModule<LoadoutModule>();
                    m_SkillID = loadoutModule.GetSkillIDByTypeID(ConstsCollection.SkillConsts.METEORITE_ID);

                    m_SkillsController.UseSkill(m_SkillID, m_BattleModel.PlayerEntity.ID, m_TargetEntity.ID);
                    m_PlayerAnimationModule.PlayAnimation(EnumsCollection.AnimationTypes.UseCastableSkill);
                }
            }
            else if (keyCode == KeyCode.V)
            {
                m_TargetEntity = m_FindTargetController.GetNearestTarget(m_BattleModel.PlayerEntity);
                if (m_TargetEntity != null)
                {
                    LoadoutModule loadoutModule = m_BattleModel.PlayerEntity.GetModule<LoadoutModule>();
                    m_SkillID = loadoutModule.GetSkillIDByTypeID(ConstsCollection.SkillConsts.FIREBALL_ID);

                    m_PlayerAnimationModule.OnAnimationMoment += SkillAnimationMomentHandler;
                    m_PlayerAnimationModule.PlayAnimation(EnumsCollection.AnimationTypes.UseWeaponSkill);
                }
            }

            else if (keyCode == KeyCode.Z)
            {
                LoadoutModule loadoutModule = m_BattleModel.PlayerEntity.GetModule<LoadoutModule>();
                m_SkillID = loadoutModule.GetSkillIDByTypeID(ConstsCollection.SkillConsts.HEALTH_ID);

                m_SkillsController.UseSkill(m_SkillID, m_BattleModel.PlayerEntity.ID, m_BattleModel.PlayerEntity.ID);

                m_PlayerAnimationModule.OnAnimationMoment += SkillAnimationMomentHandler;
                m_PlayerAnimationModule.PlayAnimation(EnumsCollection.AnimationTypes.UseCastableSkill);
            }
        }

        private void SkillAnimationMomentHandler()
        {
            m_PlayerAnimationModule.OnAnimationMoment -= SkillAnimationMomentHandler;
            m_SkillsController.UseSkill(m_SkillID, m_BattleModel.PlayerEntity.ID, m_TargetEntity.ID);
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

        private void PlayerInitializedHandlder(BattleEntity playerEntity)
        {
            m_BattleModel.OnPlayerEntityInitialized -= PlayerInitializedHandlder;

            m_PlayerAnimationModule = playerEntity.GetModule<AnimationModule>();
        }
    }
}
