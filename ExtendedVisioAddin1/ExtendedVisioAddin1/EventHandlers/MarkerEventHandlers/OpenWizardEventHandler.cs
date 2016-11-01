using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.WindowsFormPopups;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    class OpenWizardEventHandler : IMarkerEventHandler
    {
        public void Execute(RationallyModel model, Shape changedShape, string identifier)
        {
            //ProjectSetupWizard wizard = new ProjectSetupWizard();
            ProjectSetupWizard.Instance.ShowDialog(false);   
        }
    }
}
