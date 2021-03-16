using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnviromentCell))]
public class EnviromentCell_Editor : Editor
{
    private Material mat;

    public override void OnInspectorGUI()
    {
        EnviromentCell castedTarget = (EnviromentCell)target;

        if (GUILayout.Button("Cache renderers"))
        {
            if (castedTarget.InitialMaterials == null)
                castedTarget.InitialMaterials = new Dictionary<int, Material>();

            castedTarget.GetComponentsInChildren(castedTarget.Renderers);
            foreach(MeshRenderer renderer in castedTarget.Renderers)
            {
                castedTarget.InitialMaterials[renderer.GetInstanceID()] = renderer.sharedMaterial;
            }
        }

        if (GUILayout.Button("Restore materials"))
        {
            foreach (MeshRenderer renderer in castedTarget.Renderers)
            {
                renderer.sharedMaterial = castedTarget.InitialMaterials[renderer.GetInstanceID()];
            }
        }

        if (GUILayout.Button("Set material"))
        {
            foreach (MeshRenderer renderer in castedTarget.Renderers)
            {
                renderer.sharedMaterial = mat;
            }
        }

        mat = (Material)EditorGUILayout.ObjectField(mat, typeof(Material), false);

        base.OnInspectorGUI();
    }
}
