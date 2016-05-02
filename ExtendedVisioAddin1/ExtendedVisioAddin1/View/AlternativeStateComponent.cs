using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeStateComponent : TextLabel
    {
        public AlternativeStateComponent(Page page, string state ) : base(page, state)
        {

        }
    }
}
