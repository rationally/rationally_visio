using System;
using System.Drawing;
using System.Windows.Forms;

namespace Rationally.Visio.WindowsFormPopups.WizardComponents
{
    internal class MenuButton : Button
    {
        private readonly Font normalFont;
        private readonly Font boldFont;
        private readonly MenuPanel containingMenu;
        public MenuButton(MenuPanel containingMenu)
        {
            this.containingMenu = containingMenu;
            
            normalFont = new Font("calibri", 12, FontStyle.Regular);
            boldFont = new Font("calibri", 12, FontStyle.Bold);
            BackColor = Color.FromArgb(1, 235, 235, 235);
            //FlatAppearance.MouseOverForeColor = 
            MouseEnter += button1_MouseEnter;
            MouseLeave += button1_MouseLeave;
            Click += button1_Click;

            this.containingMenu.Buttons.Add(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            containingMenu.HighLightedButton = this;
        }


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
            BackColor = Color.FromArgb(1, 194, 207, 242);
            Font = boldFont;
            Refresh();
        }

        public void Lowlight()
        {
            BackColor = Color.FromArgb(1, 235, 235, 235);
            Font = normalFont;
        }
    }
}
