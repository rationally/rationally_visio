using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Rationally.Visio.RationallyConstants;

namespace Rationally.Visio.Forms.WizardComponents
{
    public sealed class MenuButton : AntiAliasedButton
    {
        private readonly MenuPanel containingMenu;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public MenuButton(MenuPanel containingMenu)
        {
            this.containingMenu = containingMenu;
            Width = 244;
            Height = 40;
            TextAlign = ContentAlignment.MiddleLeft;
            BackColor = Color.FromArgb(235, 235, 235);
            //FlatAppearance.MouseOverForeColor = 
            FlatStyle = FlatStyle.Flat;
            UseVisualStyleBackColor = false;
            MouseEnter += button1_MouseEnter;
            MouseLeave += button1_MouseLeave;
            Click += button1_Click;
            Margin = new Padding(0);

            this.containingMenu.Buttons.Add(this);
        }

        private void button1_Click(object sender, EventArgs e) => containingMenu.HighLightedButton = this;


        private void button1_MouseEnter(object sender, EventArgs e)
        {
            if (!Equals(containingMenu.HighLightedButton))
            {
                Highlight();
            }
        }
        private void button1_MouseLeave(object sender, EventArgs e)
        {
            if (!Equals(containingMenu.HighLightedButton))
            {
                Lowlight();
            }
        }


        public void Highlight()
        {
            Font = WizardConstants.HighlightedFont;
            BackColor = Color.FromArgb(194, 207, 242);
            Refresh();
        }

        public void Lowlight()
        {
            BackColor = Color.FromArgb(235, 235, 235);
            Font = WizardConstants.NormalFont;
        }

    }
}
