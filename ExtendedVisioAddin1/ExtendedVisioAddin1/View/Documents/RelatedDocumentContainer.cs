using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    class RelatedDocumentContainer : HeaderlessContainer
    {
        public RelatedDocumentContainer(Page page) : base(page)
        {
            AddUserRow("rationallyType");
            RationallyType = "relatedDocumentContainer";
            InitStyle();
        }

        public RelatedDocumentContainer(Page page, bool makeShape) : base(page, makeShape)
        {
            InitStyle();
        }

        public void InitStyle()
        {

            LinePattern = 0;//borderless
            SetMargin(0.2);
            MarginTop = 0.3;
            MarginBottom = 0;
            Height = 1;
            UsedSizingPolicy |= SizingPolicy.ExpandYIfNeeded;
        }
    }
}
