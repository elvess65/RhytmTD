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
        [SerializeField] private UIWidget_ProgressIndicator UIWidget_ProgressIndicator = null;

        private BattleModel m_BattleModel;
        private SkillsCooldownController m_SkillsCooldownController;

        private int m_SkillTypeID;
        private int m_SkillID;
        

        public void Initialize(int skillTypeID, int skillID)
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_SkillsCooldownController = Dispatcher.GetController<SkillsCooldownController>();

            m_BattleModel.OnSpellbookOpened += SpellbookOpenedHandler;

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
            (float remainTime, float totalTime) cooldownData = m_SkillsCooldownController.GetSkillCooldownTime(m_SkillID);
            SetCooldownState(cooldownData.remainTime > 0);

            if (cooldownData.remainTime > 0)
            {
                UIWidget_ProgressIndicator.SetProgress(cooldownData.remainTime / cooldownData.totalTime);
            }
        }

        private void SpellInfoPressHandler()
        {
            Debug.Log("Get info for " + m_SkillTypeID);
        }

        private void SetCooldownState(bool isInCooldown)
        {
            //SpellInfo Widget
            Color color = UIWidgetSpellInfo.ExposedImageSpellIcon.color;
            color.a = isInCooldown ? 0.5f : 1;

            UIWidgetSpellInfo.ExposedImageSpellIcon.color = color;

            //SpellSequence Widget
            UIWidgetSpellSequence.SetCooldown(isInCooldown);

            //Progress Widget
            UIWidget_ProgressIndicator.SetWidgetActive(isInCooldown, false);
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
