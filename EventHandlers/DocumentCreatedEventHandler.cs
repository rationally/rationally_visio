using Microsoft.Office.Interop.Visio;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.Model;
using Rationally.Visio.Forms;

namespace Rationally.Visio.EventHandlers
{
    internal class DocumentCreatedEventHandler
    {
        public DocumentCreatedEventHandler(IVDocument document)
        {
            if (document.Template.Contains(Constants.TemplateName))
            {
                Globals.RationallyAddIn.Model = new RationallyModel();
                ShowSetupWizard();
            }
        }

        private static void ShowSetupWizard()
        {
            ProjectSetupWizard.Instance.ShowDialog(true);
        }
    }
}
