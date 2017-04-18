
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Rationally.Visio.EventHandlers.WizardPageHandlers;
using Rationally.Visio.Model;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentAlternatives : TableLayoutPanel, IWizardPanel
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public readonly List<FlowLayoutAlternative> Alternatives;

        public readonly AntiAliasedButton AddAlternativeButton;

        public TableLayoutMainContentAlternatives()
        {
            Alternatives = new List<FlowLayoutAlternative>();

            AddAlternativeButton = new AntiAliasedButton();
            Init();
        }

        public void Init()
        {
            //
            // alternatives information panel
            //
            BackColor = Color.WhiteSmoke;
            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            Dock = DockStyle.Fill;
            Location = new Point(4, 4);
            Margin = new Padding(4);
            Name = "tableLayoutMainContentAlternatives";
            
            Size = new Size(760, 482);
            TabIndex = 0;
            //
            // addAlternativeButton
            //
            AddAlternativeButton.Name = "AddAlternativeButton";
            AddAlternativeButton.UseVisualStyleBackColor = true;
            AddAlternativeButton.Click += AddAlternativeButton_Click;
            AddAlternativeButton.Text = "Add Alternative";
            AddAlternativeButton.Size = new Size(200, 34);
            AddAlternativeButton.Margin = new Padding(0, 0, 360, 0);
            AddAlternativeButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            UpdateRows();
        }

        private void InitScrollBar()
        {
            //the following lines are a weird hack to enable vertical scrolling without enabling horizontal scrolling:
            HorizontalScroll.Maximum = 0;
            AutoScroll = false;
            //VerticalScroll.Visible = false;
            AutoScroll = true;
        }

        public void UpdateRows()
        {
            Controls.Clear();
            RowStyles.Clear();

            RowCount = Alternatives.Count;
            InitScrollBar();

            //update alternative identifier strings

            for (int i = 0; i < Alternatives.Count; i++)
            {
                Alternatives[i].Alternative.GenerateIdentifier(i);
                Controls.Add(Alternatives[i], 0, i);//add control to view
                
                RowStyles.Add(new RowStyle(SizeType.Absolute, 95));//style the just added row
            }
        }

        private void AddAlternativeButton_Click(object sender, EventArgs e) => AddAlternative();

        private void AddAlternative()
        {
            Alternatives.Add(new FlowLayoutAlternative());
            UpdateRows();
        }

        public void InitData()
        {
            RationallyModel model = ProjectSetupWizard.Instance.ModelCopy;
            Alternatives.Clear();
            //for each present alternative in the model, create a representing row in the wizard panel
            model.Alternatives.ForEach(alt => Alternatives.Add(new FlowLayoutAlternative(alt)));

            UpdateRows();
            Alternatives.ForEach(d => d.UpdateData());
            Log.Debug("Initialized alternatives wizard page.");
        }

        public bool IsValid()
        {
            bool validFields = ProjectSetupWizard.Instance.TableLayoutMainContentAlternatives.Alternatives.TrueForAll(row => (row.Alternative == null) || !string.IsNullOrWhiteSpace(row.TextBoxAlternativeTitle.Text));
            if (!validFields)
            {
                MessageBox.Show("Enter a name for every existing alternative.", "Alternative name missing");
            }
            return validFields;
        }
        

        public void UpdateModel()
        {
            //handle changes in the "Alternatives" page
            WizardUpdateAlternativesHandler.Execute(ProjectSetupWizard.Instance);
            Log.Debug("Alternatives updated.");
        }
    }
}
