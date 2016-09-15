using System.Linq;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteRelatedDocumentEventHandler : IDeleteEventHandler
    {
        public void Execute(string eventKey, RModel model, Shape changedShape)
        {
            //trace documents container in view tree
            RComponent documentComponent = Globals.ThisAddIn.View.GetComponentByShape(changedShape);

            if (documentComponent is RelatedDocumentContainer)
            {
                RelatedDocumentContainer containerToDelete = (RelatedDocumentContainer)documentComponent;
                if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                {
                    containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c =>
                    {
                        c.Deleted = true;
                        c.RShape.Delete();
                    }); //schedule the missing delete events (children not selected during the manual delete)
                }

                RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)Globals.ThisAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
                //update model
                int docIndex = containerToDelete.DocumentIndex;
                if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                {
                    model.Documents.RemoveAt(docIndex);
                }
                //update view tree
                relatedDocumentsContainer.Children.Remove(containerToDelete);

                if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                {
                    model.RegenerateDocumentIdentifiers();
                    relatedDocumentsContainer.MsvSdContainerLocked = true;
                }
                
                new RepaintHandler(relatedDocumentsContainer);
            }
        }
    }
}
