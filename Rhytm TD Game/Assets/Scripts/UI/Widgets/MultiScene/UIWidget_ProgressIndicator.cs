using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Widget
{
    /// <summary>
    /// Widget that show progress state
    /// </summary>
    public class UIWidget_ProgressIndicator : UIWidget
    {
        [Header("Cooldown")]
        [SerializeField] private Image Image_ProgressFG = null;

        public void SetProgress(float progress)
        {
            Image_ProgressFG.fillAmount = progress;
        }
    }
}
