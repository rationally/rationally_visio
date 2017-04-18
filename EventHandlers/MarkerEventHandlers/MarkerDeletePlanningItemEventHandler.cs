using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Planning;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MarkerDeletePlanningItemEventHandler : IMarkerEventHandler
    {
        public void Execute(Shape s, string identifier)
        {

            VisioShape component = new VisioShape(Globals.RationallyAddIn.Application.ActivePage) { Shape = s };


            DialogResult confirmResult = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Shape shapeToPass;

                if (PlanningItemComponent.IsPlanningItem(s.Name))
                {
                    shapeToPass = s;
                }
                else //subpart of planning item component
                {
                    //trace planning container
                    PlanningContainer planningContainer = (PlanningContainer)Globals.RationallyAddIn.View.Children.First(c => c is PlanningContainer);
                    //trace the correct planningItemComponent
                    PlanningItemComponent planningItemComponent = (PlanningItemComponent)planningContainer.Children.First(c => c is PlanningItemComponent && (component.Index == c.Index));

                    shapeToPass = planningItemComponent.Shape;
                }
                //initiate a delete handler with the container's shape
                shapeToPass.Delete();
            }
        }
    }
}
