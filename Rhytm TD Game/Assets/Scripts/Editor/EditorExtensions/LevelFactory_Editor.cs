﻿using RhytmTD.Data.DataBase.Simulation;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static RhytmTD.Data.DataBase.Simulation.LevelFactory;

namespace RhytmTD.Editor.EditorExtensions
{
    [CustomEditor(typeof(LevelFactory))]
    public class LevelFactory_Editor : UnityEditor.Editor
    {
        private class WaveEditorData
        {
            public enum OverrideTypes { None, Part, Full }

            public bool IsFoldedOut = false;
            public OverrideTypes AmountOverride = OverrideTypes.None;
        }

        private bool m_IsWavesFoldedOut = false;
        private List<WaveEditorData> m_WavesEditorData;

        public override void OnInspectorGUI()
        {
            DrawLevel();

            if (GUI.changed)
                ValidateData();
        }

        void OnEnable()
        {
            LevelFactory castedTarget = (LevelFactory)target;

            m_WavesEditorData = new List<WaveEditorData>();
            for (int i = 0; i < castedTarget.Waves.Count; i++)
            {
                m_WavesEditorData.Add(new WaveEditorData());
            }
        }

        void ValidateData()
        {
            LevelFactory castedTarget = (LevelFactory)target;

            int waveIndex = 0;
            foreach(Wave wave in castedTarget.Waves)
            {
                int overrideAmountCounter = 0;
                int overrideDamageCounter = 0;
                int overrideHPCounter = 0;
                foreach(Chunk chunk in wave.Chunks)
                {
                    if (!chunk.OverrideAmount)
                    {
                        chunk.EnemiesAmount = wave.EnemiesAmount;
                    }
                    else
                    {
                        overrideAmountCounter++;
                    }


                    if (!chunk.OverrideDamage)
                    {
                        chunk.MinDamage = wave.MinDamage;
                        chunk.MaxDamage = wave.MaxDamage;
                    }
                    else
                    {
                        overrideDamageCounter++;
                    }

                    if (!chunk.OverrideHP)
                    {
                        chunk.MinHP = wave.MinHP;
                        chunk.MaxHP = wave.MaxHP;
                    }
                    else
                    {
                        overrideHPCounter++;
                    }
                }

                if (overrideAmountCounter == wave.Chunks.Count)
                {
                    m_WavesEditorData[waveIndex].AmountOverride = WaveEditorData.OverrideTypes.Full;
                }
                else if (overrideAmountCounter > 0)
                {
                    m_WavesEditorData[waveIndex].AmountOverride = WaveEditorData.OverrideTypes.Part;
                }
                else
                {
                    m_WavesEditorData[waveIndex].AmountOverride = WaveEditorData.OverrideTypes.None;
                }

                waveIndex++;
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

                DrawWaveProperties(wave, waveIndex);

                #endregion

                #region Chunks

                m_WavesEditorData[waveIndex].IsFoldedOut = EditorGUILayout.Foldout(m_WavesEditorData[waveIndex].IsFoldedOut, "Chunk Details", true);
                if (m_WavesEditorData[waveIndex].IsFoldedOut)
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

                DrawChunkProperies(chunk, parentWave, chunkIndex);

                #endregion
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }


        void DrawWaveProperties(LevelFactory.Wave wave, int waveIndex)
        {
            //Enemies Amount

            GUI.enabled = m_WavesEditorData[waveIndex].AmountOverride != WaveEditorData.OverrideTypes.Full;

            Color initColor = EditorStyles.label.normal.textColor;
            if (m_WavesEditorData[waveIndex].AmountOverride == WaveEditorData.OverrideTypes.Part)
                EditorStyles.label.normal.textColor = Color.yellow;

            wave.EnemiesAmount = EditorGUILayout.IntField("EnemiesAmount:", wave.EnemiesAmount);

            EditorStyles.label.normal.textColor = initColor;
            GUI.enabled = true;

            EditorGUILayout.Space();

            //Damage
            DrawMinMax("MinDamage:", ref wave.MinDamage, "MaxDamage", ref wave.MaxDamage);
            EditorGUILayout.Space();

            //HP
            DrawMinMax("MinHP:", ref wave.MinHP, "MaxHP", ref wave.MaxHP);
            EditorGUILayout.Space();
        }

        void DrawChunkProperies(LevelFactory.Chunk chunk, LevelFactory.Wave parentWave, int chunkIndex)
        {
            //Enemies Amount
            GUI.enabled = chunk.OverrideAmount;
            chunk.EnemiesAmount = EditorGUILayout.IntField("EnemiesAmount:", chunk.EnemiesAmount);
            GUI.enabled = true;

            chunk.OverrideAmount = EditorGUILayout.Toggle("Override", chunk.OverrideAmount);

            EditorGUILayout.Space();

            //Damage
            GUI.enabled = chunk.OverrideDamage;
            DrawMinMax("MinDamage:", ref chunk.MinDamage, "MaxDamage", ref chunk.MaxDamage);
            GUI.enabled = true;

            chunk.OverrideDamage = EditorGUILayout.Toggle("Override", chunk.OverrideDamage);

            EditorGUILayout.Space();

            //HP
            GUI.enabled = chunk.OverrideHP;
            DrawMinMax("MinHP:", ref chunk.MinHP, "MaxHP", ref chunk.MaxHP);
            GUI.enabled = true;

            chunk.OverrideHP = EditorGUILayout.Toggle("Override", chunk.OverrideHP);

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
            m_WavesEditorData.Add(new WaveEditorData());
        }

        void RemoveWave(LevelFactory levelFactory, int waveIndex)
        {
            levelFactory.Waves.RemoveAt(waveIndex);
            m_WavesEditorData.RemoveAt(waveIndex);
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
