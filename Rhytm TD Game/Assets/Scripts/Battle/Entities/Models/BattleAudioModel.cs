using CoreFramework;
using CoreFramework.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace RhytmTD.Battle.Entities.Models
{
    public class BattleAudioModel : BaseModel
    {
        private int m_BPM;

        public int BPM
        {
            get => m_BPM;
            set
            {
                m_BPM = value;
                OnBPMChanged?.Invoke(m_BPM);
            }
        }

        public Metronome Metronome;
        public AudioSource Music;
        public AudioMixer Mixer;
        public AudioMixerSnapshot BattleSnapshot;
        public AudioMixerSnapshot SpellbookSnapshot;

        public System.Action<bool> OnPlayMetronome;
        public System.Action<bool> OnPlayMusic;
        public System.Action<int> OnBPMChanged;
    }
}
