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

        private int m_SkillTypeID;
        private int m_SkillID;

        public void Initialize(int skillTypeID, int skillID)
        {
            m_SkillTypeID = skillTypeID;
            m_SkillID = skillID;

            UIWidgetSpellInfo.Initialize(TEMP_GetSpellNameByID(m_SkillTypeID));
            UIWidgetSpellInfo.OnButtonInfoPressHandler += SpellInfoPressHandler;

            UIWidgetSpellSequence.Initialize(skillTypeID);

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
            Debug.Log("Get info for " + m_SkillTypeID + " " + m_SkillID);
        }
    }
}
