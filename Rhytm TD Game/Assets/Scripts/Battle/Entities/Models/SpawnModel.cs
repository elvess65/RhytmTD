using CoreFramework;
using System;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Models
{
    public class SpawnModel : BaseModel
    {
        public Action<int, BattleEntity> OnPlayerCreated;
        public Action<int, BattleEntity> OnEnemyCreated;
        public Action<int, BattleEntity> OnBulletCreated;

        public Vector3 PlayerSpawnPosition;
        public Vector3[] EnemySpawnPosition;
        public Action SpawnsInitialized;

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

        public void RiseSpawnsInitialized()
        {
            SpawnsInitialized?.Invoke();
        }
    }
}
