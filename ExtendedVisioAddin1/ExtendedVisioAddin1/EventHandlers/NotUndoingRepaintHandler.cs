using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    class NotUndoingRepaintHandler : MarkerEventHandler
    {
        public override void Execute(RModel model, Shape changedShape, string identifier)
        {
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                //Thread.Sleep(1000);
                //new RepaintHandler();
            }
        }
    }
}
