using RhytmTD.Battle.Entities.Views;
using RhytmTD.Data.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Editor.EditorExtensions
{
    [CustomEditor(typeof(EnviromentCellView))]
    public class EnviromentCell_Editor : UnityEditor.Editor
    {
        private EnviromentCellMaterialSetup m_MaterialSetup;

        private bool m_CollidersEnable = false;

        private Dictionary<EnviromentTypes, bool> m_FoldedOutStateRenderers = new Dictionary<EnviromentTypes, bool>();


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EnviromentCellView castedTarget = (EnviromentCellView)target;

            DrawCachedRenderers(castedTarget);
            DrawMaterialSubstitute(castedTarget);
            DrawUpdateColliders(castedTarget);
        }

        private void OnEnable()
        {
            CacheRenderersForType(EnviromentTypes.All, (EnviromentCellView)target);
        }


        private void DrawCachedRenderers(EnviromentCellView castedTarget)
        {
            EditorGUILayout.Space(10);

            using (new EditorGUILayout.VerticalScope("box"))
            {
                if (castedTarget.Renderers.Count == 0)
                {
                    EditorGUILayout.LabelField("No renderers");
                    return;
                }

                if (!m_FoldedOutStateRenderers.ContainsKey(EnviromentTypes.All))
                    m_FoldedOutStateRenderers[EnviromentTypes.All] = false;

                m_FoldedOutStateRenderers[EnviromentTypes.All] = EditorGUILayout.Foldout(m_FoldedOutStateRenderers[EnviromentTypes.All], $"Enviroment Groups: {castedTarget.Renderers.Count}", true);
                if (m_FoldedOutStateRenderers[EnviromentTypes.All])
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        foreach (EnviromentTypes type in castedTarget.Renderers.Keys)
                        {
                            using (new EditorGUILayout.VerticalScope("box"))
                            {
                                EnviromentTypes localType = (EnviromentTypes)EditorGUILayout.EnumPopup(string.Empty, type);
                                List<MeshRenderer> renderers = castedTarget.Renderers[type];

                                if (!m_FoldedOutStateRenderers.ContainsKey(localType))
                                    continue;

                                m_FoldedOutStateRenderers[localType] = EditorGUILayout.Foldout(m_FoldedOutStateRenderers[localType], $"Renderers {renderers.Count}", true);
                                if (m_FoldedOutStateRenderers[localType])
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

        private void DrawMaterialSubstitute(EnviromentCellView castedTarget)
        {
            if (castedTarget.Renderers.Count == 0)
                return;

            EditorGUILayout.Space(10);

            using (new EditorGUILayout.HorizontalScope("box"))
            {
                EditorGUILayout.LabelField("Setup");
                m_MaterialSetup = (EnviromentCellMaterialSetup)EditorGUILayout.ObjectField(m_MaterialSetup, typeof(EnviromentCellMaterialSetup), false);

            }

            using (new EditorGUI.DisabledScope(m_MaterialSetup == null))
            {
                if (GUILayout.Button("Substitute Materials"))
                {
                    for (int i = 0; i < m_MaterialSetup.EnviromentCellMaterials.Count; i++)
                    {
                        SubstituteMaterial(m_MaterialSetup.EnviromentCellMaterials[i].Type,
                                            m_MaterialSetup.EnviromentCellMaterials[i].Material,
                                            castedTarget);
                    }
                }
            }
        }

        private void DrawUpdateColliders(EnviromentCellView castedTarget)
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

        private void CacheRenderersForType(EnviromentTypes type, EnviromentCellView castedTarget)
        {
            switch (type)
            {
                case EnviromentTypes.All:
                    {
                        ForeachGroup(CacheRenderersForType, castedTarget);

                        break;
                    }
                default:
                    {
                        m_FoldedOutStateRenderers[EnviromentTypes.All] = false;

                        if (FindChildWithName(type.ToString(), castedTarget.transform, out Transform parent))
                        {
                            castedTarget.Renderers[type] = new List<MeshRenderer>(parent.GetComponentsInChildren<MeshRenderer>());
                            m_FoldedOutStateRenderers[type] = false;
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


        private void SubstituteMaterial(EnviromentTypes type, Material material, EnviromentCellView castedTarget)
        {
            if (material != null && castedTarget.Renderers.TryGetValue(type, out List<MeshRenderer> renderers))
            {
                for (int i = 0; i < renderers.Count; i++)
                {
                    renderers[i].sharedMaterial = material;
                }
            }
        }

        private void UpdateColliders(EnviromentCellView castedTarget)
        {
            Collider[] colliders = castedTarget.transform.GetComponentsInChildren<Collider>(true);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (!colliders[i].transform.parent.name.Equals(EnviromentTypes.Ground.ToString()))
                    colliders[i].enabled = m_CollidersEnable;
            }

            Debug.Log($"Successfully updated {colliders.Length} colliders");
        }


        private void ForeachGroup(System.Action<EnviromentTypes, EnviromentCellView> action, EnviromentCellView castedTarget)
        {
            int from = (int)EnviromentTypes.All + 1;
            int to = (int)EnviromentTypes.Max;

            for (int i = from; i < to; i++)
                action((EnviromentTypes)i, castedTarget);
        }
    }
}
