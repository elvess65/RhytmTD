
using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views.Effects
{
    public class EffectFactoryView : BaseView
    {
        public GameObject[] EffectsPrefab;

        private EffectsModel m_EffectsModel;

        private void Awake()
        {
            m_EffectsModel = Dispatcher.GetModel<EffectsModel>();
            m_EffectsModel.OnEffectCreated += EffectCreatedHandler;
        }

        private void EffectCreatedHandler(BattleEntity entity)
        {
            EffectModule effectModule = entity.GetModule<EffectModule>();

            GameObject prefab = EffectsPrefab[effectModule.TypeID - 1];
            GameObject effectGo = GameObject.Instantiate(prefab);

            BattleEntityView battleEntityView = effectGo.GetComponent<BattleEntityView>();
            battleEntityView.Initialize(entity);
        }
    }
}
