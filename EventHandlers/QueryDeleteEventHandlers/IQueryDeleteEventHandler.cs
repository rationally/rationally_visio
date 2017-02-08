using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal interface IQueryDeleteEventHandler
    {
        void Execute(RationallyView view, Shape changedShape);
    }
}
