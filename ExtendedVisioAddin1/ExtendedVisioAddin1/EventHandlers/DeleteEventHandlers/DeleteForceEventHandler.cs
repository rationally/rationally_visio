using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.DeleteEventHandlers
{
    internal class DeleteForceEventHandler : DeleteEventHandler
    {
        public override void Execute(string eventKey, RModel model, Shape changedShape)
        {
            //NOTE: this eventhandler is ment to be called while the changedShape is not completely deleted. Preferrable from ShapeDeleted eventhandler.

            //trace force row in view tree
            RComponent forceComponent = Globals.ThisAddIn.View.GetComponentByShape(changedShape);

            if (forceComponent is ForceContainer)
            {
                ForceContainer containerToDelete = (ForceContainer)forceComponent;
                containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c => { c.Deleted = true; c.RShape.Delete(); });//schedule the missing delete events (children not selected during the manual delete)

                ForcesContainer forcesContainer = (ForcesContainer)Globals.ThisAddIn.View.Children.First(c => c is ForcesContainer);
                //update model
                int forceIndex = forcesContainer.Children.IndexOf(containerToDelete) - 1;
                model.Forces.RemoveAt(forceIndex);
                //update view tree
                forcesContainer.Children.Remove(containerToDelete);
                new RepaintHandler(forcesContainer);
            }
        }
    }
}
