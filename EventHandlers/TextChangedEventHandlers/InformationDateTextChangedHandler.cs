using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class InformationDateTextChangedHandler : ITextChangedEventHandler
    {
        public void Execute(string eventKey, RationallyView view, Shape changedShape)
        {
            Globals.RationallyAddIn.Model.DateString = changedShape.Text;
        }
    }
}
