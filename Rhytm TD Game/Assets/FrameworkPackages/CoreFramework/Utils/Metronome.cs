﻿using UnityEngine;

namespace CoreFramework.Utils
{
    [RequireComponent(typeof(AudioSource))]
    public class Metronome : BaseView
    {
        public double bpm = 140.0F;
        public float gain = 0.5F;
        public int signatureHi = 4;
        public int signatureLo = 4;

        private double nextTick = 0.0F;
        private float amp = 0.0F;
        private float phase = 0.0F;
        private double sampleRate = 0.0F;
        private int accent;
        private bool running = false;

        private ApplicationModel m_ApplicationModel;

        private void Awake()
        {
            m_ApplicationModel = Dispatcher.GetModel<ApplicationModel>();
            m_ApplicationModel.OnPause += ApplicationModel_OnPause;
            m_ApplicationModel.OnResume += ApplicationModel_OnResume;
        }

        private void ApplicationModel_OnPause()
        {
            StopMetronome();
        }

        private void ApplicationModel_OnResume()
        {
            StartMetronome();
        }

        public void StartMetronome()
        {
            sampleRate = AudioSettings.outputSampleRate;
            accent = signatureHi;

            double startTick = AudioSettings.dspTime;
            nextTick = startTick * sampleRate;

            running = true;
        }

        public void StopMetronome()
        {
            running = false;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            if (!running)
                return;

            double samplesPerTick = sampleRate * 60.0F / bpm * 4.0F / signatureLo;
            double sample = AudioSettings.dspTime * sampleRate;
            int dataLen = data.Length / channels;
            int n = 0;

            while (n < dataLen)
            {
                float x = gain * amp * Mathf.Sin(phase);
                int i = 0;
                while (i < channels)
                {
                    data[n * channels + i] += x;
                    i++;
                }

                while (sample + n >= nextTick)
                {
                    nextTick += samplesPerTick;
                    amp = 1.0F;
                    if (++accent > signatureHi)
                    {
                        accent = 1;
                        amp *= 2.0F;
                    }
                }

                phase += amp * 0.3F;
                amp *= 0.993F;
                n++;
            }
        }
    }
}