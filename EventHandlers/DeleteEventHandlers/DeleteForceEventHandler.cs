using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteForceEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Log.Debug("Entered DeleteForceEventHandler.");
            //trace force row in view tree
            VisioShape forceComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);

            if (forceComponent is ForceContainer)
            {
                ForceContainer containerToDelete = (ForceContainer)forceComponent;
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    Log.Debug("Deleting all child components of the force container...");
                    containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c =>
                    {
                        c.Deleted = true;
                        c.Shape.Delete();
                    }); //schedule the missing delete events (children not selected during the manual delete)
                }

                ForcesContainer forcesContainer = (ForcesContainer)Globals.RationallyAddIn.View.Children.First(c => c is ForcesContainer);
                //update model
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    model.Forces.RemoveAll(force => force.Id == containerToDelete.Id);
                    Log.Debug("Deleting force from model list of forces.");
                }
                //update view tree
                forcesContainer.Children.Remove(containerToDelete);
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    model.RegenerateForceIdentifiers();
                    Log.Debug("Regenerated force identifiers in model.");
                    forcesContainer.MsvSdContainerLocked = true;
                }

                RepaintHandler.Repaint(forcesContainer);
            }
        }
    }
}
