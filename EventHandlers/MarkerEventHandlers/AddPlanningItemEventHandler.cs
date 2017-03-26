using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Planning;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    class AddPlanningItemEventHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape s, string context)
        {
            PlanningContainer planningContainer = (PlanningContainer)Globals.RationallyAddIn.View.Children.First(ch => ch is PlanningContainer);
            planningContainer.AddPlanningItem();

        }
    }
}
