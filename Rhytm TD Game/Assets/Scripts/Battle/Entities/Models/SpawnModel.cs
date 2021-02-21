using CoreFramework;
using System;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Models
{
    public class SpawnModel : BaseModel
    {
        public Action OnSpawnPlayer;
        public Action<int, BattleEntity> OnPlayerCreated;
        public Action<int, BattleEntity> OnEnemyCreated;
        public Action<int, BattleEntity> OnBulletCreated;
        public Action<BattleEntity> OnEnemyRemoved;
        public Action OnSpawnsInitialized;

        public Vector3 PlayerSpawnPosition;
        public Vector3[] EnemySpawnPosition;

        public bool IsBattleSpawnFinished = false;
    }
}
