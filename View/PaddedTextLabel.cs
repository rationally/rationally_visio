using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View
{
    class PaddedTextLabel : TextLabel
    {
        public PaddedTextLabel(Page page, Shape shape) : base(page, shape)
        {
            InitStyle();
        }

        public PaddedTextLabel(Page page, string labelText) : base(page, labelText)
        {
            InitStyle();
        }

        public void InitStyle()
        {
            SetMargin(0.01);
            MarginLeft = 0.02;
            SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
        }
    }
}
