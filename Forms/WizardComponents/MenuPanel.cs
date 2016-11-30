using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class MenuPanel : TableLayoutPanel
    {
        private MenuButton button1;
        private MenuButton buttonShowAlternatives;
        private MenuButton buttonShowForces;

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
            buttonShowAlternatives = new MenuButton(this);
            button1 = new MenuButton(this);
            buttonShowForces = new MenuButton(this);

            // 
            // buttonShowAlternatives
            // 
            //this.buttonShowAlternatives.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            buttonShowAlternatives.FlatAppearance.BorderSize = 0;
            buttonShowAlternatives.Name = "buttonShowAlternatives";
            buttonShowAlternatives.UseVisualStyleBackColor = false;
            buttonShowAlternatives.Text = "Alternatives";
            buttonShowAlternatives.Click += buttonShowAlternatives_Click;
            // 
            // button1
            // 
            //this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(194)))), ((int)(((byte)(207)))), ((int)(((byte)(242)))));
            button1.FlatAppearance.BorderSize = 0;
            button1.Name = "button1";
            button1.Text = "General Information";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            //
            // button forces
            //
            buttonShowForces.FlatAppearance.BorderSize = 0;
            buttonShowForces.Name = "buttonShowForces";
            buttonShowForces.Text = "Forces";
            buttonShowForces.UseVisualStyleBackColor = false;
            buttonShowForces.Click += buttonShowForces_Click;
            //self
            
            Controls.Add(button1, 0, 0);
            Controls.Add(buttonShowAlternatives, 0, 1);
            Controls.Add(buttonShowForces, 0, 2);

            HighLightedButton = button1;
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

        private void buttonShowForces_Click(object sender, System.EventArgs e)
        {
            ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Clear();
            ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.TableLayoutMainContentForces);
            ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.flowLayoutBottomButtons);
            ProjectSetupWizard.Instance.flowLayoutBottomButtons.Refresh();
        }
    }
}
