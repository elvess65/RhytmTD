using System.Collections;
using System.Collections.Generic;
using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;
using RhytmTD.Data.Models.DataTableModels;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class PlayerManaController : BaseController
    {
        private BattleModel m_BattleModel;
        private SkillsModel m_SkillsModel;
        private UpdateModel m_UpdateModel;
        private SpellBookModel m_SpellBookModel;
        private AccountBaseParamsDataModel m_AccountBaseParamsDataModel;

        private ManaModule m_ManaModule;


        public PlayerManaController(Dispatcher dispatcher) : base(dispatcher)
        {
        }


        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_SkillsModel = Dispatcher.GetModel<SkillsModel>();
            m_UpdateModel = Dispatcher.GetModel<UpdateModel>();
            m_SpellBookModel = Dispatcher.GetModel<SpellBookModel>();
            m_AccountBaseParamsDataModel = Dispatcher.GetModel<AccountBaseParamsDataModel>();

            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandlder;
            m_BattleModel.OnBattleStarted += BattleStartedHandler;
            m_BattleModel.OnBattleFinished += BattleFinishedHandler;

            m_SpellBookModel.OnSpellbookOpened += SpellBookOpenedHandler;
            m_SpellBookModel.OnSpellbookClosed += SpellBookClosedAndPostUsedHandler;
            m_SpellBookModel.OnSpellbookPostUsed += SpellBookClosedAndPostUsedHandler;

            m_SkillsModel.OnSkillUsed += SkillUsedHandler;
        }

        public bool IsEnoughManaForSkill(int skillTypeID)
        {
            return m_ManaModule.CurrentMana >= m_AccountBaseParamsDataModel.GetSkillBaseDataByID(skillTypeID).ManaCost;
        }

        public void AddMana()
        {
            AddMana(m_AccountBaseParamsDataModel.BaseCharacterData.ManaInputRestore);
        }


        private void AddMana(int amount)
        {
            Debug.Log("Add mana: " + amount);
        }

        private void RemoveMana(int skillTypeID)
        {
            m_ManaModule.RemoveMana(m_AccountBaseParamsDataModel.GetSkillBaseDataByID(skillTypeID).ManaCost);
        }

        private void ProcessManaIncrease(float deltaTime)
        {
            //AddMana(m_AccountBaseParamsDataModel.BaseCharacterData.ManaAutoRestore);
        }

        #region Handlers

        private void SkillUsedHandler(int skillID, int skillTypeID)
        {
            RemoveMana(skillTypeID);
        }

        private void PlayerInitializedHandlder(BattleEntity playerEntity)
        {
            m_BattleModel.OnPlayerEntityInitialized -= PlayerInitializedHandlder;

            m_ManaModule = playerEntity.GetModule<ManaModule>();
        }

        private void BattleStartedHandler()
        {
            m_UpdateModel.OnUpdate += UpdateHandler;
        }

        private void BattleFinishedHandler(bool isSuccess)
        {
            m_UpdateModel.OnUpdate -= UpdateHandler;
        }

        private void SpellBookOpenedHandler()
        {
            m_UpdateModel.OnUpdate -= UpdateHandler;
        }

        private void SpellBookClosedAndPostUsedHandler()
        {
            m_UpdateModel.OnUpdate += UpdateHandler;
        }

        private void UpdateHandler(float deltaTime)
        {
            if (m_ManaModule != null)
                ProcessManaIncrease(deltaTime);
        }

        #endregion
    }
}
