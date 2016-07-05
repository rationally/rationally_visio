using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.TextChangedEventHandlers
{
    internal class AlternativeStateTextChangedEventHandler : ITextChangedEventHandler
    {
        public void Execute(string eventKey, RView view, Shape changedShape)
        {
            AlternativeStateComponent alternativeState = (AlternativeStateComponent)view.GetComponentByShape(changedShape);
            int index = alternativeState.AlternativeIndex;
            Globals.ThisAddIn.Model.Alternatives[index].Status = alternativeState.Text;
        }
    }
}
