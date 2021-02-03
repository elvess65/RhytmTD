

using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    [CreateAssetMenu(menuName = "BattleAssets/DefaultBattleFactory")]
    [System.Serializable]
    public class DefaultBattleFactory : ScriptableObject, IBattleFactory
    {
        [SerializeField] private int ID;

        public void CreateBattle()
        {
        }
    }
}
