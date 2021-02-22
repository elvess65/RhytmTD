using CoreFramework;
using System;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Models
{
    public class SpawnModel : BaseModel
    {
        public Action OnShouldCreatePlayer;
        public Action<int, BattleEntity> OnPlayerEntityCreated;
        public Action<int, BattleEntity> OnEnemyEntityCreated;
        public Action<int, BattleEntity> OnBulletEntityCreated;
        public Action<BattleEntity> OnEnemyRemoved;
        public Action OnSpawnPointsInitialized;

        public Vector3 PlayerSpawnPosition;
        public Vector3[] EnemySpawnPosition;

        public bool IsBattleSpawnFinished = false;
    }
}
