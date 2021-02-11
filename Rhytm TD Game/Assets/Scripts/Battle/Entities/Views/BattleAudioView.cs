using CoreFramework;
using CoreFramework.Utils;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class BattleAudioView : BaseView
    {
        public Metronome Metronome;
        public AudioSource Music;

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
        }
    }
}
