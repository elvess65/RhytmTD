using CoreFramework;
using CoreFramework.Utils;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;
using UnityEngine.Audio;

namespace RhytmTD.Battle.Entities.Views
{
    public class BattleAudioView : BaseView
    {
        public Metronome Metronome;
        public AudioSource Music;
        public AudioMixer Mixer;
        public AudioMixerSnapshot BattleSnapshot;
        public AudioMixerSnapshot SpellbookSnapshot;

        private BattleAudioModel m_AudioModel;

        void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_AudioModel = Dispatcher.GetModel<BattleAudioModel>();

            m_AudioModel.Metronome = Metronome;
            m_AudioModel.Music = Music;
            m_AudioModel.Mixer = Mixer;
            m_AudioModel.BattleSnapshot = BattleSnapshot;
            m_AudioModel.SpellbookSnapshot = SpellbookSnapshot;
        }
    }
}
