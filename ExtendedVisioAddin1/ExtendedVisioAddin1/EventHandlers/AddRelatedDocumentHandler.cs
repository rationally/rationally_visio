using System.Linq;
using System.Windows.Forms;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Interop.Visio;
using Application = Microsoft.Office.Interop.Visio.Application;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class AddRelatedDocumentHandler : MarkerEventHandler
    {

        public override void Execute(RModel model, Shape changedShape, string identifier)
        {
            Application application = Globals.ThisAddIn.Application;
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
