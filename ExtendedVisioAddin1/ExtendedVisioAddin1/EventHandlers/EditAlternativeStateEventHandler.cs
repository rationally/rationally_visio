using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExtendedVisioAddin1.Components;
using ExtendedVisioAddin1.Model;
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
                RationallyComponent c = new RationallyComponent(s);
                if (c.Type == "alternativeState")
                {
                    //todo get alternative by identifier
                    //TODO update alternative to new state (alternative.editStatusBox.selectedText)
                    //todo REPAINT
                }
            }
            
        }
    }
}
