﻿using RhytmTD.Data;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views.Effects
{
    public class MeteoriteEffectView : BattleEntityView
    {
        [SerializeField] private GameObject BlowEffectPrefab;

        private TransformModule m_TransformModule;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_TransformModule = entity.GetModule<TransformModule>();
            m_TransformModule.OnPositionChanged += PositionChanged;

            transform.position = m_TransformModule.Position;

            EffectModule effectModule = entity.GetModule<EffectModule>();
            effectModule.OnEffectAction += EffectActionHandler;
        }

        private void PositionChanged(Vector3 position)
        {
            transform.position = position;
        }

        private void EffectActionHandler(DataContainer data)
        {
            string action = data.GetString("action");
            switch (action)
            {
                case "blow":
                    {
                        float radius = data.GetFloat("radius");

                        Blow(radius);
                        Destroy(gameObject);
                    }
                    break;
            }
        }

        private void Blow(float radius)
        {
            GameObject blowEffect = Instantiate(BlowEffectPrefab, transform.position, Quaternion.identity);
            blowEffect.transform.localScale = Vector3.one * radius;

            Destroy(blowEffect, 5f);

            m_TransformModule.OnPositionChanged -= PositionChanged;
        }
    }
}