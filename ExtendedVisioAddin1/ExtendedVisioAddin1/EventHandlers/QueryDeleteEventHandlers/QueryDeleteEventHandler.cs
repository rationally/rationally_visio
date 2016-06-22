﻿using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    internal abstract class QueryDeleteEventHandler
    {
        public abstract void Execute(string eventKey, RView view, Shape changedShape);
    }
}
