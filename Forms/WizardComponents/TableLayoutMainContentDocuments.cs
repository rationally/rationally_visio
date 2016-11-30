using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rationally.Visio.Forms.WizardComponents
{
    class TableLayoutMainContentDocuments : TableLayoutPanel
    {
        private List<FlowLayoutDocument> documents;
        public TableLayoutMainContentDocuments()
        {
            documents = new List<FlowLayoutDocument>() { new FlowLayoutDocument() };
            Init();
        }

        private void Init()
        {
            BackColor = Color.WhiteSmoke;
            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Controls.Add(documents[0],0,0);
            Dock = DockStyle.Fill;
            Location = new System.Drawing.Point(4, 4);
            Margin = new Padding(4);
            Name = "tableLayoutMainContentDocuments";
            RowCount = 1;

        }
    }
}
