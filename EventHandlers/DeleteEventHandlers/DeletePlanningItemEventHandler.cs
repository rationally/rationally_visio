using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Planning;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeletePlanningItemEventHandler : IDeleteEventHandler
    {
        private const string DeleteUndoScope = "Delete planningitem";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Log.Debug("Entered delete planningitem event handler.");
            //store the rationally type of the last shape, which is responsible for ending the undo scope
            if (string.IsNullOrEmpty(Globals.RationallyAddIn.LastDelete) && (Globals.RationallyAddIn.StartedUndoState == 0) && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                Log.Debug("Starting undo scope.");
                Globals.RationallyAddIn.LastDelete = changedShape.Name;
                Globals.RationallyAddIn.StartedUndoState = Globals.RationallyAddIn.Application.BeginUndoScope(DeleteUndoScope);
            }

            //trace planning item container in view tree
            VisioShape planningComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);

            PlanningItemComponent delete = planningComponent as PlanningItemComponent;
            if (delete != null)
            {
                PlanningItemComponent containerToDelete = delete;
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    Log.Debug("deleting children of the planning item to delete");
                    containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c =>
                    {
                        c.Deleted = true;
                        c.Shape.DeleteEx((short)VisDeleteFlags.visDeleteNormal);
                    }); //schedule the missing delete events (children not selected during the manual delete)
                }
                PlanningContainer planningContainer = (PlanningContainer)Globals.RationallyAddIn.View.Children.First(c => c is PlanningContainer);
                //update model
                model.PlanningItems.RemoveAll(p => p.Id == containerToDelete.Id);
                Log.Debug("Planning item removed from planning container.");
                //update view tree
                planningContainer.Children.Remove(containerToDelete);

                model.RegeneratePlanningIdentifiers();//one index no longer exists, so let the view adapt to the new index range

                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    planningContainer.MsvSdContainerLocked = true;
                }
                RepaintHandler.Repaint();
            }
        }
    }
}
