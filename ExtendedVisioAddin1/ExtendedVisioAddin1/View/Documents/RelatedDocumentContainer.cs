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
            this.AddUserRow("rationallyType");
            this.RationallyType = "relatedDocumentContainer";
            InitStyle();
        }

        public RelatedDocumentContainer(Page page, bool makeShape) : base(page, makeShape)
        {
            InitStyle();
        }

        public void InitStyle()
        {

            LinePattern = 0;//borderless
            this.SetMargin(0.2);
            this.MarginTop = 0.3;
            this.MarginBottom = 0;
            this.Height = 1;

        }
    }
}
