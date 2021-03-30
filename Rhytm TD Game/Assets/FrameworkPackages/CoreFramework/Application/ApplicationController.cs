using UnityEngine;

namespace CoreFramework
{
    public class ApplicationController : BaseController
    {
        private ApplicationModel m_ApplicationModel;

        public ApplicationController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_ApplicationModel = Dispatcher.GetModel<ApplicationModel>();
        }

        public void Pause()
        {
            Time.timeScale = 0;
            m_ApplicationModel.Paused();
        }

        public void UpPause()
        {
            Time.timeScale = 1;
            m_ApplicationModel.Resumed();
        }
    }
}
