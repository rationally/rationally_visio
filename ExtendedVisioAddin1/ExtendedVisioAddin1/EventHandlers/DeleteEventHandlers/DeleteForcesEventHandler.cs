using System.Linq;
using Rationally.Visio.Model;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteForcesEventHandler : IDeleteEventHandler
    {
        public void Execute(string eventKey, RModel model, Shape forcesContainer)
        {
            
            Globals.ThisAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(forcesContainer));
            if (!Globals.ThisAddIn.View.Children.Any(x => x is ForcesContainer))
            {
                model.Forces.Clear();
                new RepaintHandler();
            }
        }
    }
}
