using CoreFramework;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class SpellBookController : BaseController
    {
        private SpellBookModel m_SpellBookModel;

        public SpellBookController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_SpellBookModel = Dispatcher.GetModel<SpellBookModel>();
        }

        public void OpenSpellBook()
        {
            m_SpellBookModel.SpeedMultiplayer = ConstsCollection.SPELLBOOK_SPEED_MULTIPLAYER;
            m_SpellBookModel.SpellBookOpened();
        }

        public void CloseSpellBook()
        {
            m_SpellBookModel.SpeedMultiplayer = 1f;
            m_SpellBookModel.SpellbookClosed();
        }

        public void SelectDirectionalSpell()
        {
            m_SpellBookModel.DirectionalSpellSelected();
        }

        public void UseSpellBook()
        {
            m_SpellBookModel.SpeedMultiplayer = 1f;
            m_SpellBookModel.SpellbookUsed();
        }

        public void SpellbookPostUsed()
        {
            m_SpellBookModel.SpellbookPostUsed();
        }
    }
}
