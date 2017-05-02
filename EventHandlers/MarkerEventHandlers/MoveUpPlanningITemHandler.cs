using System.Linq;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View;
using Rationally.Visio.View.Planning;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MoveUpPlanningItemHandler : IMarkerEventHandler
    {
        public void Execute(Shape changedShape, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            //locate the stakeholder(component) to move
            VisioShape toChangeComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);
            int currentIndex = toChangeComponent.Index;
            //locate the stakeholder to swap with
            PlanningContainer planningContainer = (PlanningContainer)Globals.RationallyAddIn.View.Children.First(c => c is PlanningContainer);
            PlanningItemComponent toChange = (PlanningItemComponent)planningContainer.Children.First(c => (int)c.Shape.CellsU[VisioFormulas.Cell_Index].ResultIU == currentIndex);
            PlanningItemComponent other = (PlanningItemComponent)planningContainer.Children.First(c => (int)c.Shape.CellsU[VisioFormulas.Cell_Index].ResultIU == currentIndex - 1);

            //swap
            PlanningItem one = model.PlanningItems[currentIndex];
            model.PlanningItems[currentIndex] = model.PlanningItems[currentIndex - 1];
            model.PlanningItems[currentIndex - 1] = one;

            //update the index of the component and his children
            toChange.SetPlanningItemIndex(currentIndex - 1);
            //same, for the other component
            other.SetPlanningItemIndex(currentIndex);
            //swap the elements
            VisioShape temp = planningContainer.Children[currentIndex];
            planningContainer.Children[currentIndex] = planningContainer.Children[currentIndex - 1];
            planningContainer.Children[currentIndex - 1] = temp;


            RepaintHandler.Repaint();
        }
    }
}
