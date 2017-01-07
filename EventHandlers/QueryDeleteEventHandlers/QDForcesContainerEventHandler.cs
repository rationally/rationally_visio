using System.Reflection;
using log4net;
using Rationally.Visio.View;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDForcesContainerEventHandler : IQueryDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape forcesContainer) => Globals.RationallyAddIn.View.GetComponentByShape(forcesContainer).RemoveDeleteLock(true);
    }
}
