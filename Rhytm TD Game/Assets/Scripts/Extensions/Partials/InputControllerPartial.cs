using UnityEngine;

namespace CoreFramework.Input
{
    public partial class InputController
    {
        partial void HandleSpecialInput()
        {
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
    }
}
