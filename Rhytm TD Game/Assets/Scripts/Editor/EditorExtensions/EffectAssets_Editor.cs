using RhytmTD.Assets.Battle;
using RhytmTD.Battle.Entities.Views;
using UnityEditor;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Editor.EditorExtensions
{
    [CustomEditor(typeof(EffectAssets))]
    public class EffectPrefabAssets_Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawEffectEntityViews();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        void DrawEffectEntityViews()
        {
            EffectAssets castedTarget = (EffectAssets)target;

            using (new GUILayout.VerticalScope("box"))
            {
                DrawEffectEntityViewTitle(castedTarget);
                GUILayout.Space(5);

                DrawEffectEntityViewButtons(castedTarget);
                GUILayout.Space(5);

                DrawEffectEntityViewItems(castedTarget);
            }

            GUILayout.Space(5);
        }

        void DrawEffectEntityViewTitle(EffectAssets castedTarget)
        {
            string title = "EffectViews";

            if (castedTarget.EffectEntityViewPrefabs == null || castedTarget.EffectEntityViewPrefabs.Count == 0)
            {
                title += " (No views)";
            }

            GUILayout.Label(title);
        }

        void DrawEffectEntityViewButtons(EffectAssets castedTarget)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("+"))
                {
                    castedTarget.EffectEntityViewPrefabs.Add(new EffectAssets.EffectEntityViewPrefabData());
                }

                using (new EditorGUI.DisabledScope(castedTarget.EffectEntityViewPrefabs == null || castedTarget.EffectEntityViewPrefabs.Count == 0))
                {
                    if (GUILayout.Button("-"))
                    {
                        castedTarget.EffectEntityViewPrefabs.RemoveAt(castedTarget.EffectEntityViewPrefabs.Count - 1);
                    }
                }
            }
        }

        void DrawEffectEntityViewItems(EffectAssets castedTarget)
        {
            for (int i = 0; i < castedTarget.EffectEntityViewPrefabs.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    castedTarget.EffectEntityViewPrefabs[i].ID = (BattlEffectID)EditorGUILayout.EnumPopup(string.Empty, castedTarget.EffectEntityViewPrefabs[i].ID);
                    castedTarget.EffectEntityViewPrefabs[i].Prefab = (BattleEntityView)EditorGUILayout.ObjectField(castedTarget.EffectEntityViewPrefabs[i].Prefab, typeof(BattleEntityView), false);
                }
            }
        }
    }
}
