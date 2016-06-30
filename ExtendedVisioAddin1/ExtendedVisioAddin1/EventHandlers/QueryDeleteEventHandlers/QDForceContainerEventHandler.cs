using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    class QDForceContainerEventHandler : QueryDeleteEventHandler
    {
        public override void Execute(string rationallyType, RView view, Shape changedShape)
        {
            RComponent comp = view.Children.Find(x => x is ForcesContainer);
            if (comp is ForcesContainer)
            {
                comp.MsvSdContainerLocked = false;
            }
        }
    }
}
