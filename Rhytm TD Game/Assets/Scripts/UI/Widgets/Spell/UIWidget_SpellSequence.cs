using System.Collections.Generic;
using CoreFramework;
using CoreFramework.UI.Widget;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models.DataTableModels;
using RhytmTD.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Widget
{
    public class UIWidget_SpellSequence : UIWidget
    {
        [Space]
        [SerializeField] private RectTransform m_ItemsRoot = null;
        [SerializeField] private Text m_TextManaRequiredInfo = null;

        private SpellBookModel m_SpellBookModel;
        private WorldDataModel m_WorldDataModel;
        private PrepareSkilIUseModel m_PrepareSkilIUseModel;
        private SkillSequenceDataModel m_SkillSequenceDataModel;
        private AccountBaseParamsDataModel m_AccountBaseParamsDataModel;

        private int m_SkillTypeID;
        private int m_CurStepIndex;
        private bool m_IsSelected;
        private bool m_PrevTickWasInput;
        private bool m_IsInCooldown = false;
        private bool m_HasEnoughMana = true;

        private List<bool> m_Pattern;
        private UIComponent_SpellSequenceItem[] m_SequenceItems;

        private bool m_IsActive => !m_IsInCooldown && m_HasEnoughMana;


        public void Initialize(int skillTypeID)
        {
            m_SpellBookModel = Dispatcher.GetModel<SpellBookModel>();
            m_WorldDataModel = Dispatcher.GetModel<WorldDataModel>();
            m_PrepareSkilIUseModel = Dispatcher.GetModel<PrepareSkilIUseModel>();
            m_SkillSequenceDataModel = Dispatcher.GetModel<SkillSequenceDataModel>();
            m_AccountBaseParamsDataModel = Dispatcher.GetModel<AccountBaseParamsDataModel>();

            m_PrepareSkilIUseModel.OnSkillReset += SkillResetHandler;
            m_PrepareSkilIUseModel.OnSkillStepReachedInput += OnSkillStepReachedInputHandler;
            m_PrepareSkilIUseModel.OnSkillStepReachedAuto += OnSkillStepReachedAutoHandler;
            m_PrepareSkilIUseModel.OnSequenceFailed += SequenceFailedHandler;
            m_PrepareSkilIUseModel.OnSkillSelected += SkillSelectedHandler;

            m_SpellBookModel.OnSpellbookOpened += SpellbookOpenedHandler;

            m_SkillTypeID = skillTypeID;

            m_TextManaRequiredInfo.text = "Not enough mana";

            CreateSequenceItems();
            InternalInitialize();
        }

        public void SetCooldown(bool isInCooldown)
        {
            m_IsInCooldown = isInCooldown;
        }

        public void SetEnoughMana(bool hasEnoughMana)
        {
            m_HasEnoughMana = hasEnoughMana;
        }


        private void CreateSequenceItems()
        {
            EnumsCollection.SkillSequencePatternID patternID = m_AccountBaseParamsDataModel.GetSkillBaseDataByID(m_SkillTypeID).DefaultPatternID;
            m_Pattern = m_SkillSequenceDataModel.GetSkillSequencePatternByID(patternID);

            m_SequenceItems = new UIComponent_SpellSequenceItem[m_Pattern.Count];

            for (int i = 0; i < m_Pattern.Count; i++)
            {
                UIComponent_SpellSequenceItem item = m_WorldDataModel.UIAssets.InstantiatePrefab(m_WorldDataModel.UIAssets.UIComponentSpellSequenceItemPrefab);
                item.transform.SetParent(m_ItemsRoot);
                item.transform.localPosition = Vector3.zero;

                item.Initialize(m_Pattern[i]);

                m_SequenceItems[i] = item;
            }
        }

       
        private void SkillSelectedHandler(int skillID)
        {
            if (skillID != m_SkillTypeID || !m_IsActive)
                return;

            m_IsSelected = true;

            for (int i = 0; i < m_SequenceItems.Length; i++)
            {
                m_SequenceItems[i].SetState(UIComponent_SpellSequenceItem.ItemStates.Selected);
            }
        }

        private void OnSkillStepReachedInputHandler(int skillTypeID)
        {
            if (skillTypeID != m_SkillTypeID || !m_IsActive)
                return;

            m_SequenceItems[m_CurStepIndex].SetState(UIComponent_SpellSequenceItem.ItemStates.Visited);
            m_CurStepIndex++;

            m_PrevTickWasInput = true;
        }

        private void OnSkillStepReachedAutoHandler(int skillTypeID)
        {
            if (skillTypeID != m_SkillTypeID || !m_IsActive)
                return;

            if (!m_Pattern[m_CurStepIndex] && !m_PrevTickWasInput)
            {
                m_SequenceItems[m_CurStepIndex].SetState(UIComponent_SpellSequenceItem.ItemStates.Visited);
                m_CurStepIndex++;
            }

            m_PrevTickWasInput = false;
        }

        private void SkillResetHandler(int skillTypeID)
        {
            if (skillTypeID != m_SkillTypeID || !m_IsActive)
                return;

            m_CurStepIndex = 0;

            if (m_IsSelected)
                return;

            for (int i = 0; i < m_SequenceItems.Length; i++)
            {
                m_SequenceItems[i].SetState(UIComponent_SpellSequenceItem.ItemStates.Reseted);
            }
        }

        private void SequenceFailedHandler()
        {
            if (m_IsSelected || !m_IsActive)
                return;

            for (int i = 0; i < m_SequenceItems.Length; i++)
            {
                m_SequenceItems[i].SetState(UIComponent_SpellSequenceItem.ItemStates.Active);
            }
        }


        private void SpellbookOpenedHandler()
        {
            m_IsSelected = false;

            m_TextManaRequiredInfo.gameObject.SetActive(!m_HasEnoughMana);

            for (int i = 0; i < m_SequenceItems.Length; i++)
            {
                m_SequenceItems[i].gameObject.SetActive(m_HasEnoughMana);

                if (!m_HasEnoughMana)
                    continue;

                UIComponent_SpellSequenceItem.ItemStates state = UIComponent_SpellSequenceItem.ItemStates.Active;
                if (m_IsInCooldown)
                    state = UIComponent_SpellSequenceItem.ItemStates.Reseted;

                m_SequenceItems[i].SetState(state);
            }
        }
    }
}
