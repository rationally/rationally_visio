using Microsoft.Office.Interop.Visio;
using Rationally.Visio.WindowsFormPopups;

namespace Rationally.Visio.EventHandlers
{
    internal class DocumentCreatedEventHandler
    {
        public DocumentCreatedEventHandler(IVDocument document)
        {
            if (document.Template.Contains(Constants.TemplateName))
            {
                ShowSetupWizard();
            }
        }

        private static void ShowSetupWizard()
        {
            ProjectSetupWizard.Instance.ShowDialog(true);
        }
    }
}
