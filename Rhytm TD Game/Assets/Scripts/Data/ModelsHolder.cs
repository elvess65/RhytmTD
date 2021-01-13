using RhytmTD.Data.DataBase;
using RhytmTD.Data.Models;

namespace RhytmTD.Data
{
    /// <summary>
    /// Holder for data related objects
    /// </summary>
    [System.Serializable]
    public class ModelsHolder
    {
        public DBProxy DBProxy;

        public DataTableModel DataTableModel;
        public AccountDataModel AccountModel;
        public BattleSessionModel BattleSessionModel;

        public ModelsHolder()
        {
            DBProxy = new DBProxy();
            BattleSessionModel = new BattleSessionModel();
        }
    }
}
