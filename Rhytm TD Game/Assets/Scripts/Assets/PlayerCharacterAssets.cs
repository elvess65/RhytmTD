using RhytmTD.Assets.Abstract;
using RhytmTD.Battle.Entities.Views;
using UnityEngine;

namespace RhytmTD.Assets.Battle
{
    [CreateAssetMenu(fileName = "New PlayerCharacter Assets", menuName = "Assets/Player Character Assets", order = 101)]
    public class PlayerCharacterAssets : PrefabAssets
    {
        public PlayerView PlayerPrefab;

        public override void Initialize()
        {
        }
    }
}
