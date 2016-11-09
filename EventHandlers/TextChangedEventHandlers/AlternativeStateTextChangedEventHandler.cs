﻿using Rationally.Visio.View;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class AlternativeStateTextChangedEventHandler : ITextChangedEventHandler
    {
        public void Execute(RationallyView view, Shape changedShape)
        {
            AlternativeStateComponent alternativeState = (AlternativeStateComponent)view.GetComponentByShape(changedShape);
            if (alternativeState == null) { return;}

            int index = alternativeState.AlternativeIndex;
            Globals.RationallyAddIn.Model.Alternatives[index].Status = alternativeState.Text;
        }
    }
}
