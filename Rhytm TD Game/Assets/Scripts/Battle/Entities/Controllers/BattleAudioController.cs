using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class BattleAudioController : BaseController
    {
        private RhytmController m_RhytmController;
        private BattleAudioModel m_AudioModel;

        public BattleAudioController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_AudioModel = Dispatcher.GetModel<BattleAudioModel>();

            m_AudioModel.OnPlayMetronome += PlayMetronomeHandler;
            m_AudioModel.OnPlayMusic += PlayMusicHandler;
            m_AudioModel.OnBPMChanged += BPMChangedHandler;
        }


        private void PlayMetronomeHandler(bool startPlaying)
        {
            if (startPlaying)
                m_AudioModel.Metronome.StartMetronome();
            else
                m_AudioModel.Metronome.StopMetronome();
        }

        private void PlayMusicHandler(bool startPlaying)
        {
            if (startPlaying)
                m_AudioModel.Music.Play();
            else
                m_AudioModel.Music.Stop();
        }

        private void BPMChangedHandler(int bpm)
        {
            m_AudioModel.Metronome.bpm = bpm;
            m_RhytmController.SetBPM(bpm);
        }
    }
}
