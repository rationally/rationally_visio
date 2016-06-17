using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    class QDForceContainerEventHandler : QueryDeleteEventHandler
    {
        public override void Execute(string rationallyType, RView view, Shape changedShape)
        {
            //nothing special needs to happen here, except optionally starting an undo scope
            if (Globals.ThisAddIn.StartedUndoState == 0)
            {
                Globals.ThisAddIn.StartedUndoState = Globals.ThisAddIn.Application.BeginUndoScope("scope");
            }
        }
    }
}
