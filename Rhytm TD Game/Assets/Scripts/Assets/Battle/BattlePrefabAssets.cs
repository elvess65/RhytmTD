using System.Collections.Generic;
using RhytmTD.Assets.Abstract;
using RhytmTD.Battle.Entities.Views;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Assets.Battle
{
    [CreateAssetMenu(fileName = "New Battle PrefabsLibrary", menuName = "Assets/Battle Prefabs Library", order = 101)]
    public class BattlePrefabAssets : PrefabAssets
    {
        public PlayerView PlayerPrefab;
        public List<EffectEntityViewPrefabData> EffectEntityViewPrefabs;

        private Dictionary<BattleEntityEffectID, BattleEntityView> m_EffectEntityViewPrefabs;

        public override void Initialize()
        {
        }

        public BattleEntityView GetEffectViewPrefab(BattleEntityEffectID id)
        {
            if (m_EffectEntityViewPrefabs == null)
            {
                m_EffectEntityViewPrefabs = new Dictionary<BattleEntityEffectID, BattleEntityView>();
                for (int i = 0; i < EffectEntityViewPrefabs.Count; i++)
                {
                    m_EffectEntityViewPrefabs[EffectEntityViewPrefabs[i].ID] = EffectEntityViewPrefabs[i].Prefab;
                }
            }

            if (m_EffectEntityViewPrefabs.ContainsKey(id))
            {
                return m_EffectEntityViewPrefabs[id];
            }

            return null;
        }

        [System.Serializable]
        public class EffectEntityViewPrefabData
        {
            public BattleEntityEffectID ID;
            public BattleEntityView Prefab;
        }
    }
}
