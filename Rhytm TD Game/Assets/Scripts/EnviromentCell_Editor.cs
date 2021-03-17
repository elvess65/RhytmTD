using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static EnviromentCell;

[CustomEditor(typeof(EnviromentCell))]
public class EnviromentCell_Editor : Editor
{
    private Material mat;
    private EnviromentTypes m_SelectedType = EnviromentTypes.All;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        m_SelectedType = (EnviromentTypes)EditorGUILayout.EnumPopup("SelectedEnviromentType:", m_SelectedType);

        EnviromentCell castedTarget = (EnviromentCell)target;

        if (GUILayout.Button("Cache renderers"))
        {
            CacheRenderersForType(m_SelectedType, castedTarget);
        }

        foreach(EnviromentTypes type in castedTarget.Renderers.Keys)
        {

        }
    }

    private void CacheRenderersForType(EnviromentTypes type, EnviromentCell castedTarget)
    {
        switch(type)
        {
            case EnviromentTypes.All:
                int from = (int)EnviromentTypes.All + 1;
                int to = (int)EnviromentTypes.Max;

                for (int i = from; i < to; i++)
                    CacheRenderersForType((EnviromentTypes)i, castedTarget);

                break;
            default:
                {
                    if (FindChildWithName(type.ToString(), castedTarget.transform, out Transform parent))
                    {
                        castedTarget.Renderers[type] = new List<MeshRenderer>(parent.GetComponentsInChildren<MeshRenderer>());
                    }
                    break;
                }
        }
    }

    private bool FindChildWithName(string name, Transform source, out Transform result)
    {
        result = null;
        for (int i = 0; i < source.childCount; i++)
        {
            Transform child = source.GetChild(i);
            if (child.name.Equals(name))
            {
                result = child;
                return true;
            }
        }

        return false;
    }
}
