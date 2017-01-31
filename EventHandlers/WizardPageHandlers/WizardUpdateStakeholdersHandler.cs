using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using Rationally.Visio.Forms;
using Rationally.Visio.Forms.WizardComponents;
using Rationally.Visio.Model;
using Rationally.Visio.View.Stakeholders;
using static System.String;

namespace Rationally.Visio.EventHandlers.WizardPageHandlers
{
    class WizardUpdateStakeholdersHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Execute(ProjectSetupWizard wizard)
        {
            PleaseWait pleaseWait = new PleaseWait();
            pleaseWait.Show();
            pleaseWait.Refresh();
            Globals.RationallyAddIn.RebuildTree(Globals.RationallyAddIn.Application.ActiveDocument);
            //validation is done here, so just pick the filled in rows
            List<FlowLayoutStakeholder> filledInPanels = wizard.TableLayoutMainContentStakeholders.Stakeholders;
            filledInPanels.ForEach(filledInPanel => filledInPanel.UpdateModel());
            Log.Debug("filled in panels:" + filledInPanels.Count);
            //user might have deleted rows => delete them from the model
            List<Stakeholder> modelStakeholders = Globals.RationallyAddIn.Model.Stakeholders;
            Log.Debug("model stakeholders count:" + modelStakeholders.Count);

            //locate stakeholders in the model for which no element in the wizard exists (anymore)
            List<int> scheduledForDeletion = modelStakeholders.Where((s, i) => wizard.TableLayoutMainContentStakeholders.Stakeholders.FirstOrDefault(stakeholder => stakeholder.StakeholderIndex == i) == null).Select(modelStakeholder => modelStakeholders.IndexOf(modelStakeholder)).ToList();
            Log.Debug("scheduled for deletion:" + scheduledForDeletion.Count);
            StakeholdersContainer stakeholdersContainer = (StakeholdersContainer)Globals.RationallyAddIn.View.Children.First(c => c is StakeholdersContainer);
            Log.Debug("container present:" + (stakeholdersContainer != null));
            //delete these elements from the view, which will automatically remove them from the model
            scheduledForDeletion.ForEach(stakeholderIndex => stakeholdersContainer.Children.Cast<StakeholderContainer>().First(con => con.StakeholderIndex == stakeholderIndex).RShape.Delete());

            //repaint the view according to the new model
            RepaintHandler.Repaint(stakeholdersContainer);
            pleaseWait.Close();
        }
    }
}
