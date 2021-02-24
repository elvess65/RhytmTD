using RhytmTD.Data;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views.Effects
{
    public class FireballEffectView : BattleEntityView
    {
        [SerializeField] private GameObject MuzzleEffectPrefab;
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

            ShowMuzzleEffect();
        }

        private void PositionChanged(Vector3 position)
        {
            transform.position = position;
        }

        private void EffectActionHandler(DataContainer data)
        {
            string action = data.GetString(ConstsCollection.DataConsts.ACTION);
            switch (action)
            {
                case ConstsCollection.DataConsts.EXPLOSION:
                {
                    ShowExplosionEffect();
                    Destroy(gameObject);
                    break;
                }    
            }
        }

        private void ShowMuzzleEffect()
        {
            GameObject muzzleEffect = Instantiate(MuzzleEffectPrefab, transform.position, Quaternion.identity);
            muzzleEffect.transform.localScale = Vector3.one * 3;

            Destroy(muzzleEffect, 5f);
        }

        private void ShowExplosionEffect()
        {
            GameObject blowEffect = Instantiate(BlowEffectPrefab, transform.position, Quaternion.identity);
            blowEffect.transform.localScale = Vector3.one * 3;

            Destroy(blowEffect, 5f);

            m_TransformModule.OnPositionChanged -= PositionChanged;
        }
    }
}
