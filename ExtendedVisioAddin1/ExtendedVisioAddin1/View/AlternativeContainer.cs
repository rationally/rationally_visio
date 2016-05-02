using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeContainer : HeaderlessContainer
    {
        public AlternativeContainer(Page page, IVShape shape) : base(page)
        {
            this.RShape = shape;
        }
    }
}
