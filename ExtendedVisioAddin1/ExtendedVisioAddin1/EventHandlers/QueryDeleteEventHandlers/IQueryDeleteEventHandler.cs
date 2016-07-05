using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    internal interface IQueryDeleteEventHandler
    {
        void Execute(string eventKey, RView view, Shape changedShape);
    }
}
