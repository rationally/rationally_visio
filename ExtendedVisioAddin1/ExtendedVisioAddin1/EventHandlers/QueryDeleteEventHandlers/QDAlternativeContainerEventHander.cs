using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDAlternativeContainerEventHander : QueryDeleteEventHandler
    {
        public override void Execute(string eventKey, RView view, Shape changedShape)
        {
        }
    }
}
