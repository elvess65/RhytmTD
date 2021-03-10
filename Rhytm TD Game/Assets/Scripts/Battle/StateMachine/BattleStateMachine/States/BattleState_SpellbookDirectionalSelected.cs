using CoreFramework.Input;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.StateMachine
{
    public class BattleState_SpellbookDirectionalSelected : BattleState_Abstract
    {
        private InputModel m_InputModel;
        private PrepareSkilIUseModel m_PrepareSkilIUseModel;

        private RhytmInputProxy m_RhytmInputProxy;

        public BattleState_SpellbookDirectionalSelected() : base()
        {
            m_InputModel = Dispatcher.GetModel<InputModel>();
            m_PrepareSkilIUseModel = Dispatcher.GetModel<PrepareSkilIUseModel>();

            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();
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
                m_PrepareSkilIUseModel.OnSkilDirectionSelected?.Invoke(new Vector3(0, 0, 1));
            }

            m_RhytmInputProxy.RegisterInput();
        }
    }
}
