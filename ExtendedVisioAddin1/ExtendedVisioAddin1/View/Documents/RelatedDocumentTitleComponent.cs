using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    class RelatedDocumentTitleComponent : TextLabel
    {
        public RelatedDocumentTitleComponent(Page page, Shape shape) : base(page, shape)
        {
        }

        public RelatedDocumentTitleComponent(Page page, string labelText) : base(page, labelText)
        {
        }

        public void InitStyle()
        {
            this.SetMargin(0.2);
            this.MakeListItem();
        }
    }
}
