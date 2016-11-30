using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentForces : TableLayoutPanel
    {
        private DataGridView forcesDataGrid;
        public DataGridViewTextBoxColumn ColumnDescription { get; set; }

        public DataGridViewTextBoxColumn ColumnConcern { get; set; }

        public TableLayoutMainContentForces()
        {
            Init();
        }

        private void Init()
        {
            BackColor = Color.WhiteSmoke;
            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            Dock = DockStyle.Fill;
            Location = new System.Drawing.Point(4, 4);
            Margin = new Padding(4);
            Name = "tableLayoutMainContentForces";

            Size = new System.Drawing.Size(760, 482);
            TabIndex = 0;

            //data grid


            this.forcesDataGrid = new System.Windows.Forms.DataGridView();
            

            // 
            // dataGridView1
            // 
            this.forcesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            
            this.forcesDataGrid.Location = new System.Drawing.Point(3, 3);
            this.forcesDataGrid.Name = "forcesDataGrid";
            this.forcesDataGrid.Size = new System.Drawing.Size(760, 255);
            this.forcesDataGrid.TabIndex = 0;
            forcesDataGrid.BorderStyle = BorderStyle.None;
            forcesDataGrid.BackgroundColor = Color.WhiteSmoke;

            InitColumns();
            this.Controls.Add(forcesDataGrid);
        }

        public void InitColumns()
        {
            this.forcesDataGrid.Columns.Clear();

            //add the two base columns of a force: concern and description
            this.ColumnConcern = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            // 
            // ColumnConcern
            // 
            this.ColumnConcern.HeaderText = "Concern";
            this.ColumnConcern.Name = "ColumnConcern";
            // 
            // ColumnDescription
            // 
            this.ColumnDescription.HeaderText = "Description";
            this.ColumnDescription.Name = "ColumnDescription"; 

            this.forcesDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnConcern,
            this.ColumnDescription});


            //examine the model to see how many alternatives are present on the view, and generate as many columns for the user to fill in force values
            List<Alternative> alternatives = Globals.RationallyAddIn.Model.Alternatives;
            foreach (Alternative alternative in alternatives)
            {
                DataGridViewTextBoxColumn alternativeColumn = new DataGridViewTextBoxColumn();
                alternativeColumn.HeaderText = alternative.IdentifierString;
                alternativeColumn.Name = "ColumnAlternative" + alternative.UniqueIdentifier;
                this.forcesDataGrid.Columns.Add(alternativeColumn);
            }
        }

        public void InitData()
        {
            //update column count to match current amount of alternatives
            InitColumns();
            //clear current rows
            this.forcesDataGrid.Rows.Clear();
            //create a row for each force in the view, matching the forces table from the view
            foreach (Force force in Globals.RationallyAddIn.Model.Forces)
            {
                DataGridViewRow newRow = (DataGridViewRow)forcesDataGrid.RowTemplate.Clone();
                newRow.Cells.Add(new DataGridViewTextBoxCell() {Value = force.Concern});
                newRow.Cells.Add(new DataGridViewTextBoxCell() { Value = force.Description });
                //newRow.Cells[0].Value = force.Concern;
                //newRow.Cells[1].Value = force.Description;
                int i = 2;
                foreach (Alternative alternative in Globals.RationallyAddIn.Model.Alternatives)
                {
                    newRow.Cells.Add(new DataGridViewTextBoxCell() {Value = force.ForceValueDictionary[alternative.UniqueIdentifier]});
                }
                    //newRow.Cells[i++].Value = force.ForceValueDictionary[alternative.UniqueIdentifier];

            }
        }
    }
}
