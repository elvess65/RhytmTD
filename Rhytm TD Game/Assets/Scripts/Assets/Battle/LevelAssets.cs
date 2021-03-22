using CoreFramework;
using RhytmTD.Assets.Abstract;
using RhytmTD.Battle.Entities.Views;
using UnityEngine;

namespace RhytmTD.Assets.Battle
{
    [CreateAssetMenu(fileName = "New Level Assets", menuName = "Assets/Level Assets", order = 101)]
    public class LevelAssets : PrefabAssets
    {
        [SerializeField] private EnemyView[] m_EnemyPrefabs = null;

        [Header("Enviroment")]
        [SerializeField] public GameObject FarObjectPrefab = null;
        [SerializeField] public EnviromentCellView StartEnviromentCelViewlPrefab = null;

        [SerializeField] private EnviromentCellView[] m_EnviromentCelViewlPrefabs = null;


        public override void Initialize()
        {
        }


        public EnemyView GetRandomEnemyViewPrefab() => GetRandomPrefab(m_EnemyPrefabs);

        public EnviromentCellView GetRandomEnviromentCellViewPrefab() => GetRandomPrefab(m_EnviromentCelViewlPrefabs);


        private T GetRandomPrefab<T>(T[] views) where T : BaseView
        {
            int rndIndex = Random.Range(0, views.Length);
            return views[rndIndex];
        }
    }
}
