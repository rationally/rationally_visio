using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Planning;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDPlanningItemComponentEventHandler : IQueryDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            PlanningContainer cont = (PlanningContainer)view.Children.First(x => x is PlanningContainer);

            foreach (PlanningItemComponent planningComponent in cont.Children.Where(c => c is PlanningItemComponent).Cast<PlanningItemComponent>().ToList())
            {
                //check if this planning component contains the to be deleted component and is not already deleted
                if (planningComponent.ExistsInTree(changedShape) && !planningComponent.Deleted)
                {
                    planningComponent.Deleted = true;
                    planningComponent.Shape.Delete(); //delete the parent wrapper of s
                }

            }
        }
    }
}
