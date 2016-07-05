using System.Linq;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.MarkerEventHandlers
{
    internal class MoveDownDocumentHandler : IMarkerEventHandler
    {
        public void Execute(RModel model, Shape changedShape, string identifier)
        {
            RelatedDocumentsContainer docsContainer = (RelatedDocumentsContainer)Globals.ThisAddIn.View.Children.First(c => c is RelatedDocumentsContainer);

            RComponent currentComponent = new RComponent(changedShape.ContainingPage) { RShape = changedShape };
            int currentIndex = currentComponent.DocumentIndex;

            //swap the forces in the model
            RelatedDocument currentDoc = model.Documents[currentIndex];
            model.Documents[currentIndex] = model.Documents[currentIndex + 1];
            model.Documents[currentIndex + 1] = currentDoc;

            RelatedDocumentContainer toMove = docsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().First(c => c.DocumentIndex == currentIndex);
            RelatedDocumentContainer toSwapWith = docsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().First(c => c.DocumentIndex == currentIndex + 1);

            //update the index of the component and his children
            toMove.SetDocumentIdentifier(currentIndex + 1);

            //same, for the other component
            toSwapWith.SetDocumentIdentifier(currentIndex);

            RComponent temp = docsContainer.Children[currentIndex];
            docsContainer.Children[currentIndex] = docsContainer.Children[currentIndex + 1];
            docsContainer.Children[currentIndex + 1] = temp;

            new RepaintHandler(docsContainer);
        }
    }
}
