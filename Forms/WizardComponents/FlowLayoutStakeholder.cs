using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class FlowLayoutStakeholder : FlowLayoutPanel
    {
        public readonly int StakeholderIndex;
        private readonly AntiAliasedLabel stakeholderNameLabel;
        internal readonly TextBox StakeholderName;
        private readonly AntiAliasedButton deleteStakeholderButton;

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
            deleteStakeholderButton = new AntiAliasedButton();
            SuspendLayout();
            Init();
        }

        private void Init()
        {
            Controls.Add(stakeholderNameLabel);
            Controls.Add(StakeholderName);
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
            ProjectSetupWizard.Instance.TableLayoutMainContentStakeholders.Stakeholders.RemoveAt(StakeholderIndex);
            ProjectSetupWizard.Instance.TableLayoutMainContentStakeholders.UpdateRows();
        }
    }
}
