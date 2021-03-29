using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class RhytmDynamicController : BaseController
    {
        private BattleAudioModel m_BattleAudioModel;
        private RhytmDynamicModel m_RhytmDynamicModel;


        public RhytmDynamicController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleAudioModel = Dispatcher.GetModel<BattleAudioModel>();
            m_RhytmDynamicModel = Dispatcher.GetModel<RhytmDynamicModel>();
        }

        public void ChangeDynamic(RhytmDynamic dynamic)
        {
            if (m_RhytmDynamicModel.Dynamic != dynamic)
            {
                m_RhytmDynamicModel.Dynamic = dynamic;

                switch (dynamic)
                {
                    case RhytmDynamic.x1:
                        m_BattleAudioModel.BPM = m_BattleAudioModel.OriginalBPM;
                        break;

                    case RhytmDynamic.x2:
                        m_BattleAudioModel.BPM = m_BattleAudioModel.OriginalBPM * 2;
                        break;
                }
            }
        }
    }
}
