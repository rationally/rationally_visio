using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class MenuPanel : TableLayoutPanel
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public readonly MenuButton ButtonShowGeneral;
        private readonly MenuButton buttonShowAlternatives;
        private readonly MenuButton buttonShowForces;
        private readonly MenuButton buttonShowRelatedDocuments;
        private readonly MenuButton buttonShowStakeholders;
        private readonly MenuButton buttonShowPlanning;

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
            buttonShowAlternatives = new MenuButton(this);
            ButtonShowGeneral = new MenuButton(this);
            buttonShowForces = new MenuButton(this);
            buttonShowRelatedDocuments = new MenuButton(this);
            buttonShowStakeholders = new MenuButton(this);
            buttonShowPlanning = new MenuButton(this);
            Init();

        }

        private void Init()
        {
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
            // ButtonShowGeneral
            // 
            //this.ButtonShowGeneral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(194)))), ((int)(((byte)(207)))), ((int)(((byte)(242)))));
            ButtonShowGeneral.FlatAppearance.BorderSize = 0;
            ButtonShowGeneral.Name = "ButtonShowGeneral";
            ButtonShowGeneral.Text = "General Information";
            ButtonShowGeneral.UseVisualStyleBackColor = false;
            ButtonShowGeneral.Click += ButtonShowGeneralClick;
            //
            // button forces
            //
            buttonShowForces.FlatAppearance.BorderSize = 0;
            buttonShowForces.Name = "buttonShowForces";
            buttonShowForces.Text = "Forces";
            buttonShowForces.UseVisualStyleBackColor = false;
            buttonShowForces.Click += buttonShowForces_Click;
            //
            // button forces
            //
            buttonShowRelatedDocuments.FlatAppearance.BorderSize = 0;
            buttonShowRelatedDocuments.Name = "buttonShowRelatedDocuments";
            buttonShowRelatedDocuments.Text = "Related Documents";
            buttonShowRelatedDocuments.UseVisualStyleBackColor = false;
            buttonShowRelatedDocuments.Click += buttonShowDocuments_Click;
            //
            // button stakeholders
            //
            buttonShowStakeholders.FlatAppearance.BorderSize = 0;
            buttonShowStakeholders.Name = "buttonShowStakeholders";
            buttonShowStakeholders.Text = "Stakeholders";
            buttonShowStakeholders.UseVisualStyleBackColor = false;
            buttonShowStakeholders.Click += buttonShowStakeholders_Click;
            //
            // button planning
            //
            buttonShowPlanning.FlatAppearance.BorderSize = 0;
            buttonShowPlanning.Name = "buttonShowPlanning";
            buttonShowPlanning.Text = "Planning";
            buttonShowPlanning.UseVisualStyleBackColor = false;
            buttonShowPlanning.Click += buttonShowPlanning_Click;
            //self

            Controls.Add(ButtonShowGeneral, 0, 0);
            Controls.Add(buttonShowAlternatives, 0, 1);
            Controls.Add(buttonShowForces, 0, 2);
            Controls.Add(buttonShowRelatedDocuments, 0, 3);
            Controls.Add(buttonShowStakeholders, 0, 4);
            Controls.Add(buttonShowPlanning, 0, 5);
            HighLightedButton = ButtonShowGeneral;
        }

        private void UpdateButtons()
        {
            Buttons.Where(button => !button.Equals(highlightedButton)).ToList().ForEach(button => button.Lowlight());
            highlightedButton.Highlight();
        }

        private void buttonShowAlternatives_Click(object sender, EventArgs e)
        {
            if ((sender as MenuButton)?.HandleEvent ?? false)
            {
                ProjectSetupWizard.Instance.CurrentPanel = ProjectSetupWizard.Instance.TableLayoutMainContentAlternatives;
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Clear();
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.TableLayoutMainContentAlternatives);
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.FlowLayoutBottomButtons);
                //define bottom buttons
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Clear();
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Add(ProjectSetupWizard.Instance.CreateButton);
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Add(ProjectSetupWizard.Instance.TableLayoutMainContentAlternatives.AddAlternativeButton);

                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Refresh();

                ProjectSetupWizard.Instance.TableLayoutMainContentAlternatives.InitData();
            }
        }

        private void ButtonShowGeneralClick(object sender, EventArgs e)
        {
            if ((sender as MenuButton)?.HandleEvent ?? false)
            {
                ProjectSetupWizard.Instance.SetGeneralPanel();
            }
        }

        private void buttonShowForces_Click(object sender, EventArgs e)
        {
            if ((sender as MenuButton)?.HandleEvent ?? false)
            {
                ProjectSetupWizard.Instance.CurrentPanel = ProjectSetupWizard.Instance.TableLayoutMainContentForces;
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Clear();
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.TableLayoutMainContentForces);
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.FlowLayoutBottomButtons);
                //define bottom buttons
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Clear();
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Add(ProjectSetupWizard.Instance.CreateButton);

                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Refresh();

                ProjectSetupWizard.Instance.TableLayoutMainContentForces.InitData();
            }
        }

        private void buttonShowDocuments_Click(object sender, EventArgs e)
        {
            if ((sender as MenuButton)?.HandleEvent ?? false)
            {
                ProjectSetupWizard.Instance.CurrentPanel = ProjectSetupWizard.Instance.TableLayoutMainContentDocuments;
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Clear();
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.TableLayoutMainContentDocuments);
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.FlowLayoutBottomButtons);
                //define bottom buttons
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Clear();
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Add(ProjectSetupWizard.Instance.CreateButton);
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Add(ProjectSetupWizard.Instance.TableLayoutMainContentDocuments.AddDocumentButton);

                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Refresh();

                ProjectSetupWizard.Instance.TableLayoutMainContentDocuments.InitData();
            }
        }

        private void buttonShowStakeholders_Click(object sender, EventArgs e)
        {
            if ((sender as MenuButton)?.HandleEvent ?? false)
            {
                ProjectSetupWizard.Instance.CurrentPanel = ProjectSetupWizard.Instance.TableLayoutMainContentStakeholders;
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Clear();
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.TableLayoutMainContentStakeholders);
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.FlowLayoutBottomButtons);
                //define bottom buttons
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Clear();
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Add(ProjectSetupWizard.Instance.CreateButton);
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Add(ProjectSetupWizard.Instance.TableLayoutMainContentStakeholders.AddStakeholderButton);

                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Refresh();

                ProjectSetupWizard.Instance.TableLayoutMainContentStakeholders.InitData();
            }
        }

        private void buttonShowPlanning_Click(object sender, EventArgs e)
        {
            if ((sender as MenuButton)?.HandleEvent ?? false)
            {
                ProjectSetupWizard.Instance.CurrentPanel = ProjectSetupWizard.Instance.TableLayoutMainContentPlanningItems;
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Clear();
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.TableLayoutMainContentPlanningItems);
                ProjectSetupWizard.Instance.tableLayoutRightColumn.Controls.Add(ProjectSetupWizard.Instance.FlowLayoutBottomButtons);
                //define bottom buttons
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Clear();
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Add(ProjectSetupWizard.Instance.CreateButton);
                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Controls.Add(ProjectSetupWizard.Instance.TableLayoutMainContentPlanningItems.AddPlanningItemButton);

                ProjectSetupWizard.Instance.FlowLayoutBottomButtons.Refresh();

                ProjectSetupWizard.Instance.TableLayoutMainContentPlanningItems.InitData();
            }
        }
    }
}
