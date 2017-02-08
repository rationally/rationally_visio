using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Documents;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteRelatedDocumentEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Log.Debug("Entered DeleteRelatedDocumentEventHandler.");
            //trace documents container in view tree
            RationallyComponent documentComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);

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
                /*if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    
                }*/
                Log.Debug("Document being removed from model list...");
                model.Documents.RemoveAt(docIndex);
                //update view tree
                relatedDocumentsContainer.Children.Remove(containerToDelete);

                
                Log.Debug("Regenerated identifiers of document list in model.");

                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    model.RegenerateDocumentIdentifiers();
                    relatedDocumentsContainer.MsvSdContainerLocked = true;
                }
                
                RepaintHandler.Repaint(relatedDocumentsContainer);
            }
        }
    }
}
