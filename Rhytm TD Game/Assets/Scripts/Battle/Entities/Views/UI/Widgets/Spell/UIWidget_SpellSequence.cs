using System.Collections;
using System.Collections.Generic;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models.DataTableModels;
using RhytmTD.UI.Components;
using UnityEngine;

namespace RhytmTD.UI.Widget
{
    public class UIWidget_SpellSequence : UIWidget
    {
        [Space]
        [SerializeField] private RectTransform m_ItemsRoot = null;

        private WorldDataModel m_WorldDataModel;
        private PrepareSkilIUseModel m_PrepareSkilIUseModel;

        private int m_SkillTypeID;

        private bool[] m_Pattern;
        private UIComponent_SpellSequenceItem[] m_SequenceItems;

        public void Initialize(int skillTypeID)
        {
            m_WorldDataModel = Dispatcher.GetModel<WorldDataModel>();
            m_PrepareSkilIUseModel = Dispatcher.GetModel<PrepareSkilIUseModel>();
            m_PrepareSkilIUseModel.OnSpellReset += SpellResetHandler;
            m_PrepareSkilIUseModel.OnSpellNextTickInput += SpellNextTickInputHadler;
            m_PrepareSkilIUseModel.OnSpellNextTickAuto += SpellNextTickAutoHadler;
            m_PrepareSkilIUseModel.OnAllSpellsReset += AllSpellsResetHandler;
            m_PrepareSkilIUseModel.OnSpellSelected += SpellSelectedHandler;

            m_SkillTypeID = skillTypeID;

            CreateSequenceItems();
            InternalInitialize();
        }

        private void CreateSequenceItems()
        {
            m_Pattern = PrepareSkillUseController.tempSkillPatterns[m_SkillTypeID];
            m_SequenceItems = new UIComponent_SpellSequenceItem[m_Pattern.Length];

            for (int i = 0; i < m_Pattern.Length; i++)
            {
                UIComponent_SpellSequenceItem item = m_WorldDataModel.UIAssets.InstantiatePrefab(m_WorldDataModel.UIAssets.UIComponentSpellSequenceItemPrefab);
                item.transform.SetParent(m_ItemsRoot);
                item.transform.localPosition = Vector3.zero;

                item.Initialize(m_Pattern[i]);

                m_SequenceItems[i] = item;
            }
        }

        private bool m_IsSelected = false;

        private void SpellSelectedHandler(int spellID)
        {
            if (spellID != m_SkillTypeID)
                return;

            m_IsSelected = true;

            for (int i = 0; i < m_SequenceItems.Length; i++)
                m_SequenceItems[i].SetState(UIComponent_SpellSequenceItem.ItemStates.Visited);
        }

        private void AllSpellsResetHandler()
        {
            m_CurStepIndex = 0;

            if (m_IsSelected)
                return;

            for (int i = 0; i < m_SequenceItems.Length; i++)
                m_SequenceItems[i].SetState(UIComponent_SpellSequenceItem.ItemStates.Active);
        }

        private void SpellResetHandler(int skillTypeID)
        {
            if (m_IsSelected)
                return;

            if (skillTypeID != m_SkillTypeID)
                return;

            for (int i = 0; i < m_SequenceItems.Length; i++)
                m_SequenceItems[i].SetState(UIComponent_SpellSequenceItem.ItemStates.Reseted);
        }

        private int m_CurStepIndex = 0;
        private bool m_PrevTickWasInput = false;
        private void SpellNextTickInputHadler(int skillTypeID)
        {
            if (skillTypeID != m_SkillTypeID)
                return;

            Debug.Log(m_SkillTypeID + " " + m_CurStepIndex);
            m_SequenceItems[m_CurStepIndex].SetState(UIComponent_SpellSequenceItem.ItemStates.Visited);
            m_CurStepIndex++;
            m_PrevTickWasInput = true;
        }

        private void SpellNextTickAutoHadler(int skillTypeID)
        {
            if (skillTypeID != m_SkillTypeID)
                return;

            Debug.Log(m_SkillTypeID + " " + m_CurStepIndex);
            if (!m_Pattern[m_CurStepIndex] && !m_PrevTickWasInput)
            {
                m_SequenceItems[m_CurStepIndex].SetState(UIComponent_SpellSequenceItem.ItemStates.Visited);
                m_CurStepIndex++;
            }

            m_PrevTickWasInput = false;
        }
    }
}
