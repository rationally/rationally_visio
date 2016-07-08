using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDForcesContainerEventHandler : IQueryDeleteEventHandler
    {
        public void Execute(string eventKey, RView view, Shape forcesContainer)
        {
            Globals.ThisAddIn.View.GetComponentByShape(forcesContainer).RemoveDeleteLock(true);
        }
    }
}
