using UnityEngine;

namespace CoreFramework
{
    public class UpdateController : BaseController
    {
        public GameObject UpdaterObject { get; private set; }

        private UpdateModel m_UpdateModel;

        public UpdateController(Dispatcher dispatcher) : base(dispatcher)
        {

        }

        public override void InitializeComplete()
        {
            m_UpdateModel = Dispatcher.GetModel<UpdateModel>();

            UpdaterObject = new GameObject("Updater");
            UpdaterObject.AddComponent<MonoUpdater>();
        }

        public void Update(float t)
        {
            m_UpdateModel.OnUpdate?.Invoke(t);
        }
    }

    internal class MonoUpdater : BaseView
    {
        private UpdateController m_UpdateController;

        private void Awake()
        {
            m_UpdateController = Dispatcher.GetController<UpdateController>();
        }

        private void Update()
        {
            m_UpdateController.Update(Time.deltaTime);
        }
    }
}
