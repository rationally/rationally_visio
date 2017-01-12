﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Rationally.Visio.Model;
using System.Linq;
using Rationally.Visio.View.Documents;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class FlowLayoutStakeholder : FlowLayoutPanel
    {
        public int StakeholderIndex { get; private set; }

        private readonly AntiAliasedLabel stakeholderNameLabel;
        internal readonly TextBox StakeholderName;
        private readonly AntiAliasedButton deleteStakeholderButton;

        public Stakeholder Stakeholder => Globals.RationallyAddIn.Model.Stakeholders.Count > StakeholderIndex ? Globals.RationallyAddIn.Model.Stakeholders[StakeholderIndex] : null;

        private AntiAliasedLabel stakeholderRoleLabel;
        internal readonly TextBox StakeholderRole;

        public FlowLayoutStakeholder(int stakeholderIndex)
        {
            StakeholderIndex = stakeholderIndex;

            Dock = DockStyle.Fill;
            //this.Anchor = AnchorStyles.Left;
            Location = new Point(3, 3);
            Name = "flowLayoutPanelStakeholder" + this.StakeholderIndex;
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
            stakeholderNameLabel.Location = new Point(3, 9);
            stakeholderNameLabel.Margin = new Padding(3, 10, 3, 0);
            stakeholderNameLabel.Name = "stakeholderNameLabel";
            stakeholderNameLabel.Size = new Size(100, 19);
            stakeholderNameLabel.TabIndex = 0;
            stakeholderNameLabel.Text = "Name:";
            //
            // fileName
            //
            StakeholderName.Location = new Point(110, 7);
            StakeholderName.Margin = new Padding(3, 6, 400, 3);
            StakeholderName.Name = "stakeholderName";
            StakeholderName.Size = new Size(300, 27);
            StakeholderName.TabIndex = 1;
            //
            // filepathlabel
            //
            stakeholderRoleLabel.AutoSize = true;
            stakeholderRoleLabel.Location = new Point(3, 59);
            stakeholderRoleLabel.Margin = new Padding(3, 10, 3, 0);
            stakeholderRoleLabel.Name = "stakeholderRoleLabel";
            stakeholderRoleLabel.Size = new Size(100, 19);
            stakeholderRoleLabel.TabIndex = 2;
            stakeholderRoleLabel.Text = "Role:";
            //
            // stakeholderRole
            //
            StakeholderRole.Location = new Point(110, 57);
            StakeholderRole.Margin = new Padding(3, 6, 3, 3);
            StakeholderRole.Name = "stakeholderRole";
            StakeholderRole.Size = new Size(300, 27);
            StakeholderRole.TabIndex = 3;
            //
            // deleteStakeholderButton
            //
            deleteStakeholderButton.Name = "DeleteStakeholderButton";
            deleteStakeholderButton.UseVisualStyleBackColor = true;
            deleteStakeholderButton.Click += RemoveStakeholder;
            deleteStakeholderButton.TabIndex = 5;
            deleteStakeholderButton.Location = new Point(580, 56);
            deleteStakeholderButton.Size = new Size(150, 30);
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
                StakeholderIndex = Math.Min(StakeholderIndex, Globals.RationallyAddIn.Model.Stakeholders.Count);
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
