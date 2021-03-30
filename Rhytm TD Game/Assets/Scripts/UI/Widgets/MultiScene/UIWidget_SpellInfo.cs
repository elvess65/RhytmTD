using CoreFramework.UI.Widget;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Widget
{
    /// <summary>
    /// Brief spell info
    /// </summary>
    public class UIWidget_SpellInfo : UIWidget
    {
        public System.Action OnButtonInfoPressHandler;

        [SerializeField] private Image Image_SpellIcon = null;
        [SerializeField] private Text Text_SpellName = null;
        [SerializeField] private UIWidget_Button UIWidget_ButtonInfo = null;

        public Image ExposedImageSpellIcon => Image_SpellIcon;


        public void Initialize(string spellName, Sprite sprite, Color color)
        {
            Text_SpellName.text = spellName;
            Image_SpellIcon.sprite = sprite;
            Image_SpellIcon.color = color;

            UIWidget_ButtonInfo.Initialize();
            UIWidget_ButtonInfo.OnWidgetPress += UIWidgetButtonInfoPressHandler;

            InternalInitialize();
        }

        public override void LockInput(bool isLocked)
        {
            UIWidget_ButtonInfo.LockInput(isLocked);
        }


        private void UIWidgetButtonInfoPressHandler()
        {
            OnButtonInfoPressHandler?.Invoke();
        }
    }
}
