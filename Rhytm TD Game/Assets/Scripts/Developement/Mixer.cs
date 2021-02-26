using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace RhytmTD.Developement
{
    public class Mixer : MonoBehaviour
    {
        public AudioMixer T;
        public AudioMixerSnapshot NormalSnapshot;
        public AudioMixerSnapshot FadedSnapshot;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NormalSnapshot.TransitionTo(0.5f);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                FadedSnapshot.TransitionTo(0.5f);
            }
        }
    }
}
