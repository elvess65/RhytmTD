using RhytmTD.Battle.Entities.Models;
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
        private CameraModel m_CameraModel;

        public InputController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_InputModel = Dispatcher.GetModel<InputModel>();
            m_CameraModel = Dispatcher.GetModel<CameraModel>();

            UpdateModel updateModel = Dispatcher.GetModel<UpdateModel>();
            updateModel.OnUpdate += Update;
        }

        public void Update(float deltaTime)
        {
            if (InputDetected() && !IsPointerOverUI())
            {
                m_InputModel.Touch(UnityEngine.Input.mousePosition);

                Ray ray = m_CameraModel.MainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    m_InputModel.LastTouchHitPoint = hit.point;
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                m_InputModel.KeyDown(KeyCode.Space);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.V))
            {
                m_InputModel.KeyDown(KeyCode.V);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Z))
            {
                m_InputModel.KeyDown(KeyCode.Z);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.D))
            {
                m_InputModel.KeyDown(KeyCode.D);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.P))
            {
                m_InputModel.KeyDown(KeyCode.P);
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
