using RhytmTD.UI.Widget;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Components.Spell
{
    public class UIComponent_SpellInfo : MonoBehaviour
    {
        public System.Action OnButtonInfoPressHandler;

        [SerializeField] private Text Text_SpellName;
        [SerializeField] private Image Image_SpellIcon;
        [SerializeField] private UIWidget_Button UIWidget_ButtonInfo;

        public void Initialize(string spellName)
        {
            Text_SpellName.text = spellName;

            UIWidget_ButtonInfo.Initialize();
            UIWidget_ButtonInfo.OnWidgetPress += UIWidgetButtonInfoPressHandler;
        }

        private void UIWidgetButtonInfoPressHandler()
        {
            OnButtonInfoPressHandler?.Invoke();
        }
    }
}
