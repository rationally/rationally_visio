using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Planning;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MoveDownPlanningItemHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape changedShape, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            PlanningContainer planningContainer = (PlanningContainer)Globals.RationallyAddIn.View.Children.First(c => c is PlanningContainer);

            VisioShape toChangeComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);
            int currentIndex = toChangeComponent.Index;

            PlanningItemComponent toChange = (PlanningItemComponent)planningContainer.Children.First(c => c.Index == currentIndex);
            //locate the stakeholder that we are going to swap with
            PlanningItemComponent other = (PlanningItemComponent)planningContainer.Children.First(c => c.Index == currentIndex + 1);

            PlanningItem one = model.PlanningItems[currentIndex];
            model.PlanningItems[currentIndex] = model.PlanningItems[currentIndex + 1];
            model.PlanningItems[currentIndex + 1] = one;

            //update the index of the component and his children
            toChange.SetPlanningItemIndex(currentIndex + 1);

            //same, for the other component
            other.SetPlanningItemIndex(currentIndex);

            //swap the elements in the view tree
            VisioShape temp = planningContainer.Children[currentIndex];
            planningContainer.Children[currentIndex] = planningContainer.Children[currentIndex + 1];
            planningContainer.Children[currentIndex + 1] = temp;


            RepaintHandler.Repaint();
        }
    }
}
