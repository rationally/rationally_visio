using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Documents;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MarkerDeleteRelatedDocumentEventHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape s, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            VisioShape component = new VisioShape(Globals.RationallyAddIn.Application.ActivePage) { Shape = s };

            int index = component.Index;
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
                    RelatedDocumentsContainer documentsContainer = (RelatedDocumentsContainer)Globals.RationallyAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
                    //trace the correct document container
                    RelatedDocumentContainer documentContainer = (RelatedDocumentContainer)documentsContainer.Children.First(c => c is RelatedDocumentContainer && (component.Index == c.Index));

                    shapeToPass = documentContainer.Shape;
                }
                //initiate a delete handler with the container's shape
                shapeToPass.Delete();
            }
        }
    }
}
