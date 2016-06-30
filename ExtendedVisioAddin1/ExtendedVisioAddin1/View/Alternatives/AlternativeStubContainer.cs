using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Alternatives
{
    class AlternativeStubContainer : RComponent
    {
        public override int AlternativeIndex
        {
            get;
            set;
        }


        public AlternativeStubContainer(Page page,int alternativeIndex) : base(page)
        {
            AlternativeIndex = alternativeIndex;
            Page = page;
        }

        public override bool ExistsInTree(Shape s)
        {
            return false;
        }
    }
}
