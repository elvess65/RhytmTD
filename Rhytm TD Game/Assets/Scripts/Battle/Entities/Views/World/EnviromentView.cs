using CoreFramework;
using RhytmTD.Assets.Battle;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;
using RhytmTD.Data.Models.DataTableModels;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnviromentView : BaseView
    {
        [SerializeField] private Transform EnviromentParent = null;
        [SerializeField] private Transform FarParent = null;

        private EnviromentModel m_EnviromentModel;
        private LevelAssets m_Assets;
        

        void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_EnviromentModel = Dispatcher.GetModel<EnviromentModel>();
            m_EnviromentModel.OnInitializeEnviroment += InitializeEnviromentHandler;
            m_EnviromentModel.OnCellShouldBeAdded += AddCellHandler;
            m_EnviromentModel.OnCellRemoved += RemoveCellHandler;

            WorldDataModel m_WorldModel = Dispatcher.GetModel<WorldDataModel>();
            AccountDataModel m_AccountModel = Dispatcher.GetModel<AccountDataModel>();
            m_Assets = m_WorldModel.Areas[m_AccountModel.CompletedAreas].LevelsData[m_AccountModel.CompletedLevels].Assets;
        }

        private void InitializeEnviromentHandler()
        {
            CreateCell(m_Assets.StartEnviromentCelViewPrefab);
            CreateFarEnviroment();
        }

        private void AddCellHandler()
        {
            CreateCell(m_Assets.GetRandomEnviromentCellViewPrefab());
        }

        private void RemoveCellHandler(EnviromentCellView cell)
        {
            Destroy(cell.gameObject);

            m_EnviromentModel.RecalculateActionDistances();
        }

        private void CreateCell(EnviromentCellView prefabSource)
        {
            EnviromentCellView cell = Instantiate(prefabSource);
            cell.transform.SetParent(EnviromentParent);
            cell.transform.localScale = Vector3.one;
            cell.transform.localPosition = m_EnviromentModel.LastCellPos + Vector3.forward * m_EnviromentModel.LastCellLength;

            m_EnviromentModel.AddCell(cell);
            m_EnviromentModel.RecalculateActionDistances();
        }

        private void CreateFarEnviroment()
        {
            GameObject far = Instantiate(m_Assets.FarObjectPrefab);
            far.transform.SetParent(FarParent);
            far.transform.localScale = Vector3.one;
            far.transform.localPosition = Vector3.zero;
        }

        private void OnDestroy()
        {
            m_EnviromentModel.OnInitializeEnviroment -= InitializeEnviromentHandler;
            m_EnviromentModel.OnCellShouldBeAdded -= AddCellHandler;
            m_EnviromentModel.OnCellRemoved -= RemoveCellHandler;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(EnviromentParent.position, new Vector3(10, 1, 15));
        }
    }
}
