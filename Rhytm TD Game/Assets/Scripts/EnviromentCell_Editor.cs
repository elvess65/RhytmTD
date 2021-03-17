using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static EnviromentCell;

[CustomEditor(typeof(EnviromentCell))]
public class EnviromentCell_Editor : Editor
{
    private bool m_CollidersEnable = false;
    private Material m_SubstituteMaterial;
    private EnviromentTypes m_SelectedEnviromentType = EnviromentTypes.All;
    private Dictionary<EnviromentTypes, bool> m_FoldedOuts = new Dictionary<EnviromentTypes, bool>();


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnviromentCell castedTarget = (EnviromentCell)target;

        DrawEnviromentTypeSelection();
        DrawCachedRenderers(castedTarget);
        DrawMaterialSubstitute(castedTarget);
        DrawUpdateColliders(castedTarget);
    }

    private void OnEnable()
    {
        EnviromentCell castedTarget = (EnviromentCell)target;

        CacheRenderersForType(EnviromentTypes.All, castedTarget);
    }


    private void DrawEnviromentTypeSelection()
    {
        m_SelectedEnviromentType = (EnviromentTypes)EditorGUILayout.EnumPopup("Enviroment Type:", m_SelectedEnviromentType);
    }

    private void DrawCachedRenderers(EnviromentCell castedTarget)
    {
        EditorGUILayout.Space(10);

        using (new EditorGUILayout.VerticalScope("box"))
        {
            if (castedTarget.Renderers.Count == 0)
            {
                EditorGUILayout.LabelField("No renderers");
                return;
            }

            if (!m_FoldedOuts.ContainsKey(EnviromentTypes.All))
                m_FoldedOuts[EnviromentTypes.All] = false;

            m_FoldedOuts[EnviromentTypes.All] = EditorGUILayout.Foldout(m_FoldedOuts[EnviromentTypes.All], $"Enviroment Groups: {castedTarget.Renderers.Count}", true);
            if (m_FoldedOuts[EnviromentTypes.All])
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    foreach (EnviromentTypes type in castedTarget.Renderers.Keys)
                    {
                        using (new EditorGUILayout.VerticalScope("box"))
                        {
                            EnviromentTypes localType = (EnviromentTypes)EditorGUILayout.EnumPopup(string.Empty, type);
                            List<MeshRenderer> renderers = castedTarget.Renderers[type];

                            if (!m_FoldedOuts.ContainsKey(localType))
                                continue;

                            m_FoldedOuts[localType] = EditorGUILayout.Foldout(m_FoldedOuts[localType], $"Renderers {renderers.Count}", true);
                            if (m_FoldedOuts[localType])
                            {
                                for (int i = 0; i < renderers.Count; i++)
                                {
                                    renderers[i] = (MeshRenderer)EditorGUILayout.ObjectField(renderers[i], typeof(MeshRenderer), false);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void DrawMaterialSubstitute(EnviromentCell castedTarget)
    {
        if (castedTarget.Renderers.Count == 0)
            return;

        EditorGUILayout.Space(10);

        using (new EditorGUILayout.VerticalScope("box"))
        {

            m_SubstituteMaterial = (Material)EditorGUILayout.ObjectField(m_SubstituteMaterial, typeof(Material), false);

            using (new EditorGUI.DisabledScope(m_SubstituteMaterial == null))
            {
                if (GUILayout.Button("Substitute Material"))
                {
                    switch (m_SelectedEnviromentType)
                    {
                        case EnviromentTypes.All:
                        {
                            ForeachGroup(SubstituteMaterial, castedTarget);

                            break;
                        }
                        default:
                        {
                            SubstituteMaterial(m_SelectedEnviromentType, castedTarget);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void DrawUpdateColliders(EnviromentCell castedTarget)
    {
        EditorGUILayout.Space(10);

        using (new EditorGUILayout.VerticalScope("box"))
        {
            m_CollidersEnable = EditorGUILayout.Toggle("Enable Colliders:", m_CollidersEnable);

            if (GUILayout.Button("Update colliders"))
            {
                UpdateColliders(castedTarget);
            }
        }
    }


    private void CacheRenderersForType(EnviromentTypes type, EnviromentCell castedTarget)
    {
        switch(type)
        {
            case EnviromentTypes.All:
            {
                ForeachGroup(CacheRenderersForType, castedTarget);

                break;
            }
            default:
            {
                m_FoldedOuts[EnviromentTypes.All] = false;

                if (FindChildWithName(type.ToString(), castedTarget.transform, out Transform parent))
                {
                    castedTarget.Renderers[type] = new List<MeshRenderer>(parent.GetComponentsInChildren<MeshRenderer>());
                    m_FoldedOuts[type] = false;
                }
                break;
            }
        }
    }

    private bool FindChildWithName(string name, Transform source, out Transform result)
    {
        result = null;

        Transform[] children = source.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name.Equals(name))
            {
                result = children[i];
                return true;
            }
        }

        return false;
    }


    private void SubstituteMaterial(EnviromentTypes type, EnviromentCell castedTarget)
    {
        Debug.Log(type);
        List<MeshRenderer> renderers = castedTarget.Renderers[type];
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].sharedMaterial = m_SubstituteMaterial;
        }
    }

    private void UpdateColliders(EnviromentCell castedTarget)
    {
        Collider[] colliders = castedTarget.transform.GetComponentsInChildren<Collider>(true);
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = m_CollidersEnable;

        Debug.Log($"Successfully updated {colliders.Length} colliders");
    }


    private void ForeachGroup(System.Action<EnviromentTypes, EnviromentCell> action, EnviromentCell castedTarget)
    {
        int from = (int)EnviromentTypes.All + 1;
        int to = (int)EnviromentTypes.Max;

        for (int i = from; i < to; i++)
            action((EnviromentTypes)i, castedTarget);
    }
}
