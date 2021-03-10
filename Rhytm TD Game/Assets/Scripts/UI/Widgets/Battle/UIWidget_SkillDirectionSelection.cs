using RhytmTD.Battle.Entities.Models;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Widget
{
    public class UIWidget_SkillDirectionSelection : UIWidget
    {
        [Space]
        public Text Text_DirectionHint;

        public void Initialize()
        {
            InternalInitialize();

            PrepareSkilIUseModel prepareSkilIUseModel = Dispatcher.GetModel<PrepareSkilIUseModel>();
            prepareSkilIUseModel.OnSkillSelected += SkillSelectedHandler;
        }

        private void SkillSelectedHandler(int skillTypeID)
        {
            Debug.Log("Show data for " + skillTypeID);

            //TODO Get from config

            if (skillTypeID == ConstsCollection.SkillConsts.FIREBALL_ID)
                Text_DirectionHint.text = "Select spell direction";
            else
                Text_DirectionHint.text = "Select spell attack area";
        }
    }
}
