
using CoreFramework;

namespace RhytmTD.Battle.Entities.Effects
{
    public class DefaultEffectFactory : IEffectFactory
    {
        public BattleEntity CreateMeteoriteEffect()
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new EffectModule(1));

            return battleEntity;
        }
    }
}
