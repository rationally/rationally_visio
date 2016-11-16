
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentAlternatives : TableLayoutPanel
    {

        public readonly List<FlowLayoutAlternative> AlternativeRows = new List<FlowLayoutAlternative>();

        public TableLayoutMainContentAlternatives()
        {
            RowCount = RationallyConstants.Constants.SupportedAmountOfAlternatives+1;
            
            for (int i = 0; i < RationallyConstants.Constants.SupportedAmountOfAlternatives;i++) 
            {
                FlowLayoutAlternative alternativeRow = new FlowLayoutAlternative(i+1);
                AlternativeRows.Add(alternativeRow);
                RowStyles.Add(new RowStyle(SizeType.Percent, 10F));//TODO what if rowCount > 9
                Controls.Add(alternativeRow,0,i);
            }

            RowStyles.Add(new RowStyle(SizeType.Percent, 100- RationallyConstants.Constants.SupportedAmountOfAlternatives * 10));

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
            Location = new System.Drawing.Point(4, 4);
            Margin = new Padding(4);
            Name = "tableLayoutMainContentAlternatives";
            
            Size = new System.Drawing.Size(760, 482);
            TabIndex = 0;

        }
    }
}
