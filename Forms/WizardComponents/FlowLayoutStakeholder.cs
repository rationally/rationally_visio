using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class FlowLayoutStakeholder : GroupBox
    {
        public int StakeholderIndex { get; private set; }

        private readonly AntiAliasedLabel stakeholderNameLabel;
        internal readonly TextBox StakeholderName;
        private readonly AntiAliasedButton deleteStakeholderButton;

        public Stakeholder Stakeholder => ProjectSetupWizard.Instance.ModelCopy.Stakeholders.Count > StakeholderIndex ? ProjectSetupWizard.Instance.ModelCopy.Stakeholders[StakeholderIndex] : null;

        private readonly AntiAliasedLabel stakeholderRoleLabel;
        internal readonly TextBox StakeholderRole;

        public FlowLayoutStakeholder(int stakeholderIndex)
        {
            StakeholderIndex = stakeholderIndex;

            Dock = DockStyle.Top;
            //this.Anchor = AnchorStyles.Left;
            Location = new Point(3, 3);
            Name = "flowLayoutPanelStakeholder" + StakeholderIndex;
            Size = new Size(714, 84);
            TabIndex = 0;

            stakeholderNameLabel = new AntiAliasedLabel();
            StakeholderName = new TextBox();
            stakeholderRoleLabel = new AntiAliasedLabel();
            StakeholderRole = new TextBox();
            deleteStakeholderButton = new AntiAliasedButton();
            SuspendLayout();
            Init();
        }

        private void Init()
        {
            Controls.Add(stakeholderNameLabel);
            Controls.Add(StakeholderName);
            Controls.Add(stakeholderRoleLabel);
            Controls.Add(StakeholderRole);
            Controls.Add(deleteStakeholderButton);
            //
            // fileNameLabel
            //
            stakeholderNameLabel.AutoSize = true;
            stakeholderNameLabel.Location = new Point(8, 17);
            stakeholderNameLabel.Margin = new Padding(3, 10, 3, 0);
            stakeholderNameLabel.Name = "stakeholderNameLabel";
            stakeholderNameLabel.Size = new Size(100, 19);
            stakeholderNameLabel.TabIndex = 0;
            stakeholderNameLabel.Text = "Name:";
            //
            // fileName
            //
            StakeholderName.Location = new Point(110, 15);
            StakeholderName.Margin = new Padding(3, 6, 400, 3);
            StakeholderName.Name = "stakeholderName";
            StakeholderName.Size = new Size(350, 27);
            StakeholderName.TabIndex = 1;
            //
            // filepathlabel
            //
            stakeholderRoleLabel.AutoSize = true;
            stakeholderRoleLabel.Location = new Point(8, 52);
            stakeholderRoleLabel.Margin = new Padding(3, 10, 3, 0);
            stakeholderRoleLabel.Name = "stakeholderRoleLabel";
            stakeholderRoleLabel.Size = new Size(100, 19);
            stakeholderRoleLabel.TabIndex = 2;
            stakeholderRoleLabel.Text = "Role:";
            //
            // stakeholderRole
            //
            StakeholderRole.Location = new Point(110, 50);
            StakeholderRole.Margin = new Padding(3, 6, 3, 3);
            StakeholderRole.Name = "stakeholderRole";
            StakeholderRole.Size = new Size(350, 27);
            StakeholderRole.TabIndex = 3;
            //
            // deleteStakeholderButton
            //
            deleteStakeholderButton.Name = "DeleteStakeholderButton";
            deleteStakeholderButton.UseVisualStyleBackColor = true;
            deleteStakeholderButton.Click += RemoveStakeholder;
            deleteStakeholderButton.TabIndex = 5;
            deleteStakeholderButton.Location = new Point(580, 50);
            deleteStakeholderButton.Size = new Size(150, 27);
            deleteStakeholderButton.Margin = new Padding(3, 0, 3, 3);
            deleteStakeholderButton.Text = "Remove";
        }

        private void RemoveStakeholder(object sender, EventArgs e)
        {
            ProjectSetupWizard.Instance.TableLayoutMainContentStakeholders.Stakeholders.Remove(this);
            ProjectSetupWizard.Instance.TableLayoutMainContentStakeholders.UpdateRows();
        }

        public void UpdateModel()
        {
            //if this row represents an existing stakeholder in the model, update it.
            if (Stakeholder != null)
            {
                Stakeholder.Name = StakeholderName.Text;
                Stakeholder.Role = StakeholderRole.Text;
            }
            else
            {
                StakeholderIndex = Math.Min(StakeholderIndex, ProjectSetupWizard.Instance.ModelCopy.Stakeholders.Count);
                (Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is StakeholdersContainer) as StakeholdersContainer)?.AddStakeholder(StakeholderName.Text, StakeholderRole.Text);
            }
        }

        public void UpdateData()
        {
            if (Stakeholder != null)
            {
                StakeholderName.Text = Stakeholder.Name;
                StakeholderRole.Text = Stakeholder.Role;
            }
        }
    }
}
