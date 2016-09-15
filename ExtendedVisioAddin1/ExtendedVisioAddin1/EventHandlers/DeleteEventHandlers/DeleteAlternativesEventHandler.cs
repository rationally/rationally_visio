using System.Linq;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteAlternativesEventHandler : IDeleteEventHandler
    {
        public void Execute(string eventKey, RModel model, Shape changedShape)
        {
            Globals.ThisAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(changedShape));
            if (!Globals.ThisAddIn.View.Children.Any(x => x is AlternativesContainer))
            {
                model.Alternatives.Clear();
                new RepaintHandler();
            }
        }
    }
}
