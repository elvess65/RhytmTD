using System.Collections;
using RhytmTD.Persistant.Abstract;
using RhytmTD.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Widget
{
    /// <summary>
    /// Виджет бара опыта
    /// </summary>
    public class UIWidget_ExperianceBar : UIWidget, iUpdatable
    {
        [Space(10)]
        [SerializeField] private UIComponent_Bar m_Bar;
        [SerializeField] private Text m_Text_Level;
        [SerializeField] private Text m_Text_Experiance;
        [SerializeField] private Text m_Text_GainedExperiance;
        
        private int m_ExpAmount = 0;
        private WaitForSeconds m_WaitForGainedExpShowTime;

        private const float m_WAIT_FOR_GAINED_EXP_SHOP_TIME = 1;


        public void Initialize(int expAmount)
        {
            m_Text_GainedExperiance.enabled = false;
            m_WaitForGainedExpShowTime = new WaitForSeconds(m_WAIT_FOR_GAINED_EXP_SHOP_TIME);

            UpdateData(expAmount);
        }

        public void UpdateBar(int expGained)
        {
            UpdateData(m_ExpAmount + expGained);
            ShowGainedExp(expGained);
        }

        public void PerformUpdate(float deltaTime)
        {
        }


        private void UpdateData(int expAmount)
        {
            m_ExpAmount = expAmount;

            int curLevel = GetLevelByExp(m_ExpAmount);
            int expToNextLvl = GetExpToNextLevel(curLevel);

            m_Bar.UpdateBar(m_ExpAmount, expToNextLvl);
            UpdateTexts(curLevel, m_ExpAmount, expToNextLvl);
        }

        private void UpdateTexts(int curLevel, int curExp, int expToNextLvl)
        {
            m_Text_Level.text = curLevel.ToString();
            m_Text_Experiance.text = $"{curExp}/{expToNextLvl}";
        }

        private void ShowGainedExp(int gainedExp)
        {
            m_Text_GainedExperiance.text = $"+{gainedExp}";

            StartCoroutine(WaitGainedExpShowTime());
        }



        private int GetLevelByExp(int expAmount)
        {
            return 0;//GameManager.Instance.DataHolder.DataTableModel.LevelingDataModel.GetWeaponLevelByExp(m_CharacterID, expAmount);
        }

        private int GetExpToNextLevel(int curLevel)
        {
            return 0;// GameManager.Instance.DataHolder.DataTableModel.LevelingDataModel.GetWeaponExpForLevel(m_CharacterID, curLevel + 1);
        }

        IEnumerator WaitGainedExpShowTime()
        {
            m_Text_GainedExperiance.enabled = true;

            yield return m_WaitForGainedExpShowTime;

            m_Text_GainedExperiance.enabled = false;
        }
    }
}
