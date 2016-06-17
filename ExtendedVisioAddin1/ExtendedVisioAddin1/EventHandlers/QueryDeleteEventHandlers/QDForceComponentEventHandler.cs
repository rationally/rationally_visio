using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDForceComponentEventHandler : QueryDeleteEventHandler
    {
        public override void Execute(string eventKey, RView view, Shape changedShape)
        {
            //create an undo scope, if we are not already in one that was created
            if (Globals.ThisAddIn.StartedUndoState == 0)
            {
                Globals.ThisAddIn.StartedUndoState = Globals.ThisAddIn.Application.BeginUndoScope("scope");
            }

            ForcesContainer forcesContainer = (ForcesContainer) view.Children.First(c => c is ForcesContainer);
            foreach (ForceContainer forceContainer in forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList()) //find all candidate containers
            {
                if (forceContainer.Children.Where(c => c.RShape.Equals(changedShape)).ToList().Count > 0)//find the right container of changedShape
                {
                    if (!forceContainer.Deleted) //the container was not part of the selection at the querycancel shapedelete event
                    {
                        forceContainer.Deleted = true;
                        forceContainer.RShape.Delete();
                    }
                }
            }
        }
    }
}
