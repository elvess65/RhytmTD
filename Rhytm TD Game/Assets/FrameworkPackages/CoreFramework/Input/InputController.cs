using RhytmTD.Data.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CoreFramework.Input
{
    /// <summary>
    /// Tracking low level input
    /// </summary>
    public class InputController : BaseController
    {
        private InputModel m_InputModel;

        public InputController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_InputModel = Dispatcher.GetModel<InputModel>();

            UpdateModel updateModel = Dispatcher.GetModel<UpdateModel>();
            updateModel.OnUpdate += Update;
        }

        public void Update(float deltaTime)
        {
            if (InputDetected() && !IsPointerOverUI())
            {
                m_InputModel.Touch(UnityEngine.Input.mousePosition);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                m_InputModel.KeyDown(KeyCode.Space);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.V))
            {
                m_InputModel.KeyDown(KeyCode.V);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.D))
            {
                m_InputModel.KeyDown(KeyCode.D);
            }
        }

        private bool InputDetected()
        {
#if !UNITY_EDITOR
            return UnityEngine.Input.touchCount > 0 && UnityEngine.Input.touches[0].phase == TouchPhase.Began;
#endif

            return UnityEngine.Input.GetMouseButtonDown(0);
        }

        private bool IsPointerOverUI()
        {
#if !UNITY_EDITOR
            return EventSystem.current.IsPointerOverGameObject(UnityEngine.Input.touches[0].fingerId); 
#endif
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
