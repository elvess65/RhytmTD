using RhytmTD.Data.DataBase.Simulation;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RhytmTD.Editor.EditorExtensions
{
    [CustomEditor(typeof(LevelFactory))]
    public class LevelFactory_Editor : UnityEditor.Editor
    {
        private bool m_IsWavesFoldedOut = false;
        private List<bool> m_FoldedOutWaves;

        public override void OnInspectorGUI()
        {
            DrawLevel();
        }

        void OnEnable()
        {
            LevelFactory castedTarget = (LevelFactory)target;

            m_FoldedOutWaves = new List<bool>();
            for (int i = 0; i < castedTarget.Waves.Count; i++)
            {
                m_FoldedOutWaves.Add(false);
            }
        }


        void DrawLevel()
        {
            LevelFactory castedTarget = (LevelFactory)target;

            #region Properties

            castedTarget.TestData1 = EditorGUILayout.IntField($"TestValue 1", castedTarget.TestData1);
            castedTarget.TestData2 = EditorGUILayout.IntField($"TestValue 2", castedTarget.TestData2);
            castedTarget.TestData3 = EditorGUILayout.IntField($"TestValue 3", castedTarget.TestData3);

            castedTarget.Curve = EditorGUILayout.CurveField("TestCurve", castedTarget.Curve);

            EditorGUILayout.Space();

            #endregion

            #region Waves

            m_IsWavesFoldedOut = EditorGUILayout.Foldout(m_IsWavesFoldedOut, $"Wave Details", true);
            if (m_IsWavesFoldedOut)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    #region Title and AddWaveButton

                    Color initColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.green;

                    EditorGUILayout.BeginHorizontal("box");
                    {
                        GUI.backgroundColor = initColor;

                        //Title
                        EditorGUILayout.LabelField($"Total Waves: {castedTarget.Waves.Count}");

                        //Add button
                        if (GUILayout.Button("Add Wave"))
                        {
                            AddWave(castedTarget);
                            return;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    #endregion

                    #region Wave Details

                    for (int i = 0; i < castedTarget.Waves.Count; i++)
                    {
                        DrawWave(castedTarget.Waves[i], i);
                    }

                    #endregion
                }
                EditorGUILayout.EndVertical();
            }

            #endregion
        }

        void DrawWave(LevelFactory.Wave wave, int waveIndex)
        {
            Color initColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;

            EditorGUILayout.BeginVertical("box");
            {
                GUI.backgroundColor = initColor;

                #region Title and RemoveButton

                EditorGUILayout.BeginHorizontal();
                {
                    //Title
                    EditorGUILayout.LabelField($"Wave [{waveIndex}]");

                    //Remove Button
                    if (GUILayout.Button("X"))
                    {
                        RemoveWave((LevelFactory)target, waveIndex);
                        return;
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                #endregion

                #region Properies

                DrawWaveProperties(wave);

                #endregion

                #region Chunks

                      m_FoldedOutWaves[waveIndex] = EditorGUILayout.Foldout(m_FoldedOutWaves[waveIndex], "Chunk Details", true);
                if (m_FoldedOutWaves[waveIndex])
                {
                    #region Title and AddButton

                    initColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.blue;
                    EditorGUILayout.BeginHorizontal("box");
                    {
                        GUI.backgroundColor = initColor;

                        //Title
                        EditorGUILayout.LabelField($"Total Chunks: {wave.Chunks.Count}");

                        //Add button
                        if (GUILayout.Button("Add Chunk"))
                        {
                            AddChunk(wave);
                            return;
                        }

                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    #endregion

                    #region Chunk Details

                    for (int i = 0; i < wave.Chunks.Count; i++)
                    {
                        DrawChunk(wave.Chunks[i], wave, i);
                    }

                    #endregion
                }

                #endregion
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        void DrawChunk(LevelFactory.Chunk chunk, LevelFactory.Wave parentWave, int chunkIndex)
        {
            Color initColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.blue;
            EditorGUILayout.BeginVertical("box");
            {
                GUI.backgroundColor = initColor;

                #region Title and RemoveButton

                EditorGUILayout.BeginHorizontal();
                {
                    //Title
                    EditorGUILayout.LabelField($"Chunk [{chunkIndex}]");

                    //Remove button
                    if (GUILayout.Button("X"))
                    {
                        RemoveChunk(parentWave, chunkIndex);
                        return;
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                #endregion

                #region Properties

                DrawChunkProperies(chunk, parentWave);

                #endregion
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }


        void DrawWaveProperties(LevelFactory.Wave wave)
        {
            //Enemies Amount
            wave.EnemiesAmount = EditorGUILayout.IntField("EnemiesAmount:", wave.EnemiesAmount);
            EditorGUILayout.Space();

            //Damage
            DrawMinMax("MinDamage:", ref wave.MinDamage, "MaxDamage", ref wave.MaxDamage);
            EditorGUILayout.Space();

            //HP
            DrawMinMax("MinHP:", ref wave.MinHP, "MaxHP", ref wave.MaxHP);
            EditorGUILayout.Space();
        }

        void DrawChunkProperies(LevelFactory.Chunk chunk, LevelFactory.Wave parentWave)
        {
            //Enemies Amount
            GUI.enabled = chunk.OverrideAmount;
            chunk.EnemiesAmount = EditorGUILayout.IntField("EnemiesAmount:", chunk.EnemiesAmount);

            GUI.enabled = true;
            EditorGUI.BeginChangeCheck();

            chunk.OverrideAmount = EditorGUILayout.Toggle("Override", chunk.OverrideAmount);
            if (EditorGUI.EndChangeCheck() && !chunk.OverrideAmount)
            {
                chunk.EnemiesAmount = parentWave.EnemiesAmount;
            }
            EditorGUILayout.Space();

            //Damage
            GUI.enabled = chunk.OverrideDamage;
            DrawMinMax("MinDamage:", ref chunk.MinDamage, "MaxDamage", ref chunk.MaxDamage);

            GUI.enabled = true;
            EditorGUI.BeginChangeCheck();

            chunk.OverrideDamage = EditorGUILayout.Toggle("Override", chunk.OverrideDamage);
            if (EditorGUI.EndChangeCheck() &&!chunk.OverrideDamage)
            {
                chunk.MinDamage = parentWave.MinDamage;
                chunk.MaxDamage = parentWave.MaxDamage;
            }
            EditorGUILayout.Space();

            //HP
            GUI.enabled = chunk.OverrideHP;
            DrawMinMax("MinHP:", ref chunk.MinHP, "MaxHP", ref chunk.MaxHP);

            GUI.enabled = true;
            EditorGUI.BeginChangeCheck();

            chunk.OverrideHP = EditorGUILayout.Toggle("Override", chunk.OverrideHP);
            if (EditorGUI.EndChangeCheck() && !chunk.OverrideHP)
            {
                chunk.MinHP = parentWave.MinHP;
                chunk.MaxHP = parentWave.MaxHP;
            }
            EditorGUILayout.Space();
        }

        void DrawMinMax(string minName, ref int minValue, string maxName, ref int maxValue, float preferedWidth = 100)
        {
            EditorGUILayout.BeginHorizontal();
            {
                float initLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = preferedWidth;

                minValue = EditorGUILayout.IntField(minName, minValue);
                maxValue = EditorGUILayout.IntField(maxName, maxValue);

                EditorGUIUtility.labelWidth = initLabelWidth;
            }
            EditorGUILayout.EndHorizontal();
        }


        void AddWave(LevelFactory levelFactory)
        {
            levelFactory.Waves.Add(new LevelFactory.Wave());
            m_FoldedOutWaves.Add(true);
        }

        void RemoveWave(LevelFactory levelFactory, int waveIndex)
        {
            levelFactory.Waves.RemoveAt(waveIndex);
            m_FoldedOutWaves.RemoveAt(waveIndex);
        }

        void AddChunk(LevelFactory.Wave wave)
        {
            wave.Chunks.Add(new LevelFactory.Chunk());
        }

        void RemoveChunk(LevelFactory.Wave wave, int chunkIndex)
        {
            wave.Chunks.RemoveAt(chunkIndex);
        }
    }
}
