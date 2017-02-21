using System.Linq;
using System.Reflection;
using log4net;
using Rationally.Visio.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Information;

namespace Rationally.Visio.EventHandlers.WizardPageHandlers
{
    internal static class WizardUpdateGeneralInformationHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Execute(ProjectSetupWizard wizard) => UpdateGeneralInformationInModel(wizard.tableLayoutMainContentGeneral.TextAuthor.Text,
            wizard.tableLayoutMainContentGeneral.TextDecisionTopic.Text,
            wizard.tableLayoutMainContentGeneral.DateTimePickerCreationDate.Value.ToLongDateString(),
            wizard.tableLayoutMainContentGeneral.TextVersion.Text);


        private static void UpdateGeneralInformationInModel(string author, string decisionName, string date, string version)
        {
            Globals.RationallyAddIn.RebuildTree(Globals.RationallyAddIn.Application.ActiveDocument);
            RationallyModel model = ProjectSetupWizard.Instance.ModelCopy;

            // Read the contents of setupDialog's TextBox.
            model.Author = author;
            model.DecisionName = decisionName;
            model.DateString = date;
            model.Version = version;
            Log.Debug("Wrote data to copy of model: (" + author + "," + decisionName + "," + date + "," + version + ")");
        }
    }
}
