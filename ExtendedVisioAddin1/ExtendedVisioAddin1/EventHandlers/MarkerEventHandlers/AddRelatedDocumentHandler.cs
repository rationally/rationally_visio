using System.Windows.Forms;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.MarkerEventHandlers
{
    internal class AddRelatedDocumentHandler : MarkerEventHandler
    {

        public override void Execute(RModel model, Shape changedShape, string identifier)
        {
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
                Globals.ThisAddIn.View.AddRelatedDocument(document);
            }
            openFileDialog.Dispose();
        }
    }
}
