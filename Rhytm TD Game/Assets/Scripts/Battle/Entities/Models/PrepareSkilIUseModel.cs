using CoreFramework;

namespace RhytmTD.Battle.Entities.Models
{
    public class PrepareSkilIUseModel : BaseModel
    {
        public System.Action OnCorrectTouch;
        public System.Action OnWrongTouch;

        public System.Action<int> OnSpellReset;
        public System.Action OnAllSpellsReset;
        public System.Action<int> OnSpellNextTickInput;
        public System.Action<int> OnSpellNextTickAuto;
        public System.Action<int> OnSpellSelected;
    }
}
