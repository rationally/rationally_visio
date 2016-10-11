using System.Windows.Forms;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.WindowsFormPopups;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class AddRelatedUrlHandler : IMarkerEventHandler
    {
        public void Execute(RModel model, Shape changedShape, string context)
        {
            UrlSelecter selectUrlDialog = new UrlSelecter();
            if (selectUrlDialog.ShowDialog() == DialogResult.OK)
            {
                RelatedDocument document = new RelatedDocument(selectUrlDialog.urlTextBox.Text, selectUrlDialog.nameTextbox.Text, false);
                model.Documents.Add(document);
                Globals.RationallyAddIn.View.AddRelatedDocument(document);
            }
            selectUrlDialog.Dispose();
        }
    }
}
