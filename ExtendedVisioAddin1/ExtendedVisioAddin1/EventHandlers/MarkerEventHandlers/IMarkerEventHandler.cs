using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.MarkerEventHandlers
{
    internal interface IMarkerEventHandler
    {
        void Execute(RModel model, Shape changedShape, string identifier);

    }
}
