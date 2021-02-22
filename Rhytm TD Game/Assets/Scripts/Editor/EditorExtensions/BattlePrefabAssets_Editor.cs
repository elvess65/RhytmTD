using RhytmTD.Assets.Battle;
using RhytmTD.Battle.Entities.Views;
using UnityEditor;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Editor.EditorExtensions
{
    [CustomEditor(typeof(BattlePrefabAssets))]
    public class BattlePrefabAssets_Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawPlayerView();
            DrawEffectEntityViews();

            if (GUI.changed)
            {
                ValidateData();
                EditorUtility.SetDirty(target);
            }
        }


        void DrawPlayerView()
        {
            BattlePrefabAssets castedTarget = (BattlePrefabAssets)target;

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("PlayerView");
                castedTarget.PlayerPrefab = (PlayerView)EditorGUILayout.ObjectField(castedTarget.PlayerPrefab, typeof(PlayerView), false);
            }

            GUILayout.Space(5);
        }


        void DrawEffectEntityViews()
        {
            BattlePrefabAssets castedTarget = (BattlePrefabAssets)target;

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

        void DrawEffectEntityViewTitle(BattlePrefabAssets castedTarget)
        {
            string title = "EffectViews";

            if (castedTarget.EffectEntityViewPrefabs == null || castedTarget.EffectEntityViewPrefabs.Count == 0)
            {
                title += " (No views)";
            }

            GUILayout.Label(title);
        }

        void DrawEffectEntityViewButtons(BattlePrefabAssets castedTarget)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("+"))
                {
                    castedTarget.EffectEntityViewPrefabs.Add(new BattlePrefabAssets.EffectEntityViewPrefabData());
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

        void DrawEffectEntityViewItems(BattlePrefabAssets castedTarget)
        {
            for (int i = 0; i < castedTarget.EffectEntityViewPrefabs.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    castedTarget.EffectEntityViewPrefabs[i].ID = (BattleEntityEffectID)EditorGUILayout.EnumPopup(string.Empty, castedTarget.EffectEntityViewPrefabs[i].ID);
                    castedTarget.EffectEntityViewPrefabs[i].Prefab = (BattleEntityView)EditorGUILayout.ObjectField(castedTarget.EffectEntityViewPrefabs[i].Prefab, typeof(BattleEntityView), false);
                }
            }
        }


        void ValidateData()
        {
            Debug.Log("Validate");

            BattlePrefabAssets castedTarget = (BattlePrefabAssets)target;
            ValidateEffectEntityViews(castedTarget);
        }

        void ValidateEffectEntityViews(BattlePrefabAssets castedTarget)
        {
            castedTarget.Initialize();
        }
    }
}
