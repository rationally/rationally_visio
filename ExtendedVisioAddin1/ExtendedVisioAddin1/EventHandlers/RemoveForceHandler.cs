using System.Collections.Generic;
using System.Linq;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    class RemoveForceHandler : MarkerEventHandler
    {
        public override void Execute(RModel model, Shape changedShape, string identifier)
        {
            ForcesContainer forcesContainer = (ForcesContainer)Globals.ThisAddIn.View.Children.First(c => c is ForcesContainer);
            //trace force row in view tree
            RComponent forceComponent = Globals.ThisAddIn.View.GetComponentByShape(changedShape);
            if (forceComponent is ForceContainer)
            {
                //first, remove all the child shapes of the forcecontainer
                List<Shape> toRemove = new List<Shape>();
                ((ForceContainer)forceComponent).Children.ForEach(c => toRemove.Add(c.RShape));//store for deletion later
                ((ForceContainer)forceComponent).Children.ForEach(c => Globals.ThisAddIn.View.DeleteFromTree(c.RShape));//only deletes component from tree
                toRemove.ForEach(tr => tr.DeleteEx(0));
            }
            else //changedShape is one of the child components of the whole forcerow
            {
                
            }
            //remove the container itself
            Globals.ThisAddIn.View.DeleteFromTree(changedShape);
            new RepaintHandler(forcesContainer);
        }
    }
}
