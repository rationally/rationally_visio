using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Enums;
using Rationally.Visio.Forms;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class OpenWizardEventHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape changedShape, string identifier)
        {
            switch (changedShape.Name)
            {
                case "Topic":
                    ProjectSetupWizard.Instance.ShowDialog(false, WizardFieldTypes.Title);
                    break;
                case "InformationAuthor":
                    ProjectSetupWizard.Instance.ShowDialog(false, WizardFieldTypes.Author);
                    break;
                case "InformationDate":
                    ProjectSetupWizard.Instance.ShowDialog(false, WizardFieldTypes.Date);
                    break;
                case "InformationVersion":
                    ProjectSetupWizard.Instance.ShowDialog(false, WizardFieldTypes.Version);
                    break;
            }
        }
    }
}
