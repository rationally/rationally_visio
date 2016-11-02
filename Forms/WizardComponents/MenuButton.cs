using System;
using System.Drawing;
using System.Windows.Forms;

namespace Rationally.Visio.Forms.WizardComponents
{
    internal class MenuButton : Button
    {
        private readonly MenuPanel containingMenu;
        public MenuButton(MenuPanel containingMenu)
        {
            this.containingMenu = containingMenu;
            
            //BackColor = Color.FromArgb(1, 235, 235, 235);
            BackColor = Color.FromArgb(1, 194, 207, 242);
            //FlatAppearance.MouseOverForeColor = 
            FlatStyle = FlatStyle.Flat;
            UseVisualStyleBackColor = false;
            MouseEnter += button1_MouseEnter;
            MouseLeave += button1_MouseLeave;
            Click += button1_Click;

            this.containingMenu.Buttons.Add(this);
        }

        public sealed override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
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
            //Font = boldFont;
            BackColor = Color.FromArgb(1, 194, 207, 242);
            
            Refresh();
        }

        public void Lowlight()
        {
            BackColor = Color.FromArgb(1, 235, 235, 235);
            //Font = normalFont;
        }
    }
}
