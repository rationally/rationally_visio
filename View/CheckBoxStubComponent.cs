using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View
{
    class CheckBoxStubComponent : RationallyComponent
    {
        public override int Index
        {
            get; set;
        }

        public CheckBoxStubComponent(Page page, int index) : base(page)
        {
            Index = index;
        }

        public override bool ExistsInTree(Shape s) => false;
    }
}
