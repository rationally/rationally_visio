using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Documents;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MoveUpDocumentHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape changedShape, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            RelatedDocumentsContainer docsContainer = (RelatedDocumentsContainer)Globals.RationallyAddIn.View.Children.First(c => c is RelatedDocumentsContainer);

            RationallyComponent currentComponent = new RationallyComponent(changedShape.ContainingPage) {RShape = changedShape};
            int currentIndex = currentComponent.Index;

            //swap the forces in the model
            RelatedDocument currentDoc = model.Documents[currentIndex];
            model.Documents[currentIndex] = model.Documents[currentIndex - 1];
            model.Documents[currentIndex - 1] = currentDoc;

            RelatedDocumentContainer toMove = docsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().First(c => c.Index == currentIndex);
            RelatedDocumentContainer toSwapWith = docsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().First(c => c.Index == currentIndex - 1);

            //update the index of the component and his children
            toMove.SetDocumentIdentifier(currentIndex - 1);

            //same, for the other component
            toSwapWith.SetDocumentIdentifier(currentIndex);

            RationallyComponent temp = docsContainer.Children[currentIndex];
            docsContainer.Children[currentIndex] = docsContainer.Children[currentIndex - 1];
            docsContainer.Children[currentIndex - 1] = temp;

            RepaintHandler.Repaint(docsContainer);
        }
    }
}
