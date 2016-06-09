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

            if (!forceComponent.Deleted) //happens when the menu option 'delete force' is called on the container
            {


                if (forceComponent is ForceContainer) //remove the children first, but note that handlers are async
                {
                    ((ForceContainer) forceComponent).Children[0].RShape.Delete();
                }
                else
                {
                    forceComponent.RShape.Delete();
                }
                return;
            }

            //determine relevant force container
            ForceContainer forceContainer = null;
            if (forceComponent is ForceContainer)
            {
                forceContainer = forceComponent as ForceContainer;
            }
            else
            {
                forceContainer = forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().First(fc => fc.Children.Any(c => c.Equals(forceComponent)));
            }

            //find out the first child that is not deleted
            RComponent nextToDelete = forceContainer.Children.FirstOrDefault(c => !c.Deleted);
            if (nextToDelete != null && !forceContainer.Deleted) //second part: the container, on deletion, schedules deletes for the children, so let those handle the deletion
            {
                nextToDelete.RShape.Delete();
            }
            else if (nextToDelete == null)
            {
                if (forceContainer.Deleted) //all children, plus the container are now deleted: done!
                {
                    int forceIndex = forcesContainer.Children.IndexOf(forceContainer) - 1;
                    forcesContainer.Children.Remove(forceContainer);
                    model.Forces.RemoveAt(forceIndex);
                    new RepaintHandler();
                }
                else
                {
                    forceContainer.RShape.Delete();
                }
            }
        }
    }
}
