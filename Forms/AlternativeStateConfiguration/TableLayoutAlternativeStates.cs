using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;

namespace Rationally.Visio.Forms.AlternativeStateConfiguration
{
    class TableLayoutAlternativeStates : TableLayoutPanel
    {
        public List<FlowLayoutAlternativeState> StateRows = new List<FlowLayoutAlternativeState>();

        public TableLayoutAlternativeStates()
        {
            ReadStates();
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

            UpdateRows();
        }

        public void UpdateRows()
        {
            Controls.Clear();
            RowStyles.Clear();

            RowCount = StateRows.Count;
            //InitScrollBar();

            for (int i = 0; i < StateRows.Count; i++)
            {
                Controls.Add(StateRows[i], 0, i);//add control to view
                RowStyles.Add(new RowStyle(SizeType.Absolute, 40));//style the just added row
            }
        }

        public void AddRow()
        {
            StateRows.Add(new FlowLayoutAlternativeState(Constants.DefaultStateName, Constants.DefaultStateColor, StateRows.Count));
            UpdateRows();
        }

        public void Save()
        {

            if (!File.Exists(Constants.StateResourceFile))
            {
                File.Create(Constants.StateResourceFile);
            }
            using (ResXResourceWriter resx = new ResXResourceWriter(Constants.StateResourceFile))
            {
                resx.AddResource("Root","");
                for (int i = 0; i < StateRows.Count; i++)
                {
                    resx.AddResource("alternativeState" + i, new AlternativeState { Color = StateRows[i].Color, State = StateRows[i].StateTextBox.Text });
                }
            }

        }

        public void ReadStates()
        {
            if (!File.Exists(Constants.StateResourceFile))
            {
                
                using (ResXResourceWriter resx = new ResXResourceWriter(File.Create(Constants.StateResourceFile)))
                {
                    resx.AddResource("Root", "");
                }
            }
            if (File.Exists(Constants.StateResourceFile))
            {
                using (ResXResourceReader resxReader = new ResXResourceReader(Constants.StateResourceFile))
                {
                    StateRows = new List<FlowLayoutAlternativeState>();
                    foreach (DictionaryEntry entry in resxReader)
                    {
                        if (((string) entry.Key).StartsWith("alternativeState"))
                        {
                            StateRows.Add(new FlowLayoutAlternativeState((AlternativeState) entry.Value, StateRows.Count));
                        }
                    }
                }
            }
        }
    }
}
