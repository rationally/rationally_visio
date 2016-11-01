using Rationally.Visio.View;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDForcesContainerEventHandler : IQueryDeleteEventHandler
    {
        public void Execute(string eventKey, RationallyView view, Shape forcesContainer)
        {
            Globals.RationallyAddIn.View.GetComponentByShape(forcesContainer).RemoveDeleteLock(true);
        }
    }
}
