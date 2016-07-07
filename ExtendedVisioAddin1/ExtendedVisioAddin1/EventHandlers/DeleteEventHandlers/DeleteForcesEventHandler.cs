
using System.Linq;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.DeleteEventHandlers
{
    internal class DeleteForcesEventHandler : IDeleteEventHandler
    {
        public void Execute(string eventKey, RModel model, Shape forcesContainer)
        {
            Globals.ThisAddIn.View.GetComponentByShape(forcesContainer).RemoveDeleteLock(true);
            Globals.ThisAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(forcesContainer));
            if (!Globals.ThisAddIn.View.Children.Any(x => x is ForcesContainer))
            {
                model.Forces.Clear();
                new RepaintHandler();
            }
        }
    }
}
