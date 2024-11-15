﻿using CoreFramework;
using CoreFramework.Input;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Setup
{
    /// <summary>
    /// Setup для боя
    /// </summary>
    public class BattleGameSetup : IGameSetup
    {
        public void Setup()
        {
            Dispatcher dispatcher = Dispatcher.Instance;

            // Controllers
            dispatcher.CreateController<MoveController>();
            dispatcher.CreateController<RotateController>();
            dispatcher.CreateController<FocusController>();
            dispatcher.CreateController<RhytmController>();
            dispatcher.CreateController<RhytmInputProxy>();
            dispatcher.CreateController<InputController>();
            dispatcher.CreateController<SolidEntitySpawnController>();
            dispatcher.CreateController<DamageController>();
            dispatcher.CreateController<EnemyBehavoiurController>();
            dispatcher.CreateController<CameraController>();
            dispatcher.CreateController<BattleAudioController>();
            dispatcher.CreateController<ShootController>();
            dispatcher.CreateController<PlayerBehavoiurController>();
            dispatcher.CreateController<TargetingController>();
            dispatcher.CreateController<BattleController>();
            dispatcher.CreateController<BattleUIController>();
            dispatcher.CreateController<BattleProgressController>();
            dispatcher.CreateController<SkillsController>();
            dispatcher.CreateController<EffectsController>();
            dispatcher.CreateController<WaveController>();
            dispatcher.CreateController<MarkerController>();
            dispatcher.CreateController<PrepareSkillUseController>();
            dispatcher.CreateController<PlayerRhytmInputHandleController>();
            dispatcher.CreateController<SkillsCooldownController>();
            dispatcher.CreateController<SyncPositionController>();
            dispatcher.CreateController<EnviromentController>();
            dispatcher.CreateController<SpellBookController>();
            dispatcher.CreateController<ApplicationController>();
            dispatcher.CreateController<PlayerManaController>();
            dispatcher.CreateController<RhytmDynamicController>();

            // Models
            dispatcher.CreateModel<BattleModel>();
            dispatcher.CreateModel<CameraModel>();
            dispatcher.CreateModel<BattleAudioModel>();
            dispatcher.CreateModel<SpawnModel>();
            dispatcher.CreateModel<BattleUIModel>();
            dispatcher.CreateModel<SkillsModel>();
            dispatcher.CreateModel<EffectsModel>();
            dispatcher.CreateModel<InputModel>();
            dispatcher.CreateModel<MarkerModel>();
            dispatcher.CreateModel<StartBattleSequenceModel>();
            dispatcher.CreateModel<PrepareSkilIUseModel>();
            dispatcher.CreateModel<PlayerRhytmInputHandleModel>();
            dispatcher.CreateModel<SkillsCooldownModel>();
            dispatcher.CreateModel<EnviromentModel>();
            dispatcher.CreateModel<SpellBookModel>();
            dispatcher.CreateModel<ApplicationModel>();
            dispatcher.CreateModel<RhytmDynamicModel>();
        }
    }
}
