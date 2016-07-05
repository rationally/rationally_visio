using System.Linq;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDRelatedDocumentComponentEventHandler : IQueryDeleteEventHandler
    {
        public void Execute(string eventKey, RView view, Shape changedShape)
        {
            RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)view.Children.First(x => x is RelatedDocumentsContainer);
            foreach (RelatedDocumentContainer relatedDocumentContainer in relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().ToList())
            {
                if (relatedDocumentContainer.Children.Where(c => c.RShape.Equals(changedShape)).ToList().Count > 0 && !relatedDocumentContainer.Deleted) //check if this related document contains the to be deleted component and is not already deleted
                {
                    relatedDocumentContainer.Deleted = true;
                    relatedDocumentContainer.RShape.Delete(); //delete the parent wrapper of s
                }
            }
        }
    }
}
