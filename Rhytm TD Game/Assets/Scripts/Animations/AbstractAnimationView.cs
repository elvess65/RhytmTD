using UnityEngine;
using System.Collections.Generic;
using static CoreFramework.EnumsCollection;
using RhytmTD.Battle.Entities.Views;
using RhytmTD.Battle.Entities;

namespace RhytmTD.Animation
{
    [System.Serializable]
    public abstract class AbstractAnimationView : BattleEntityView
    {
        public Animator Controller;
        public AnimationKeys[] ExposedAnimationKeys;

        protected AnimationEventListener m_EventListener;

        private AnimationModule m_AnimationModule;

        private Dictionary<AnimationTypes, string> m_AnimationKeys;
        private Dictionary<string, float> m_AnimationActionEventsExecuteTime;

        protected const string m_BASE_LAYER = "Base Layer";


        public abstract void PlayAnimation(AnimationTypes animationType);

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_AnimationModule = entity.GetModule<AnimationModule>();
            m_AnimationModule.OnPlayAnimation += PlayAnimation;
            m_AnimationModule.OnChangeSpeedMultiplayer += SetSpeedMultiplayer;

            Initialize();
        }

        public virtual void Initialize()
        {
            m_EventListener = Controller.GetComponent<AnimationEventListener>();
            if (m_EventListener != null && m_AnimationModule != null)
                m_EventListener.OnAnimationMoment += m_AnimationModule.AnimationMomentHandler;

            m_AnimationKeys = new Dictionary<AnimationTypes, string>();
            m_AnimationActionEventsExecuteTime = new Dictionary<string, float>();

            //Match action types with keys
            for (int i = 0; i < ExposedAnimationKeys.Length; i++)
            {
                if (!m_AnimationKeys.ContainsKey(ExposedAnimationKeys[i].Type))
                    m_AnimationKeys.Add(ExposedAnimationKeys[i].Type, ExposedAnimationKeys[i].Key);
            }

            //Match action types with delays
            AnimationClip[] clips = Controller.runtimeAnimatorController.animationClips;

            //Loop through all animation clips available in animator controller
            foreach (AnimationClip clip in clips)
            {
                //Loop through all animation keys mentioned in exposed keys list
                foreach (string animKey in m_AnimationKeys.Values)
                {
                    //Compare clip name with key (names should match)
                    if (clip.name.Equals(animKey))
                    {
                        //If there are some events and the key was not added - add to animation delay list
                        if (clip.events.Length > 0 && !m_AnimationActionEventsExecuteTime.ContainsKey(animKey))
                            m_AnimationActionEventsExecuteTime.Add(animKey, clip.events[0].time);

                        continue;
                    }
                }
            }
        }

        public float GetActionEventExecuteTime(AnimationTypes animationType)
        {
            if (m_AnimationActionEventsExecuteTime.TryGetValue(GetKeyByType(animationType), out float eventExecuteTime))
                return eventExecuteTime;

            return 0;
        }


        protected void SetTrigger(string name)
        {
            Controller.SetTrigger(name);
        }

        protected void SetBool(string name, bool state)
        {
            Controller.SetBool(name, state);
        }

        protected bool GetBool(string name)
        {
            return Controller.GetBool(name);
        }

        protected string GetKeyByType(AnimationTypes animationType)
        {
            if (m_AnimationKeys.ContainsKey(animationType))
                return m_AnimationKeys[animationType];

            return string.Empty;
        }

        private void SetSpeedMultiplayer(float speedMultiplayer)
        {
            Controller.speed = 1f * speedMultiplayer;
        }

        private void OnDestroy()
        {
            if (m_AnimationModule != null)
            {
                m_AnimationModule.OnPlayAnimation -= PlayAnimation;
            }
        }


        [System.Serializable]
        public class AnimationKeys
        {
            public AnimationTypes Type;
            public string Key;
        }
    }
}

