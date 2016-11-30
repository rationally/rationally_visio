using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal interface IMarkerEventHandler
    {
        void Execute(Shape changedShape, string identifier);

    }
}
