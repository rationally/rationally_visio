
using System.Windows.Forms;

namespace Rationally.Visio.WindowsFormPopups.WizardComponents
{
    public class TableLayoutMainContentAlternatives : TableLayoutPanel
    {
        public FlowLayoutAlternative flowLayoutPanelAlternative1 = new FlowLayoutAlternative(1);
        public FlowLayoutAlternative flowLayoutPanelAlternative2 = new FlowLayoutAlternative(2);
        public FlowLayoutAlternative flowLayoutPanelAlternative3 = new FlowLayoutAlternative(3);

        public TableLayoutMainContentAlternatives()
        {
            Controls.Add(this.flowLayoutPanelAlternative1, 0, 0);
            Controls.Add(this.flowLayoutPanelAlternative2, 0, 1);
            Controls.Add(this.flowLayoutPanelAlternative3, 0, 2);
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
            this.flowLayoutPanelAlternative1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelAlternative1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelAlternative1.Name = "flowLayoutPanelAlternative1";
            this.flowLayoutPanelAlternative1.Size = new System.Drawing.Size(754, 42);
            this.flowLayoutPanelAlternative1.TabIndex = 0;
            // 
            // flowLayoutPanelAlternative2
            // 
            this.flowLayoutPanelAlternative2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelAlternative2.Location = new System.Drawing.Point(3, 51);
            this.flowLayoutPanelAlternative2.Name = "flowLayoutPanelAlternative2";
            this.flowLayoutPanelAlternative2.Size = new System.Drawing.Size(754, 42);
            this.flowLayoutPanelAlternative2.TabIndex = 0;
            // 
            // flowLayoutPanelAlternative3
            // 
            this.flowLayoutPanelAlternative3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelAlternative3.Location = new System.Drawing.Point(3, 99);
            this.flowLayoutPanelAlternative3.Name = "flowLayoutPanelAlternative3";
            this.flowLayoutPanelAlternative3.Size = new System.Drawing.Size(754, 42);
            this.flowLayoutPanelAlternative3.TabIndex = 0;
        }
    }
}
