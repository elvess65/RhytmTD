using CoreFramework;

namespace RhytmTD.Battle.Entities.Models
{
    public class PlayerRhytmInputHandleModel : BaseModel
    {
        public System.Action<int> OnDDRPInputCounterChanged;

        private int m_CorrectDDRPInputsCounter = 0;

        public int CorrectInputsCounter
        {
            get { return m_CorrectDDRPInputsCounter; }
            set
            {
                m_CorrectDDRPInputsCounter = value;
                OnDDRPInputCounterChanged?.Invoke(m_CorrectDDRPInputsCounter);
            }
        }
    }
}
