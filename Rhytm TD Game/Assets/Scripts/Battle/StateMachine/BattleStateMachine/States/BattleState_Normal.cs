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
        private MarkerController m_MarkerController;

        public BattleState_Normal() : base()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_InputModel = Dispatcher.GetModel<InputModel>();
            m_ShootController = Dispatcher.GetController<ShootController>();
            m_FindTargetController = Dispatcher.GetController<FindTargetController>();
            m_SkillsController = Dispatcher.GetController<SkillsController>();
            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();
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
                    AnimationModule animationModule = m_BattleModel.PlayerEntity.GetModule<AnimationModule>();
                    animationModule.PlayAnimation(CoreFramework.EnumsCollection.AnimationTypes.Attack);

                    m_ShootController.Shoot(m_BattleModel.PlayerEntity, targetEntity);
                }
            }

            m_RhytmInputProxy.RegisterInput();
        }

        private void HandleKeyDown(KeyCode keyCode)
        {
            if (keyCode == KeyCode.Space)
            {
                BattleEntity target = m_FindTargetController.GetNearestTarget(m_BattleModel.PlayerEntity);
                if (target != null)
                {
                    LoadoutModule loadoutModule = m_BattleModel.PlayerEntity.GetModule<LoadoutModule>();
                    int skillID = loadoutModule.GetSkillIDByTypeID(1);

                    m_SkillsController.UseSkill(skillID, m_BattleModel.PlayerEntity.ID, target.ID);
                }
            }
            else if (keyCode == KeyCode.V)
            {
                BattleEntity target = m_FindTargetController.GetNearestTarget(m_BattleModel.PlayerEntity);
                if (target != null)
                {
                    LoadoutModule loadoutModule = m_BattleModel.PlayerEntity.GetModule<LoadoutModule>();
                    int skillID = loadoutModule.GetSkillIDByTypeID(2);

                    m_SkillsController.UseSkill(skillID, m_BattleModel.PlayerEntity.ID, target.ID);
                }
            }
        }
    }
}
