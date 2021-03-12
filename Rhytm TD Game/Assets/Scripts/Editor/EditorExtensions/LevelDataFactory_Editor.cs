using RhytmTD.Assets.Battle;
using RhytmTD.Data.Factory;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RhytmTD.Editor.EditorExtensions
{
    [CustomEditor(typeof(LevelDataFactory))]
    public class LevelDataFactory_Editor : UnityEditor.Editor
    {
        private class WaveEditorData
        {
            public enum OverrideTypes { None, Part, Full }

            public bool IsFoldedOut = false;
            public OverrideTypes AmountOverride = OverrideTypes.None;
            public OverrideTypes DamageOverride = OverrideTypes.None;
            public OverrideTypes HPOverride = OverrideTypes.None;
        }

        private bool m_IsWavesFoldedOut = true;
        private List<WaveEditorData> m_WavesEditorData;

        public override void OnInspectorGUI()
        {
            DrawLevel();

            if (GUI.changed)
            {
                ValidateLevel();
                EditorUtility.SetDirty(target);
            }
        }

        void OnEnable()
        {
            LevelDataFactory castedTarget = (LevelDataFactory)target;

            m_WavesEditorData = new List<WaveEditorData>();
            for (int i = 0; i < castedTarget.Waves.Count; i++)
            {
                m_WavesEditorData.Add(new WaveEditorData());
            }
        }


        void ValidateLevel()
        {
            LevelDataFactory castedTarget = (LevelDataFactory)target;

            for (int i = 0; i < castedTarget.Waves.Count; i++)
                ValidateWave(castedTarget.Waves[i], i);
        }

        void ValidateWave(LevelDataFactory.WaveDataFactory wave, int waveIndex)
        {
            ValidateMinMax(ref wave.MinDamage, ref wave.MaxDamage);
            ValidateMinMax(ref wave.MinHP, ref wave.MaxHP);

            if (wave.EnemiesAmount <= 0)
                wave.EnemiesAmount = 1;

            int overrideAmountCounter = 0;
            int overrideDamageCounter = 0;
            int overrideHPCounter = 0;
            foreach (LevelDataFactory.ChunkDataFactory chunk in wave.Chunks)
            {
                ValidateChunk(wave, chunk);

                if (chunk.OverrideAmount)
                    overrideAmountCounter++;

                if (chunk.OverrideDamage)
                    overrideDamageCounter++;

                if (chunk.OverrideHP)
                    overrideHPCounter++;
            }

            //Amount
            if (overrideAmountCounter == wave.Chunks.Count)
                m_WavesEditorData[waveIndex].AmountOverride = WaveEditorData.OverrideTypes.Full;
            else if (overrideAmountCounter > 0)
                m_WavesEditorData[waveIndex].AmountOverride = WaveEditorData.OverrideTypes.Part;
            else
                m_WavesEditorData[waveIndex].AmountOverride = WaveEditorData.OverrideTypes.None;

            //Damage
            if (overrideDamageCounter == wave.Chunks.Count)
                m_WavesEditorData[waveIndex].DamageOverride = WaveEditorData.OverrideTypes.Full;
            else if (overrideDamageCounter > 0)
                m_WavesEditorData[waveIndex].DamageOverride = WaveEditorData.OverrideTypes.Part;
            else
                m_WavesEditorData[waveIndex].DamageOverride = WaveEditorData.OverrideTypes.None;

            //HP
            if (overrideHPCounter == wave.Chunks.Count)
                m_WavesEditorData[waveIndex].HPOverride = WaveEditorData.OverrideTypes.Full;
            else if (overrideHPCounter > 0)
                m_WavesEditorData[waveIndex].HPOverride = WaveEditorData.OverrideTypes.Part;
            else
                m_WavesEditorData[waveIndex].HPOverride = WaveEditorData.OverrideTypes.None;
        }

        void ValidateChunk(LevelDataFactory.WaveDataFactory parentWave, LevelDataFactory.ChunkDataFactory chunk)
        {
            //Amount
            if (!chunk.OverrideAmount)
            {
                chunk.EnemiesAmount = parentWave.EnemiesAmount;
            }
            else
            {
                if (chunk.EnemiesAmount <= 0)
                    chunk.EnemiesAmount = 1;
            }

            //Damage
            if (!chunk.OverrideDamage)
            {
                chunk.MinDamage = parentWave.MinDamage;
                chunk.MaxDamage = parentWave.MaxDamage;
            }
            else
            {
                ValidateMinMax(ref chunk.MinDamage, ref chunk.MaxDamage);
            }

            //HP
            if (!chunk.OverrideHP)
            {
                chunk.MinHP = parentWave.MinHP;
                chunk.MaxHP = parentWave.MaxHP;
            }
            else
            {
                ValidateMinMax(ref chunk.MinHP, ref chunk.MaxHP);
            }
        }

        void ValidateMinMax(ref int min, ref int max)
        {
            if (min < 0)
                min = 0;

            if (min > max)
                max = min;
        }


        void DrawLevel()
        {
            LevelDataFactory castedTarget = (LevelDataFactory)target;

            #region Properties

            DrawLevelProperies(castedTarget);

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

        void DrawWave(LevelDataFactory.WaveDataFactory wave, int waveIndex)
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
                        RemoveWave((LevelDataFactory)target, waveIndex);
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

        void DrawChunk(LevelDataFactory.ChunkDataFactory chunk, LevelDataFactory.WaveDataFactory parentWave, int chunkIndex)
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


        void DrawLevelProperies(LevelDataFactory level)
        {
            level.DamageForMissRhytm = EditorGUILayout.IntField("DamageForMissRhytm", level.DamageForMissRhytm);
            level.DelayBeforeStartLevel = EditorGUILayout.IntField("DelayBeforeStartLevel", level.DelayBeforeStartLevel);
            level.RecomendedAverageDmg = EditorGUILayout.IntField("RecomendedAverageDmg", level.RecomendedAverageDmg);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("DynamicDifficutlyReducePercent");
                level.DynamicDifficutlyReducePercent = EditorGUILayout.IntSlider(level.DynamicDifficutlyReducePercent, 10, 70);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Assets");
                level.Assets = (LevelAssets)EditorGUILayout.ObjectField(level.Assets, typeof(LevelAssets), false);
            }
        }

        void DrawWaveProperties(LevelDataFactory.WaveDataFactory wave, int waveIndex)
        {
            wave.DurationRestTicks = EditorGUILayout.IntField("Duration Rest (Ticks)", wave.DurationRestTicks);

            EditorGUILayout.Space();

            //Enemies Amount
            PreWavePropertyDraw(m_WavesEditorData[waveIndex].AmountOverride, out Color initColor);

            wave.EnemiesAmount = EditorGUILayout.IntField("Enemies Amount:", wave.EnemiesAmount);

            PostWavePropertyDraw(initColor);

            //Damage
            PreWavePropertyDraw(m_WavesEditorData[waveIndex].DamageOverride, out initColor);

            DrawMinMax("Min Damage:", ref wave.MinDamage, "Max Damage", ref wave.MaxDamage);

            PostWavePropertyDraw(initColor);

            //HP
            PreWavePropertyDraw(m_WavesEditorData[waveIndex].HPOverride, out initColor);

            DrawMinMax("Min HP:", ref wave.MinHP, "Max HP", ref wave.MaxHP);

            PostWavePropertyDraw(initColor);
        }

        void DrawChunkProperies(LevelDataFactory.ChunkDataFactory chunk, LevelDataFactory.WaveDataFactory parentWave, int chunkIndex)
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

        void PreWavePropertyDraw(WaveEditorData.OverrideTypes propertyOverride, out Color initColor)
        {
            GUI.enabled = propertyOverride != WaveEditorData.OverrideTypes.Full;
            initColor = EditorStyles.label.normal.textColor;

            if (propertyOverride == WaveEditorData.OverrideTypes.Part)
                EditorStyles.label.normal.textColor = Color.yellow;
        }

        void PostWavePropertyDraw(Color initColor)
        {
            EditorStyles.label.normal.textColor = initColor;
            GUI.enabled = true;

            EditorGUILayout.Space();
        }



        void AddWave(LevelDataFactory levelFactory)
        {
            levelFactory.Waves.Add(new LevelDataFactory.WaveDataFactory());
            m_WavesEditorData.Add(new WaveEditorData());
        }

        void RemoveWave(LevelDataFactory levelFactory, int waveIndex)
        {
            levelFactory.Waves.RemoveAt(waveIndex);
            m_WavesEditorData.RemoveAt(waveIndex);
        }

        void AddChunk(LevelDataFactory.WaveDataFactory wave)
        {
            wave.Chunks.Add(new LevelDataFactory.ChunkDataFactory());
        }

        void RemoveChunk(LevelDataFactory.WaveDataFactory wave, int chunkIndex)
        {
            wave.Chunks.RemoveAt(chunkIndex);
        }
    }
}
