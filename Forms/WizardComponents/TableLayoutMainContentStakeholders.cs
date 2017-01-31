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
    public class TableLayoutMainContentStakeholders : TableLayoutPanel, IWizardPanel
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public readonly List<FlowLayoutStakeholder> Stakeholders;
        public readonly AntiAliasedButton AddStakeholderButton;

        public TableLayoutMainContentStakeholders()
        {
            Stakeholders = new List<FlowLayoutStakeholder>();
            AddStakeholderButton = new AntiAliasedButton();
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
            Name = "tableLayoutMainContentStakeholders";
            //
            // addStakeholderButton
            //
            AddStakeholderButton.Name = "AddStakeholderButton";
            AddStakeholderButton.UseVisualStyleBackColor = true;
            AddStakeholderButton.Click += AddStakeholderButton_Click;
            AddStakeholderButton.Text = "Add Stakeholder";
            AddStakeholderButton.Size = new Size(200, 34);
            AddStakeholderButton.Margin = new Padding(0, 0, 360, 0);
            AddStakeholderButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;


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

            RowCount = Stakeholders.Count;

            InitScrollBar();
            for (int i = 0; i < Stakeholders.Count; i++)
            {
                Controls.Add(Stakeholders[i], 0, i);//add control to view
                RowStyles.Add(new RowStyle(SizeType.Absolute, 95));//style the just added row
            }
        }

        private void AddStakeholderButton_Click(object sender, EventArgs e) => AddStakeholder();

        private void AddStakeholder()
        {
            Stakeholders.Add(new FlowLayoutStakeholder(Stakeholders.Count > 0 ? Stakeholders.Last().StakeholderIndex+1 : 0));
            UpdateRows();
        }

        public void InitData()
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            Stakeholders.Clear();
            for (int i = 0; i < model.Stakeholders.Count; i++)
            {
                Stakeholders.Add(new FlowLayoutStakeholder(i));
            }
            UpdateRows();
            Stakeholders.ForEach(d => d.UpdateData());
            Log.Debug("Initialized stakeholders wizard page.");
        }

        public bool IsValid()
        {
            //check if all rows have an entry
            if (Stakeholders.Any(sh => IsNullOrEmpty(sh.StakeholderName.Text) && IsNullOrEmpty(sh.StakeholderRole.Text)))
            {
                MessageBox.Show("For all stakeholders, enter at least a name or a role.");
                return false;
            }
            return true;
        }
        
        public void UpdateModel()
        {
            //handle changes in the "Stakeholders" page
            WizardUpdateStakeholdersHandler.Execute(ProjectSetupWizard.Instance);
            Log.Debug("Stakeholders updated.");
        }
    }
}
