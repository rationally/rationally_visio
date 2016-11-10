using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Enums;
using Rationally.Visio.Model;
using Rationally.Visio.Forms;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class OpenWizardEventHandler : IMarkerEventHandler
    {
        public void Execute(Shape changedShape, string identifier)
        {
            ProjectSetupWizard.Instance.ShowDialog(false, WizardFieldTypes.Title);   
        }
    }
}
