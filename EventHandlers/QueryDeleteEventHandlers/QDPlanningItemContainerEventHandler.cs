using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Planning;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDPlanningContainerEventHandler : IQueryDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            VisioShape comp = view.Children.Find(x => x is PlanningContainer);
            if (comp is PlanningContainer)
            {
                comp.MsvSdContainerLocked = false;
            }
        }
    }
}
