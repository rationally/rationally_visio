using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class PlanningTextChangedEventHandler : ITextChangedEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyView view, Shape changedShape)
        {
            VisioShape planningItemTextComponent = new VisioShape(view.Page) { Shape = changedShape };

            if (Globals.RationallyAddIn.Model.PlanningItems.Count <= planningItemTextComponent.Index) { return; }

            PlanningItem toUpdate = Globals.RationallyAddIn.Model.PlanningItems[planningItemTextComponent.Index];
            
            toUpdate.ItemText = planningItemTextComponent.Text;
        }
    }
}
