using CoreFramework;
using RhytmTD.Data;
using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    [CreateAssetMenu(menuName = "BattleAssets/DefaultEnemyFactory")]
    [System.Serializable]
    public class DefaultEnemyFactory : BaseBattleEntityFactory
    {
        [SerializeField] private int FocusSpeed = 1;

        public MinMaxProgressionConfig ProgressionEnemyHP;
        public MinMaxProgressionConfig ProgressionEnemyDamage;

        public override BattleEntity CreateEntity(Transform transform, float progression01)
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new TransformModule(transform, FocusSpeed));
            battleEntity.AddModule(new HealthModule(entityID, GetHealth(progression01)));
            battleEntity.AddModule(new DamageModule(GetDamage(progression01)));

            return battleEntity;
        }

        private int GetHealth(float progression01)
        {
            (int min, int max) result = ProgressionEnemyHP.EvaluateInt(progression01);
            int rndHealth = Random.Range(result.min, result.max + 1);

            return rndHealth;
        }

        private (int min, int max) GetDamage(float progression01)
        {
            return ProgressionEnemyDamage.EvaluateInt(progression01);
        }
    }
}
