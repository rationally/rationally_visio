using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class AddAlternativeEventHandler : EventHandler
    {
        public AddAlternativeEventHandler(RModel model)
        {
            Selection selectedComponents = Globals.ThisAddIn.Application.ActiveWindow.Selection;
            foreach (Shape s in selectedComponents)
            {
                RationallyComponent c = new RationallyComponent(s);
                if (c.Type == "alternatives")
                {
                    //model.RationallyDocument.Masters[""];
                }
                //TODO remove lock msvSDContainerLocked
            }
        }
    }
}
