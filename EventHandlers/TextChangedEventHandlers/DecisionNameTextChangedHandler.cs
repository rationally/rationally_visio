using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class DecisionNameTextChangedHandler : ITextChangedEventHandler
    {
        public void Execute(RationallyView view, Shape changedShape)
        {
            if (changedShape.Text != string.Empty)
            {
                Globals.RationallyAddIn.Model.DecisionName = changedShape.Text;
            }
        }
    }
}
