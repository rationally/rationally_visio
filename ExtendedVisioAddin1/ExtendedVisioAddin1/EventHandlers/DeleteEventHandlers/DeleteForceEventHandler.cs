using System.Linq;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteForceEventHandler : IDeleteEventHandler
    {
        public void Execute(string eventKey, RModel model, Shape changedShape)
        {
            //trace force row in view tree
            RComponent forceComponent = Globals.ThisAddIn.View.GetComponentByShape(changedShape);

            if (forceComponent is ForceContainer)
            {
                ForceContainer containerToDelete = (ForceContainer)forceComponent;
                if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                {
                    containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c =>
                    {
                        c.Deleted = true;
                        c.RShape.Delete();
                    }); //schedule the missing delete events (children not selected during the manual delete)
                }

                ForcesContainer forcesContainer = (ForcesContainer)Globals.ThisAddIn.View.Children.First(c => c is ForcesContainer);
                //update model
                int forceIndex = forcesContainer.Children.IndexOf(containerToDelete) - 1;
                if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                {
                    model.Forces.RemoveAt(forceIndex);
                }
                //update view tree
                forcesContainer.Children.Remove(containerToDelete);
                if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                {
                    model.RegenerateForceIdentifiers();
                    forcesContainer.MsvSdContainerLocked = true;
                }

                new RepaintHandler(forcesContainer);
            }
        }
    }
}
