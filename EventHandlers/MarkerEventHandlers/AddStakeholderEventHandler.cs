using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class AddStakeholderEventHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape changedShape, string identifier)
        {
            StakeholdersContainer stakeholdersContainer = (StakeholdersContainer)Globals.RationallyAddIn.View.Children.First(ch => ch is StakeholdersContainer);
            stakeholdersContainer?.AddStakeholder("<<name>>","<<role>>");  
        }
    }
}
