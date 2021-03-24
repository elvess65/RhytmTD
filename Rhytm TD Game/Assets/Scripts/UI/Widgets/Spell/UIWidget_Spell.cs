using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models.DataTableModels;
using UnityEngine;
using UnityEngine.UI;

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

        private SpellBookModel m_SpellBookModel;

        private SkillsCooldownController m_SkillsCooldownController;
        private PlayerManaController m_PlayerManaController;

        private int m_SkillTypeID;
        private int m_SkillID;
        
        public void Initialize(int skillTypeID, int skillID)
        {
            m_SkillsCooldownController = Dispatcher.GetController<SkillsCooldownController>();
            m_PlayerManaController = Dispatcher.GetController<PlayerManaController>();

            m_SpellBookModel = Dispatcher.GetModel<SpellBookModel>();
            m_SpellBookModel.OnSpellbookOpened += SpellbookOpenedHandler;

            m_SkillsCooldownController = Dispatcher.GetController<SkillsCooldownController>();

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

            bool hasEnoughMana = m_PlayerManaController.IsEnoughManaForSkill(m_SkillTypeID);
            bool isInCooldown = cooldownData.remainTime > 0;

            SetAlpha(isInCooldown || !hasEnoughMana, UIWidgetSpellInfo.ExposedImageSpellIcon);
            UIWidgetSpellSequence.SetEnoughMana(hasEnoughMana);
            UIWidgetSpellSequence.SetCooldown(isInCooldown);
            UIWidget_ProgressIndicator.SetWidgetActive(isInCooldown, false);

            if (isInCooldown)
            {
                UIWidget_ProgressIndicator.SetProgress(cooldownData.remainTime / cooldownData.totalTime);
            }
        }

        private void SpellInfoPressHandler()
        {
            Debug.Log("Get info for " + m_SkillTypeID);
        }

        private void SetAlpha(bool isHalfAlpha, Image image)
        {
            Color color = image.color;
            color.a = isHalfAlpha ? 0.5f : 1;

            image.color = color;
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
