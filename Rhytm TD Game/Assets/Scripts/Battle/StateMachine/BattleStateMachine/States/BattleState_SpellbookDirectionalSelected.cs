using CoreFramework.Input;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models.DataTableModels;
using UnityEngine;

namespace RhytmTD.Battle.StateMachine
{
    public class BattleState_SpellbookDirectionalSelected : BattleState_Abstract
    {
        private InputModel m_InputModel;
        private BattleModel m_BattleModel;
        private PrepareSkilIUseModel m_PrepareSkilIUseModel;
        private AccountBaseParamsDataModel m_AccountBaseParamsDataModel;

        private RhytmInputProxy m_RhytmInputProxy;
        private TargetingController m_TargetingController;

        private SlotModule m_PlayerSlotModule;


        public BattleState_SpellbookDirectionalSelected() : base()
        {
            m_InputModel = Dispatcher.GetModel<InputModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_PrepareSkilIUseModel = Dispatcher.GetModel<PrepareSkilIUseModel>();
            m_AccountBaseParamsDataModel = Dispatcher.GetModel<AccountBaseParamsDataModel>();

            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();
            m_TargetingController = Dispatcher.GetController<TargetingController>();

            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandlder;
        }

        public override void EnterState()
        {
            base.EnterState();

            m_InputModel.OnTouch += HandleTouch;
        }

        public override void ExitState()
        {
            base.ExitState();

            m_InputModel.OnTouch -= HandleTouch;
        }


        private void HandleTouch(Vector3 mouseScreenPos)
        {
            if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid())
            {
                Vector3 skillDirection = m_TargetingController.GetDirection(mouseScreenPos, m_PlayerSlotModule.ProjectileSlot.position, out Vector3 hitPos);

                //Make possible to attack only forward
                if (skillDirection.z <= 0)
                {
                    m_RhytmInputProxy.RegisterInput();
                    return;
                }

                m_PrepareSkilIUseModel.OnSkilDirectionSelected?.Invoke(skillDirection, hitPos);
            }

            m_RhytmInputProxy.RegisterInput();
        }

        private void PlayerInitializedHandlder(BattleEntity playerEntity)
        {
            m_BattleModel.OnPlayerEntityInitialized -= PlayerInitializedHandlder;

            m_PlayerSlotModule = playerEntity.GetModule<SlotModule>();
        }
    }
}
