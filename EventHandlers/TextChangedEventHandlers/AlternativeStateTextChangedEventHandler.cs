﻿using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class AlternativeStateTextChangedEventHandler : ITextChangedEventHandler
    {
        public void Execute(string eventKey, RationallyView view, Shape changedShape)
        {
            AlternativeStateComponent alternativeState = (AlternativeStateComponent)view.GetComponentByShape(changedShape);
            int index = alternativeState.AlternativeIndex;
            Globals.RationallyAddIn.Model.Alternatives[index].Status = alternativeState.Text;
        }
    }
}