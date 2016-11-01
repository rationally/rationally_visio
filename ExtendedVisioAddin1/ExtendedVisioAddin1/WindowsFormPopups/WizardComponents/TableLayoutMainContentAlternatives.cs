
using System.Windows.Forms;

namespace Rationally.Visio.WindowsFormPopups.WizardComponents
{
    public class TableLayoutMainContentAlternatives : TableLayoutPanel
    {
        public FlowLayoutAlternative FlowLayoutPanelAlternative1 = new FlowLayoutAlternative(1);
        public FlowLayoutAlternative FlowLayoutPanelAlternative2 = new FlowLayoutAlternative(2);
        public FlowLayoutAlternative FlowLayoutPanelAlternative3 = new FlowLayoutAlternative(3);

        public TableLayoutMainContentAlternatives()
        {
            Controls.Add(FlowLayoutPanelAlternative1, 0, 0);
            Controls.Add(FlowLayoutPanelAlternative2, 0, 1);
            Controls.Add(FlowLayoutPanelAlternative3, 0, 2);
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

            // 
            // flowLayoutPanelAlternative1
            // 
            FlowLayoutPanelAlternative1.Dock = DockStyle.Fill;
            FlowLayoutPanelAlternative1.Location = new System.Drawing.Point(3, 3);
            FlowLayoutPanelAlternative1.Name = "flowLayoutPanelAlternative1";
            FlowLayoutPanelAlternative1.Size = new System.Drawing.Size(754, 42);
            FlowLayoutPanelAlternative1.TabIndex = 0;
            // 
            // flowLayoutPanelAlternative2
            // 
            FlowLayoutPanelAlternative2.Dock = DockStyle.Fill;
            FlowLayoutPanelAlternative2.Location = new System.Drawing.Point(3, 51);
            FlowLayoutPanelAlternative2.Name = "flowLayoutPanelAlternative2";
            FlowLayoutPanelAlternative2.Size = new System.Drawing.Size(754, 42);
            FlowLayoutPanelAlternative2.TabIndex = 0;
            // 
            // flowLayoutPanelAlternative3
            // 
            FlowLayoutPanelAlternative3.Dock = DockStyle.Fill;
            FlowLayoutPanelAlternative3.Location = new System.Drawing.Point(3, 99);
            FlowLayoutPanelAlternative3.Name = "flowLayoutPanelAlternative3";
            FlowLayoutPanelAlternative3.Size = new System.Drawing.Size(754, 42);
            FlowLayoutPanelAlternative3.TabIndex = 0;
        }
    }
}
