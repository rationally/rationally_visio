using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Planning;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    class MarkerDeletePlanningItemEventHandler : IMarkerEventHandler
    {
        public void Execute(Shape s, string identifier)
        {

            RationallyComponent component = new RationallyComponent(Globals.RationallyAddIn.Application.ActivePage) { RShape = s };


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

                    shapeToPass = planningItemComponent.RShape;
                }
                //initiate a delete handler with the container's shape
                shapeToPass.Delete();
            }
        }
    }
}
