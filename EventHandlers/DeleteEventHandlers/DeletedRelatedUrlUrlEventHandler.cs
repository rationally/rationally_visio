using System.Linq;
using log4net;
using Rationally.Visio.Model;
using Rationally.Visio.View.Documents;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Logger;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    public class DeletedRelatedUrlUrlEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            TempFileLogger.Log("Entered DeletedRelatedUrlUrlEventHandler.");
            RelatedDocumentsContainer relatedDocumentsContainer = Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is RelatedDocumentsContainer) as RelatedDocumentsContainer;

            if (relatedDocumentsContainer != null)
            {
                foreach (RelatedDocumentContainer relatedDocumentContainer in relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().ToList())
                {
                    TempFileLogger.Log("Removing the actual url url component from the view tree...");
                    relatedDocumentContainer.Children.RemoveAll(c => c.RShape.Equals(changedShape)); //Remove the component from the tree
                }
            }
        }
    }
}
