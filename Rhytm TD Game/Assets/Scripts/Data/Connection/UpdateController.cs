
using CoreFramework;
using RhytmTD.Data.Models;
using UnityEngine;

namespace RhytmTD.Data.Controllers
{
    public class UpdateController : BaseController
    {
        private GameObject m_GameObject;
        private UpdateModel m_UpdateModel;

        public UpdateController(Dispatcher dispatcher) : base(dispatcher)
        {

        }

        public override void InitializeComplete()
        {
            m_UpdateModel = Dispatcher.GetModel<UpdateModel>();
            m_GameObject = new GameObject("Updater");
            m_GameObject.AddComponent<MonoUpdater>();
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
