using CoreFramework;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Models
{
    public class RhytmDynamicModel : BaseModel
    {
        private RhytmDynamic m_Dynamic = RhytmDynamic.x1;

        public System.Action<RhytmDynamic> OnDynamicChanged;

        public RhytmDynamic Dynamic
        {
            get => m_Dynamic;
            set
            {
                m_Dynamic = value;
                OnDynamicChanged?.Invoke(m_Dynamic);
            }
        }
    }
}
