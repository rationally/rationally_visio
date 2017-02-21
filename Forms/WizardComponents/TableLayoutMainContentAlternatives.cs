﻿
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Rationally.Visio.EventHandlers.WizardPageHandlers;
using Rationally.Visio.RationallyConstants;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentAlternatives : TableLayoutPanel, IWizardPanel
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public readonly List<FlowLayoutAlternative> AlternativeRows = new List<FlowLayoutAlternative>();

        public TableLayoutMainContentAlternatives()
        {
            


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

            //UpdateRows();
        }

        public void UpdateRows()
        {
            Controls.Clear();
            RowStyles.Clear();

            int numberOfRows = Math.Max(Constants.SupportedAmountOfAlternatives, ProjectSetupWizard.Instance.ModelCopy.Alternatives.Count);
            RowCount = numberOfRows + 1;

            for (int i = 0; i < numberOfRows; i++)
            {
                FlowLayoutAlternative alternativeRow = new FlowLayoutAlternative(i + 1);
                AlternativeRows.Add(alternativeRow);
                RowStyles.Add(new RowStyle(SizeType.Percent, 10F));//TODO what if rowCount > 9
                Controls.Add(alternativeRow, 0, i);
            }

            RowStyles.Add(new RowStyle(SizeType.Percent, 100 - (numberOfRows * 10)));
        }

        public void InitData()
        {
            UpdateRows();
            AlternativeRows.ForEach(a => a.UpdateData());

            Log.Debug("Initialized alternatives wizard page.");
        }

        public bool IsValid()
        {
            bool validFields = ProjectSetupWizard.Instance.TableLayoutMainContentAlternatives.AlternativeRows.TrueForAll(row => (row.Alternative == null) || !string.IsNullOrWhiteSpace(row.TextBoxAlternativeTitle.Text));
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
