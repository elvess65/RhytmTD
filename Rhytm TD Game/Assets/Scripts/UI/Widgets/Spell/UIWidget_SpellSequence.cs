using System.Collections.Generic;
using CoreFramework;
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

        private BattleModel m_BattleModel;
        private WorldDataModel m_WorldDataModel;
        private PrepareSkilIUseModel m_PrepareSkilIUseModel;
        private SkillSequenceDataModel m_SkillSequenceDataModel;
        private AccountBaseParamsDataModel m_AccountBaseParamsDataModel;

        private int m_SkillTypeID;
        private int m_CurStepIndex;
        private bool m_IsSelected;
        private bool m_PrevTickWasInput;
        private bool m_IsInCooldown = false;

        private List<bool> m_Pattern;
        private UIComponent_SpellSequenceItem[] m_SequenceItems;


        public void Initialize(int skillTypeID)
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_WorldDataModel = Dispatcher.GetModel<WorldDataModel>();
            m_PrepareSkilIUseModel = Dispatcher.GetModel<PrepareSkilIUseModel>();
            m_SkillSequenceDataModel = Dispatcher.GetModel<SkillSequenceDataModel>();
            m_AccountBaseParamsDataModel = Dispatcher.GetModel<AccountBaseParamsDataModel>();

            m_PrepareSkilIUseModel.OnSkillReset += SkillResetHandler;
            m_PrepareSkilIUseModel.OnSkillStepReachedInput += OnSkillStepReachedInputHandler;
            m_PrepareSkilIUseModel.OnSkillStepReachedAuto += OnSkillStepReachedAutoHandler;
            m_PrepareSkilIUseModel.OnSequenceFailed += SequenceFailedHandler;
            m_PrepareSkilIUseModel.OnSkillSelected += SkillSelectedHandler;
            
            m_BattleModel.OnSpellbookOpened += SpellbookOpenedHandler;

            m_SkillTypeID = skillTypeID;

            CreateSequenceItems();
            InternalInitialize();
        }

        public void SetCooldown(bool isInCooldown)
        {
            m_IsInCooldown = isInCooldown;
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
            if (skillID != m_SkillTypeID || m_IsInCooldown)
                return;

            m_IsSelected = true;

            for (int i = 0; i < m_SequenceItems.Length; i++)
            {
                m_SequenceItems[i].SetState(UIComponent_SpellSequenceItem.ItemStates.Selected);
            }
        }

        private void OnSkillStepReachedInputHandler(int skillTypeID)
        {
            if (skillTypeID != m_SkillTypeID || m_IsInCooldown)
                return;

            m_SequenceItems[m_CurStepIndex].SetState(UIComponent_SpellSequenceItem.ItemStates.Visited);
            m_CurStepIndex++;

            m_PrevTickWasInput = true;
        }

        private void OnSkillStepReachedAutoHandler(int skillTypeID)
        {
            if (skillTypeID != m_SkillTypeID || m_IsInCooldown)
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
            if (skillTypeID != m_SkillTypeID || m_IsInCooldown)
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
            if (m_IsSelected || m_IsInCooldown)
                return;

            for (int i = 0; i < m_SequenceItems.Length; i++)
            {
                m_SequenceItems[i].SetState(UIComponent_SpellSequenceItem.ItemStates.Active);
            }
        }


        private void SpellbookOpenedHandler()
        {
            m_IsSelected = false;

            for (int i = 0; i < m_SequenceItems.Length; i++)
            {
                m_SequenceItems[i].SetState(m_IsInCooldown ? UIComponent_SpellSequenceItem.ItemStates.Reseted : UIComponent_SpellSequenceItem.ItemStates.Active);
            }
        }
    }
}
