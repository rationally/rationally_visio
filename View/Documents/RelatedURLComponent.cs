using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Documents
{
    internal sealed class RelatedUrlComponent : RationallyComponent
    {
        private static readonly Regex RelatedRegex = new Regex(@"RelatedUrl(\.\d+)?$");
        public RelatedUrlComponent(Page page, Shape urlShape) : base(page)
        {
            RShape = urlShape;
            InitStyle();
        }

        public RelatedUrlComponent(Page page, int index, string url) : base(page)
        {
            string docPath = Constants.FolderPath + "RationallyHidden.vssx";
            Document rationallyDocument = Globals.RationallyAddIn.Application.Documents.OpenEx(docPath, (short)VisOpenSaveArgs.visAddHidden);
            Master rectMaster = rationallyDocument.Masters["LinkIcon"]; 
            RShape = page.Drop(rectMaster, 0, 0);

            Hyperlink link = RShape.AddHyperlink();
            link.Address = url;
            EventDblClick = "HYPERLINK(Hyperlink.Row_1.Address)";

            InitStyle();

            Name = "RelatedUrl";
            AddUserRow("rationallyType");
            RationallyType = "relatedUrl";
            AddUserRow("documentIndex");
            DocumentIndex = index;

            AddAction("addRelatedFile", "QUEUEMARKEREVENT(\"addRelatedFile\")", "\"Add file\"", false);
            AddAction("addRelatedUrl", "QUEUEMARKEREVENT(\"addRelatedUrl\")", "\"Add url\"", false);
            AddAction("deleteRelatedDocument", "QUEUEMARKEREVENT(\"delete\")", "\"Delete document\"", false);

            rationallyDocument.Close();
        }

        private void InitStyle()
        {
            Width = 0.6;
            Height = 0.6;
            SetMargin(0.1);
        }

        internal static bool IsRelatedUrlComponent(string name)
        {
            return RelatedRegex.IsMatch(name);
        }

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (DocumentIndex == 0)
            {
                DeleteAction("moveUp");
            }

            if (DocumentIndex == Globals.RationallyAddIn.Model.Documents.Count - 1)
            {
                DeleteAction("moveDown");
            }
        }

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)//Visio does this for us
            {
                UpdateReorderFunctions();
            }
            base.Repaint();
        }
    }
}
