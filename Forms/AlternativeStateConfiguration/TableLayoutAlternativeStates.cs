using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.Forms.AlternativeStateConfiguration
{
    internal class TableLayoutAlternativeStates : TableLayoutPanel
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
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Dock = DockStyle.Fill;
            Location = new Point(3, 3);
            Name = "tableLayoutStateContent";
            RowCount = 1;
            RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            Size = new Size(489, 272);
            TabIndex = 1;

            UpdateRows();
        }

        public void UpdateRows()
        {
            Controls.Clear();
            RowStyles.Clear();

            RowCount = StateRows.Count;
            InitScrollBar();

            for (int i = 0; i < StateRows.Count; i++)
            {
                Controls.Add(StateRows[i], 0, i);//add control to view
                RowStyles.Add(new RowStyle(SizeType.Absolute, 40));//style the just added row
            }
        }

        public void AddRow()
        {
            StateRows.Add(new FlowLayoutAlternativeState(StateRows.Count));
            UpdateRows();
        }

        public void Save()
        {
            if (Validate())
            {
                if (!File.Exists(Constants.StateResourceFile))
                {
                    File.Create(Constants.StateResourceFile);
                }
                //write current states to file
                using (ResXResourceWriter resx = new ResXResourceWriter(Constants.StateResourceFile))
                {
                    resx.AddResource("Root", "");
                    for (int i = 0; i < StateRows.Count; i++)
                    {
                        AlternativeState newAlternativeState;
                        Enum.TryParse(StateRows[i].NewState, out newAlternativeState);
                        resx.AddResource("alternativeState" + i, newAlternativeState);
                    }
                }
                //write current states to model
                //StateRows.ForEach(stateRow => Globals.RationallyAddIn.Model.AlternativeStateColors.Add(stateRow.NewState, stateRow.Color)); //NoLongerSupported


                //locate renamed alternative states
                Dictionary<string, string> stateRenames = new Dictionary<string, string>(); //<from,to>
                StateRows.Where(s => (s.OldState != s.NewState) && (s.OldState != null)).ToList().ForEach(s => stateRenames.Add(s.OldState, s.NewState));
                //update renamed alternative states
                Globals.RationallyAddIn.Model.Alternatives
                    .Where(alternative => stateRenames.ContainsKey(alternative.Status)).ToList()
                    .ForEach(alternative => alternative.Status = stateRenames[alternative.Status]);


                //update non-existent alternative states to the default state
                /*Globals.RationallyAddIn.Model.Alternatives
                    .Where(alternative => !Globals.RationallyAddIn.Model.AlternativeStateColors.ContainsKey(alternative.Status)).ToList()
                    .ForEach(alternative => alternative.Status = Globals.RationallyAddIn.Model.AlternativeStateColors.Keys.First());*/


                //repaint all currently present alternative state components
                AlternativesContainer alternativesContainer = (AlternativesContainer) Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is AlternativesContainer);
                //map all alternatives to their state component shape
                IEnumerable<AlternativeStateShape> toUpdate = alternativesContainer?.Children
                    .Select(alt => ((AlternativeShape) alt).Children.First(c => c is AlternativeStateShape))
                    .Cast<AlternativeStateShape>();
                toUpdate?.ToList().ForEach(stateComp => stateComp.Repaint());
            }
            else
            {
                MessageBox.Show("States must all have unique names", "Duplicate State Name Error");
            }
        }

        private bool Validate() => StateRows.Select(row => row.NewState).Distinct().ToList().Count == StateRows.Count;

        public void ReadStates()
        {
            if (!File.Exists(Constants.StateResourceFile))
            {

                using (ResXResourceWriter resx = new ResXResourceWriter(File.Create(Constants.StateResourceFile)))
                {
                    resx.AddResource("Root", "");
                    int i = 0;
                    foreach (string state in Enum.GetNames(typeof(AlternativeState)))
                    {
                        AlternativeState newAlternativeState;
                        Enum.TryParse(state, out newAlternativeState);
                        resx.AddResource("alternativeState" + i, newAlternativeState);
                        i++;
                    }
                }
                
            }

            StateRows = new List<FlowLayoutAlternativeState>();
            Globals.RationallyAddIn.Model.AlternativeStateColorsFromFile.Select(rawState => (AlternativeState)rawState.Value).ToList().ToList().ForEach(stateColor => StateRows.Add(new FlowLayoutAlternativeState(stateColor, StateRows.Count)));

        }

        private void InitScrollBar()
        {
            //the following lines are a weird hack to enable vertical scrolling without enabling horizontal scrolling:
            HorizontalScroll.Maximum = 0;
            AutoScroll = false;
            VerticalScroll.Visible = false;
            AutoScroll = true;
        }
    }
}
