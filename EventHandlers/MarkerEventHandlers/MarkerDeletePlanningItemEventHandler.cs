using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Documents;
using Rationally.Visio.View.Planning;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MarkerDeletePlanningItemEventHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape s, string identifier)
        {
            RationallyComponent component = new RationallyComponent(Globals.RationallyAddIn.Application.ActivePage) { RShape = s };
            
            DialogResult confirmResult = MessageBox.Show("Are you sure you want to delete this planning item?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Shape shapeToPass;

                if (RelatedDocumentContainer.IsRelatedDocumentContainer(s.Name))
                {
                    shapeToPass = s;
                }
                else //subpart of document container
                {
                    //trace documents container
                    PlanningContainer planningContainer = (PlanningContainer)Globals.RationallyAddIn.View.Children.First(c => c is PlanningContainer);
                    //trace the correct document container
                    PlanningItemComponent documentContainer = (PlanningItemComponent)planningContainer.Children.First(c => c is PlanningItemComponent && (component.Index == c.Index));

                    shapeToPass = documentContainer.RShape;
                }
                //initiate a delete handler with the container's shape
                shapeToPass.Delete();
            }
        }
    }
}
