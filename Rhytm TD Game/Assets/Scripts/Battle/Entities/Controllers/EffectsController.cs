using CoreFramework;
using RhytmTD.Battle.Entities.Effects;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class EffectsController : BaseController
    {
        private EffectsModel m_EffectsModel;
        private IEffectFactory m_EffectFactory;

        public EffectsController(Dispatcher dispatcher) : base(dispatcher)
        {
            m_EffectFactory = new DefaultEffectFactory();
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_EffectsModel = Dispatcher.GetModel<EffectsModel>();
        }

        public BattleEntity CreateMeteoriteEffect(Vector3 position, Quaternion rotation, float moveSpeed)
        {
            BattleEntity battleEntity = m_EffectFactory.CreateMeteoriteEffect(position, rotation, moveSpeed);
            m_EffectsModel.EffectCreated(battleEntity);

            return battleEntity;
        }

        public BattleEntity CreateFireballEffect(Vector3 position, Quaternion rotation, float moveSpeed)
        {
            BattleEntity battleEntity = m_EffectFactory.CreateFireballEffect(position, rotation, moveSpeed);
            m_EffectsModel.EffectCreated(battleEntity);

            return battleEntity;
        }
    }
}
