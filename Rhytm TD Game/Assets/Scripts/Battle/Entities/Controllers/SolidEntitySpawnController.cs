using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.EntitiesFactory;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models.DataTableModels;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Controls solid battle entities spawning
    /// </summary>
    public class SolidEntitySpawnController : BaseController
    {
        private SpawnModel m_SpawnModel;
        private BattleModel m_BattleModel;
        private AccountBaseParamsDataModel m_AccountBaseParamsDataModel;

        private RhytmController m_RhytmController;
        private SkillsController m_SkillController;

        private IBattleEntityFactory m_BattleEntityFactory;


        public SolidEntitySpawnController(Dispatcher dispatcher) : base(dispatcher)
        {
            m_BattleEntityFactory = new DefaultBattleEntityFactory();
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();
            m_SpawnModel.OnShouldCreatePlayer += SpawnPlayer;

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_AccountBaseParamsDataModel = Dispatcher.GetModel<AccountBaseParamsDataModel>();

            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_SkillController = Dispatcher.GetController<SkillsController>();
        }

      
        public BattleEntity SpawnEnemy(int typeID, Vector3 position, Quaternion rotation, float rotateSpeed, int health, int minDamage, int maxDamage)
        {
            BattleEntity entity = m_BattleEntityFactory.CreateEnemy(typeID, position, rotation, rotateSpeed, health, minDamage, maxDamage);
            m_SpawnModel.OnEnemyEntityCreated?.Invoke(typeID, entity);

            return entity;
        }


        private void SpawnPlayer()
        {
            float moveSpeed = m_AccountBaseParamsDataModel.BaseCharacterData.MoveSpeedUnitsPerTick * (1 / (float)m_RhytmController.TickDurationSeconds);
            int typeID = 1;

            //Spawn Entity
            BattleEntity entity = CreatePlayer(typeID, m_SpawnModel.PlayerSpawnPosition, Quaternion.identity,
                                                       moveSpeed,
                                                       m_AccountBaseParamsDataModel.BaseCharacterData.Health,
                                                       m_AccountBaseParamsDataModel.BaseCharacterData.MinDamage,
                                                       m_AccountBaseParamsDataModel.BaseCharacterData.MaxDamage);

            m_BattleModel.AddBattleEntity(entity);
            m_BattleModel.PlayerEntity = entity;
        }

        private BattleEntity CreatePlayer(int typeID, Vector3 position, Quaternion rotation, float moveSpeed, int health, int minDamage, int maxDamage)
        {
            BattleEntity entity = m_BattleEntityFactory.CreatePlayer(typeID, position, rotation, moveSpeed, health, minDamage, maxDamage);

            BattleEntity meteoriteSkill = m_SkillController.CreateMeteoriteSkillEntity();
            BattleEntity fireballSkill = m_SkillController.CreateFireballSkillEntity();

            LoadoutModule loadoutModule = entity.GetModule<LoadoutModule>();
            loadoutModule.AddSkill(meteoriteSkill.ID, 1);
            loadoutModule.AddSkill(fireballSkill.ID, 2);

            m_SpawnModel.OnPlayerEntityCreated?.Invoke(typeID, entity);

            return entity;
        }
    }
}
