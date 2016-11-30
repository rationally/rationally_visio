using System.Drawing.Text;
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
