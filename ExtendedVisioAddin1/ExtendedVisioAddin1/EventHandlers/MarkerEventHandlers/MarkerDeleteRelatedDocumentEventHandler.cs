using System.Linq;
using System.Windows.Forms;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.MarkerEventHandlers
{
    class MarkerDeleteRelatedDocumentEventHandler : IMarkerEventHandler
    {
        public void Execute(RModel model, Shape s, string identifier)
        {
            RComponent component = new RComponent(Globals.ThisAddIn.Application.ActivePage) { RShape = s };

            int index = component.DocumentIndex;
            RelatedDocument document = model.Documents[index];
            DialogResult confirmResult = MessageBox.Show("Are you sure you want to delete " + document.Name + "?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Shape shapeToPass;

                if (RelatedDocumentContainer.IsRelatedDocumentContainer(s.Name))
                {
                    shapeToPass = s;
                }
                else //subpart of document container
                {
                    //trace documents container
                    RelatedDocumentsContainer documentsContainer = (RelatedDocumentsContainer)Globals.ThisAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
                    //trace the correct document container
                    RelatedDocumentContainer documentContainer = (RelatedDocumentContainer)documentsContainer.Children.First(c => c is RelatedDocumentContainer && component.DocumentIndex == c.DocumentIndex);

                    shapeToPass = documentContainer.RShape;
                }
                //initiate a delete handler with the container's shape
                shapeToPass.Delete();
            }
        }
    }
}
