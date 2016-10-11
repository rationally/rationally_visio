﻿using System.Linq;
using log4net;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    
    internal class DeleteAlternativeEventHandler : IDeleteEventHandler
    {
        private const string DeleteUndoScope = "Delete alternative";
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(string eventKey, RModel model, Shape changedShape)
        {
            Log.Debug("Entered delete alternative event handler.");
            //store the rationally type of the last shape, which is responsible for ending the undo scope
            if (string.IsNullOrEmpty(Globals.RationallyAddIn.LastDelete) && Globals.RationallyAddIn.StartedUndoState == 0 && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                Log.Debug("Starting undo scope.");
                Globals.RationallyAddIn.LastDelete = changedShape.Name;
                Globals.RationallyAddIn.StartedUndoState = Globals.RationallyAddIn.Application.BeginUndoScope(DeleteUndoScope);
            }

            //trace alternative container in view tree
            RComponent alternativeComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);

            AlternativeContainer delete = alternativeComponent as AlternativeContainer;
            if (delete != null)
            {
                AlternativeContainer containerToDelete = delete;
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    Log.Debug("deleting children of the alternative to delete");
                    containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c =>
                    {
                        c.Deleted = true;
                        c.RShape.DeleteEx((short)VisDeleteFlags.visDeleteNormal);
                    }); //schedule the missing delete events (children not selected during the manual delete)
                }
                AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.RationallyAddIn.View.Children.First(c => c is AlternativesContainer);
                //update model
                model.Alternatives.RemoveAll(a => a.TimelessId == containerToDelete.TimelessId);
                Log.Debug("Alternative removed from alternatives container.");
                //update view tree
                alternativesContainer.Children.Remove(containerToDelete);

                model.RegenerateAlternativeIdentifiers();
                Log.Debug("Identifiers regenerated of alternatives.");
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    alternativesContainer.MsvSdContainerLocked = true;
                }
                
                new RepaintHandler();//requires forces to repaint as well!  //TODO: Code smell. Should be RepaintHandler.requestRepaint() or repaint().   
            }
        }
    }
}
