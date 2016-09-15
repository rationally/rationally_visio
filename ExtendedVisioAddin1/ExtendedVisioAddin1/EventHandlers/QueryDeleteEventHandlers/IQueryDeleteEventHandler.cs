using Rationally.Visio.View;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal interface IQueryDeleteEventHandler
    {
        void Execute(string eventKey, RView view, Shape changedShape);
    }
}
