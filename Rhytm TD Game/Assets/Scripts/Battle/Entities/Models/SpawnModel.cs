using CoreFramework;

namespace RhytmTD.Battle.Entities.Models
{
    public class SpawnModel : BaseModel
    {
        public System.Action<int, BattleEntity> OnPlayerCreated;
        public System.Action<int, BattleEntity> OnEnemyCreated;
        public System.Action<int, BattleEntity> OnBulletCreated;
        public System.Action OnResetSpawnAreaUsedAmount;
        public System.Action OnAdjustSpawnAreaPosition;
        public System.Action<TransformModule> OnCacheSpawnAreaPosition;

        public void RiseOnPlayerCreated(int typeID, BattleEntity battleEntity)
        {
            OnPlayerCreated?.Invoke(typeID, battleEntity);
        }

        public void RiseOnEnemyCreated(int typeID, BattleEntity battleEntity)
        {
            OnEnemyCreated?.Invoke(typeID, battleEntity);
        }

        public void RiseOnBulletCreated(int typeID, BattleEntity battleEntity)
        {
            OnBulletCreated?.Invoke(typeID, battleEntity);
        }
    }
}
