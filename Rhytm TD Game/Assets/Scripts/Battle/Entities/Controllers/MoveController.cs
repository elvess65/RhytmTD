﻿using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Передвижение
    /// </summary>
    public class MoveController : BaseController
    {
        private BattleModel m_BattleModel;
        private float m_CurSpeedMultiplayer = 1;

        public MoveController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnBattleStarted += BattleStartedHandler;
            m_BattleModel.OnBattleFinished += BattleFinishedHandler;
            m_BattleModel.OnSpellbookOpened += SpellBookEnterHandler;
            m_BattleModel.OnSpellbookClosed += SpellBookExitHandler;
            m_BattleModel.OnSpellbookUsed += SpellBookExitHandler;

            UpdateModel updateModel = Dispatcher.GetModel<UpdateModel>();
            updateModel.OnUpdate += Update;
        }

        public void Update(float deltaTime)
        {
            MoveAll(deltaTime);
        }

        private void MoveAll(float deltaTime)
        {
            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (entity.HasModule<MoveModule>())
                {
                    MoveModule moveModule = entity.GetModule<MoveModule>();

                    if (moveModule.IsMoving)
                    {
                        TransformModule transformModule = entity.GetModule<TransformModule>();
                        transformModule.Position += moveModule.MoveDirection * moveModule.CurrentSpeed * deltaTime * m_CurSpeedMultiplayer;
                    }
                }
            }
        }

        private void BattleStartedHandler()
        {
            m_BattleModel.PlayerEntity.GetModule<MoveModule>().StartMove(UnityEngine.Vector3.forward);
        }

        private void BattleFinishedHandler(bool isSuccess)
        {
            m_BattleModel.PlayerEntity?.GetModule<MoveModule>().Stop();
        }

        private void SpellBookEnterHandler()
        {
            m_CurSpeedMultiplayer = ConstsCollection.SPELLBOOK_SPEED_MULTIPLAYER;
        }

        private void SpellBookExitHandler()
        {
            m_CurSpeedMultiplayer = 1f;
        }
    }
}
