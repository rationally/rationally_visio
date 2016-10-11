using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Documents
{
    internal sealed class RelatedDocumentTitleComponent : TextLabel
    {
        private static readonly Regex RelatedRegex = new Regex(@"Related Document Title(\.\d+)?$");

        public RelatedDocumentTitleComponent(Page page, Shape shape) : base(page, shape)
        {
            RShape = shape;
            InitStyle();
        }

        public RelatedDocumentTitleComponent(Page page, int index, string text) : base(page, text)
        {
            AddUserRow("rationallyType");
            RationallyType = "relatedDocumentTitle";
            Name = "Related Document Title";
            AddUserRow("documentIndex");
            DocumentIndex = index;

            AddAction("addRelatedFile", "QUEUEMARKEREVENT(\"addRelatedFile\")", "\"Add file\"", false);
            AddAction("addRelatedUrl", "QUEUEMARKEREVENT(\"addRelatedUrl\")", "\"Add url\"", false);
            AddAction("deleteRelatedDocument", "QUEUEMARKEREVENT(\"delete\")", "\"Delete document\"", false);
            InitStyle();
        }

        public void InitStyle()
        {
            Width = 3.3;
            Height = 0.6;
            SetMargin(0.1);
            HAlign = 0;
            SetUsedSizingPolicy(SizingPolicy.FixedSize);
        }

        internal static bool IsRelatedDocumentTitleContainer(string name)
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
