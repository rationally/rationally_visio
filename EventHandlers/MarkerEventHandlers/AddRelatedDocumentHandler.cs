using System.Windows.Forms;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class AddRelatedDocumentHandler : IMarkerEventHandler
    {

        public void Execute(Shape changedShape, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //AddToModel
                RelatedDocument document = new RelatedDocument(openFileDialog.FileName, openFileDialog.FileName, true);
                model.Documents.Add(document);
                Globals.RationallyAddIn.View.AddRelatedDocument(document);
            }
            openFileDialog.Dispose();
        }
    }
}
