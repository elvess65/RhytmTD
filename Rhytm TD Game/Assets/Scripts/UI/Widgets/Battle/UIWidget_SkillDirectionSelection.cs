using CoreFramework;
using CoreFramework.UI.Widget;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models.DataTableModels;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Widget
{
    public class UIWidget_SkillDirectionSelection : UIWidget
    {
        [Space]
        [SerializeField] private Text Text_DirectionHint = null;

        private AccountBaseParamsDataModel m_AccountBaseParamsDataModel;


        public void Initialize()
        {
            InternalInitialize();

            PrepareSkilIUseModel prepareSkilIUseModel = Dispatcher.GetModel<PrepareSkilIUseModel>();
            prepareSkilIUseModel.OnSkillSelected += SkillSelectedHandler;

            m_AccountBaseParamsDataModel = Dispatcher.GetModel<AccountBaseParamsDataModel>();
        }

        private void SkillSelectedHandler(int skillTypeID)
        {
            string message = string.Empty;
            switch(m_AccountBaseParamsDataModel.GetSkillBaseDataByID(skillTypeID).TargetingType)
            {
                case EnumsCollection.SkillTargetingType.Area:
                    message = "Select spell attack area";
                    break;
                case EnumsCollection.SkillTargetingType.Direction:
                    message = "Select spell direction";
                    break;
            }

            Text_DirectionHint.text = message;
        }
    }
}
