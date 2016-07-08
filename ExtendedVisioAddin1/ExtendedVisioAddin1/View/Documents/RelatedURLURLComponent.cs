using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    internal sealed class RelatedURLURLComponent : TextLabel
    {
        private static readonly Regex UrlUrlRegex = new Regex(@"RelatedUrlUrl(\.\d+)?$");

        public RelatedURLURLComponent(Page page, Shape shape) : base(page, shape)
        {
            InitStyle();
        }

        public RelatedURLURLComponent(Page page, int index, string labelText) : base(page, labelText)
        {
            AddUserRow("rationallyType");
            RationallyType = "relatedUrlUrl";
            Name = "RelatedUrlUrl";
            AddUserRow("documentIndex");
            DocumentIndex = index;

            AddAction("addRelatedFile", "QUEUEMARKEREVENT(\"addRelatedFile\")", "\"Add file\"", false);
            AddAction("addRelatedUrl", "QUEUEMARKEREVENT(\"addRelatedUrl\")", "\"Add url\"", false);
            AddAction("deleteRelatedDocument", "QUEUEMARKEREVENT(\"delete\")", "\"Delete document\"", false);

            Width = 4.2;
            InitStyle();
        }

        private void InitStyle()
        {
            UsedSizingPolicy = SizingPolicy.All;
            SetUsedSizingPolicy(UsedSizingPolicy &= ~SizingPolicy.ExpandXIfNeeded);//we want to remove this one from the policy: AND with everything else on true
        }

        public static bool IsRelatedUrlUrlComponent(string name)
        {
            return UrlUrlRegex.IsMatch(name);
        }

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (DocumentIndex == 0)
            {
                DeleteAction("moveUp");
            }

            if (DocumentIndex == Globals.ThisAddIn.Model.Documents.Count - 1)
            {
                DeleteAction("moveDown");
            }
        }

        public override void Repaint()
        {
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing) //Visio does this for us
            {
                UpdateReorderFunctions();
            }
            base.Repaint();
        }
    }
}
