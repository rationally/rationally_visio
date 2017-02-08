using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Documents;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDRelatedDocumentComponentEventHandler : IQueryDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)view.Children.First(x => x is RelatedDocumentsContainer);
            foreach (RelatedDocumentContainer relatedDocumentContainer in relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().ToList())
            {
                if ((relatedDocumentContainer.Children.Where(c => c.RShape.Equals(changedShape)).ToList().Count > 0) && !relatedDocumentContainer.Deleted) //check if this related document contains the to be deleted component and is not already deleted
                {
                    relatedDocumentContainer.Deleted = true;
                    relatedDocumentContainer.RShape.Delete(); //delete the parent wrapper of s
                }
            }
        }
    }
}
