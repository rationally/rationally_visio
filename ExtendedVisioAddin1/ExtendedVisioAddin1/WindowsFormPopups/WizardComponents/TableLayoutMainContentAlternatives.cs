
using System.Windows.Forms;

namespace Rationally.Visio.WindowsFormPopups.WizardComponents
{
    public class TableLayoutMainContentAlternatives : TableLayoutPanel
    {


        public TableLayoutMainContentAlternatives()
        {

            Init();
        }

        public void Init()
        {
            //
            // alternatives information panel
            //
            BackColor = System.Drawing.SystemColors.Control;
            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            Dock = DockStyle.Fill;
            Location = new System.Drawing.Point(4, 4);
            Margin = new Padding(4);
            Name = "tableLayoutMainContentAlternatives";
            RowCount = 4;
            RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            Size = new System.Drawing.Size(760, 482);
            TabIndex = 0;
            

        }
    }
}
