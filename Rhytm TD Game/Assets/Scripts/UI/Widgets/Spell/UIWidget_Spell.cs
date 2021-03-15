﻿using CoreFramework.Rhytm;
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

        private SkillsController m_SkillsController;

        private int m_SkillTypeID;
        

        public void Initialize(int skillTypeID, int skillID)
        {
            m_SkillsController = Dispatcher.GetController<SkillsController>();

            m_SkillTypeID = skillTypeID;

            WorldDataModel worldDataModel = Dispatcher.GetModel<WorldDataModel>();
            (Sprite sprite, Color color) iconSpriteData = worldDataModel.UISpriteAssets.GetSkillIconSprite(m_SkillTypeID);

            //TODO: Get name from localization
            UIWidgetSpellInfo.Initialize(TEMP_GetSpellNameByID(m_SkillTypeID), iconSpriteData.sprite, iconSpriteData.color);
            UIWidgetSpellInfo.OnButtonInfoPressHandler += SpellInfoPressHandler;

            bool isInCooldown = false;
            if (m_SkillsController.IsSkillInCooldown(skillID, out float remainsCooldownSeconds))
            {
                isInCooldown = true;
                Debug.Log("SKILL " + skillTypeID + " is in cooldown and will be there for " + remainsCooldownSeconds + " sec");
            }


            UIWidgetSpellSequence.Initialize(skillTypeID, isInCooldown);

            InternalInitialize();
        }

        public override void LockInput(bool isLocked)
        {
            UIWidgetSpellInfo.LockInput(isLocked);
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

        private void SpellInfoPressHandler()
        {
            Debug.Log("Get info for " + m_SkillTypeID);
        }
    }
}
