using CoreFramework;
using RhytmTD.Battle.Entities.EntitiesFactory;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Controlls battle entities creation
    /// </summary>
    public class SpawnController : BaseController, IBattleEntityFactory
    {
        private SpawnModel m_SpawnModel;
        private BattleModel m_BattleModel;

        private SkillsController m_SkillController;


        private IBattleEntityFactory m_BattleEntityFactory;


        public SpawnController(Dispatcher dispatcher) : base(dispatcher)
        {
            m_BattleEntityFactory = new DefaultBattleEntityFactory();
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
           
            m_SkillController = Dispatcher.GetController<SkillsController>();
        }


        public BattleEntity CreatePlayer(int typeID, Vector3 position, Quaternion rotation, float moveSpeed, int health, int minDamage, int maxDamage)
        {
            BattleEntity entity = m_BattleEntityFactory.CreatePlayer(typeID, position, rotation, moveSpeed, health, minDamage, maxDamage);

            BattleEntity meteoriteSkill = m_SkillController.CreateMeteoriteSkill();
            BattleEntity fireballSkill = m_SkillController.CreateFireballSkill();

            LoadoutModule loadoutModule = entity.GetModule<LoadoutModule>();
            loadoutModule.AddSkill(meteoriteSkill.ID, 1);
            loadoutModule.AddSkill(fireballSkill.ID, 2);

            m_SpawnModel.OnPlayerCreated?.Invoke(typeID, entity);

            return entity;
        }

        public BattleEntity CreateEnemy(int typeID, Vector3 position, Quaternion rotation, float rotateSpeed, int health, int minDamage, int maxDamage)
        {
            BattleEntity entity = m_BattleEntityFactory.CreateEnemy(typeID, position, rotation, rotateSpeed, health, minDamage, maxDamage);
            m_SpawnModel.OnEnemyCreated?.Invoke(typeID, entity);

            return entity;
        }

        public BattleEntity CreateBullet(int typeID, Vector3 position, Quaternion rotation, float speed, BattleEntity owner)
        {
            BattleEntity entity = m_BattleEntityFactory.CreateBullet(typeID, position, rotation, speed, owner);
            m_SpawnModel.OnBulletCreated?.Invoke(typeID, entity);

            DestroyModule destroyModule = entity.GetModule<DestroyModule>();
            destroyModule.OnDestroyed += BulletDestroyedHandler;

            return entity;
        }


        private void BulletDestroyedHandler(BattleEntity battleEntity)
        {
            m_BattleModel.RemoveBattleEntity(battleEntity.ID);
        }

    }
}
