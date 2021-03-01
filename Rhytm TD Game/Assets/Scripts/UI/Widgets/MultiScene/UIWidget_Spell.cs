using RhytmTD.UI.Components.Spell;
using UnityEngine;

namespace RhytmTD.UI.Widget
{
    public class UIWidget_Spell : UIWidget
    {
        public System.Action<int, int> OnPrepareSkillUse;

        [Space]
        [SerializeField] private UIWidget_SpellInfo UIWidgetSpellInfo;

        private int m_SkillTypeID;
        private int m_SkillID;

        public void Initialize(int skillTypeID, int skillID)
        {
            m_SkillTypeID = skillTypeID;
            m_SkillID = skillID;

            UIWidgetSpellInfo.Initialize(TEMP_GetSpellNameByID(m_SkillTypeID));
            UIWidgetSpellInfo.OnButtonInfoPressHandler += SpellInfoPressHandler;

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
            OnPrepareSkillUse?.Invoke(m_SkillTypeID, m_SkillID);
        }
    }
}
