using CoreFramework;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class SpellBookController : BaseController
    {
        private SpellBookModel m_SpellBookModel;
        private MoveModel m_MoveModel;

        public SpellBookController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_SpellBookModel = Dispatcher.GetModel<SpellBookModel>();
            m_MoveModel = Dispatcher.GetModel<MoveModel>();
        }

        public void OpenSpellBook()
        {
            m_MoveModel.MoveSpeedMultiplayer = 0.1f;
            m_SpellBookModel.SpellBookOpened();
        }

        public void CloseSpellBook()
        {
            m_MoveModel.MoveSpeedMultiplayer = 1f;
            m_SpellBookModel.SpellbookClosed();
        }

        public void SelectDirectionalSpell()
        {
            m_SpellBookModel.DirectionalSpellSelected();
        }

        public void UseSpellBook()
        {
            m_SpellBookModel.SpellbookUsed();
        }

        public void SpellbookPostUsed()
        {
            m_SpellBookModel.SpellbookPostUsed();
        }
    }
}
