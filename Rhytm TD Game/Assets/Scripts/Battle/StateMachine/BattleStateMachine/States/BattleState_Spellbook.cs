using CoreFramework.Input;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.StateMachine
{
    public class BattleState_Spellbook : BattleState_Abstract
    {
        private InputModel m_InputModel;
        private PrepareSkilIUseModel m_PrepareSkilIUseModel;

        private RhytmInputProxy m_RhytmInputProxy;
        private RhytmController m_RhytmController;

        public BattleState_Spellbook() : base()
        {
            m_InputModel = Dispatcher.GetModel<InputModel>();
            m_PrepareSkilIUseModel = Dispatcher.GetModel<PrepareSkilIUseModel>();

            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();
            m_RhytmController = Dispatcher.GetController<RhytmController>();
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
            double mappedProgressToNextTickAnalog = m_RhytmController.ProgressToNextTickAnalog;
            if (mappedProgressToNextTickAnalog <= 0.5f)
            {
                mappedProgressToNextTickAnalog *= 2;
            }
            else
            {
                mappedProgressToNextTickAnalog = (mappedProgressToNextTickAnalog - 0.5f) * 2;
            }

            Debug.Log("Touch " + m_RhytmController.ProgressToNextTickAnalog + " " + mappedProgressToNextTickAnalog);

            if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid(mappedProgressToNextTickAnalog))
            {
                m_PrepareSkilIUseModel.OnTouch?.Invoke();
            }

            m_RhytmInputProxy.RegisterInput();
        }
    }
}
