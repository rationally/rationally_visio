using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Rationally.Visio.Model;
using static System.String;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentStakeholders : TableLayoutPanel, IWizardPanel
    {
        public readonly List<FlowLayoutStakeholder> Stakeholders;
        private readonly AntiAliasedButton addStakeholderButton;

        public TableLayoutMainContentStakeholders()
        {
            Stakeholders = new List<FlowLayoutStakeholder>();
            addStakeholderButton = new AntiAliasedButton();
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
            addStakeholderButton.Name = "AddStakeholderButton";
            addStakeholderButton.UseVisualStyleBackColor = true;
            addStakeholderButton.Click += AddStakeholderButton_Click;
            addStakeholderButton.Text = "Add Stakeholder";
            addStakeholderButton.Size = new Size(200, 30);
            addStakeholderButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;


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

            RowCount = Stakeholders.Count + 1;//+ row with "add stakeholders" button

            for (int i = 0; i < Stakeholders.Count; i++)
            {
                Controls.Add(Stakeholders[i], 0, i);//add control to view
                RowStyles.Add(new RowStyle(SizeType.Absolute, 100));//style the just added row
            }
            Controls.Add(addStakeholderButton, 0, RowCount - 1);//c-indexed
            RowStyles.Add(new RowStyle(SizeType.AutoSize));//add a style for the add file button
            InitScrollBar();
        }

        private void AddStakeholderButton_Click(object sender, EventArgs e) => AddStakeholder();

        private void AddStakeholder()
        {
            Stakeholders.Add(new FlowLayoutStakeholder(Stakeholders.Count > 0 ? Stakeholders.Last().StakeholderIndex+1 : 0));
            UpdateRows();
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
