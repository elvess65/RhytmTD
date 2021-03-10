using RhytmTD.Assets.Battle;
using RhytmTD.Battle.Entities.Views;
using UnityEditor;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Editor.EditorExtensions
{
    [CustomEditor(typeof(UISpriteAssets))]
    public class UISpritesAssets_Editor : UnityEditor.Editor
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
            UISpriteAssets castedTarget = (UISpriteAssets)target;

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

        void DrawEffectEntityViewTitle(UISpriteAssets castedTarget)
        {
            string title = "UISprites";

            if (castedTarget.SkillIconSprites == null || castedTarget.SkillIconSprites.Count == 0)
            {
                title += " (No sprites)";
            }

            GUILayout.Label(title);
        }

        void DrawEffectEntityViewButtons(UISpriteAssets castedTarget)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("+"))
                {
                    castedTarget.SkillIconSprites.Add(new UISpriteAssets.SkillIconSpriteData());
                }

                using (new EditorGUI.DisabledScope(castedTarget.SkillIconSprites == null || castedTarget.SkillIconSprites.Count == 0))
                {
                    if (GUILayout.Button("-"))
                    {
                        castedTarget.SkillIconSprites.RemoveAt(castedTarget.SkillIconSprites.Count - 1);
                    }
                }
            }
        }

        void DrawEffectEntityViewItems(UISpriteAssets castedTarget)
        {
            for (int i = 0; i < castedTarget.SkillIconSprites.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    castedTarget.SkillIconSprites[i].ID = (SkillTypeID)EditorGUILayout.EnumPopup(string.Empty, castedTarget.SkillIconSprites[i].ID);
                    castedTarget.SkillIconSprites[i].Sprite = (Sprite)EditorGUILayout.ObjectField(castedTarget.SkillIconSprites[i].Sprite, typeof(Sprite), false);
                    castedTarget.SkillIconSprites[i].Color = EditorGUILayout.ColorField(castedTarget.SkillIconSprites[i].Color);
                }
            }
        }
    }
}
