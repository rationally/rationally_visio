﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Rationally.Visio.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.View.Forces;
using static System.String;

namespace Rationally.Visio.EventHandlers.WizardPageHandlers
{
    internal static class WizardUpdateForcesHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Execute(ProjectSetupWizard wizard)
        {
            Globals.RationallyAddIn.RebuildTree(Globals.RationallyAddIn.Application.ActiveDocument);
            //select filled in force rows
            List<DataGridViewRow> forceRows = wizard.TableLayoutMainContentForces.ForcesDataGrid.Rows.Cast<DataGridViewRow>().Where(row => !IsNullOrEmpty(row.Cells[0].Value?.ToString())).ToList();
            Log.Debug("Found " + forceRows.Count + " filled in force rows.");
            ProjectSetupWizard.Instance.ModelCopy.Forces = forceRows.Select(ConstructForce).ToList();
            Log.Debug("Stored forces in model.");
        }

        private static Force ConstructForce(DataGridViewRow row)
        {
            List<Alternative> alternatives = ProjectSetupWizard.Instance.ModelCopy.Alternatives;

            Force force = new Force
            {
                Concern = row.Cells[0].Value?.ToString() ?? ForceConcernComponent.DefaultConcern,
                Description = row.Cells[1].Value?.ToString() ?? ""
            };
            Dictionary<int, string> forceValues = new Dictionary<int, string>();

            List< DataGridViewCell> forceValueCells = row.Cells.Cast<DataGridViewCell>().ToList().Skip(2).ToList();//skip concern and description
            for (int i = 0; i < forceValueCells.Count; i++)
            {
                forceValues.Add(alternatives[i].Id,forceValueCells[i].Value?.ToString() ?? "0");
            }
            force.ForceValueDictionary = forceValues;

            return force;
        }
    }
}
