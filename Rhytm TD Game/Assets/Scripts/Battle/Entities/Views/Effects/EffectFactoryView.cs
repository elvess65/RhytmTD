using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models.DataTableModels;
using System;

namespace RhytmTD.Battle.Entities.Views.Effects
{
    public class EffectFactoryView : BaseView, IDisposable
    {
        private EffectsModel m_EffectsModel;
        private WorldDataModel m_WorldModel;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Dispatcher.AddDisposable(this);
 
            m_EffectsModel = Dispatcher.GetModel<EffectsModel>();
            m_EffectsModel.OnEffectEntityCreated += CreateEffectView;

            m_WorldModel = Dispatcher.GetModel<WorldDataModel>();
        }

        private void CreateEffectView(BattleEntity entity)
        {
            EffectModule effectModule = entity.GetModule<EffectModule>();

            BattleEntityView battleEntityView = m_WorldModel.Assets.InstantiatePrefab(m_WorldModel.Assets.EffectsPrefab[effectModule.TypeID - 1]);
            battleEntityView.Initialize(entity);
        }

        public void Dispose()
        {
            m_EffectsModel.OnEffectEntityCreated -= CreateEffectView;

            Dispatcher.RemoveDisposable(this);
        }
    }
}
