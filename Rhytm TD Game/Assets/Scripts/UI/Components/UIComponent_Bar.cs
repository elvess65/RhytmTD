using RhytmTD.Persistant.Abstract;
using UnityEngine;

namespace RhytmTD.UI.Components
{
    /// <summary>
    /// Контролирует поведение бара
    /// </summary>
    public class UIComponent_Bar : MonoBehaviour, iUpdatable
    {
        [SerializeField] private SimpleHealthBar HealthBar;

        public void PerformUpdate(float deltaTime)
        {
            
        }

        public void UpdateBar(int cur, int max)
        {
            HealthBar.UpdateBar(cur, max);
        }
    }
}
