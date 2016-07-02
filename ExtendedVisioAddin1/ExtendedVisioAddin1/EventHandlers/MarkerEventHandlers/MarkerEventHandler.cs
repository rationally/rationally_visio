using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.MarkerEventHandlers
{
    internal abstract class MarkerEventHandler
    {
        public abstract void Execute(RModel model, Shape changedShape, string identifier);

    }
}
