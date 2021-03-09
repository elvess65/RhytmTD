using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Components
{
    public class UIComponent_SpellSequenceItem : MonoBehaviour
    {
        public enum ItemStates
        {
            /// <summary>
            /// Ready for input (initial state)
            /// </summary>
            Active,
            
            /// <summary>
            /// Input is missed
            /// </summary>
            Reseted,

            /// <summary>
            /// Input was correct
            /// </summary>
            Visited,

            /// <summary>
            /// Spell was selected
            /// </summary>
            Selected
        }

        [SerializeField] private Image Image_Action = null;
        [SerializeField] private Image Image_Visited = null;
        [SerializeField] private Sprite m_ActionSprite;
        [SerializeField] private Sprite m_SkipSprite;
        [SerializeField] private Color m_ActionColor;
        [SerializeField] private Color m_SkipColor;

        private bool m_IsAction;

        public void Initialize(bool isAction)
        {
            Image_Action.color = isAction ? m_ActionColor : m_SkipColor;
            Image_Action.sprite = isAction ? m_ActionSprite : m_SkipSprite;

            m_IsAction = isAction;

            SetState(ItemStates.Active);
        }

        public void SetState(ItemStates state)
        {
            switch(state)
            {
                case ItemStates.Active:

                    SetScale(1);
                    SetAlpha(1);
                    SetVisited(false);

                    break;
                case ItemStates.Reseted:

                    SetScale(1);
                    SetAlpha(0.5f);
                    SetVisited(false);

                    break;
                case ItemStates.Visited:

                    if (m_IsAction)
                    {
                        SetVisited(true);
                        SetScale(1.2f);
                    }
                    else
                        SetScale(1.5f);

                    break;

                case ItemStates.Selected:

                    SetScale(1.5f);

                    if (m_IsAction)
                    {
                        SetVisited(true);
                        SetScale(1.5f);
                    }
                    else
                        SetScale(1.7f);

                    break;
            }
        }

        private void SetAlpha(float alpha)
        {
            Color color = Image_Action.color;
            color.a = alpha;

            Image_Action.color = color;
        }

        private void SetScale(float multiplayer)
        {
            Image_Action.transform.localScale = Vector3.one * multiplayer;
        }

        private void SetVisited(bool isVisited)
        {
            Image_Visited.enabled = isVisited;
        }
    }
}
