using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.WindowsFormPopups;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    class OpenWizardEventHandler : IMarkerEventHandler
    {
        public void Execute(RModel model, Shape changedShape, string identifier)
        {
            ProjectSetupWizard wizard = new ProjectSetupWizard();
            wizard.Show();   
        }
    }
}
