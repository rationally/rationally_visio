using System;
using System.Drawing;
using System.Windows.Forms;
using Rationally.Visio.Model;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class FlowLayoutPlanningItem : GroupBox
    {
        public int PlanningItemIndex { get; private set; }

        private readonly AntiAliasedLabel planningItemTextLabel;
        internal readonly TextBox PlanningItemText;
        private readonly AntiAliasedButton deletePlanningItemButton;

        public PlanningItem PlanningItem => ProjectSetupWizard.Instance.ModelCopy.PlanningItems.Count > PlanningItemIndex ? ProjectSetupWizard.Instance.ModelCopy.PlanningItems[PlanningItemIndex] : null;

        private readonly AntiAliasedLabel planningItemFinishedLabel;
        internal readonly CheckBox PlanningItemFinished;

        public FlowLayoutPlanningItem(int planningItemIndex)
        {
            PlanningItemIndex = planningItemIndex;

            Dock = DockStyle.Top;
            //this.Anchor = AnchorStyles.Left;
            Location = new Point(3, 3);
            Name = "flowLayoutPanelPlanningItem" + PlanningItemIndex;
            Size = new Size(714, 84);
            TabIndex = 0;

            planningItemTextLabel = new AntiAliasedLabel();
            PlanningItemText = new TextBox();
            planningItemFinishedLabel = new AntiAliasedLabel();
            PlanningItemFinished = new CheckBox();
            deletePlanningItemButton = new AntiAliasedButton();
            SuspendLayout();
            Init();
        }

        private void Init()
        {
            Controls.Add(planningItemTextLabel);
            Controls.Add(PlanningItemText);
            Controls.Add(planningItemFinishedLabel);
            Controls.Add(PlanningItemFinished);
            Controls.Add(deletePlanningItemButton);
            //
            // planningItemTextLabel
            //
            planningItemTextLabel.AutoSize = true;
            planningItemTextLabel.Location = new Point(8, 17);
            planningItemTextLabel.Margin = new Padding(3, 10, 3, 0);
            planningItemTextLabel.Name = "planningItemTextLabel";
            planningItemTextLabel.Size = new Size(100, 19);
            planningItemTextLabel.TabIndex = 0;
            planningItemTextLabel.Text = "Item:";
            //
            // PlanningItemText
            //
            PlanningItemText.Location = new Point(110, 15);
            PlanningItemText.Margin = new Padding(3, 6, 400, 3);
            PlanningItemText.Name = "PlanningItemText";
            PlanningItemText.Size = new Size(350, 27);
            PlanningItemText.TabIndex = 1;
            //
            // planningItemFinishedLabel
            //
            planningItemFinishedLabel.AutoSize = true;
            planningItemFinishedLabel.Location = new Point(8, 52);
            planningItemFinishedLabel.Margin = new Padding(3, 10, 3, 0);
            planningItemFinishedLabel.Name = "planningItemFinishedLabel";
            planningItemFinishedLabel.Size = new Size(100, 19);
            planningItemFinishedLabel.TabIndex = 2;
            planningItemFinishedLabel.Text = "Finished:";
            //
            // PlanningItemFinished
            //
            PlanningItemFinished.Location = new Point(110, 50);
            PlanningItemFinished.Margin = new Padding(3, 6, 3, 3);
            PlanningItemFinished.Name = "planningItemFinished";
            PlanningItemFinished.Size = new Size(350, 27);
            PlanningItemFinished.TabIndex = 3;
            //
            // deletePlanningItemButton
            //
            deletePlanningItemButton.Name = "DeletePlanningItemButton";
            deletePlanningItemButton.UseVisualStyleBackColor = true;
            deletePlanningItemButton.Click += RemovePlanningItem;
            deletePlanningItemButton.TabIndex = 5;
            deletePlanningItemButton.Location = new Point(580, 50);
            deletePlanningItemButton.Size = new Size(150, 27);
            deletePlanningItemButton.Margin = new Padding(3, 0, 3, 3);
            deletePlanningItemButton.Text = "Remove";
        }

        private void RemovePlanningItem(object sender, EventArgs e)
        {
            ProjectSetupWizard.Instance.TableLayoutMainContentPlanningItems.PlanningItems.Remove(this);
            ProjectSetupWizard.Instance.ModelCopy.PlanningItems.Remove(PlanningItem);
            ProjectSetupWizard.Instance.TableLayoutMainContentPlanningItems.UpdateRows();
        }

        public void UpdateModel()
        {
            //if this row represents an existing PlanningItem in the model, update it.
            if (PlanningItem != null)
            {
                PlanningItem.ItemText = PlanningItemText.Text;
                PlanningItem.Finished = PlanningItemFinished.Checked;
            }
            else
            {
                PlanningItem newPlanningItem = new PlanningItem(PlanningItemText.Text, PlanningItemFinished.Checked);
                PlanningItemIndex = Math.Min(PlanningItemIndex, ProjectSetupWizard.Instance.ModelCopy.PlanningItems.Count);
                ProjectSetupWizard.Instance.ModelCopy.PlanningItems.Insert(PlanningItemIndex, newPlanningItem);
            }
        }

        public void UpdateData()
        {
            if (PlanningItem != null)
            {
                PlanningItemText.Text = PlanningItem.ItemText;
                PlanningItemFinished.Checked = PlanningItem.Finished;
            }
        }
    }
}
