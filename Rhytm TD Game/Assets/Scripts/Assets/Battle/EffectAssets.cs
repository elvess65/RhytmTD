using System.Collections.Generic;
using RhytmTD.Assets.Abstract;
using RhytmTD.Battle.Entities.Views;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Assets.Battle
{
    [CreateAssetMenu(fileName = "New Effect Assets", menuName = "Assets/Effects Assets", order = 101)]
    public class EffectAssets : PrefabAssets
    {
        public List<EffectEntityViewPrefabData> EffectEntityViewPrefabs;

        private Dictionary<BattlEffectID, BattleEntityView> m_EffectEntityViewPrefabs;

        public override void Initialize()
        {
            if (m_EffectEntityViewPrefabs == null)
            {
                m_EffectEntityViewPrefabs = new Dictionary<BattlEffectID, BattleEntityView>();
                for (int i = 0; i < EffectEntityViewPrefabs.Count; i++)
                {
                    m_EffectEntityViewPrefabs[EffectEntityViewPrefabs[i].ID] = EffectEntityViewPrefabs[i].Prefab;
                }
            }
        }

        public BattleEntityView GetEffectViewPrefab(BattlEffectID id)
        {
            if (m_EffectEntityViewPrefabs.ContainsKey(id))
            {
                return m_EffectEntityViewPrefabs[id];
            }

            return null;
        }

        [System.Serializable]
        public class EffectEntityViewPrefabData
        {
            public BattlEffectID ID;
            public BattleEntityView Prefab;
        }
    }
}
