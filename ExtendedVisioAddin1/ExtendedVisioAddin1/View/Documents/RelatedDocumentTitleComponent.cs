using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    class RelatedDocumentTitleComponent : TextLabel
    {
        public RelatedDocumentTitleComponent(Page page, Shape shape) : base(page, shape)
        {
            AddUserRow("rationallyType");
            RationallyType = "relatedDocumentTitle";
            InitStyle();
        }

        public RelatedDocumentTitleComponent(Page page, string labelText) : base(page, labelText)
        {
            AddUserRow("rationallyType");
            RationallyType = "relatedDocumentTitle";
            InitStyle();
        }

        public void InitStyle()
        {
            SetMargin(0.2);
            MakeListItem();
            Width += 0.3;
        }
    }
}
