using System.Linq;
using Rationally.Visio.Model;
using Rationally.Visio.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    public class DeletedRelatedUrlUrlEventHandler : IDeleteEventHandler
    {
        public void Execute(string eventKey, RModel model, Shape changedShape)
        {
            RelatedDocumentsContainer relatedDocumentsContainer = Globals.ThisAddIn.View.Children.FirstOrDefault(c => c is RelatedDocumentsContainer) as RelatedDocumentsContainer;

            if (relatedDocumentsContainer != null)
            {
                foreach (RelatedDocumentContainer relatedDocumentContainer in relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().ToList())
                {
                    relatedDocumentContainer.Children.RemoveAll(c => c.RShape.Equals(changedShape)); //Remove the component from the tree
                }
            }
        }
    }
}
