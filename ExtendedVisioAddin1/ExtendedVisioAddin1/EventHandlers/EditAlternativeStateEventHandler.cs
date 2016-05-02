using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExtendedVisioAddin1.Components;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    class EditAlternativeStateEventHandler : EventHandler
    {
        public EditAlternativeStateEventHandler(RModel model, string newState)
        {
            Selection selectedComponents = Globals.ThisAddIn.Application.ActiveWindow.Selection;
            foreach (IVShape s in selectedComponents)
            {
                RComponent c = new RComponent(Globals.ThisAddIn.Application.ActivePage);
                c.RShape = s;
                if (c.Type == "alternativeState")
                {
                    //var x = "DebugVar";
                    //todo get alternative by identifier
                    //TODO update alternative to new state (alternative.editStatusBox.selectedText)
                    //todo REPAINT
                }
            }
            
        }
    }
}
