using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    class DeleteStakeholderEventHandler : IDeleteEventHandler
    {
        private const string DeleteUndoScope = "Delete stakeholder";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Log.Debug("Entered delete stakeholder event handler.");
            //store the rationally type of the last shape, which is responsible for ending the undo scope
            if (string.IsNullOrEmpty(Globals.RationallyAddIn.LastDelete) && (Globals.RationallyAddIn.StartedUndoState == 0) && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                Log.Debug("Starting undo scope.");
                Globals.RationallyAddIn.LastDelete = changedShape.Name;
                Globals.RationallyAddIn.StartedUndoState = Globals.RationallyAddIn.Application.BeginUndoScope(DeleteUndoScope);
            }

            //trace stakeholder container in view tree
            VisioShape stakeholderComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);

            StakeholderContainer delete = stakeholderComponent as StakeholderContainer;
            if (delete != null)
            {
                StakeholderContainer containerToDelete = delete;
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    Log.Debug("killing children of the stakeholder to kill");
                    containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c =>
                    {
                        c.Deleted = true;
                        c.Shape.DeleteEx((short)VisDeleteFlags.visDeleteNormal);
                    }); //schedule the missing delete events (children not selected during the manual delete)
                }
                StakeholdersContainer stakeholdersContainer = (StakeholdersContainer)Globals.RationallyAddIn.View.Children.First(c => c is StakeholdersContainer);
                //update model
                model.Stakeholders.RemoveAll(sth => sth.Id == containerToDelete.Id);
                Log.Debug("stakeholder removed from stakeholders container.");
                //update view tree
                stakeholdersContainer.Children.Remove(containerToDelete);

                model.RegenerateStakeholderIdentifiers();
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    stakeholdersContainer.MsvSdContainerLocked = true;
                }
                RepaintHandler.Repaint();
            }
        }
    }
}
