using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Enums;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.Model;
using Rationally.Visio.Forms;

namespace Rationally.Visio.EventHandlers
{
    internal static class DocumentCreatedEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static void ShowSetupWizard() => ProjectSetupWizard.Instance.ShowDialog(true, WizardFieldTypes.Title);

        public static void Execute(IVDocument document)
        {
            if (document.Template.Contains(Constants.TemplateName))
            {
                Globals.RationallyAddIn.Model = new RationallyModel();
                Globals.RationallyAddIn.View.Page = Globals.RationallyAddIn.Application.ActivePage;
                ShowSetupWizard();
                Log.Debug("Showed setup wizard.");
            }
        }
    }
}
