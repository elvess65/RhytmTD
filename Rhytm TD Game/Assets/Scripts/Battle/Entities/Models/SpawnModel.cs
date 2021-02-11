using CoreFramework;
using RhytmTD.Battle.Entities.EntitiesFactory;

namespace RhytmTD.Battle.Entities.Models
{
    public class SpawnModel : BaseModel
    {
        public System.Func<EntityFactorySetup, BattleEntity> OnSpawnPlayerEntity;
        public System.Func<EntityFactorySetup, BattleEntity> OnSpawnEnemyEntity;
        public System.Action OnResetSpawnAreaUsedAmount;
        public System.Action OnAdjustSpawnAreaPosition;
        public System.Action<TransformModule> OnCacheSpawnAreaPosition;
    }
}
