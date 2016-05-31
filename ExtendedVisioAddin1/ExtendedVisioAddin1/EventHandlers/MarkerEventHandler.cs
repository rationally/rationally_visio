using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal abstract class MarkerEventHandler : EventHandler
    {
        public abstract void Execute(RModel model, Shape changedShape, string identifier);

    }
}
