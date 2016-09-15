using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal interface IMarkerEventHandler
    {
        void Execute(RModel model, Shape changedShape, string identifier);

    }
}
