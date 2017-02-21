using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Rationally.Visio.Forms;
using Rationally.Visio.Forms.WizardComponents;
using Rationally.Visio.Model;
using Rationally.Visio.View.Documents;
using static System.String;

namespace Rationally.Visio.EventHandlers.WizardPageHandlers
{
    internal class WizardUpdateDocumentsHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Execute(ProjectSetupWizard wizard)
        {
            Globals.RationallyAddIn.RebuildTree(Globals.RationallyAddIn.Application.ActiveDocument);
            //validation is done here, so just pick the filled in rows
            List<FlowLayoutDocument> filledInPanels = wizard.TableLayoutMainContentDocuments.Documents.Where(doc => !IsNullOrEmpty(doc.FilePath.Text)).ToList();
            filledInPanels.ForEach(filledInPanel => filledInPanel.UpdateModel());
            Log.Debug("filled in panels:" + filledInPanels.Count);
            //user might have deleted rows => delete them from the model
            List<RelatedDocument> modelDocuments = ProjectSetupWizard.Instance.ModelCopy.Documents;
            Log.Debug("model document count:" + modelDocuments.Count);
            //locate documents in the model for which no element in the wizard exists (anymore)
            List<int> scheduledForDeletion = modelDocuments.Where((t, i) => wizard.TableLayoutMainContentDocuments.Documents.FirstOrDefault(doc => doc.DocumentIndex == i) == null).Select(modelDocument => modelDocuments.IndexOf(modelDocument)).ToList();
            Log.Debug("scheduled for deletion:" + scheduledForDeletion.Count);
            RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)Globals.RationallyAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
            Log.Debug("container present:" + (relatedDocumentsContainer != null));
            //delete these elements from the view, which will automatically remove them from the model
            scheduledForDeletion.ForEach(docIndex => relatedDocumentsContainer.Children.Cast<RelatedDocumentContainer>().First(rdc => rdc.Index == docIndex).RShape.Delete());
        }
    }
}

