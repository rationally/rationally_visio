﻿using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Planning
{
    internal sealed class PlanningStubItem : VisioShape
    {
        public override int Index
        {
            get; set;
        }

        public PlanningStubItem(Page page, int index) : base(page)
        {
            Index = index;
        }

        public override bool ExistsInTree(Shape s) => false;
    }
}
