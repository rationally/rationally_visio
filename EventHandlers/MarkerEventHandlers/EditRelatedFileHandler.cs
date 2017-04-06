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
    internal class EditRelatedFileHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape changedShape, string context)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                VisioShape comp = new VisioShape(Globals.RationallyAddIn.Application.ActivePage) { Shape = changedShape };
                int index = comp.Index;

                //container of all related documents:
                RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer) Globals.RationallyAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
                //find the the RelatedDocumentContainer of the selected file
                RelatedDocumentContainer documentContainer = (RelatedDocumentContainer) relatedDocumentsContainer.Children.First(f => f.Index == index);

                RelatedDocument doc = model.Documents[index];
                doc.Name = openFileDialog.FileName;
                doc.Path = openFileDialog.FileName;
                documentContainer.EditFile(doc, index);
                RepaintHandler.Repaint(relatedDocumentsContainer);
            }
        }
    }
}
