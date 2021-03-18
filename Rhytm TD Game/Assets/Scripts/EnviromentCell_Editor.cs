using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static EnviromentCell;

[CustomEditor(typeof(EnviromentCell))]
public class EnviromentCell_Editor : Editor
{
    private bool m_CollidersEnable = false;
    private bool m_CheckedOutsFolded = false;
    private Dictionary<EnviromentTypes, bool> m_FoldedOuts = new Dictionary<EnviromentTypes, bool>();
    private List<(EnviromentTypes, Material)> m_EnviromentTypesMaterialsSubstitute = new List<(EnviromentTypes, Material)>();


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnviromentCell castedTarget = (EnviromentCell)target;

        DrawCachedRenderers(castedTarget);
        DrawMaterialSubstitute(castedTarget);
        DrawUpdateColliders(castedTarget);

        if (GUI.changed)
            Validate();

        if (GUILayout.Button("Test"))
        {
            Debug.Log(castedTarget.NearEdge.position);
        }
    }

    private void OnEnable()
    {
        EnviromentCell castedTarget = (EnviromentCell)target;

        CacheRenderersForType(EnviromentTypes.All, castedTarget);
        ConvertEnviromentTypesToList();
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
            m_CheckedOutsFolded = EditorGUILayout.Foldout(m_CheckedOutsFolded, "Enviroment Types:", true);
            if (m_CheckedOutsFolded)
            {
                for (int i = 0; i < m_EnviromentTypesMaterialsSubstitute.Count; i++)
                {
                    (EnviromentTypes, Material) item = m_EnviromentTypesMaterialsSubstitute[i];

                    item.Item2 = (Material)EditorGUILayout.ObjectField(item.Item1.ToString(), item.Item2, typeof(Material), false);
                    m_EnviromentTypesMaterialsSubstitute[i] = item;
                }
            }

            if (GUILayout.Button("Substitute Materials"))
            {
                for (int i = 0; i < m_EnviromentTypesMaterialsSubstitute.Count; i++)
                {
                    SubstituteMaterial(m_EnviromentTypesMaterialsSubstitute[i].Item1,
                                       m_EnviromentTypesMaterialsSubstitute[i].Item2,
                                       castedTarget);
                }
            }

            if (GUILayout.Button("Clear Materials"))
            {
                for (int i = 0; i < m_EnviromentTypesMaterialsSubstitute.Count; i++)
                {
                    (EnviromentTypes, Material) item = m_EnviromentTypesMaterialsSubstitute[i];
                    item.Item2 = null;
                    m_EnviromentTypesMaterialsSubstitute[i] = item;
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


    private void Validate()
    {
        if (IsAllTypeInitialized())
        {
            for (int i = 1; i < m_EnviromentTypesMaterialsSubstitute.Count; i++)
            {
                (EnviromentTypes, Material) item = m_EnviromentTypesMaterialsSubstitute[i];

                item.Item2 = m_EnviromentTypesMaterialsSubstitute[0].Item2;
                m_EnviromentTypesMaterialsSubstitute[i] = item;
            }

            (EnviromentTypes, Material) allItem = m_EnviromentTypesMaterialsSubstitute[0];
            allItem.Item2 = null;
            m_EnviromentTypesMaterialsSubstitute[0] = allItem;
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


    private void SubstituteMaterial(EnviromentTypes type, Material material, EnviromentCell castedTarget)
    {
        if (material != null && castedTarget.Renderers.TryGetValue(type, out List<MeshRenderer> renderers))
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].sharedMaterial = material;
            }
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

    private void ConvertEnviromentTypesToList()
    {
        for (int i = 0; i < (int)EnviromentTypes.Max; i++)
        {
            m_EnviromentTypesMaterialsSubstitute.Add(((EnviromentTypes)i, null));
        }
    }

    private bool IsAllTypeInitialized()
    {
        for (int i = 0; i < m_EnviromentTypesMaterialsSubstitute.Count; i++)
        {
            if (m_EnviromentTypesMaterialsSubstitute[i].Item1 == EnviromentTypes.All &&
                m_EnviromentTypesMaterialsSubstitute[i].Item2 != null)
                return true;
        }

        return false;
    }
}
