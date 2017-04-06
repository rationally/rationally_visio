using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View.Documents;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    public class DeletedRelatedUrlUrlEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Log.Debug("Entered DeletedRelatedUrlUrlEventHandler.");
            RelatedDocumentsContainer relatedDocumentsContainer = Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is RelatedDocumentsContainer) as RelatedDocumentsContainer;

            if (relatedDocumentsContainer != null)
            {
                foreach (RelatedDocumentContainer relatedDocumentContainer in relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().ToList())
                {
                    Log.Debug("Removing the actual url url component from the view tree...");
                    relatedDocumentContainer.Children.RemoveAll(c => c.Shape.Equals(changedShape)); //Remove the component from the tree
                }
            }
        }
    }
}
