using System.Windows.Forms;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Forms;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class AddRelatedUrlHandler : IMarkerEventHandler
    {
        public void Execute(Shape changedShape, string context)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
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
