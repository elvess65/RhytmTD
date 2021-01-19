﻿using RhytmTD.Rhytm;

namespace RhytmTD.Battle.Core
{
    /// <summary>
    /// Holder for all controllers
    /// </summary>
    public class ControllersHolder
    {
        public RhytmController RhytmController { get; private set; }
        public InputController InputController { get; private set; }

        public ControllersHolder()
        {
            RhytmController = new RhytmController();
        }
    }
}
