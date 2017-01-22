using System.Linq;
using log4net;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Logger;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteForceEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            TempFileLogger.Log("Entered DeleteForceEventHandler.");
            //trace force row in view tree
            RationallyComponent forceComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);

            if (forceComponent is ForceContainer)
            {
                ForceContainer containerToDelete = (ForceContainer)forceComponent;
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    TempFileLogger.Log("Deleting all child components of the force container...");
                    containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c =>
                    {
                        c.Deleted = true;
                        c.RShape.Delete();
                    }); //schedule the missing delete events (children not selected during the manual delete)
                }

                ForcesContainer forcesContainer = (ForcesContainer)Globals.RationallyAddIn.View.Children.First(c => c is ForcesContainer);
                //update model
                int forceIndex = forcesContainer.Children.IndexOf(containerToDelete) - 1;
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    model.Forces.RemoveAt(forceIndex);
                    TempFileLogger.Log("Deleting force from model list of forces.");
                }
                //update view tree
                forcesContainer.Children.Remove(containerToDelete);
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    model.RegenerateForceIdentifiers();
                    TempFileLogger.Log("Regenerated force identifiers in model.");
                    forcesContainer.MsvSdContainerLocked = true;
                }

                RepaintHandler.Repaint(forcesContainer);
            }
        }
    }
}
