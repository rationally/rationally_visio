using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class AntiAliasedButton : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            base.OnPaint(pevent);
        }
    }
}
