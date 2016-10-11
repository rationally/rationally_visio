using System.Linq;
using log4net;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteRelatedDocumentEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(string eventKey, RModel model, Shape changedShape)
        {
            Log.Debug("Entered DeleteRelatedDocumentEventHandler.");
            //trace documents container in view tree
            RComponent documentComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);

            if (documentComponent is RelatedDocumentContainer)
            {
                RelatedDocumentContainer containerToDelete = (RelatedDocumentContainer)documentComponent;
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    Log.Debug("Deleting child shapes of related document...");
                    containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c =>
                    {
                        c.Deleted = true;
                        c.RShape.Delete();
                    }); //schedule the missing delete events (children not selected during the manual delete)
                }

                RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)Globals.RationallyAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
                //update model
                int docIndex = containerToDelete.DocumentIndex;
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    Log.Debug("Document being removed from model list...");
                    model.Documents.RemoveAt(docIndex);
                }
                //update view tree
                relatedDocumentsContainer.Children.Remove(containerToDelete);

                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    model.RegenerateDocumentIdentifiers();
                    Log.Debug("Regenerated identifiers of document list in model.");
                    relatedDocumentsContainer.MsvSdContainerLocked = true;
                }
                
                RepaintHandler.Repaint(relatedDocumentsContainer);
            }
        }
    }
}
