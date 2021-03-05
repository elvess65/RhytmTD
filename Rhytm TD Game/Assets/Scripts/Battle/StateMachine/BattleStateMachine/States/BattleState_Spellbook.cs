using CoreFramework.Input;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.StateMachine
{
    public class BattleState_Spellbook : BattleState_Abstract
    {
        private InputModel m_InputModel;
        private BattleAudioModel m_AudioModel;
        private PrepareSkilIUseModel m_PrepareSkilIUseModel;

        private RhytmInputProxy m_RhytmInputProxy;

        public BattleState_Spellbook() : base()
        {
            m_InputModel = Dispatcher.GetModel<InputModel>();
            m_AudioModel = Dispatcher.GetModel<BattleAudioModel>();
            m_PrepareSkilIUseModel = Dispatcher.GetModel<PrepareSkilIUseModel>();

            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();
        }

        public override void EnterState()
        {
            base.EnterState();

            //m_AudioModel.BPM = m_AudioModel.BPM * 2;
            m_InputModel.OnTouch += HandleTouch;
        }

        public override void ExitState()
        {
            base.ExitState();

            //m_AudioModel.BPM = m_AudioModel.BPM / 2;
            m_InputModel.OnTouch -= HandleTouch;
        }

        private void HandleTouch(Vector3 mouseScreenPos)
        {
            if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid())
            {
                m_PrepareSkilIUseModel.OnCorrectTouch?.Invoke();
            }
            else
            {
                m_PrepareSkilIUseModel.OnWrongTouch?.Invoke();
            }


            m_RhytmInputProxy.RegisterInput();
        }
    }
}
