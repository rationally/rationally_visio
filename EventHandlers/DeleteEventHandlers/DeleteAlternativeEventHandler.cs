using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    
    internal class DeleteAlternativeEventHandler : IDeleteEventHandler
    {
        private const string DeleteUndoScope = "Delete alternative";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Log.Debug("Entered delete alternative event handler.");
            //store the rationally type of the last shape, which is responsible for ending the undo scope
            if (string.IsNullOrEmpty(Globals.RationallyAddIn.LastDelete) && (Globals.RationallyAddIn.StartedUndoState == 0) && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                Log.Debug("Starting undo scope.");
                Globals.RationallyAddIn.LastDelete = changedShape.Name;
                Globals.RationallyAddIn.StartedUndoState = Globals.RationallyAddIn.Application.BeginUndoScope(DeleteUndoScope);
            }

            //trace alternative container in view tree
            VisioShape alternativeComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);

            AlternativeShape delete = alternativeComponent as AlternativeShape;
            if (delete != null)
            {
                model.Forces.ForEach(force => force.ForceValueDictionary.Remove(delete.Id));
                AlternativeShape containerToDelete = delete;
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    Log.Debug("deleting children of the alternative to delete");
                    containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c =>
                    {
                        c.Deleted = true;
                        c.Shape.DeleteEx((short)VisDeleteFlags.visDeleteNormal);
                    }); //schedule the missing delete events (children not selected during the manual delete)
                }
                AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.RationallyAddIn.View.Children.First(c => c is AlternativesContainer);
                //update model
                model.Alternatives.RemoveAll(a => a.Id == containerToDelete.Id);
                Log.Debug("Alternative removed from alternatives container.");
                //update view tree
                alternativesContainer.Children.Remove(containerToDelete);

                model.RegenerateAlternativeIdentifiers();
                Log.Debug("Identifiers regenerated of alternatives.");
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    alternativesContainer.MsvSdContainerLocked = true;
                }
                RepaintHandler.Repaint(); 
            }
        }
    }
}
