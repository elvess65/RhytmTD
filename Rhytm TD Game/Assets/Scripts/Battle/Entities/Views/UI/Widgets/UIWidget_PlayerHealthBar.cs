using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Widget
{
    public class UIWidget_PlayerHealthBar : UIWidget
    {
        [Space]

        [SerializeField] private Image Foreground;

        public void Initialize()
        {
            InternalInitialize();
        }

        public void UpdateHealthBar(int currentHeath, int health)
        {
            Foreground.fillAmount = currentHeath / (float)health;
        }
    }
}
