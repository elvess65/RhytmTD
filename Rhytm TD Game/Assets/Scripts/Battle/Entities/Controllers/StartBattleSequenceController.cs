using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Controllers;
using System.Collections;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class StartBattleSequenceController : BaseController
    {
        private StartBattleSequenceModel m_StartBattleSequenceModel;

        public StartBattleSequenceController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_StartBattleSequenceModel = Dispatcher.GetModel<StartBattleSequenceModel>();
        }

        public void StartStartBattleSequence()
        {
            Dispatcher.GetController<UpdateController>().UpdaterObject.GetComponent<MonoUpdater>().StartCoroutine(SequenceCoroutine());
        }

        private IEnumerator SequenceCoroutine()
        {
            yield return new WaitForSeconds(m_StartBattleSequenceModel.AnimationDelay);

            m_StartBattleSequenceModel.OnSequencePrepared?.Invoke();

            yield return new WaitForSeconds(m_StartBattleSequenceModel.AnimationDelay);

            m_StartBattleSequenceModel.OnSequenceFinished?.Invoke();
        }
    }
}
