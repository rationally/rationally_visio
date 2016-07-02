using System.Windows.Forms;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.MarkerEventHandlers
{
    internal class AddRelatedUrlHandler : MarkerEventHandler
    {
        public override void Execute(RModel model, Shape changedShape, string context)
        {
            UrlSelecter selectUrlDialog = new UrlSelecter();
            if (selectUrlDialog.ShowDialog() == DialogResult.OK)
            {
                RelatedDocument document = new RelatedDocument(selectUrlDialog.urlTextBox.Text, selectUrlDialog.nameTextbox.Text, false);
                model.Documents.Add(document);
                Globals.ThisAddIn.View.AddRelatedDocument(document);
            }
            selectUrlDialog.Dispose();
        }
    }
}
