using Rationally.Visio.View;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDForcesContainerEventHandler : IQueryDeleteEventHandler
    {
        public void Execute(string eventKey, RView view, Shape forcesContainer)
        {
            Globals.ThisAddIn.View.GetComponentByShape(forcesContainer).RemoveDeleteLock(true);
        }
    }
}
