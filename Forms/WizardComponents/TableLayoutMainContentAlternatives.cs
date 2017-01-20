
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentAlternatives : TableLayoutPanel, IWizardPanel
    {

        public readonly List<FlowLayoutAlternative> AlternativeRows = new List<FlowLayoutAlternative>();

        public TableLayoutMainContentAlternatives() 
        {
            

            Init();
        }

        private void Init()
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
            UpdateRows();
        }

        private void UpdateRows()
        {
            Controls.Clear();
            RowStyles.Clear();
            int numberOfRows = Math.Max(RationallyConstants.Constants.SupportedAmountOfAlternatives, Globals.RationallyAddIn.Model.Alternatives.Count);
            RowCount = numberOfRows;

            for (int i = 0; i < numberOfRows; i++)
            {
                FlowLayoutAlternative alternativeRow = new FlowLayoutAlternative(i + 1);
                AlternativeRows.Add(alternativeRow);
                RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
                Controls.Add(alternativeRow, 0, i);
            }

            RowStyles.Add(new RowStyle(SizeType.Percent, 100 - (numberOfRows * 10)));
        }

        public void UpdateModel()
        {
            throw new System.NotImplementedException();
        }
    }
}
