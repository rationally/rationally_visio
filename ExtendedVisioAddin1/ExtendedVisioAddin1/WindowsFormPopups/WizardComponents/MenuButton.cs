using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rationally.Visio.WindowsFormPopups.WizardComponents
{
    class MenuButton : Button
    {
        private Font normalFont;
        private Font boldFont;
        private MenuPanel containingMenu;
        public MenuButton(MenuPanel containingMenu)
        {
            this.containingMenu = containingMenu;
            
            normalFont = new Font("calibri", 12, FontStyle.Regular);
            boldFont = new Font("calibri", 12, FontStyle.Bold);
            BackColor = Color.FromArgb(1, 235, 235, 235);
            //FlatAppearance.MouseOverForeColor = 
            this.MouseEnter += button1_MouseEnter;
            this.MouseLeave += button1_MouseLeave;
            this.Click += button1_Click;

            this.containingMenu.Buttons.Add(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            containingMenu.HighLightedButton = this;
        }


        private void button1_MouseEnter(object sender, EventArgs e)
        {
            if (!this.Equals(containingMenu.HighLightedButton))
            {
                Highlight();
            }
        }
        private void button1_MouseLeave(object sender, EventArgs e)
        {
            if (!this.Equals(containingMenu.HighLightedButton))
            {
                Lowlight();
            }
        }


        public void Highlight()
        {
            BackColor = Color.FromArgb(1, 194, 207, 242);
            this.Font = boldFont;
            Refresh();
        }

        public void Lowlight()
        {
            BackColor = Color.FromArgb(1, 235, 235, 235);
            this.Font = normalFont;
        }
    }
}
