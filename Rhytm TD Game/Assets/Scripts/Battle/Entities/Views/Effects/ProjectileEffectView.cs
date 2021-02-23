using RhytmTD.Data;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class ProjectileEffectView : BattleEntityView
    {
        [SerializeField] private GameObject MuzzleEffectPrefab;
        [SerializeField] private GameObject BlowEffectPrefab;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            TransformModule transformModule = entity.GetModule<TransformModule>();
            transformModule.OnPositionChanged += OnPositionChanged;

            EffectModule effectModule = entity.GetModule<EffectModule>();
            effectModule.OnEffectAction += EffectActionHandler;
        }

        private void OnPositionChanged(Vector3 position)
        {
            transform.position = position;
        }

        private void EffectActionHandler(DataContainer data)
        {
            string action = data.GetString(DataConsts.ACTION);
            switch (action)
            {
                case DataConsts.BLOW:
                    {
                        ShowBlowEffect();
                        Destroy(gameObject);
                    }
                    break;
                case DataConsts.MUZZLE:
                    {
                        ShowMuzzleEffect();
                    }
                    break;
            }
        }

        private void ShowMuzzleEffect()
        {
            Debug.Log("ShowMuzzleEffect " + transform.position);
            GameObject muzzleEffect = Instantiate(MuzzleEffectPrefab, transform.position, Quaternion.identity);
            muzzleEffect.transform.localScale = Vector3.one * 3;

            Destroy(muzzleEffect, 5f);
        }

        private void ShowBlowEffect()
        {
            GameObject blowEffect = Instantiate(BlowEffectPrefab, transform.position, Quaternion.identity);
            blowEffect.transform.localScale = Vector3.one * 3;

            Destroy(blowEffect, 5f);

            //m_TransformModule.OnPositionChanged -= PositionChanged;
        }
    }
}
