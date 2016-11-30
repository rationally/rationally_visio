using System.Drawing.Text;
using System.Windows.Forms;

namespace Rationally.Visio.Forms.WizardComponents
{
    internal class AntiAliasedLabel : Label
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            base.OnPaint(e);
        }
    }
}
