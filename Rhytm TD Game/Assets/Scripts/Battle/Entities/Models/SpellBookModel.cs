using CoreFramework;
using System;

namespace RhytmTD.Battle.Entities.Models
{
    public class SpellBookModel : BaseModel
    {
        /// <summary>
        /// Spellbook is opened and input mode is changed
        /// </summary>
        public event Action OnSpellbookOpened;

        /// <summary>
        /// Spellbook closed by close button
        /// </summary>
        public event Action OnSpellbookClosed;

        /// <summary>
        /// Spell was selected as waiting for direction choose
        /// </summary>
        public event Action OnDirectionalSpellSelected;

        /// <summary>
        /// Spell sequence aplied and spell animation started to play
        /// </summary>
        public event Action OnSpellbookUsed;

        /// <summary>
        /// Spell animation reached moment of creating effect 
        /// </summary>
        public event Action OnSpellbookPostUsed;

        public float SpeedMultiplayer = 1;

        public void SpellBookOpened()
        {
            OnSpellbookOpened?.Invoke();
        }

        public void SpellbookClosed()
        {
            OnSpellbookClosed?.Invoke();
        }

        public void DirectionalSpellSelected()
        {
            OnDirectionalSpellSelected?.Invoke();
        }

        public void SpellbookUsed()
        {
            OnSpellbookUsed?.Invoke();
        }

        public void SpellbookPostUsed()
        {
            OnSpellbookPostUsed?.Invoke();
        }
    }
}
