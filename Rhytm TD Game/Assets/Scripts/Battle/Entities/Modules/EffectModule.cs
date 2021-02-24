﻿
using RhytmTD.Data;
using System;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities
{
    public class EffectModule : IBattleModule
    {
        public int TypeID { get; }

        public Action<DataContainer> OnEffectAction;

        public EffectModule(int typeID)
        {
            TypeID = typeID;
        }

        public void EffectAction(DataContainer data)
        {
            OnEffectAction?.Invoke(data);
        }
    }
}
