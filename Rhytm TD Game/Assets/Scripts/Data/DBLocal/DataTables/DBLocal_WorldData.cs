using RhytmTD.Assets.Battle;
using RhytmTD.Data.Factory;
using UnityEngine;

namespace RhytmTD.Data.DataBaseLocal
{
    /// <summary>
    /// Мир, который содержит зоны
    /// Каждая зона строит уровни содержащимся в ней данным
    /// Уровни строят волны
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Local WorldData", menuName = "DBLocal/Levels/WorldData", order = 101)]
    public class DBLocal_WorldData : ScriptableObject
    {
        public PlayerCharacterAssets PlayerCharacterAssets;
        public EffectAssets EffectAssets;
        public UIAssets UIAssets;
        public UISpriteAssets UISpriteAssets;
        public AreaData[] Areas;

        [System.Serializable]
        public class AreaData
        {
            public int ID;
            public LevelDataFactory[] LevelsData;
        }
    }
}
