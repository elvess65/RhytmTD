using RhytmTD.UI.Components.Spell;
using UnityEngine;

namespace RhytmTD.UI.Widget
{
    public class UIWidget_Spell : UIWidget
    {
        public System.Action<int> OnSpellUse;

        [Space]
        [SerializeField] private UIComponent_SpellInfo UIComponentSpellInfo;

        private int m_SkillTypeID;
        private int m_SkillID;

        public void Initialize(int skillTypeID, int skillID)
        {
            m_SkillTypeID = skillTypeID;
            m_SkillID = skillID;

            UIComponentSpellInfo.Initialize(TEMP_GetSpellNameByID(m_SkillTypeID));
            UIComponentSpellInfo.OnButtonInfoPressHandler += SpellInfoPressHandler;

            InternalInitialize();
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
            OnSpellUse?.Invoke(m_SkillID);
        }
    }
}
