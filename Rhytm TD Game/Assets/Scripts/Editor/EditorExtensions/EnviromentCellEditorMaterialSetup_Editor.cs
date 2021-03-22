using RhytmTD.Data.Editor;
using UnityEditor;
using UnityEngine;
using static CoreFramework.EnumsCollection;
using static RhytmTD.Data.Editor.EnviromentCellMaterialSetup;

namespace RhytmTD.Editor.EditorExtensions
{
    [CustomEditor(typeof(EnviromentCellMaterialSetup))]
    public class EnviromentCellEditorMaterialSetup_Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EnviromentCellMaterialSetup castedTarget = (EnviromentCellMaterialSetup)target;

            DrawList(castedTarget);

            if (GUI.changed)
                ValidateMaterials(castedTarget);
        }

        private void OnEnable()
        {
            ValidateList((EnviromentCellMaterialSetup)target);
        }


        private void DrawList(EnviromentCellMaterialSetup castedTarget)
        {
            for (int i = 0; i < castedTarget.EnviromentCellMaterials.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope("box"))
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        castedTarget.EnviromentCellMaterials[i].Type = (EnviromentTypes)EditorGUILayout.EnumPopup(string.Empty, castedTarget.EnviromentCellMaterials[i].Type);
                    }

                    castedTarget.EnviromentCellMaterials[i].Material = (Material)EditorGUILayout.ObjectField(castedTarget.EnviromentCellMaterials[i].Material, typeof(Material), false);
                }
            }
        }


        private void ValidateList(EnviromentCellMaterialSetup castedTarget)
        {
            int totalSize = (int)EnviromentTypes.Max;
            if (castedTarget.EnviromentCellMaterials.Count < totalSize)
            {
                for (int i = 0; i < totalSize; i++)
                {
                    EnviromentTypes type = (EnviromentTypes)i;
                    if (!ContainsType(type, castedTarget))
                    {
                        castedTarget.EnviromentCellMaterials.Add(new EnviromentCellMaterialSetupData() { Type = type });
                    }
                }
            }
        }

        private void ValidateMaterials(EnviromentCellMaterialSetup castedTarget)
        {
            EnviromentCellMaterialSetupData allSetup = GetSetupOfType(EnviromentTypes.All, castedTarget);
            if (allSetup != null && allSetup.Material != null)
            {
                for (int i = 0; i < castedTarget.EnviromentCellMaterials.Count; i++)
                {
                    castedTarget.EnviromentCellMaterials[i].Material = allSetup.Material;
                }
            }

            allSetup.Material = null;
        }


        private EnviromentCellMaterialSetupData GetSetupOfType(EnviromentTypes type, EnviromentCellMaterialSetup castedTarget)
        {
            foreach (EnviromentCellMaterialSetupData setup in castedTarget.EnviromentCellMaterials)
            {
                if (setup.Type == type)
                    return setup;
            }

            return null;
        }

        private bool ContainsType(EnviromentTypes type, EnviromentCellMaterialSetup castedTarget)
        {
            foreach (EnviromentCellMaterialSetupData setup in castedTarget.EnviromentCellMaterials)
            {
                if (setup.Type == type)
                    return true;
            }

            return false;
        }
    }
}
