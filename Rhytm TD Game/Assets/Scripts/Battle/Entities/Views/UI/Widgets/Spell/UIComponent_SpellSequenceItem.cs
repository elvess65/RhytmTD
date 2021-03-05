using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Components
{
    public class UIComponent_SpellSequenceItem : MonoBehaviour
    {
        public enum ItemStates { Active, Reseted, Visited }

        [SerializeField] private Image Image_Action;

        public void Initialize(bool isAction)
        {
            Image_Action.color = isAction ? Color.green : Color.red;
            SetState(ItemStates.Active);
        }

        public void SetState(ItemStates state)
        {
            switch(state)
            {
                case ItemStates.Active:

                    SetScale(1);
                    SetAlpha(1);
                    

                    break;
                case ItemStates.Reseted:

                    SetAlpha(0.5f);

                    break;
                case ItemStates.Visited:

                    SetScale(1.5f);

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
    }
}
