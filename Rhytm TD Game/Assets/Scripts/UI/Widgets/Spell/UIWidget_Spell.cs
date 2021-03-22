using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models.DataTableModels;
using UnityEngine;

namespace RhytmTD.UI.Widget
{
    /// <summary>
    /// Widgets that contains spell info and sequence
    /// </summary>
    public class UIWidget_Spell : UIWidget
    {
        [Space]
        [SerializeField] private UIWidget_SpellInfo UIWidgetSpellInfo = null;
        [SerializeField] private UIWidget_SpellSequence UIWidgetSpellSequence = null;

        private SpellBookModel m_SpellBookModel;
        private SkillsCooldownController m_SkillsCooldownController;

        private int m_SkillTypeID;
        private int m_SkillID;
        

        public void Initialize(int skillTypeID, int skillID)
        {
            m_SpellBookModel = Dispatcher.GetModel<SpellBookModel>();
            m_SkillsCooldownController = Dispatcher.GetController<SkillsCooldownController>();

            m_SpellBookModel.OnSpellbookOpened += SpellbookOpenedHandler;

            m_SkillTypeID = skillTypeID;
            m_SkillID = skillID;

            WorldDataModel worldDataModel = Dispatcher.GetModel<WorldDataModel>();
            (Sprite sprite, Color color) iconSpriteData = worldDataModel.UISpriteAssets.GetSkillIconSprite(m_SkillTypeID);

            //TODO: Get name from localization
            UIWidgetSpellInfo.Initialize(TEMP_GetSpellNameByID(m_SkillTypeID), iconSpriteData.sprite, iconSpriteData.color);
            UIWidgetSpellInfo.OnButtonInfoPressHandler += SpellInfoPressHandler;

            UIWidgetSpellSequence.Initialize(skillTypeID);

            InternalInitialize();
        }

        public override void LockInput(bool isLocked)
        {
            UIWidgetSpellInfo.LockInput(isLocked);
        }


        private void SpellbookOpenedHandler()
        {
            float skillCooldownRemainTime = m_SkillsCooldownController.GetSkillInCooldownRemainTime(m_SkillID);
            if (skillCooldownRemainTime > 0)
            {
                Debug.Log("SKILL " + m_SkillID + " is in cooldown and will be there for " + skillCooldownRemainTime + " sec");
            }
        }

        private void SpellInfoPressHandler()
        {
            Debug.Log("Get info for " + m_SkillTypeID);
        }

        private string TEMP_GetSpellNameByID(int skillTypeID)
        {
            switch (skillTypeID)
            {
                case ConstsCollection.SkillConsts.METEORITE_ID:
                    return "Meteorite";
                case ConstsCollection.SkillConsts.FIREBALL_ID:
                    return "Fireball";
                case ConstsCollection.SkillConsts.HEALTH_ID:
                    return "Heal";
            }

            return string.Empty;
        }

    }
}
