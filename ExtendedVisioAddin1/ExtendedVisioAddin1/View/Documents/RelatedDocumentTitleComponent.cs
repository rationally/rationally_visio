using System;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    class RelatedDocumentTitleComponent : TextLabel
    {
        private static readonly Regex RelatedRegex = new Regex(@"Related Document Title(\.\d+)?$");
        public RelatedDocumentTitleComponent(Page page, Shape shape) : base(page, shape)
        {
            RShape = shape;
            InitStyle();
        }

        public RelatedDocumentTitleComponent(Page page, string labelText) : base(page, labelText)
        {
            AddUserRow("rationallyType");
            RationallyType = "relatedDocumentTitle";
            Name = "Related Document Title";
            InitStyle();
        }

        public void InitStyle()
        {
            Width = 3.3;
            SetMargin(0.2);
            MakeListItem();
        }

        internal static bool IsRelatedDocumentTitleContainer(string name)
        {
            return RelatedRegex.IsMatch(name);
        }
    }
}
