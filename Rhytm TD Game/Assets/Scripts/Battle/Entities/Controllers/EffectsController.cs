using CoreFramework;
using RhytmTD.Battle.Entities.Effects;
using RhytmTD.Battle.Entities.Models;

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

        public BattleEntity CreateMeteoriteEffect()
        {
            BattleEntity battleEntity = m_EffectFactory.CreateMeteoriteEffect();
            m_EffectsModel.EffectCreated(battleEntity);

            return battleEntity;
        }
    }
}
