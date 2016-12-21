using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Stakeholders
{
    class StakeholderStubContainer : RationallyComponent
    {
        public override int StakeholderIndex
        {
            get; set;
        }

        public StakeholderStubContainer(Page page, int stakeholderIndex) : base(page)
        {
            StakeholderIndex = stakeholderIndex;
        }

        public override bool ExistsInTree(Shape s)
        {
            return false;
        }
    }
}
