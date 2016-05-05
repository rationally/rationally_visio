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
    class RemoveAlternativeEventHandler : EventHandler
    {
        public RemoveAlternativeEventHandler(RModel model)
        {
            Selection selectedComponents = Globals.ThisAddIn.Application.ActiveWindow.Selection;
            foreach (Shape s in selectedComponents)
            {
                RComponent c = new RComponent(Globals.ThisAddIn.Application.ActivePage);
                c.RShape = s;
                if (c.Type == "alternative")
                {
                    //todo get alternative by identifier
                    DialogResult confirmResult = MessageBox.Show("Are you sure you want to delete the " + "ALTERNATIVE NAME", "Confirm Deletion", MessageBoxButtons.YesNo); //todo alternative name
                    if (confirmResult == DialogResult.Yes)
                    {
                        //todo: remove alternative from list
                        //todo repaint
                    }
                }
            }
        }
    }
}
