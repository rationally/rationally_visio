using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View.Forces;
using static System.String;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentForces : TableLayoutPanel
    {
        public DataGridView ForcesDataGrid;
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


            this.ForcesDataGrid = new System.Windows.Forms.DataGridView();
            

            // 
            // dataGridView1
            // 
            this.ForcesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ForcesDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.ForcesDataGrid.Location = new System.Drawing.Point(3, 3);
            this.ForcesDataGrid.Name = "forcesDataGrid";
            this.ForcesDataGrid.Size = new System.Drawing.Size(760, 255);
            ForcesDataGrid.MinimumSize = new System.Drawing.Size(760, 255);
            this.ForcesDataGrid.TabIndex = 0;
            ForcesDataGrid.BorderStyle = BorderStyle.None;
            ForcesDataGrid.BackgroundColor = Color.WhiteSmoke;//Color.FromArgb(214, 227, 255);
            ForcesDataGrid.RowsDefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
            ForcesDataGrid.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            ForcesDataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(210, 210, 210);

            ForcesDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(97, 192, 167);
            ForcesDataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            ForcesDataGrid.ColumnHeadersDefaultCellStyle.Font = WizardConstants.HighlightedFont;
            ForcesDataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

            ForcesDataGrid.GridColor = Color.Gray;
            ForcesDataGrid.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(194, 207, 242);
            ForcesDataGrid.EnableHeadersVisualStyles = false;
            InitColumns();
            this.Controls.Add(ForcesDataGrid);
        }

        public void InitColumns()
        {
            this.ForcesDataGrid.Columns.Clear();

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

            this.ForcesDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnConcern,
            this.ColumnDescription});


            //examine the model to see how many alternatives are present on the view, and generate as many columns for the user to fill in force values
            List<Alternative> alternatives = Globals.RationallyAddIn.Model.Alternatives;
            foreach (Alternative alternative in alternatives)
            {
                DataGridViewTextBoxColumn alternativeColumn = new DataGridViewTextBoxColumn();
                alternativeColumn.HeaderText = alternative.IdentifierString;
                alternativeColumn.Name = "ColumnAlternative" + alternative.UniqueIdentifier;
                this.ForcesDataGrid.Columns.Add(alternativeColumn);
            }
        }

        public void InitData()
        {
            //update column count to match current amount of alternatives
            InitColumns();
            //clear current rows
            this.ForcesDataGrid.Rows.Clear();
            //create a row for each force in the view, matching the forces table from the view
            foreach (Force force in Globals.RationallyAddIn.Model.Forces)
            {
                DataGridViewRow newRow = (DataGridViewRow)ForcesDataGrid.RowTemplate.Clone();
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
                ForcesDataGrid.Rows.Add(newRow);
            }
        }

        public bool IsValid()
        {
            bool forceGridIsValid = ForcesDataGrid.Rows.Cast<DataGridViewRow>().ToList().Select(ValidateRow).Aggregate(true, (valid1, valid2) => valid1 && valid2);
            if (!forceGridIsValid)
            {
                MessageBox.Show("Enter a force concern for each force.", "Force Concern Missing");
            }
            return forceGridIsValid;
        }

        private bool ValidateRow(DataGridViewRow row)
        {
            //the whole row must be empty, or a force concern must be entered
            return (row.Cells.Cast<DataGridViewTextBoxCell>().All(cell => IsNullOrEmpty(cell.Value?.ToString())))
                   || (!IsNullOrEmpty(row.Cells[0].Value?.ToString()));
        }
    }
}
