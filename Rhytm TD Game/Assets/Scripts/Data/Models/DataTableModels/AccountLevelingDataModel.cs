namespace RhytmTD.Data.Models.DataTableModels
{
    /// <summary>
    /// Информация об опыте и уровнях аккаунта
    /// </summary>
    public class AccountLevelingDataModel : DeserializableDataModel<AccountLevelingDataModel>
    {
        /// Используется только для парсинга данных
        public int Damage;
        public int HP;
        public int Mana;
        public LevelingProgressionConfig TestLevelProgression;
        public ProgressionConfig TestPriceProgression;
        public ProgressionConfig TestValueProgression;
        public MinMaxProgressionConfig TestMinMaxProgression;

        public override void ReorganizeData()
        {
            UnityEngine.Debug.Log($"Damage {Damage}. HP {HP}. Mana {Mana}.");
            
            for (int i = 0; i <= TestLevelProgression.TotalLevels; i++)
            {
                int realLevel = i;

                float progress = TestLevelProgression.EvaluateProgress01(realLevel);

                UnityEngine.Debug.Log($"LEVEL: {realLevel} ({progress})");

                if (realLevel != 0)
                    UnityEngine.Debug.Log($" - Exp for lvl {realLevel} - {TestLevelProgression.EvaluateExpForLevel(realLevel)}");

                int price = (int)(TestPriceProgression.BaseValue + TestPriceProgression.BaseValue * TestPriceProgression.Evaluate(progress));
                UnityEngine.Debug.Log($" - Price {price}");

                int value = (int)(TestValueProgression.BaseValue + TestValueProgression.BaseValue * TestValueProgression.Evaluate(progress));
                UnityEngine.Debug.Log($" - HP {value}");

                (int min, int max) damage = TestMinMaxProgression.EvaluateInt(progress);
                UnityEngine.Debug.Log($" - Damage {damage.min} - {damage.max}");
            }
        }
    }
}
