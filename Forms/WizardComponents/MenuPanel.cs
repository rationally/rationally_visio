using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class MenuPanel : TableLayoutPanel
    {
        private MenuButton button1;
        private MenuButton buttonShowAlternatives;

        public readonly List<MenuButton> Buttons;

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
            Init();

        }

        private void Init()
        {
            this.buttonShowAlternatives = new MenuButton(this);
            this.button1 = new MenuButton(this);

            
            // 
            // buttonShowAlternatives
            // 
            //this.buttonShowAlternatives.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.buttonShowAlternatives.FlatAppearance.BorderSize = 0;
            this.buttonShowAlternatives.Name = "buttonShowAlternatives";
            this.buttonShowAlternatives.UseVisualStyleBackColor = false;
            buttonShowAlternatives.Text = "Alternatives";
            this.buttonShowAlternatives.Click += new System.EventHandler(this.buttonShowAlternatives_Click);
            // 
            // button1
            // 
            //this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(194)))), ((int)(((byte)(207)))), ((int)(((byte)(242)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.Name = "button1";
            button1.Text = "General Information";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);


            //self
            Controls.Add(this.buttonShowAlternatives, 0, 1);
            Controls.Add(this.button1, 0, 0);


            HighLightedButton = this.button1;
        }

        private void UpdateButtons()
        {
            Buttons.Where(button => !button.Equals(highlightedButton)).ToList().ForEach(button => button.Lowlight());
            highlightedButton.Highlight();
        }

        private void buttonShowAlternatives_Click(object sender, System.EventArgs e)
        {

            ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Clear();
            ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.TableLayoutMainContentAlternatives);
            ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.flowLayoutBottomButtons);
            ProjectSetupWizard.Instance.flowLayoutBottomButtons.Refresh();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            ProjectSetupWizard.Instance.SetGeneralPanel();
        }
    }
}
