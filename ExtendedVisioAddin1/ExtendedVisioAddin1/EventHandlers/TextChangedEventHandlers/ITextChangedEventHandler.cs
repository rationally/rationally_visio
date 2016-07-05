using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.TextChangedEventHandlers
{
    internal interface ITextChangedEventHandler
    {
        void Execute(string eventKey, RView view, Shape changedShape);
    }
}
