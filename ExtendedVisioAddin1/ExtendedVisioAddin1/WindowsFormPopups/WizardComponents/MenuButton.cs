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

        public MenuButton()
        {
            normalFont = new Font("calibri", 12, FontStyle.Regular);
            boldFont = new Font("calibri", 12, FontStyle.Bold);
            BackColor = Color.FromArgb(1, 235, 235, 235);
            //FlatAppearance.MouseOverForeColor = 
            this.MouseEnter += button1_MouseEnter;
            this.MouseLeave += button1_MouseLeave;
            this.MouseHover += OnMouseHover;
        }

        private void OnMouseHover(object sender, EventArgs e)
        {

        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(1, 194, 207, 242);
            this.Font = boldFont;
        }
        private void button1_MouseLeave(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(1,235,235,235);
            this.Font = normalFont;
        }

    }
}
