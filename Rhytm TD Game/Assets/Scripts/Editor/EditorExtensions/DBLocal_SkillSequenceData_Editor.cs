using RhytmTD.Data.DataBaseLocal;
using UnityEditor;
using UnityEngine;
using static CoreFramework.EnumsCollection;
using static RhytmTD.Data.DataBaseLocal.DBLocal_SkillSequenceData;

namespace RhytmTD.Editor.EditorExtensions
{
    [CustomEditor(typeof(DBLocal_SkillSequenceData))]
    public class DBLocal_SkillSequenceData_Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawBody();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        void DrawBody()
        {
            DBLocal_SkillSequenceData castedTarget = (DBLocal_SkillSequenceData)target;

            using (new GUILayout.VerticalScope("box"))
            {
                DrawTitle(castedTarget);
                GUILayout.Space(5);

                DrawButtons(castedTarget);
                GUILayout.Space(5);

                DrawItems(castedTarget);
            }

            GUILayout.Space(5);
        }

        void DrawTitle(DBLocal_SkillSequenceData castedTarget)
        {
            string title = "Sequence";

            if (castedTarget.SkillSequencePatterns == null || castedTarget.SkillSequencePatterns.Count == 0)
            {
                title += " (No items)";
            }

            GUILayout.Label(title);
        }

        void DrawButtons(DBLocal_SkillSequenceData castedTarget)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("+"))
                {
                    castedTarget.SkillSequencePatterns.Add(new DBLocal_SkillSequenceData.SequencePatternData());
                }

                using (new EditorGUI.DisabledScope(castedTarget.SkillSequencePatterns == null || castedTarget.SkillSequencePatterns.Count == 0))
                {
                    if (GUILayout.Button("-"))
                    {
                        castedTarget.SkillSequencePatterns.RemoveAt(castedTarget.SkillSequencePatterns.Count - 1);
                    }
                }
            }
        }

        void DrawItems(DBLocal_SkillSequenceData castedTarget)
        {
            for (int i = 0; i < castedTarget.SkillSequencePatterns.Count; i++)
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    SequencePatternData patternData = castedTarget.SkillSequencePatterns[i];

                    patternData.ID = (SkillSequencePatternID)EditorGUILayout.EnumPopup(string.Empty, patternData.ID);

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        for (int j = 0; j < patternData.Pattern.Count; j++)
                        {
                            patternData.Pattern[j] = EditorGUILayout.Toggle(patternData.Pattern[j]);
                        }

                        if (GUILayout.Button("+"))
                        {
                            patternData.Pattern.Add(false);
                        }

                        using (new EditorGUI.DisabledScope(patternData.Pattern == null || patternData.Pattern.Count <= 2))
                        {
                            if (GUILayout.Button("-"))
                            {
                                patternData.Pattern.RemoveAt(patternData.Pattern.Count - 1);
                            }
                        }
                    }
                }

                if (i < castedTarget.SkillSequencePatterns.Count - 1)
                    EditorGUILayout.Space(10);
            }
        }
    }
}
