using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            ForceContainer forceContainerToDelete = null;
            if (forceComponent is ForceContainer)
            {
                //model.Forces.RemoveAt(forceComponent.ForceIndex);
                forceContainerToDelete = (ForceContainer)forceComponent;
            } else if (forceComponent is ForceValueComponent || forceComponent is ForceDescriptionComponent || forceComponent is ForceConcernComponent) //changedShape is one of the child components of the whole forcerow
            {
                //locate forcecontainer of the subcomponent: select it from forcesContainer.Children, as the component that has a child equal to forceComponent
                forceContainerToDelete = forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().FirstOrDefault(fc => fc.Children.Any(fcc => fcc.Equals(forceComponent)));

            }

            //first, remove all the child shapes of the forcecontainer
            if (forceContainerToDelete != null)
            {
                //if (forceContainerToDelete.ForceIndex >= 0 && forceContainerToDelete.Equals(forceComponent))
                //{
                    //if (model.Forces.Count > forceContainerToDelete.ForceIndex)
                    //{
                        
                    //}
                //}

                List<RComponent> toRemove = new List<RComponent>();
                ((ForceContainer) forceContainerToDelete).Children.ForEach(c => toRemove.Add(c)); //store for deletion later
                ((ForceContainer) forceContainerToDelete).Children.ForEach(c => Globals.ThisAddIn.View.DeleteFromTree(c.RShape)); //only deletes component from tree
                toRemove.ForEach(tr => tr.DeleteShape(false));

                //remove the container itself
                Globals.ThisAddIn.View.DeleteFromTree(forceContainerToDelete.RShape);
                forceContainerToDelete.DeleteShape(false); //tricky...
            }
            //new RepaintHandler(forcesContainer);
        }
    }
}
