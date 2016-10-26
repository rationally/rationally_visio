using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rationally.Visio.WindowsFormPopups.WizardComponents
{
    class MenuPanel : TableLayoutPanel
    {
        public List<MenuButton> Buttons;

        private MenuButton highlightedButton;
        public MenuButton HighLightedButton
        {
            get { return highlightedButton; }
            set
            {
                highlightedButton = value;
                UpdateButtons();
            }
        }

        
        public MenuPanel()
        {
            Buttons = new List<MenuButton>();
        }

        private void UpdateButtons()
        {
            Buttons.Where(button => !button.Equals(highlightedButton)).ToList().ForEach(button => button.Lowlight());
            highlightedButton.Highlight();
        }
    }
}
