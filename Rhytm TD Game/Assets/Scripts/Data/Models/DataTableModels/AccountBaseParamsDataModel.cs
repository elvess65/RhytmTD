using CoreFramework;

namespace RhytmTD.Data.Models.DataTableModels
{
    /// <summary>
    /// Базовые параметры аккаунта (без прокачки)
    /// </summary>
    public class AccountBaseParamsDataModel : BaseModel
    {
        public float MoveSpeedUnitsPerTick;
        public float FocusSpeed;
        public int MinDamage;
        public int MaxDamage;
        public int Health;
        public int Mana;
    }
}
