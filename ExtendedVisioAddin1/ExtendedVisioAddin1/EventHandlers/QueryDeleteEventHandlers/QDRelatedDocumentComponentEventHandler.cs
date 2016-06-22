using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    class QDRelatedDocumentComponentEventHandler : QueryDeleteEventHandler
    {
        public override void Execute(string eventKey, RView view, Shape changedShape)
        {
           RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)view.Children.First(x => x is RelatedDocumentsContainer);
            foreach (RelatedDocumentContainer relatedDocumentContainer in relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().ToList())
            {
                if (relatedDocumentContainer.Children.Where(c => c.RShape.Equals(changedShape)).ToList().Count > 0) //check if this related document contains the to be deleted component
                {
                    if (!relatedDocumentContainer.Deleted)
                    {
                        relatedDocumentContainer.Deleted = true;
                        relatedDocumentContainer.RShape.Delete(); //delete the parent wrapper of s
                    }
                }
            }
        }
    }
}
