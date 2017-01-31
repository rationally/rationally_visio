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
            PleaseWait pleaseWait = new PleaseWait();
            pleaseWait.Show();
            pleaseWait.Refresh();
            Globals.RationallyAddIn.RebuildTree(Globals.RationallyAddIn.Application.ActiveDocument);
            RationallyModel model = Globals.RationallyAddIn.Model;

            // Read the contents of setupDialog's TextBox.
            model.Author = author;
            model.DecisionName = decisionName;
            model.DateString = date;
            model.Version = version;
            Log.Debug("Wrote data to model: (" + author + "," + decisionName + "," + date + "," + version + ")");
            
            if (ProjectSetupWizard.DocumentCreation)
            {
                //draw the header
                TitleLabel header = new TitleLabel(Globals.RationallyAddIn.Application.ActivePage, model.DecisionName);
                Globals.RationallyAddIn.View.Children.Add(header);
                RepaintHandler.Repaint(header);
                Log.Debug("Added title component to the sheet.");
                //draw the information container
                InformationContainer informationContainer = new InformationContainer(Globals.RationallyAddIn.Application.ActivePage, model.Author, model.DateString, model.Version);
                Globals.RationallyAddIn.View.Children.Add(informationContainer);
                RepaintHandler.Repaint(informationContainer);
                Log.Debug("Added information container to the sheet.");
                ProjectSetupWizard.DocumentCreation = false;
            }
            else
            {
                Log.Debug("not the first time general info gets repainted, only repaint existing components.");
                RationallyView view = Globals.RationallyAddIn.View;
                if (view.Children.Any(x => x is InformationContainer))
                {
                    InformationContainer container = view.Children.FirstOrDefault(x => x is InformationContainer) as InformationContainer;
                    RepaintHandler.Repaint(container);
                }
                if (view.Children.Any(x => x is TitleLabel))
                {
                    TitleLabel titleLabel = view.Children.FirstOrDefault(x => x is TitleLabel) as TitleLabel;
                    RepaintHandler.Repaint(titleLabel);
                }
            }
            pleaseWait.Close();
        }
    }
}
