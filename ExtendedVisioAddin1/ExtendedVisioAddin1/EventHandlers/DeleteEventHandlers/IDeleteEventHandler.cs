using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.DeleteEventHandlers
{
    internal interface IDeleteEventHandler
    {
        void Execute(string eventKey, RModel model, Shape changedShape);
    }
}
