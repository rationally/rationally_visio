using System;
using System.Collections.Generic;
using System.Linq;
using Rationally.Visio.Forms;
using Rationally.Visio.Forms.WizardComponents;

namespace Rationally.Visio.EventHandlers.WizardPageHandlers
{
    internal class WizardUpdateDocumentsHandler
    {
        public static void Execute(ProjectSetupWizard wizard)
        {
            //List<FlowLayoutDocument> filledInPanels = wizard.TableLayoutMainContentDocuments.Documents.Where(doc => !string.IsNullOrEmpty(doc.FilePath.Text)).ToList();
            List<FlowLayoutDocument> filledInPanels = wizard.TableLayoutMainContentDocuments.Documents.Where(doc => !String.IsNullOrEmpty(doc.FilePath.Text)).ToList();
            filledInPanels.ForEach(filledInPanel => filledInPanel.UpdateModel());
        }
    }
}
