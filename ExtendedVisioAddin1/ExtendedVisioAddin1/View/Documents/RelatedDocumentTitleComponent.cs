using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    internal class RelatedDocumentTitleComponent : TextLabel
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
    }
}
