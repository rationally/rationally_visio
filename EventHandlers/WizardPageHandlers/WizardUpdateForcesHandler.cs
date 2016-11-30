using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.View.Forces;
using static System.String;

namespace Rationally.Visio.EventHandlers.WizardPageHandlers
{
    class WizardUpdateForcesHandler
    {
        public static void Execute(ProjectSetupWizard wizard)
        {
            //clear the forces part of the model
            Globals.RationallyAddIn.Model.Forces.Clear();
            //select filled in force rows
            List<DataGridViewRow> forceRows = wizard.TableLayoutMainContentForces.ForcesDataGrid.Rows.Cast<DataGridViewRow>().Where(row => !IsNullOrEmpty(row.Cells[0].Value?.ToString())).ToList();

            Globals.RationallyAddIn.Model.Forces = forceRows.Select(ConstructForce).ToList();
            RepaintHandler.Repaint(Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is ForcesContainer));
        }

        private static Force ConstructForce(DataGridViewRow row)
        {
            List<Alternative> alternatives = Globals.RationallyAddIn.Model.Alternatives;

            Force force = new Force
            {
                Concern = row.Cells[0].Value?.ToString() ?? "",
                Description = row.Cells[1].Value?.ToString() ?? ""
            };
            Dictionary<int, string> forceValues = new Dictionary<int, string>();

            List< DataGridViewCell> forceValueCells = row.Cells.Cast<DataGridViewCell>().ToList().Skip(2).ToList();//skip concern and description
            for (int i = 0; i < forceValueCells.Count; i++)
            {
                forceValues.Add(alternatives[i].UniqueIdentifier,forceValueCells[i].Value?.ToString() ?? "");
            }
            force.ForceValueDictionary = forceValues;

            return force;
        }
    }
}
