using UnityEngine;

namespace RhytmTD.UI.Components
{
    /// <summary>
    /// Контролирует поведение бара
    /// </summary>
    public class UIComponent_Bar : MonoBehaviour
    {
        [SerializeField] private SimpleHealthBar HealthBar;

        public void UpdateBar(int cur, int max)
        {
            HealthBar.UpdateBar(cur, max);
        }
    }
}
