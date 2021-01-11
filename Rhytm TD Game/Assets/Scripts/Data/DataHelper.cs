using RhytmTD.Persistant;
using static RhytmTD.Data.Models.AccountModel;

namespace RhytmTD.Data
{
    /// <summary>
    /// Помошник для получения данных из разных источников
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// Текущий урон персонажа
        /// </summary>
        public static (int, int) GetCharacterDamage(int characterID)
        {
            //Данные о персонаже
            //CharacterData characterData = GetCharacterData(characterID);

            int WeaponExperiance = 10;

            //Уровень оружия
            int weaponLevel = GameManager.Instance.ModelsHolder.DataTableModel.LevelingDataModel.
                              GetWeaponLevelByExp(characterID, WeaponExperiance);

            //Урон
            (int, int) weaponDamage = GameManager.Instance.ModelsHolder.DataTableModel.LevelingDataModel.
                                      GetWeaponDamage(characterID, weaponLevel);

            return weaponDamage;
        }

        /// <summary>
        /// Текущая стоимость единицы опыта улучшения оружия персонажа
        /// </summary>
        public static float GetWeaponExperiancePointPrice(int characterID)
        {
            //Данные о персонаже
            //CharacterData characterData = GetCharacterData(characterID);

            int WeaponExperiance = 10;

            //Уровень оружия
            int weaponLevel = GameManager.Instance.ModelsHolder.DataTableModel.LevelingDataModel.
                              GetWeaponLevelByExp(characterID, WeaponExperiance);

            //Цена единицы опыта
            float price = GameManager.Instance.ModelsHolder.DataTableModel.LevelingDataModel.
                          GetExperiancePointPrice(characterID, weaponLevel);

            return price;
        }
    }
}
