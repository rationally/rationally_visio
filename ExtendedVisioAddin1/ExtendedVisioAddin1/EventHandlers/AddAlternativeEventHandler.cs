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
            foreach (IVShape s in selectedComponents)
            {
                RationallyComponent c = new RationallyComponent(s);
                if (c.Type == "alternatives")//TODO might be redundant
                {
                    model.Alternatives.Add(new Alternative("alt title","status","desc"));
                    model.Alternatives.Last().AddTo(s,0);
                    //model.RationallyDocument.Masters[""];
                }
                //TODO remove lock msvSDContainerLocked
            }
        }
    }
}
