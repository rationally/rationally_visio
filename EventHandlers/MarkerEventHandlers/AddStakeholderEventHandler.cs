using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class AddStakeholderEventHandler : IMarkerEventHandler
    {
        public void Execute(Shape changedShape, string identifier)
        {
            StakeholdersContainer stakeholdersContainer = (StakeholdersContainer)Globals.RationallyAddIn.View.Children.First(ch => ch is StakeholdersContainer);
            stakeholdersContainer?.AddStakeholder("Jake Stake");  
        }
    }
}
