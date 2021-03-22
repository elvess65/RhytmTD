using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class BattleAudioController : BaseController
    {
        private RhytmController m_RhytmController;
        private BattleAudioModel m_AudioModel;
        private SpellBookModel m_SpellBookModel;
        private ApplicationModel m_ApplicationModel;

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

            m_SpellBookModel = Dispatcher.GetModel<SpellBookModel>();
            m_SpellBookModel.OnSpellbookOpened += SpellBookOpenedHandler;
            m_SpellBookModel.OnSpellbookClosed += SpellBookClosedHandler;
            m_SpellBookModel.OnSpellbookUsed += SpellBookClosedHandler;

            m_ApplicationModel = Dispatcher.GetModel<ApplicationModel>();
            m_ApplicationModel.OnPause += ApplicationModel_OnPause;
            m_ApplicationModel.OnResume += ApplicationModel_OnResume;
        }

        private void ApplicationModel_OnPause()
        {
            m_AudioModel.Music.Pause();
        }

        private void ApplicationModel_OnResume()
        {
            m_AudioModel.Music.UnPause();
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

        private void SpellBookOpenedHandler()
        {
            m_AudioModel.SpellbookSnapshot.TransitionTo(ConstsCollection.SPELLBOOK_AUDIO_TRANISTION_DURATION);
        }

        private void SpellBookClosedHandler()
        {
            m_AudioModel.BattleSnapshot.TransitionTo(ConstsCollection.SPELLBOOK_AUDIO_TRANISTION_DURATION);
        }
    }
}
