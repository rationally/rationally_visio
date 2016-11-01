﻿using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal interface IDeleteEventHandler
    {
        void Execute(string eventKey, RationallyModel model, Shape changedShape);
    }
}
