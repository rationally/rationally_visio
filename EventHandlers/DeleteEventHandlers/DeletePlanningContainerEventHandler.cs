using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View.Planning;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeletePlanningContainerEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Globals.RationallyAddIn.View.Children.RemoveAll(obj => obj.Shape.Equals(changedShape));
            Log.Debug("Handler of delete planning container entered.");
            if (!Globals.RationallyAddIn.View.Children.Any(x => x is PlanningContainer))
            {
                model.PlanningItems.Clear();
                Log.Debug("model planning items list emptied.");
                RepaintHandler.Repaint();
            }
        }
    }
}
