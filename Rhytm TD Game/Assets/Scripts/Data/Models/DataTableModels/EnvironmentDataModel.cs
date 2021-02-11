using CoreFramework;
using RhytmTD.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Data.Models.DataTableModels
{
    /// <summary>
    /// Информация о построении уровней
    /// </summary>
    public class EnvironmentDataModel : BaseModel
    {
        public LevelParams[] LevelParamsData;

        private Dictionary<int, LevelParams> m_LevelParams;

        
        public void ReorganizeData()
        {
            m_LevelParams = new Dictionary<int, LevelParams>();
            for (int i = 0; i < LevelParamsData.Length; i++)
            {
                if (!m_LevelParams.ContainsKey(LevelParamsData[i].ID))
                    m_LevelParams.Add(LevelParamsData[i].ID, LevelParamsData[i]);
            }

            LevelParamsData = null;
        }

        /// <summary>
        /// Get level data by ID
        /// </summary>
        public LevelParams GetLevelParams(int levelID)
        {
            if (m_LevelParams.ContainsKey(levelID))
                return m_LevelParams[levelID];

            return null;
        }

        /// <summary>
        /// Получить прогресс прохождения уровней для рассчета прогрессий
        /// </summary>
        /// <param name="completedLevelIDs"></param>
        /// <returns></returns>
        public float GetCompletionForProgression(int[] completedLevelIDs)
        {
            int completedLevels = 0;
 
            foreach(LevelParams levelParam in m_LevelParams.Values)
            {
                if (IsLevelComplete(completedLevelIDs, levelParam.ID))
                    completedLevels++;
            }

            return (float)completedLevels / Mathf.Clamp(m_LevelParams.Count - 1, 1, m_LevelParams.Count); 
        }


        private bool IsLevelComplete(int[] completedLevelIDs, int levelID)
        {
            for (int i = 0; i < completedLevelIDs.Length; i++)
            {
                if (completedLevelIDs[i] == levelID)
                    return true;
            }

            return false;
        }


        [Serializable]
        public class LevelParams
        {
            public int ID = 1;
            public int BPM = 130;

            public BuildData BuildParams;
        }

        [Serializable]
        public class BuildData
        {
            public int Seed => OverrideSeed? LevelSeed : (int)ConvertersCollection.ConvertToUnixTimestamp(DateTime.Now);

            public bool OverrideSeed;
            public int LevelSeed;
        }
    }
}
