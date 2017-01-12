using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Rationally.Visio.Model;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentStakeholders : TableLayoutPanel, IWizardPanel
    {
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
            //the following lines are a weird hack to enable vertical scrolling without enabling horizontal scrolling:
            HorizontalScroll.Maximum = 0;
            AutoScroll = false;
            VerticalScroll.Visible = false;
            AutoScroll = true;
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

        public void UpdateRows()
        {
            Controls.Clear();
            RowStyles.Clear();

            RowCount = Stakeholders.Count;

            for (int i = 0; i < Stakeholders.Count; i++)
            {
                Controls.Add(Stakeholders[i], 0, i);//add control to view
                RowStyles.Add(new RowStyle(SizeType.Absolute, 100));//style the just added row
            }
        }

        private void AddStakeholderButton_Click(object sender, EventArgs e) => AddStakeholder();

        private void AddStakeholder()
        {
            Stakeholders.Add(new FlowLayoutStakeholder(Stakeholders.Count > 0 ? Stakeholders.Last().StakeholderIndex+1 : 0));
            UpdateRows();
        }

        public bool IsValid() => true;

        public void UpdateData()
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            Stakeholders.Clear();
            for (int i = 0; i < model.Stakeholders.Count; i++)
            {
                Stakeholders.Add(new FlowLayoutStakeholder(i));
            }
            UpdateRows();
        }

        public void UpdateModel()
        {
            throw new NotImplementedException(); 
        }
    }
}
