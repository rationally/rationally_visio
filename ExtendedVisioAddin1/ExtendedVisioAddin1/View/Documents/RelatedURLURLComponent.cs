using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    class RelatedURLURLComponent : TextLabel
    {
        public RelatedURLURLComponent(Page page, Shape shape) : base(page, shape)
        {
            InitStyle();
        }

        public RelatedURLURLComponent(Page page, string labelText) : base(page, labelText)
        {
            AddUserRow("rationallyType");
            RationallyType = "relatedUrlUrl";
            Name = "Related Url Url";
            InitStyle();
        }

        private void InitStyle()
        {
            Width = 4.2;
            UsedSizingPolicy &= ~SizingPolicy.ExpandXIfNeeded;//we want to remove this one from the policy: AND with everything else on true
        }
    }
}
