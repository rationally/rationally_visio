using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Rationally.Visio.RationallyConstants
{
    class WizardConstants
    {
        public Font NormalFont;
        public Font HighlightedFont;
        public WizardConstants()
        {
            NormalFont = new Font("Calibri", 12);
            HighlightedFont = new Font("Calibri", 12, FontStyle.Bold);
        }
    }
}
