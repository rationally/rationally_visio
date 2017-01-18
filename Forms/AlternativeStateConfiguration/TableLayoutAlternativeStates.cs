using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rationally.Visio.Forms.AlternativeStateConfiguration
{
    class TableLayoutAlternativeStates : TableLayoutPanel
    {
        public TableLayoutAlternativeStates()
        {
            Init();
        }
        public void Init()
        {
            // 
            // tableLayoutStateContent
            // 
            ColumnCount = 1;
            ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            Dock = System.Windows.Forms.DockStyle.Fill;
            Location = new System.Drawing.Point(3, 3);
            Name = "tableLayoutStateContent";
            RowCount = 1;
            RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            Size = new System.Drawing.Size(489, 272);
            TabIndex = 1;
        }

        public void UpdateRows()
        {
            
        }
    }
}
