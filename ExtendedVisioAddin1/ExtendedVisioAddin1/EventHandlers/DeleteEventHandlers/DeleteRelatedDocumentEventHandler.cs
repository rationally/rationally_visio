
using System.Linq;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.DeleteEventHandlers
{
    internal class DeleteRelatedDocumentEventHandler : IDeleteEventHandler
    {
        public void Execute(string eventKey, RModel model, Shape changedShape)
        {
            //NOTE: this eventhandler is ment to be called while the changedShape is not completely deleted. Preferrable from ShapeDeleted eventhandler.

            //trace force row in view tree
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
