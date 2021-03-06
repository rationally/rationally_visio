﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Rationally.Visio.EventHandlers.WizardPageHandlers;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;
using static System.String;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentForces : TableLayoutPanel, IWizardPanel
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public DataGridView ForcesDataGrid;

        public DataGridViewTextBoxColumn ColumnId { get; set; }

        public DataGridViewTextBoxColumn ColumnDescription { get; set; }

        public DataGridViewTextBoxColumn ColumnConcern { get; set; }

        public TableLayoutMainContentForces()
        {
            Init();
        }

        public void Init()
        {
            BackColor = Color.WhiteSmoke;
            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            Dock = DockStyle.Fill;
            Location = new Point(4, 4);
            Margin = new Padding(4);
            Name = "tableLayoutMainContentForces";

            Size = new Size(760, 482);
            TabIndex = 0;

            //data grid


            ForcesDataGrid = new DataGridView
            {
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Location = new Point(3, 3),
                Name = "forcesDataGrid",
                Size = new Size(760, 255),
                MinimumSize = new Size(760, 255),
                TabIndex = 0,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.WhiteSmoke,
                RowsDefaultCellStyle = {BackColor = Color.FromArgb(230, 230, 230)},
                GridColor = Color.Gray,
                EnableHeadersVisualStyles = false,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            };


            ForcesDataGrid.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            ForcesDataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(210, 210, 210);
            
            ForcesDataGrid.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(194, 207, 242);

            ForcesDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(97, 192, 167);
            ForcesDataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            ForcesDataGrid.ColumnHeadersDefaultCellStyle.Font = WizardConstants.HighlightedFont;
            //InitColumns();
            Controls.Add(ForcesDataGrid);
        }

        private void InitColumns()
        {
            ForcesDataGrid.Columns.Clear();

            ColumnId = new DataGridViewTextBoxColumn {Visible = false};

            //add the two base columns of a force: concern and description
            ColumnConcern = new DataGridViewTextBoxColumn();
            ColumnDescription = new DataGridViewTextBoxColumn();
            // 
            // ColumnConcern
            // 
            ColumnConcern.HeaderText = "Concern";
            ColumnConcern.Name = "ColumnConcern";
            // 
            // ColumnDescription
            // 
            ColumnDescription.HeaderText = "Force";
            ColumnDescription.Name = "ColumnDescription"; 

            ForcesDataGrid.Columns.AddRange(ColumnId, ColumnConcern, ColumnDescription);


            //examine the model to see how many alternatives are present on the view, and generate as many columns for the user to fill in force values
            List<Alternative> alternatives = ProjectSetupWizard.Instance.ModelCopy.Alternatives;
            foreach (DataGridViewTextBoxColumn alternativeColumn in alternatives.Select(alternative => new DataGridViewTextBoxColumn{ HeaderText = alternative.IdentifierString, Name = "ColumnAlternative" + alternative.Id}))
            {
                ForcesDataGrid.Columns.Add(alternativeColumn);
            }
        }

        public void InitData()
        {
            //update column count to match current amount of alternatives
            InitColumns();
            //clear current rows
            ForcesDataGrid.Rows.Clear();
            //create a row for each force in the view, matching the forces table from the view
            foreach (Force force in ProjectSetupWizard.Instance.ModelCopy.Forces)
            {
                DataGridViewRow newRow = (DataGridViewRow)ForcesDataGrid.RowTemplate.Clone();
                newRow.Cells.Add(new DataGridViewTextBoxCell { Value = force.Id});
                newRow.Cells.Add(new DataGridViewTextBoxCell {Value = force.Concern});
                newRow.Cells.Add(new DataGridViewTextBoxCell { Value = force.Description });
                foreach (Alternative alternative in ProjectSetupWizard.Instance.ModelCopy.Alternatives)
                {
                    newRow.Cells.Add(new DataGridViewTextBoxCell {Value = force.ForceValueDictionary[alternative.Id]});
                }
                ForcesDataGrid.Rows.Add(newRow);
            }
            Log.Debug($"Initialised forces table with {ProjectSetupWizard.Instance.ModelCopy.Alternatives.Count} alternatives.");
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

        private bool ValidateRow(DataGridViewRow row) => row.Cells.Cast<DataGridViewTextBoxCell>().All(cell => IsNullOrEmpty(cell.Value?.ToString()))
                                                         || !IsNullOrEmpty(row.Cells[1].Value?.ToString());

        public void UpdateModel()
        {
            //handle changes in the "Forces" page
            WizardUpdateForcesHandler.Execute(ProjectSetupWizard.Instance);
            Log.Debug("Forces updated.");
        }
    }
}
