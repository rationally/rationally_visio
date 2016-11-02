﻿using Rationally.Visio.View;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal interface ITextChangedEventHandler
    {
        void Execute(RationallyView view, Shape changedShape);
    }
}
