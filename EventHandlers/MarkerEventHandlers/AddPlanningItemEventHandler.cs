using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View.Planning;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class AddPlanningItemEventHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape s, string context)
        {
            PlanningContainer planningContainer = (PlanningContainer)Globals.RationallyAddIn.View.Children.First(ch => ch is PlanningContainer);
            planningContainer.AddPlanningItem();

        }
    }
}
