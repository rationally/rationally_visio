using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    class ForceStubContainer : RComponent
    {
        public override int ForceIndex
        {
            get;
            set;
        }

        public ForceStubContainer(Page page, int forceIndex) : base(page)
        {
            ForceIndex = forceIndex;
        }

        public override bool ExistsInTree(Shape s)
        {
            return false;
        }
    }
}
