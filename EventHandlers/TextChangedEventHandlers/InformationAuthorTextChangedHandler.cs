using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class InformationAuthorTextChangedHandler : ITextChangedEventHandler
    {
        public void Execute(string eventKey, RationallyView view, Shape changedShape)
        {
            Globals.RationallyAddIn.Model.Author = changedShape.Text;
        }
    }
}
