using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Rationally.Visio.EventHandlers.WizardPageHandlers;
using Rationally.Visio.Model;
using static System.String;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentPlanningItems : TableLayoutPanel, IWizardPanel
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public readonly List<FlowLayoutPlanningItem> PlanningItems;
        public readonly AntiAliasedButton AddPlanningItemButton;

        public TableLayoutMainContentPlanningItems()
        {
            PlanningItems = new List<FlowLayoutPlanningItem>();
            AddPlanningItemButton = new AntiAliasedButton();
            Init();
        }

        private void Init()
        {
            BackColor = Color.WhiteSmoke;
            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Dock = DockStyle.Fill;
            Location = new Point(4, 4);
            Size = new Size(760, 482);
            Margin = new Padding(4);
            Name = "tableLayoutMainContentPlanningItems";
            //
            // addStakeholderButton
            //
            AddPlanningItemButton.Name = "AddPlanningItemButton";
            AddPlanningItemButton.UseVisualStyleBackColor = true;
            AddPlanningItemButton.Click += AddPlanningItemButton_Click;
            AddPlanningItemButton.Text = "Add Planning item";
            AddPlanningItemButton.Size = new Size(200, 34);
            AddPlanningItemButton.Margin = new Padding(0, 0, 360, 0);
            AddPlanningItemButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;


            UpdateRows();
        }

        private void InitScrollBar()
        {
            //the following lines are a weird hack to enable vertical scrolling without enabling horizontal scrolling:
            HorizontalScroll.Maximum = 0;
            AutoScroll = false;
            VerticalScroll.Visible = false;
            AutoScroll = true;
        }

        public void UpdateRows()
        {
            Controls.Clear();
            RowStyles.Clear();

            RowCount = PlanningItems.Count;

            InitScrollBar();
            for (int i = 0; i < PlanningItems.Count; i++)
            {
                Controls.Add(PlanningItems[i], 0, i);//add control to view
                RowStyles.Add(new RowStyle(SizeType.Absolute, 95));//style the just added row
            }
        }

        private void AddPlanningItemButton_Click(object sender, EventArgs e) => AddPlanningItem();

        private void AddPlanningItem()
        {
            PlanningItems.Add(new FlowLayoutPlanningItem(PlanningItems.Count > 0 ? PlanningItems.Last().PlanningItemIndex+1 : 0));
            UpdateRows();
        }

        public void InitData()
        {
            RationallyModel model = ProjectSetupWizard.Instance.ModelCopy;
            PlanningItems.Clear();
            for (int i = 0; i < model.PlanningItems.Count; i++)
            {
                PlanningItems.Add(new FlowLayoutPlanningItem(i));
            }
            UpdateRows();
            PlanningItems.ForEach(d => d.UpdateData());
            Log.Debug("Initialized planning items wizard page.");
        }

        public bool IsValid()
        {
            //check if all rows have an entry
            if (PlanningItems.Any(pi => IsNullOrWhiteSpace(pi.PlanningItemText.Text)))
            {
                MessageBox.Show("For all planning items, enter a description.");
                return false;
            }
            return true;
        }
        
        public void UpdateModel()
        {
            //handle changes in the "Stakeholders" page
            WizardUpdatePlanningHandler.Execute(ProjectSetupWizard.Instance);
            Log.Debug("PlanningItems updated.");
        }
    }
}
